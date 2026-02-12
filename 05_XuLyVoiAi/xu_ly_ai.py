# -*- coding: utf-8 -*-
# FILE: 07_xu_ly_ai\xu_ly_ai.py (AI Orchestrator - Professional Final Version)
# Chức năng: Phân tích PDF (Sandwich Context), Xử lý ảnh Clipboard, Xoay vòng API Key, Gọi Gemini AI.

import sys
import os
import json
import re
import time
import sqlite3
import traceback
import logging
import io
from io import BytesIO
from typing import Dict, Any, List, Optional

import fitz  # PyMuPDF
from google import genai
from google.genai.errors import APIError 
from google.genai import types 

# =================================================
# 1. CẤU HÌNH HỆ THỐNG & ENCODING
# =================================================
# ÉP LUỒNG HỆ THỐNG DÙNG UTF-8 ĐỂ FIX LỖI FONT TRÊN C# RICH-TEXTBOX
try:
    sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')
    sys.stderr = io.TextIOWrapper(sys.stderr.buffer, encoding='utf-8')
except:
    pass

# Chuyển hướng log sang stderr để C# đọc và hiển thị trong richTextBox_TienTrinh
logging.basicConfig(stream=sys.stderr, level=logging.INFO, format='[PYTHON_LOG] %(levelname)s: %(message)s')

# Biến toàn cục quan trọng
DB_PATH = ""
TEN_MO_HINH_MAC_DINH = "gemini-2.5-flash" # Bản ổn định nhất cho xử lý tài liệu

# =================================================
# 2. LOGIC DATABASE: API KEY & PROMPT
# =================================================

def lay_api_key_tu_db() -> Optional[str]:
    """Lấy ngẫu nhiên 1 API Key đang hoạt động (DangBiLoi = 0) từ Database."""
    global DB_PATH
    if not DB_PATH:
        return None
    conn = None
    try:
        conn = sqlite3.connect(DB_PATH)
        conn.execute('PRAGMA journal_mode=WAL') # Chống khóa Database
        cursor = conn.cursor()
        cursor.execute("SELECT GiaTri FROM bang_api_key WHERE DangBiLoi = 0 ORDER BY RANDOM() LIMIT 1")
        res = cursor.fetchone()
        return res[0] if res else None
    except Exception as e:
        logging.error(f"Loi truy van DB Key: {e}")
        return None
    finally:
        if conn: conn.close()

def danh_dau_key_loi(key: str, ly_do: str):
    """Đánh dấu key bị lỗi để hệ thống tự động xoay vòng sang key khác."""
    global DB_PATH
    conn = None
    try:
        conn = sqlite3.connect(DB_PATH)
        conn.execute("UPDATE bang_api_key SET DangBiLoi = 1, LyDoLoi = ?, ThoiGianLoi = datetime('now') WHERE GiaTri = ?", (ly_do, key))
        conn.commit()
    except Exception as e:
        logging.error(f"DB Error (Mark Key): {e}")
    finally:
        if conn: conn.close()

