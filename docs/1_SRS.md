# Software Requirements Specification (SRS) - Hệ thống Quản lý Hội viên Phòng Gym/Yoga

## 1. Giới thiệu

### 1.1 Mục đích

Dự án nhằm xây dựng một hệ thống phần mềm chuyên biệt giúp ban quản lý phòng Gym/Yoga số hóa toàn bộ quy trình. Hệ thống giải quyết bài toán quản lý thông tin hội viên, tự động hóa tính toán ngày hết hạn của thẻ tập, kiểm soát ra vào chặt chẽ bằng mã vạch/QR và quản lý các nghiệp vụ phát sinh như bảo lưu thẻ.

### 1.2 Phạm vi dự án

- **Trong phạm vi:** Quản lý danh mục (hội viên, nhân viên, gói tập), xử lý giao dịch bán/gia hạn thẻ, quản lý bảo lưu (freeze), kiểm soát check-in quét mã và thống kê doanh thu.
- **Ngoài phạm vi:** Không tích hợp thanh toán trực tuyến qua cổng ngân hàng (chỉ ghi nhận trạng thái thanh toán tiền mặt/chuyển khoản tại quầy).

## 2. Mô tả tổng quan

### 2.1 Các nhóm người dùng (User Classes)

- **Quản lý/Admin:** Có toàn quyền. Quản lý gói tập, nhân sự, xem báo cáo doanh thu, tình trạng thiết bị.
- **Nhân viên Lễ tân / Sale:** Tác nhân sử dụng hệ thống thường xuyên nhất. Được quyền thêm mới hội viên, bán/gia hạn gói tập, thu tiền, check-in cho khách và xử lý yêu cầu bảo lưu thẻ.
- **Hội viên (Member):** Đăng nhập để xem gói tập đang dùng, ngày hết hạn, lịch sử check-in và gửi yêu cầu bảo lưu thẻ.

### 2.2 Môi trường vận hành

- **Hệ thống Backend:** Windows Server / Linux Server chạy Web API (.NET 10).
- **Cơ sở dữ liệu:** SQL Server.
- **Giao diện:** Trình duyệt Web (Chrome, Edge, Safari) cho Admin/Lễ tân và App/Web di động cho Hội viên.

## 3. Yêu cầu chức năng (Functional Requirements)

| ID    | Tính năng                      | Mô tả (User Story)                                                                                                                   | Ưu tiên    |
| :---- | :----------------------------- | :----------------------------------------------------------------------------------------------------------------------------------- | :--------- |
| FR-01 | Quản lý Tài khoản & Phân quyền | Là người dùng, tôi muốn đăng nhập bằng Email/Password an toàn và chỉ truy cập được các chức năng theo vai trò (Admin, Sale, Member). | Cao        |
| FR-02 | Quản lý Hội viên               | Là Lễ tân, tôi muốn thêm, sửa, ẩn và tìm kiếm thông tin cá nhân của khách hàng qua SĐT.                                              | Cao        |
| FR-03 | Quản lý Gói tập                | Là Admin, tôi muốn tạo các gói tập (Gym 1 tháng, Yoga 1 năm) kèm thông số số ngày hiệu lực và đơn giá.                               | Cao        |
| FR-04 | Bán và Gia hạn thẻ (Core)      | Là Lễ tân, tôi muốn bán thẻ. Hệ thống phải tự động tính `Ngày hết hạn` (cộng dồn nếu thẻ cũ còn hạn, tính từ đầu nếu mua mới).       | Cao        |
| FR-05 | Kiểm soát Check-in (Core)      | Là Lễ tân, tôi muốn quét QR/Mã vạch để điểm danh. Hệ thống phải cảnh báo đỏ nếu thẻ hết hạn hoặc đang bị bảo lưu.                    | Cao        |
| FR-06 | Bảo lưu thẻ (Freeze)           | Là Hội viên/Lễ tân, tôi muốn xin tạm ngưng tập. Hệ thống tự động ghi nhận và cộng bù số ngày chưa tập vào hạn thẻ.                   | Trung bình |
| FR-07 | Cảnh báo hết hạn               | Là Lễ tân, tôi muốn xem danh sách hội viên sắp hết hạn thẻ (trong 3-7 ngày) để gọi điện chăm sóc.                                    | Trung bình |

## 4. Yêu cầu phi chức năng (Non-functional Requirements)

- **Công nghệ Back-end:** .NET 10, C# 14, Clean Architecture.
- **Bảo mật:** Tuyệt đối không lưu mật khẩu dạng plain-text. Mật khẩu phải được băm (Hash) bằng thư viện BCrypt. Bắt buộc có Token JWT để gọi API.
- **Độ tin cậy:** Logic tính toán ngày tháng (cộng dồn gia hạn, trừ ngày bảo lưu) phải chính xác 100%, không bị ảnh hưởng bởi năm nhuận hay tháng có 31/30 ngày.
