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
    /// Retouch_Photo2's the only <see cref = "ImageResPage" />. 
    /// </summary>
    public sealed partial class ImageResPage : Page
    {

        private async Task Pick()
        {
            //ImageRe
            StorageFile copyFile = await FileUtil.PickAndCopySingleImageFileAsync(PickerLocationId.Desktop);
            if (copyFile == null) return;
            ImageRe imageRe = await FileUtil.CreateImageReFromCopyFileAsync(this.ViewModel.CanvasDevice, copyFile);
            ImageRe.DuplicateChecking(imageRe);
        }

        private void Add()
        {
            //ImageRe
            ImageRe imageRe = this._vsImageRe;
            if (imageRe == null) return;

            //Transformer
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

            //Layer
            ImageStr imageStr = imageRe.ToImageStr();
            ImageLayer imageLayer = new ImageLayer
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformerSource),
                StyleManager = new Brushs.StyleManager(transformerSource, transformerSource, imageStr),
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
            //ImageRe
            ImageRe imageRe = this._vsImageRe;
            if (imageRe == null) return;

            //Transformer
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);
            Transformer transformer = this.SelectionViewModel.Transformer;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.ImageSource = transformerSource;
                        layer.StyleManager.FillBrush.ImageDestination = transformer;
                        layer.StyleManager.FillBrush.ImageStr = imageRe.ToImageStr();
                        //Selection
                        this.SelectionViewModel.StyleManager.FillBrush.ImageSource = transformerSource;
                        this.SelectionViewModel.StyleManager.FillBrush.ImageDestination = transformer;
                        this.SelectionViewModel.StyleManager.FillBrush.ImageStr = imageRe.ToImageStr();
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.ImageSource = transformerSource;
                        layer.StyleManager.StrokeBrush.ImageDestination = transformer;
                        layer.StyleManager.StrokeBrush.ImageStr = imageRe.ToImageStr();
                        //Selection
                        this.SelectionViewModel.StyleManager.StrokeBrush.ImageSource = transformerSource;
                        this.SelectionViewModel.StyleManager.StrokeBrush.ImageDestination = transformer;
                        this.SelectionViewModel.StyleManager.StrokeBrush.ImageStr = imageRe.ToImageStr();
                        break;
                }
            });

            this.SelectionViewModel.BrushImageDestination = transformer;//Selection

            this.Frame.GoBack();
        }

        private void Select()
        {
            //ImageRe
            ImageRe imageRe = this._vsImageRe;
            if (imageRe == null) return;

            this.SelectionViewModel.ImageStr = imageRe.ToImageStr();//ImageRe

            this.Frame.GoBack();
        }

        private void Replace()
        {
            //ImageRe
            ImageRe imageRe = this._vsImageRe;
            if (imageRe == null) return;

            //Transformer
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is ImageLayer imageLayer)
                {
                    imageLayer.TransformManager.Source = transformerSource;
                    imageLayer.StyleManager.FillBrush.ImageStr = imageRe.ToImageStr();
                }
            });

            this.Frame.GoBack();
        }

    }
}