# -*- coding: utf-8 -*-
# FILE: 07_xu_ly_ai\Tac_Vu_Ai.py
# Chức năng: Xử lý tác vụ AI nâng cao (Giải toán, Chữa văn, Chat bổ sung) trực tiếp từ Word

import sys
import os
import json
import sqlite3
import logging
import traceback
import time
import io
from typing import Dict, Any, List, Optional
from google import genai
from google.genai.errors import APIError 
from google.genai import types 

# =================================================
# 1. CẤU HÌNH HỆ THỐNG & ENCODING (FIX LỖI FONT)
# =================================================
try:
    sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')
    sys.stderr = io.TextIOWrapper(sys.stderr.buffer, encoding='utf-8')
except:
    pass

# Log ra stderr để C# đọc và hiển thị trong richTextBox_LogTienTrinh
logging.basicConfig(stream=sys.stderr, level=logging.INFO, format='[PYTHON_LOG] %(levelname)s: %(message)s')

DB_PATH = ""
TEN_MO_HINH_MAC_DINH = "gemini-2.5-flash"

# =================================================
# 2. LOGIC TRUY XUẤT DATABASE (REUSE)
# =================================================

def lay_api_key_tu_db() -> Optional[str]:
    """Lấy ngẫu nhiên 1 API Key đang hoạt động từ DB."""
    global DB_PATH
    if not DB_PATH: return None
    conn = None
    try:
        conn = sqlite3.connect(DB_PATH)
        conn.execute('PRAGMA journal_mode=WAL')
        cursor = conn.cursor()
        cursor.execute("SELECT GiaTri FROM bang_api_key WHERE DangBiLoi = 0 ORDER BY RANDOM() LIMIT 1")
        res = cursor.fetchone()
        return res[0] if res else None
    except:
        return None
    finally:
        if conn: conn.close()

def danh_dau_key_loi(key: str, ly_do: str):
    """Đánh dấu key hỏng để hệ thống tự động xoay vòng."""
    global DB_PATH
    conn = None
    try:
        conn = sqlite3.connect(DB_PATH)
        conn.execute("UPDATE bang_api_key SET DangBiLoi = 1, LyDoLoi = ?, ThoiGianLoi = datetime('now') WHERE GiaTri = ?", (ly_do, key))
        conn.commit()
    except:
        pass
    finally:
        if conn: conn.close()

# =================================================
# 3. LOGIC PROMPT NGHIỆP VỤ (REUSE & EXPAND)
# =================================================

