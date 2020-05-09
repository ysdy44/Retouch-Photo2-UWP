using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "StraightenEffect"/>.
    /// </summary>
    public sealed partial class StraightenEffect : Page, IEffect
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public StraightenEffect()
        {
            this.InitializeComponent();
            this.ConstructString();
            
            this.AnglePicker.RadiansChange += (s, radians) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.EffectManager.Straighten_Angle = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }

    /// <summary>
    /// Page of <see cref = "StraightenEffect"/>.
    /// </summary>
    public sealed partial class StraightenEffect : Page, IEffect
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
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.Straighten_Angle = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.AnglePicker.Radians = effectManager.Straighten_Angle;

            this.ToggleSwitch.IsOn = effectManager.Straighten_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.Straighten_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}