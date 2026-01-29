@echo off
echo Dang thiet lap tin cay cho Tien Ich Toan Hoc Word...
:: Lenh cai dat vao vung Trusted Root
certutil -addstore -f "Root" "TienIchToanHocWord.cer"
:: Lenh cai dat vao vung Trusted Publisher
certutil -addstore -f "TrustedPublisher" "TienIchToanHocWord.cer"
echo Da thiet lap xong. Bay gio ban co the chay file setup.exe.
pause