using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Emboss_IsOn"/>.
    /// </summary>
    public sealed partial class EmbossEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        
        //@Construct
        /// <summary>
        /// Initializes a EmbossEffectPage. 
        /// </summary>
        public EmbossEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructEmboss_Radius();
            this.ConstructEmboss_Angle();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Emboss_IsOn"/>.
    /// </summary>
    public sealed partial class EmbossEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/Emboss");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/Emboss_Radius");
            this.AngleTextBlock.Text = resource.GetString("/Effects/Emboss_Angle");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.Emboss;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new EmbossIcon()
        };
        
        public void Reset()
        {
            this.RadiusSlider.Value = 1;
            this.AnglePicker.Radians = 0;
            
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect emboss");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous1 = layer.Effect.Emboss_Radius;
                var previous2 = layer.Effect.Emboss_Angle;
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Effect.Emboss_Radius = previous1;
                    layer.Effect.Emboss_Angle = previous2;
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Effect.Emboss_Radius = 1;
                layer.Effect.Emboss_Angle = 0;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.Emboss_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.RadiusSlider.Value = effect.Emboss_Radius;
            this.AnglePicker.Radians = effect.Emboss_Angle;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Emboss_IsOn"/>.
    /// </summary>
    public sealed partial class EmbossEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructButton()
        {
            this.Button.Toggled += (isOn) =>
            {
                this.MethodViewModel.EffectChanged<bool>
                (
                    set: (effect) => effect.Emboss_IsOn = isOn,

                    historyTitle: "Set effect emboss is on",
                    getHistory: (effect) => effect.Emboss_IsOn,
                    setHistory: (effect, previous) => effect.Emboss_IsOn = previous
                );
            };
        }


        //Emboss_Radius
        private void ConstructEmboss_Radius()
        {
            this.RadiusSlider.Minimum = 0;
            this.RadiusSlider.Maximum = 10;
            this.RadiusSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheEmboss());
            this.RadiusSlider.ValueChangeDelta += (s, value) =>  this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Emboss_Radius = (float)value);
            this.RadiusSlider.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.Emboss_Radius = (float)value,

                historyTitle: "Set effect emboss radius",
                getHistory: (effect) => effect.StartingEmboss_Radius,
                setHistory: (effect, previous) => effect.Emboss_Radius = previous
            );
        }


        //Emboss_Angle
        private void ConstructEmboss_Angle()
        {
            //this.AnglePicker.Minimum = 0;
            //this.AnglePicker.Maximum = FanKit.Math.PiTwice;
            this.AnglePicker.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheEmboss());
            this.AnglePicker.ValueChangeDelta += (s, value) =>   this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Emboss_Angle = (float)value);
            this.AnglePicker.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.Emboss_Angle = (float)value,

                historyTitle: "Set effect emboss angle",
                getHistory: (effect) => effect.StartingEmboss_Angle,
                setHistory: (effect, previous) => effect.Emboss_Angle = previous
            );
        }

    }
}