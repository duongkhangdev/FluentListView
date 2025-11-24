# FluentListView - Ví dụ sử dụng (Vietnamese Guide)

## Giới thiệu

File mẫu `SampleFluentListViewForm.cs` trình bày cách sử dụng control FluentListView để tạo giao diện quản lý công việc (task manager) với đầy đủ các tính năng:

## Các tính năng được demo

1. **Cột Task (Công việc)**
   - Hiển thị tên công việc
   - Icon đại diện cho loại công việc
   - Sử dụng AspectGetter và ImageGetter để lấy dữ liệu từ model

2. **Cột Priority (Độ ưu tiên)**
   - Hiển thị mức độ ưu tiên: Low, Medium, High, Critical
   - Icon bóng đèn (hoặc biểu tượng màu sắc) cho mỗi mức độ
   - Low = Xám, Medium = Vàng, High = Đỏ, Critical = Đỏ đậm

3. **Cột Status (Trạng thái)**
   - Hiển thị trạng thái: Chưa làm, Đang làm, Hoàn tất, Tạm dừng
   - Icon trạng thái tương ứng:
     - Chưa làm (Not Started): Xám nhạt
     - Đang làm (In Progress): Cam
     - Hoàn tất (Completed): Xanh lá
     - Tạm dừng (Frozen): Xanh nhạt

4. **Cột Action (Hành động)**
   - Các nút nhấn thay đổi theo trạng thái công việc:
     - "Start" (Bắt đầu) - khi chưa làm
     - "Complete" (Hoàn tất) - khi đang làm
     - "Reopen" (Mở lại) - khi đã hoàn tất
     - "Unfreeze" (Tiếp tục) - khi tạm dừng
   - Xử lý sự kiện ButtonClick để thực hiện hành động
   - Shift+Click để đóng băng công việc

## Hướng dẫn sử dụng

### Cách 1: Thêm vào project WinForms có sẵn

1. Build FluentListView project:
   ```bash
   cd FluentListView
   dotnet build FluentListView.sln
   ```

2. Tạo project WinForms mới:
   ```bash
   dotnet new winforms -n QuanLyCongViec
   cd QuanLyCongViec
   ```

3. Thêm reference tới FluentListView.dll:
   ```bash
   dotnet add reference ../FluentListView/bin/Debug/net9.0-windows/FluentListView.dll
   ```

4. Copy file `SampleFluentListViewForm.cs` vào project

5. Sửa file `Program.cs`:
   ```csharp
   using System;
   using System.Windows.Forms;
   using FluentListViewSample;

   Application.EnableVisualStyles();
   Application.SetCompatibleTextRenderingDefault(false);
   Application.Run(new SampleFluentListViewForm());
   ```

6. Chạy chương trình:
   ```bash
   dotnet run
   ```

### Cách 2: Chạy trực tiếp

File `SampleFluentListViewForm.cs` có sẵn hàm Main() để chạy độc lập:

1. Build FluentListView
2. Compile file mẫu kèm reference
3. Chạy file exe được tạo ra

## Cấu trúc code

### Class chính

- **SampleFluentListViewForm**: Form chứa FluentListView
- **TaskItem**: Model class đại diện cho một công việc
- **TaskPriority**: Enum cho độ ưu tiên (Low, Medium, High, Critical)
- **TaskStatus**: Enum cho trạng thái (Not_Started, In_Progress, Completed, Frozen)

### Các phương thức quan trọng

1. **InitializeTaskListView()**: Khởi tạo và cấu hình FluentListView
   - Thiết lập view mode, font, row height
   - Bật hot tracking cho buttons
   - Gắn ImageList

2. **SetupColumns()**: Cấu hình các cột
   - Tạo cột Task với AspectGetter và ImageGetter
   - Tạo cột Priority với icon
   - Tạo cột Status với icon
   - Tạo cột Action với IsButton = true

3. **TaskListView_ButtonClick()**: Xử lý sự kiện nhấn nút
   - Đổi trạng thái công việc khi nhấn nút
   - Hiển thị thông báo
   - Refresh object để cập nhật giao diện

4. **LoadSampleData()**: Tải dữ liệu mẫu
   - Tạo danh sách TaskItem
   - Gọi SetObjects() để hiển thị

## Tùy chỉnh

### Thay đổi icon

Thay thế placeholder icons bằng file icon thật:

```csharp
// Trong InitializeImageLists()
smallImageList.Images.Add("task_default", Image.FromFile("icons/task.png"));
smallImageList.Images.Add("priority_high", Image.FromFile("icons/bulb_red.png"));
```

### Thêm cột mới

Ví dụ thêm cột "Người phụ trách":

```csharp
OLVColumn assigneeColumn = new OLVColumn("Người phụ trách", "Assignee");
assigneeColumn.AspectGetter = delegate (object rowObject)
{
    TaskItem task = (TaskItem)rowObject;
    return task.AssigneeName;
};
taskListView.AllColumns.Add(assigneeColumn);
```

### Thay đổi hành động nút

Sửa trong `TaskListView_ButtonClick()`:

```csharp
case "Start":
    task.Status = TaskStatus.In_Progress;
    task.StartDate = DateTime.Now;
    // Lưu vào database
    // Gửi notification
    break;
```

## Các tính năng nâng cao có thể thêm

1. **Grouping**: Nhóm tasks theo priority hoặc status
   ```csharp
   taskListView.ShowGroups = true;
   taskListView.GroupByColumn = priorityColumn;
   ```

2. **Sorting**: Sắp xếp khi click vào header
   ```csharp
   taskListView.Sortable = true;
   ```

3. **Filtering**: Lọc tasks theo điều kiện
   ```csharp
   taskListView.ModelFilter = new TextMatchFilter(taskListView, searchText);
   ```

4. **Drag & Drop**: Kéo thả để sắp xếp lại
   ```csharp
   taskListView.IsSimpleDragSource = true;
   taskListView.IsSimpleDropSink = true;
   ```

5. **In-place Editing**: Chỉnh sửa trực tiếp trên cell
   ```csharp
   taskColumn.IsEditable = true;
   taskListView.CellEditActivation = CellEditActivateMode.SingleClick;
   ```

6. **Context Menu**: Menu chuột phải
   ```csharp
   taskListView.CellRightClick += (s, e) => {
       // Hiển thị menu context
   };
   ```

## Lưu ý quan trọng

1. **Không dùng ListView chuẩn**: Code mẫu này chỉ dùng AdvancedListView từ FluentListView library
2. **Model-driven**: FluentListView làm việc với model objects, không phải ListViewItems
3. **AspectGetter**: Dùng delegates để lấy dữ liệu từ objects
4. **ImageGetter**: Dùng delegates để xác định icon hiển thị
5. **IsButton**: Set true để cột hiển thị dạng button
6. **ButtonClick event**: Xử lý sự kiện nhấn nút
7. **RefreshObject**: Gọi sau khi thay đổi model để cập nhật UI

## Yêu cầu hệ thống

- .NET 9.0 trở lên (Windows only)
- Windows Forms
- FluentListView library (build từ repo này)

## Tài liệu tham khảo

- [ADVANCED.md](ADVANCED.md) - Tài liệu đầy đủ về FluentListView
- [README.md](README.md) - Hướng dẫn cơ bản
- [SAMPLE_README.md](SAMPLE_README.md) - English documentation

## License

GNU General Public License v3.0

## Tác giả

Dựa trên ObjectListView của Phillip Piper, được maintain bởi Robin Rodricks và contributors.
