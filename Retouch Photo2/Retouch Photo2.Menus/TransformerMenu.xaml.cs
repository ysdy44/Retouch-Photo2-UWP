// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "FanKit.Transformers.Transformer"/>.
    /// </summary>
    public sealed partial class TransformerMenu : Expander, IMenu
    {

        //@Content
        public bool IsOpen { set => this.TransformerMainPage.IsOpen = value; }
        public override UIElement MainPage => this.TransformerMainPage;

        readonly TransformerMainPage TransformerMainPage = new TransformerMainPage();


        //@Construct
        /// <summary>
        /// Initializes a TransformerMenu. 
        /// </summary>
        public TransformerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.TransformerMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.Page != secondPage) this.Page = secondPage;
                this.IsSecondPage = true;
                this.Title = (string)title;
            };
        }

    }

    /// <summary>
    /// Menu of <see cref = "FanKit.Transformers.Transformer"/>.
    /// </summary>
    public sealed partial class TransformerMenu : Expander, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = resource.GetString("Menus_Transformer");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Transformer;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            Content = new FanKit.Transformers.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }

    /// <summary>
    /// MainPage of <see cref = "FanKit.Transformers.Transformer"/>.
    /// </summary>
    public sealed partial class TransformerMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;        
        SettingViewModel SettingViewModel => App.SettingViewModel;

        bool IsRatio => this.SettingViewModel.IsRatio;
        Transformer SelectionTransformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }


        //@Delegate
        /// <summary> Occurs when second-page changed. </summary>
        public event EventHandler<UIElement> SecondPageChanged;


        //@Content
        /// <summary> Position RemoteControl. </summary>
        public RemoteControl PositionRemoteControl { get; } = new RemoteControl
        {
            Margin = new Thickness(4),
            Width = double.NaN,
            Height = double.NaN,
            FlowDirection = FlowDirection.LeftToRight
        };


        private IndicatorMode IndicatorMode = IndicatorMode.LeftTop;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "TransformerMainPage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransformerMainPage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TransformerMainPage), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TransformerMainPage. 
        /// </summary>
        public TransformerMainPage()
        {
            this.InitializeComponent();
            this.TransformerStateControl.StateChanged += this.SetTransformerState;
            this.ConstructStrings();

            this.ConstructWidthHeight();
            this.ConstructRadianSkew();
            this.ConstructXY();

            this.ConstructPositionRemoteControl();
            this.ConstructIndicatorControl();
        }


        //TransformerMainPage
        private void SetTransformerState(TransformerState value, Transformer transformer)
        {
            switch (value)
            {
                case TransformerState.Enabled:
                    {
                        //Value
                        {
                            Vector2 horizontal = transformer.Horizontal;
                            Vector2 vertical = transformer.Vertical;

                            //Radians Skew
                            float radians = Transformer.GetRadians(horizontal);
                            float skew = Transformer.GetSkew(vertical, radians);
                            this.RPicker.Value = (int)radians;
                            this.SPicker.Value = (int)skew;

                            //@Release: case Debug
                            //Width Height
                            //float width = horizontal.Length();
                            //float height = vertical.Length();
                            //@Release: case Release
                            double width = Math.Sqrt(horizontal.X * horizontal.X + horizontal.Y * horizontal.Y);
                            double height = Math.Sqrt(vertical.X * vertical.X + vertical.Y * vertical.Y);
                            this.WPicker.Value = (int)width;
                            this.HPicker.Value = (int)height;

                            //X Y
                            Vector2 vector = transformer.GetIndicatorVector(this.IndicatorMode);
                            this.XPicker.Value = (int)vector.X;
                            this.YPicker.Value = (int)vector.Y;

                            //Indicator
                            this.IndicatorControl.Radians = radians;
                        }
                        //IsEnabled
                        {
                            this.RPicker.IsEnabled = true;
                            this.SPicker.IsEnabled = true;

                            this.WPicker.IsEnabled = true;
                            this.HPicker.IsEnabled = true;

                            this.XPicker.IsEnabled = true;
                            this.YPicker.IsEnabled = true;

                            this.RatioToggleControl.IsEnabled = true;
                            this.StepFrequencyButton.IsEnabled = true;

                            this.IndicatorControl.Mode = this.IndicatorMode;
                            this.PositionRemoteButton.IsEnabled = true;
                        }
                    }
                    break;

                case TransformerState.Disabled:
                    {
                        //Value
                        {
                            //Radians Skew
                            this.RPicker.Value = 0;
                            this.SPicker.Value = 0;

                            //Width Height
                            this.WPicker.Value = 0;
                            this.HPicker.Value = 0;

                            //X Y
                            this.XPicker.Value = 0;
                            this.YPicker.Value = 0;

                            //Indicator
                            this.IndicatorControl.Radians = 0;
                        }
                        //IsEnabled
                        {
                            this.WPicker.IsEnabled = false;
                            this.HPicker.IsEnabled = false;

                            this.RPicker.IsEnabled = false;
                            this.SPicker.IsEnabled = false;

                            this.XPicker.IsEnabled = false;
                            this.YPicker.IsEnabled = false;

                            this.IndicatorControl.Mode = IndicatorMode.None;
                            this.PositionRemoteButton.IsEnabled = false;
                            this.RatioToggleControl.IsEnabled = false;
                            this.StepFrequencyButton.IsEnabled = false;
                        }
                    }
                    break;

            }

        }

    }
}