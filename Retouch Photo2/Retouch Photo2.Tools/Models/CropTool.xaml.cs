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

        ListViewSelectionMode SelectionMode => this.SelectionViewModel.SelectionMode;

        MarqueeCompositeMode MarqueeCompositeMode => this.SettingViewModel.CompositeMode;
        bool IsRatio => this.SettingViewModel.IsRatio;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsStepFrequency => this.SettingViewModel.IsStepFrequency;

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Construct
        /// <summary>
        /// Initializes a CropTool. 
        /// </summary>
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
                        history.UndoAction += () =>
                        {               
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            layer.Transform.IsCrop = previous;
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
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
                        var previous1 = layer.Transform.Transformer;
                        var previous2 = layer.Transform.IsCrop;
                        history.UndoAction += () =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            layer.Transform.Transformer = previous1;
                            layer.Transform.IsCrop = previous2;
                        };

                        Transformer cropTransformer = layer.Transform.CropTransformer;
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Transform.Transformer = cropTransformer;
                        layer.Transform.IsCrop = false;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
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

            this.Button.Title = resource.GetString("/Tools/Crop");

            this.ResetTextBlock.Text = resource.GetString("/Tools/Crop_Reset");//Reset Crop
            this.FitTextBlock.Text = resource.GetString("/Tools/Crop_Fit");//Fit Crop
        }


        //@Content
        public ToolType Type => ToolType.Crop;
        public FrameworkElement Icon { get; } = new CropIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new CropIcon()
        };
        public FrameworkElement Page => this;


        Layerage Layerage;
        bool IsMove;
        TransformerMode TransformerMode;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            Layerage layerage = this.GetTransformerLayer(startingPoint, matrix);
            if (layerage == null) return;
            ILayer layer = layerage.Self;


            //Transformer
            Transformer transformer = layerage.GetActualTransformer();
            if (this.TransformerMode == TransformerMode.None)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);

                this.IsMove = transformer.FillContainsPoint(canvasStartingPoint);
                if (this.IsMove == false) return;
            }


            //Snap
            if (this.IsSnap) this.ViewModel.VectorBorderSnapInitiate(layer.Transform.Transformer);


            this.Layerage = layerage;
            layer.Transform.CacheTransform();
            if (layer.Transform.IsCrop == false) this.CropStarted(layer);

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

            this.CropDelta(canvasStartingPoint, canvasPoint);

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

            this.CropDelta(canvasStartingPoint, canvasPoint);
            this.CropComplete();

            this.Layerage = null;
            this.IsMove = false;
            this.TransformerMode = TransformerMode.None;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);


        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    return;

                case ListViewSelectionMode.Single:
                    {
                        ILayer layer = this.SelectionViewModel.SelectionLayerage.Self;
                        layer.Transform.DrawCrop(drawingSession, matrix, this.ViewModel.AccentColor);
                    }
                    break;

                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.SelectionViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;
                        layer.Transform.DrawCrop(drawingSession, matrix, this.ViewModel.AccentColor);
                    }
                    break;
            }

            //Snapping
            if (this.IsSnap) this.Snap.Draw(drawingSession, matrix);
        }
    }


    /// <summary>
    /// <see cref="ITool"/>'s CropTool.
    /// </summary>
    public sealed partial class CropTool : Page, ITool
    {

        private Layerage GetTransformerLayer(Vector2 startingPoint, Matrix3x2 matrix)
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None: return null;

                case ListViewSelectionMode.Single:
                    {
                        Layerage layerage = this.SelectionViewModel.SelectionLayerage;
                        if (layerage == null) return null;

                        //Transformer
                        Transformer transformer = layerage.GetActualTransformer();
                        this.TransformerMode = Transformer.ContainsNodeMode(startingPoint, transformer, matrix, false);

                        return layerage;
                    }

                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.SelectionViewModel.SelectionLayerages)
                    {
                        //Transformer
                        Transformer transformer = layerage.GetActualTransformer();
                        TransformerMode mode = Transformer.ContainsNodeMode(startingPoint, transformer, matrix, false);
                        if (mode != TransformerMode.None)
                        {
                            this.TransformerMode = mode;
                            return layerage;
                        }
                    }
                    break;
            }
            return null;
        }

        private void CropStarted(ILayer firstLayer)
        {
            firstLayer.Transform.CropTransformer = firstLayer.Transform.Transformer;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set transform is crop");

            //History
            var previous = firstLayer.Transform.IsCrop;
            history.UndoAction += () =>
            {
                ILayer firstLayer2 = firstLayer;

                firstLayer2.Transform.IsCrop = previous;
            };

            //History
            this.ViewModel.HistoryPush(history);

            firstLayer.Transform.IsCrop = true;
        }

        private void CropDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            ILayer layer = this.Layerage.Self;
            if (this.IsMove == false)//Transformer
            {
                //Transformer
                Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, layer.Transform.StartingCropTransformer, this.IsRatio, this.IsCenter, this.IsStepFrequency);
          
                //Refactoring
                layer.IsRefactoringRender = true;
                this.Layerage.RefactoringParentsRender();
                layer.Transform.CropTransformer = transformer;
            }
            else//Move
            {
                Vector2 canvasMove = canvasPoint - canvasStartingPoint;

                //Refactoring
                layer.IsRefactoringRender = true;
                this.Layerage.RefactoringParentsRender();
                layer.Transform.CropTransformAdd(canvasMove);
            }
        }

        private void CropComplete()
        {
            ILayer layer = this.Layerage.Self;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set transform crop");

            //History
            var previous = layer.Transform.StartingCropTransformer;
            history.UndoAction += () =>
            {                
                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layer.Transform.CropTransformer = previous;
            };

            //History
            this.ViewModel.HistoryPush(history);

            //Refactoring
            layer.IsRefactoringRender = true;
            layer.IsRefactoringIconRender = true;
            this.Layerage.RefactoringParentsRender();
            this.Layerage.RefactoringParentsIconRender();
        }
        
    }
}