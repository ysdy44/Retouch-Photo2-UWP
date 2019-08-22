using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Image of <see cref="ImageRe">.
    /// </summary>
    public class ImageRe
    {
        /// <summary> Provide <see cref="ImageRe"/>'s animation in property notifications. </summary>
        public bool IsStoryboardNotify = false;

        /// <summary> Gets key. </summary>
        public string Key => this.Name;
        /// <summary> Gets bitmap dpi. </summary>  
        public float Dpi => this.Source.Dpi;
        /// <summary> Gets bitmap size width pixel. </summary>  
        public uint Width => this.Source.SizeInPixels.Width;
        /// <summary> Gets bitmap size height pixel. </summary>  
        public uint Height => this.Source.SizeInPixels.Height;

        /// <summary> The source bitmap. </summary>
        public CanvasBitmap Source;
        /// <summary> Gets or Sets <see cref="StorageFile.Name"/>. </summary>
        public string Name;
        /// <summary> Gets or Sets <see cref="StorageFile.FolderRelativeId"/>. </summary>
        public string FolderRelativeId;

        /// <summary>
        /// Returns a boolean indicating whether the given ImageRe is equal to this ImageRe instance.
        /// </summary>
        /// <param name="other"> The ImageRe to compare this instance to. </param>
        /// <returns> True if the other ImageRe is equal to this instance; False otherwise. </returns>
        public bool Equals(ImageRe other)
        {
            if (this.Key != other.Key) return false;

            if (this.Source == null || other.Source == null) return false;

            if (this.Dpi != other.Dpi) return false;
            if (this.Width != other.Width) return false;
            if (this.Height != other.Height) return false;

            return true;
        }

        //@Override
        /// <summary>
        /// Returns a String representing this ImageRe instance.
        /// </summary>
        /// <returns> The string representation. </returns>
        public override string ToString() => string.Format("{0} {1}x{2}pixels {3}Dpi", this.Key, this.Width, this.Height, this.Dpi);

        //@Static 
        /// <summary>
        /// Create a Image form a LocationId.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="location"> The destination LocationId. </param>
        /// <returns> The product ImageRe. </returns>
        public async static Task<ImageRe> CreateFromLocationIdAsync(ICanvasResourceCreator resourceCreator, PickerLocationId location)
        {
            //Picker
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = location,
                FileTypeFilter =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".bmp"
                }
            };

            //File
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null) return null;

            //ImageRe
            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, stream);

                return new ImageRe
                {
                    Source = bitmap,
                    Name = file.Name,
                    FolderRelativeId = file.FolderRelativeId,
                };
            }
        }
    }
}