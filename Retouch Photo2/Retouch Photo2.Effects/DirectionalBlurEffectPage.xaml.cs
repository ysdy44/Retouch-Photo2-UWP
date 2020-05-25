using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public DirectionalBlurEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructDirectionalBlur_Radius();
            this.ConstructDirectionalBlur_Angle();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/DirectionalBlur");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/DirectionalBlur_Radius");
            this.AngleTextBlock.Text = resource.GetString("/Effects/DirectionalBlur_Angle");
        }

        //@Content
        public EffectType Type => EffectType.DirectionalBlur;
        public FrameworkElement Page => this;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new DirectionalBlurIcon()
        };


        public void Reset()
        {
            this.RadiusSlider.Value = 0;
            this.AnglePicker.Radians = 0;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect directional blur");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous1 = layer.Effect.DirectionalBlur_Radius;
                var previous2 = layer.Effect.DirectionalBlur_Angle;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Effect.DirectionalBlur_Radius = previous1;
                    layer2.Effect.DirectionalBlur_Angle = previous2;
                });

                layer.Effect.DirectionalBlur_Radius = 0;
                layer.Effect.DirectionalBlur_Angle = 0;
            });

            //History
            this.ViewModel.HistoryPush(history);
        }
        public void FollowEffect(Effect effect, bool isOnlyButton)
        {
            if (isOnlyButton == false)
            {
                this.RadiusSlider.Value = effect.DirectionalBlur_Radius;
                this.AnglePicker.Radians = effect.DirectionalBlur_Angle;
            }

            this.Button.IsButtonTapped = false;
            this.Button.ToggleSwitch.IsOn = effect.DirectionalBlur_IsOn;
            this.Button.IsButtonTapped = true;
        }
    }
    
    /// <summary>
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurEffectPage : Page, IEffectPage
    {
        private void ConstructButton()
        {
            this.Button.ToggleSwitch.Toggled += (s, e) =>
            {
                if (this.Button.IsButtonTapped == false) return;
                bool isOn = this.Button.ToggleSwitch.IsOn;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect directional blur");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.DirectionalBlur_IsOn;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.DirectionalBlur_IsOn = previous;
                    });

                    layer.Effect.DirectionalBlur_IsOn = isOn;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructDirectionalBlur_Radius()
        {
            //History
            LayersPropertyHistory history = null;

            //Radius
            this.RadiusSlider.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set effect directional blur");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheDirectionalBlur();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.DirectionalBlur_Radius = radius;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingDirectionalBlur_Radius;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.DirectionalBlur_Radius = previous;
                    });

                    layer.Effect.DirectionalBlur_Radius = radius;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


        private void ConstructDirectionalBlur_Angle()
        {
            //History
            LayersPropertyHistory history = null;

            //Angle
            this.AnglePicker.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set effect directional blur");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheDirectionalBlur();
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

                    layer.Effect.DirectionalBlur_Angle = radians;
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
                    var previous = layer.Effect.StartingDirectionalBlur_Angle;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.DirectionalBlur_Angle = previous;
                    });

                    layer.Effect.DirectionalBlur_Angle = radians;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }
          

    }
}