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
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;

        float _sizeWidth;
        float _sizeHeight;

        public bool IsSelected
        {
            set
            {
                this.Button.IsSelected = value;
                this._imagePage.IsSelected = value;
            }
        }
        public ToolType Type => ToolType.Image;
        public FrameworkElement Icon { get; } = new ImageIcon();
        public IToolButton Button { get; } = new ImageButton();
        public Page Page => this._imagePage;
        ImagePage _imagePage { get; } = new ImagePage();
        
        public void Starting(Vector2 point) { }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            ImageRe imageRe = this.SelectionViewModel.ImageRe;

            //ImageRe
            if (imageRe == null)
            {
                this._imagePage.EaseStoryboard.Begin();
                return;
            }

            //Transformer
            this._sizeWidth = imageRe.Width;
            this._sizeHeight = imageRe.Height;
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);
            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, imageRe.Width, imageRe.Height);

            //Mezzanine
            ILayer createLayer = new ImageLayer()
            {
                IsChecked = true,
                TransformManager = new TransformManager(transformerSource, transformerDestination),

                ImageRe = imageRe,
            };
            this.MezzanineViewModel.SetLayer(createLayer, this.ViewModel.Layers);

            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.MezzanineViewModel.Layer == null) return;

            //Transformer
            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, this._sizeWidth, this._sizeHeight);

            //Mezzanine
            this.MezzanineViewModel.Layer.TransformManager.Destination = transformerDestination;

            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            if (this.MezzanineViewModel.Layer == null) return;

            if (isSingleStarted)
            {
                ImageRe imageRe = this.SelectionViewModel.ImageRe;

                //ImageRe
                if (imageRe == null) return;

                //Transformer
                float sizeWidth = imageRe.Width;
                float sizeHeight = imageRe.Height;
                Transformer transformer = this.CreateTransformer(startingPoint, point, sizeWidth, sizeHeight);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.IsChecked = false;
                });

                //Transformer
                this._sizeWidth = imageRe.Width;
                this._sizeHeight = imageRe.Height;
                Transformer transformerDestination = this.CreateTransformer(startingPoint, point, imageRe.Width, imageRe.Height);
                Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

                //Mezzanine
                ILayer createLayer = new ImageLayer()
                {
                    IsChecked = true,
                    TransformManager = new TransformManager(transformerSource, transformerDestination),
                    
                    ImageRe = imageRe,
                };
                this.MezzanineViewModel.Insert(createLayer, this.ViewModel.Layers);
            }
            else this.MezzanineViewModel.None();//Mezzanine

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }


        public void Draw(CanvasDrawingSession drawingSession) { }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}