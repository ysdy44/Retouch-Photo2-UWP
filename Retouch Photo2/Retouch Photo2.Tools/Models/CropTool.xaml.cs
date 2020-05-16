using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s CropTool.
    /// </summary>
    public sealed partial class CropTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel ;
        TipViewModel TipViewModel => App.TipViewModel;

        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;
        MarqueeCompositeMode MarqueeCompositeMode => this.SettingViewModel.CompositeMode;
        bool IsRatio => this.SettingViewModel.IsRatio;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsStepFrequency => this.SettingViewModel.IsStepFrequency;


        //@Construct
        public CropTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ResetButton.Tapped += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Transform.IsCrop = false;
                });

                this.SelectionViewModel.IsCrop = false;//Selection
                this.ViewModel.Invalidate();//Invalidate
            };
            this.FitButton.Tapped += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer.Transform.IsCrop)
                    {
                        Transformer cropTransformer = layer.Transform.CropDestination;
                        layer.Transform.Destination = cropTransformer;
                        layer.Transform.IsCrop = false;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            // The transformer may change after the layer is cropped.
            // So, reset the transformer.
            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
        }

    }
    
    /// <summary>
    /// <see cref="ITool"/>'s CropTool.
    /// </summary>
    public sealed partial class CropTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/Crop");

            this.ResetTextBlock.Text = resource.GetString("/Tools/Crop_Reset");//Reset Crop
            this.FitTextBlock.Text = resource.GetString("/Tools/Crop_Fit");//Fit Crop
        }


        //@Content
        public ToolType Type => ToolType.Crop;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new CropIcon();
        readonly ToolButton _button = new ToolButton(new CropIcon());


        ILayer _layer;
        bool _startingIsCrop;
        Transformer _startingDestination;
        Transformer _startingCropDestination;
        TransformerMode _transformerMode;
        Transformer _startingActualDestination => this._startingIsCrop ? this._startingCropDestination : this._startingDestination;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
            {
                if (layer.SelectMode.ToBool())
                {
                    //Transformer
                    Transformer transformer = layer.GetActualDestinationWithRefactoringTransformer;
                    Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                    bool dsabledRadian = false;
                    TransformerMode transformerMode = Transformer.ContainsNodeMode
                    (
                         startingPoint,
                         transformer,
                         matrix,
                         dsabledRadian
                    );

                    if (transformerMode != TransformerMode.None)
                    {
                        this._layer = layer;
                        this._startingDestination = layer.Transform.Destination;
                        this._startingIsCrop = layer.Transform.IsCrop;
                        this._startingCropDestination = layer.Transform.CropDestination;
                        this._transformerMode = transformerMode;

                        break;
                    }
                }
            }

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this._layer == null) return;
            if (this._transformerMode == TransformerMode.None) return;

            bool isTranslation = (this._transformerMode == TransformerMode.Translation);
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();

            //Transformer
            Transformer startingDestination;
            if (isTranslation)
            {
                startingDestination = this._startingDestination;
            }
            else
            {
                startingDestination = this._startingActualDestination;
            }

            Transformer transformer = Transformer.Controller
            (
                this._transformerMode,
                startingPoint,
                point,
                startingDestination,
                inverseMatrix,
                this.IsRatio,
                this.IsCenter,
                this.IsStepFrequency
            );

            //Crop
            this._layer.Transform.IsCrop = true;
            if (isTranslation)
            {
                this._layer.Transform.Destination = transformer;
                if (this._startingIsCrop == false)
                {
                    this._layer.Transform.CropDestination = this._startingDestination;
                }
            }
            else
            {
                this._layer.Transform.CropDestination = transformer;
            }

            this.SelectionViewModel.IsCrop = true;//Selection
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            this._layer = null;
            this._transformerMode = TransformerMode.None;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => this.TipViewModel.TransformerTool.Clicke(point);


        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
            {
                if (layer.SelectMode.ToBool())
                {
                    Transformer transformer = layer.GetActualDestinationWithRefactoringTransformer;
                    drawingSession.DrawCrop(transformer, matrix, Colors.BlueViolet);
                }
            }
        }

    }
}