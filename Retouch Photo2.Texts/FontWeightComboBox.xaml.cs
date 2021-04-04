// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Texts
{
    /// <summary>
    /// Segmented of <see cref="FontWeight"/>.
    /// </summary>
    public sealed partial class FontWeightComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when weight change. </summary>
        public EventHandler<FontWeight> WeightChanged;

        //@VisualState
        FontWeight _vsWeight;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsWeight.Weight)
                {
                    case 900: return this.BlackState;
                    case 700: return this.BoldState;
                    case 950: return this.ExtraBlackState;
                    case 800: return this.ExtraBoldState;
                    case 200: return this.ExtraLightState;
                    case 300: return this.LightState;
                    case 500: return this.MediumState;
                    case 400: return this.NoneState;
                    case 600: return this.SemiBoldState;
                    case 350: return this.SemiLightState;
                    case 100: return this.ThinState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets the fontvweight. </summary>
        public FontWeight Weight
        {
            get => (FontWeight)base.GetValue(FontWeight2Property);
            set => base.SetValue(FontWeight2Property, value);
        }
        /// <summary> Identifies the <see cref = "FontWeightComboBox.Weight" /> dependency property. </summary>
        public static readonly DependencyProperty FontWeight2Property = DependencyProperty.Register(nameof(Weight), typeof(FontWeight), typeof(FontWeightComboBox), new PropertyMetadata(FontWeights.Normal, (sender, e) =>
        {
            FontWeightComboBox control = (FontWeightComboBox)sender;

            if (e.NewValue is FontWeight value)
            {
                control._vsWeight = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        /// <summary> Gets or sets the title. </summary>
        public object Title
        {
            get => (object)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "FontWeightComboBox.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(object), typeof(FontWeightComboBox), new PropertyMetadata(null));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a FontWeightComboBox. 
        /// </summary>
        public FontWeightComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.Black.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.Black);//Delegate
            this.Bold.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.Bold);//Delegate

            this.ExtraBlack.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.ExtraBlack);//Delegate
            this.ExtraBold.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.ExtraBold);//Delegate
            this.ExtraLight.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.ExtraLight);//Delegate

            this.Light.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.Light);//Delegate
            this.Medium.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.Medium);//Delegate
            this.None.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.Normal);//Delegate

            this.SemiBold.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.SemiBold);//Delegate
            this.SemiLight.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.SemiLight);//Delegate

            this.Thin.Click += (s, e) => this.WeightChanged?.Invoke(this, FontWeights.Thin);//Delegate

            this.Button.Click += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Black.Content = resource.GetString("Texts_FontWeight_Black");
            this.Black.Content = resource.GetString("Texts_FontWeight_Black");
            this.Bold.Content = resource.GetString("Texts_FontWeight_Bold");

            this.ExtraBlack.Content = resource.GetString("Texts_FontWeight_ExtraBlack");
            this.ExtraBold.Content = resource.GetString("Texts_FontWeight_ExtraBold");
            this.ExtraLight.Content = resource.GetString("Texts_FontWeight_ExtraLight");

            this.Light.Content = resource.GetString("Texts_FontWeight_Light");
            this.Medium.Content = resource.GetString("Texts_FontWeight_Medium");
            this.None.Content = resource.GetString("Texts_FontWeight_Normal");

            this.SemiBold.Content = resource.GetString("Texts_FontWeight_SemiBold");
            this.SemiLight.Content = resource.GetString("Texts_FontWeight_SemiLight");

            this.Thin.Content = resource.GetString("Texts_FontWeight_Thin");
        }
    }
}