def tao_prompt_tac_vu(ma_tac_vu: str) -> str:
    """
    Chuyển hóa mục tiêu từ PDF thành Prompt chuyên sâu.
    Các mục 'logic sau' để trống theo yêu cầu.
    """
    
    # LUẬT LATEX CHUNG (Duy trì logic cũ)
    LATEX_RULE = "QUY TẮC: Mọi công thức toán học PHẢI dùng LaTeX bọc trong $. Ví dụ: $x^2$."

    prompts = {
        # --- NHÓM TOÁN HỌC ---
        "Hướng dẫn giải": f"{LATEX_RULE}\nHãy phân tích và đưa ra các bước gợi ý tư duy để giải bài toán này.",
        "Lời giải chi tiết": f"{LATEX_RULE}\nHãy giải chi tiết bài toán này, trình bày sư phạm, rõ ràng từng bước.",
        "Câu 4 lựa chọn": f"{LATEX_RULE}\nHãy tạo 01 câu hỏi trắc nghiệm tương tự nội dung này với 4 đáp án A, B, C, D.",
        "Câu dạng Đúng/Sai": f"{LATEX_RULE}\nHãy tạo câu hỏi dạng khẳng định Đúng/Sai gồm 4 ý a, b, c, d tương tự nội dung này.",
        "Câu trả lời ngắn": f"{LATEX_RULE}\nHãy tạo câu hỏi yêu cầu trả lời ngắn (điền số/đáp số) tương tự nội dung này.",
        "Câu tự luận": f"{LATEX_RULE}\nHãy tạo 01 bài tập tự luận tương tự nội dung đã chọn.",
        "Thiết kế dạy học": (
            "Bạn là chuyên gia giáo dục toán học THPT. Hãy thiết kế các hoạt động dạy học "
            "ngắn gọn, tập trung vào các bước hình thành kiến thức cho nội dung toán học sau."
        ),
        "Thiết kế HĐ CV5512": "", # Logic sau
        "Viết lại GA 5512": "",    # Logic sau

        # --- NHÓM VĂN BẢN THƯỜNG ---
        "Tóm tắt nội dung": (
            "Bạn là một thư ký chuyên nghiệp. Hãy tóm tắt nội dung văn bản sau theo các ý chính dính danh, "
            "đảm bảo ngắn gọn, súc tích và tuyệt đối không trùng lặp ý."
        ),
        "Sửa lại câu từ": (
            "Bạn là chuyên gia ngôn ngữ và soạn thảo văn bản Việt Nam. Hãy chỉnh sửa văn bản sau "
            "sao cho mượt mà, trơn chu, sửa lỗi chính tả, dấu câu, tránh lặp từ. "
            "LƯU Ý: Không làm thay đổi nội dung và ý nghĩa gốc, đảm bảo tính logic tuần tự."
        ),
        "Văn học": (
            "Bạn là một nhà văn chuyên nghiệp. Hãy viết lại văn bản sau với phong cách văn chương, "
            "ngôn từ trau chuốt, truyền cảm và giàu hình ảnh."
        ),
        "Báo trí": (
            "Bạn là nhà báo lão thành. Hãy viết lại văn bản sau theo văn phong báo chí hiện đại, "
            "mang tính thông tin, tuyên truyền và lôi cuốn độc giả."
        ),
        "Hành chính": (
            "Bạn là chuyên gia soạn thảo văn bản hành chính nhà nước. Hãy viết lại văn bản sau "
            "theo đúng phong cách công vụ: ngôn ngữ chuẩn mực, chính xác, khách quan và trang trọng."
        ),
        "Dịch thuật EV": (
            "Bạn là chuyên gia dịch thuật song ngữ Anh-Việt. Nếu văn bản là tiếng Việt, hãy dịch sang tiếng Anh. "
            "Nếu văn bản là tiếng Anh, hãy dịch sang tiếng Việt. Đảm bảo sát nghĩa và tự nhiên."
        ),
        "Tham luận": (
            "Bạn là một học giả. Hãy viết một bài tham luận chuyên sâu, có luận điểm rõ ràng "
            "liên quan đến vấn đề được nêu trong văn bản sau."
        ),
        "KĐ Cá nhân": "", # Logic sau
        "KĐ Đảng viên": "", # Logic sau

        # --- NHÓM VIẾT PROMPT ---
        "CSharp": (
            "Bạn là chuyên gia lập trình C# cao cấp với hơn 20 năm kinh nghiệm. Hãy viết một bản "
            "Prompt hướng dẫn hệ thống (System Instruction) tư duy sâu để AI thực hiện tốt ý tưởng sau. "
            "LƯU Ý: Chỉ viết Prompt hướng dẫn, không được viết mã nguồn (code)."
        ),
        "Python": (
            "Bạn là chuyên gia lập trình Python và PyQt6. Hãy viết một bản Prompt hướng dẫn hệ thống "
            "chuyên sâu để AI thực hiện tốt ý tưởng sau. "
            "LƯU Ý: Chỉ viết Prompt hướng dẫn, không được viết mã nguồn (code)."
        ),
        "CSharp&Python": (
            "Bạn là kiến trúc sư phần mềm am hiểu cả C# và Python. Hãy viết bản Prompt hướng dẫn "
            "hệ thống để AI thực hiện ý tưởng sau (phân rõ C# và Python). "
            "LƯU Ý: Chỉ viết Prompt hướng dẫn, không được viết mã nguồn."
        ),

        # --- NHÓM TỰ DO ---
        "Tự do": (
            "Bạn là một nhà thông thái và chuyên gia giáo dục đa lĩnh vực. Hãy tham gia trò chuyện "
            "và giải đáp mọi thắc mắc của người dùng một cách thông minh, chân thành."
        )
    }
    return prompts.get(ma_tac_vu, "Hãy xử lý nội dung sau một cách chuẩn xác.")

