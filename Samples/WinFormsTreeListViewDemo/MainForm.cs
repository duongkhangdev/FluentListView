using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fluent;
using Fluent.Lists;
using WinFormsTreeListViewDemo.Models;

namespace WinFormsTreeListViewDemo
{
    public partial class MainForm : Form
    {
        private TreeListView treeListView = null!;
        private ImageList imageList = null!;
        private StarRatingRenderer starRatingRenderer = null!;
        private OLVColumn titleColumn = null!;
        private OLVColumn sizeColumn = null!;
        private OLVColumn lastPlayedColumn = null!;
        private OLVColumn ratingColumn = null!;
        private List<Artist> artists = null!;
        private bool isLoadingTracks = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeTreeListView();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // MainForm
            this.ClientSize = new Size(900, 600);
            this.Name = "MainForm";
            this.Text = "TreeListView Demo - Music Library";
            this.StartPosition = FormStartPosition.CenterScreen;
            
            this.ResumeLayout(false);
        }

        private void InitializeTreeListView()
        {
            // Create and configure TreeListView
            treeListView = new TreeListView();
            treeListView.Dock = DockStyle.Fill;
            treeListView.View = View.Details;
            treeListView.FullRowSelect = true;
            treeListView.HideSelection = false;
            treeListView.UseExplorerTheme = true;
            treeListView.ShowGroups = false;
            
            // Setup ImageList with icons for different node types
            imageList = new ImageList();
            imageList.ImageSize = new Size(16, 16);
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            
            // Create simple colored icons for demonstration
            imageList.Images.Add("artist", CreateIcon(Color.DodgerBlue));   // User/Artist icon
            imageList.Images.Add("album", CreateIcon(Color.MediumPurple));  // CD/Album icon
            imageList.Images.Add("track", CreateIcon(Color.LimeGreen));     // Music note/Track icon
            
            treeListView.SmallImageList = imageList;
            
            // Create columns
            titleColumn = new OLVColumn("Title", "Title");
            titleColumn.Width = 350;
            titleColumn.AspectGetter = delegate(object x) {
                if (x is ITreeNode node) return node.Title;
                return "";
            };
            titleColumn.ImageGetter = delegate(object x) {
                if (x is ITreeNode node)
                {
                    return node.NodeType switch
                    {
                        NodeType.Artist => "artist",
                        NodeType.Album => "album",
                        NodeType.Track => "track",
                        _ => null
                    };
                }
                return null;
            };

            sizeColumn = new OLVColumn("Size", "Size");
            sizeColumn.Width = 120;
            sizeColumn.TextAlign = HorizontalAlignment.Right;
            sizeColumn.AspectGetter = delegate(object x) {
                if (x is Track track && track.SizeBytes.HasValue)
                    return Helpers.FormatSize(track.SizeBytes.Value);
                return "";
            };

            lastPlayedColumn = new OLVColumn("Last Played", "LastPlayed");
            lastPlayedColumn.Width = 180;
            lastPlayedColumn.AspectGetter = delegate(object x) {
                if (x is Track track && track.LastPlayed.HasValue)
                    return Helpers.FormatDateTime(track.LastPlayed.Value);
                return "";
            };

            ratingColumn = new OLVColumn("Rating", "Rating");
            ratingColumn.Width = 150;
            ratingColumn.AspectGetter = delegate(object x) {
                if (x is ITreeNode node) return node.Rating;
                return 0;
            };
            
            // Set up StarRatingRenderer
            starRatingRenderer = new StarRatingRenderer();
            ratingColumn.Renderer = starRatingRenderer;

            treeListView.AllColumns.Add(titleColumn);
            treeListView.AllColumns.Add(sizeColumn);
            treeListView.AllColumns.Add(lastPlayedColumn);
            treeListView.AllColumns.Add(ratingColumn);
            
            treeListView.Columns.AddRange(new OLVColumn[] {
                titleColumn, sizeColumn, lastPlayedColumn, ratingColumn
            });

            // Configure tree delegates
            treeListView.CanExpandGetter = delegate(object x) {
                if (x is ITreeNode node) return node.CanExpand;
                return false;
            };

            treeListView.ChildrenGetter = delegate(object x) {
                if (x is ITreeNode node && node.Children != null)
                {
                    // Lazy load tracks when expanding an album
                    if (x is Album album && !isLoadingTracks)
                    {
                        var tracks = node.Children.OfType<Track>().ToList();
                        if (tracks.Count == 0)
                        {
                            // Trigger async loading
                            LoadTracksForAlbum(album);
                        }
                    }
                    return node.Children;
                }
                return null;
            };

            // Enable sorting
            treeListView.ColumnClick += TreeListView_ColumnClick;
            
            // Enable double-click handler
            treeListView.CellClick += TreeListView_CellClick;
            treeListView.DoubleClick += TreeListView_DoubleClick;
            
            // Mouse events for star rating
            treeListView.MouseMove += TreeListView_MouseMove;
            treeListView.MouseLeave += TreeListView_MouseLeave;

            this.Controls.Add(treeListView);
        }

