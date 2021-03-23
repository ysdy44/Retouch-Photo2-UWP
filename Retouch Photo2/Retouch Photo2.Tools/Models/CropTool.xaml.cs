// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
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
        ViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        ListViewSelectionMode SelectionMode => this.SelectionViewModel.SelectionMode;

        MarqueeCompositeMode MarqueeCompositeMode => this.SettingViewModel.CompositeMode;
        bool IsRatio => this.SettingViewModel.IsRatio;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsStepFrequency => this.SettingViewModel.IsStepFrequency;

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Content
        public ToolType Type => ToolType.Crop;
        public ToolGroupType GroupType => ToolGroupType.Tool;
        public string Title => this.CropPage.Title;
        public ControlTemplate Icon => this.CropPage.Icon;
        public FrameworkElement Page => this.CropPage;

        readonly CropPage CropPage = new CropPage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


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
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);


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
                    layer2.Transform.DrawCrop(drawingSession, matrix, this.ViewModel.AccentColor);
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.ViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;
                        layer.Transform.DrawCrop(drawingSession, matrix, this.ViewModel.AccentColor);
                    }
                    break;
            }


            //Snapping
            if (this.IsSnap) this.Snap.Draw(drawingSession, matrix);
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }

    }


    /// <summary>
    /// Page of <see cref="CropTool"/>.
    /// </summary>
    internal partial class CropPage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content 
        public string Title { get; private set; }
        public ControlTemplate Icon => this.IconContentControl.Template;


        //@Construct
        /// <summary>
        /// Initializes a CropPage. 
        /// </summary>
        public CropPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructReset();
            this.ConstructFit(); 
            this.ConstructClear();
        }

    }

    /// <summary>
    /// Page of <see cref="CropTool"/>.
    /// </summary>
    internal partial class CropPage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Title = resource.GetString("Tools_Crop");

            this.ResetTextBlock.Text = resource.GetString("Tools_Crop_Reset");
            this.FitTextBlock.Text = resource.GetString("Tools_Crop_Fit");
            this.ClearTextBlock.Text = resource.GetString("Tools_Crop_Clear");
        }

        private void ConstructReset()
        {
            this.ResetButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetTransform_ResetTransformer);

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
        }

        private void ConstructFit()
        {
            this.FitButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetTransform_FitTransformer);

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

        private void ConstructClear()
        {
            this.ClearButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetTransform_ClearTransformer);

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Transform.IsCrop)
                    {
                        //History
                        var previous = true;
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
                        layer.Transform.IsCrop = false;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

    }
}