# =================================================
# 4. LOGIC GỌI AI VÀ XOAY VÒNG KEY (CORE LOGIC)
# =================================================

def ai_reasoning_with_rotation(chunk: Dict[str, Any], prompts: Dict[str, str], ten_mo_hinh: str, nhiet_do: float = 0.6) -> str:
    """
    Logic cốt lõi: Thực hiện gọi AI với cơ chế xoay vòng Key từ Database.
    
    Tham số:
    - chunk: Chứa dữ liệu đầu vào (binary data và mime type).
    - prompts: Chứa system_instruction và user_prompt.
    - ten_mo_hinh: Tên model Gemini (vd: gemini-1.5-flash).
    - nhiet_do: Giá trị từ 0.0 đến 2.0 (mặc định 0.6), quyết định độ sáng tạo của AI.
    """
    last_error = ""
    
    # Thử tối đa 5 lần với các Key khác nhau nếu gặp lỗi API hoặc Quota
    for attempt in range(5):
        # 1. Lấy API Key ngẫu nhiên từ danh sách đang hoạt động trong DB
        api_key = lay_api_key_tu_db()
        if not api_key:
            raise Exception("He thong can kiet API Key. Vui long bo sung Key moi vao Database.")

        try:
            # 2. Khởi tạo Client với Key vừa lấy
            client = genai.Client(api_key=api_key)
            
            # 3. Xây dựng danh sách các thành phần nội dung (Multimodal Parts)
            content_parts = []
            
            # Kiểm tra và đóng gói dữ liệu binary nếu có (Ảnh chụp vùng chọn hoặc Text thô)
            if chunk.get("data"):
                if chunk.get("mime") == "image/png":
                    # Trường hợp Toán học: Gửi ảnh binary
                    content_parts.append(types.Part.from_bytes(data=chunk["data"], mime_type="image/png"))
                elif chunk.get("mime") == "text/plain":
                    # Trường hợp Văn bản thường: Gửi text đã decode
                    content_parts.append(types.Part(text=chunk["data"].decode("utf-8")))

            # Luôn đính kèm chỉ thị của người dùng (User Prompt) vào cuối danh sách Parts
            content_parts.append(types.Part(text=prompts["user"]))

            # 4. Thực hiện gọi API Gemini
            response = client.models.generate_content(
                model=ten_mo_hinh,
                contents=[types.Content(role="user", parts=content_parts)],
                config=types.GenerateContentConfig(
                    system_instruction=prompts["system"], # Chỉ thị hệ thống (Vai trò chuyên gia)
                    temperature=nhiet_do,                # NHIỆT ĐỘ ĐỘNG từ thanh trượt C#
                    top_p=0.95,
                    max_output_tokens=4096               # Đảm bảo đủ độ dài cho lời giải chi tiết
                )
            )
            
            # 5. Kiểm tra và trả về kết quả
            if response and response.text:
                return response.text
            else:
                raise Exception("AI phan hoi thanh cong nhung noi dung text bi trong.")

        except Exception as e:
            last_error = str(e)
            
            # Nếu gặp lỗi API nghiêm trọng (Hết quota, Key hỏng, Model không tồn tại)
            # thực hiện đánh dấu Key lỗi vào DB để hệ thống bỏ qua ở các lượt sau.
            if any(err in last_error for err in ["400", "403", "429", "404", "API_KEY"]):
                danh_dau_key_loi(api_key, last_error)
            
            # Tính toán thời gian nghỉ tăng dần (Exponential backoff) để bảo vệ hệ thống
            wait_time = (attempt + 1) * 2 
            logging.warning(f"Loi API voi Key {api_key[:6]}... Dang nghi {wait_time}s truoc khi doi Key. Chi tiet: {last_error}")
            time.sleep(wait_time)
            continue

    # Nếu sau 5 lần thử vẫn thất bại, ném ngoại lệ cuối cùng báo về cho C# hiển thị
    raise Exception(f"That bai sau 5 lan thu voi cac Key khac nhau. Loi cuoi cung: {last_error}")

