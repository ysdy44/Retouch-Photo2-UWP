using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "HueRotationAdjustment"/>.
    /// </summary>
    public sealed partial class HueRotationPage : IAdjustmentGenericPage<HueRotationAdjustment>
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Generic
        /// <summary> Gets IAdjustment's adjustment. </summary>
        public HueRotationAdjustment Adjustment { get; set; }
        

        //@Construct
        public HueRotationPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructHueRotation();
        }
    }

    /// <summary>
    /// Page of <see cref = "HueRotationAdjustment"/>.
    /// </summary>
    public sealed partial class HueRotationPage : IAdjustmentGenericPage<HueRotationAdjustment>
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/HueRotation");

            this.AngleTextBlock.Text = resource.GetString("/Adjustments/HueRotation_Angle");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.HueRotation;
        public FrameworkElement Icon { get; } = new HueRotationIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new HueRotationAdjustment();

        public void Reset()
        {
            this.HueRotationSlider.Value = 0;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (this.Adjustment is HueRotationAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set hue rotation adjustment");

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previous1 = adjustment.Angle;
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is HueRotationAdjustment adjustment2)
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment2.Angle = previous1;
                        }
                    };
                    
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    adjustment.Angle = 0.0f;

                    //History
                    this.ViewModel.HistoryPush(history);

                    this.ViewModel.Invalidate();//Invalidate
                }
            }
        }
        public void Follow(HueRotationAdjustment adjustment)
        {
            this.HueRotationSlider.Value = adjustment.Angle * 180.0f / FanKit.Math.Pi;
        }
    }

    /// <summary>
    /// Page of <see cref = "HueRotationAdjustment"/>.
    /// </summary>
    public sealed partial class HueRotationPage : IAdjustmentGenericPage<HueRotationAdjustment>
    {

        private void ConstructHueRotation()
        {
            this.HueRotationSlider.Value = 0;
            this.HueRotationSlider.Minimum = 0;
            this.HueRotationSlider.Maximum = 360;

            this.HueRotationSlider.SliderBrush = this.AngleBrush;

            this.HueRotationSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HueRotationAdjustment adjustment)
                    {
                        adjustment.CacheAngle();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.HueRotationSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HueRotationAdjustment adjustment)
                    {
                        float angle = (float)value * FanKit.Math.Pi / 180.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Angle = angle;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.HueRotationSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HueRotationAdjustment adjustment)
                    {
                        float angle = (float)value * FanKit.Math.Pi / 180.0f;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set hue rotation adjustment angle");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingAngle;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is HueRotationAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringTransformer = true;
                                layer.IsRefactoringRender = true;
                                adjustment2.Angle = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringTransformer = true;
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Angle = angle;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}