def tao_prompt_chuyen_sau(ma_che_do: str) -> Dict[str, str]:
    """Tự động tạo prompt mặc định chuyên sâu hoặc lấy từ Database."""
    global DB_PATH
    conn = None
    try:
        conn = sqlite3.connect(DB_PATH)
        cursor = conn.cursor()
        cursor.execute("SELECT prompt_he_thong, prompt_nguoi_dung FROM bang_prompt_xu_ly WHERE ma_che_do = ?", (ma_che_do,))
        res = cursor.fetchone()
        if res: return {"system": res[0], "user": res[1]}
    except: pass
    finally:
        if conn: conn.close()

    # Fallback: Bộ khung mặc định chuyên toán học
    sys_prompt = (
        "Bạn là chuyên gia số hóa tài liệu Toán học cấp cao. Nhiệm vụ của bạn là chuyển đổi nội dung từ hình ảnh/PDF "
        "sang văn bản thuần túy (Plain Text) UTF-8, tuyệt đối tuân thủ các quy tắc kỹ thuật sau:\n\n"

        "1. QUY TẮC LATEX (SỐNG CÒN):\n"
        "- Mọi thực thể toán học PHẢI viết bằng LaTeX.\n"
        "- Công thức nằm trong dòng văn bản: Bọc bằng cặp dấu $ (ví dụ: $f(x)$).\n"
        "- Công thức độc lập, quan trọng hoặc phức tạp: Bọc bằng cặp dấu $$ để căn giữa (ví dụ: $$\\int_{a}^{b} f(x)dx$$).\n"
        "- Tuyệt đối KHÔNG sử dụng ký tự Unicode toán học (như √, π, ∑, ∞). Phải dùng lệnh LaTeX tương ứng (\\sqrt, \\pi, \\sum, \\infty).\n\n"

        "2. QUY TẮC HÌNH ẢNH (CHÍNH XÁC VỊ TRÍ):\n"
        "- Bạn sẽ nhận được các nhãn [[[HINH ANH X]]] kèm mô tả ngữ cảnh. Hãy chèn đúng nhãn đó vào vị trí logic trong văn bản.\n"
        "- SAU KHI CHÈN NHÃN: Tuyệt đối không viết lại số thứ tự, không chú thích hình, không ghi số trang mồ côi.\n"
        "- Ví dụ sai: '[[[HINH ANH 1]]] 1'. Ví dụ đúng: '[[[HINH ANH 1]]]'.\n\n"

        "3. ĐỊNH DẠNG VĂN BẢN:\n"
        "- Giữ nguyên phân cấp: Câu 1, Câu 2, Tiêu đề, Định lý, Ví dụ.\n"
        "- Đối với bảng biểu: Chuyển thành dạng text tab hoặc bảng đơn giản, không được bỏ sót dữ liệu.\n"
        "- Xuống dòng hợp lý giữa các đoạn văn để đảm bảo tính dễ đọc khi đưa vào Microsoft Word.\n\n"

        "4. ĐIỀU KHOẢN NGHIÊM CẤM:\n"
        "- KHÔNG giải thích thêm, không chào hỏi, không nhận xét nội dung.\n"
        "- KHÔNG sử dụng định dạng Markdown (như dấu # cho tiêu đề, ** cho chữ đậm, hoặc các khối mã ```).\n"
        "- Chỉ trả về văn bản sạch để chèn trực tiếp vào Word."
    )
    user_prompts = {
        # Chế độ trích xuất text thuần từ PDF gốc
        "CHI_TEXT": (
            "Hãy đọc kỹ tài liệu PDF được cung cấp. Thực hiện trích xuất toàn bộ nội dung văn bản và công thức toán học. "
            "YÊU CẦU: Giữ nguyên cấu trúc phân cấp, không bỏ sót các ký tự nhỏ trong công thức. "
            "Bỏ qua mọi hình ảnh, sơ đồ và các nhãn đánh dấu hình ảnh (nếu có)."
        ),

        # Chế độ giữ vị trí ảnh (Sử dụng Sandwich Context)
        "TEXT_GIU_ANH": (
            "Hãy thực hiện trích xuất nội dung văn bản và công thức toán học từ tài liệu. "
            "ĐẶC BIỆT: Sử dụng danh sách 'THÔNG TIN VỊ TRÍ HÌNH ẢNH' được cung cấp bên dưới để xác định vị trí chính xác của các nhãn [[[HINH ANH X]]]. "
            "Hãy chèn mã [[[HINH ANH X]]] vào đúng khoảng trống giữa các đoạn văn mà hình ảnh đó xuất hiện trong tài liệu gốc."
        ),

        # Chế độ OCR từ ảnh đơn (Dán từ clipboard)
        "TU_ANH": (
            "Đây là hình ảnh (chụp màn hình hoặc ảnh dán) chứa nội dung toán học. "
            "Hãy thực hiện nhận diện chữ viết (OCR) một cách chính xác nhất. "
            "Chuyển đổi các biểu đồ, ký hiệu hình học hoặc công thức phức tạp sang định dạng văn bản LaTeX chuẩn xác."
        ),

        # Chế độ PDF Scan (Từng trang là một ảnh)
        "PDF_ANH": (
            "Tài liệu này là bản quét (scan) từ sách giấy. Hãy thực hiện quét toàn bộ trang, "
            "nhận diện chính xác văn bản tiếng Việt và các ký hiệu toán học. "
            "Chuyển đổi toàn bộ sang văn bản thuần túy và LaTeX, đảm bảo giữ đúng thứ tự đọc từ trên xuống dưới."
        )
    }
    return {"system": sys_prompt, "user": user_prompts.get(ma_che_do, user_prompts["CHI_TEXT"])}

