const BASE_URL = "http://localhost:5141/api";

async function callApi(endpoint, method = "GET", data = null) {
  const token = localStorage.getItem("jwt_token");
  const headers = { "Content-Type": "application/json" };

  if (token) {
    headers["Authorization"] = "Bearer " + token;
  }

  const options = { method: method, headers: headers };

  if (data) {
    options.body = JSON.stringify(data);
  }

  try {
    const response = await fetch(BASE_URL + endpoint, options);
    const result = await response.json();
    return { status: response.status, data: result };
  } catch (error) {
    console.error("Lỗi kết nối API:", error);
    return null;
  }
}

function kiemTraDangNhap() {
  if (!localStorage.getItem("jwt_token")) {
    window.location.href = "login.html";
  }
}

function dangXuat() {
  localStorage.removeItem("jwt_token");
  window.location.href = "login.html";
}
function phanQuyenGiaoDien() {
  const role = localStorage.getItem("userRole");

  if (role === "Receptionist") {
    const menuPackage = document.getElementById("menu-package");
    const menuReport = document.getElementById("menu-report");
    const cardDoanhThu = document.getElementById("card-doanhthu");
    const chartDoanhThu = document.getElementById("chart-doanhthu");

    if (menuPackage) menuPackage.style.display = "none";
    if (menuReport) menuReport.style.display = "none";
    if (cardDoanhThu) cardDoanhThu.style.display = "none";
    if (chartDoanhThu) chartDoanhThu.style.display = "none";
  }

  const userRoleDisplay = document.getElementById("userRoleDisplay");
  if (userRoleDisplay) {
    userRoleDisplay.innerText =
      role === "Admin" ? "Quản trị viên" : "Nhân viên Lễ tân";
  }

  const userNameDisplay = document.getElementById("userNameDisplay");
  if (userNameDisplay) {
    userNameDisplay.innerText =
      role === "Admin" ? "Hoàng Hoắc Đô" : "Bùi Hoàng Anh";
  }
}

document.addEventListener("DOMContentLoaded", phanQuyenGiaoDien);
