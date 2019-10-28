using System.Numerics;
using Windows.Devices.Input;
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
        /// <summary> BackButton. </summary>
        public Button BackButton => this._BackButton;
        /// <summary> CenterBorder's Child. </summary>
        public UIElement CenterChild { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }

        /// <summary> RightBorder's Child. </summary>
        public UIElement RightPane { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        /// <summary> LeftBorder's Child. </summary>
        public UIElementCollection LeftPaneChildren => this.LeftStackPanel.Children;

        /// <summary> IconLeftContentControl's Content. </summary>
        public object LeftIcon { get => this.IconLeftContentControl.Content; set => this.IconLeftContentControl.Content = value; }
        /// <summary> IconRightContentControl's Content. </summary>
        public object RightIcon { get => this.IconRightContentControl.Content; set => this.IconRightContentControl.Content = value; }

        /// <summary> TouchbarBorder's Child. </summary>
        public UIElement Touchbar { get => this.TouchbarBorder.Child; set => this.TouchbarBorder.Child = value; }
        /// <summary> Gets or sets RadiusAnimaPanel's content. </summary>
        public FrameworkElement FootPage { set { this.RadiusAnimaPanel.CenterContent = value; } }


        /// <summary> HeadLeftBorder's Child. </summary>
        public UIElement HeadLeftPane { get => this.HeadLeftBorder.Child; set => this.HeadLeftBorder.Child = value; }
        /// <summary> HeadRightStackPane's Children. </summary>
        public UIElementCollection HeadRightChildren => this.HeadRightStackPane.Children;
        

        /// <summary> Sets the backgroud's Color. </summary>
        public ElementTheme Theme
        {
            set
            {
                switch (value)
                {
                    case ElementTheme.Light:
                        this.LightStoryboard.Begin();//Storyboard
                        break;
                    case ElementTheme.Dark:
                        this.DarkStoryboard.Begin();//Storyboard
                        break;
                }
            }
        }
        /// <summary> Sets the page layout is full-screen. </summary>
        public bool IsFullScreen
        {
            set
            {
                this._vsIsFullScreen = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@VisualState
        bool _vsIsFullScreen = true;
        PhoneLayoutType _vsPhoneType = PhoneLayoutType.Hided;
        DeviceLayoutType _vsActualWidthType = DeviceLayoutType.Adaptive;

        public DeviceLayoutType VisualStateDeviceType = DeviceLayoutType.Adaptive;
        public double VisualStatePhoneMaxWidth = 600.0;
        public double VisualStatePadMaxWidth = 900.0;

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


        //@Construct
        public DrawLayout()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                double width = e.NewSize.Width;

                if (width > this.VisualStatePadMaxWidth) this._vsActualWidthType = DeviceLayoutType.PC;
                else if (width > this.VisualStatePhoneMaxWidth) this._vsActualWidthType = DeviceLayoutType.Pad;
                else this._vsActualWidthType = DeviceLayoutType.Phone;

                this.VisualState = this.VisualState;//State
            };

            //DismissOverlay
            this.IconDismissOverlay.PointerPressed += (s, e) =>
            {
                this._vsPhoneType = PhoneLayoutType.Hided;
                this.VisualState = this.VisualState;//State
            };

            //IconLeft
            this.IconLeftGrid.Tapped += (s, e) =>
            {
                this._vsPhoneType = PhoneLayoutType.ShowLeft;
                this.VisualState = this.VisualState;//State
            };
            this.IconLeftGrid.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this._vsPhoneType = PhoneLayoutType.ShowLeft;
                    this.VisualState = this.VisualState;//State
                }
            };
            //IconRight
            this.IconRightGrid.Tapped += (s, e) =>
            {
                this._vsPhoneType = PhoneLayoutType.ShowRight;
                this.VisualState = this.VisualState;//State
            };
            this.IconRightGrid.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this._vsPhoneType = PhoneLayoutType.ShowRight;
                    this.VisualState = this.VisualState;//State
                }
            };
        }
        
               
        private bool isPadLayersControlWidth;
        /// <summary>
        /// Chnage RightBorder width.
        /// </summary>
        public void PadChangeLayersWidth()
        {
            if (this._vsActualWidthType == DeviceLayoutType.Pad)
            {
                bool value = !this.isPadLayersControlWidth;
                this.isPadLayersControlWidth = value;

                double width = value ? 220 : 70;
                this.RightGridLenght.Width = new GridLength(width);
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
        
    }     
}