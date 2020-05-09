using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Elements;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

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

            this.Button.ToolTip.Content = resource.GetString("/Menus/Character");
            this.Expander.Title = resource.GetString("/Menus/Character");

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
            this.Button.ToolTip.Opened += (s, e) =>
            {
                if (this.Expander.IsSecondPage==false)
                {
                    if (this.Expander.State == ExpanderState.Overlay)
                    {
                        this.IsOpen = true;
                        this.FontAlignmentSegmented.IsOpen = true;
                    }
                }
            };
            this.Button.ToolTip.Closed += (s, e) =>
            {
                this.IsOpen = false;
                this.FontAlignmentSegmented.IsOpen = false;
            };
        }

        //Menu
        public MenuType Type => MenuType.Character;
        public IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Characters.Icon()
        };
        public IExpander Expander => this._Expander;
        public ExpanderState State
        {
            set
            {
                this.Button.State = value;
                this.Expander.State = value;
            }
        }
        public FrameworkElement Self => this;

        public void ConstructMenu()
        {
            this._Expander.Button = this.Button.Self;

            this.Button.StateChanged += (state) => this.State = state;
            this.Expander.StateChanged += (state) => this.State = state;
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "CharacterMenu" />. 
    /// </summary>
    public sealed partial class CharacterMenu : UserControl, IMenu
    {
        
        //Align
        private void ConstructAlign()
        {
            this.FontAlignmentSegmented.AlignmentChanged += (s, fontAlignment) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer.Type == LayerType.TextArtistic || layer.Type == LayerType.TextFrame)
                    {
                        ITextLayer textLayer = (ITextLayer)layer;
                        textLayer.FontAlignment = fontAlignment;
                    }
                });
                this.SelectionViewModel.FontAlignment = fontAlignment;

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //FontStyle
        private void ConstructFontStyle()
        {
            this.BoldButton.Tapped += (s, e) =>
            {
                //Whether the judgment is small or large.
                bool isBold = this.SelectionViewModel.FontWeight.Weight == FontWeights.Bold.Weight;
                // isBold ? ""Normal"" : ""Bold""
                FontWeight fontWeight = isBold ? FontWeights.Normal : FontWeights.Bold;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer.Type == LayerType.TextArtistic || layer.Type == LayerType.TextFrame)
                    {
                        ITextLayer textLayer = (ITextLayer)layer;
                        textLayer.FontWeight = fontWeight;
                    }
                });
                this.SelectionViewModel.FontWeight = fontWeight;

                this.ViewModel.Invalidate();//Invalidate
            };

            this.ItalicButton.Tapped += (s, e) =>
            {
                //Whether the judgment is Normal or Italic.
                bool isNormal = this.SelectionViewModel.FontStyle == FontStyle.Normal;
                // isNormal ? ""Italic"" : ""Normal""
                FontStyle fontStyle = isNormal ? FontStyle.Italic : FontStyle.Normal;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer.Type == LayerType.TextArtistic || layer.Type == LayerType.TextFrame)
                    {
                        ITextLayer textLayer = (ITextLayer)layer;
                        textLayer.FontStyle = fontStyle;
                    }
                });
                this.SelectionViewModel.FontStyle = fontStyle;

                this.ViewModel.Invalidate();//Invalidate
            };

            this.UnderLineButton.Tapped += (s, e) =>
            {
            };
        }


        //FontWeight
        private void ConstructFontWeight()
        {
            this.FontWeightButton.Tapped += (s, e) =>
            {
                this.CharacterState = CharacterState.FontWeight;
                this.Expander.IsSecondPage = true;
            };

            this.FontWeightControl.WeightChanged += (s, fontWeight) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer.Type == LayerType.TextArtistic || layer.Type == LayerType.TextFrame)
                    {
                        ITextLayer textLayer = (ITextLayer)layer;
                        textLayer.FontWeight = fontWeight;
                    }
                });
                this.SelectionViewModel.FontWeight = fontWeight;

                this.ViewModel.Invalidate();//Invalidate
            };

        }


        //FontFamily
        private void ConstructFontFamily()
        {
            // Get all FontFamilys in your device.
            IOrderedEnumerable<string> fontFamilys = CanvasTextFormat.GetSystemFontFamilies(ApplicationLanguages.Languages).OrderBy(k => k);
            this.FontFamilyListView.ItemsSource = fontFamilys;

            this.FontFamilyButton.Tapped += (s, e) =>
            {
                this.CharacterState = CharacterState.FontFamily;
                this.Expander.IsSecondPage = true;
            };
            
            this.FontFamilyListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is string value)
                {
                    //Selection
                    this.SelectionViewModel.SetValue((layer) =>
                    {
                        if (layer.Type == LayerType.TextArtistic || layer.Type == LayerType.TextFrame)
                        {
                            ITextLayer textLayer = (ITextLayer)layer;
                            textLayer.FontFamily = value;
                        }
                    });
                    this.SelectionViewModel.FontFamily = value;

                    this.ViewModel.Invalidate();//Invalidate
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

            this.FontSizeButton.Tapped += (s, e) =>
            {
                this.CharacterState = CharacterState.FontSize;
                this.Expander.IsSecondPage = true;
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
        private void SetFontSize(float value)
        {
            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer.Type == LayerType.TextArtistic || layer.Type == LayerType.TextFrame)
                {
                    ITextLayer textLayer = (ITextLayer)layer;
                    textLayer.FontSize = value;
                }
            });
            this.SelectionViewModel.FontSize = value;

            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();//Refactoring
            this.ViewModel.Invalidate();//Invalidate
        }

    }
}
