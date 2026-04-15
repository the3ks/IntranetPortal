import Link from 'next/link';

export default function AssetsWikiPage() {
  return (
    <div className="space-y-8 relative">
      <div className="absolute top-4 right-4 z-20">
        <Link href="/assets/wiki/en" className="inline-flex items-center gap-2 px-4 py-2 bg-background/50 hover:bg-background border border-border/50 text-foreground text-sm font-medium rounded-full shadow-sm transition-all">
          🇬🇧 Read in English
        </Link>
      </div>
      <div className="bg-card rounded-3xl p-8 border border-border/50 shadow-sm relative overflow-hidden">
        <div className="absolute top-0 right-0 w-64 h-64 bg-emerald-50 rounded-full blur-3xl -mr-20 -mt-20"></div>
        <div className="relative z-10">
          <h1 className="text-3xl font-black text-foreground mb-4 flex items-center gap-3">
            <svg className="w-8 h-8 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477-4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" /></svg>
            Wiki Quản lý Tài sản
          </h1>
          <p className="text-foreground/60 text-lg max-w-3xl">
            Chào mừng bạn đến với hướng dẫn toàn tập về hệ thống quản lý kho vật lý của Intranet.
            Phân hệ này tổ chức phần cứng, liên kết phần mềm và quyền lưu giữ của công ty theo thời gian thực.
          </p>
        </div>
      </div>

      <div className="bg-card rounded-3xl shadow-sm border border-border/50 overflow-hidden">
        <div className="px-8 py-6 border-b border-gray-50 bg-background/50/50">
          <h2 className="text-xl font-bold text-foreground flex items-center gap-2">
            <svg className="w-6 h-6 text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            Cách thức hoạt động
          </h2>
        </div>
        <div className="p-8 prose dark:prose-invert max-w-none prose-emerald prose-p:text-foreground/80 text-foreground prose-headings:text-foreground prose-strong:text-foreground prose-code:text-foreground">

          <h3 className="text-xl font-bold mt-0 mb-4 border-b border-border/50 pb-2">1. Dành cho Nhân viên</h3>
          <p className="mb-6">
            Là một nhân viên, thao tác chính của bạn với phân hệ này là theo dõi những gì bạn đang sở hữu và yêu cầu những gì bạn cần:
          </p>
          <ul className="list-disc pl-5 mb-8 space-y-3">
            <li><strong>Tài sản của tôi:</strong> Bảng này hiển thị mọi thiết bị có số sê-ri hoặc phụ kiện được liên kết với hồ sơ của bạn. Nếu bạn làm mất thiết bị, hãy liên hệ IT ngay lập tức.</li>
            <li><strong>Trung tâm Yêu cầu:</strong> Bạn cần bàn phím mới hoặc laptop chuyên dụng? Hãy kiểm tra từ điển nội bộ để xem các thiết bị có sẵn và gửi yêu cầu chính thức.</li>
            <li><strong>Điều hướng & Phê duyệt:</strong> Các yêu cầu được điều hướng thông minh dựa trên danh mục. Mặc dù quản lý trực tiếp có thể phê duyệt ngân sách, các thiết bị chuyên dụng đôi khi cần sự đồng ý bổ sung từ phòng ban tương ứng.</li>
          </ul>

          <h3 className="text-xl font-bold mt-10 mb-4 border-b border-border/50 pb-2">2. Quy trình Phê duyệt & Điều hướng</h3>
          <p className="mb-6">
            Hệ thống sử dụng <strong>Quy trình Điều hướng theo Danh mục</strong> để đảm bảo nhân sự phù hợp giám sát các luồng tài sản cụ thể:
          </p>
          <ul className="list-disc pl-5 mb-8 space-y-3">
            <li><strong>Quản lý Trực tiếp:</strong> Mặc định, các danh mục thường được cấu hình yêu cầu phê duyệt ngân sách hoặc tính cần thiết từ quản lý trực tiếp thông qua hàng đợi <strong>Chờ Phê duyệt</strong>.</li>
            <li><strong>Nhóm Phê duyệt Chỉ định:</strong> Đối với các danh mục chuyên biệt, luồng phê duyệt có thể tự động chuyển đến một Nhóm Phê duyệt định trước (ví dụ: Trưởng bộ phận hoặc người nắm ngân sách) thay vì quản lý trực tiếp.</li>
            <li><strong>Chỉ định Người phê duyệt:</strong> Nếu được bật rõ ràng trên danh mục, người yêu cầu có quyền linh hoạt ghi đè luồng phê duyệt mặc định và tự chọn một cá nhân cụ thể chịu trách nhiệm xử lý yêu cầu của họ.</li>
          </ul>

          <h3 className="text-xl font-bold mt-10 mb-4 border-b border-border/50 pb-2">3. Quản lý Danh mục & Cấp phát</h3>
          <p className="mb-6 border-l-4 border-emerald-500 pl-4 py-1 italic bg-emerald-50/50 rounded-r-lg">
            Lưu ý: Bạn phải có quyền <code>Perm:Assets.Manage</code> để thao tác ở cấp độ quản trị.
          </p>
          <ul className="list-disc pl-5 mb-4 space-y-3">
            <li><strong>Cấp phát Phân tán (Phân quyền):</strong> Không phải mọi tài sản đều do nhóm quản trị IT cấp phát. Mỗi danh mục có thể được giao cho một <em>Nhóm Quản trị</em> chuyên biệt (ví dụ: Nhóm "Hành chính" cấp phát nội thất, "IT Hardware" cấp phát máy tính).</li>
            <li><strong>Hàng đợi Cấp phát & Phân quyền:</strong> Khi các yêu cầu đã duyệt nằm trong Hàng đợi Cấp phát, bạn sẽ chỉ thấy và xử lý những thiết bị thuộc các danh mục mà nhóm của bạn được phép cấp phát. Bạn hoàn tất quá trình ở đây bằng cách cấp phát và liên kết thiết bị vật lý cụ thể với hồ sơ của nhân viên.</li>
            <li><strong>Quản trị viên Toàn cầu:</strong> Các quản trị viên toàn cầu có thể bỏ qua giới hạn cấp phát theo danh mục và kiểm soát toàn bộ từ điển hệ thống cũng như tất cả hàng đợi cấp phát mà không bị giới hạn.</li>
            <li><strong>Từ điển Hệ thống:</strong> Là kiến trúc nghiêm ngặt định nghĩa những gì có trong hệ thống hiện tại. <em>Danh mục</em> chứa <em>Mẫu thiết bị</em>. Bạn không thể đưa bất kỳ mẫu máy lạ nào vào hệ thống; chúng phải được định nghĩa trong từ điển này trước.</li>
            <li><strong>Tài sản Sê-ri và Phụ kiện chung:</strong> Tài sản được quản lý và bàn giao ở đây là những thiết bị có số sê-ri chặt chẽ (Ví dụ: Laptop). Các vật dụng chung như chuột, bàn phím cơ bản... được quản lý với tư cách là số lượng ở phân hệ <em>Phụ kiện</em> riêng biệt.</li>
          </ul>

        </div>
      </div>
    </div>
  );
}
