using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Layers.Models.ImageLayer" />'s images. </summary>
        public Dictionary<string, CanvasBitmap> Images = new Dictionary<string, CanvasBitmap>();

        /// <summary> <see cref = "Retouch_Photo2.Layers.Models.ImageLayer" />'s getter. </summary>
        public CanvasBitmap GetImage(string name)
        {
            bool isContains = this.Images.ContainsKey(name);

            if (isContains == false) return null;

            return this.Images[name];
        }

        public async Task<CanvasBitmap> GetCanvasBitmap(StorageFile file)
        {
            //Image
            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(this.CanvasDevice, stream);
                return bitmap;
            }
        }
        public async Task<StorageFile> PickSingleFileAsync(PickerLocationId location)
        {
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

            StorageFile file = await openPicker.PickSingleFileAsync();
            return file;
        }

    }
}