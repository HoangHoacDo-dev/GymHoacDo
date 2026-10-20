(function () {
  "use strict";

  const role = localStorage.getItem("userRole");
  if (!role) {
    window.location.href = "login.html";
    return;
  }
  if (role !== "Member") {
    window.location.href = "index.html";
    return;
  }

  const memberId = localStorage.getItem("memberId");
  const token = localStorage.getItem("jwt_token");
  const phone = readTokenClaim(token, "email");
  const fullName = readTokenClaim(token, "FullName") || "Hội viên";
  const qrNode = document.getElementById("maQrHoiVien");
  const qrStatus = document.getElementById("trangThaiQr");
  const copyButton = document.getElementById("btnSaoChepSdt");
  const downloadButton = document.getElementById("btnTaiQr");
  const refreshButton = document.getElementById("btnTaiLai");

  document.getElementById("portalYear").textContent = String(new Date().getFullYear());
  document.getElementById("tenHoiVien").textContent = fullName;
  document.getElementById("maHoiVien").textContent = memberId ? `#${memberId}` : "—";
  document.getElementById("soDienThoai").textContent = phone || "Chưa có số điện thoại";

  document.getElementById("btnDangXuat").addEventListener("click", function () {
    localStorage.clear();
    window.location.href = "login.html";
  });

  refreshButton.addEventListener("click", loadPortalData);
  copyButton.addEventListener("click", copyPhone);
  downloadButton.addEventListener("click", downloadQr);

  if (!memberId || !token) {
    setHistoryState("Không tìm thấy thông tin phiên đăng nhập. Vui lòng đăng nhập lại.", true);
    setSubscriptionEmpty("Không thể tải thông tin hội viên.");
    qrStatus.textContent = "Không tìm thấy thông tin tài khoản để tạo mã.";
  } else {
    if (phone) createQr(phone);
    else qrStatus.textContent = "Tài khoản chưa có số điện thoại trong phiên đăng nhập.";
    loadPortalData();
  }

  async function loadPortalData() {
    if (!memberId) return;
    refreshButton.disabled = true;
    refreshButton.classList.add("is-refreshing");
    setHistoryState("Đang tải lịch sử check-in...", false, true);

    const [subscriptionResponse, historyResponse] = await Promise.all([
      callApi(`/Subscription/Member/${encodeURIComponent(memberId)}`, "GET"),
      callApi(`/CheckIn/History/${encodeURIComponent(memberId)}`, "GET"),
    ]);

    renderSubscription(subscriptionResponse);
    renderCheckIns(historyResponse);

    refreshButton.disabled = false;
    refreshButton.classList.remove("is-refreshing");
  }

  function renderSubscription(response) {
    const name = document.getElementById("tenGoiTap");
    const endDate = document.getElementById("ngayHetHan");
    const remaining = document.getElementById("soNgayConLai");
    const chip = document.getElementById("trangThaiThe");
    const message = document.getElementById("trangThaiTheText");

    if (!response || response.status < 200 || response.status >= 300) {
      setSubscriptionEmpty("Không tải được dữ liệu thẻ. Hãy thử làm mới.");
      return;
    }

    const subscription = response.data && response.data.data;
    if (!subscription || typeof subscription !== "object") {
      setSubscriptionEmpty("Tài khoản hiện chưa có thông tin gói tập.");
      return;
    }

    const status = typeof subscription.status === "string" ? subscription.status : "Chưa xác định";
    const statusLower = status.toLowerCase();
    name.textContent = subscription.package_name || "Gói tập chưa xác định";
    endDate.textContent = formatDate(subscription.end_date) || "Chưa có thông tin";
    remaining.textContent = Number.isFinite(Number(subscription.remaining_days))
      ? `${subscription.remaining_days} ngày`
      : "—";
    chip.className = "status-chip";
    if (statusLower.includes("frozen")) chip.classList.add("status-frozen");
    else if (statusLower.includes("expired")) chip.classList.add("status-expired");
    else if (statusLower.includes("active")) chip.classList.add("status-active");
    else chip.classList.add("status-neutral");
    chip.innerHTML = "<span></span>";
    chip.append(document.createTextNode(status));

    if (statusLower.includes("frozen")) {
      message.textContent = "Thẻ đang được bảo lưu theo trạng thái từ hệ thống.";
    } else if (statusLower.includes("expired")) {
      message.textContent = "Thẻ đã hết hạn. Liên hệ lễ tân để được hỗ trợ gia hạn.";
    } else {
      message.textContent = "Trạng thái thẻ được cập nhật từ hệ thống hội viên.";
    }
  }

  function setSubscriptionEmpty(messageText) {
    document.getElementById("tenGoiTap").textContent = "Chưa có gói tập";
    document.getElementById("ngayHetHan").textContent = "—";
    document.getElementById("soNgayConLai").textContent = "—";
    const chip = document.getElementById("trangThaiThe");
    chip.className = "status-chip status-neutral";
    chip.innerHTML = "<span></span>Chưa có dữ liệu";
    document.getElementById("trangThaiTheText").textContent = messageText;
  }

  function renderCheckIns(response) {
    if (!response || response.status !== 200 || !response.data) {
      document.getElementById("tongCheckIn").textContent = "—";
      setHistoryState("Không tải được lịch sử. Hãy thử làm mới dữ liệu.", true);
      return;
    }

    const payload = response.data;
    const records = Array.isArray(payload.data) ? payload.data : [];
    const total = Number.isFinite(Number(payload.total_checkins))
      ? Number(payload.total_checkins)
      : records.length;
    document.getElementById("tongCheckIn").textContent = String(total);
    const tbody = document.getElementById("bangCheckInKhach");
    tbody.replaceChildren();

    if (records.length === 0) {
      setHistoryState("Chưa có lượt check-in nào được ghi nhận.", false);
      return;
    }

    records.slice(0, 10).forEach((record) => {
      const row = document.createElement("tr");
      const dateCell = document.createElement("td");
      dateCell.className = "checkin-date";
      dateCell.textContent = formatDateTime(record) || "Thời gian không xác định";
      const actionCell = document.createElement("td");
      actionCell.className = "checkin-kind";
      actionCell.textContent = "Check-in tại phòng tập";
      const stateCell = document.createElement("td");
      stateCell.className = "text-end";
      const badge = document.createElement("span");
      badge.className = "checkin-badge";
      const icon = document.createElement("i");
      icon.className = "bi bi-check-circle-fill";
      icon.setAttribute("aria-hidden", "true");
      badge.append(icon, document.createTextNode("Đã ghi nhận"));
      stateCell.append(badge);
      row.append(dateCell, actionCell, stateCell);
      tbody.append(row);
    });
  }

  function setHistoryState(messageText, isError, isLoading) {
    const tbody = document.getElementById("bangCheckInKhach");
    const row = document.createElement("tr");
    const cell = document.createElement("td");
    cell.colSpan = 3;
    cell.className = `table-state${isError ? " text-danger" : ""}`;
    if (isLoading) {
      const loader = document.createElement("span");
      loader.className = "portal-loader";
      loader.setAttribute("aria-hidden", "true");
      cell.append(loader);
    }
    cell.append(document.createTextNode(messageText));
    row.append(cell);
    tbody.replaceChildren(row);
  }

  function createQr(value) {
    if (typeof window.QRCode !== "function") {
      qrStatus.textContent = "Không tải được thư viện tạo QR. Vui lòng tải lại trang.";
      return;
    }
    qrNode.replaceChildren();
    try {
      new window.QRCode(qrNode, {
        text: value,
        width: 220,
        height: 220,
        colorDark: "#191a16",
        colorLight: "#ffffff",
        correctLevel: window.QRCode.CorrectLevel.M,
      });
      qrStatus.textContent = "Mã QR chứa số điện thoại check-in.";
      copyButton.disabled = false;
      downloadButton.disabled = false;
    } catch (_) {
      qrStatus.textContent = "Không thể tạo mã QR cho tài khoản này.";
    }
  }

  async function copyPhone() {
    if (!phone) return;
    try {
      await navigator.clipboard.writeText(phone);
      copyButton.setAttribute("aria-label", "Đã sao chép số điện thoại");
      window.setTimeout(() => copyButton.setAttribute("aria-label", "Sao chép số điện thoại"), 1600);
    } catch (_) {
      const temporary = document.createElement("textarea");
      temporary.value = phone;
      temporary.setAttribute("readonly", "");
      temporary.style.position = "fixed";
      temporary.style.opacity = "0";
      document.body.append(temporary);
      temporary.select();
      document.execCommand("copy");
      temporary.remove();
    }
  }

  function downloadQr() {
    const canvas = qrNode.querySelector("canvas");
    const image = qrNode.querySelector("img");
    const sourceNode = canvas || image;
    if (!sourceNode) return;

    const sourceWidth = canvas ? canvas.width : image.naturalWidth;
    const sourceHeight = canvas ? canvas.height : image.naturalHeight;
    if (!sourceWidth || !sourceHeight) return;

    // Add the QR standard's four-module quiet zone around the code.
    const quietZone = Math.ceil((sourceWidth / 21) * 4);
    const exportCanvas = document.createElement("canvas");
    exportCanvas.width = sourceWidth + quietZone * 2;
    exportCanvas.height = sourceHeight + quietZone * 2;
    const context = exportCanvas.getContext("2d");
    if (!context) return;
    context.fillStyle = "#ffffff";
    context.fillRect(0, 0, exportCanvas.width, exportCanvas.height);
    context.drawImage(sourceNode, quietZone, quietZone, sourceWidth, sourceHeight);

    const link = document.createElement("a");
    link.href = exportCanvas.toDataURL("image/png");
    link.download = "gymhoacdo-ma-checkin.png";
    link.click();
  }

  function readTokenClaim(jwt, key) {
    if (!jwt) return "";
    try {
      const payloadPart = jwt.split(".")[1];
      if (!payloadPart) return "";
      const base64 = payloadPart.replace(/-/g, "+").replace(/_/g, "/");
      const paddedBase64 = base64.padEnd(Math.ceil(base64.length / 4) * 4, "=");
      const json = decodeURIComponent(
        Array.from(atob(paddedBase64), (character) => `%${character.charCodeAt(0).toString(16).padStart(2, "0")}`).join(""),
      );
      const claims = JSON.parse(json);
      return typeof claims[key] === "string" ? claims[key] : "";
    } catch (_) {
      return "";
    }
  }

  function formatDate(value) {
    if (typeof value !== "string" || !value) return "";
    const date = new Date(`${value.slice(0, 10)}T00:00:00`);
    return Number.isNaN(date.getTime()) ? "" : date.toLocaleDateString("vi-VN");
  }

  function formatDateTime(value) {
    if (typeof value !== "string" || !value) return "";
    const date = new Date(value.includes("T") ? value : value.replace(" ", "T"));
    return Number.isNaN(date.getTime()) ? value : date.toLocaleString("vi-VN");
  }
})();
