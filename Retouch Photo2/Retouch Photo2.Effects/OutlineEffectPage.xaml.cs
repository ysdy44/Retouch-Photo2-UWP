using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Outline_IsOn"/>.
    /// </summary>
    public sealed partial class OutlineEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public OutlineEffectPage()
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
                    layer.Effect.Outline_Size = size;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.SizeSlider.ValueChangeCompleted += (s, value) => { };
            

        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Outline_IsOn"/>.
    /// </summary>
    public sealed partial class OutlineEffectPage : Page, IEffectPage
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
        public void ResetEffect(Effect effect)
        {
            effect.Outline_Size = 0;
        }
        public void FollowEffect(Effect effect)
        {
            this.SizeSlider.Value = effect.Outline_Size;

            this.ToggleSwitch.IsOn = effect.Outline_IsOn;
        }
        public void OverwritingEffect(Effect effect)
        {
            effect.Outline_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}

