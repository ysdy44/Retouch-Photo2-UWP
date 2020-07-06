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
        ViewModel MethodViewModel => App.MethodViewModel;
        
        
        //@Construct
        /// <summary>
        /// Initializes a MorphologyEffectPage. 
        /// </summary>
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
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.Morphology;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new MorphologyIcon()
        };
        
        public void Reset()
        {
            this.SizeSlider.Value = 1;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect morphology");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = layer.Effect.Morphology_Size;
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Effect.Morphology_Size = previous;
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Effect.Morphology_Size = 1;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.Morphology_IsOn;
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

        //IsOn
        private void ConstructButton()
        {
            this.Button.Toggled += (isOn) =>
            {
                this.MethodViewModel.EffectChanged<bool>
                (
                    set: (effect) => effect.Morphology_IsOn = isOn,

                    historyTitle: "Set effect morphology is on",
                    getHistory: (effect) => effect.Morphology_IsOn,
                    setHistory: (effect, previous) => effect.Morphology_IsOn = previous
                );
            };
        }


        //Morphology_Size
        private void ConstructMorphology_Size()
        {
            this.SizeSlider.Minimum = -100.0d;
            this.SizeSlider.Maximum = 100.0d;
            this.SizeSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheMorphology());
            this.SizeSlider.ValueChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Morphology_Size = (int)value);
            this.SizeSlider.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<int>
            (
                set: (effect) => effect.Morphology_Size = (int)value,

                historyTitle: "Set effect morphology size",
                getHistory: (effect) => effect.StartingMorphology_Size,
                setHistory: (effect, previous) => effect.Morphology_Size = previous
            );
        }

    }
}