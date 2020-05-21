using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.Historys;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
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
        public EffectType Type => EffectType.Emboss;
        public FrameworkElement Page => this;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new EmbossIcon()
        };


        public void Reset()
        {
            this.RadiusSlider.Value = 0;
            this.AnglePicker.Radians = 0;
        }
        public void ResetEffect(Effect effect)
        {
            effect.Emboss_Radius = 0;
            effect.Emboss_Angle = 0;
        }
        public void FollowEffect(Effect effect)
        {
            this.RadiusSlider.Value = effect.Emboss_Radius;
            this.AnglePicker.Radians = effect.Emboss_Angle;

            this.Button.ToggleSwitch.IsOn = effect.Emboss_IsOn;
        }
        public void FollowEffect(Effect effect, bool isOnlyButton)
        {
            if (isOnlyButton == false)
            {
                this.RadiusSlider.Value = effect.GaussianBlur_Radius;
            }

            this.Button.IsButtonTapped = false;
            this.Button.ToggleSwitch.IsOn = effect.GaussianBlur_IsOn;
            this.Button.IsButtonTapped = true;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Emboss_IsOn"/>.
    /// </summary>
    public sealed partial class EmbossEffectPage : Page, IEffectPage
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
                    var previous = layer.Effect.Emboss_IsOn;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Effect.Emboss_IsOn = previous);

                    layer.Effect.Emboss_IsOn = isOn;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructEmboss_Radius()
        {
            //History
            IHistoryBase history = null;

            //Radius
            this.RadiusSlider.ValueChangeStarted += (s, value) =>
            {
                history = new IHistoryBase("Set effect value");

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.CacheEmboss();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.Emboss_Radius = radius;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    var previous = layer.Effect.StartingEmboss_Radius;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Effect.Emboss_Radius = previous);

                    layer.Effect.Emboss_Radius = radius;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


        private void ConstructEmboss_Angle()
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
                    layer.Effect.CacheEmboss();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.AnglePicker.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.Emboss_Angle = radians;
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
                    var previous = layer.Effect.StartingEmboss_Angle;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Effect.Emboss_Angle = previous);

                    layer.Effect.Emboss_Angle = radians;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }

    }
}