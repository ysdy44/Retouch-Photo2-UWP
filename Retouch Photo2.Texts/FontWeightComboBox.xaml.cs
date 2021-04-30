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
    /// Segmented of <see cref="FontWeight2"/>.
    /// </summary>
    public sealed partial class FontWeightComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when weight change. </summary>
        public event EventHandler<FontWeight2> WeightChanged;

        //@VisualState
        FontWeight2 _vsWeight;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {

                switch (this._vsWeight)
                {
                    case FontWeight2.Black: return this.BlackState;
                    case FontWeight2.Bold: return this.BoldState;

                    case FontWeight2.ExtraBlack: return this.ExtraBlackState;
                    case FontWeight2.ExtraBold: return this.ExtraBoldState;
                    case FontWeight2.ExtraLight: return this.ExtraLightState;

                    case FontWeight2.Light: return this.LightState;
                    case FontWeight2.Medium: return this.MediumState;
                    case FontWeight2.Normal: return this.NoneState;

                    case FontWeight2.SemiBold: return this.SemiBoldState;
                    case FontWeight2.SemiLight: return this.SemiLightState;

                    case FontWeight2.Thin: return this.ThinState;

                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets the fontvweight. </summary>
        public FontWeight2 Weight
        {
            get => (FontWeight2)base.GetValue(FontWeight2Property);
            set => base.SetValue(FontWeight2Property, value);
        }
        /// <summary> Identifies the <see cref = "FontWeightComboBox.Weight" /> dependency property. </summary>
        public static readonly DependencyProperty FontWeight2Property = DependencyProperty.Register(nameof(Weight), typeof(FontWeight2), typeof(FontWeightComboBox), new PropertyMetadata(FontWeight2.Normal, (sender, e) =>
        {
            FontWeightComboBox control = (FontWeightComboBox)sender;

            if (e.NewValue is FontWeight2 value)
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

            this.BlackItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.Black);//Delegate
            this.BoldItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.Bold);//Delegate

            this.ExtraBlackItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.ExtraBlack);//Delegate
            this.ExtraBoldItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.ExtraBold);//Delegate
            this.ExtraLightItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.ExtraLight);//Delegate

            this.LightItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.Light);//Delegate
            this.MediumItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.Medium);//Delegate
            this.NoneItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.Normal);//Delegate

            this.SemiBoldItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.SemiBold);//Delegate
            this.SemiLightItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.SemiLight);//Delegate

            this.ThinItem.Tapped += (s, e) => this.WeightChanged?.Invoke(this, FontWeight2.Thin);//Delegate

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

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