        private Bitmap CreateIcon(Color color)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (SolidBrush brush = new SolidBrush(color))
                {
                    g.FillEllipse(brush, 2, 2, 12, 12);
                }
                using (Pen pen = new Pen(Color.FromArgb(color.A, 
                    Math.Max(0, color.R - 50), 
                    Math.Max(0, color.G - 50), 
                    Math.Max(0, color.B - 50)), 2))
                {
                    g.DrawEllipse(pen, 2, 2, 12, 12);
                }
            }
            return bmp;
        }

        private void LoadData()
        {
            artists = new List<Artist>();

            // Create sample artists and albums
            var artist1 = new Artist("The Beatles");
            artist1.Children?.Add(new Album("Abbey Road"));
            artist1.Children?.Add(new Album("Let It Be"));
            artist1.Children?.Add(new Album("Revolver"));

            var artist2 = new Artist("Pink Floyd");
            artist2.Children?.Add(new Album("The Dark Side of the Moon"));
            artist2.Children?.Add(new Album("The Wall"));
            artist2.Children?.Add(new Album("Wish You Were Here"));

            var artist3 = new Artist("Queen");
            artist3.Children?.Add(new Album("A Night at the Opera"));
            artist3.Children?.Add(new Album("News of the World"));
            artist3.Children?.Add(new Album("The Game"));

            var artist4 = new Artist("Led Zeppelin");
            artist4.Children?.Add(new Album("Led Zeppelin IV"));
            artist4.Children?.Add(new Album("Physical Graffiti"));
            artist4.Children?.Add(new Album("Houses of the Holy"));

            artists.Add(artist1);
            artists.Add(artist2);
            artists.Add(artist3);
            artists.Add(artist4);

            // Set roots to display artists
            treeListView.Roots = artists;
        }

        private async void LoadTracksForAlbum(Album album)
        {
            if (isLoadingTracks) return;
            
            isLoadingTracks = true;
            
            // Simulate async loading with delay
            await Task.Delay(300);
            
            // Generate sample tracks based on album name
            var tracks = GenerateTracksForAlbum(album.Title);
            album.Children?.Clear();
            album.Children?.AddRange(tracks);
            
            isLoadingTracks = false;
            
            // Refresh the album to show the loaded tracks
            treeListView.RefreshObject(album);
        }

        private List<ITreeNode> GenerateTracksForAlbum(string albumName)
        {
            var tracks = new List<ITreeNode>();
            Random rand = new Random(albumName.GetHashCode()); // Consistent random based on album name
            
            int trackCount = rand.Next(8, 15);
            for (int i = 1; i <= trackCount; i++)
            {
                string trackName = $"Track {i:D2}";
                long sizeBytes = (long)(rand.NextDouble() * 8 * 1024 * 1024 + 2 * 1024 * 1024); // 2-10 MB
                DateTime lastPlayed = DateTime.Now.AddDays(-rand.Next(1, 365));
                int rating = rand.Next(0, 6); // 0-5 stars
                
                tracks.Add(new Track(trackName, sizeBytes, lastPlayed, rating));
            }
            
            return tracks;
        }

        private void TreeListView_ColumnClick(object? sender, ColumnClickEventArgs e)
        {
            var column = treeListView.GetColumn(e.Column);
            if (column == null) return;

            // Toggle sort order
            if (treeListView.LastSortColumn == column)
            {
                treeListView.LastSortOrder = treeListView.LastSortOrder == SortOrder.Ascending 
                    ? SortOrder.Descending 
                    : SortOrder.Ascending;
            }
            else
            {
                treeListView.LastSortColumn = column;
                treeListView.LastSortOrder = SortOrder.Ascending;
            }

            // Sort while maintaining hierarchy
            SortTreeHierarchically(column, treeListView.LastSortOrder);
        }

        private void SortTreeHierarchically(OLVColumn column, SortOrder order)
        {
            if (artists == null) return;

            int sortMultiplier = (order == SortOrder.Ascending) ? 1 : -1;

            // Sort artists
            artists.Sort((a, b) => CompareNodes(a, b, column, sortMultiplier));

            // Sort albums within each artist
            foreach (var artist in artists)
            {
                if (artist.Children != null)
                {
                    artist.Children.Sort((a, b) => CompareNodes(a, b, column, sortMultiplier));
                    
                    // Sort tracks within each album
                    foreach (var child in artist.Children.OfType<Album>())
                    {
                        if (child.Children != null)
                        {
                            child.Children.Sort((a, b) => CompareNodes(a, b, column, sortMultiplier));
                        }
                    }
                }
            }

            // Refresh the view
            treeListView.Roots = artists;
            treeListView.RebuildAll(true);
        }

        private int CompareNodes(ITreeNode a, ITreeNode b, OLVColumn column, int sortMultiplier)
        {
            object aValue = column.GetValue(a);
            object bValue = column.GetValue(b);

            if (aValue == null && bValue == null) return 0;
            if (aValue == null) return -1 * sortMultiplier;
            if (bValue == null) return 1 * sortMultiplier;

            if (aValue is IComparable comparableA)
            {
                return comparableA.CompareTo(bValue) * sortMultiplier;
            }

            return string.Compare(aValue.ToString(), bValue.ToString(), StringComparison.Ordinal) * sortMultiplier;
        }

        private void TreeListView_CellClick(object? sender, CellClickEventArgs e)
        {
            // Handle rating cell clicks
            if (e.Column == ratingColumn && e.Model is Track track) // Rating column
            {
                var subItem = e.SubItem;
                if (subItem != null)
                {
                    Rectangle bounds = e.Item.GetSubItemBounds(e.ColumnIndex);
                    int? newRating = starRatingRenderer.HandleMouseClick(e.Location, bounds);
                    if (newRating.HasValue)
                    {
                        track.Rating = newRating.Value;
                        treeListView.RefreshObject(track);
                    }
                }
            }
        }

        private void TreeListView_DoubleClick(object? sender, EventArgs e)
        {
            var selectedObject = treeListView.SelectedObject;
            if (selectedObject is Track track)
            {
                MessageBox.Show($"Playing: {track.Title}", "Now Playing", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TreeListView_MouseMove(object? sender, MouseEventArgs e)
        {
            // Update star rating hover
            var hitTest = treeListView.OlvHitTest(e.X, e.Y);
            if (hitTest.Item != null && hitTest.SubItem != null && hitTest.Column == ratingColumn)
            {
                Rectangle bounds = hitTest.Item.GetSubItemBounds(hitTest.ColumnIndex);
                starRatingRenderer.HandleMouseMove(hitTest.Item, hitTest.SubItem, e.Location, bounds);
                treeListView.Invalidate(bounds);
            }
            else
            {
                starRatingRenderer.ResetHover();
            }
        }

        private void TreeListView_MouseLeave(object? sender, EventArgs e)
        {
            starRatingRenderer.ResetHover();
            treeListView.Invalidate();
        }
    }
}
