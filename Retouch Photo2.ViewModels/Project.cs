// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★
// Only:              ★★★★★
// Complete:      ★
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Graphics.Imaging;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// Represents a project class with size and layerages.
    /// </summary>
    public class Project
    {
        /// <summary> Gets or sets the width. </summary>
        public int Width { set; get; }

        /// <summary> Gets or sets the height. </summary>
        public int Height { set; get; }

        /// <summary> Gets or sets the layerages. </summary>
        public IEnumerable<Layerage> Layerages;

        //@Construct
        /// <summary>
        /// Initializes a Project. 
        /// </summary>
        public Project() { }
        /// <summary>
        /// Initializes a Project. 
        /// </summary>
        /// <param name="size"> The size. </param>
        public Project(BitmapSize size)
        {
            this.Width = (int)size.Width;
            this.Height = (int)size.Height;
        }

        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned Project. </returns>
        public Project Clone()
        {
            return new Project
            {
                Width = this.Width,
                Height = this.Height,
                Layerages =
                this.Layerages is null ?
                null :
                (
                    from l
                    in this.Layerages
                    select l.Clone()
                )
            };
        }

        /// <summary> Gets or sets the width for preset, Range 2 to 75. </summary>
        public double PresetWidth => this.BitmapSizeConverter().Width;
        /// <summary> Gets or sets the height for preset, Range 2 to 75. </summary>
        public double PresetHeight => this.BitmapSizeConverter().Height;

        /// <summary> Turn to the UI size from bitmap size, Range (2, 2) to (75, 75). </summary>
        private Size BitmapSizeConverter()
        {
            // 65 * 65 = 4225
            if (this.Width == this.Height) return new Size(65, 65);
            if (this.Width <= 0) return new Size(65, 65);
            if (this.Height <= 0) return new Size(65, 65);

            // 56 * 75 = 4200
            double area = this.Width * this.Height;
            double scale = Math.Sqrt(4200 / area);
            if (this.Width > this.Height)
            {
                double width = scale * this.Width;
                if (width < 2) width = 2;
                else if (width > 75) width = 75;
                return new Size(width, width / this.Width * this.Height);
            }
            else // if (this.Width < this.Height)
            {
                double height = scale * this.Height;
                if (height < 2) height = 2;
                else if (height > 75) height = 75;
                return new Size(height / this.Height * this.Width, height);
            }
        }

    }
}