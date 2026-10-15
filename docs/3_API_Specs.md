# Tài liệu API

# API Specification - Hệ thống Quản lý Hội viên Phòng Gym/Yoga

## 1. Thông tin chung

- **Base URL:** `https://api.gymmanager.com/v1`
- **Định dạng:** `application/json`
- **Xác thực:** Bearer Token (JWT)

## 2. Chi tiết Endpoints

### 2.1 Đăng nhập hệ thống (Auth)

- **Endpoint:** `/api/Auth/login`
- **Method:** `POST`
- **Body:**
  {
  "email": "admin@gym.com",
  "password": "123"
  }
- **Response (200 OK):**
  {
  "status": "success",
  "data": {
  "token": "eyJhbGciOiJIUzI1NiIsInR...",
  "role": "Admin",
  "employee_id": 1,
  "full_name": "Nguyễn Văn Admin"
  }
  }

### 2.2 Lấy danh sách hội viên

- **Endpoint:** `/api/Member`
- **Method:** `GET`
- **Query Params:**
  - `limit` (int, optional): Số lượng item trên 1 trang.
  - `page` (int, optional): Số trang.
  - `search` (string, optional): Tìm kiếm theo tên hoặc SĐT.
- **Response (200 OK):**
  {
  "status": "success",
  "data": [
  {
  "id": 115,
  "full_name": "Hoàng Hoắc Đô",
  "phone_number": "0987654321",
  "gender": "Nam"
  }
  ],
  "pagination": { "current_page": 1, "total_pages": 5 }
  }

### 2.3 Thêm hội viên mới

- **Endpoint:** `/api/Member`
- **Method:** `POST`
- **Body:**
  {
  "full_name": "Nguyễn Văn A",
  "phone_number": "0911222333",
  "dob": "1995-10-15",
  "gender": "Nam",
  "address": "Vinh, Nghệ An"
  }
- **Responses:**
  - `201 Created`: Thêm mới thành công, trả về ID mới tạo.
  - `400 Bad Request`: Dữ liệu đầu vào không hợp lệ.
  - `401 Unauthorized`: Chưa xác thực JWT.

### 2.3.1 Sửa thông tin hội viên

- **Endpoint:** `/api/Member/{id}`
- **Method:** `PUT`
- **Body:**
  {
  "full_name": "Nguyễn Văn B",
  "phone_number": "0911222444",
  "dob": "1995-10-15",
  "gender": "Nam",
  "address": "Cửa Lò, Nghệ An"
  }
- **Responses:**
  - `200 OK`: Cập nhật thông tin thành công.
  - `400 Bad Request`: Dữ liệu đầu vào không hợp lệ.
  - `404 Not Found`: Không tìm thấy hội viên với ID tương ứng để cập nhật.
  - `401 Unauthorized`: Chưa xác thực JWT.

### 2.3.2 Xóa hội viên

- **Endpoint:** `/api/Member/{id}`
- **Method:** `DELETE`
- **Body:** (Không cần truyền Body, chỉ cần ID trên URL)
- **Responses:**
  - `200 OK`: Đã xóa hội viên thành công.
  - `404 Not Found`: Không tìm thấy hội viên với ID tương ứng để xóa.
  - `401 Unauthorized`: Chưa xác thực JWT.

### 2.4 Lấy danh sách gói tập

- **Endpoint:** `/api/Package`
- **Method:** `GET`
- **Response (200 OK):**
  {
  "status": "success",
  "data": [
  { "id": 1, "name": "Gym 1 Tháng", "duration_in_days": 30, "price": 500000 },
  { "id": 2, "name": "Yoga 1 Năm", "duration_in_days": 365, "price": 4500000 }
  ]
  }

### 2.4.1 Thêm gói tập mới

- **Endpoint:** `/api/Package`
- **Method:** `POST`
- **Body:**
  {
  "name": "Zumba Mùa Hè",
  "duration_in_days": 90,
  "price": 1200000
  }
- **Responses:**
  - `201 Created`: Thêm gói tập thành công.
  - `400 Bad Request`: Dữ liệu không hợp lệ (VD: giá tiền âm, số ngày âm).
  - `401 Unauthorized`: Chưa xác thực JWT (Chỉ Admin mới được tạo).

### 2.4.2 Sửa gói tập

- **Endpoint:** `/api/Package/{id}`
- **Method:** `PUT`
- **Body:**
  {
  "name": "Gym 1 Tháng (Cập nhật giá)",
  "duration_in_days": 30,
  "price": 550000
  }
- **Responses:**
  - `200 OK`: Cập nhật gói tập thành công.
  - `400 Bad Request`: Dữ liệu đầu vào không hợp lệ.
  - `404 Not Found`: Không tìm thấy gói tập.

### 2.4.3 Xóa gói tập

