using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System;
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
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        float SizeWidth;
        float SizeHeight;


        public ToolType Type => ToolType.Image;
        public FrameworkElement Icon { get; } = new ImageControl();
        public FrameworkElement ShowIcon { get; } = new ImageControl();
        public Page Page { get; } = new ImagePage();


        public void Starting(Vector2 point) { }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            ImageRe imageRe = this.SelectionViewModel.ImageRe;

            //ImageRe
            if (imageRe == null)
            {
                this.SelectionViewModel.ImageRe = new ImageRe { IsStoryboardNotify = true };
                return;
            }
            if (imageRe.IsStoryboardNotify == true)
            {
                this.SelectionViewModel.ImageRe = new ImageRe { IsStoryboardNotify = true };
                return;
            }

            //Transformer
            this.SizeWidth = imageRe.Width;
            this.SizeHeight = imageRe.Height;
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);
            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, imageRe.Width, imageRe.Height);

            //Mezzanine
            ILayer createLayer = new ImageLayer()
            {
                ImageRe = imageRe,
                Source = transformerSource,
                Destination = transformerDestination,
                IsChecked = true,
            };
            this.MezzanineViewModel.SetLayer(createLayer, this.ViewModel.Layers);

            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.MezzanineViewModel.Layer == null) return;

            //Transformer
            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, this.SizeWidth, this.SizeHeight);

            this.MezzanineViewModel.Layer.Destination = transformerDestination;//Mezzanine

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
                if (imageRe.IsStoryboardNotify == true) return;

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
                this.SizeWidth = imageRe.Width;
                this.SizeHeight = imageRe.Height;
                Transformer transformerDestination = this.CreateTransformer(startingPoint, point, imageRe.Width, imageRe.Height);
                Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

                //Mezzanine
                ILayer createLayer = new ImageLayer()
                {
                    ImageRe = imageRe,
                    Source = transformerSource,
                    Destination = transformerDestination,
                    IsChecked = true,
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