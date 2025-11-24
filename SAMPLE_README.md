# FluentListView Sample - Task Manager

This sample demonstrates how to use the FluentListView control to create a task management interface with icons, priorities, statuses, and action buttons.

## Features Demonstrated

The `SampleFluentListViewForm.cs` file demonstrates:

1. **Task Column**: Displays task name with an icon
2. **Priority Column**: Shows priority level (Low, Medium, High, Critical) with colored indicator icons
3. **Status Column**: Displays current status (Not Started, In Progress, Completed, Frozen) with status icons
4. **Action Column**: Contains clickable buttons that change based on task status:
   - "Start" - for tasks not started
   - "Complete" - for tasks in progress
   - "Reopen" - for completed tasks
   - "Unfreeze" - for frozen tasks

## How to Use

### Option 1: Add to an Existing WinForms Project

1. Build the FluentListView project:
   ```bash
   dotnet build FluentListView.sln
   ```

2. Create a new WinForms project or open an existing one:
   ```bash
   dotnet new winforms -n MyTaskManager
   cd MyTaskManager
   ```

3. Add a reference to the built FluentListView.dll:
   ```bash
   dotnet add reference ../FluentListView/bin/Debug/net9.0-windows/FluentListView.dll
   ```

4. Copy `SampleFluentListViewForm.cs` into your project

5. Modify `Program.cs` to launch the sample form:
   ```csharp
   using System;
   using System.Windows.Forms;
   using FluentListViewSample;

   namespace MyTaskManager
   {
       static class Program
       {
           [STAThread]
           static void Main()
           {
               Application.EnableVisualStyles();
               Application.SetCompatibleTextRenderingDefault(false);
               Application.Run(new SampleFluentListViewForm());
           }
       }
   }
   ```

6. Run your project:
   ```bash
   dotnet run
   ```

### Option 2: Run as Standalone

The `SampleFluentListViewForm.cs` file includes a `Main()` method and can be compiled and run directly:

1. Build FluentListView first:
   ```bash
   cd /path/to/FluentListView
   dotnet build FluentListView.sln
   ```

2. Compile the sample:
   ```bash
   csc /target:winexe /r:FluentListView/bin/Debug/net9.0-windows/FluentListView.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll SampleFluentListViewForm.cs
   ```

3. Run the executable:
   ```bash
   ./SampleFluentListViewForm.exe
   ```

## Code Structure

### Main Components

- **SampleFluentListViewForm**: The main form class that hosts the FluentListView
- **TaskItem**: Model class representing a task with properties:
  - `TaskName`: The name of the task
  - `TaskIcon`: The icon identifier for the task
  - `Priority`: Priority level (enum)
  - `Status`: Current status (enum)

### Key FluentListView Features Used

1. **AdvancedListView**: The underlying control that provides advanced features
   ```csharp
   taskListView = new AdvancedListView();
   ```

2. **AspectGetter**: Delegates that extract data from model objects
   ```csharp
   taskColumn.AspectGetter = delegate (object rowObject)
   {
       TaskItem task = (TaskItem)rowObject;
       return task.TaskName;
   };
   ```

3. **ImageGetter**: Delegates that determine which icon to display
   ```csharp
   taskColumn.ImageGetter = delegate (object rowObject)
   {
       TaskItem task = (TaskItem)rowObject;
       return task.TaskIcon;
   };
   ```

4. **Button Columns**: Columns that render as clickable buttons
   ```csharp
   actionColumn.IsButton = true;
   actionColumn.ButtonSizing = OLVColumn.ButtonSizingMode.TextBounds;
   ```

5. **Button Click Event**: Handling button clicks
   ```csharp
   taskListView.ButtonClick += TaskListView_ButtonClick;
   ```

## Customization

### Adding Custom Icons

Replace the placeholder icons with real icons by modifying the `InitializeImageLists()` method:

```csharp
// Instead of AddPlaceholderIcon(), use:
smallImageList.Images.Add("task_default", Image.FromFile("icons/task.png"));
smallImageList.Images.Add("priority_high", Image.FromFile("icons/priority_high.png"));
```

### Changing Button Actions

Modify the `TaskListView_ButtonClick` event handler to implement your custom logic:

```csharp
private void TaskListView_ButtonClick(object sender, CellClickEventArgs e)
{
    TaskItem task = (TaskItem)e.Model;
    // Your custom logic here
}
```

### Adding More Columns

Add additional columns in the `SetupColumns()` method:

```csharp
OLVColumn dueeDateColumn = new OLVColumn("Due Date", "DueDate");
dueDateColumn.AspectGetter = delegate (object rowObject)
{
    TaskItem task = (TaskItem)rowObject;
    return task.DueDate.ToShortDateString();
};
taskListView.AllColumns.Add(dueDateColumn);
```

### Styling Options

Customize the appearance in `InitializeTaskListView()`:

```csharp
// Alternate row colors
taskListView.UseAlternatingBackColors = true;
taskListView.AlternateRowBackColor = Color.WhiteSmoke;

// Grid lines
taskListView.GridLines = true;

// Row height
taskListView.RowHeight = 48;

// Hot tracking
taskListView.UseHotItem = true;
taskListView.HotItemStyle = new HotItemStyle();
taskListView.HotItemStyle.BackColor = Color.LightBlue;
```

## Advanced Features

For more advanced FluentListView features, see:
- [ADVANCED.md](ADVANCED.md) - Complete FluentListView documentation
- [README.md](README.md) - Basic FluentListView usage

### Additional Features You Can Add

- Grouping tasks by priority or status
- Sorting by clicking column headers
- Filtering tasks
- Drag and drop reordering
- In-place editing
- Custom cell renderers
- Progress bars in cells
- Checkboxes for selection

## Requirements

- .NET 9.0 or later (Windows only)
- Windows Forms
- FluentListView library (built from this repository)

## License

This sample code is provided under the same GNU General Public License v3.0 as the FluentListView project.

## Credits

FluentListView is based on ObjectListView by Phillip Piper and maintained by Robin Rodricks and contributors.
