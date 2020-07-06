using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
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

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        TextMainPage TextMainPage = new TextMainPage();


        //@Construct
        /// <summary>
        /// Initializes a TextMenu. 
        /// </summary>
        public TextMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.MainPage = this.TextMainPage;
            this.TextMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.SecondPage != secondPage) this.SecondPage = secondPage;
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
            this.Title = resource.GetString("/Menus/Text");

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
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Delegate
        /// <summary> Occurs when second-page change. </summary>
        public event EventHandler<UIElement> SecondPageChanged;


        //@Content
        /// <summary> FontWeight ComboBox. </summary>
        public FontWeightComboBox FontWeightComboBox { get; } = new FontWeightComboBox();
        /// <summary> FontFamily ListView. </summary>
        public ListView FontFamilyListView { get; private set; }
        /// <summary> FontSize ListView. </summary>
        public ListView FontSizeListView { get; private set; }
        
        //@Converter
        private int FontSizeConverter(float fontSize) => (int)fontSize;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "TextMainPage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
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

            this.FontAlignmentTextBlock.Text = resource.GetString("/Texts/FontAlignment");

            this.FontStyleTextBlock.Text = resource.GetString("/Texts/FontStyle");
            this.BoldToolTip.Content = resource.GetString("/Texts/FontStyle_Bold");
            this.ItalicToolTip.Content = resource.GetString("/Texts/FontStyle_Italic");
            this.UnderLineToolTip.Content = resource.GetString("/Texts/FontStyle_UnderLine");

            this.FontWeightTextBlock.Text = resource.GetString("/Texts/FontWeight");

            this.FontFamilyTextBlock.Text = resource.GetString("/Texts/FontFamily");

            this.FontSizeTextBlock.Text = resource.GetString("/Texts/FontSize");
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
            this.FontFamilyListView = new ListView
            {
                IsItemClickEnabled = true,
                SelectionMode = ListViewSelectionMode.Single,

                ItemTemplate = this.FontFamilyDataTemplate,
             
                // Get all FontFamilys in your device.
                ItemsSource = CanvasTextFormat.GetSystemFontFamilies(ApplicationLanguages.Languages).OrderBy(k => k)
            };

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
            this.FontSizeListView = new ListView
            {
                IsItemClickEnabled = true,
                SelectionMode = ListViewSelectionMode.Single,

                ItemTemplate = this.FontSizeDataTemplate,

                // Get all fontSizes in your device.
                ItemsSource = new List<int>
                {
                     5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 24, 30, 36, 48, 64, 72, 96, 144, 288,
                }
            };

            this.FontSizeButton.Click += (s, e) =>
            {
                string title = this.FontSizeTextBlock.Text;
                UIElement secondPage = this.FontSizeListView;
                this.SecondPageChanged?.Invoke(title, secondPage);//Delegate
            };

            this.FontSizePicker.ValueChange += (s, value) => this.SetFontSize(value);
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
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set font alignment");

            //Selection
            this.SelectionViewModel.FontAlignment = fontAlignment;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = textLayer.FontAlignment;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        textLayer.FontAlignment = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    textLayer.FontAlignment = fontAlignment;
                }
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }


        private void SetFontWeight(FontWeight fontWeight)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set font weight");

            //Selection
            this.SelectionViewModel.FontWeight = fontWeight;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = textLayer.FontWeight;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        textLayer.FontWeight = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    textLayer.FontWeight = fontWeight;
                }
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }


        private void SetFontStyle(FontStyle fontStyle)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set font style");

            //Selection
            this.SelectionViewModel.FontStyle = fontStyle;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = textLayer.FontStyle;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        textLayer.FontStyle = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    textLayer.FontStyle = fontStyle;
                }
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }


        private void SetFontFamily(string fontFamily)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set font family");

            //Selection
            this.SelectionViewModel.FontFamily = fontFamily;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = textLayer.FontFamily;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        textLayer.FontFamily = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    textLayer.FontFamily = fontFamily;
                }
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }


        private void SetFontSize(float value)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set font size");

            //Selection
            this.SelectionViewModel.FontSize = value;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = textLayer.FontSize;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        textLayer.FontSize = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    textLayer.FontSize = value;
                }
            });
            //Refactoring
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}