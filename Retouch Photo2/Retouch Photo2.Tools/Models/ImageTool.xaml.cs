// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Photos;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using System.Threading.Tasks;
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
        SettingViewModel SettingViewModel => App.SettingViewModel;

        Layerage MezzanineLayerage = null;
        /// <summary> Tip. </summary>
        public void TipSelect() => this.EaseStoryboard.Begin();//Storyboard


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;



        //@Content 
        public ToolType Type => ToolType.Image;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.ConvertToCurvesToolTip.IsOpen; set => this.ConvertToCurvesToolTip.IsOpen = value; }


        //@Construct
        /// <summary>
        /// Initializes a ImageTool. 
        /// </summary>
        public ImageTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.SelectButton.Tapped += async (s, e) => this.Select();
            this.ReplaceButton.Tapped += async (s, e) => this.Replace();
            this.ClearButton.Tapped += (s, e) => this.SelectionViewModel.Photocopier = new Photocopier();//Photocopier
        }


        private float _sizeWidth;
        private float _sizeHeight;

        private Transformer CreateTransformer(Vector2 startingPoint, Vector2 point, float sizeWidth, float sizeHeight)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer canvasTransformer = Transformer.CreateWithAspectRatio(startingPoint, point, sizeWidth, sizeHeight);
            return canvasTransformer * inverseMatrix;
        }

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Photocopier photocopier = this.SelectionViewModel.Photocopier;
            if (photocopier.FolderRelativeId == null)
            {
                this.TipSelect();
                return;
            }

            Photo photo = Photo.FindFirstPhoto(photocopier);
            if (photo == null)
            {
                this.TipSelect();
                return;
            }

            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer);
            this.ViewModel.HistoryPush(history);

            //Transformer
            this._sizeWidth = photo.Width;
            this._sizeHeight = photo.Height;
            Transformer transformer = this.CreateTransformer(startingPoint, point, photo.Width, photo.Height);

            //Mezzanine         
            ImageLayer imageLayer = new ImageLayer
            {
                Photocopier = photocopier,
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
            Layerage imageLayerage = imageLayer.ToLayerage();
            string id = imageLayerage.Id;
            LayerBase.Instances.Add(id, imageLayer);


            this.MezzanineLayerage = imageLayerage;
            LayerManager.Mezzanine(this.MezzanineLayerage);

            this.SelectionViewModel.Transformer = transformer;//Selection

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


                foreach (Layerage layerage in LayerManager.RootLayerage.Children)
                {
                    ILayer layer = layerage.Self;

                    layer.IsSelected = false;
                }

                mezzanineLayer.IsSelected = true;
                this.MezzanineLayerage = null;
            }
            else LayerManager.RemoveMezzanine(this.MezzanineLayerage);//Mezzanine

            // this.SelectionViewModel.SetMode();//Selection

            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) => this.ViewModel.ClickeTool.Cursor(point);


        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            //@DrawBound
            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    ILayer layer2 = this.SelectionViewModel.SelectionLayerage.Self;
                    drawingSession.DrawLayerBound(layer2, matrix, this.ViewModel.AccentColor);
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.ViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;
                        drawingSession.DrawLayerBound(layer, matrix, this.ViewModel.AccentColor);
                    }
                    break;
            }
        }


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate();//Invalidate
        }
        public void OnNavigatedFrom()
        {
            TouchbarExtension.Instance = null;
        }

    }


    public partial class ImageTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.SelectTextBlock.Text = resource.GetString("Tools_Image_Select");
            this.ReplaceTextBlock.Text = resource.GetString("Tools_Image_Replace");
            this.ClearTextBlock.Text = resource.GetString("Tools_Image_Clear");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");
        }

        private async Task Select()
        {
            Photo photo = await Retouch_Photo2.DrawPage.ShowGalleryFunc?.Invoke();

            if (photo == null) return;
            Photocopier photocopier = photo.ToPhotocopier();
            this.SelectionViewModel.Photocopier = photocopier;
        }

        private async Task Replace()
        {
            Photo photo = await Retouch_Photo2.DrawPage.ShowGalleryFunc?.Invoke();

            if (photo == null) return;
            Photocopier photocopier = photo.ToPhotocopier();
            this.SelectionViewModel.Photocopier = photocopier;

            this.MethodViewModel.TLayerChanged<Photocopier, ImageLayer>
            (
                layerType: LayerType.Image,
                set: (imageLayer) => imageLayer.Photocopier = photocopier,

                type: HistoryType.LayersProperty_SetPhotocopier,
                getUndo: (imageLayer) => imageLayer.Photocopier,
                setUndo: (imageLayer, previous) => imageLayer.Photocopier = previous
            );
        }

    }
}