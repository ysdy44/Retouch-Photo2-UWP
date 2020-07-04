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
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        //@Construct
        /// <summary>
        /// Initializes a DirectionalBlurEffectPage. 
        /// </summary>
        public DirectionalBlurEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructDirectionalBlur_Radius();
            this.ConstructDirectionalBlur_Angle();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/DirectionalBlur");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/DirectionalBlur_Radius");
            this.AngleTextBlock.Text = resource.GetString("/Effects/DirectionalBlur_Angle");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.DirectionalBlur;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new DirectionalBlurIcon()
        };
        
        public void Reset()
        {
            this.RadiusSlider.Value = 0;
            this.AnglePicker.Radians = 0;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect directional blur");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous1 = layer.Effect.DirectionalBlur_Radius;
                var previous2 = layer.Effect.DirectionalBlur_Angle;
                history.UndoAction += () =>
                {   
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Effect.DirectionalBlur_Radius = previous1;
                    layer.Effect.DirectionalBlur_Angle = previous2;
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Effect.DirectionalBlur_Radius = 0;
                layer.Effect.DirectionalBlur_Angle = 0;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.DirectionalBlur_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.RadiusSlider.Value = effect.DirectionalBlur_Radius;
            this.AnglePicker.Radians = effect.DirectionalBlur_Angle;
        }
    }
    
    /// <summary>
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructButton()
        {
            this.Button.Toggled += (isOn) => this.MethodViewModel.EffectChanged<bool>
            (
                set: (effect) => effect.DirectionalBlur_IsOn = isOn,

                historyTitle: "Set effect directional blur is on",
                getHistory: (effect) => effect.DirectionalBlur_IsOn,
                setHistory: (effect, previous) => effect.DirectionalBlur_IsOn = previous
            );            
        }


        //DirectionalBlur_Radius
        private void ConstructDirectionalBlur_Radius()
        {
            this.RadiusSlider.Minimum = 0;
            this.RadiusSlider.Maximum = 100;
            this.RadiusSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheDirectionalBlur());
            this.RadiusSlider.ValueChangeDelta += (s, value) =>  this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.DirectionalBlur_Radius = (float)value);
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>   this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.DirectionalBlur_Radius = (float)value,

                historyTitle: "Set effect directional blur radius",
                getHistory: (effect) => effect.StartingDirectionalBlur_Radius,
                setHistory: (effect, previous) => effect.DirectionalBlur_Radius = previous
            );
        }


        //DirectionalBlur_Angle
        private void ConstructDirectionalBlur_Angle()
        {
            //this.AnglePicker.Minimum = 0;
            //this.AnglePicker.Maximum = FanKit.Math.PiTwice;
            this.AnglePicker.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheDirectionalBlur());
            this.AnglePicker.ValueChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.DirectionalBlur_Angle = (float)value);
            this.AnglePicker.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.DirectionalBlur_Angle = (float)value,

                historyTitle: "Set effect directional blur angle",
                getHistory: (effect) => effect.StartingDirectionalBlur_Angle,
                setHistory: (effect, previous) => effect.DirectionalBlur_Angle = previous
            );
        }          

    }
}