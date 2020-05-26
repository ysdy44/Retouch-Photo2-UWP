using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
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
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;
        MarqueeCompositeMode MarqueeCompositeMode => this.SettingViewModel.CompositeMode;
        bool IsRatio => this.SettingViewModel.IsRatio;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsStepFrequency => this.SettingViewModel.IsStepFrequency;

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Construct
        public CropTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ResetButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set transform crop");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Transform.IsCrop)
                    {
                        //History
                        var previous = layer.Transform.IsCrop;
                        history.UndoActions.Push(() =>
                        {
                            ILayer layer2 = layerage.Self;

                            layer2.Transform.IsCrop = previous;
                        });

                        layer.Transform.IsCrop = false;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
            this.FitButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set transform crop");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Transform.IsCrop)
                    {
                        //History
                        var previous1 = layer.Transform.Destination;
                        var previous2 = layer.Transform.IsCrop;
                        history.UndoActions.Push(() =>
                        {
                            ILayer layer2 = layerage.Self;

                            layer2.Transform.Destination = previous1;
                            layer2.Transform.IsCrop = previous2;
                        });

                        Transformer cropTransformer = layer.Transform.CropDestination;
                        layer.Transform.Destination = cropTransformer;
                        layer.Transform.IsCrop = false;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }

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
        bool IsMove;
        TransformerMode TransformerMode;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);

            Layerage firstLayerage = this.SelectionViewModel.GetFirstSelectedLayerage();
            if (firstLayerage == null) return;
            ILayer firstLayer = firstLayerage.Self;

            //Transformer
            Transformer transformer = firstLayer.Transform.GetActualTransformer();
            this.TransformerMode = Transformer.ContainsNodeMode(canvasStartingPoint, transformer, false);
            if (this.TransformerMode == TransformerMode.None)
            {
                this.IsMove = transformer.FillContainsPoint(canvasStartingPoint);
                if (this.IsMove == false) return;
            }

            //Snap
            if (this.IsSnap) this.ViewModel.VectorBorderSnapStarted(firstLayer.Transform.Destination);


            this.Layerage = firstLayerage;
            firstLayer.Transform.CacheTransform();
            if (firstLayer.Transform.IsCrop == false) this._started(firstLayer);

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Layerage == null) return;
            if (this.IsMove == false && this.TransformerMode == TransformerMode.None) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            //Snap
            if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);

            this._delta(canvasStartingPoint, canvasPoint);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            if (this.Layerage == null) return;
            if (this.IsMove == false && this.TransformerMode == TransformerMode.None) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
       
            //Snap
            if (this.IsSnap)
            {
                canvasPoint = this.Snap.Snap(canvasPoint);
                this.Snap.Default();
            }

            this._delta(canvasStartingPoint, canvasPoint);
            this._complete();

            this.Layerage = null;
            this.IsMove = false;
            this.TransformerMode = TransformerMode.None;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);


        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    {
                        this._draw(drawingSession, this.SelectionViewModel.SelectionLayerage, matrix);

                        //Snapping
                        if (this.IsSnap) this.Snap.Draw(drawingSession, matrix);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        foreach (Layerage layerage in this.SelectionViewModel.SelectionLayerages)
                        {
                            this._draw(drawingSession, layerage, matrix);
                        }

                        //Snapping
                        if (this.IsSnap) this.Snap.Draw(drawingSession, matrix);
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

        private void _started(ILayer firstLayer)
        {
            firstLayer.Transform.CropDestination = firstLayer.Transform.Destination;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set transform is crop");

            //History
            var previous = firstLayer.Transform.IsCrop;
            history.UndoActions.Push(() =>
            {
                ILayer firstLayer2 = firstLayer;

                firstLayer2.Transform.IsCrop = previous;
            });

            //History
            this.ViewModel.HistoryPush(history);

            firstLayer.Transform.IsCrop = true;
        }

        private void _delta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            ILayer layer = this.Layerage.Self;
            if (this.IsMove == false)//Transformer
            {
                //Transformer
                Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, layer.Transform.StartingCropDestination, this.IsRatio, this.IsCenter, this.IsStepFrequency);
                layer.Transform.CropDestination = transformer;
            }
            else//Move
            {
                Vector2 canvasMove = canvasPoint - canvasStartingPoint;
                layer.Transform.CropTransformAdd(canvasMove);
            }
        }

        private void _complete()
        {
            ILayer layer = this.Layerage.Self;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set transform crop");

            //History
            var previous = layer.Transform.StartingCropDestination;
            history.UndoActions.Push(() =>
            {
                ILayer layer2 = this.Layerage.Self;

                layer2.Transform.CropDestination = previous;
            });

            //History
            this.ViewModel.HistoryPush(history);
        }
        

        private void _draw(CanvasDrawingSession drawingSession, Layerage layerage, Matrix3x2 matrix)
        {
            ILayer layer = layerage.Self;

            if (layer.IsSelected == true)
            {
                if (layer.Transform.IsCrop)
                {
                    Transformer transformer = layer.Transform.Destination;
                    drawingSession.DrawBound(transformer, matrix, this.ViewModel.AccentColor);

                    Transformer cropTransformer = layer.Transform.CropDestination;
                    drawingSession.DrawCrop(cropTransformer, matrix, Colors.BlueViolet);
                }
                else
                {
                    Transformer transformer = layer.Transform.Destination;
                    drawingSession.DrawCrop(transformer, matrix, this.ViewModel.AccentColor);
                }
            }
        }

    }
}