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

            this.ResetButton.Click += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Transform.IsCrop = false;
                });

                this.SelectionViewModel.IsCrop = false;//Selection
                this.ViewModel.Invalidate();//Invalidate
            };
            this.FitButton.Click += (s, e) =>
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


        ILayer Layer;
        bool StartingIsCrop;
        Transformer StartingDestination;
        Transformer StartingCropDestination;

        bool IsMove;
        TransformerMode TransformerMode;
        Transformer StartingActualDestination => this.StartingIsCrop ? this.StartingCropDestination : this.StartingDestination;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
            {
                if (layer.SelectMode.ToBool())
                {
                    Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                    Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                    
                    //Transformer
                    Transformer transformer = layer.GetActualDestinationWithRefactoringTransformer;
                    this.IsMove = transformer.FillContainsPoint(canvasStartingPoint);
                    this.TransformerMode = Transformer.ContainsNodeMode(canvasStartingPoint, transformer, false);

                    if (this.TransformerMode != TransformerMode.None)
                    {
                        this.Layer = layer;
                        this.StartingDestination = layer.Transform.Destination;
                        this.StartingIsCrop = layer.Transform.IsCrop;
                        this.StartingCropDestination = layer.Transform.CropDestination;
                        break;
                    }
                }
            }

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Layer == null) return;
            if (this.IsMove == false)
                if (this.TransformerMode == TransformerMode.None)
                    return;
                       
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);


            //Crop
            this.Layer.Transform.IsCrop = true;
            if (this.IsMove)
            {
                Vector2 canvasMove = canvasPoint - canvasStartingPoint;
                this.Layer.Transform.Destination = Transformer.Add(this.StartingDestination, canvasMove);

                if (this.StartingIsCrop == false)
                {
                    this.Layer.Transform.CropDestination = this.StartingDestination;
                }
            }
            if (this.TransformerMode != TransformerMode.None)
            {
                //Transformer
                Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, this.StartingActualDestination, this.IsRatio, this.IsCenter, this.IsStepFrequency);

                this.Layer.Transform.CropDestination = transformer;
            }


            this.SelectionViewModel.IsCrop = true;//Selection
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            this.Layer = null;
            this.TransformerMode = TransformerMode.None;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);


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