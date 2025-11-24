using System;
using System.Collections.Generic;

namespace WinFormsTreeListViewDemo.Models
{
    /// <summary>
    /// Represents the type of node in the tree hierarchy
    /// </summary>
    public enum NodeType
    {
        Artist,
        Album,
        Track
    }

    /// <summary>
    /// Interface for tree node items supporting hierarchical structure
    /// </summary>
    public interface ITreeNode
    {
        /// <summary>
        /// Gets the title/name of the node
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the size in bytes (only applicable to Tracks)
        /// </summary>
        long? SizeBytes { get; }

        /// <summary>
        /// Gets the last played date time (only applicable to Tracks)
        /// </summary>
        DateTime? LastPlayed { get; }

        /// <summary>
        /// Gets or sets the rating (0-5, only applicable to Tracks)
        /// </summary>
        int Rating { get; set; }

        /// <summary>
        /// Gets the type of node
        /// </summary>
        NodeType NodeType { get; }

        /// <summary>
        /// Gets the children of this node
        /// </summary>
        List<ITreeNode>? Children { get; }

        /// <summary>
        /// Determines if this node can be expanded
        /// </summary>
        bool CanExpand { get; }
    }

    /// <summary>
    /// Represents an artist with albums
    /// </summary>
    public class Artist : ITreeNode
    {
        public string Title { get; set; } = string.Empty;
        public long? SizeBytes => null;
        public DateTime? LastPlayed => null;
        public int Rating { get; set; } = 0;
        public NodeType NodeType => NodeType.Artist;
        public List<ITreeNode>? Children { get; set; }
        public bool CanExpand => Children != null && Children.Count > 0;

        public Artist(string name)
        {
            Title = name;
            Children = new List<ITreeNode>();
        }
    }

    /// <summary>
    /// Represents an album with tracks
    /// </summary>
    public class Album : ITreeNode
    {
        public string Title { get; set; } = string.Empty;
        public long? SizeBytes => null;
        public DateTime? LastPlayed => null;
        public int Rating { get; set; } = 0;
        public NodeType NodeType => NodeType.Album;
        public List<ITreeNode>? Children { get; set; }
        public bool CanExpand => Children != null && Children.Count > 0;

        public Album(string name)
        {
            Title = name;
            Children = new List<ITreeNode>();
        }
    }

    /// <summary>
    /// Represents a track with size, last played and rating
    /// </summary>
    public class Track : ITreeNode
    {
        public string Title { get; set; } = string.Empty;
        public long? SizeBytes { get; set; }
        public DateTime? LastPlayed { get; set; }
        public int Rating { get; set; }
        public NodeType NodeType => NodeType.Track;
        public List<ITreeNode>? Children => null;
        public bool CanExpand => false;

        public Track(string name, long sizeBytes, DateTime lastPlayed, int rating = 0)
        {
            Title = name;
            SizeBytes = sizeBytes;
            LastPlayed = lastPlayed;
            Rating = rating;
        }
    }
}
