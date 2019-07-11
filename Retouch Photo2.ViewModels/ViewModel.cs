using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI;
using Retouch_Photo2.Layers.Models;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> Mode of the <see cref = "ViewModel.Invalidate" />. </summary>
    public enum InvalidateMode
    {
        /// <summary> Normal </summary>
        None,
        /// <summary> Thumbnail </summary>
        Thumbnail,
        /// <summary> High-definition </summary>
        HD,
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />.
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Reload <see cref = "ViewModel" /> 
        /// </summary>
        /// <param name="project"> project </param>
        public void LoadFromProject(Project project)
        {
            if (project == null) return;

            this.CanvasTransformer.Width = project.Width;
            this.CanvasTransformer.Height = project.Height;

            this.Layers.Clear();
            foreach (Layer layer in project.Layers)
            {
                this.Layers.Add(layer);
            }
        }


        /// <summary> Retouch_Photo2's the only AccentColor. </summary>
        public Color AccentColor { get; set; }
        /// <summary> Retouch_Photo2's the only <see cref = "Microsoft.Graphics.Canvas.CanvasDevice" />. </summary>
        public CanvasDevice CanvasDevice { get; } = new CanvasDevice();


        /// <summary>
        /// Indicates that the contents of the CanvasControl need to be redrawn.
        /// </summary>
        /// <param name="mode"> invalidate mode </param>
        public void Invalidate(InvalidateMode mode = InvalidateMode.None) => this.InvalidateAction?.Invoke(mode);
        /// <summary> <see cref = "Action" /> of the <see cref = "ViewModel.Invalidate" />. </summary>
        public Action<InvalidateMode> InvalidateAction { private get; set; }


        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Layers.Layer" />s. </summary>
        public ObservableCollection<Layer> Layers { get; } = new ObservableCollection<Layer>();



        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Layers.Models.ImageLayer" />'s images. </summary>
        public Stack<ImageRe> Images = new Stack<ImageRe>();
        
        /// <summary>
        /// Check duplicate ImageRe.
        /// If it exists, replace it, or insert it into the Images.
        /// </summary>
        /// <param name="imageRe"> The source ImageRe. </param>
        public void DuplicateChecking(ImageRe imageRe)
        {
            foreach (ImageRe imageRe2 in Images)
            {
                if (imageRe2.Key == imageRe.Key)
                {
                    imageRe= imageRe2;
                    return;
                }
            }

            this.Images.Push(imageRe);//Images
        }

                
        /// <summary>
        /// Async pick a file.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
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


        //Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="name"> Name of the property used to notify listeners. </param>
        protected void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}