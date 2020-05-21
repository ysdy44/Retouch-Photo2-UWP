using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Retouch_Photo2.Historys;
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
        }
        public void ResetEffect(Effect effect)
        {
            effect.Outline_Size = 0;
        }
        public void FollowEffect(Effect effect, bool isOnlyButton)
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
                IHistoryBase history = new IHistoryBase("Set effect isOn");

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    var previous = layer.Effect.Outline_IsOn;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Effect.Outline_IsOn = previous);

                    layer.Effect.Outline_IsOn = isOn;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructOutline_Size()
        {
            //History
            IHistoryBase history = null;

            //Radius
            this.SizeSlider.ValueChangeStarted += (s, value) =>
            {
                history = new IHistoryBase("Set effect value");

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.CacheOutline();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
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
            this.SizeSlider.ValueChangeCompleted += (s, value) =>
            {
                int size = (int)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    var previous = layer.Effect.StartingOutline_Size;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Effect.Outline_Size = previous);

                    layer.Effect.Outline_Size = size;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };

        }

    }
}