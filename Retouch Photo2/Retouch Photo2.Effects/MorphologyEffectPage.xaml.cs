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
    /// Page of <see cref = "Effect.Morphology_IsOn"/>.
    /// </summary>
    public sealed partial class MorphologyEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public MorphologyEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructMorphology_Size();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Morphology_IsOn"/>.
    /// </summary>
    public sealed partial class MorphologyEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/Morphology");

            this.SizeTextBlock.Text = resource.GetString("/Effects/Morphology_Size");
        }

        //@Content
        public EffectType Type => EffectType.Morphology;
        public FrameworkElement Page => this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new MorphologyIcon()
        };


        public void Reset()
        {
            this.SizeSlider.Value = 1;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect Morphology");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = layer.Effect.Morphology_Size;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Effect.Morphology_Size = previous;
                });

                layer.Effect.Morphology_Size = 1;
            });

            //History
            this.ViewModel.HistoryPush(history);
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsButtonTapped = false;
            this.Button.ToggleSwitch.IsOn = effect.Morphology_IsOn;
            this.Button.IsButtonTapped = true;
        }
        public void FollowPage(Effect effect)
        {
            this.SizeSlider.Value = effect.Morphology_Size;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Morphology_IsOn"/>.
    /// </summary>
    public sealed partial class MorphologyEffectPage : Page, IEffectPage
    {

        private void ConstructButton()
        {
            this.Button.ToggleSwitch.Toggled += (s, e) =>
            {
                if (this.Button.IsButtonTapped == false) return;
                bool isOn = this.Button.ToggleSwitch.IsOn;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect morphology");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.Morphology_IsOn;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.Morphology_IsOn = previous;
                    });

                    layer.Effect.Morphology_IsOn = isOn;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructMorphology_Size()
        {
            //History
            LayersPropertyHistory history = null;

            //Radius
            this.SizeSlider.Value = 1;
            this.SizeSlider.Minimum = -100;
            this.SizeSlider.Maximum = 100;
            this.SizeSlider.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set effect morphology");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheMorphology();
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

                    layer.Effect.Morphology_Size = size;
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
                    var previous = layer.Effect.StartingMorphology_Size;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Effect.Morphology_Size = previous;
                    });

                    layer.Effect.Morphology_Size = size;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };

        }

    }
}