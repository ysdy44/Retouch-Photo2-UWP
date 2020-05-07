using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Characters
{
    /// <summary>
    /// Represents the combo box that is used to select font weight.
    /// </summary>
    public sealed partial class FontWeightComboBox : UserControl
    {
        
        //@Delegate
        public EventHandler<FontWeight> FontWeightChanged;


        //@VisualState
        FontWeight _vsFontWeight;
        public VisualState VisualState
        {
            get
            {
                ushort weight = this._vsFontWeight.Weight;

                if (weight == FontWeights.Black.Weight) return this.Black;
                if (weight == FontWeights.Bold.Weight) return this.Bold;

                if (weight == FontWeights.ExtraBlack.Weight) return this.ExtraBlack;
                if (weight == FontWeights.ExtraBold.Weight) return this.ExtraBold;
                if (weight == FontWeights.ExtraLight.Weight) return this.ExtraLight;
                if (weight == FontWeights.Light.Weight) return this.Light;
                if (weight == FontWeights.Medium.Weight) return this.Medium;
                if (weight == FontWeights.Normal.Weight) return this.None;
                if (weight == FontWeights.SemiBold.Weight) return this.SemiBold;
                if (weight == FontWeights.SemiLight.Weight) return this.SemiLight;
                if (weight == FontWeights.Thin.Weight) return this.Thin;
                return this.Normal;
             }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets the fontvweight. </summary>
        public FontWeight FontWeight2
        {
            get { return (FontWeight)GetValue(FontWeight2Property); }
            set { SetValue(FontWeight2Property, value); }
        }
        /// <summary> Identifies the <see cref = "FontWeightComboBox.FontWeight2" /> dependency property. </summary>
        public static readonly DependencyProperty FontWeight2Property = DependencyProperty.Register(nameof(FontWeight2), typeof(FontWeight), typeof(FontWeightComboBox), new PropertyMetadata(FontWeights.Normal, (sender, e) =>
        {
            FontWeightComboBox con = (FontWeightComboBox)sender;

            if (e.NewValue is FontWeight value)
            {
                con._vsFontWeight = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion



        //@Construct
        public FontWeightComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructButton(this.BlackButton, resource.GetString("/Characters/FontWeight_Black"), FontWeights.Black);
            this.ConstructButton(this.BoldButton, resource.GetString("/Characters/FontWeight_Bold"), FontWeights.Bold);

            this.ConstructButton(this.ExtraBlackButton, resource.GetString("/Characters/FontWeight_ExtraBlack"), FontWeights.ExtraBlack);
            this.ConstructButton(this.ExtraBoldButton, resource.GetString("/Characters/FontWeight_ExtraBold"), FontWeights.ExtraBold);
            this.ConstructButton(this.ExtraLightButton, resource.GetString("/Characters/FontWeight_ExtraLight"), FontWeights.ExtraLight);

            this.ConstructButton(this.LightButton, resource.GetString("/Characters/FontWeight_Light"), FontWeights.Light);
            this.ConstructButton(this.MediumButton, resource.GetString("/Characters/FontWeight_Medium"), FontWeights.Medium);
            this.ConstructButton(this.NormalButton, resource.GetString("/Characters/FontWeight_Normal"), FontWeights.Normal);

            this.ConstructButton(this.SemiBoldButton, resource.GetString("/Characters/FontWeight_SemiBold"), FontWeights.SemiBold);
            this.ConstructButton(this.SemiLightButton, resource.GetString("/Characters/FontWeight_SemiLight"), FontWeights.SemiLight);

            this.ConstructButton(this.ThinButton, resource.GetString("/Characters/FontWeight_Thin"), FontWeights.Thin);
        }

        private void ConstructButton(Button button, string text, FontWeight fontWeight)
        {
            button.Tag = new ContentControl
            {
                Content = fontWeight.Weight,
                Style = this.ContentControlStyle,
            };
            button.Content = text;
            button.Tapped += (s, e) =>
            {
                this.FontWeightChanged?.Invoke(this, fontWeight);//Delegate
            };
        }

    }
}
