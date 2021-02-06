// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Texts;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Texts"/>.
    /// </summary>
    public sealed partial class TextMenu : Expander, IMenu
    {

        //@Content     
        public override UIElement MainPage => this.TextMainPage;

        readonly TextMainPage TextMainPage = new TextMainPage();


        //@Construct
        /// <summary>
        /// Initializes a TextMenu. 
        /// </summary>
        public TextMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.TextMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.Page != secondPage) this.Page = secondPage;
                this.IsSecondPage = true;
                this.Title = (string)title;
            };
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Texts"/>.
    /// </summary>
    public sealed partial class TextMenu : Expander, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = resource.GetString("Menus_Text");

            this.Button.ToolTip.Closed += (s, e) => this.TextMainPage.IsOpen = false;
            this.Button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSecondPage) return;
                if (this.State != ExpanderState.Overlay) return;

                this.TextMainPage.IsOpen = true;
            };
        }

        //Menu  
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Text;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Texts.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }

    /// <summary>
    /// MainPage of <see cref = "TextMenu"/>.
    /// </summary>
    public sealed partial class TextMainPage : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Delegate
        /// <summary> Occurs when second-page change. </summary>
        public event EventHandler<UIElement> SecondPageChanged;


        //@Content
        /// <summary> FontWeight ComboBox. </summary>
        public FontWeightComboBox FontWeightComboBox { get; } = new FontWeightComboBox
        {
            MinHeight = 165,
            MaxHeight = 300
        };
        /// <summary> FontFamily ListView. </summary>
        public ListView FontFamilyListView { get; } = new ListView
        {
            MinHeight = 165,
            MaxHeight = 300
        };
        /// <summary> FontSize ListView. </summary>
        public ListView FontSizeListView { get; } = new ListView
        {
            MinHeight = 165,
            MaxHeight = 300
        };

        //@Converter
        private int FontSizeConverter(float fontSize) => (int)fontSize;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "TextMainPage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "TextMainPage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TextMainPage), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TextMainPage. 
        /// </summary>
        public TextMainPage()
        {
            this.InitializeComponent();
            this.ConstructFontWeightDataContext
            (
                 dataContext: this.SelectionViewModel,
                 path: nameof(this.SelectionViewModel.FontWeight),
                 dp: FontWeightComboBox.FontWeight2Property
            );
            this.ConstructStrings();

            this.ConstructAlign();
            this.ConstructFontStyle();
            this.ConstructFontWeight();
            this.ConstructFontFamily();
            this.ConstructFontSize();
        }
    }

    /// <summary>
    /// MainPage of <see cref = "TextMenu"/>.
    /// </summary>
    public sealed partial class TextMainPage : UserControl
    {

        //DataContext
        private void ConstructFontWeightDataContext(object dataContext, string path, DependencyProperty dp)
        {
            this.FontWeightComboBox.DataContext = dataContext;

            // Create the binding description.
            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(path)
            };

            // Attach the binding to the target.
            this.FontWeightComboBox.SetBinding(dp, binding);
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FontAlignmentTextBlock.Text = resource.GetString("Texts_FontAlignment");

            this.FontStyleTextBlock.Text = resource.GetString("Texts_FontStyle");
            this.BoldToolTip.Content = resource.GetString("Texts_FontStyle_Bold");
            this.ItalicToolTip.Content = resource.GetString("Texts_FontStyle_Italic");
            this.UnderLineToolTip.Content = resource.GetString("Texts_FontStyle_UnderLine");

            this.FontWeightTextBlock.Text = resource.GetString("Texts_FontWeight");

            this.FontFamilyTextBlock.Text = resource.GetString("Texts_FontFamily");

            this.FontSizeTextBlock.Text = resource.GetString("Texts_FontSize");
        }

    }

    /// <summary>
    /// MainPage of <see cref = "TextMenu"/>.
    /// </summary>
    public sealed partial class TextMainPage : UserControl
    {

        //FontAlignment
        private void ConstructAlign()
        {
            this.FontAlignmentSegmented.AlignmentChanged += (s, fontAlignment) =>
            {
                this.SetFontAlignment(fontAlignment);
            };
        }


        //FontStyle
        private void ConstructFontStyle()
        {
            this.BoldButton.Click += (s, e) =>
            {
                //Whether the judgment is small or large.
                bool isBold = this.SelectionViewModel.FontWeight.Weight == FontWeights.Bold.Weight;
                // isBold ? ""Normal"" : ""Bold""
                FontWeight fontWeight = isBold ? FontWeights.Normal : FontWeights.Bold;

                this.SetFontWeight(fontWeight);
            };

            this.ItalicButton.Click += (s, e) =>
            {
                //Whether the judgment is Normal or Italic.
                bool isNormal = this.SelectionViewModel.FontStyle == FontStyle.Normal;
                // isNormal ? ""Italic"" : ""Normal""
                FontStyle fontStyle = isNormal ? FontStyle.Italic : FontStyle.Normal;

                this.SetFontStyle(fontStyle);
            };

            this.UnderLineButton.Click += (s, e) =>
            {
            };
        }


        //FontWeight
        private void ConstructFontWeight()
        {
            this.FontWeightButton.Click += (s, e) =>
            {
                string title = this.FontWeightTextBlock.Text;
                UIElement secondPage = this.FontWeightComboBox;
                this.SecondPageChanged?.Invoke(title, secondPage);//Delegate
            };

            this.FontWeightComboBox.WeightChanged += (s, fontWeight) => this.SetFontWeight(fontWeight);
        }


        //FontFamily
        private void ConstructFontFamily()
        {
            this.FontFamilyListView.IsItemClickEnabled = true;
            this.FontFamilyListView.SelectionMode = ListViewSelectionMode.Single;

            this.FontFamilyListView.ItemTemplate = this.FontFamilyDataTemplate;

            // Get all FontFamilys in your device.
            this.FontFamilyListView.ItemsSource = CanvasTextFormat.GetSystemFontFamilies(ApplicationLanguages.Languages).OrderBy(k => k);

            this.FontFamilyButton.Click += (s, e) =>
            {
                string title = this.FontFamilyTextBlock.Text;
                UIElement secondPage = this.FontFamilyListView;
                this.SecondPageChanged?.Invoke(title, secondPage);//Delegate
            };

            this.FontFamilyListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is string value)
                {
                    this.SetFontFamily(value);
                }
            };
        }


        //FontSize
        private void ConstructFontSize()
        {
            this.FontSizeListView.IsItemClickEnabled = true;
            this.FontSizeListView.SelectionMode = ListViewSelectionMode.Single;

            this.FontSizeListView.ItemTemplate = this.FontSizeDataTemplate;

            // Get all fontSizes in your device.
            this.FontSizeListView.ItemsSource = new List<int>
                {
                     5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 24, 30, 36, 48, 64, 72, 96, 144, 288,
                };

            this.FontSizeButton.Click += (s, e) =>
            {
                string title = this.FontSizeTextBlock.Text;
                UIElement secondPage = this.FontSizeListView;
                this.SecondPageChanged?.Invoke(title, secondPage);//Delegate
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
        }
    }


    /// <summary>
    /// MainPage of <see cref = "TextMenu"/>.
    /// </summary>
    public sealed partial class TextMainPage : UserControl
    {

        private void SetFontAlignment(CanvasHorizontalAlignment fontAlignment)
        {
            this.SelectionViewModel.FontAlignment = fontAlignment;
            this.MethodViewModel.ITextLayerChanged<CanvasHorizontalAlignment>
            (
                set: (textLayer) => textLayer.FontAlignment = fontAlignment,

                historyTitle: "Set font alignment",
                getHistory: (textLayer) => textLayer.FontAlignment,
                setHistory: (textLayer, previous) => textLayer.FontAlignment = previous
           );
        }


        private void SetFontWeight(FontWeight fontWeight)
        {
            this.SelectionViewModel.FontWeight = fontWeight;
            this.MethodViewModel.ITextLayerChanged<FontWeight>
            (
                set: (textLayer) => textLayer.FontWeight = fontWeight,

                historyTitle: "Set font weight",
                getHistory: (textLayer) => textLayer.FontWeight,
                setHistory: (textLayer, previous) => textLayer.FontWeight = previous
           );
        }


        private void SetFontStyle(FontStyle fontStyle)
        {
            this.SelectionViewModel.FontStyle = fontStyle;
            this.MethodViewModel.ITextLayerChanged<FontStyle>
            (
                set: (textLayer) => textLayer.FontStyle = fontStyle,

                historyTitle: "Set font style",
                getHistory: (textLayer) => textLayer.FontStyle,
                setHistory: (textLayer, previous) => textLayer.FontStyle = previous
           );
        }


        private void SetFontFamily(string fontFamily)
        {
            this.SelectionViewModel.FontFamily = fontFamily;
            this.MethodViewModel.ITextLayerChanged<string>
            (
                set: (textLayer) => textLayer.FontFamily = fontFamily,

                historyTitle: "Set font family",
                getHistory: (textLayer) => textLayer.FontFamily,
                setHistory: (textLayer, previous) => textLayer.FontFamily = previous
           );
        }


        private void SetFontSize(float fontSize)
        {
            this.SelectionViewModel.FontSize = fontSize;
            this.MethodViewModel.ITextLayerChanged<float>
            (
                set: (textLayer) => textLayer.FontSize = fontSize,

                historyTitle: "Set font size",
                getHistory: (textLayer) => textLayer.FontSize,
                setHistory: (textLayer, previous) => textLayer.FontSize = previous
           );

            //Refactoring
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }

    }
}