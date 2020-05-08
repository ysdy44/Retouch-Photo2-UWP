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
        public EventHandler<FontWeight> WeightChanged;

        //@Group
        private EventHandler<FontWeight> Group;

        #region DependencyProperty


        /// <summary> Gets or sets the fontvweight. </summary>
        public FontWeight Weight
        {
            get { return (FontWeight)GetValue(FontWeight2Property); }
            set { SetValue(FontWeight2Property, value); }
        }
        /// <summary> Identifies the <see cref = "FontWeightComboBox.Weight" /> dependency property. </summary>
        public static readonly DependencyProperty FontWeight2Property = DependencyProperty.Register(nameof(Weight), typeof(FontWeight), typeof(FontWeightComboBox), new PropertyMetadata(FontWeights.Normal, (sender, e) =>
        {
            FontWeightComboBox con = (FontWeightComboBox)sender;

            if (e.NewValue is FontWeight value)
            {
                con.Group?.Invoke(con, value);//Delegate
            }
        }));


        /// <summary> Gets or sets the title. </summary>
        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary> Identifies the <see cref = "FontWeightComboBox.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(object), typeof(FontWeightComboBox), new PropertyMetadata(null));


        #endregion


        //@Construct
        public FontWeightComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Represents the combo box that is used to select font weight.
    /// </summary>
    public sealed partial class FontWeightComboBox : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructGroup(this.BlackButton, resource.GetString("/Characters/FontWeight_Black"), FontWeights.Black);
            this.ConstructGroup(this.BoldButton, resource.GetString("/Characters/FontWeight_Bold"), FontWeights.Bold);

            this.ConstructGroup(this.ExtraBlackButton, resource.GetString("/Characters/FontWeight_ExtraBlack"), FontWeights.ExtraBlack);
            this.ConstructGroup(this.ExtraBoldButton, resource.GetString("/Characters/FontWeight_ExtraBold"), FontWeights.ExtraBold);
            this.ConstructGroup(this.ExtraLightButton, resource.GetString("/Characters/FontWeight_ExtraLight"), FontWeights.ExtraLight);

            this.ConstructGroup(this.LightButton, resource.GetString("/Characters/FontWeight_Light"), FontWeights.Light);
            this.ConstructGroup(this.MediumButton, resource.GetString("/Characters/FontWeight_Medium"), FontWeights.Medium);
            this.ConstructGroup(this.NormalButton, resource.GetString("/Characters/FontWeight_Normal"), FontWeights.Normal);

            this.ConstructGroup(this.SemiBoldButton, resource.GetString("/Characters/FontWeight_SemiBold"), FontWeights.SemiBold);
            this.ConstructGroup(this.SemiLightButton, resource.GetString("/Characters/FontWeight_SemiLight"), FontWeights.SemiLight);

            this.ConstructGroup(this.ThinButton, resource.GetString("/Characters/FontWeight_Thin"), FontWeights.Thin);
        }

        //Group
        private void ConstructGroup(Button button, string text, FontWeight weight)
        {
            void group(FontWeight groupWeight)
            {
                if (groupWeight.Weight == weight.Weight)
                {
                    button.IsEnabled = false;

                    this.Title = text;
                }
                else button.IsEnabled = true;
            }

            //NoneButton
            group(this.Weight);

            //Buttons
            button.Content = text;
            button.Tag = new ContentControl
            {
                Content = weight.Weight,
                Style = this.ContentControlStyle
            };
            button.Tapped += (s, e) => this.WeightChanged?.Invoke(this, weight);//Delegate

            //Group
            this.Group += (s, e) => group(e);
        }

    }
}
