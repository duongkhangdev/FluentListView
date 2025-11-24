# WinForms TreeListView Demo

## Purpose

This sample demonstrates how to use the `TreeListView` component from FluentListView in a WinForms application. It showcases a complete implementation of a hierarchical music library browser with the following features:

- **3-Level Hierarchy**: Artist → Album → Track
- **Custom Rendering**: Star rating column with interactive editing
- **Lazy Loading**: Tracks are loaded asynchronously when expanding albums (simulated 300ms delay)
- **Sorting**: Click column headers to sort while maintaining hierarchy
- **Icons**: Different icons for each node type (Artist, Album, Track)
- **Interactive Features**: Double-click to "play" tracks, click to edit ratings

## Screenshots

Below are the reference screenshots showing the expected appearance and functionality:

![Reference Screenshot 1](../../_images/tree-demo-1.png)
![Reference Screenshot 2](../../_images/tree-demo-2.png)

*Note: The actual implementation may vary slightly from the reference images, but maintains the same core functionality.*

## Features

### Hierarchical Data Display
The TreeListView displays music library data in a three-level hierarchy:
- **Artists** (top level) - displayed with a blue icon
- **Albums** (second level) - displayed with a purple icon  
- **Tracks** (third level) - displayed with a green icon

### Column Layout
1. **Title** - Shows the name with an appropriate icon for each node type
2. **Size** - Displays file size in MB format (tracks only), right-aligned
3. **Last Played** - Shows the last played date/time in "dd-MMM-yyyy HH:mm" format (tracks only)
4. **Rating** - Interactive 0-5 star rating display (tracks only)

### Lazy Loading
When you first load the application, only Artists and Albums are loaded. Tracks are loaded asynchronously when you expand an Album node:
- 300ms simulated delay to demonstrate async loading
- Smooth user experience without blocking the UI thread

### Inline Rating Editor
The Rating column features a custom `StarRatingRenderer`:
- **Visual Display**: Shows 0-5 gold stars for each track's rating
- **Hover Effect**: Stars highlight as you move the mouse over them
- **Click to Edit**: Click on a star to set the track's rating
- **Custom Rendering**: Uses owner-draw techniques to render star shapes

### Sorting
Click any column header to sort the data:
- **Maintains Hierarchy**: Sorting is applied within each level (Artists, Albums, Tracks)
- **Toggle Order**: Click the same header again to reverse sort order
- **Works on All Columns**: Title, Size, Last Played, and Rating

### Double-Click Interaction
Double-click any track to see a "Now Playing" message box displaying the track title.

## Running the Sample

### Prerequisites
- .NET 9.0 SDK or later
- Windows operating system

### Build and Run

1. **Using Visual Studio**:
   - Open `FluentListView.sln`
   - Set `WinFormsTreeListViewDemo` as the startup project
   - Press F5 to build and run

2. **Using Command Line**:
   ```bash
   cd Samples/WinFormsTreeListViewDemo
   dotnet run
   ```

3. **From Solution Root**:
   ```bash
   dotnet build FluentListView.sln /p:EnableWindowsTargeting=true
   dotnet run --project Samples/WinFormsTreeListViewDemo/WinFormsTreeListViewDemo.csproj
   ```

## Code Structure

### Models (`Models.cs`)
- **ITreeNode**: Interface defining the contract for hierarchical tree nodes
  - Properties: Title, SizeBytes, LastPlayed, Rating, NodeType, Children, CanExpand
- **Artist**: Represents a music artist with albums
- **Album**: Represents an album with tracks
- **Track**: Represents a music track with size, last played date, and rating

### Helpers (`Helpers.cs`)
- **FormatSize**: Converts bytes to MB display format
- **FormatDateTime**: Formats DateTime to "dd-MMM-yyyy HH:mm" format

### StarRatingRenderer (`StarRatingRenderer.cs`)
Custom cell renderer for the Rating column:
- Inherits from `BaseRenderer`
- Draws 0-5 star shapes using polygon graphics
- Handles mouse move events for hover highlighting
- Processes mouse clicks to update ratings
- Uses gold/light-gray color scheme for visual appeal

### MainForm (`MainForm.cs`)
Main application form containing:
- TreeListView configuration and initialization
- Column setup with custom renderers
- Tree delegates (CanExpandGetter, ChildrenGetter)
- Event handlers for sorting, clicking, and mouse interaction
- Sample data generation
- Lazy loading implementation

## Extending the Sample

### Connecting to Real Data
To use this with actual music files:
1. Replace `LoadData()` to scan a music directory
2. Use libraries like TagLib# to read ID3 tags
3. Update `GenerateTracksForAlbum()` to create Track objects from file metadata
4. Implement actual playback in `TreeListView_DoubleClick`

### Performance Optimization
For large libraries:
1. Enable `VirtualMode` on TreeListView
2. Implement on-demand loading for albums
3. Use background tasks for scanning directories
4. Cache loaded data to reduce I/O operations

### Additional Features
Consider adding:
- **Search/Filter**: Use TreeListView filtering capabilities
- **Context Menus**: Right-click actions for nodes
- **Drag & Drop**: Reorder tracks or create playlists
- **Persistence**: Save ratings and play counts to database
- **Theming**: Support dark mode and custom color schemes
- **Album Art**: Display cover images in a separate column or tooltip

## Notes

- This is a demonstration sample showing FluentListView's TreeListView capabilities
- The star rating renderer uses custom owner-draw painting
- Lazy loading is simulated with Task.Delay for demonstration purposes
- Icons are simple colored circles; replace with actual icons for production use
- The sample uses .NET 9.0 to match the FluentListView library target framework

## License

This sample follows the same license as the FluentListView library (GNU General Public License v3.0).

## References

- [FluentListView Repository](https://github.com/robinrodricks/FluentListView)
- [ObjectListView Documentation](http://objectlistview.sourceforge.net/) - Original inspiration for FluentListView
