using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Straighten_IsOn"/>.
    /// </summary>
    public sealed partial class StraightenEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public StraightenEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            
            //Angle
            this.AnglePicker.ValueChangeStarted += (s, value) => { };
            this.AnglePicker.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.Straighten_Angle = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AnglePicker.ValueChangeCompleted += (s, value) => { };
        }
    }

    /// <summary>
    /// Page of <see cref = "StraightenEffectPage"/>.
    /// </summary>
    public sealed partial class StraightenEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Text = resource.GetString("/Effects/Straighten");

            this.AngleTextBlock.Text = resource.GetString("/Effects/Straighten_Angle");
        }

        //@Content
        public EffectType Type => EffectType.Straighten;
        public FrameworkElement Page => this;
        public Control Button => this._button;
        public ToggleSwitch ToggleSwitch => this._button.ToggleSwitch;
        private EffectButton _button = new EffectButton
        {
            Icon = new StraightenIcon()
        };


        public void Reset()
        {
            this.AnglePicker.Radians = 0;
        }
        public void ResetEffect(Effect effect)
        {
            effect.Straighten_Angle = 0;
        }
        public void FollowEffect(Effect effect)
        {
            this.AnglePicker.Radians = effect.Straighten_Angle;

            this.ToggleSwitch.IsOn = effect.Straighten_IsOn;
        }
        public void OverwritingEffect(Effect effect)
        {
            effect.Straighten_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}