# =================================================
# 3. LOGIC PHÂN TÍCH PDF (SANDWICH CONTEXT)
# =================================================

def phan_tich_va_lay_ngu_canh(pdf_path: str, ma_che_do: str) -> List[Dict[str, Any]]:
    """Logic lấy văn bản neo 120px quanh hình ảnh để AI định vị chính xác (Sandwich Context)."""
    try:
        doc = fitz.open(pdf_path)
        chunks = []
        
        if ma_che_do == "CHI_TEXT":
            full_text = ""
            for page in doc: full_text += page.get_text("text") + "\n"
            chunks.append({"data": full_text.encode("utf-8"), "mime": "text/plain", "context": ""})
            doc.close()
            return chunks

        img_idx = 0
        chunk_size = 10
        for i in range(0, len(doc), chunk_size):
            end = min(i + chunk_size, len(doc))
            chunk_doc = fitz.open()
            chunk_doc.insert_pdf(doc, from_page=i, to_page=end-1)
            
            mapping_text = "\n--- THÔNG TIN VỊ TRÍ HÌNH ẢNH ---\n"
            for pno in range(i, end):
                page = doc[pno]
                bboxes = []
                for img in page.get_images():
                    try: bboxes.append(page.get_image_bbox(img))
                    except: pass
                
                bboxes.sort(key=lambda b: b.y0) # Sắp xếp từ trên xuống
                for bbox in bboxes:
                    if bbox.width < 30 or bbox.height < 30: continue
                    img_idx += 1
                    # Lấy text bao quanh
                    t_rect = fitz.Rect(0, max(0, bbox.y0 - 120), page.rect.width, bbox.y0)
                    b_rect = fitz.Rect(0, bbox.y1, page.rect.width, min(page.rect.height, bbox.y1 + 120))
                    t_txt = page.get_text("text", clip=t_rect).strip().replace('\n', ' ')
                    b_txt = page.get_text("text", clip=b_rect).strip().replace('\n', ' ')
                    mapping_text += f"- [[[HINH ANH {img_idx}]]]: Sau '{t_txt[-50:]}' và trước '{b_txt[:50]}'\n"

            buf = BytesIO()
            chunk_doc.save(buf)
            chunks.append({"data": buf.getvalue(), "mime": "application/pdf", "context": mapping_text})
            chunk_doc.close()
        doc.close()
        return chunks
    except Exception as e:
        logging.error(f"Loi phan tich PDF: {e}")
        return []

# =================================================
# 4. PHASE 2: AI REASONING WITH ROTATION (KEY RETRY)
# =================================================

def ai_reasoning_with_rotation(chunk: Dict[str, Any], prompts: Dict[str, str], ten_mo_hinh: str) -> str:
    """Gọi AI với cơ chế xoay vòng Key và sửa lỗi cấu trúc SDK mới."""
    last_error = ""
    for attempt in range(5):
        api_key = lay_api_key_tu_db()
        if not api_key:
            raise Exception("Hết API Key hoạt động trong Database.")

        try:
            client = genai.Client(api_key=api_key)
            user_instruction = prompts["user"] + "\n\n" + chunk["context"]
            
            # GỌI GEMINI VỚI CẤU TRÚC SDK MỚI NHẤT
            response = client.models.generate_content(
                model=ten_mo_hinh,
                contents=[
                    types.Part.from_bytes(data=chunk["data"], mime_type=chunk["mime"]),
                    types.Part(text=user_instruction)
                ],
                config=types.GenerateContentConfig(
                    system_instruction=prompts["system"],
                    temperature=0.1
                )
            )
            
            if response and response.text:
                return response.text
            else:
                raise Exception("AI trả về nội dung rỗng.")

        except Exception as e:
            last_error = str(e)
            # Nếu lỗi API nghiêm trọng, đánh dấu hỏng key
            if any(err in last_error for err in ["400", "403", "429", "404"]):
                danh_dau_key_loi(api_key, last_error)
            
            wait_time = (attempt + 1) * 2
            logging.warning(f"Key {api_key[:6]}... gap loi. Thu lai sau {wait_time}s...")
            time.sleep(wait_time)
            continue

    raise Exception(f"That bai sau 5 lan thu. Loi cuoi: {last_error}")