- **Endpoint:** `/api/Package/{id}`
- **Method:** `DELETE`
- **Responses:**
  - `200 OK`: Đã xóa gói tập thành công.
  - `400 Bad Request`: Không thể xóa do gói tập này đã có hội viên đăng ký.
  - `404 Not Found`: Không tìm thấy gói tập.

### 2.5 Bán mới / Gia hạn thẻ tập (Core Logic)

- **Endpoint:** `/api/Subscription`
- **Method:** `POST`
- **Body:**
  {
  "member_id": 115,
  "package_id": 2,
  "employee_id": 1
  }
- **Responses:** - `201 Created`: Giao dịch thành công, hệ thống tự động tính ngày hết hạn dựa trên logic cộng dồn
  {
  "status": "success",
  "data": {
  "subscription_id": 10,
  "start_date": "2026-04-09",
  "end_date": "2027-04-09",
  "status": "Active"
  }
  }
  - `400 Bad Request`: Gói tập không tồn tại hoặc ID hội viên sai.

### 2.5.1 Lấy thông tin thẻ tập hiện tại

- **Endpoint:** `/api/Subscription/Member/{member_id}`
- **Method:** `GET`
- **Response (200 OK):**
  {
  "status": "success",
  "data": {
  "package_name": "Yoga 1 Năm",
  "start_date": "2026-04-09",
  "end_date": "2027-04-09",
  "remaining_days": 365,
  "status": "Active (Đang hoạt động)"
  }
  }

### 2.6 Yêu cầu bảo lưu thẻ (Freeze)

- **Endpoint:** `/api/Subscription/{id}/freeze`
- **Method:** `POST`
- **Body:**
  {
  "start_date": "2026-05-01",
  "end_date": "2026-05-15",
  "reason": "Đi công tác dài ngày"
  }
- **Responses:** - `200 OK`: Bảo lưu thành công. Ghi nhận dữ liệu vào bảng freeze_logs .
  {
  "status": "success",
  "message": "Bảo lưu thành công. Thẻ sẽ được cộng thêm 15 ngày."
  }
  - `400 Bad Request`: Thời gian bảo lưu không hợp lệ.

### 2.6.1 Hủy bảo lưu sớm (Unfreeze)

- **Endpoint:** `/api/Subscription/{id}/unfreeze`
- **Method:** `POST`
- **Responses:**
  - `200 OK`: Hủy bảo lưu thành công, thẻ được kích hoạt lại và trừ đi số ngày chưa sử dụng.
  - `400 Bad Request`: Thẻ hiện không trong thời gian bảo lưu.

### 2.7 Quét mã Check-in ra vào

- **Endpoint:** `/api/CheckIn`
- **Method:** `POST`
- **Body:**
  {
  "phone_number": "0987654321"
  }
- **Responses:** - `200 OK`: Thẻ hợp lệ, ghi nhận check-in thành công .
  {
  "status": "success",
  "message": "Xin chào Hoàng Hoắc Đô. Check-in thành công.",
  "checkin_time": "2026-04-09T08:30:00"
  }
  - `403 Forbidden`: Thẻ đã hết hạn hoặc đang trong thời gian bảo lưu (Frozen) .

### 2.7.1 Xem lịch sử Check-in

- **Endpoint:** `/api/CheckIn/History/{member_id}`
- **Method:** `GET`
- **Response (200 OK):**
  {
  "status": "success",
  "total_checkins": 2,
  "data": [
  "2026-04-10 08:30:00",
  "2026-04-09 17:45:00"
  ]
  }

  ### 2.7.2 Lấy danh sách check-in trong ngày hôm nay

- **Endpoint:** `/api/CheckIn/Today`
- **Method:** `GET`
- **Response (200 OK):**
  {
  "status": "success",
  "total_today": 2,
  "data": [
  "member_name": "Hoàng a",
  "phone_number": "0987654321",
  "checkin_time": "2026-05-31 08:30:00"
  ]
  }

### 2.8 Báo cáo danh sách sắp hết hạn

- **Endpoint:** `/api/Report/Expiring`
- **Method:** `GET`
- **Query Params:**
  - `days_left` (int, optional): Lọc theo số ngày còn lại (VD: 7 ngày tới).
- **Response (200 OK):**
  {
  "status": "success",
  "data": [
  {
  "member_name": "Trần Văn B",
  "phone_number": "0922333444",
  "package_name": "Gym 1 Tháng",
  "end_date": "2026-04-12",
  "remaining_days": 3
  }
  ]
  }

### 2.9 Báo cáo danh sách thẻ đang bảo lưu

- **Endpoint:** `/api/Report/Frozen`
- **Method:** `GET`
- **Response (200 OK):**
  {
  "status": "success",
  "total_frozen": 1,
  "data": [
  {
  "member_name": "Trần Văn Thiên",
  "phone_number": "0922334455",
  "package_name": "Gym 6 Tháng",
  "frozen_from": "2026-05-15",
  "frozen_to": "2026-06-15",
  "reason": "Chấn thương"
  }
  ]
  }
