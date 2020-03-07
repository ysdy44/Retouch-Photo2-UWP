using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Text;

namespace Retouch_Photo2.Controls
{
    internal struct FontWeightName
    {
        public string Name;
        public FontWeight FontWeight;
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "CharacterControl" />. 
    /// </summary>
    public sealed partial class CharacterControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        TextFrameLayer FrameLayer => this.SelectionViewModel.TextFrameLayer;

        //@VisualState
        public bool _vsSelectFontFamily;
        public bool _vsSelectFontSize;
        public bool _vsSelectFontWeight;        
        public VisualState VisualState
        {
            get
            {
                if (this._vsSelectFontFamily) return this.SelectFontFamily;
                if (this._vsSelectFontSize) return this.SelectFontSize;
                if (this._vsSelectFontWeight) return this.SelectFontWeight;
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region FontFamily, FontSize, FontWeight

        IOrderedEnumerable<string> FontFamilyNames = CanvasTextFormat.GetSystemFontFamilies(ApplicationLanguages.Languages).OrderBy(k => k);
        List<int> FontSizes = new List<int>
        {
            5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 24, 30, 36, 48, 64, 72, 96, 144, 288,
        };
        List<FontWeightName> FontWeights = new List<FontWeightName>
        {
            new FontWeightName { Name="Black", FontWeight= new FontWeight{ Weight = 900 } },
            new FontWeightName { Name="Bold", FontWeight= new FontWeight{ Weight = 700 } },
            new FontWeightName { Name="ExtraBlack", FontWeight= new FontWeight{ Weight = 900 } },
            new FontWeightName { Name="ExtraBold", FontWeight= new FontWeight{ Weight = 900 } },
            new FontWeightName { Name="ExtraLight", FontWeight= new FontWeight{ Weight = 300 } },
            new FontWeightName { Name="Light", FontWeight= new FontWeight{ Weight = 300 } },
            new FontWeightName { Name="Medium", FontWeight= new FontWeight{ Weight = 600 } },
            new FontWeightName { Name="Normal", FontWeight= new FontWeight{ Weight = 400 } },
            new FontWeightName { Name="SemiBold", FontWeight= new FontWeight{ Weight = 600 } },
            new FontWeightName { Name="SemiLight", FontWeight= new FontWeight{ Weight = 350 } },
            new FontWeightName { Name="Thin", FontWeight= new FontWeight{ Weight = 300 } },
        };

        #endregion

        //@Content
        public MenuTitle MenuTitle => this._MenuTitle;

        //@Construct
        public CharacterControl()
        {
            this.InitializeComponent();

            #region HorizontalAlignment

            this.HorizontalButton0.Tapped += (s, e) =>
            {
                if (this.FrameLayer == null) return;
                this.FrameLayer.HorizontalAlignment = CanvasHorizontalAlignment.Left;
                this.ViewModel.Invalidate();//Invalidate
            };
            this.HorizontalButton1.Tapped += (s, e) =>
            {
                if (this.FrameLayer == null) return;
                this.FrameLayer.HorizontalAlignment = CanvasHorizontalAlignment.Center;
                this.ViewModel.Invalidate();//Invalidate
            };
            this.HorizontalButton2.Tapped += (s, e) =>
            {
                if (this.FrameLayer == null) return;
                this.FrameLayer.HorizontalAlignment = CanvasHorizontalAlignment.Right;
                this.ViewModel.Invalidate();//Invalidate
            };
            this.HorizontalButton3.Tapped += (s, e) =>
            {
                if (this.FrameLayer == null) return;
                this.FrameLayer.HorizontalAlignment = CanvasHorizontalAlignment.Justified;
                this.ViewModel.Invalidate();//Invalidate
            };

            #endregion


            #region FontFamily

            this.FontFamilyListView.ItemsSource = this.FontFamilyNames;
            this.FontFamilyButton.Tapped += (s, e) =>
            {
                this._vsSelectFontFamily = true;
                this.VisualState = this.VisualState;//State
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

            #endregion


            #region FontSize

            this.FontSizeListView.ItemsSource = this.FontSizes;
            this.FontSizeButton.Tapped += (s, e) =>
            {
                this._vsSelectFontSize = true;
                this.VisualState = this.VisualState;//State
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

            #endregion


            #region FontStyle

            this.FontWeightToggleButton.Tapped += (s, e) =>
            {
                if (this.FrameLayer == null) return;

                //Whether the judgment is small or large.
                bool isBold = this.FrameLayer.FontWeight.Weight > 500;
                string name = isBold ? "Normal" : "Bold";

                // Linq form FontWeights.
                FontWeightName fontWeight = this.FontWeights.First(f => f.Name == name);

                this.FrameLayer.FontWeight = fontWeight.FontWeight;
                this.ViewModel.Invalidate();//Invalidate
            };

            this.FontStyleToggleButton.Tapped += (s, e) =>
            {
                if (this.FrameLayer == null) return;

                //Whether the judgment is Normal or Italic.
                bool isNormal = this.FrameLayer.FontStyle == FontStyle.Normal;
                FontStyle fontStyle = isNormal ? FontStyle.Italic : FontStyle.Normal;

                this.FrameLayer.FontStyle = fontStyle;
                this.ViewModel.Invalidate();//Invalidate
            };

            this.UnderLineToggleButton.Tapped += (s, e) =>
            {
            };
                
            this.FontWeightListView.ItemsSource = this.FontWeights;
            this.FontWeightButton.Tapped += (s, e) =>
            {
                this._vsSelectFontWeight = true;
                this.VisualState = this.VisualState;//State
            };
            this.FontWeightListView.ItemClick += (s, e) =>
            {
                if (this.FrameLayer == null) return;

                if (e.ClickedItem is FontWeightName fontWeight)
                {
                    this.FrameLayer.FontWeight = fontWeight.FontWeight;
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

            #endregion

            //Button
            this.MenuTitle.BackButton.Tapped += (s, e) =>
            {
                this._vsSelectFontFamily = false;
                this._vsSelectFontSize = false;
                this._vsSelectFontWeight = false;
                this.VisualState = this.VisualState;//State
            };
        }
    }
}