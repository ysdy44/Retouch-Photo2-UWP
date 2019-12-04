using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Image of <see cref="ImageRe">.
    /// </summary>
    public partial class ImageRe
    {

        /// <summary> Gets bitmap dpi. </summary>  
        public float Dpi => this.Source.Dpi;
        /// <summary> Gets bitmap size width pixel. </summary>  
        public uint Width => this.Source.SizeInPixels.Width;
        /// <summary> Gets bitmap size height pixel. </summary>  
        public uint Height => this.Source.SizeInPixels.Height;

        /// <summary> The source bitmap. </summary>
        public CanvasBitmap Source;
        /// <summary> Gets or sets <see cref="StorageFile.Name"/>. </summary>
        public string Name;
        /// <summary> Gets or sets <see cref="StorageFile.FileType"/>. </summary>
        public string FileType;
        /// <summary> Gets or sets <see cref="StorageFile.FolderRelativeId"/>. </summary>
        public string FolderRelativeId;


        /// <summary>
        /// To <see cref="ImageStr"/>.
        /// </summary>
        /// <returns> The producted <see cref="ImageStr"/>. </returns>
        public ImageStr ToImageStr()
        {
            return new ImageStr
            {
                Name = this.Name,
                FileType = this.FileType,
                FolderRelativeId = this.FolderRelativeId,
            };
        }


        /// <summary>
        /// Returns a boolean indicating whether the given ImageRe is equal to this ImageRe instance.
        /// </summary>
        /// <param name="other"> The ImageRe to compare this instance to. </param>
        /// <returns> True if the other ImageRe is equal to this instance; False otherwise. </returns>
        public bool Equals(ImageRe other)
        {
            if (this.FolderRelativeId != other.FolderRelativeId) return false;

            if (this.Source == null || other.Source == null) return false;

            if (this.Dpi != other.Dpi) return false;
            if (this.Width != other.Width) return false;
            if (this.Height != other.Height) return false;

            return true;
        }


        /// <summary>
        /// Returns a String representing this ImageRe instance.
        /// </summary>
        /// <returns> The string representation. </returns>
        public override string ToString() => string.Format("{0} {1}x{2}pixels {3}Dpi", this.FolderRelativeId, this.Width, this.Height, this.Dpi);

    }
}