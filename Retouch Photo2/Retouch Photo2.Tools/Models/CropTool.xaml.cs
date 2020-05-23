using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
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
        SettingViewModel SettingViewModel => App.SettingViewModel;
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
                this.ViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Transform.IsCrop = false;
                });

                this.ViewModel.IsCrop = false;//Selection
                this.ViewModel.Invalidate();//Invalidate
            };
            this.FitButton.Click += (s, e) =>
            {
                //Selection
                this.ViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

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
            this.ViewModel.SetMode(this.ViewModel.LayerCollection);//Selection
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


        Layerage Layerage;
        bool StartingIsCrop;
        Transformer StartingDestination;
        Transformer StartingCropDestination;

        bool IsMove;
        TransformerMode TransformerMode;
        Transformer StartingActualDestination => this.StartingIsCrop ? this.StartingCropDestination : this.StartingDestination;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Layerage firstLayer = this.ViewModel.GetFirstLayer();
            IList<Layerage> parentsChildren = this.ViewModel.LayerCollection.GetParentsChildren(firstLayer);


            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);

            switch (this.ViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    this.Check(this.ViewModel.Layerage, canvasStartingPoint);
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layer in this.ViewModel.Layerages)
                    {
                        this.Check(layer, canvasStartingPoint);
                    }
                    break;
            }

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Layerage == null) return;
            if (this.IsMove == false)
                if (this.TransformerMode == TransformerMode.None)
                    return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);


            //Crop
            ILayer layer = this.Layerage.Self;

            layer.Transform.IsCrop = true;
            if (this.TransformerMode != TransformerMode.None)
            {
                //Transformer
                Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, this.StartingActualDestination, this.IsRatio, this.IsCenter, this.IsStepFrequency);
                layer.Transform.CropDestination = transformer;
            }
            else if (this.IsMove)
            {
                Vector2 canvasMove = canvasPoint - canvasStartingPoint;
                layer.Transform.Destination = Transformer.Add(this.StartingDestination, canvasMove);

                if (this.StartingIsCrop == false)
                {
                    layer.Transform.CropDestination = this.StartingDestination;
                }
            }


            this.ViewModel.IsCrop = true;//Selection
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            this.Layerage = null;
            this.TransformerMode = TransformerMode.None;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);


        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            switch (this.ViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    this.Draw(drawingSession, this.ViewModel.Layerage, matrix);
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.ViewModel.LayerCollection.RootLayerages)
                    {
                        this.Draw(drawingSession, layerage, matrix);
                    }
                    break;
            }
        }
    }


    /// <summary>
    /// <see cref="ITool"/>'s CropTool.
    /// </summary>
    public sealed partial class CropTool : Page, ITool
    {

        private bool Check(Layerage layer, Vector2 canvasStartingPoint)
        {
            ILayer layer2 = layer.Self;
            if (layer2.IsSelected == true)
            {
                //Transformer
                Transformer transformer = layer2.GetActualDestinationWithRefactoringTransformer;
                this.IsMove = transformer.FillContainsPoint(canvasStartingPoint);
                this.TransformerMode = Transformer.ContainsNodeMode(canvasStartingPoint, transformer, false);

                if (this.IsMove || this.TransformerMode != TransformerMode.None)
                {
                    this.Layerage = layer;
                    this.StartingDestination = layer2.Transform.Destination;
                    this.StartingIsCrop = layer2.Transform.IsCrop;
                    this.StartingCropDestination = layer2.Transform.CropDestination;
                    return true;
                }
            }

            return false;
        }


        private void Draw(CanvasDrawingSession drawingSession, Layerage layer, Matrix3x2 matrix)
        {
            ILayer layer2 = layer.Self;

            if (layer2.IsSelected == true)
            {
                Transformer transformer = layer2.GetActualDestinationWithRefactoringTransformer;
                drawingSession.DrawCrop(transformer, matrix, Colors.BlueViolet);
            }
        }

    }
}