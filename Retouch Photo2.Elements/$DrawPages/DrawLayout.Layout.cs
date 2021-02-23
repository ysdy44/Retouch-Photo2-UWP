using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class DrawLayout : UserControl
    {

        #region DependencyProperty

        /// <summary> Gets or sets whether the <see cref = "DrawLayout" /> visibility for UnFullScreenButton. </summary>
        public Visibility UnFullScreenButtonVisibility
        {
            get => (Visibility)base.GetValue(UnFullScreenButtonVisibilityProperty);
            set => base.SetValue(UnFullScreenButtonVisibilityProperty, value);
        }
        /// <summary> Identifies the <see cref = "DrawLayout.UnFullScreenButtonVisibility" /> dependency property. </summary>
        public static readonly DependencyProperty UnFullScreenButtonVisibilityProperty = DependencyProperty.Register(nameof(UnFullScreenButtonVisibility), typeof(Visibility), typeof(DrawLayout), new PropertyMetadata(Visibility.Visible));

        #endregion


        //@VisualState
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
                            break;
                        }
                }

                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, true);
        }
        private VisualState VisualStateCore { set => VisualStateManager.GoToState(this, value.Name, false); }



        /// <summary> Gets or sets the page layout is full-screen. </summary>
        public bool IsFullScreen
        {
            get => this._vsIsFullScreen;
            set
            {
                if (this._vsIsFullScreen == value) return;

                this.UnFullScreenButtonVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                this._vsIsFullScreen = value;
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

                this.FootPanel.IsHitTestVisible = value;
                this._LeftIcon.IsHitTestVisible = value;
                this._RightIcon.IsHitTestVisible = value;
            }
        }

    }
}