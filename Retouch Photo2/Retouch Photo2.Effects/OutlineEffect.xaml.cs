using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "OutlineEffect"/>.
    /// </summary>
    public sealed partial class OutlineEffect : Page, IEffect
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public OutlineEffect()
        {
            this.InitializeComponent();
            this.ConstructString();


            //Radius
            this.SizeSlider.ValueChangeStarted += (s, value) => { };
            this.SizeSlider.ValueChangeDelta += (s, value) =>
            {
                int size = (int)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.EffectManager.Outline_Size = size;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.SizeSlider.ValueChangeCompleted += (s, value) => { };
            

        }
    }

    /// <summary>
    /// Page of <see cref = "OutlineEffect"/>.
    /// </summary>
    public sealed partial class OutlineEffect : Page, IEffect
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Text = resource.GetString("/Effects/Outline");

            this.SizeTextBlock.Text = resource.GetString("/Effects/Outline_Size");
        }

        //@Content
        public EffectType Type => EffectType.Outline;
        public FrameworkElement Page => this;
        public Control Button => this._button;
        public ToggleSwitch ToggleSwitch => this._button.ToggleSwitch;
        private EffectButton _button = new EffectButton
        {
            Icon = new OutlineIcon()
        };


        public void Reset()
        {
            this.SizeSlider.Value = 0;
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.Outline_Size = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.SizeSlider.Value = effectManager.Outline_Size;

            this.ToggleSwitch.IsOn = effectManager.Outline_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.Outline_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}