# =================================================
# 5. MAIN EXECUTION
# =================================================

def ghi_ket_qua_va_thoat(output_path, text, code):
    if output_path:
        try:
            with open(output_path, "w", encoding="utf-8") as f:
                f.write(text)
        except: pass
    sys.exit(code)

def chuyen_pdf_thanh_danh_sach_anh(pdf_path: str) -> List[Dict[str, Any]]:
    """
    Logic rẽ nhánh cho rad_TuPDF_Anh: 
    Chuyển mỗi trang PDF thành một ảnh PNG để AI thực hiện OCR Toán học.
    """
    chunks = []
    try:
        doc = fitz.open(pdf_path)
        logging.info(f"Bat dau render {len(doc)} trang PDF thanh anh...")
        
        for i in range(len(doc)):
            page = doc[i]
            # DPI 200 là 'điểm ngọt': Đủ rõ để AI đọc công thức, đủ nhẹ để không tốn quota
            pix = page.get_pixmap(dpi=200) 
            img_data = pix.tobytes("png")
            
            chunks.append({
                "data": img_data,
                "mime": "image/png",
                "context": f"\n--- NOI DUNG TRANG {i + 1} (TU ANH SCAN) ---\n"
            })
        doc.close()
        return chunks
    except Exception as e:
        logging.error(f"Loi khi anh hoa PDF: {e}")
        return []

def trich_xuat_anh_vat_ly(duong_dan_pdf: str, thu_muc_dau_ra: str) -> List[str]:
    """
    Trích xuất toàn bộ ảnh từ PDF ra thư mục tạm /images/ dưới dạng PNG chuẩn.
    
    LOGIC ĐỒNG BỘ: 
    1. Duyệt trang từ 0 -> N.
    2. Trong mỗi trang, lấy bboxes của ảnh và SẮP XẾP theo tọa độ Y (y0) để khớp 
       thứ tự gán nhãn [[[HINH ANH X]]] của AI.
    3. Ép mọi định dạng (JPG, JP2, ...) về PNG và hệ màu RGB để Word hiển thị tốt nhất.
    """
    danh_sach_duong_dan = []
    tai_lieu = None
    try:
        # 1. Mở tài liệu PDF
        tai_lieu = fitz.open(duong_dan_pdf)
        
        # 2. Tạo thư mục images/ bên trong thư mục đầu ra do C# chỉ định
        thu_muc_anh = os.path.join(thu_muc_dau_ra, "images")
        if not os.path.exists(thu_muc_anh):
            os.makedirs(thu_muc_anh)

        dem_anh_toan_cuc = 0
        
        # 3. Duyệt qua từng trang (Phải giống hệt logic hàm 'phan_tich_va_lay_ngu_canh')
        for so_trang in range(len(tai_lieu)):
            trang = tai_lieu[so_trang]
            
            # Lấy danh sách ảnh thô trên trang
            anh_trong_trang_tho = trang.get_images(full=True)
            
            # Thu thập bboxes để sắp xếp thứ tự xuất hiện thực tế trên trang
            vung_anh_co_toa_do = []
            for thong_tin_anh in anh_trong_trang_tho:
                try:
                    xref = thong_tin_anh[0]
                    # Lấy vùng bao (bbox) của ảnh trên trang giấy
                    vung_bao = trang.get_image_bbox(thong_tin_anh)
                    vung_anh_co_toa_do.append({
                        "xref": xref, 
                        "vung_bao": vung_bao
                    })
                except Exception:
                    # Bỏ qua nếu không lấy được tọa độ (ảnh dạng mask hoặc lỗi stream)
                    continue
            
            # SẮP XẾP: Ảnh nào nằm trên (y0 nhỏ hơn) thì xuất trước
            # Đây là mấu chốt để ID hinh_1, hinh_2... khớp với văn bản AI trả về
            vung_anh_co_toa_do.sort(key=lambda x: x["vung_bao"].y0)

            # 4. Trích xuất và xử lý ảnh đã sắp xếp
            for muc in vung_anh_co_toa_do:
                vung_bao = muc["vung_bao"]
                xref = muc["xref"]

                # BỘ LỌC KÍCH THƯỚC: Phải khớp tuyệt đối với logic AI (30x30px)
                if vung_bao.width < 30 or vung_bao.height < 30:
                    continue

                dem_anh_toan_cuc += 1
                
                try:
                    # Tạo Pixmap từ ảnh trong PDF
                    anh_pix = fitz.Pixmap(tai_lieu, xref)

                    # XỬ LÝ HỆ MÀU (Color Space Conversion)
                    # Nếu ảnh là CMYK hoặc có kênh Alpha (n > 3), ép về RGB
                    if anh_pix.colorspace.n > 3:
                        anh_pix_rgb = fitz.Pixmap(fitz.csRGB, anh_pix)
                    else:
                        anh_pix_rgb = anh_pix

                    # Đặt tên file: hinh_{ID}.png (phải khớp Regex của C#)
                    ten_file = f"hinh_{dem_anh_toan_cuc}.png"
                    duong_dan_full = os.path.join(thu_muc_anh, ten_file)

                    # Lưu file (Pixmap.save mặc định lưu dạng PNG)
                    anh_pix_rgb.save(duong_dan_full)
                    danh_sach_duong_dan.append(duong_dan_full)
                    
                    # Giải phóng vùng nhớ trung gian ngay lập tức
                    if anh_pix_rgb is not anh_pix:
                        anh_pix_rgb = None
                    anh_pix = None
                    
                except Exception as e_inner:
                    logging.error(f"Khong thuc hien duoc xref {xref} tai trang {so_trang+1}: {e_inner}")

        logging.info(f"Hoan tat: Da trich xuat {dem_anh_toan_cuc} hinh anh vao /images/")
        return danh_sach_duong_dan

    except Exception as e:
        logging.error(f"Loi nghiem trong trong qua trinh trich xuat anh: {str(e)}")
        logging.error(traceback.format_exc())
        return []
    finally:
        if tai_lieu:
            tai_lieu.close()


