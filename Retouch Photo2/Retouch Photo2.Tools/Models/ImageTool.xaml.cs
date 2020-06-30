using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s ImageTool.
    /// </summary>
    public partial class ImageTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        Layerage MezzanineLayerage = null;

        //@Construct
        /// <summary>
        /// Initializes a ImageTool. 
        /// </summary>
        public ImageTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ClearButton.Click += (s, e) => this.SelectionViewModel.Photocopier = new Photocopier();//Photocopier

            //Select
            this.SelectButton.Click += (s, e) => Retouch_Photo2.DrawPage.FrameNavigatePhotosPage?.Invoke(PhotosPageMode.SelectImage);//Delegate
            Retouch_Photo2.PhotosPage.SelectCallBack += (photo) =>
            {
                if (photo == null) return;

                this.SelectionViewModel.Photocopier = photo.ToPhotocopier();//Photo
            };

            //Replace
            this.ReplaceButton.Click += (s, e) => Retouch_Photo2.DrawPage.FrameNavigatePhotosPage?.Invoke(PhotosPageMode.ReplaceImage);//Delegate
            Retouch_Photo2.PhotosPage.ReplaceCallBack += (photo) =>
            {
                if (photo == null) return;
                Photocopier photocopier = photo.ToPhotocopier();

                //Transformer
                Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.Image)
                    {
                        ImageLayer imageLayer = (ImageLayer)layer;
                        imageLayer.Photocopier = photocopier;
                    }
                });
            };
        }


        /// <summary> Tip. </summary>
        public void TipSelect() => this.EaseStoryboard.Begin();//Storyboard

        private Transformer CreateTransformer(Vector2 startingPoint, Vector2 point, float sizeWidth, float sizeHeight)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer canvasTransformer = Transformer.CreateWithAspectRatio(startingPoint, point, sizeWidth, sizeHeight);
            return canvasTransformer * inverseMatrix;
        }
        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }

    /// <summary>
    /// <see cref="ITool"/>'s ImageTool.
    /// </summary>
    public partial class ImageTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content =
                this.Title = resource.GetString("/Tools/Image");

            this.SelectTextBlock.Text = resource.GetString("/Tools/Image_Select");
            this.ReplaceTextBlock.Text = resource.GetString("/Tools/Image_Replace");
            this.ClearTextBlock.Text = resource.GetString("/Tools/Image_Clear");
            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.Image;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new ImageIcon();
        readonly ToolButton _button = new ToolButton(new ImageIcon());

        private float _sizeWidth;
        private float _sizeHeight;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Photocopier photocopier = this.SelectionViewModel.Photocopier;
            if (photocopier.FolderRelativeId == null) { this.TipSelect(); return; }

            Photo photo = Photo.FindFirstPhoto(photocopier);
            if (photo == null)
            {
                this.TipSelect();
                return;
            }

            //Transformer
            this._sizeWidth = photo.Width;
            this._sizeHeight = photo.Height;
            Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);
            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, photo.Width, photo.Height);

            //Mezzanine         
            ImageLayer imageLayer = new ImageLayer(this.ViewModel.CanvasDevice)
            {
                Photocopier = photocopier,
                IsSelected = true,
                Transform = new Transform(transformerSource, transformerDestination),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
            Layerage imageLayerage = imageLayer.ToLayerage();
            LayerBase.Instances.Add(imageLayer);


            this.MezzanineLayerage = imageLayerage;
            LayerageCollection.Mezzanine(this.ViewModel.LayerageCollection, this.MezzanineLayerage);

            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //ILayer
            if (this.MezzanineLayerage == null) return;
            ILayer mezzanineLayer = this.MezzanineLayerage.Self;

            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, this._sizeWidth, this._sizeHeight);
            mezzanineLayer.Transform.Transformer = transformerDestination;

            //Refactoring
            mezzanineLayer.IsRefactoringRender = true;
            mezzanineLayer.IsRefactoringIconRender = true;
            this.MezzanineLayerage.RefactoringParentsRender();
            this.MezzanineLayerage.RefactoringParentsIconRender();


            //Selection
            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            if (this.MezzanineLayerage == null) return;

            if (isOutNodeDistance)
            {
                if (this.MezzanineLayerage == null) return;
                ILayer mezzanineLayer = this.MezzanineLayerage.Self;


                Transformer transformerDestination = this.CreateTransformer(startingPoint, point, this._sizeWidth, this._sizeHeight);

                this.SelectionViewModel.Transformer = transformerDestination;//Selection
                mezzanineLayer.Transform.Transformer = transformerDestination;

                //Refactoring
                mezzanineLayer.IsRefactoringRender = true;
                mezzanineLayer.IsRefactoringIconRender = true;
                this.MezzanineLayerage.RefactoringParentsRender();
                this.MezzanineLayerage.RefactoringParentsIconRender();

           
                foreach (Layerage layerage in this.ViewModel.LayerageCollection.RootLayerages)
                {
                    ILayer layer = layerage.Self;

                    layer.IsSelected = false;
                }

                mezzanineLayer.IsSelected = true;
                this.MezzanineLayerage = null;
            }
            else LayerageCollection.RemoveMezzanine(this.ViewModel.LayerageCollection, this.MezzanineLayerage);//Mezzanine

            this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection

            LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.ViewModel.LayerageCollection);

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);


        public void Draw(CanvasDrawingSession drawingSession) { }

    }
}
