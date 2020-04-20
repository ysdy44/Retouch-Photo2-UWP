using Microsoft.Graphics.Canvas.Text;
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

        TextFrameLayer FrameLayer => this.SelectionViewModel.TextFrameLayer;


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

        CharacterState CharacterState
        {
            set
            {
                this.FontFamilyListView.Visibility = (value == CharacterState.FontFamily) ? Visibility.Visible : Visibility.Collapsed;
                this.FontSizeListView.Visibility = (value == CharacterState.FontSize) ? Visibility.Visible : Visibility.Collapsed;
                this.FontWeightListView.Visibility = (value == CharacterState.FontWeight) ? Visibility.Visible : Visibility.Collapsed;
            }
        } 
                     

        //@Construct
        public CharacterMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructToolTip();
            this.ConstructMenu();

            this.ConstructAlign();
            this.ConstructFontStyle();
            this.ConstructFontFamily();
            this.ConstructFontSize();
            this.ConstructFontWeight();
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

            this._button.ToolTip.Content = resource.GetString("/Menus/Character");
            this._Expander.Title = resource.GetString("/Menus/Character");

            this.AlignTextBlock.Text = resource.GetString("/Characters/Align");
            this.LeftToolTip.Content = resource.GetString("/Characters/Align_Left");
            this.CenterToolTip.Content = resource.GetString("/Characters/Align_Center");
            this.RightToolTip.Content = resource.GetString("/Characters/Align_Right");
            this.JustifiedToolTip.Content = resource.GetString("/Characters/Align_Justified");
                        
            this.FontStyleTextBlock.Text = resource.GetString("/Characters/FontStyle");
            this.BoldToolTip.Content = resource.GetString("/Characters/FontStyle_Bold");
            this.ItalicToolTip.Content = resource.GetString("/Characters/FontStyle_Italic");
            this.UnderLineToolTip.Content = resource.GetString("/Characters/FontStyle_UnderLine");
            
            this.FontFamilyTextBlock.Text = resource.GetString("/Characters/FontFamily");

            this.FontSizeTextBlock.Text = resource.GetString("/Characters/FontSize");

            this.FontWeightToolTip.Content = resource.GetString("/Characters/FontWeight");
            this.BlackRun.Text = resource.GetString("/Characters/FontWeight_Black");
            this.BoldRun.Text = resource.GetString("/Characters/FontWeight_Bold");
            this.ExtraBlackRun.Text = resource.GetString("/Characters/FontWeight_ExtraBlack");
            this.ExtraBoldRun.Text = resource.GetString("/Characters/FontWeight_ExtraBold");
            this.ExtraLightRun.Text = resource.GetString("/Characters/FontWeight_ExtraLight");
            this.LightRun.Text = resource.GetString("/Characters/FontWeight_Light");
            this.MediumRun.Text = resource.GetString("/Characters/FontWeight_Medium");
            this.NormalRun.Text = resource.GetString("/Characters/FontWeight_Normal");
            this.SemiBoldRun.Text = resource.GetString("/Characters/FontWeight_SemiBold");
            this.SemiLightRun.Text = resource.GetString("/Characters/FontWeight_SemiLight");
            this.ThinRun.Text = resource.GetString("/Characters/FontWeight_Thin");
        }

        //ToolTip
        private void ConstructToolTip()
        {
            this._button.ToolTip.Opened += (s, e) =>
            {
                if (this._Expander.IsSecondPage) return;

                if (this.State == MenuState.Overlay)
                {
                    this.IsOpen = true;
                }
            };
            this._button.ToolTip.Closed += (s, e) =>
            {
                this.IsOpen = false;
            };
        }

        //@Delegate
        public Action Move { get; set; }
        public Action Closed { get; set; }
        public Action Opened { get; set; }


        //@Content
        public MenuType Type => MenuType.Character;
        public FlyoutPlacementMode PlacementMode { get; set; } = FlyoutPlacementMode.Bottom;
        public Point Postion { get; set; }
        public FrameworkElement Layout => this;
        public FrameworkElement Button => this._button;
        private MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Menus.Icons.CharacterIcon()
        };

        public MenuState State
        {
            get => this.state;
            set
            {
                this._button.State = value;
                this._Expander.State = value;
                MenuHelper.SetMenuState(value, this);
                this.state = value;
            }
        }
        private MenuState state;


        //@Construct  
        public void ConstructMenu()
        {
            this.State = MenuState.Hide;
            this.Button.Tapped += (s, e) => this.State = MenuHelper.GetState(this.State);
            this._Expander.CloseButton.Tapped += (s, e) => this.State = MenuState.Hide;
            this._Expander.StateButton.Tapped += (s, e) => this.State = MenuHelper.GetState2(this.State);
            this._Expander.ResetButton.Visibility = Visibility.Collapsed;
            this._Expander.BackButton.Tapped += (s, e) => this._Expander.IsSecondPage = false;
            MenuHelper.ConstructTitleGrid(this._Expander.TitleGrid, this);
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
            
            this.LeftButton.Tapped += (s, e) =>
            {
                if (this.FrameLayer == null) return;
                this.FrameLayer.HorizontalAlignment = CanvasHorizontalAlignment.Left;
                this.ViewModel.Invalidate();//Invalidate
            };
            this.CenterButton.Tapped += (s, e) => 
            {
                if (this.FrameLayer == null) return;
                this.FrameLayer.HorizontalAlignment = CanvasHorizontalAlignment.Center;
                this.ViewModel.Invalidate();//Invalidate
            };
            this.RightButton.Tapped += (s, e) => 
            {
                if (this.FrameLayer == null) return;
                this.FrameLayer.HorizontalAlignment = CanvasHorizontalAlignment.Right;
                this.ViewModel.Invalidate();//Invalidate
            };

            this.JustifiedButton.Tapped += (s, e) => 
            {
                if (this.FrameLayer == null) return;
                this.FrameLayer.HorizontalAlignment = CanvasHorizontalAlignment.Justified;
                this.ViewModel.Invalidate();//Invalidate
            };

        }

        //FontStyle
        private void ConstructFontStyle()
        {

            this.BoldButton.Tapped += (s, e) =>
            {
                if (this.FrameLayer == null) return;

                //Whether the judgment is small or large.
                bool isBold = this.FrameLayer.FontWeight.Weight > 500;

                // isBold ? ""Normal"" : ""Bold""
                ushort weight = isBold ? (ushort)400 : (ushort)700;

                this.FrameLayer.FontWeight = new FontWeight
                {
                    Weight = weight
                };
                this.ViewModel.Invalidate();//Invalidate
            };

            this.ItalicButton.Tapped += (s, e) =>
            {
                if (this.FrameLayer == null) return;

                //Whether the judgment is Normal or Italic.
                bool isNormal = this.FrameLayer.FontStyle == FontStyle.Normal;
                FontStyle fontStyle = isNormal ? FontStyle.Italic : FontStyle.Normal;

                this.FrameLayer.FontStyle = fontStyle;
                this.ViewModel.Invalidate();//Invalidate
            };

            this.UnderLineButton.Tapped += (s, e) =>
            {
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
                this._Expander.IsSecondPage = true;
            };

            this.FontFamilyListView.ItemClick += (s, e) =>
            {
                if (this.FrameLayer == null) return;

                if (e.ClickedItem is string value)
                {
                    this.SelectionViewModel.TextFontFamily = value;
                    this.FrameLayer.FontFamily = value;
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
                this._Expander.IsSecondPage = true;
            };

            this.FontSizeListView.ItemClick += (s, e) =>
            {
                if (this.FrameLayer == null) return;

                if (e.ClickedItem is int value)
                {
                    this.SelectionViewModel.TextFontSize = value;
                    this.FrameLayer.FontSize = value;
                    this.FontSizePicker.Value = value;
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

            this.FontSizePicker.ValueChange += (s, value) =>
            {
                this.SelectionViewModel.TextFontSize = value;
                this.FrameLayer.FontSize = value;
                this.ViewModel.Invalidate();//Invalidate
            };

        }

        //FontWeight
        private void ConstructFontWeight()
        {

            //   this.FontWeightListView.ItemsSource = this.FontWeights;
            this.FontWeightButton.Tapped += (s, e) =>
            {
                this.CharacterState = CharacterState.FontWeight;
                this._Expander.IsSecondPage = true;
            };

            this.FontWeightListView.ItemClick += (s, e) =>
            {
                if (this.FrameLayer == null) return;

                if (e.ClickedItem is TextBlock textBlock)
                {
                    this.FrameLayer.FontWeight = textBlock.FontWeight;
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

        }

    }
}
