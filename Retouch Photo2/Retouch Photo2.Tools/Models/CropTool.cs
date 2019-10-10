using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s CropTool.
    /// </summary>
    public partial class CropTool : ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;
        CompositeMode CompositeMode => this.KeyboardViewModel.CompositeMode;
        bool IsRatio => this.KeyboardViewModel.IsRatio;
        bool IsCenter => this.KeyboardViewModel.IsCenter;
        bool IsStepFrequency => this.KeyboardViewModel.IsStepFrequency;


        public bool IsSelected { set { this.Button.IsSelected = value; } }
        public ToolType Type => ToolType.Crop;
        public FrameworkElement Icon { get; } = new CropIcon();
        public IToolButton Button { get; } = new CropButton();
        public Page Page => this._cropPage;
        CropPage _cropPage { get; } = new CropPage();


        ILayer _layer;
        bool _startingIsCrop;
        Transformer _startingDestination;
        Transformer _startingCropDestination;
        TransformerMode _transformerMode;
        Transformer _startingActualDestination => this._startingIsCrop ? this._startingCropDestination : this._startingDestination;
        

        public void Starting(Vector2 point)
        {

        }
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
                        this._startingDestination = layer.TransformManager.Destination;
                        this._startingIsCrop = layer.TransformManager.IsCrop;
                        this._startingCropDestination = layer.TransformManager.CropDestination;
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
            if (this._layer.TransformManager.DisabledRadian) return;
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
            this._layer.TransformManager.IsCrop = true;
            if (isTranslation)
            {
                this._layer.TransformManager.Destination = transformer;
                if (this._startingIsCrop == false)
                {
                    this._layer.TransformManager.CropDestination = this._startingDestination;
                }
            }
            else
            {
                this._layer.TransformManager.CropDestination = transformer;
            }

            this.SelectionViewModel.IsCrop = true;//Selection
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            this._layer = null;
            this._transformerMode = TransformerMode.None;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
            {
                if (layer.SelectMode.ToBool())
                {
                    if (layer.TransformManager.DisabledRadian==false)
                    {
                        Transformer transformer = layer.GetActualDestinationWithRefactoringTransformer;
                        drawingSession.DrawCrop(transformer, matrix, Colors.BlueViolet);
                    }
                }
            }
        }
        

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            // The transformer may change after the layer is cropped.
            // So, reset the transformer.
            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
        }
    }
}