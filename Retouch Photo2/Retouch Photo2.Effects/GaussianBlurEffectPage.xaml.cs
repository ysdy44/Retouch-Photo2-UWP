using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public GaussianBlurEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructSharpen_Amount();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/GaussianBlur");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/GaussianBlur_Radius");
        }

        //@Content
        public EffectType Type => EffectType.GaussianBlur;
        public FrameworkElement Page => this;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new GaussianBlurIcon()
        };


        public void Reset()
        {
            this.RadiusSlider.Value = 0;
        }
        public void ResetEffect(Effect effect)
        {
            effect.GaussianBlur_Radius = 0;
        }
        public void FollowEffect(Effect effect, bool isOnlyButton)
        {
            if (isOnlyButton==false)
            {
                this.RadiusSlider.Value = effect.GaussianBlur_Radius;
            }

            this.Button.IsButtonTapped = false;
            this.Button.ToggleSwitch.IsOn = effect.GaussianBlur_IsOn;
            this.Button.IsButtonTapped = true;
        }

    }
    
    /// <summary>
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffectPage : Page, IEffectPage
    {
        private void ConstructButton()
        {
            this.Button.ToggleSwitch.Toggled += (s, e) =>
            {
                if (this.Button.IsButtonTapped == false) return;
                bool isOn = this.Button.ToggleSwitch.IsOn;

                //History
                IHistoryBase history = new IHistoryBase("Set effect isOn");

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    var previous = layer.Effect.GaussianBlur_IsOn;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Effect.GaussianBlur_IsOn = previous);

                    layer.Effect.GaussianBlur_IsOn = isOn;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructSharpen_Amount()
        {
            //History
            IHistoryBase history = null;

            //Radius
            this.RadiusSlider.ValueChangeStarted += (s, value) =>
            {
                history = new IHistoryBase("Set effect value");

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.CacheGaussianBlur();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.GaussianBlur_Radius = radius;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    var previous = layer.Effect.StartingGaussianBlur_Radius;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Effect.GaussianBlur_Radius = previous);

                    layer.Effect.GaussianBlur_Radius = radius;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }

    }
}