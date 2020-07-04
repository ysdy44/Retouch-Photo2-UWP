using System;
using System.Numerics;
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
        //@Delegate  
        /// <summary> Occurs when the IsFullScreen changed. </summary>
        public Action<bool> IsFullScreenChanged { get; set; }

        //@Content
        //Body
        /// <summary> CenterBorder's Child. </summary>
        public UIElement CenterChild { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }

        //Touchbar
        /// <summary> TouchbarPickerBorder's Child. </summary>
        public UIElement TouchbarPicker { get => this.TouchbarPickerBorder.Child; set => this.TouchbarPickerBorder.Child = value; }
        /// <summary> TouchbarSliderBorder's Child. </summary>
        public UIElement TouchbarSlider { get => this.TouchbarSliderBorder.Child; set => this.TouchbarSliderBorder.Child = value; }

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
        /// <summary> RightCenterBorder's Child. </summary>
        public UIElement RightCenterPanel { get => this.RightCenterBorder.Child; set => this.RightCenterBorder.Child = value; }
        /// <summary> RightPhotosButton. </summary>   
        public Button RightPhotosButton => this._RightPhotosButton;
        /// <summary> RightPhotosToolTip. </summary>   
        public ToolTip RightPhotosToolTip => this._RightPhotosToolTip;
        /// <summary> RightWidthButton. </summary>   
        public Button RightWidthButton => this._RightWidthButton;
        /// <summary> RightWidthToolTip. </summary>   
        public ToolTip RightWidthToolTip => this._RightWidthToolTip;


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
            // Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.WidthKeyFrames, this.RightBorder);
            Storyboard.SetTargetProperty(this.WidthKeyFrames, "(UIElement.Width)");

            this._RightWidthButton.Click += (s, e) =>
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