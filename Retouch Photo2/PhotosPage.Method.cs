using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.Models;
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
            Photo photo = await Photo.CreatePhotoFromCopyFileAsync(this.ViewModel.CanvasDevice, copyFile);
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
            ImageLayer imageLayer = new ImageLayer(transformerSource, photocopier)
            {
                SelectMode = SelectMode.Selected,
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


        private void Fill()
        {
            ImageBrush imageBrush = this._getImageBrush();
            if (imageBrush == null) return;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.StyleManager.FillBrush = imageBrush.Clone();
                this.SelectionViewModel.StyleLayer = layer;
            });
            this.SelectionViewModel.FillBrush = imageBrush;

            this.Frame.GoBack();
        }
        private void Stroke()
        {
            ImageBrush imageBrush = this._getImageBrush();
            if (imageBrush == null) return;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.StyleManager.StrokeBrush = imageBrush.Clone();
                this.SelectionViewModel.StyleLayer = layer;
            });
            this.SelectionViewModel.StrokeBrush = imageBrush;

            this.Frame.GoBack();
        }
        private ImageBrush _getImageBrush()
        {
            //Photo
            Photo photo = this._vsPhoto;
            if (photo == null) return null;

            //Transformer
            Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);
            Transformer transformer = this.SelectionViewModel.Transformer;

            return new ImageBrush
            {
                Source = transformerSource,
                Destination = transformer,
                Photocopier = photo.ToPhotocopier(),
            };
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
                if (layer.Type == LayerType.Image)
                {

                    layer.TransformManager = new TransformManager
                    {
                        Source = transformerSource,
                        Destination = layer.TransformManager.Destination,
                    };
                    layer.StyleManager.FillBrush = new ImageBrush(transformerSource)
                    {
                        Photocopier = photo.ToPhotocopier()
                    };
                    this.SelectionViewModel.StyleLayer = layer;

                }
            });

            this.Frame.GoBack();
        }

    }
}