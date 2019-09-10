using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Buttons
{
    /// <summary>
    /// Retouch_Photo2 Tools 's Button.
    /// </summary>
    public sealed partial class Button : UserControl
    {
        //@Content
        /// <summary> Button's Type. </summary>
        public ToolType Type { get; set; }
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
                if (con.Manager.IsSelected == isSelected) return;

                con.Manager.IsSelected = isSelected;
                con.State = con.Manager.GetState();
            }
        }));

        #endregion

        
        /// <summary> Manager of <see cref="Button"/>. </summary>
        ButtonStateManager Manager = new ButtonStateManager();
        /// <summary> State of <see cref="Button"/>. </summary>
        public ButtonState State
        {
            set
            {
                switch (value)
                {
                    case ButtonState.None: VisualStateManager.GoToState(this, this.Normal.Name, false); break;
                    case ButtonState.PointerOver: VisualStateManager.GoToState(this, this.PointerOver.Name, false); break;
                    case ButtonState.Pressed: VisualStateManager.GoToState(this, this.Pressed.Name, false); break;

                    case ButtonState.Selected: VisualStateManager.GoToState(this, this.Selected.Name, false); break;
                    case ButtonState.PointerOverSelected: VisualStateManager.GoToState(this, this.PointerOverSelected.Name, false); break;
                    case ButtonState.PressedSelected: VisualStateManager.GoToState(this, this.PressedSelected.Name, false); break;
                }
            }
        }


        //@Construct
        public Button()
        {
            this.InitializeComponent();
            this.ContentPresenter.PointerEntered += (s, e) =>
            {
                this.Manager.PointerState = ButtonStateManager.ButtonPointerState.PointerOver;
                this.State = this.Manager.GetState();//State
            };
            this.ContentPresenter.PointerPressed += (s, e) =>
            {
                this.Manager.PointerState = ButtonStateManager.ButtonPointerState.Pressed;
                this.State = this.Manager.GetState();//State
            };
            this.ContentPresenter.PointerExited += (s, e) =>
            {
                this.Manager.PointerState = ButtonStateManager.ButtonPointerState.None;
                this.State = this.Manager.GetState();//State
            };
        }
    }
}