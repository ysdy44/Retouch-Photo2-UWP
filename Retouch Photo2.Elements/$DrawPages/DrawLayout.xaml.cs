// Core:              ★★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★★★
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Elements.DrawPages
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
        /// <summary> TouchbarPickerBorder's Child. </summary>
        public FrameworkElement TouchbarPicker { set => this.TouchbarBorder.Children.Add(value); }
        /// <summary> TouchbarSliderBorder's Child. </summary>
        public FrameworkElement TouchbarSlider
        {
            set
            {
                Grid.SetColumn(value, 2);
                this.TouchbarBorder.Children.Add(value);
            }
        }


        //Foot
        /// <summary> Gets or sets RadiusAnimaPanel's content. </summary>
        public FrameworkElement FootPage { set => this.RadiusAnimaPanel.CenterContent = value; }
        /// <summary> LeftRadiusAnimaIcon's CenterContent. </summary>
        public object LeftIcon { get => this.LeftRadiusAnimaIcon.CenterContent; set => this.LeftRadiusAnimaIcon.CenterContent = value; }
        /// <summary> RightRadiusAnimaIcon's CenterContent. </summary>
        public object RightIcon { get => this.RightRadiusAnimaIcon.CenterContent; set => this.RightRadiusAnimaIcon.CenterContent = value; }

        //Head
        /// <summary> HeadBorder's Child. </summary>
        public UIElement HeadChild { get => this.HeadBorder.Child; set => this.HeadBorder.Child = value; }

        //Left
        /// <summary> LeftBorder's Child. </summary>
        public UIElement LeftPanel { get => this.LeftBorder.Child; set => this.LeftBorder.Child = value; }
        //Right
        /// <summary> RightBorder's Child. </summary>
        public UIElement RightPanel { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        /// <summary> GalleryButton. </summary>   
        public Button GalleryButton => this._GalleryButton; 
        /// <summary> GalleryToolTip. </summary>   
        public ToolTip GalleryToolTip => this._RightPhotosToolTip;
        /// <summary> WidthButton. </summary>   
        public Button WidthButton => this._WidthButton; 
        /// <summary> WidthToolTip. </summary>   
        public ToolTip WidthToolTip => this._WidthToolTip; 


        //@Construct
        /// <summary>
        /// Initializes a DrawLayout. 
        /// </summary>
        public DrawLayout()
        {
            this.InitializeComponent();
            this.ConstructWidthStoryboard();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            
            //Foot
            this.LeftRadiusAnimaIcon.Toggled += (s, e) => this.PhoneType = PhoneLayoutType.ShowLeft;
            this.RightRadiusAnimaIcon.Toggled += (s, e) => this.PhoneType = PhoneLayoutType.ShowRight;

            //DismissOverlay
            this.DismissOverlay.PointerPressed += (s, e) => this.PhoneType = PhoneLayoutType.Hided;
        }

        private void ConstructWidthStoryboard()
        {
            this._WidthButton.Click += (s, e) =>
            {
                if (this.RightBorder.ActualWidth < 100)
                {
                    this.RightWidthIcon.Glyph = "\uE126";
                    this.WidthFrame.Value = 220;
                }
                else
                {
                    this.RightWidthIcon.Glyph = "\uE127";
                    this.WidthFrame.Value = 70;
                }
                this.WidthStoryboard.Begin();//Storyboard
            };
        }
    }
}