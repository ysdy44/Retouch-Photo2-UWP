// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Texts;
using Retouch_Photo2.ViewModels;
using System;
using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s TextArtisticTool.
    /// </summary>
    public partial class TextArtisticTool : ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;
        private bool FontWeightConverter(FontWeight2 fontWeight) => this.MethodViewModel.FontWeightConverter(fontWeight);
        private bool FontStyleConverter(FontStyle fontStyle) => this.MethodViewModel.FontStyleConverter(fontStyle);
        private string Round2Converter(float value) => $"{(float)Math.Round(value, 2)}";


        //@Content 
        public ToolType Type => ToolType.TextArtistic;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "TextArtisticTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "TextArtisticTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TextArtisticTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TextArtisticTool. 
        /// </summary>
        public TextArtisticTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.BoldButton.Tapped += (s, e) => this.MethodViewModel.MethodSetFontWeight();
            this.ItalicButton.Tapped += (s, e) => this.MethodViewModel.MethodSetFontStyle();
            this.UnderlineButton.Tapped += (s, e) => this.MethodViewModel.MethodSetUnderline();

            // Get all FontFamilys in your device.
            this.FontFamilyListView.ItemsSource = CanvasTextFormat.GetSystemFontFamilies(ApplicationLanguages.Languages).OrderBy(k => k);
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
            this.FontSizeListView.ItemsSource = new float[] { 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f, 16f, 18f, 20f, 24f, 30f, 36f, 48f, 64f, 72f, 96f, 144f, 288f };
            this.FontSizeButton.Tapped += (s, e) => this.FontSizeFlyout.ShowAt(this.FontSizeButton);
            this.FontSizeListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is float value)
                {
                    this.MethodViewModel.MethodSetFontSize(value);
                }
            };
            this.FontSizeFlyout.Closed += (s, e) => this.SettingViewModel.RegisteKey(); // Setting
            this.FontSizeFlyout.Opened += (s, e) => this.SettingViewModel.UnregisteKey(); // Setting


            this.TextButton.Tapped += (s, e) => Expander.ShowAt(MenuType.Text, this.TextButton);

            //@Focus
            this.TextBox.GotFocus += (s, e) => this.SettingViewModel.UnregisteKey();
            this.TextBox.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            this.TextBox.TextChanged += (s, e) =>
            {
                if (this.TextBox.FocusState == FocusState.Unfocused) return;

                string fontText = this.TextBox.Text;
                this.MethodViewModel.MethodSetFontText(fontText);
            };
        }


        /// <summary>
        /// Create a ILayer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The producted ILayer. </returns>
        public ILayer CreateLayer(Transformer transformer)
        {
            return new TextArtisticLayer
            {
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandardTextStyle,
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.ViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) => this.ViewModel.CreateTool.Cursor(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.ViewModel.CreateTool.Draw(drawingSession);


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate(); // Invalidate
        }
        public void OnNavigatedFrom()
        {
            TouchbarExtension.Instance = null;
        }


        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FontStyleTextBlock.Text = resource.GetString("Texts_FontStyle");
            this.BoldToolTip.Content = resource.GetString("Texts_FontStyle_Bold");
            this.ItalicToolTip.Content = resource.GetString("Texts_FontStyle_Italic");
            this.UnderlineToolTip.Content = resource.GetString("Texts_FontStyle_Underline");

            this.FontFamilyTextBlock.Text = resource.GetString("Texts_FontFamily");

            this.FontSizeTextBlock.Text = resource.GetString("Texts_FontSize");

            this.TextToolTip.Content = resource.GetString("Menus_Text");

            this.TextBox.PlaceholderText = resource.GetString("Tools_Text_PlaceholderText");
        }
    }
}