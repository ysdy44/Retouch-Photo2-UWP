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
    /// Page of <see cref = "Effect.Outline_IsOn"/>.
    /// </summary>
    public sealed partial class OutlineEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public OutlineEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructOutline_Size();
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

            this.Button.Text = resource.GetString("/Effects/Outline");

            this.SizeTextBlock.Text = resource.GetString("/Effects/Outline_Size");
        }

        //@Content
        public EffectType Type => EffectType.Outline;
        public FrameworkElement Page => this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new OutlineIcon()
        };


        public void Reset()
        {
            this.SizeSlider.Value = 0;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect outline");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = layer.Effect.Outline_Size;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Effect.Outline_Size = previous;
                });

                layer.Effect.Outline_Size = 0;
            });

            //History
            this.ViewModel.HistoryPush(history);
        }
        public void Follow(Effect effect, bool isOnlyButton)
        {
            if (isOnlyButton == false)
            {
                this.SizeSlider.Value = effect.Outline_Size;
            }

            this.Button.IsButtonTapped = false;
            this.Button.ToggleSwitch.IsOn = effect.Outline_IsOn;
            this.Button.IsButtonTapped = true;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Outline_IsOn"/>.
    /// </summary>
    public sealed partial class OutlineEffectPage : Page, IEffectPage
    {

        private void ConstructButton()
        {
            this.Button.ToggleSwitch.Toggled += (s, e) =>
            {
                if (this.Button.IsButtonTapped == false) return;
                bool isOn = this.Button.ToggleSwitch.IsOn;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect outline");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.Outline_IsOn;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.Outline_IsOn = previous;
                    });

                    layer.Effect.Outline_IsOn = isOn;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructOutline_Size()
        {
            //History
            LayersPropertyHistory history = null;

            //Radius
            this.SizeSlider.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set effect outline");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheOutline();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.SizeSlider.ValueChangeDelta += (s, value) =>
            {
                int size = (int)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.Outline_Size = size;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.SizeSlider.ValueChangeCompleted += (s, value) =>
            {
                int size = (int)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingOutline_Size;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.Outline_Size = previous;
                    });

                    layer.Effect.Outline_Size = size;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };

        }

    }
}