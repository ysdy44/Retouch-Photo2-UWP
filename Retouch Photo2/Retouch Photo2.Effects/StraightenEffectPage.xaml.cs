using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Straighten_IsOn"/>.
    /// </summary>
    public sealed partial class StraightenEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public StraightenEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructStraighten_Angle();
        }
    }

    /// <summary>
    /// Page of <see cref = "StraightenEffectPage"/>.
    /// </summary>
    public sealed partial class StraightenEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/Straighten");

            this.AngleTextBlock.Text = resource.GetString("/Effects/Straighten_Angle");
        }

        //@Content
        public EffectType Type => EffectType.Straighten;
        public FrameworkElement Page => this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new StraightenIcon()
        };


        public void Reset()
        {
            this.AnglePicker.Radians = 0;
        }
        public void ResetEffect(Effect effect)
        {
            effect.Straighten_Angle = 0;
        }
        public void FollowEffect(Effect effect, bool isOnlyButton)
        {
            if (isOnlyButton == false)
            {
                this.AnglePicker.Radians = effect.Straighten_Angle;
            }

            this.Button.IsButtonTapped = false;
            this.Button.ToggleSwitch.IsOn = effect.Straighten_IsOn;
            this.Button.IsButtonTapped = true;
        }
    }

    /// <summary>
    /// Page of <see cref = "StraightenEffectPage"/>.
    /// </summary>
    public sealed partial class StraightenEffectPage : Page, IEffectPage
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
                    var previous = layer.Effect.Straighten_IsOn;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Effect.Straighten_IsOn = previous);

                    layer.Effect.Straighten_IsOn = isOn;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructStraighten_Angle()
        {
            //History
            IHistoryBase history = null;

            //Angle
            this.AnglePicker.ValueChangeStarted += (s, value) =>
            {
                history = new IHistoryBase("Set effect value");

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.CacheStraighten();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.AnglePicker.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.Straighten_Angle = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AnglePicker.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    var previous = layer.Effect.StartingStraighten_Angle;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Effect.Straighten_Angle = previous);

                    layer.Effect.Straighten_Angle = radians;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }

    }
}