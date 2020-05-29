using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public SharpenEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructSharpen_Amount();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/Sharpen");

            this.AmountTextBlock.Text = resource.GetString("/Effects/Sharpen_Amount");
        }

        //@Content
        public EffectType Type => EffectType.Sharpen;
        public FrameworkElement Page => this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new SharpenIcon()
        };


        public void Reset()
        {
            this.AmountSlider.Value = 0;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect sharpen");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = layer.Effect.Sharpen_Amount;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Effect.Sharpen_Amount = previous;
                });

                layer.Effect.Sharpen_Amount = 0;
            });

            //History
            this.ViewModel.HistoryPush(history);
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsButtonTapped = false;
            this.ToggleSwitch.IsOn = effect.Sharpen_IsOn;
            this.Button.IsButtonTapped = true;
        }
        public void FollowPage(Effect effect)
        {
            this.AmountSlider.Value = effect.Sharpen_Amount;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {

        private void ConstructButton()
        {
            this.Button.ToggleSwitch.Toggled += (s, e) =>
            {
                if (this.Button.IsButtonTapped == false) return;
                bool isOn = this.Button.ToggleSwitch.IsOn;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect sharpen");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.Sharpen_IsOn;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.Sharpen_IsOn = previous;
                    });

                    layer.Effect.Sharpen_IsOn = isOn;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructSharpen_Amount()
        {
            //History
            LayersPropertyHistory history = null;

            //Radius
            this.AmountSlider.Maximum = 10;
            this.AmountSlider.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set effect sharpen");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheSharpen();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                float amount = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.Sharpen_Amount = amount;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AmountSlider.ValueChangeCompleted += (s, value) =>
            {
                float amount = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingSharpen_Amount;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.Sharpen_Amount = previous;
                    });

                    layer.Effect.Sharpen_Amount = amount;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }

    }
}