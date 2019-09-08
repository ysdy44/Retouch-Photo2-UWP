using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{

        /// <summary> 
        /// PointerState of <see cref="ButtonState"/>. 
        /// </summary>
        enum PointerState
        {
            /// <summary> Normal. </summary>
            None,
            /// <summary> Pointer-over. </summary>
            PointerOver,
            /// <summary> Pressed. </summary>
            Pressed,
        }
    /// <summary>
    /// Retouch_Photo2 Tools 's Button.
    /// </summary>
    public sealed partial class Button : UserControl
    {
        //@Converter
        private double BoolToOpacityConverter(bool isChecked) => isChecked ? 1.0 : 0.5;
        private Visibility BoolToVisibilityConverter(bool isChecked) => isChecked ? Visibility.Visible : Visibility.Collapsed;


        /// <summary> <see cref = "Button" />'s Type. </summary>
        public ToolType Type { get; set; }
        /// <summary> <see cref = "Button" />'s RootGrid. </summary>
        public Border RootGrid { set => this._RootGrid = value; get => this._RootGrid; }
        /// <summary> ContentPresenter's Content. </summary>
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }

        #region DependencyProperty

        /// <summary> The identifier of the buttonthat is currently selected. </summary>
        public ToolType GroupType
        {
            get { return (ToolType)GetValue(GroupTypeProperty); }
            set { SetValue(GroupTypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "Button.GroupType" /> dependency property. </summary>
        public static readonly DependencyProperty GroupTypeProperty = DependencyProperty.Register(nameof(GroupType), typeof(ToolType), typeof(Button), new PropertyMetadata(null, (sender, e) =>
        {
            Button con = (Button)sender;

            if (e.NewValue is ToolType value)
            {
                bool isSelected = (value == con.Type);
                if (con.IsSelected == isSelected) return;

                con.IsSelected = isSelected;
                con.State = con.GetState();
            }
        }));

        #endregion


        /// <summary> State of <see cref="Button"/>. </summary>
        public ButtonState State
        {
            get => this.state;
            set
            {
                switch (value)
                {
                    case ButtonState.None:
                        VisualStateManager.GoToState(this, this.Normal.Name, false);
                        break;
                    case ButtonState.PointerOver:
                        VisualStateManager.GoToState(this, this.PointerOver.Name, false);
                        break;
                    case ButtonState.Pressed:
                        VisualStateManager.GoToState(this, this.Pressed.Name, false);
                        break;

                    case ButtonState.Selected:
                        VisualStateManager.GoToState(this, this.Selected.Name, false);
                        break;
                    case ButtonState.PointerOverSelected:
                        VisualStateManager.GoToState(this, this.PointerOverSelected.Name, false);
                        break;
                    case ButtonState.PressedSelected:
                        VisualStateManager.GoToState(this, this.PressedSelected.Name, false);
                        break;
                }
                this.state = value;
            }
        }
        private ButtonState state;

        bool? IsSelected;
        PointerState PointerState;

        private ButtonState GetState()
        {
            switch (this.IsSelected)
            {
                case null: return ButtonState.None;

                case false:
                    {
                        switch (this.PointerState)
                        {
                            case PointerState.None: return ButtonState.None;
                            case PointerState.Pressed: return ButtonState.Pressed;
                            case PointerState.PointerOver: return ButtonState.PointerOver;
                        }
                    }
                    break;

                case true:
                    {
                        switch (this.PointerState)
                        {
                            case PointerState.None: return ButtonState.Selected;
                            case PointerState.Pressed: return ButtonState.PressedSelected;
                            case PointerState.PointerOver: return ButtonState.PointerOverSelected;
                        }
                    }
                    break;

                default:
                    break;
            }

            return ButtonState.None;
        }
        

        //@Construct
        public Button()
        {
            this.InitializeComponent();
            this._RootGrid.PointerEntered += (s, e) =>
            {
                this.PointerState = PointerState.PointerOver;
                this.State = this.GetState();//State
            };
            this._RootGrid.PointerPressed += (s, e) =>
            {
                this.PointerState = PointerState.Pressed;
                this.State = this.GetState();//State
            };
            this._RootGrid.PointerExited += (s, e) =>
            {
                this.PointerState = PointerState.None;
                this.State = this.GetState();//State
            };
        }
    }
}