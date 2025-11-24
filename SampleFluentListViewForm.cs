/*
 * SampleFluentListViewForm - Sample form demonstrating FluentListView usage
 *
 * This sample demonstrates how to create a task list using FluentListView with:
 * - Task column (with icon and name)
 * - Priority column (with icon indicators)
 * - Status column (with status icons)
 * - Action column (with clickable buttons)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Fluent;
using Fluent.Lists;

namespace FluentListViewSample
{
    /// <summary>
    /// Sample form demonstrating FluentListView with tasks, priorities, statuses, and action buttons
    /// </summary>
    public partial class SampleFluentListViewForm : Form
    {
        private AdvancedListView taskListView;
        private List<TaskItem> tasks;
        private ImageList smallImageList;
        private ImageList largeImageList;

        public SampleFluentListViewForm()
        {
            InitializeComponent();
            InitializeTaskListView();
            LoadSampleData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form
            this.ClientSize = new Size(900, 600);
            this.Name = "SampleFluentListViewForm";
            this.Text = "FluentListView Sample - Task Manager";
            this.StartPosition = FormStartPosition.CenterScreen;
            
            this.ResumeLayout(false);
        }

        private void InitializeTaskListView()
        {
            // Create the AdvancedListView
            taskListView = new AdvancedListView();
            taskListView.Dock = DockStyle.Fill;
            taskListView.View = View.Details;
            taskListView.FullRowSelect = true;
            taskListView.GridLines = true;
            taskListView.UseAlternatingBackColors = true;
            taskListView.RowHeight = 48;
            taskListView.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            
            // Enable hot item tracking for buttons
            taskListView.UseHotItem = true;
            taskListView.UseHotControls = true;

            // Create ImageLists
            InitializeImageLists();
            taskListView.SmallImageList = smallImageList;
            taskListView.LargeImageList = largeImageList;

            // Configure columns
            SetupColumns();

            // Handle button clicks
            taskListView.ButtonClick += TaskListView_ButtonClick;
            
            // Add to form
            this.Controls.Add(taskListView);
        }

        private void InitializeImageLists()
        {
            smallImageList = new ImageList();
            smallImageList.ImageSize = new Size(16, 16);
            smallImageList.ColorDepth = ColorDepth.Depth32Bit;

            largeImageList = new ImageList();
            largeImageList.ImageSize = new Size(32, 32);
            largeImageList.ColorDepth = ColorDepth.Depth32Bit;

            // Add task type icons (using colored rectangles as placeholders)
            // In a real application, you would load actual icon files
            AddPlaceholderIcon(smallImageList, "task_default", Color.DodgerBlue);
            AddPlaceholderIcon(smallImageList, "task_important", Color.OrangeRed);
            AddPlaceholderIcon(smallImageList, "task_meeting", Color.Purple);
            AddPlaceholderIcon(smallImageList, "task_code", Color.Green);

            AddPlaceholderIcon(largeImageList, "task_default", Color.DodgerBlue);
            AddPlaceholderIcon(largeImageList, "task_important", Color.OrangeRed);
            AddPlaceholderIcon(largeImageList, "task_meeting", Color.Purple);
            AddPlaceholderIcon(largeImageList, "task_code", Color.Green);

            // Add priority icons (light bulb representations)
            AddPlaceholderIcon(smallImageList, "priority_low", Color.Gray);
            AddPlaceholderIcon(smallImageList, "priority_medium", Color.Gold);
            AddPlaceholderIcon(smallImageList, "priority_high", Color.Red);
            AddPlaceholderIcon(smallImageList, "priority_critical", Color.DarkRed);

            // Add status icons
            AddPlaceholderIcon(smallImageList, "status_notstarted", Color.LightGray);
            AddPlaceholderIcon(smallImageList, "status_inprogress", Color.Orange);
            AddPlaceholderIcon(smallImageList, "status_completed", Color.Green);
            AddPlaceholderIcon(smallImageList, "status_frozen", Color.LightBlue);
        }

        /// <summary>
        /// Helper method to create placeholder icons (colored circles)
        /// In a real application, you would load actual icon files using:
        /// imageList.Images.Add(key, Image.FromFile(path));
        /// </summary>
        private void AddPlaceholderIcon(ImageList imageList, string key, Color color)
        {
            Bitmap bmp = new Bitmap(imageList.ImageSize.Width, imageList.ImageSize.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                using (SolidBrush brush = new SolidBrush(color))
                {
                    int margin = 2;
                    g.FillEllipse(brush, margin, margin, 
                        imageList.ImageSize.Width - margin * 2, 
                        imageList.ImageSize.Height - margin * 2);
                }
            }
            imageList.Images.Add(key, bmp);
        }

        private void SetupColumns()
        {
            // Column 1: Task (with icon and name)
            OLVColumn taskColumn = new OLVColumn("Task", "TaskName");
            taskColumn.Width = 250;
            taskColumn.AspectGetter = delegate (object rowObject)
            {
                TaskItem task = (TaskItem)rowObject;
                return task.TaskName;
            };
            taskColumn.ImageGetter = delegate (object rowObject)
            {
                TaskItem task = (TaskItem)rowObject;
                return task.TaskIcon;
            };
            taskListView.AllColumns.Add(taskColumn);

            // Column 2: Priority (with icon)
            OLVColumn priorityColumn = new OLVColumn("Priority", "Priority");
            priorityColumn.Width = 150;
            priorityColumn.TextAlign = HorizontalAlignment.Center;
            priorityColumn.AspectGetter = delegate (object rowObject)
            {
                TaskItem task = (TaskItem)rowObject;
                return task.Priority.ToString();
            };
            priorityColumn.ImageGetter = delegate (object rowObject)
            {
                TaskItem task = (TaskItem)rowObject;
                return GetPriorityIcon(task.Priority);
            };
            taskListView.AllColumns.Add(priorityColumn);

            // Column 3: Status (with icon)
            OLVColumn statusColumn = new OLVColumn("Status", "Status");
            statusColumn.Width = 150;
            statusColumn.TextAlign = HorizontalAlignment.Center;
            statusColumn.AspectGetter = delegate (object rowObject)
            {
                TaskItem task = (TaskItem)rowObject;
                return task.Status.ToString().Replace("_", " ");
            };
            statusColumn.ImageGetter = delegate (object rowObject)
            {
                TaskItem task = (TaskItem)rowObject;
                return GetStatusIcon(task.Status);
            };
            taskListView.AllColumns.Add(statusColumn);

            // Column 4: Action (with buttons)
            OLVColumn actionColumn = new OLVColumn("Action", "ActionText");
            actionColumn.Width = 200;
            actionColumn.TextAlign = HorizontalAlignment.Center;
            actionColumn.IsButton = true;
            actionColumn.AspectGetter = delegate (object rowObject)
            {
                TaskItem task = (TaskItem)rowObject;
                // Return the button text based on current status
                return GetActionButtonText(task);
            };
            // Configure button sizing
            actionColumn.ButtonSizing = OLVColumn.ButtonSizingMode.TextBounds;
            actionColumn.ButtonPadding = new Size(15, 5);
            actionColumn.ButtonMaxWidth = 150;
            
            taskListView.AllColumns.Add(actionColumn);

            // Set visible columns
            taskListView.Columns.AddRange(new ColumnHeader[] { 
                taskColumn, 
                priorityColumn, 
                statusColumn, 
                actionColumn 
            });
        }

        private string GetPriorityIcon(TaskPriority priority)
        {
            switch (priority)
            {
                case TaskPriority.Low:
                    return "priority_low";
                case TaskPriority.Medium:
                    return "priority_medium";
                case TaskPriority.High:
                    return "priority_high";
                case TaskPriority.Critical:
                    return "priority_critical";
                default:
                    return "priority_low";
            }
        }

        private string GetStatusIcon(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Not_Started:
                    return "status_notstarted";
                case TaskStatus.In_Progress:
                    return "status_inprogress";
                case TaskStatus.Completed:
                    return "status_completed";
                case TaskStatus.Frozen:
                    return "status_frozen";
                default:
                    return "status_notstarted";
            }
        }

        private string GetActionButtonText(TaskItem task)
        {
            switch (task.Status)
            {
                case TaskStatus.Not_Started:
                    return "Start";
                case TaskStatus.In_Progress:
                    return "Complete";
                case TaskStatus.Completed:
                    return "Reopen";
                case TaskStatus.Frozen:
                    return "Unfreeze";
                default:
                    return "Start";
            }
        }

        private void TaskListView_ButtonClick(object sender, CellClickEventArgs e)
        {
            TaskItem task = (TaskItem)e.Model;
            if (task == null) return;

            string buttonText = (string)e.SubItem.Text;

            // Handle different button actions
            switch (buttonText)
            {
                case "Start":
                    task.Status = TaskStatus.In_Progress;
                    MessageBox.Show($"Task '{task.TaskName}' started!", "Action", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Complete":
                    task.Status = TaskStatus.Completed;
                    MessageBox.Show($"Task '{task.TaskName}' completed!", "Action", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Reopen":
                    task.Status = TaskStatus.In_Progress;
                    MessageBox.Show($"Task '{task.TaskName}' reopened!", "Action", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Unfreeze":
                    task.Status = TaskStatus.Not_Started;
                    MessageBox.Show($"Task '{task.TaskName}' unfrozen!", "Action", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            // Add context menu option to freeze tasks
            if (e.ClickCount == 1 && Control.ModifierKeys == Keys.Shift)
            {
                task.Status = TaskStatus.Frozen;
                MessageBox.Show($"Task '{task.TaskName}' frozen! (Shift+Click)", "Action", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Refresh the object to update the display
            taskListView.RefreshObject(task);
        }

        private void LoadSampleData()
        {
            tasks = new List<TaskItem>
            {
                new TaskItem
                {
                    TaskName = "Design new user interface",
                    TaskIcon = "task_default",
                    Priority = TaskPriority.High,
                    Status = TaskStatus.In_Progress
                },
                new TaskItem
                {
                    TaskName = "Fix critical bug in payment module",
                    TaskIcon = "task_important",
                    Priority = TaskPriority.Critical,
                    Status = TaskStatus.Not_Started
                },
                new TaskItem
                {
                    TaskName = "Team meeting - Sprint planning",
                    TaskIcon = "task_meeting",
                    Priority = TaskPriority.Medium,
                    Status = TaskStatus.Not_Started
                },
                new TaskItem
                {
                    TaskName = "Code review for PR #123",
                    TaskIcon = "task_code",
                    Priority = TaskPriority.Medium,
                    Status = TaskStatus.Completed
                },
                new TaskItem
                {
                    TaskName = "Update documentation",
                    TaskIcon = "task_default",
                    Priority = TaskPriority.Low,
                    Status = TaskStatus.Not_Started
                },
                new TaskItem
                {
                    TaskName = "Refactor database layer",
                    TaskIcon = "task_code",
                    Priority = TaskPriority.High,
                    Status = TaskStatus.Frozen
                },
                new TaskItem
                {
                    TaskName = "Implement user authentication",
                    TaskIcon = "task_code",
                    Priority = TaskPriority.Critical,
                    Status = TaskStatus.In_Progress
                },
                new TaskItem
                {
                    TaskName = "Write unit tests",
                    TaskIcon = "task_code",
                    Priority = TaskPriority.Medium,
                    Status = TaskStatus.Not_Started
                },
                new TaskItem
                {
                    TaskName = "Client presentation",
                    TaskIcon = "task_meeting",
                    Priority = TaskPriority.High,
                    Status = TaskStatus.Not_Started
                },
                new TaskItem
                {
                    TaskName = "Performance optimization",
                    TaskIcon = "task_important",
                    Priority = TaskPriority.Medium,
                    Status = TaskStatus.Completed
                }
            };

            // Set the objects to display
            taskListView.SetObjects(tasks);
        }
    }

    #region Model Classes

    /// <summary>
    /// Task item model class
    /// </summary>
    public class TaskItem
    {
        public string TaskName { get; set; }
        public string TaskIcon { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
    }

    /// <summary>
    /// Task priority levels
    /// </summary>
    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    /// <summary>
    /// Task status values
    /// </summary>
    public enum TaskStatus
    {
        Not_Started,
        In_Progress,
        Completed,
        Frozen
    }

    #endregion

    #region Program Entry Point

    /// <summary>
    /// Sample program entry point
    /// To run this sample:
    /// 1. Build the FluentListView project
    /// 2. Create a new WinForms project
    /// 3. Add a reference to FluentListView.dll
    /// 4. Add this file to your project
    /// 5. Set this as the startup form or run: Application.Run(new SampleFluentListViewForm());
    /// </summary>
    public static class SampleProgram
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SampleFluentListViewForm());
        }
    }

    #endregion
}
