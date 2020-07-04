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
    /// Page of <see cref = "Effect.Edge_IsOn"/>.
    /// </summary>
    public sealed partial class EdgeEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        
        //@Construct
        /// <summary>
        /// Initializes a EdgeEffectPage. 
        /// </summary>
        public EdgeEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructButton();
            this.ConstructEdge_Amount();
            this.ConstructEdge_Radius();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Edge_IsOn"/>.
    /// </summary>
    public sealed partial class EdgeEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/Edge");

            this.AmountTextBlock.Text = resource.GetString("/Effects/Edge_Amount");
            this.RadiusTextBlock.Text = resource.GetString("/Effects/Edge_Radius");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.Edge;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new EdgeIcon()
        };
        
        public void Reset()
        {
            this.AmountSlider.Value = 50;
            this.RadiusSlider.Value = 0;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect outline");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous1 = layer.Effect.Edge_Amount;
                var previous2 = layer.Effect.Edge_Radius;
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Effect.Edge_Amount = previous1;
                    layer.Effect.Edge_Radius = previous2;
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Effect.Edge_Amount = 0.5f;
                layer.Effect.Edge_Radius = 0.0f;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.Edge_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.AmountSlider.Value = effect.Edge_Amount * 100.0f;
            this.RadiusSlider.Value = effect.Edge_Radius;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Edge_IsOn"/>.
    /// </summary>
    public sealed partial class EdgeEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructButton()
        {
            this.Button.Toggled += (isOn) =>
            {
                this.MethodViewModel.EffectChanged<bool>
                (
                    set: (effect) => effect.Edge_IsOn = isOn,

                    historyTitle: "Set effect edge is on",
                    getHistory: (effect) => effect.Edge_IsOn,
                    setHistory: (effect, previous) => effect.Edge_IsOn = previous
                );
            };
        }


        //Edge_Amount
        private void ConstructEdge_Amount()
        {
            this.AmountSlider.Minimum = 0;
            this.AmountSlider.Maximum = 100;
            this.AmountSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheEdge());
            this.AmountSlider.ValueChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Edge_Amount = (float)value / 100.0f);
            this.AmountSlider.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.Edge_Amount = (float)value / 100.0f,

                historyTitle: "Set effect edge amount",
                getHistory: (effect) => effect.StartingEdge_Amount,
                setHistory: (effect, previous) => effect.Edge_Amount = previous
            );
        }


        //Edge_Radius
        private void ConstructEdge_Radius()
        {
            this.RadiusSlider.Minimum = 0;
            this.RadiusSlider.Maximum = 10;
            this.RadiusSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheEdge());
            this.RadiusSlider.ValueChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Edge_Radius = (float)value);
            this.RadiusSlider.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.Edge_Radius = (float)value,

                historyTitle: "Set effect edge radius",
                getHistory: (effect) => effect.StartingEdge_Radius,
                setHistory: (effect, previous) => effect.Edge_Radius = previous
            );
        }

    }
}