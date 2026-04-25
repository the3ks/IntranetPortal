Dưới đây là bản **tổng hợp features/functions** của module *Phê duyệt Văn phòng phẩm* được viết lại theo góc nhìn **user/business**, dễ hiểu, bỏ qua technical (DB, API, schema).

---

# 🎯 Tổng quan module

Module này giúp công ty quản lý việc:

* Nhân viên **yêu cầu cấp phát văn phòng phẩm**
* Quản lý **phê duyệt (1 cấp)**
* Tạo **phiếu PDF chuẩn để xuất kho**
* Và **tự động liên kết với tồn kho** 

👉 Nói đơn giản:
**Giống một “shopping cart nội bộ” → gửi duyệt → xuất kho → có chứng từ PDF.**

---

# 👥 Các vai trò chính

* **Nhân viên (Requester)**: tạo yêu cầu
* **Quản lý (Approver)**: duyệt hoặc từ chối
* **Kho/Admin**: theo dõi & xuất kho
* **Admin hệ thống**: quản lý danh mục vật phẩm 

---

# 🧩 Các chức năng chính

## 1. Xem & tìm kiếm văn phòng phẩm (Catalog)

Người dùng có thể:

* Xem danh sách vật phẩm
* Tìm kiếm nhanh theo tên
* Lọc theo:

  * Phòng ban
  * Phân loại
* Xem thông tin:

  * Tên, hình ảnh, đơn vị
  * Áp dụng cho phòng ban nào
  * Có còn dùng hay không 

👉 Trải nghiệm giống “chọn hàng” trong app shopping.

---

## 2. Tạo yêu cầu (Cart / Draft)

Người dùng có thể:

* Thêm vật phẩm vào “giỏ”
* Nhập:

  * Số lượng
  * Ghi chú (optional)
* Xóa / chỉnh sửa item
* Xem tổng số lượng, tổng item

👉 Đây là trạng thái **Draft (chưa gửi)**.

---

## 3. Chọn người phê duyệt

* Bắt buộc chọn **1 người duyệt**
* Có thể:

  * Hệ thống gợi ý sẵn
  * Người dùng thay đổi trước khi gửi

👉 Không có approver → **không được submit**

---

## 4. Xem trước phiếu PDF

Trước khi gửi, user có thể:

* Preview phiếu theo đúng mẫu thật
* Kiểm tra:

  * Danh sách vật phẩm
  * Số lượng
  * Người đề xuất, bộ phận

👉 Đây là bước rất quan trọng (giống “review đơn hàng”).

---

## 5. Gửi yêu cầu (Submit)

* Gửi request lên hệ thống
* Trạng thái chuyển sang:
  👉 **Chờ duyệt (Pending Approval)**

Sau khi gửi:

* Không chỉnh sửa được nữa
* Có mã số phiếu

---

## 6. Quản lý duyệt (Approval)

Dành cho manager:

### Inbox phê duyệt

* Xem danh sách yêu cầu cần duyệt
* Thông tin nhanh:

  * Ai gửi
  * Bộ phận
  * Số lượng item

### Chi tiết duyệt

* Xem full nội dung request
* Xem PDF
* Thực hiện:

  * ✅ Approve
  * ❌ Reject (có thể kèm lý do)

👉 Chỉ duyệt được khi request đang “Pending”

---

## 7. Kết quả phê duyệt

Sau khi xử lý:

* Nếu **Approve**

  * Request được duyệt
  * Chuyển sang bước kho
* Nếu **Reject**

  * Có lý do từ chối
  * Người gửi thấy được

---

## 8. Liên kết kho (Inventory)

Sau khi duyệt:

* Hệ thống sẽ:

  * Trừ tồn kho tương ứng
* Có trạng thái:

  * Đã trừ tồn
  * Hoặc lỗi trừ tồn

👉 Quan trọng:
**Approve ≠ hoàn tất → còn bước xử lý kho**

---

## 9. Phiếu PDF chính thức

Hệ thống sẽ:

* Sinh PDF theo mẫu chuẩn
* Người dùng có thể:

  * Xem lại
  * Tải về
  * In

👉 Đây là **chứng từ chính thức để xuất kho**

---

## 10. Xem lịch sử yêu cầu

Người dùng có thể:

* Xem tất cả request của mình
* Lọc theo:

  * Trạng thái
  * Thời gian
* Xem nhanh:

  * Đã duyệt / bị từ chối / chờ duyệt

---

## 11. Xem chi tiết request

Bao gồm:

* Thông tin người gửi
* Danh sách vật phẩm
* Người duyệt
* Kết quả duyệt
* Trạng thái kho
* PDF
* Timeline xử lý

---

## 12. Dashboard / Summary

Hệ thống có thể hiển thị:

* Tổng số request
* Bao nhiêu đang chờ duyệt
* Bao nhiêu đã duyệt / bị từ chối
* Inbox của approver có bao nhiêu pending 

