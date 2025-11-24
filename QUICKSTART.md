# Quick Start Guide - FluentListView Sample

## 🚀 Fastest Way to Run the Sample

### Prerequisites
- Windows OS
- .NET 9.0 SDK installed
- Visual Studio 2022 or VS Code (optional, but recommended)

### Step 1: Build FluentListView Library

Open a command prompt in the repository root and run:

```bash
cd FluentListView
dotnet build FluentListView.sln -c Release
```

This will create `FluentListView.dll` in:
```
FluentListView/bin/Release/net9.0-windows/FluentListView.dll
```

### Step 2: Create a Test Project

```bash
# Go back to repository root
cd ..

# Create a new WinForms project
dotnet new winforms -n TaskManagerSample
cd TaskManagerSample
```

### Step 3: Add Reference to FluentListView

```bash
dotnet add reference ../FluentListView/bin/Release/net9.0-windows/FluentListView.dll
```

### Step 4: Copy the Sample File

```bash
# On Windows Command Prompt
copy ..\SampleFluentListViewForm.cs .

# On PowerShell
Copy-Item ..\SampleFluentListViewForm.cs .
```

### Step 5: Update Program.cs

Replace the content of `Program.cs` with:

```csharp
using System;
using System.Windows.Forms;
using FluentListViewSample;

namespace TaskManagerSample
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

### Step 6: Run the Application

```bash
dotnet run
```

You should see a window with a task list showing:
- ✅ Task column with icons
- ✅ Priority column with colored indicators
- ✅ Status column with status icons  
- ✅ Action column with clickable buttons

## 🎯 What You'll See

The sample displays a task manager interface with:

| Task | Priority | Status | Action |
|------|----------|--------|--------|
| 🔵 Design new user interface | 🔴 High | 🟠 In Progress | **Complete** |
| 🔴 Fix critical bug in payment | 🔴 Critical | ⚪ Not Started | **Start** |
| 🟣 Team meeting - Sprint planning | 🟡 Medium | ⚪ Not Started | **Start** |
| 🟢 Code review for PR #123 | 🟡 Medium | 🟢 Completed | **Reopen** |
| ... and more ... |

## 🎮 Interactive Features

1. **Click Action Buttons**: Click "Start", "Complete", "Reopen", or "Unfreeze" buttons to change task status
2. **Shift+Click**: Hold Shift and click any button to freeze a task
3. **Dynamic Buttons**: Button text changes based on current task status
4. **Visual Feedback**: MessageBox shows confirmation of each action

## 📝 Key Code Sections to Customize

### Change Icon Images (Line 93-117)
Replace placeholder icons with real images:
```csharp
// Instead of AddPlaceholderIcon()
smallImageList.Images.Add("task_default", Image.FromFile("icons/task.png"));
```

### Modify Button Actions (Line 267-312)
Customize what happens when buttons are clicked:
```csharp
private void TaskListView_ButtonClick(object sender, CellClickEventArgs e)
{
    TaskItem task = (TaskItem)e.Model;
    // Add your custom logic here
    // e.g., save to database, send notifications, etc.
}
```

### Add More Tasks (Line 314-385)
Add your own task data:
```csharp
tasks.Add(new TaskItem
{
    TaskName = "Your task name",
    TaskIcon = "task_default",
    Priority = TaskPriority.High,
    Status = TaskStatus.Not_Started
});
```

### Customize Column Setup (Line 137-227)
Add new columns or modify existing ones:
```csharp
OLVColumn newColumn = new OLVColumn("Column Title", "PropertyName");
newColumn.AspectGetter = delegate (object rowObject)
{
    TaskItem task = (TaskItem)rowObject;
    return task.YourProperty;
};
taskListView.AllColumns.Add(newColumn);
```

## 🔧 Troubleshooting

### Error: "Could not load file or assembly 'FluentListView'"
- Make sure you built FluentListView.dll successfully
- Check that the reference path is correct
- Verify .NET 9.0 SDK is installed

### Error: "The type or namespace name 'Fluent' could not be found"
- Ensure you added the reference to FluentListView.dll
- Try cleaning and rebuilding: `dotnet clean && dotnet build`

### Icons not showing
- This is normal! The sample uses placeholder icons (colored circles)
- To use real icons, replace the `AddPlaceholderIcon()` calls with `Image.FromFile()`

### Buttons not clickable
- Make sure `UseHotItem = true` and `UseHotControls = true` are set
- Verify the `ButtonClick` event is subscribed
- Check that `IsButton = true` on the action column

## 📚 Next Steps

1. **Read the Documentation**:
   - [SAMPLE_README.md](SAMPLE_README.md) - Full English documentation
   - [SAMPLE_README_VI.md](SAMPLE_README_VI.md) - Vietnamese documentation
   - [ADVANCED.md](ADVANCED.md) - Advanced FluentListView features

2. **Explore Advanced Features**:
   - Grouping and sorting
   - Filtering and searching
   - Drag and drop
   - In-place editing
   - Custom cell renderers

3. **Customize for Your Needs**:
   - Add database integration
   - Implement real icon files
   - Add more columns (Due Date, Assignee, Tags, etc.)
   - Create custom button actions
   - Add context menus

## 💡 Tips

- Use `taskListView.RefreshObject(task)` after modifying a task to update the UI
- Use `taskListView.SetObjects(tasks)` to reload all data
- Use `taskListView.SelectedObject` to get the currently selected task
- Set `taskListView.Sortable = true` to enable column header sorting
- Set `taskListView.ShowGroups = true` to enable grouping

## 📞 Support

For more information about FluentListView:
- GitHub: https://github.com/robinrodricks/FluentListView
- NuGet: https://www.nuget.org/packages/FluentListView

## ✨ Example Output

When you run the sample, you'll see a professional-looking task manager with:
- Clean, modern interface
- Alternating row colors for better readability
- Icons for visual clarity
- Interactive buttons for quick actions
- Smooth hot-tracking effects

Enjoy building with FluentListView! 🎉
