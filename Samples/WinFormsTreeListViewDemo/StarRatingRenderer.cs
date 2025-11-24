using System;
using System.Drawing;
using System.Windows.Forms;
using Fluent;

namespace WinFormsTreeListViewDemo
{
    /// <summary>
    /// Custom renderer for displaying and editing star ratings (0-5 stars)
    /// Supports mouse hover and click interactions
    /// </summary>
    public class StarRatingRenderer : BaseRenderer
    {
        private const int MaxStars = 5;
        private const int StarSize = 16;
        private const int StarSpacing = 2;

        private int hoveredRating = -1;
        private OLVListItem? hoveredItem = null;
        private OLVListSubItem? hoveredSubItem = null;

        public override void Render(Graphics g, Rectangle r)
        {
            DrawBackground(g, r);

            if (this.Aspect is int rating)
            {
                DrawStars(g, r, rating, GetIsHovering());
            }
        }

        private bool GetIsHovering()
        {
            return hoveredItem == this.ListItem && hoveredSubItem == this.SubItem && hoveredRating >= 0;
        }

        private void DrawStars(Graphics g, Rectangle r, int rating, bool isHovering)
        {
            int displayRating = isHovering ? hoveredRating : rating;
            int startX = r.X + 4;
            int startY = r.Y + (r.Height - StarSize) / 2;

            using (SolidBrush goldBrush = new SolidBrush(Color.Gold))
            using (SolidBrush grayBrush = new SolidBrush(Color.LightGray))
            using (Pen outlinePen = new Pen(Color.DarkGoldenrod))
            {
                for (int i = 0; i < MaxStars; i++)
                {
                    int x = startX + i * (StarSize + StarSpacing);
                    Rectangle starRect = new Rectangle(x, startY, StarSize, StarSize);

                    // Draw filled or empty star
                    Brush fillBrush = (i < displayRating) ? goldBrush : grayBrush;
                    DrawStar(g, starRect, fillBrush, outlinePen);
                }
            }
        }

        private void DrawStar(Graphics g, Rectangle bounds, Brush fillBrush, Pen outlinePen)
        {
            // Simple star shape using polygon
            PointF[] starPoints = GetStarPoints(bounds);
            g.FillPolygon(fillBrush, starPoints);
            g.DrawPolygon(outlinePen, starPoints);
        }

        private PointF[] GetStarPoints(Rectangle bounds)
        {
            float cx = bounds.X + bounds.Width / 2f;
            float cy = bounds.Y + bounds.Height / 2f;
            float outerRadius = bounds.Width / 2f;
            float innerRadius = outerRadius * 0.4f;

            PointF[] points = new PointF[10];
            for (int i = 0; i < 10; i++)
            {
                double angle = Math.PI / 2 + i * Math.PI / 5;
                float radius = (i % 2 == 0) ? outerRadius : innerRadius;
                points[i] = new PointF(
                    cx + (float)(Math.Cos(angle) * radius),
                    cy - (float)(Math.Sin(angle) * radius)
                );
            }
            return points;
        }

        /// <summary>
        /// Handle mouse move for hover effect
        /// </summary>
        public void HandleMouseMove(OLVListItem? item, OLVListSubItem? subItem, Point location, Rectangle bounds)
        {
            hoveredItem = item;
            hoveredSubItem = subItem;

            if (item != null && subItem != null)
            {
                int startX = bounds.X + 4;
                int mouseX = location.X;
                
                if (mouseX >= startX && mouseX < startX + MaxStars * (StarSize + StarSpacing))
                {
                    int starIndex = (mouseX - startX) / (StarSize + StarSpacing);
                    hoveredRating = Math.Min(starIndex + 1, MaxStars);
                }
                else
                {
                    hoveredRating = -1;
                }
            }
            else
            {
                hoveredRating = -1;
            }
        }

        /// <summary>
        /// Handle mouse click to set rating
        /// </summary>
        public int? HandleMouseClick(Point location, Rectangle bounds)
        {
            int startX = bounds.X + 4;
            int mouseX = location.X;

            if (mouseX >= startX && mouseX < startX + MaxStars * (StarSize + StarSpacing))
            {
                int starIndex = (mouseX - startX) / (StarSize + StarSpacing);
                return Math.Min(starIndex + 1, MaxStars);
            }

            return null;
        }

        /// <summary>
        /// Reset hover state
        /// </summary>
        public void ResetHover()
        {
            hoveredItem = null;
            hoveredSubItem = null;
            hoveredRating = -1;
        }
    }
}
