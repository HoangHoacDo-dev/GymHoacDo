# Nhật ký Sprint

# Project Sprint Logs - Hệ thống Quản lý Hội viên Phòng Gym/Yoga

## Sprint 01: Thiết lập Core Architecture, Auth & Business Logic

- **Thời gian:** 10/04/2026 - 24/04/2026
- **Mục tiêu (Sprint Goal):** Khởi tạo dự án theo Clean Architecture, thiết lập bảo mật JWT + BCrypt, phân tích CSDL (6 bảng), và hoàn thành các API nghiệp vụ cốt lõi (Bán gói, Gia hạn cộng dồn ngày, Check-in).

### 1. Danh sách công việc (Backlog)

| ID   | Tên Task / User Story                             | Người phụ trách            | Trạng thái      | Ghi chú                                                                                       |
| :--- | :------------------------------------------------ | :------------------------- | :-------------- | :-------------------------------------------------------------------------------------------- |
| T-01 | Phân tích CSDL chi tiết (6 bảng) & Vẽ ERD         | Nguyễn Anh Tuấn (24ATT154) | [x] Done        | Đã chốt xong các quan hệ 1-N, N-N thông qua bảng subscriptions.                               |
| T-02 | Setup Clean Architecture, JWT Token & BCrypt Auth | Trần Văn Thiên (24ATT171)  | [x] Done        | Hoàn thành theo bài Lab của giảng viên.                                                       |
| T-03 | Thiết kế Lớp Entities và Interfaces (Tầng Core)   | Bùi Hoàng Anh (24ATT101)   | [x] Done        | Sử dụng C# 14 required modifier để đảm bảo toàn vẹn dữ liệu.                                  |
| T-04 | Code Logic tính hạn thẻ, cộng dồn ngày, Check-in  | Hoàng Hoắc Đô (24ATT115)   | [/] In Progress | Xử lý xong Case 1 (gia hạn) và Case 2 (mua mới). Đang làm Case Check-in cảnh báo thẻ bảo lưu. |
| T-05 | Kiểm thử hệ thống (Test Cases) & Viết báo cáo     | Chu Tự Tuấn Anh (24ATT102) | [ ] To Do       | Chờ T-04 hoàn thiện API để gọi thử nghiệm qua Postman/Scalar UI.                              |

_Ghi chú trạng thái: [ ] To Do, [/] In Progress, [x] Done_

### 2. Tổng kết Sprint (Sprint Retrospective)

- **Thành công:** - Team đã chốt được kiến trúc Database nhanh chóng. Giải quyết gọn gàng bảng trung gian `subscriptions` để triệt tiêu quan hệ N-N giữa Hội viên và Gói tập.
  - Áp dụng thành công Clean Architecture và cơ chế mã hóa mật khẩu theo đúng hướng dẫn.
- **Khó khăn:** - Logic tính toán `Ngày hết hạn mới` khi thẻ đang bị bảo lưu (Freeze) khá phức tạp, phát sinh bug liên quan đến định dạng Timezone khi lưu dữ liệu DATETIME vào SQL Server.
- **Hành động cải thiện (Action Items):** - Sprint sau cần viết Unit Test (xUnit) phủ kín các Test Case trước khi code logic nghiệp vụ để tránh hồi quy lỗi (TDD).
  - Cần thống nhất sử dụng múi giờ (UTC) khi lưu `start_date` và `end_date` vào CSDL để tránh sai lệch.
