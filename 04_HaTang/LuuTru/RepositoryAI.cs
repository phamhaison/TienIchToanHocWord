using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace TienIchToanHocWord.HaTang.LuuTru
{
    /// <summary>
    /// Lop duy nhat quan ly moi tuong tac giua C# va Database SQLite.
    /// Chuc nang: Khoi tao DB, Luu API Key tu UI.
    /// </summary>
    public class RepositoryAI
    {
        private readonly string _connectionString;
        private readonly string _dbPath;

        public RepositoryAI()
        {
            // Duong dan DB dung chung voi Python
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ai_toan_hoc.db");
            _connectionString = $"Data Source={_dbPath};Version=3;";
        }

        // =========================================================
        // KHOI TAO HE THONG (Gom tu KhoiTaoCoSoDuLieuChung)
        // =========================================================
        public void DamBaoHeThongSanSang()
        {
            if (!File.Exists(_dbPath)) { SQLiteConnection.CreateFile(_dbPath); }

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                // 1. Cập nhật câu lệnh tạo bảng: Bổ sung LyDoLoi
                string sqlKey = @"CREATE TABLE IF NOT EXISTS bang_api_key (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            GiaTri TEXT NOT NULL UNIQUE,
                            DangBiLoi INTEGER DEFAULT 0,
                            LyDoLoi TEXT, 
                            ThoiGianLoi TEXT);";

                string sqlPrompt = @"CREATE TABLE IF NOT EXISTS bang_prompt_xu_ly (
                                ma_che_do TEXT PRIMARY KEY,
                                prompt_he_thong TEXT,
                                prompt_nguoi_dung TEXT);";

                using (var cmd = new SQLiteCommand(sqlKey + sqlPrompt, conn)) { cmd.ExecuteNonQuery(); }

                // 2. LOGIC TỰ VÁ: Nếu file DB cũ đã có, ta phải thêm cột LyDoLoi thủ công
                try
                {
                    using (var cmd = new SQLiteCommand("ALTER TABLE bang_api_key ADD COLUMN LyDoLoi TEXT;", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { /* Cột đã tồn tại, bỏ qua lỗi */ }
            }
        }

        // =========================================================
        // QUAN LY API KEY (Dung cho UI FormNhapApiKey)
        // =========================================================
        public void LuuDanhSachApiKey(List<string> danhSachKey)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    foreach (var key in danhSachKey)
                    {
                        string sql = "INSERT OR IGNORE INTO bang_api_key (GiaTri) VALUES (@key)";
                        using (var cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@key", key);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    trans.Commit();
                }
            }
        }

        public int DemSoApiKey()
        {
            try
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM bang_api_key", conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch { return 0; }
        }

        public void DatLaiTrangThaiLoi()
        {
            try
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    // Đảm bảo lệnh SQL khớp với các cột đã khai báo
                    string sql = "UPDATE bang_api_key SET DangBiLoi = 0, LyDoLoi = NULL, ThoiGianLoi = NULL";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Loi Reset Key: " + ex.Message);
            }
        }
    }
}