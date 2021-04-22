using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class DrawLayout : UserControl
    {

        //@VisualState
        bool _vsIsWritable = false;
        bool _vsIsFullScreen = true;
        DeviceLayoutType _vsDeviceLayoutType = DeviceLayoutType.PC;
        PhoneLayoutType _vsPhoneType = PhoneLayoutType.Hided;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsFullScreen) return this.FullScreen;

                switch (this._vsDeviceLayoutType)
                {
                    case DeviceLayoutType.PC: return this.PC;
                    case DeviceLayoutType.Pad: return this.Pad;
                    case DeviceLayoutType.Phone:
                        {
                            switch (this._vsPhoneType)
                            {
                                case PhoneLayoutType.Hided: return this.Phone;
                                case PhoneLayoutType.ShowLeft: return this.PhoneShowLeft;
                                case PhoneLayoutType.ShowRight: return this.PhoneShowRight;
                            }
                            return this.Normal;
                        }
                }

                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, true);
        }
        private VisualState VisualStateCore { set => VisualStateManager.GoToState(this, value.Name, false); }
        /// <summary> 
        /// Represents the writable visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState WritableVisualState
        {
            get
            {
                if (this._vsIsWritable == false) return this.WritableCollapsed;

                switch (this._vsDeviceLayoutType)
                {
                    case DeviceLayoutType.PC: return this.WritablePC;
                    case DeviceLayoutType.Pad: return this.WritablePad;
                    case DeviceLayoutType.Phone: return this.WritablePhone;
                }

                return this.WritableCollapsed;
            }
            set => VisualStateManager.GoToState(this, value.Name, true);
        }



        /// <summary> Gets or sets the page layout is full-screen. </summary>
        public bool IsFullScreen
        {
            get => this._vsIsFullScreen;
            set
            {
                if (this._vsIsFullScreen == value) return;

                this._vsPhoneType = PhoneLayoutType.Hided;
                this._vsIsFullScreen = value;
                this.VisualState = this.VisualState;//State
            }
        }
        /// <summary> Gets or sets the device layout type. </summary>
        public DeviceLayoutType DeviceLayoutType
        {
            set
            {
                this._vsPhoneType = PhoneLayoutType.Hided;
                this._vsDeviceLayoutType = value;
                this.WritableVisualState = this.WritableVisualState;//State
                this.VisualState = this.VisualState;//State
            }
        }

    }
}