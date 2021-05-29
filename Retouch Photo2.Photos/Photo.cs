// Core:              ★★★★
// Referenced:   ★★★★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★★
using Microsoft.Graphics.Canvas;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Photos
{
    /// <summary>
    /// Represents a photo that contains dpi, width, height information.
    /// </summary>
    public partial class Photo : UserControl
    {

        /// <summary> Gets bitmap dpi. </summary>  
        public float Dpi => this.Source?.Dpi ?? 96f;

        /// <summary> Gets bitmap size width pixel. </summary>  
        public uint Width => this.Source?.SizeInPixels.Width ?? 100;
        /// <summary> Gets bitmap size height pixel. </summary>  
        public uint Height => this.Source?.SizeInPixels.Height ?? 100;


        /// <summary> The source bitmap. </summary>
        public CanvasBitmap Source;
        /// <summary> The image file path. </summary>
        public string ImageFilePath;

        /// <summary> Gets or sets <see cref="StorageFile.Name"/>. </summary>
        public string Name;
        /// <summary> Gets or sets <see cref="StorageFile.FileType"/>. </summary>
        public string FileType;
        /// <summary> Gets or sets <see cref="StorageFile.FolderRelativeId"/>. </summary>
        public string FolderRelativeId;


        /// <summary>
        /// To <see cref="Photocopier"/>.
        /// </summary>
        /// <returns> The producted photocopier. </returns>
        public Photocopier ToPhotocopier()
        {
            return new Photocopier
            {
                Name = this.Name,
                FileType = this.FileType,
                FolderRelativeId = this.FolderRelativeId,
            };
        }


        /// <summary>
        ///Returns a boolean indicating whether the given <see cref="Photocopier"/> is equal to this <see cref="Photo"/> instance.
        /// </summary>
        /// <param name="other"> The <see cref="Photocopier"/> to compare this instance to. </param>
        /// <returns> True if the other <see cref="Photocopier"/> is equal to this instance; False otherwise. </returns>
        public bool Equals(Photocopier other)
        {
            if (this.Name != other.Name) return false;
            if (this.FileType != other.FileType) return false;
            if (this.FolderRelativeId != other.FolderRelativeId) return false;

            return true;
        }
        /// <summary>
        ///Returns a boolean indicating whether the given <see cref="Photo"/> is equal to this <see cref="Photo"/> instance.
        /// </summary>
        /// <param name="other"> The <see cref="Photo"/> to compare this instance to. </param>
        /// <returns> True if the other <see cref="Photo"/> is equal to this instance; False otherwise. </returns>
        public bool Equals(Photo other)
        {
            if (this.FolderRelativeId != other.FolderRelativeId) return false;

            if (this.Source == null || other.Source == null) return false;

            if (this.Dpi != other.Dpi) return false;
            if (this.Width != other.Width) return false;
            if (this.Height != other.Height) return false;

            return true;
        }


        /// <summary>
        ///Returns a String representing this <see cref="Photo"/> instance.
        /// </summary>
        /// <returns> The string representation. </returns>
        public override string ToString() => $"{this.FolderRelativeId}\n{this.Width}x{this.Height}\n{this.Dpi}Dpi";

    }
}