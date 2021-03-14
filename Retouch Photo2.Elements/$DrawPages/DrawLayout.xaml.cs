// Core:              ★★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★★★
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Devices.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// <see cref = "DrawPage" />'s layout. 
    /// </summary>
    public sealed partial class DrawLayout : UserControl
    {

        //@Content
        //Body
        /// <summary> CanvasControl of <see cref="ILayer.Render"/>. </summary>
        public CanvasControl LayerRenderCanvasControl => this._LayerRenderCanvasControl;
        /// <summary> CanvasControl of <see cref="ITool.Draw"/>. </summary>
        public CanvasControl ToolDrawCanvasControl => this._ToolDrawCanvasControl;

        //Touchbar
        /// <summary> TouchbarPickerBorder's child. </summary>
        public FrameworkElement TouchbarPicker { set => this.TouchbarPickerBorder.Child = value; }
        /// <summary> TouchbarSliderBorder's child. </summary>
        public FrameworkElement TouchbarSlider { set => this.TouchbarSliderBorder.Child = value; }


        //Foot
        /// <summary> Gets or sets RadiusAnimaPanel's content. </summary>
        public UIElement FootPage { set => this.FootPanel.Child = value; }
        /// <summary> _LeftIcon's content. </summary>
        public object LeftIcon { get => this.LeftContentControl.Content; set => this.LeftContentControl.Content = value; }
        /// <summary> _RightIcon's content. </summary>
        public object RightIcon { get => this.RightContentControl.Content; set => this.RightContentControl.Content = value; }

        //Head
        /// <summary> HeadBorder's child. </summary>
        public UIElement HeadChild { get => this.HeadBorder.Child; set => this.HeadBorder.Child = value; }

        //Left
        /// <summary> LeftBorder's child. </summary>
        public UIElement LeftPanel { get => this.LeftBorder.Child; set => this.LeftBorder.Child = value; }
        //Right
        /// <summary> RightBorder's child. </summary>
        public UIElement RightPanel { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        /// <summary> Gallery button. </summary>   
        public Button GalleryButton => this._GalleryButton;
        /// <summary> PC Gallery button. </summary>   
        public Button PCGalleryButton => this._PCGalleryButton;
        /// <summary> Gallery ToolTip. </summary>   
        public ToolTip GalleryToolTip => this._RightPhotosToolTip;
        /// <summary> Width button. </summary>   
        public Button WidthButton => this._WidthButton;
        /// <summary> Width ToolTip. </summary>   
        public ToolTip WidthToolTip => this._WidthToolTip;


        //@Construct
        /// <summary>
        /// Initializes a DrawLayout. 
        /// </summary>
        public DrawLayout()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualStateCore = this.VisualState;//State

            //Foot
            this._LeftIcon.Tapped += (s, e) => this.PhoneType = PhoneLayoutType.ShowLeft;
            this._LeftIcon.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this.PhoneType = PhoneLayoutType.ShowLeft;
                }
            };
            this._RightIcon.Tapped += (s, e) => this.PhoneType = PhoneLayoutType.ShowRight;
            this._RightIcon.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this.PhoneType = PhoneLayoutType.ShowRight;
                }
            };

            //DismissOverlay
            this.DismissOverlay.PointerPressed += (s, e) => this.PhoneType = PhoneLayoutType.Hided;

            //Width
            this._WidthButton.Click += (s, e) =>
            {
                if (this.RightGrid.ActualWidth < 100)
                    this.WidthToWideStoryboard.Begin();//Storyboard
                else
                    this.WideToWidthStoryboard.Begin();//Storyboard
            };
        }

    }
}