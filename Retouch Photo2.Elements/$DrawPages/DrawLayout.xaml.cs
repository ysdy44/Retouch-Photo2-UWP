// Core:              ★★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★★★
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// <see cref = "DrawPage" />'s layout. 
    /// </summary>
    [TemplateVisualState(Name = nameof(WritableCollapsed), GroupName = nameof(WritableVisualStateGroup))]
    [TemplateVisualState(Name = nameof(WritablePhone), GroupName = nameof(WritableVisualStateGroup))]
    [TemplateVisualState(Name = nameof(WritablePad), GroupName = nameof(WritableVisualStateGroup))]
    [TemplateVisualState(Name = nameof(WritablePC), GroupName = nameof(WritableVisualStateGroup))]
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(FullScreen), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Phone), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(PhoneShowLeft), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(PhoneShowRight), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Pad), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(PC), GroupName = nameof(VisualStateGroup))]
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
        public Border TouchbarPicker => this.TouchbarPickerBorder;
        /// <summary> TouchbarSliderBorder's child. </summary>
        public Border TouchbarSlider => this.TouchbarSliderBorder;


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
        /// <summary> GalleryToolTip's content. </summary>   
        public object GalleryToolTipContent { get => this.GalleryToolTip.Content; set => this.GalleryToolTip.Content = value; }
        /// <summary> WidthToolTip's content. </summary>   
        public object WidthToolTipContent { get => this.WidthToolTip.Content; set => this.WidthToolTip.Content = value; }

        //Pin
        /// <summary> Pin StackPanel. </summary>   
        public StackPanel PinStackPanel => this._PinStackPanel;


        /// <summary> GalleryButton's click. </summary>
        public event RoutedEventHandler GalleryButtonClick
        {
            add
            {
                this.GalleryButton.Click += value;
                this.PCGalleryButton.Click += value;
            }
            remove
            {
                this.GalleryButton.Click -= value;
                this.PCGalleryButton.Click -= value;
            }
        }
        /// <summary> WritableCancelButton's click. </summary>
        public event RoutedEventHandler WritableCancelButtonClick
        {
            add => this.WritableCancelButton.Click += value;
            remove => this.WritableCancelButton.Click -= value;
        }


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


        public void ShowWritable(ControlTemplate icon, string title, object content)
        {
            this.WritableIconContentControl.Template = icon;
            this.WritableTextBlock.Text = title;
            this.WritableContentPresenter.Content = content;

            this._vsIsWritable = true;
            this.WritableVisualState = this.WritableVisualState;//State

            this._vsIsFullScreen = true;
            this.VisualStateCore = this.VisualState;//State
        }

        public void HideWritable()
        {
            this.WritableIconContentControl.Template = null;
            this.WritableTextBlock.Text = string.Empty;
            this.WritableContentPresenter.Content = null;

            this._vsIsWritable = false;
            this.WritableVisualState = this.WritableVisualState;//State

            this._vsIsFullScreen = false;
            this.VisualState = this.VisualState;//State
        }
    }
}