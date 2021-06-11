// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Microsoft.Toolkit.Uwp.UI;
using Retouch_Photo2.Texts;
using Retouch_Photo2.ViewModels;
using System;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI.Text;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Texts"/>.
    /// </summary>
    public sealed partial class TextMenu : MenuExpander
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        public float[] FontSizes => TextsExtensions.FontSizes;
        private IOrderedEnumerable<string> FontFamilies => TextsExtensions.FontFamilies;

        //@Converter
        private float MatchingFontSize(float fontSize) => TextsExtensions.MatchingFontSize(fontSize);
        private bool FontWeightConverter(FontWeight2 fontWeight) => this.MethodViewModel.FontWeightConverter(fontWeight);
        private bool FontStyleConverter(FontStyle fontStyle) => this.MethodViewModel.FontStyleConverter(fontStyle);
        private string Round2Converter(float value) => $"{(float)Math.Round(value, 2)}";

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "TextMenu" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "TextMenu.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TextMenu), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TextMenu. 
        /// </summary>
        public TextMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            base.Loaded += (s, e) => this.ConstructLanguages();

            this.HorizontalAlignmentSegmented.HorizontalAlignmentChanged += (s, alignment) => this.MethodViewModel.MethodSetHorizontalAlignment(alignment);

            this.BoldButton.Tapped += (s, e) => this.MethodViewModel.MethodSetFontWeight();
            this.ItalicButton.Tapped += (s, e) => this.MethodViewModel.MethodSetFontStyle();
            this.UnderlineButton.Tapped += (s, e) => this.MethodViewModel.MethodSetUnderline();

            this.FontWeightComboBox.WeightChanged += (s, fontWeight) => this.MethodViewModel.MethodSetFontWeight(fontWeight);
            this.FontWeightComboBox.Closed += (s, e) => this.SettingViewModel.RegisteKey(); // Setting
            this.FontWeightComboBox.Opened += (s, e) => this.SettingViewModel.UnregisteKey(); // Setting

            // Get all FontFamilys in your device.
            this.FontFamilyButton.Tapped += (s, e) => this.FontFamilyFlyout.ShowAt(this.FontFamilyButton);
            this.FontFamilyListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is string value)
                {
                    this.MethodViewModel.MethodSetFontFamily(value);
                }
            };
            this.FontFamilyFlyout.Closed += (s, e) => this.SettingViewModel.RegisteKey(); // Setting
            this.FontFamilyFlyout.Opened += (s, e) => this.SettingViewModel.UnregisteKey(); // Setting

            // Get fontSizes.
            this.FontSizeButton.Tapped += (s, e) => this.FontSizeFlyout.ShowAt(this.FontSizeButton);
            this.FontSizeFlyout.Closed += (s, e) => this.SettingViewModel.RegisteKey(); // Setting
            this.FontSizeFlyout.Opened += (s, e) => this.SettingViewModel.UnregisteKey(); // Setting

            //@Focus
            TextBoxExtensions.SetDefault(this.FontSizeTextBox, $"{TextsExtensions.DefaultFontSizes}");
            this.FontSizeTextBox.GotFocus += (s, e) => this.SettingViewModel.UnregisteKey();
            this.FontSizeTextBox.LostFocus += (s, e) =>
            {
                this.SettingViewModel.RegisteKey();
                if (this.FontSizeTextBox.Text is string value)
                {
                    if (string.IsNullOrEmpty(value)) return;

                    float size = float.Parse(value);
                    if (size < 1) size = 1;

                    this.MethodViewModel.MethodSetFontSize(size);
                }
            };

            this.FontSizeListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is float value)
                {
                    this.MethodViewModel.MethodSetFontSize(value);
                }
            };

            this.DirectionComboBox.DirectionChanged += (s, direction) => this.MethodViewModel.MethodSetDirection(direction);
        }

        // Languages
        private void ConstructLanguages()
        {
            if (string.IsNullOrEmpty(ApplicationLanguages.PrimaryLanguageOverride) == false)
            {
                if (ApplicationLanguages.PrimaryLanguageOverride != base.Language)
                {
                    this.ConstructStrings();
                }
            }
        }

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.AlignmentTextBlock.Text = resource.GetString("Texts_Alignment");

            this.FontStyleTextBlock.Text = resource.GetString("Texts_FontStyle");
            this.BoldToolTip.Content = resource.GetString("Texts_FontStyle_Bold");
            this.ItalicToolTip.Content = resource.GetString("Texts_FontStyle_Italic");
            this.UnderlineToolTip.Content = resource.GetString("Texts_FontStyle_Underline");

            this.FontWeightTextBlock.Text = resource.GetString("Texts_FontWeight");

            this.FontFamilyTextBlock.Text = resource.GetString("Texts_FontFamily");
            this.FontFamilySettingButton.Content = resource.GetString("Texts_FontFamilySetting");

            this.FontSizeTextBlock.Text = resource.GetString("Texts_FontSize");

            this.DirectionTextBlock.Text = resource.GetString("Texts_Direction");
        }
    }
}