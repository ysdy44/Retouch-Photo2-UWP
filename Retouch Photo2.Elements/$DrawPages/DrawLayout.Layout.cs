using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.DrawPages
{
    /// <summary> 
    /// <see cref = "DrawPage" />'s layout. 
    /// </summary>
    public sealed partial class DrawLayout : UserControl
    {
        
        //@VisualState
        bool _vsIsFullScreen;
        DeviceLayoutType _vsDeviceLayoutType = DeviceLayoutType.PC;
        PhoneLayoutType _vsPhoneType = PhoneLayoutType.Hided;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsFullScreen) return this.FullScreen;

                DeviceLayoutType type = this._vsDeviceLayoutType;

                switch (type)
                {
                    case DeviceLayoutType.Phone:
                        {
                            switch (this._vsPhoneType)
                            {
                                case PhoneLayoutType.Hided: return this.Phone;
                                case PhoneLayoutType.ShowLeft: return this.PhoneShowLeft;
                                case PhoneLayoutType.ShowRight: return this.PhoneShowRight;
                                default: return this.Normal;
                            }
                        }
                    case DeviceLayoutType.Pad: return this.Pad;
                    case DeviceLayoutType.PC: return this.PC;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        /// <summary> Gets or sets the page layout is full-screen. </summary>
        public bool IsFullScreen
        {
            get => this._vsIsFullScreen;
            set
            {
                this.IsFullScreenChanged?.Invoke(value);//Delegate

                this._vsIsFullScreen = value;
                this.VisualState = this.VisualState;//State
            }
        }
        /// <summary> Gets or sets the phone layout type. </summary>
        public PhoneLayoutType PhoneType
        {
            set
            {
                this._vsPhoneType = value;
                this.VisualState = this.VisualState;//State
            }
        }
        /// <summary> Gets or sets the device layout type. </summary>
        public DeviceLayoutType DeviceLayoutType
        {            
            set
            {
                this._vsDeviceLayoutType = value;
                this.VisualState = this.VisualState;//State
            }
        }


        /// <summary>
        /// Sets or Gets the on state of the IsHitTestVisible on the canvas. 
        /// </summary>
        public bool CanvasHitTestVisible
        {
            set
            {
                this.LeftBorder.IsHitTestVisible = value;
                this.RightBorder.IsHitTestVisible = value;

                this.TouchbarBorder.IsHitTestVisible = value;
                this.RadiusAnimaPanel.IsHitTestVisible = value;
                this.LeftRadiusAnimaIcon.IsHitTestVisible = value;
                this.RightRadiusAnimaIcon.IsHitTestVisible = value;
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