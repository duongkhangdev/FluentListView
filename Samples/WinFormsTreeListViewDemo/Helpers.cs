using System;

namespace WinFormsTreeListViewDemo
{
    /// <summary>
    /// Helper utilities for formatting data
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Format bytes to MB string
        /// </summary>
        public static string FormatSize(long bytes)
        {
            double mb = bytes / (1024.0 * 1024.0);
            return $"{mb:F2} MB";
        }

        /// <summary>
        /// Format DateTime to display format (dd-MMM-yyyy HH:mm)
        /// </summary>
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd-MMM-yyyy HH:mm");
        }
    }
}
