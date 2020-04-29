using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "PhotosPage" />. 
    /// </summary>
    public sealed partial class PhotosPage : Page
    {

        private async Task Pick()
        {
            //Photo
            StorageFile copyFile = await FileUtil.PickAndCopySingleImageFileAsync(PickerLocationId.Desktop);
            if (copyFile == null) return;
            Photo photo = await FileUtil.CreatePhotoFromCopyFileAsync(this.ViewModel.CanvasDevice, copyFile);
            Photo.DuplicateChecking(photo);
        }

        private void Add()
        {
            //Photo
            Photo photo = this._vsPhoto;
            if (photo == null) return;

            //Transformer
            Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);

            //Layer
            Photocopier photocopier = photo.ToPhotocopier();
            ImageLayer imageLayer = new ImageLayer
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformerSource),
                StyleManager = new Brushs.StyleManager(transformerSource, transformerSource, photocopier),
            };

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.SelectMode = SelectMode.UnSelected;
            });

            //Mezzanine
            this.ViewModel.Layers.MezzanineOnFirstSelectedLayer(imageLayer);
            this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

            this.Frame.GoBack();
        }

        private void Image()
        {
            //Photo
            Photo photo = this._vsPhoto;
            if (photo == null) return;

            //Transformer
            Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);
            Transformer transformer = this.SelectionViewModel.Transformer;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.PhotoSource = transformerSource;
                        layer.StyleManager.FillBrush.PhotoDestination = transformer;
                        layer.StyleManager.FillBrush.Photocopier = photo.ToPhotocopier();                        
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.PhotoSource = transformerSource;
                        layer.StyleManager.StrokeBrush.PhotoDestination = transformer;
                        layer.StyleManager.StrokeBrush.Photocopier = photo.ToPhotocopier();                      
                        break;
                }
            });

            this.SelectionViewModel.BrushImageDestination = transformer;//Selection

            this.Frame.GoBack();
        }

        private void Select()
        {
            //Photo
            Photo photo = this._vsPhoto;
            if (photo == null) return;

            this.SelectionViewModel.Photocopier = photo.ToPhotocopier();//Photo

            this.Frame.GoBack();
        }

        private void Replace()
        {
            //Photo
            Photo photo = this._vsPhoto;
            if (photo == null) return;

            //Transformer
            Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is ImageLayer imageLayer)
                {
                    imageLayer.TransformManager.Source = transformerSource;
                    imageLayer.StyleManager.FillBrush.Photocopier = photo.ToPhotocopier();
                }
            });

            this.Frame.GoBack();
        }

    }
}