using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Globalization;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus.Models
{
    internal enum CharacterState
    {
        None,
        FontFamily,
        FontSize,
        FontWeight,
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "CharacterMenu" />. 
    /// </summary>
    public sealed partial class CharacterMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private int FontSizeConverter(float fontSize) => (int)fontSize;

        CharacterState CharacterState
        {
            set
            {
                this.FontFamilyListView.Visibility = (value == CharacterState.FontFamily) ? Visibility.Visible : Visibility.Collapsed;
                this.FontSizeListView.Visibility = (value == CharacterState.FontSize) ? Visibility.Visible : Visibility.Collapsed;
                this.FontWeightScrollViewer.Visibility = (value == CharacterState.FontWeight) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "OperateMenu" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OperateMenu.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(CharacterMenu), new PropertyMetadata(false));


        #endregion


        //@Construct
        public CharacterMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructToolTip();
            this.ConstructMenu();

            this.ConstructAlign();
            this.ConstructFontStyle();
            this.ConstructFontWeight();
            this.ConstructFontFamily();
            this.ConstructFontSize();
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "CharacterMenu" />. 
    /// </summary>
    public sealed partial class CharacterMenu : UserControl, IMenu
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/Character");

            this.FontAlignmentTextBlock.Text = resource.GetString("/Characters/FontAlignment");

            this.FontStyleTextBlock.Text = resource.GetString("/Characters/FontStyle");
            this.BoldToolTip.Content = resource.GetString("/Characters/FontStyle_Bold");
            this.ItalicToolTip.Content = resource.GetString("/Characters/FontStyle_Italic");
            this.UnderLineToolTip.Content = resource.GetString("/Characters/FontStyle_UnderLine");

            this.FontWeightTextBlock.Text = resource.GetString("/Characters/FontWeight");

            this.FontFamilyTextBlock.Text = resource.GetString("/Characters/FontFamily");

            this.FontSizeTextBlock.Text = resource.GetString("/Characters/FontSize");
        }

        //ToolTip
        private void ConstructToolTip()
        {
            this._button.ToolTip.Opened += (s, e) =>
            {
                if (this._Expander.IsSecondPage == false)
                {
                    if (this.Expander.State == ExpanderState.Overlay)
                    {
                        this.IsOpen = true;
                        this.FontAlignmentSegmented.IsOpen = true;
                    }
                }
            };
            this._button.ToolTip.Closed += (s, e) =>
            {
                this.IsOpen = false;
                this.FontAlignmentSegmented.IsOpen = false;
            };
        }

        //Menu  
        public MenuType Type => MenuType.Character;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Characters.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "CharacterMenu" />. 
    /// </summary>
    public sealed partial class CharacterMenu : UserControl, IMenu
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
                this.CharacterState = CharacterState.FontWeight;
                this._Expander.IsSecondPage = true;
                this._Expander.CurrentTitle = this.FontWeightTextBlock.Text;
            };

            this.FontWeightControl.WeightChanged += (s, fontWeight) => this.SetFontWeight(fontWeight);
        }


        //FontFamily
        private void ConstructFontFamily()
        {
            // Get all FontFamilys in your device.
            IOrderedEnumerable<string> fontFamilys = CanvasTextFormat.GetSystemFontFamilies(ApplicationLanguages.Languages).OrderBy(k => k);
            this.FontFamilyListView.ItemsSource = fontFamilys;

            this.FontFamilyButton.Click += (s, e) =>
            {
                this.CharacterState = CharacterState.FontFamily;
                this._Expander.IsSecondPage = true;
                this._Expander.CurrentTitle = this.FontFamilyTextBlock.Text;
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
            // Get all fontSizes in your device.
            List<int> fontSizes = new List<int>
            {
                5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 24, 30, 36, 48, 64, 72, 96, 144, 288,
            };
            this.FontSizeListView.ItemsSource = fontSizes;

            this.FontSizeButton.Click += (s, e) =>
            {
                this.CharacterState = CharacterState.FontSize;
                this._Expander.IsSecondPage = true;
                this._Expander.CurrentTitle = this.FontSizeTextBlock.Text;
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
    /// Retouch_Photo2's the only <see cref = "CharacterMenu" />. 
    /// </summary>
    public sealed partial class CharacterMenu : UserControl, IMenu
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
                    history.UndoActions.Push(() =>
                    {
                        ITextLayer layer2 = textLayer;

                        layer2.FontAlignment = previous;
                    });

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
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = textLayer.FontWeight;
                    history.UndoActions.Push(() =>
                    {
                        ITextLayer layer2 = textLayer;

                        layer2.FontWeight = previous;
                    });

                    textLayer.FontWeight = fontWeight;
                }
            });
            this.SelectionViewModel.FontWeight = fontWeight;

            this.ViewModel.Invalidate();//Invalidate
        }


        private void SetFontStyle(FontStyle fontStyle)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set font style");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = textLayer.FontStyle;
                    history.UndoActions.Push(() =>
                    {
                        ITextLayer layer2 = textLayer;

                        layer2.FontStyle = previous;
                    });

                    textLayer.FontStyle = fontStyle;
                }
            });
            this.SelectionViewModel.FontStyle = fontStyle;

            this.ViewModel.Invalidate();//Invalidate
        }


        private void SetFontFamily(string fontFamily)
        {  //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set font family");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = textLayer.FontFamily;
                    history.UndoActions.Push(() =>
                    {
                        ITextLayer layer2 = textLayer;

                        layer2.FontFamily = previous;
                    });

                    textLayer.FontFamily = fontFamily;
                }
            });
            this.SelectionViewModel.FontFamily = fontFamily;

            this.ViewModel.Invalidate();//Invalidate
        }


        private void SetFontSize(float value)
        {
            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set font size");

                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = textLayer.FontSize;
                    history.UndoActions.Push(() =>
                    {
                        ITextLayer layer2 = textLayer;

                        layer2.FontSize = previous;
                    });

                    textLayer.FontSize = value;
                }
            });
            this.SelectionViewModel.FontSize = value;

            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();//Refactoring
            this.ViewModel.Invalidate();//Invalidate
        }

    }
}