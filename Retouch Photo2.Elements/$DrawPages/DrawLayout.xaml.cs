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
        //@Content

        //Body
        /// <summary> CenterBorder's Child. </summary>
        public UIElement CenterChild { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }

        //Foot
        /// <summary> TouchbarBorder's Child. </summary>
        public UIElement Touchbar { get => this.TouchbarBorder.Child; set => this.TouchbarBorder.Child = value; }
        /// <summary> Gets or sets RadiusAnimaPanel's content. </summary>
        public FrameworkElement FootPage { set => this.RadiusAnimaPanel.CenterContent = value; }
        /// <summary> LeftRadiusAnimaIcon's CenterContent. </summary>
        public object LeftIcon { get => this.LeftRadiusAnimaIcon.CenterContent; set => this.LeftRadiusAnimaIcon.CenterContent = value; }
        /// <summary> RightRadiusAnimaIcon's CenterContent. </summary>
        public object RightIcon { get => this.RightRadiusAnimaIcon.CenterContent; set => this.RightRadiusAnimaIcon.CenterContent = value; }

        //Head
        /// <summary> DocumentBorder's Child. </summary>
        public UIElement DocumentChild { get => this._DocumentBorder.Child; set => this._DocumentBorder.Child = value; }
        /// <summary> HeadLeftBorder's Child. </summary>
        public UIElement HeadLeftPanel { get => this.HeadLeftBorder.Child; set => this.HeadLeftBorder.Child = value; }
        /// <summary> HeadRightStackPanel's Children. </summary>
        public UIElementCollection HeadRightChildren => this.HeadRightStackPanel.Children;

        //Left
        /// <summary> LeftBorder's Child. </summary>
        public UIElement LeftPanel { get => this.LeftBorder.Child; set => this.LeftBorder.Child = value; }
        //Right
        /// <summary> RightCenterBorder's Child. </summary>
        public UIElement RightCenterPanel { get => this.RightCenterBorder.Child; set => this.RightCenterBorder.Child = value; }
        /// <summary> RightAddButton. </summary>   
        public Button RightAddButton => this._RightAddButton;



        //@Construct
        public DrawLayout()
        {
            this.InitializeComponent();
            this.ConstructWidthStoryboard();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                double width = e.NewSize.Width;

                this._vsActualWidthType = this._getDeviceLayoutType(width);
                this.VisualState = this.VisualState;//State
            };

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

            this.WidthButton.Tapped += (s, e) =>
            {
                if (this.RightBorder.ActualWidth < 100)
                {
                    this.WidthIcon.Glyph = "\uE126";
                    this.WidthFrame.Value = 220;
                }
                else
                {
                    this.WidthIcon.Glyph = "\uE127";
                    this.WidthFrame.Value = 70;
                }
                this.WidthStoryboard.Begin();//Storyboard
            };
        }
    }

    /// <summary> 
    /// <see cref = "DrawPage" />'s layout. 
    /// </summary>
    public sealed partial class DrawLayout : UserControl
    {

        public DeviceLayoutType VisualStateDeviceType = DeviceLayoutType.Adaptive;
        public double VisualStatePhoneMaxWidth = 600.0;
        public double VisualStatePadMaxWidth = 900.0;



        //@VisualState
        bool _vsIsFullScreen = true;
        PhoneLayoutType _vsPhoneType = PhoneLayoutType.Hided;
        DeviceLayoutType _vsActualWidthType = DeviceLayoutType.Adaptive;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsFullScreen) return this.FullScreen;

                switch (this.VisualStateDeviceType)
                {
                    case DeviceLayoutType.Phone: return this._getPhoneVisualState(this._vsPhoneType);
                    case DeviceLayoutType.Pad: return this.Pad;
                    case DeviceLayoutType.PC: return this.PC;
                    case DeviceLayoutType.Adaptive:
                        {
                            switch (this._vsActualWidthType)
                            {
                                case DeviceLayoutType.Phone: return this._getPhoneVisualState(this._vsPhoneType);
                                case DeviceLayoutType.Pad: return this.Pad;
                                case DeviceLayoutType.PC: return this.PC;
                            }
                        }
                        break;
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        private VisualState _getPhoneVisualState(PhoneLayoutType phoneLayoutType)
        {
            switch (this._vsPhoneType)
            {
                case PhoneLayoutType.Hided: return this.Phone;
                case PhoneLayoutType.ShowLeft: return this.PhoneShowLeft;
                case PhoneLayoutType.ShowRight: return this.PhoneShowRight;
            }
            return this.Normal;
        }
        private DeviceLayoutType _getDeviceLayoutType(double width)
        {
            if (width > this.VisualStatePadMaxWidth) return DeviceLayoutType.PC;
            if (width > this.VisualStatePhoneMaxWidth) return DeviceLayoutType.Pad;
            return DeviceLayoutType.Phone;
        }



        /// <summary> Sets the backgroud's Color. </summary>
        public ElementTheme Theme
        {
            set
            {
                switch (value)
                {
                    case ElementTheme.Light:
                        //   this.LightStoryboard.Begin();//Storyboard
                        break;
                    case ElementTheme.Dark:
                        //this.DarkStoryboard.Begin();//Storyboard
                        break;
                }
            }
        }
        /// <summary> Gets or sets the page layout is full-screen. </summary>
        public bool IsFullScreen
        {
            get => this._vsIsFullScreen;
            set
            {
                this._vsIsFullScreen = value;
                this.VisualState = this.VisualState;//State
            }
        }
        /// <summary> Gets or sets the phone layout type. </summary>
        private PhoneLayoutType PhoneType
        {
            set
            {
                this._vsPhoneType = value;
                this.VisualState = this.VisualState;//State
            }
        }



        /// <summary>
        /// Gets the offset of full-screen statue layout.
        /// </summary>
        /// <returns></returns>
        public Vector2 FullScreenOffset
        {
            get
            {
                if (this.VisualStateDeviceType == DeviceLayoutType.Adaptive)
                {
                    if (this._vsActualWidthType == DeviceLayoutType.PC)
                        return new Vector2(70, 50);
                }

                if (this.VisualStateDeviceType == DeviceLayoutType.PC)
                    return new Vector2(70, 50);

                return new Vector2(0, 50);
            }
        }
        /// <summary> Gets the CenterChild width. </summary>
        public float CenterChildWidth
        {
            get
            {
                float rootWidth = (float)Window.Current.Bounds.Width;

                switch (this.VisualStateDeviceType)
                {
                    case DeviceLayoutType.PC: return rootWidth - 70 - 220;
                    case DeviceLayoutType.Adaptive:
                        switch (this._vsActualWidthType)
                        {
                            case DeviceLayoutType.PC: return rootWidth - 70 - 220;
                        }
                        break;
                }
                return rootWidth;
            }
        }
        /// <summary> Gets the CenterChild height. </summary>
        public float CenterChildHeight
        {
            get
            {
                float rootHeight = (float)Window.Current.Bounds.Height;
                return rootHeight - 50;
            }
        }


        /*
        private void FullScreenButton_Tapped(object sender, TappedRoutedEventArgs e) => VisualStateManager.GoToState(this, this.FullScreen.Name, false);
        private void PhoneButton_Tapped(object sender, TappedRoutedEventArgs e) => VisualStateManager.GoToState(this, this.Phone.Name, false);
        private void PhoneShowLeftButton_Tapped(object sender, TappedRoutedEventArgs e) => VisualStateManager.GoToState(this, this.PhoneShowLeft.Name, false);
        private void PhoneShowRightButton_Tapped(object sender, TappedRoutedEventArgs e) => VisualStateManager.GoToState(this, this.PhoneShowRight.Name, false);
        private void PadButton_Tapped(object sender, TappedRoutedEventArgs e) => VisualStateManager.GoToState(this, this.Pad.Name, false);
        private void PCButton_Tapped(object sender, TappedRoutedEventArgs e) => VisualStateManager.GoToState(this, this.PC.Name, false);
         */
    }
}