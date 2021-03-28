// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "FanKit.Transformers.Transformer"/>.
    /// </summary>
    public sealed partial class TransformerMenu : UserControl
    {

        //@ViewModel
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


        /// <summary> Gets or sets <see cref = "TransformerMenu" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransformerMenu.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TransformerMenu), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TransformerMainPage. 
        /// </summary>
        public TransformerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructWidthHeight();
            this.ConstructRadianSkew();
            this.ConstructXY();

            this.ConstructPositionRemoteControl();
            this.ConstructIndicatorControl();
        }

    }
}