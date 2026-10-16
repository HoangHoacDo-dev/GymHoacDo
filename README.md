# 🏋️ GymHoacDo – Hệ thống quản lý phòng gym

**GymHoacDo** là dự án đồ án môn học xây dựng hệ thống quản lý phòng gym, hỗ trợ quản lý thông tin hội viên, đăng ký gói tập, check-in và theo dõi hoạt động của phòng tập.

Dự án được phát triển với mục tiêu số hóa quy trình quản lý, giảm thao tác thủ công và hỗ trợ nhân viên quản lý thông tin thuận tiện hơn.

**Repository:** [HoangHoacDo-dev/GymHoacDo](https://github.com/HoangHoacDo-dev/GymHoacDo)

---

## 📌 Mục lục

- [Giới thiệu](#-giới-thiệu)
- [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
- [Chức năng chính](#-chức-năng-chính)
- [Cấu trúc dự án](#-cấu-trúc-dự-án)
- [Yêu cầu hệ thống](#-yêu-cầu-hệ-thống)
- [Cài đặt và chạy dự án](#-cài-đặt-và-chạy-dự-án)
- [Kiểm thử](#-kiểm-thử)
- [Làm việc với Git](#-làm-việc-với-git)
- [Tài liệu](#-tài-liệu)

## 🎯 Giới thiệu

GymHoacDo được xây dựng theo mô hình ứng dụng web, gồm giao diện người dùng và API xử lý nghiệp vụ.

Hệ thống sử dụng ASP.NET Core Web API để xử lý các yêu cầu từ giao diện, kết hợp Entity Framework Core để truy cập cơ sở dữ liệu SQL Server.

Dự án hướng đến việc hỗ trợ các nghiệp vụ quản lý phòng gym trong một hệ thống tập trung, dễ sử dụng và thuận tiện cho việc mở rộng.

## 🛠️ Công nghệ sử dụng

| Công nghệ             | Vai trò                                     |
| --------------------- | ------------------------------------------- |
| C# / .NET             | Ngôn ngữ và nền tảng phát triển API         |
| ASP.NET Core Web API  | Xây dựng các API phục vụ ứng dụng           |
| Entity Framework Core | Truy cập và thao tác dữ liệu                |
| Microsoft SQL Server  | Hệ quản trị cơ sở dữ liệu                   |
| HTML, CSS, JavaScript | Xây dựng giao diện web                      |
| Scalar                | Giao diện khám phá và kiểm thử API          |
| Docker Compose        | Hỗ trợ cấu hình môi trường chạy bằng Docker |
| Git & GitHub          | Quản lý mã nguồn và phối hợp phát triển     |
| xUnit / .NET Test     | Kiểm thử tự động theo cấu hình dự án        |

## ✨ Chức năng chính

Các giao diện và nghiệp vụ được tổ chức theo những nhóm chức năng sau:

- **Quản lý hội viên:** Theo dõi thông tin và trạng thái hội viên.
- **Đăng ký hội viên:** Hỗ trợ giao diện đăng ký tài khoản.
- **Đăng nhập:** Cung cấp giao diện đăng nhập hệ thống.
- **Quản lý gói tập:** Hỗ trợ giao diện đăng ký và theo dõi gói tập.
- **Check-in:** Giao diện phục vụ việc ghi nhận lượt vào phòng tập.
- **Báo cáo:** Giao diện theo dõi và tổng hợp thông tin hoạt động.
- **REST API:** Cung cấp các endpoint để giao diện trao đổi dữ liệu với máy chủ.
- **Kiểm thử tự động:** Hỗ trợ kiểm tra khả năng build và tính đúng đắn của nghiệp vụ.

_Lưu ý: Phạm vi chức năng thực tế phụ thuộc vào các nghiệp vụ đã được triển khai trong mã nguồn._

## 📂 Cấu trúc dự án

```text
GymHoacDo/
├── .github/
│   └── workflows/          # Quy trình CI và chấm điểm tự động
├── docs/                   # Tài liệu phân tích, thiết kế hệ thống
├── Fronend/                # Giao diện người dùng
│   ├── assets/             # Tài nguyên giao diện
│   ├── index.html          # Trang chính
│   ├── login.html          # Giao diện đăng nhập
│   ├── dangky.html         # Giao diện đăng ký
│   ├── member.html         # Giao diện hội viên
│   ├── checkin.html        # Giao diện check-in
│   ├── subscription.html   # Giao diện gói tập
│   └── report.html         # Giao diện báo cáo
├── src/
│   └── API/                # ASP.NET Core Web API
│       ├── Controllers/    # Các API controller
│       ├── Core/            # Thành phần nghiệp vụ dùng chung
│       ├── Properties/     # Cấu hình khởi chạy
│       ├── Program.cs      # Điểm khởi chạy API
│       ├── appsettings.json
│       └── API.csproj
├── tests/
│   ├── AutogradingTests/   # Bộ kiểm thử chấm điểm
│   └── StudentTests/       # Khu vực viết kiểm thử của sinh viên
├── docker-compose.yml      # Cấu hình dịch vụ Docker
├── Directory.Build.props   # Cấu hình build dùng chung
├── Capstone.slnx           # File solution .NET
├── README.md               # Tài liệu dự án
└── .gitignore              # Các file không đưa vào Git
```

## 💻 Yêu cầu hệ thống

Trước khi chạy dự án, cần chuẩn bị:

- Windows, Linux hoặc macOS.
- [.NET SDK](https://dotnet.microsoft.com/download) tương thích với phiên bản được khai báo trong file `src/API/API.csproj`.
- Microsoft SQL Server.
- Visual Studio Code hoặc Visual Studio.
- Git.
- Trình duyệt web hiện đại.

Nếu chạy SQL Server bằng Docker, cần cài đặt thêm Docker Desktop hoặc Docker Engine.

## 🚀 Cài đặt và chạy dự án

### Bước 1: Clone repository

```bash
git clone https://github.com/HoangHoacDo-dev/GymHoacDo.git
cd GymHoacDo
```

### Bước 2: Kiểm tra môi trường .NET

```bash
dotnet --info
```

Đảm bảo SDK phù hợp với phiên bản .NET của dự án.

### Bước 3: Cấu hình cơ sở dữ liệu

Kiểm tra các file cấu hình trong `src/API/`, bao gồm `appsettings.json` và `appsettings.Development.json`.

Cập nhật chuỗi kết nối SQL Server phù hợp với môi trường máy của bạn.

Ví dụ:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1436;Database=GymManagerDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True"
  }
}
```

Đây chỉ là ví dụ cấu hình. Hãy thay thông tin kết nối theo môi trường thực tế của bạn.

**Bảo mật:** Không commit mật khẩu thật, file `.env` hoặc thông tin xác thực lên repository công khai. Nên sử dụng User Secrets hoặc biến môi trường để lưu thông tin nhạy cảm.

Nếu dự án sử dụng Docker Compose để khởi chạy SQL Server, kiểm tra cấu hình trong `docker-compose.yml` trước khi khởi động dịch vụ.

### Bước 4: Khôi phục các package

```bash
dotnet restore Capstone.slnx
```

### Bước 5: Build dự án

```bash
dotnet build Capstone.slnx
```

### Bước 6: Khởi chạy API

```bash
dotnet run --project src/API/API.csproj
```

Kiểm tra URL và cổng khởi chạy được hiển thị trong Terminal.

Nếu API chạy tại cổng `5141`, mở Scalar tại:

**http://localhost:5141/scalar/v1**

Scalar cho phép xem các endpoint được công bố và gửi yêu cầu kiểm thử API.

> Nếu cổng chạy thực tế khác `5141`, hãy sử dụng đúng địa chỉ được in ra trong Terminal.

### Bước 7: Chạy giao diện Frontend

Mở thư mục `Fronend/` trong VS Code.

Có thể sử dụng tiện ích Live Server để mở các trang HTML. Nếu API và frontend chạy trên các cổng khác nhau, hãy kiểm tra URL API được cấu hình trong JavaScript để đảm bảo giao diện kết nối đúng máy chủ.

## 🧪 Kiểm thử

Dự án hỗ trợ kiểm thử tự động theo cấu hình solution và các project test.

Chạy toàn bộ test:

```bash
dotnet test Capstone.slnx
```

Hoặc thử lệnh theo hướng dẫn của bộ test:

```bash
dotnet test tests/
```

Kiểm tra kết quả trong Terminal để biết số lượng test thành công và thất bại.

### Nguyên tắc kiểm thử

- Viết test cho các nghiệp vụ quan trọng.
- Kiểm tra cả trường hợp hợp lệ và dữ liệu không hợp lệ.
- Không sử dụng assertion giả để làm test luôn thành công.
- Không chỉnh sửa hoặc vô hiệu hóa các bài test chấm điểm do giảng viên cung cấp.
- Đặt các bài test tự viết trong `tests/StudentTests/` theo cấu trúc mà bộ chấm điểm yêu cầu.

## 🌿 Làm việc với Git

Dự án sử dụng Git và GitHub để quản lý phiên bản mã nguồn.

### 1. Cập nhật nhánh main

```bash
git switch main
git pull origin main
```

### 2. Tạo nhánh cho tính năng mới

```bash
git switch -c feature/member-management
```

Đặt tên nhánh theo tính năng hoặc lỗi cần xử lý, ví dụ:

- `feature/member-management`
- `feature/checkin`
- `feature/subscription`
- `fix/database-connection`

### 3. Commit thay đổi

```bash
git add .
git commit -m "Add member management"
```

Trước khi commit, kiểm tra `git status` để tránh đưa mật khẩu, file dữ liệu cục bộ hoặc file sinh ra khi build vào Git.

### 4. Push nhánh và tạo Pull Request

```bash
git push -u origin feature/member-management
```

Sau đó mở repository trên GitHub và tạo Pull Request để xem xét, kiểm tra và hợp nhất thay đổi.

**Quy tắc quan trọng:**

- Không phát triển trực tiếp trên `main`.
- Cập nhật mã nguồn mới nhất trước khi tích hợp thay đổi.
- Không sử dụng `git push --force` để ghi đè lịch sử dùng chung.
- Không sửa các file trong `.github/workflows/` và `tests/AutogradingTests/` nếu không được phép.

## 📚 Tài liệu

- **Repository:** [GymHoacDo trên GitHub](https://github.com/HoangHoacDo-dev/GymHoacDo)
- **Tài liệu dự án:** Thư mục `docs/`.
- **API Explorer:** `/scalar/v1` trên địa chỉ API đang chạy.
- **.NET Documentation:** [Microsoft Learn](https://learn.microsoft.com/dotnet/)

## 👨‍💻 Tác giả và đóng góp

**HoangHoacDo-dev** – Chủ repository GymHoacDo.

Dự án được phát triển trong khuôn khổ đồ án môn học. Các thành viên có thể đóng góp bằng cách tạo nhánh riêng, thực hiện thay đổi, kiểm thử và gửi Pull Request.

## 📄 Giấy phép

Chưa xác định giấy phép phân phối của dự án. Vui lòng bổ sung file `LICENSE` nếu muốn quy định rõ quyền sử dụng, sửa đổi và phân phối mã nguồn.