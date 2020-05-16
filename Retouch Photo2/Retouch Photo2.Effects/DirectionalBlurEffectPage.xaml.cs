using Retouch_Photo2.Effects.Icons;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        
        //@Construct
        public DirectionalBlurEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();


            //Radius
            this.RadiusSlider.ValueChangeStarted += (s, value) => { };
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.DirectionalBlur_Radius = radius;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) => { };


            //Angle
            this.AnglePicker.ValueChangeStarted += (s, value) => { };
            this.AnglePicker.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.DirectionalBlur_Angle = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AnglePicker.ValueChangeCompleted += (s, value) => { };

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

            this._button.Text = resource.GetString("/Effects/DirectionalBlur");
            
            this.RadiusTextBlock.Text = resource.GetString("/Effects/DirectionalBlur_Radius");
            this.AngleTextBlock.Text = resource.GetString("/Effects/DirectionalBlur_Angle");
        }        

        //@Content
        public EffectType Type => EffectType.DirectionalBlur;
        public FrameworkElement Page => this;
        public Control Button => this._button;
        public ToggleSwitch ToggleSwitch => this._button.ToggleSwitch;
        private EffectButton _button = new EffectButton
        {
            Icon = new DirectionalBlurIcon()
        };

        
        public void Reset()
        {
            this.RadiusSlider.Value = 0;
            this.AnglePicker.Radians = 0;
        }
        public void ResetEffect(Effect effect)
        {
            effect.DirectionalBlur_Radius = 0;
            effect.DirectionalBlur_Angle = 0;
        }
        public void FollowEffect(Effect effect)
        {
            this.RadiusSlider.Value = effect.DirectionalBlur_Radius;
            this.AnglePicker.Radians = effect.DirectionalBlur_Angle;

            this.ToggleSwitch.IsOn = effect.DirectionalBlur_IsOn;
        }
        public void OverwritingEffect(Effect effect)
        {
            effect.DirectionalBlur_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}