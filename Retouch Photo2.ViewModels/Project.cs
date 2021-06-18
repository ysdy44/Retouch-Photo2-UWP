// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★
// Only:              ★★★★★
// Complete:      ★
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using Windows.Foundation;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// Represents a project class with width and layerages.
    /// </summary>
    public class Project
    {
        /// <summary> Gets or sets the width. </summary>
        public int Width { set; get; }

        /// <summary> Gets or sets the height. </summary>
        public int Height { set; get; }

        /// <summary> Gets or sets the layerages. </summary>
        public IEnumerable<Layerage> Layerages;


        /// <summary> Gets or sets the width for preset, Range 2 to 75. </summary>
        public double PresetWidth => this.GetPresetSize().Width;
        /// <summary> Gets or sets the height for preset, Range 2 to 75. </summary>
        public double PresetHeight => this.GetPresetSize().Height;
      
        private Size GetPresetSize()
        {
            System.Diagnostics.Debug.WriteLine("sssssssssssssssssssssssssssssssssssssss");
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