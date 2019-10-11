using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s ImageTool.
    /// </summary>
    public partial class ImageTool : ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        float _sizeWidth;
        float _sizeHeight;

        public ToolType Type => ToolType.Image;
        public FrameworkElement Icon { get; } = new ImageIcon();
        public IToolButton Button { get; } = new ImageButton();
        public IToolPage Page => this._imagePage;
        ImagePage _imagePage { get; } = new ImagePage();
        
        public void Starting(Vector2 point) { }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            ImageRe imageRe = this.SelectionViewModel.ImageRe;

            //ImageRe
            if (imageRe == null)
            {
                this._imagePage.TipSelect();
                return;
            }
            
            //Transformer
            this._sizeWidth = imageRe.Width;
            this._sizeHeight = imageRe.Height;
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);
            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, imageRe.Width, imageRe.Height);

            //Mezzanine
            this.ViewModel.MezzanineLayer = new ImageLayer
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformerSource, transformerDestination),

                ImageRe = imageRe,
            };
            this.ViewModel.Layers.MezzanineOnFirstSelectedLayer(this.ViewModel.MezzanineLayer);

            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.ViewModel.MezzanineLayer == null) return;

            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, this._sizeWidth, this._sizeHeight);
            this.ViewModel.MezzanineLayer.TransformManager.Destination = transformerDestination;
            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            if (this.ViewModel.MezzanineLayer == null) return;

            if (isSingleStarted)
            {
                Transformer transformerDestination = this.CreateTransformer(startingPoint, point, this._sizeWidth, this._sizeHeight);
                this.ViewModel.MezzanineLayer.TransformManager.Destination = transformerDestination;
                this.SelectionViewModel.Transformer = transformerDestination;//Selection

                foreach (ILayer child in this.ViewModel.Layers.RootLayers)
                {
                    child.SelectMode = SelectMode.UnSelected;
                }
                this.ViewModel.MezzanineLayer.SelectMode = SelectMode.Selected;
                this.ViewModel.MezzanineLayer = null;

                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();
            }
            else this.ViewModel.Layers.RemoveMezzanineLayer(this.ViewModel.MezzanineLayer);//Mezzanine

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }


        public void Draw(CanvasDrawingSession drawingSession) { }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }


        private Transformer CreateTransformer(Vector2 startingPoint, Vector2 point, float sizeWidth, float sizeHeight)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer canvasTransformer = Transformer.CreateWithAspectRatio(startingPoint, point, sizeWidth, sizeHeight);
            return canvasTransformer * inverseMatrix;
        }
    }
}