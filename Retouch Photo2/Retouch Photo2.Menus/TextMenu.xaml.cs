// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Texts;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Texts"/>.
    /// </summary>
    public sealed partial class TextMenu : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private bool FontWeightConverter(FontWeight2 fontWeight)
        {
            switch (fontWeight)
            {
                case FontWeight2.Black:
                case FontWeight2.Bold:
                case FontWeight2.ExtraBlack:
                case FontWeight2.ExtraBold:
                case FontWeight2.SemiBold:
                    return true;
                default:
                    return false;
            }
        }
        private bool FontStyleConverter(FontStyle fontStyle) => fontStyle == FontStyle.Italic;
        private int FontSizeConverter(float fontSize) => (int)fontSize;


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

            this.HorizontalAlignmentSegmented.HorizontalAlignmentChanged += (s, alignment) => this.SetHorizontalAlignment(alignment);

            this.BoldButton.Click += (s, e) =>
            {
                // isBold ? ""Normal"" : ""Bold""
                bool isBold = this.FontWeightConverter(this.SelectionViewModel.FontWeight);
                FontWeight2 fontWeight = isBold ? FontWeight2.Normal : FontWeight2.Bold;

                this.SetFontWeight(fontWeight);
            };
            this.ItalicButton.Click += (s, e) =>
            {
                // isNormal ? ""Normal"" : ""Italic""
                bool isNormal = this.FontStyleConverter(this.SelectionViewModel.FontStyle);
                FontStyle fontStyle = isNormal ? FontStyle.Normal : FontStyle.Italic;

                this.SetFontStyle(fontStyle);
            };
            this.UnderlineButton.Click += (s, e) => this.SetUnderline(!this.SelectionViewModel.Underline);

            this.FontWeightComboBox.WeightChanged += (s, fontWeight) => this.SetFontWeight(fontWeight);

            // Get all FontFamilys in your device.
            this.FontFamilyListView.ItemsSource = CanvasTextFormat.GetSystemFontFamilies(ApplicationLanguages.Languages).OrderBy(k => k);
            this.FontFamilyButton.Click += (s, e) =>
            {
                this.FontFamilyListView.Visibility = Visibility.Visible;
                this.SplitView.IsPaneOpen = false;
            };
            this.FontFamilyListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is string value)
                {
                    this.SetFontFamily(value);
                }
            };

            // Get all fontSizes in your device.
            this.FontSizeListView.ItemsSource = new List<int>
            {
                5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 24, 30, 36, 48, 64, 72, 96, 144, 288,
            };
            this.FontSizePicker.ValueChanged += (s, value) => this.SetFontSize(value);
            this.FontSizeListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is int value)
                {
                    this.FontSizePicker.Value = value;

                    this.SetFontSize(value);
                }
            };

            base.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.SplitView.OpenPaneLength = e.NewSize.Width;
            };
            this.CloseButton.Click += (s, e) =>
            {
                this.FontFamilyListView.Visibility = Visibility.Collapsed;
                this.SplitView.IsPaneOpen = true;
            };
        }
    }

    public sealed partial class TextMenu : UserControl
    {

        //Strings
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

            this.FontSizeTextBlock.Text = resource.GetString("Texts_FontSize");

            this.CloseButton.Content = resource.GetString("Menus_Close");
        }


        private void SetHorizontalAlignment(CanvasHorizontalAlignment horizontalAlignment)
        {
            this.SelectionViewModel.HorizontalAlignment = horizontalAlignment;
            this.MethodViewModel.ITextLayerChanged<CanvasHorizontalAlignment>
            (
                set: (textLayer) => textLayer.HorizontalAlignment = horizontalAlignment,

                type: HistoryType.LayersProperty_SetHorizontalAlignment,
                getUndo: (textLayer) => textLayer.HorizontalAlignment,
                setUndo: (textLayer, previous) => textLayer.HorizontalAlignment = previous
           );
        }


        private void SetFontWeight(FontWeight2 fontWeight)
        {
            this.SelectionViewModel.FontWeight = fontWeight;
            this.MethodViewModel.ITextLayerChanged<FontWeight2>
            (
                set: (textLayer) => textLayer.FontWeight = fontWeight,

                type: HistoryType.LayersProperty_SetFontWeight,
                getUndo: (textLayer) => textLayer.FontWeight,
                setUndo: (textLayer, previous) => textLayer.FontWeight = previous
           );
        }

        private void SetFontStyle(FontStyle fontStyle)
        {
            this.SelectionViewModel.FontStyle = fontStyle;
            this.MethodViewModel.ITextLayerChanged<FontStyle>
            (
                set: (textLayer) => textLayer.FontStyle = fontStyle,

                type: HistoryType.LayersProperty_SetFontStyle,
                getUndo: (textLayer) => textLayer.FontStyle,
                setUndo: (textLayer, previous) => textLayer.FontStyle = previous
           );
        }

        private void SetUnderline(bool underline)
        {
            this.SelectionViewModel.Underline = underline;
            this.MethodViewModel.ITextLayerChanged<bool>
            (
                set: (textLayer) => textLayer.Underline = underline,

                type: HistoryType.LayersProperty_SetUnderline,
                getUndo: (textLayer) => textLayer.Underline,
                setUndo: (textLayer, previous) => textLayer.Underline = previous
           );
        }


        private void SetFontFamily(string fontFamily)
        {
            this.SelectionViewModel.FontFamily = fontFamily;
            this.MethodViewModel.ITextLayerChanged<string>
            (
                set: (textLayer) => textLayer.FontFamily = fontFamily,

                type: HistoryType.LayersProperty_SetFontFamily,
                getUndo: (textLayer) => textLayer.FontFamily,
                setUndo: (textLayer, previous) => textLayer.FontFamily = previous
           );
        }


        private void SetFontSize(float fontSize)
        {
            this.SelectionViewModel.FontSize = fontSize;
            this.MethodViewModel.ITextLayerChanged<float>
            (
                set: (textLayer) => textLayer.FontSize = fontSize,

                type: HistoryType.LayersProperty_SetFontSize,
                getUndo: (textLayer) => textLayer.FontSize,
                setUndo: (textLayer, previous) => textLayer.FontSize = previous
           );

            //Refactoring
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }

    }
}