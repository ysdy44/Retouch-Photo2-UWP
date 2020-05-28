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
        public EffectType Type => EffectType.OuterShadow;
        public FrameworkElement Page => this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new OuterShadowIcon()
        };
                                    

   public void Reset()
    {
        this.RadiusSlider.Value = 0;
        this.OpacitySlider.Value = 0.5f;
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
            history.UndoActions.Push(() =>
            {
                ILayer layer2 = layerage.Self;

                layer2.Effect.OuterShadow_Radius = previous1;
                layer2.Effect.OuterShadow_Opacity = previous2;
                layer2.Effect.OuterShadow_Color = previous3;

                layer2.Effect.OuterShadow_Offset = previous4;
                layer2.Effect.OuterShadow_Angle = previous5;
            });

            layer.Effect.OuterShadow_Radius = 0;
            layer.Effect.OuterShadow_Opacity = 0.5f;
            layer.Effect.OuterShadow_Color = Windows.UI.Colors.Black;

            layer.Effect.OuterShadow_Offset = 0;
            layer.Effect.OuterShadow_Angle = 0.78539816339744830961566084581988f;// 1/4 π
        });

            //History
            this.ViewModel.HistoryPush(history);
        }
        public void Follow(Effect effect, bool isOnlyButton)
        {
            if (isOnlyButton == false)
            {
                this.RadiusSlider.Value = effect.OuterShadow_Radius;
                this.OpacitySlider.Value = effect.OuterShadow_Opacity;
                this.SolidColorBrush.Color = effect.OuterShadow_Color;

                this.OffsetSlider.Value = effect.OuterShadow_Offset;
                this.AnglePicker.Radians = effect.OuterShadow_Angle;
            }

            this.Button.IsButtonTapped = false;
            this.Button.ToggleSwitch.IsOn = effect.OuterShadow_IsOn;
            this.Button.IsButtonTapped = true;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {

        private void ConstructButton()
        {
            this.Button.ToggleSwitch.Toggled += (s, e) =>
            {
                if (this.Button.IsButtonTapped == false) return;
                bool isOn = this.Button.ToggleSwitch.IsOn;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect outer shadow");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.OuterShadow_IsOn;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.OuterShadow_IsOn = previous;
                    });

                    layer.Effect.OuterShadow_IsOn = isOn;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        public void ConstructOuterShadow_Radius()
        {
            //History
            LayersPropertyHistory history = null;

            //Radius
            this.RadiusSlider.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set effect outer shadow");

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

                    layer.Effect.OuterShadow_Radius = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Radius;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.OuterShadow_Radius = previous;
                    });

                    layer.Effect.OuterShadow_Radius = radians;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


        public void ConstructOuterShadow_Opacity()
        {
            //History
            LayersPropertyHistory history = null;

            //Opacity
            this.OpacitySlider.Maximum = 1;
            this.OpacitySlider.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set effect outer shadow");

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
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.OuterShadow_Opacity = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OpacitySlider.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Opacity;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.OuterShadow_Opacity = previous;
                    });

                    layer.Effect.OuterShadow_Opacity = radians;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


        public void ConstructOuterShadow_Offset()
        {
            //History
            LayersPropertyHistory history = null;

            //Radius
            this.OffsetSlider.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set effect outer shadow");

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

                    layer.Effect.OuterShadow_Offset = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Offset;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.OuterShadow_Offset = previous;
                    });

                    layer.Effect.OuterShadow_Offset = radians;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


        public void ConstructOuterShadow_Angle()
        {
            //History
            LayersPropertyHistory history = null;

            //Angle
            this.AnglePicker.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set effect outer shadow");

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

                    layer.Effect.OuterShadow_Angle = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AnglePicker.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Angle;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.OuterShadow_Angle = previous;
                    });

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
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.OuterShadow_Color = previous;
                    });

                    layer.Effect.OuterShadow_Color = value;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructColor2()
        {
            //History
            LayersPropertyHistory history = null;


            //Color
            this.ColorPicker.ColorChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set effect outer shadow");

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

                    layer.Effect.OuterShadow_Color = value;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.ColorPicker.ColorChangeCompleted += (s, value) =>
            {
                //Selection
                this.SolidColorBrush.Color = value;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOuterShadow_Color;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.OuterShadow_Color = previous;
                    });

                    layer.Effect.OuterShadow_Color = value;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


    }
}