# =================================================
# 5. HÀM MAIN - ĐIỀU PHỐI ĐẦU VÀO TỪ C#
# =================================================

def main():
    """
    Ham chinh thuc thi pipeline AI cho Task Panel.
    Duy tri logic cu (Tac vu tinh) va Re nhanh moi (Chat tang cuong voi lich su + Ngữ cảnh ComboBox).
    Tích hợp điều khiển Nhiệt độ (Temperature) động từ C#.
    """
    global DB_PATH # Cap nhat bien toan cuc de cac ham lay_key su dung
    output_path = None
    try:
        # 1. NHẬN VÀ GIẢI MÃ JSON ĐẦU VÀO TỪ C#
        if len(sys.argv) < 2:
            return
        with open(sys.argv[1], "r", encoding="utf-8") as f:
            input_json = json.load(f)

        # Lay cac tham so ha tang (Dong bo duong dan DB tuyet doi)
        DB_PATH = input_json.get("db_path")
        output_path = input_json.get("output_path")
        
        # Lay tham so Nhiet do (Temperature) - Quy doi tu TrackBar (0.0 - 2.0)
        nhiet_do_ai = input_json.get("nhiet_do", 0.6)
        
        # Lay cac tham so nghiep vu
        ma_tac_vu = input_json.get("ma_tac_vu")
        ten_tac_vu_dang_chon = input_json.get("ten_tac_vu_dang_chon", "Trợ lý AI") 
        img_path = input_json.get("duong_dan_anh")   # Anh vung chon Word (MathType/Equation)
        text_data = input_json.get("text_vung_chon") # Text vung chon Word (Van ban thuan)
        
        # Tham so cho logic cu (Static Task Enhancement qua phim Enter)
        yeu_cau_them = input_json.get("yeu_cau_them", "")
        
        # Tham so cho logic moi (Chat Contextual qua nut Gui)
        yeu_cau_moi = input_json.get("yeu_cau_moi", "")
        lich_su = input_json.get("lich_su_truoc_do", "")

        # Chuan hoa ten mo hinh (Chống lỗi Bad Request 400)
        ten_mo_hinh = input_json.get("ten_mo_hinh", TEN_MO_HINH_MAC_DINH)
        if "1.5" in ten_mo_hinh: ten_mo_hinh = "gemini-2.5-flash"

        if not all([DB_PATH, output_path, ma_tac_vu]):
            raise ValueError("JSON thieu tham so thuc thi bat buoc (db_path, output, ma_tac_vu).")

        # 2. XÂY DỰNG CHỈ THỊ (PROMPT) DỰA TRÊN NHÁNH LOGIC
        
        # Mac dinh He thong chung (Base System Instruction)
        prompt_he_thong = (
            "Ban la chuyen gia Toan hoc va Bien tap van ban cap cao.\n"
            "QUY TAC: 1. Tra ve Plain Text. 2. Cong thuc toan boc trong $. "
            "3. Khong giai thich thua. 4. Khong dung khoi ```."
        )

        # --- RE NHÁNH LOGIC PROMPT ---
        if ma_tac_vu == "CHAT_TANG_CUONG":
            # NHÁNH MỚI: Xu ly nut "Gui" (Chat voi lich su)
            logging.info(f"Hanh vi: Chat tang cuong. Tac vu ngu canh: {ten_tac_vu_dang_chon}")
            prompt_he_thong = (
                f"Ban la tro ly chuyen gia ho tro thuc hien tac vu '{ten_tac_vu_dang_chon}'.\n"
                "Nhiem vu: Dua tren noi dung vung chon (neu co) va lich su trao doi, "
                "hay thuc hien yeu cau moi nhat cua nguoi dung mot cach chinh xac."
            )
            prompt_nguoi_dung = (
                f"TÁC VỤ: {ten_tac_vu_dang_chon}\n"
                f"YÊU CẦU MỚI: {yeu_cau_moi}\n\n"
                f"DỮ LIỆU LỊCH SỬ VÀ NỘI DUNG ĐANG XỬ LÝ:\n{lich_su}"
            )
        else:
            # NHÁNH CŨ: Xu ly cac nut bam chuc nang tinh (Giai bai, Chuyen doi...)
            logging.info(f"Hanh vi: Thuc thi tac vu tinh - {ma_tac_vu}")
            base_instruction = tao_prompt_tac_vu(ma_tac_vu)
            
            if yeu_cau_them:
                # Logic cho viec nhap them prompt vao nut bam tinh qua phim Enter
                prompt_nguoi_dung = (
                    f"{tao_prompt_tac_vu('CHINH_SUA_TANG_CUONG')}\n"
                    f"'{yeu_cau_them}'\n\nNoi dung vung chon Word goc: "
                )
            else:
                prompt_nguoi_dung = base_instruction

        prompts_dict = {
            "system": prompt_he_thong,
            "user": prompt_nguoi_dung
        }

        # 3. CHUẨN BỊ DỮ LIỆU ĐẦU VÀO (MULTIMODAL CHUNK)
        # Logic: Uu tien Anh (de doc MathType) -> Text (neu khong co anh) -> Null (neu vung chon trong)
        data_chunk = {"data": None, "mime": "text/plain", "context": ""}

        if img_path and os.path.exists(img_path):
            logging.info(f"Dinh kem binary hinh anh vung chon: {os.path.basename(img_path)}")
            with open(img_path, "rb") as f_img:
                data_chunk["data"] = f_img.read()
                data_chunk["mime"] = "image/png"
        elif text_data:
            logging.info("Dinh kem van ban vung chon.")
            data_chunk["data"] = text_data.encode("utf-8")
            data_chunk["mime"] = "text/plain"
        else:
            logging.info("Vung chon Word trong. AI se phan hoi dua tren lich su.")

        # 4. GỌI AI VỚI CƠ CHẾ XOAY VÒNG KEY VÀ NHIỆT ĐỘ ĐỘNG
        # response_text duoc lay tu ham ai_reasoning_with_rotation (da bao gom retry/rotation)
        response_text = ai_reasoning_with_rotation(
            data_chunk, 
            prompts_dict, 
            ten_mo_hinh, 
            nhiet_do=nhiet_do_ai
        )

        # 5. GHI KẾT QUẢ VÀO FILE DÍCH DANH ĐỂ C# ĐỌC
        with open(output_path, "w", encoding="utf-8") as f_out:
            f_out.write(response_text)
        
        sys.exit(0) # Thoat thanh cong

    except Exception:
        # LAY TRACEBACK CHI TIET DE C# HIEN THI LOG LOI CHUAN UNICODE
        error_trace = traceback.format_exc()
        logging.error(error_trace)
        # Dam bao luon ghi file (du rong) de C# thoat trang thai cho (Await)
        if output_path:
            try:
                with open(output_path, "w", encoding="utf-8") as f_err:
                    f_err.write("")
            except: pass
        sys.exit(1)

if __name__ == "__main__":
    main()