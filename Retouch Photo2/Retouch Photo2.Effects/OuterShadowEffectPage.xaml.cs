using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        
        //@Construct
        /// <summary>
        /// Initializes a OuterShadowEffectPage. 
        /// </summary>
        public OuterShadowEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();

            this.ConstructOuterShadow_Radius();
            this.ConstructOuterShadow_Opacity();
            this.ConstructOuterShadow_Offset();
            this.ConstructOuterShadow_Angle();
            this.ConstructColor1();
            this.ConstructColor2();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/OuterShadow");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/OuterShadow_Radius");
            this.OpacityTextBlock.Text = resource.GetString("/Effects/OuterShadow_Opacity");
            this.OffsetTextBlock.Text = resource.GetString("/Effects/OuterShadow_Offset");

            this.AngleTextBlock.Text = resource.GetString("/Effects/OuterShadow_Angle");
            this.ColorTextBlock.Text = resource.GetString("/Effects/OuterShadow_Color");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.OuterShadow;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new OuterShadowIcon()
        };
        
        public void Reset()
        {
            this.RadiusSlider.Value = 12;
            this.OpacitySlider.Value = 50;
            this.SolidColorBrush.Color = Windows.UI.Colors.Black;

            this.OffsetSlider.Value = 0;
            this.AnglePicker.Radians = 0.78539816339744830961566084581988f;// 1/4 π

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect value");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous1 = layer.Effect.OuterShadow_Radius;
                var previous2 = layer.Effect.OuterShadow_Opacity;
                var previous3 = layer.Effect.OuterShadow_Color;
                var previous4 = layer.Effect.OuterShadow_Offset;
                var previous5 = layer.Effect.OuterShadow_Angle;
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Effect.OuterShadow_Radius = previous1;
                    layer.Effect.OuterShadow_Opacity = previous2;
                    layer.Effect.OuterShadow_Color = previous3;
                    layer.Effect.OuterShadow_Offset = previous4;
                    layer.Effect.OuterShadow_Angle = previous5;
                };
                
                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Effect.OuterShadow_Radius = 12.0f;
                layer.Effect.OuterShadow_Opacity = 0.5f;
                layer.Effect.OuterShadow_Color = Windows.UI.Colors.Black;
                layer.Effect.OuterShadow_Offset = 0;
                layer.Effect.OuterShadow_Angle = 0.78539816339744830961566084581988f;// 1/4 π
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.OuterShadow_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.RadiusSlider.Value = effect.OuterShadow_Radius;
            this.OpacitySlider.Value = effect.OuterShadow_Opacity * 100.0f;
            this.SolidColorBrush.Color = effect.OuterShadow_Color;

            this.OffsetSlider.Value = effect.OuterShadow_Offset;
            this.AnglePicker.Radians = effect.OuterShadow_Angle;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {

        private void ConstructButton()
        {
            this.Button.Toggled += (isOn) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect outer shadow");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.OuterShadow_IsOn;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.OuterShadow_IsOn = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.OuterShadow_IsOn = isOn;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructOuterShadow_Radius()
        {
            //Radius
            this.RadiusSlider.Value = 0;
            this.RadiusSlider.Minimum = 0;
            this.RadiusSlider.Maximum = 100;
            this.RadiusSlider.ValueChangeStarted += (s, value) =>
            {             
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheOuterShadow();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.Effect.OuterShadow_Radius = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect outer shadow");
                
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Radius;
                    history.UndoAction += () =>
                    {   
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;

                        layer.Effect.OuterShadow_Radius = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.OuterShadow_Radius = radians;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


        private void ConstructOuterShadow_Opacity()
        {
            //Opacity
            this.OpacitySlider.Value = 50;
            this.OpacitySlider.Minimum = 0;
            this.OpacitySlider.Maximum = 100;
            this.OpacitySlider.ValueChangeStarted += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheOuterShadow();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.OpacitySlider.ValueChangeDelta += (s, value) =>
            {
                float opacity = (float)value / 100.0f;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.Effect.OuterShadow_Opacity = opacity;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OpacitySlider.ValueChangeCompleted += (s, value) =>
            {
                float opacity = (float)value / 100.0f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect outer shadow");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Opacity;
                    history.UndoAction += () =>
                    {    
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.OuterShadow_Opacity = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.OuterShadow_Opacity = opacity;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


        private void ConstructOuterShadow_Offset()
        {

            //Radius
            this.OffsetSlider.ValueChangeStarted += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheOuterShadow();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.OffsetSlider.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.Effect.OuterShadow_Offset = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect outer shadow");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Offset;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.OuterShadow_Offset = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.OuterShadow_Offset = radians;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


        private void ConstructOuterShadow_Angle()
        {
            //Angle
            this.AnglePicker.ValueChangeStarted += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheOuterShadow();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.AnglePicker.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.Effect.OuterShadow_Angle = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AnglePicker.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect outer shadow");
                
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Angle;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.OuterShadow_Angle = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.OuterShadow_Angle = radians;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


        private void ConstructColor1()
        {
            this.ColorBorder.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorBorder);
                this.ColorPicker.Color = this.SolidColorBrush.Color;
            };
            this.ColorPicker.ColorChanged += (s, value) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect outer shadow");

                //Selection
                this.SolidColorBrush.Color = value;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Color;
                    history.UndoAction += () =>
                    {
                        ILayer layer2 = layerage.Self;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer2.Effect.OuterShadow_Color = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.OuterShadow_Color = value;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructColor2()
        {
            //Color
            this.ColorPicker.ColorChangeStarted += (s, value) =>
            {
                //Selection
                this.SolidColorBrush.Color = value;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheGaussianBlur();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.ColorPicker.ColorChangeDelta += (s, value) =>
            {
                //Selection
                this.SolidColorBrush.Color = value;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.Effect.OuterShadow_Color = value;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.ColorPicker.ColorChangeCompleted += (s, value) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect outer shadow");

                //Selection
                this.SolidColorBrush.Color = value;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Color;
                    history.UndoAction += () =>
                    {
                        ILayer layer2 = layerage.Self;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer2.Effect.OuterShadow_Color = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.OuterShadow_Color = value;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


    }
}