using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    public enum GammaTransferPageState
    {
        Alpha,
        Red,
        Green,
        Blue
    }

    /// <summary>
    /// Page of <see cref = "Adjustment"/>.
    /// </summary>
    public sealed partial class GammaTransferPage : IAdjustmentPage
    {
        public GammaTransferAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.GammaTransfer;
        public FrameworkElement Icon { get; } = new GammaTransferIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        //@VisualState
        bool _vsAlphaDisable  = true;
        bool _vsRedDisable = true;
        bool _vsGreenDisable = true;
        bool _vsBlueDisable = true;
        GammaTransferPageState _vsState= GammaTransferPageState.Alpha;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsState)
                {
                    case GammaTransferPageState.Alpha: return this._vsAlphaDisable  ? this.AlphaDisable : this.AlphaEnable; 
                    case GammaTransferPageState.Red: return this._vsRedDisable ? this.RedDisable : this.RedEnable; 
                    case GammaTransferPageState.Green: return this._vsGreenDisable ? this.GreenDisable : this.GreenEnable; 
                    case GammaTransferPageState.Blue: return this._vsBlueDisable ? this.BlueDisable : this.BlueDisable; 
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        //State
        public GammaTransferPageState State
        {
            set
            {                
                this._vsState = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@Construct
        public GammaTransferPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State


            this.AlphaButton.Tapped += (s, e) => this.State = GammaTransferPageState.Alpha;
            this.RedButton.Tapped += (s, e) => this.State = GammaTransferPageState.Red;
            this.GreenButton.Tapped += (s, e) => this.State = GammaTransferPageState.Green;
            this.BlueButton.Tapped += (s, e) => this.State = GammaTransferPageState.Blue;


            this.AlphaCheckBox.Toggled += (s, e) =>
            {
                bool alphaDisable = !this.AlphaCheckBox.IsOn;

                if (this._vsAlphaDisable == alphaDisable) return;
                this._vsAlphaDisable = alphaDisable;
                this.VisualState = this.VisualState;//State 

                if (this.Adjustment == null) return;
                this.Adjustment.AlphaDisable = alphaDisable;
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.RedCheckBox.Toggled += (s, e) =>
            {
                bool redDisable = !this.RedCheckBox.IsOn;

                if (this._vsRedDisable == redDisable) return;
                this._vsRedDisable = redDisable;
                this.VisualState = this.VisualState;//State 

                if (this.Adjustment == null) return;
                this.Adjustment.AlphaDisable = redDisable;
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.GreenCheckBox.Toggled += (s, e) =>
            {
                bool greenDisable = !this.GreenCheckBox.IsOn;

                if (this._vsGreenDisable == greenDisable) return;
                this._vsGreenDisable = greenDisable;
                this.VisualState = this.VisualState;//State 

                if (this.Adjustment == null) return;
                this.Adjustment.GreenDisable = greenDisable;
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.BlueCheckBox.Toggled += (s, e) =>
            {
                bool blueDisable = !this.BlueCheckBox.IsOn;

                if (this._vsBlueDisable == blueDisable) return;
                this._vsBlueDisable = blueDisable;
                this.VisualState = this.VisualState;//State 

                if (this.Adjustment == null) return;
                this.Adjustment.BlueDisable = blueDisable;
                AdjustmentManager.Invalidate?.Invoke();
            };

            
            //Alpha
            this.AlphaOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.AlphaOffset = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.AlphaExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.AlphaExponent = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.AlphaAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.AlphaAmplitude = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };

            //Red
            this.RedOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.RedOffset = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.RedExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.RedExponent = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.RedAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.RedAmplitude = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };

            //Green
            this.GreenOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.GreenOffset = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.GreenExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.GreenExponent = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.GreenAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.GreenAmplitude = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };

            //Blue
            this.BlueOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.BlueOffset = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.BlueExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.BlueExponent = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.BlueAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.BlueAmplitude = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        
        public IAdjustment GetNewAdjustment() => new GammaTransferAdjustment();
        public void Follow(GammaTransferAdjustment adjustment)
        {
            this._vsAlphaDisable = adjustment.AlphaDisable;
            this._vsRedDisable = adjustment.RedDisable;
            this._vsGreenDisable = adjustment.GreenDisable;
            this._vsBlueDisable = adjustment.BlueDisable;

            //Alpha
            this.AlphaCheckBox.IsOn = !adjustment.AlphaDisable;
            this.AlphaOffsetSlider.Value = adjustment.AlphaOffset * 100.0f;
            this.AlphaExponentSlider.Value = adjustment.AlphaExponent * 100.0f;
            this.AlphaAmplitudeSlider.Value = adjustment.AlphaAmplitude * 100.0f;

            //Red
            this.RedCheckBox.IsOn = !adjustment.RedDisable;
            this.RedOffsetSlider.Value = adjustment.RedOffset * 100.0f;
            this.RedExponentSlider.Value = adjustment.RedExponent * 100.0f;
            this.RedAmplitudeSlider.Value = adjustment.RedAmplitude * 100.0f;

            //Green
            this.GreenCheckBox.IsOn = !adjustment.GreenDisable;
            this.GreenOffsetSlider.Value = adjustment.GreenOffset * 100.0f;
            this.GreenExponentSlider.Value = adjustment.GreenExponent * 100.0f;
            this.GreenAmplitudeSlider.Value = adjustment.GreenAmplitude * 100.0f;

            //Blue
            this.BlueCheckBox.IsOn = !adjustment.BlueDisable;
            this.BlueOffsetSlider.Value = adjustment.BlueOffset * 100.0f;
            this.BlueExponentSlider.Value = adjustment.BlueExponent * 100.0f;
            this.BlueAmplitudeSlider.Value = adjustment.BlueAmplitude * 100.0f;
        }


        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/GammaTransfer");

            this.OffsetTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Offset");
            this.ExponentTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Exponent");
            this.AmplitudeTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Amplitude");
        }

    }
}