def main():
    """
    Ham chinh thuc thi pipeline AI DB-Driven.
    Ho tro Multimodal (Da phuong thuc): 
    1. rad_TuAnh: Xu ly danh sach anh tam tu Clipboard (input_data la List).
    2. rad_TuPDF_Anh: Xu ly PDF scan bang cach render tung trang thanh anh (ma_che_do = PDF_ANH).
    3. rad_GiuViTriAnh: Xu ly PDF goc, trich xuat anh vat ly va dung Sandwich Context (ma_che_do = TEXT_GIU_ANH).
    4. rad_ChiLayText: Xu ly PDF goc, chi trich xuat van ban (ma_che_do = CHI_TEXT).
    """
    global DB_PATH # Cap nhat bien toan cuc de cac ham lay_key/lay_prompt su dung
    output_path = None

    try:
        # 1. DOC INPUT JSON TU THAM SO DONG LENH (Giao tiep voi C#)
        if len(sys.argv) < 2:
            logging.error("Loi: Khong tim thay tham so file JSON dau vao.")
            return
            
        with open(sys.argv[1], "r", encoding="utf-8") as f:
            input_json = json.load(f)

        # 2. DONG BO HOA HA TANG (DATABASE PATH)
        # Uu tien dung duong dan dích danh do C# gui de tranh loi "no such table"
        DB_PATH = input_json.get("db_path")
        
        # 3. TRICH XUAT THAM SO DIEU KHIEN
        input_data = input_json.get("duong_dan_pdf") 
        ma_che_do = input_json.get("ma_che_do")
        output_path = input_json.get("output_path")
        
        # Chuan hoa ten mo hinh (Gemini 1.5 khong ton tai, chuyen ve 1.5 hoac 2.0 hop le)
        ten_mo_hinh = input_json.get("ten_mo_hinh", TEN_MO_HINH_MAC_DINH)
        if "2.5" in ten_mo_hinh: 
            ten_mo_hinh = "gemini-2.5-flash"

        # 4. KIEM TRA TINH HOP LE CUA HE THONG
        if not all([DB_PATH, input_data, ma_che_do, output_path]):
            raise ValueError("JSON dau vao thieu tham so bat buoc (db_path, input, mode, output).")

        if not os.path.exists(DB_PATH):
            raise FileNotFoundError(f"Database khong ton tai tai duong dan: {DB_PATH}")

        # 5. KHOI TAO PROMPT (Logic nam tai Python)
        prompts = tao_prompt_chuyen_sau(ma_che_do)
        
        # 6. RE NHANH XU LY DU LIEU (MULTIMODAL LOGIC)
        chunks = []
        
        # --- NHÁNH 1: Danh sach anh tu Clipboard (rad_TuAnh) ---
        if isinstance(input_data, list):
            logging.info(f"Phase 1: Dang xu ly {len(input_data)} anh tu Clipboard...")
            for idx, img_path in enumerate(input_data):
                if os.path.exists(img_path):
                    with open(img_path, "rb") as f:
                        chunks.append({
                            "data": f.read(),
                            "mime": "image/png",
                            "context": f"\n--- NOI DUNG ANH THU {idx + 1} TU CLIPBOARD ---\n"
                        })
                else:
                    logging.warning(f"Khong tim thay file anh tam: {img_path}")

        # --- NHÁNH 2: PDF dang anh/scan (rad_TuPDF_Anh) ---
        elif ma_che_do == "PDF_ANH":
            if not os.path.exists(input_data):
                raise FileNotFoundError(f"File PDF Scan khong ton tai: {input_data}")
            
            logging.info(f"Phase 1: Dang render PDF thanh anh (Scan Mode): {os.path.basename(input_data)}")
            # Render moi trang PDF thanh 1 anh PNG binary
            chunks = chuyen_pdf_thanh_danh_sach_anh(input_data)

        # --- NHÁNH 3: PDF van ban goc (rad_ChiLayText hoac rad_GiuViTriAnh) ---
        else:
            if not os.path.exists(input_data):
                raise FileNotFoundError(f"File PDF Native khong ton tai: {input_data}")

            # Neu la che do "Giu vi tri anh", can trich xuat anh vat ly ra folder de C# nhung sau nay
            if ma_che_do == "TEXT_GIU_ANH":
                logging.info("Phase 1: Dang trich xuat anh vat ly va phan tich Sandwich Context...")
                # Trich xuat anh ra subfolder /images/ cung cap voi output_path
                trich_xuat_anh_vat_ly(input_data, os.path.dirname(output_path))
            else:
                logging.info(f"Phase 1: Dang phan tich PDF (Text Only Mode): {os.path.basename(input_data)}")
            
            # Lay du lieu van ban kem theo "Mapping neo" (Sandwich Context) cho hinh anh
            chunks = phan_tich_va_lay_ngu_canh(input_data, ma_che_do)

        # 7. PIPELINE GOI AI VOI CO CHE XOAY VONG KEY (ROTATION)
        if not chunks:
            raise Exception("Khong trich xuat duoc noi dung/hinh anh hop le de gui cho AI.")

        logging.info(f"Phase 2: Bat dau goi AI (Model: {ten_mo_hinh}). So luong doan (chunks): {len(chunks)}")
        final_result = ""
        
        for idx, chunk in enumerate(chunks):
            logging.info(f"Dang gui yeu cau AI cho doan {idx + 1}/{len(chunks)}...")
            # Ham ai_reasoning_with_rotation thuc hien: Retry, Key Rotation va thiet lap System Instruction
            res_text = ai_reasoning_with_rotation(chunk, prompts, ten_mo_hinh)
            final_result += res_text + "\n\n"

        # 8. GHI KET QUA VA THOAT (Giao thuc dong bo voi C#)
        if not final_result.strip():
            raise Exception("AI hoan thanh nhung khong tra ve noi dung (Result Empty).")

        logging.info("Phase 3: Hoan tat, dang ghi file ket qua cho C#...")
        ghi_ket_qua_va_thoat(output_path, final_result.strip(), 0)

    except Exception:
        # LAY TRACEBACK CHI TIET DE HIEN THI TREN C# (Da fix loi font Unicode Escape)
        error_msg = traceback.format_exc()
        logging.error(error_msg)
        
        # Luon ghi file (du rong) de C# nhan biet nhiem vu ket thuc, tranh treo UI
        if output_path:
            try:
                with open(output_path, "w", encoding="utf-8") as f: 
                    f.write("")
            except: pass
            
        sys.exit(1) # Thoat voi ma loi 1 (Error)

if __name__ == "__main__":
    main()