---

# 🔄 Workflow tổng thể

```text
Chọn vật phẩm → Tạo request → Chọn approver
→ Preview PDF → Submit
→ Manager approve/reject
→ Nếu approve → Trừ tồn kho
→ Sinh PDF chính thức → Hoàn tất
```

---

# ⚠️ Các rule quan trọng

* Phải có ít nhất 1 item
* Số lượng > 0
* Phải có approver
* Chỉ 1 cấp duyệt
* Sau submit → không sửa được
* Sau approve → có ảnh hưởng kho
* PDF phải giống mẫu hiện tại 

---

# 💡 Tóm lại (ngắn gọn)

Module này gồm 4 phần chính:

1. **Catalog + Cart** → chọn đồ
2. **Approval** → xin duyệt
3. **Inventory** → trừ tồn
4. **PDF** → chứng từ chính thức

👉 Đây là một **workflow nội bộ hoàn chỉnh**, không chỉ là form hay approval đơn thuần.

---

Nếu bạn muốn, mình có thể:

* Rút gọn thành **1 slide cho business**
* Hoặc chuyển thành **user story / backlog cho dev team**
* Hoặc vẽ **flow diagram chuẩn BA** 👍

---

# 📊 So sánh với tính năng hiện tại (Gap Analysis)

| Tính năng trong Specs | Hiện trạng (Module Asset Management) | Những gì còn thiếu / Cần code thêm |
| :--- | :--- | :--- |
| **1. Catalog & Tìm kiếm** | ✅ **Có sẵn:** Quản lý Danh mục, Model, Phụ kiện. | ⚠️ **Thiếu (UI):** Bộ lọc hiển thị áp dụng riêng biệt cho từng phòng ban (Department Applicability filter) khi User truy cập. |
| **2. Giỏ hàng (Cart / Draft)** | ✅ **Backend hỗ trợ:** Database đã có `AssetRequestLineItem` (nhiều item trong 1 request). | ❌ **Thiếu (UI):** Giao diện "Giỏ hàng" (Shopping Cart) đích thực để nhân viên dễ dàng "Add to Cart" nhiều văn phòng phẩm cùng lúc trước khi gửi. |
| **3. Chọn & Ghi đè người duyệt** | ✅ **Đã hoàn thiện:** Rất mạnh mẽ (Line Manager, Designated Group, Override). | Không (Đã đáp ứng 100%). |
| **4. Xem trước phiếu xuất PDF** | ❌ **Chưa có** | Cần code Engine render nội dung request thành màn hình Preview PDF trước khi bấm Submit. |
| **5. Nộp đơn & Pending Status** | ✅ **Đã hoàn thiện:** Requests được đẩy vào Pending Approval. | Không. |
| **6. Inbox & Duyệt (Approve/Reject)** | ✅ **Đã hoàn thiện:** Màn hình Pending Approvals cho Manager/Group. | Không. |
| **7. Liên kết tự động trừ Tồn kho** | ⚠️ **Có nhưng thủ công:** Đang xử lý tay qua bước "Fulfillment Queue". | ❌ **Thiếu:** Trigger tự động trừ số lượng (`AvailableQuantity`) của model Accessories trên backend ngay khi Manager vừa duyệt. |
| **8. Sinh & Xuất phiếu PDF chính thức**| ❌ **Chưa có** | Hoàn toàn chưa có thư viện hay API để sinh **PDF chứng từ chuẩn** theo template lưu trữ, download và in ấn. |
| **9. Lịch sử & Chi tiết Request** | ✅ **Có sẵn:** Requisition Center theo dõi các yêu cầu. | ⚠️ **Thiếu:** Cần tích hợp nút Xem/Download PDF và hiển thị Timeline (dấu vết mốc thời gian xử lý chi tiết). |
| **10. Dashboard Thống kê/Summary** | ❌ **Chưa có** | Cần một màn hình Dashboard gom số liệu (Tổng Request, Pending, Approved, Rejected). |

### 💡 Tóm tắt phần cần phát triển
Để biến module hiện tại thành một "Shopping Workflow" thực thụ cho văn phòng phẩm, team Dev sẽ cần mở rộng thêm **3 User Story lớn**:
1. **Giao diện Giỏ hàng (Shopping Cart UI):** Bọc các action thành UI giỏ hàng để user chọn nhiều vật phẩm số lượng lớn một cách trực quan.
2. **Auto-Inventory Deduction:** Sửa lại logic duyệt ở Backend để tự trừ tồn (`Accessories.AvailableQuantity`) thay vì phải đợi bước Fulfillment cấp phát bằng tay.
3. **Module PDF Engine:** Tích hợp thư viện sinh PDF để cung cấp tính năng Review mẫu trước khi tạo và Tải file chứng từ gốc sau khi duyệt thành công.
