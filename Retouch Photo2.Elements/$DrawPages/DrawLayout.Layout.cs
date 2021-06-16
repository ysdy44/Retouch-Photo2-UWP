using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class DrawLayout : UserControl
    {


        #region DependencyProperty


        /// <summary> Gets or sets UnFullScreen's visibility. </summary>
        public Visibility UnFullScreenVisibility
        {
            get => (Visibility)base.GetValue(UnFullScreenVisibilityProperty);
            set => base.SetValue(UnFullScreenVisibilityProperty, value);
        }
        /// <summary> Identifies the <see cref = "DrawLayout.UnFullScreenVisibility" /> dependency property. </summary>
        public static readonly DependencyProperty UnFullScreenVisibilityProperty = DependencyProperty.Register(nameof(UnFullScreenVisibility), typeof(Visibility), typeof(DrawLayout), new PropertyMetadata(Visibility.Visible));


        /// <summary> Gets or sets MenuOverlayCanvas's visibility. </summary>
        public Visibility MenuOverlayCanvasVisibility
        {
            get => (Visibility)base.GetValue(MenuOverlayCanvasVisibilityProperty);
            set => base.SetValue(MenuOverlayCanvasVisibilityProperty, value);
        }
        /// <summary> Identifies the <see cref = "DrawLayout.MenuOverlayCanvasVisibility" /> dependency property. </summary>
        public static readonly DependencyProperty MenuOverlayCanvasVisibilityProperty = DependencyProperty.Register(nameof(MenuOverlayCanvasVisibility), typeof(Visibility), typeof(DrawLayout), new PropertyMetadata(Visibility.Visible));


        #endregion


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


        /// <summary> Gets or sets the page layout is writable. </summary>
        public bool IsWritable
        {
            get => this._vsIsWritable;
            set
            {
                this._vsIsWritable = value;
                if (value) this.WritableDocker.Show();
                else this.WritableDocker.Hide();
                this.MenuOverlayCanvasVisibility = value ? Visibility.Collapsed : Visibility.Visible;
            }
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
                this.VisualState = this.VisualState; // State

                this.UnFullScreenVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        /// <summary> Gets or sets the device layout type. </summary>
        public DeviceLayoutType DeviceLayoutType
        {
            set
            {
                this._vsPhoneType = PhoneLayoutType.Hided;
                this._vsDeviceLayoutType = value;
                this.WritableDocker.DeviceLayoutType = value;
                this.VisualState = this.VisualState; // State
            }
        }

    }
}