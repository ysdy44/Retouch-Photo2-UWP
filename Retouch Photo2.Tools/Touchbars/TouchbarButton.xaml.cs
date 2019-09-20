using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    public sealed partial class TouchbarButton : UserControl
    {

        //@VisualState
        bool _vsIsSelected;
        ClickMode _vsClickMode;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsSelected) return this.Selected;

                switch (this._vsClickMode)
                {
                    case ClickMode.Release: return this.Normal;
                    case ClickMode.Hover: return this.PointerOver;
                    case ClickMode.Press: return this.Pressed;
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        #region DependencyProperty


        /// <summary> Gets or sets the type of <see cref = "TouchbarButton" />. </summary>
        public TouchbarType Type
        {
            get { return (TouchbarType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TouchbarButton.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(TouchbarType), typeof(TouchbarSlider), new PropertyMetadata(TouchbarType.None));


        /// <summary> Gets or sets the group type of <see cref = "TouchbarButton" />. </summary>
        public TouchbarType GroupType
        {
            get { return (TouchbarType)GetValue(GroupTypeProperty); }
            set { SetValue(GroupTypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TouchbarButton.GroupType" /> dependency property. </summary>
        public static readonly DependencyProperty GroupTypeProperty = DependencyProperty.Register(nameof(GroupType), typeof(TouchbarType), typeof(TouchbarSlider), new PropertyMetadata(TouchbarType.None, (sender, e) =>
        {
            TouchbarButton con = (TouchbarButton)sender;

            if (e.NewValue is TouchbarType value)
            {
                con._vsIsSelected = (value == con.Type);
                con.VisualState = con.VisualState;//State
            }
        }));


        /// <summary> Get or set the string Unit for range elements. </summary>
        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TouchbarButton.Unit" /> dependency property. </summary>
        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(nameof(Unit), typeof(string), typeof(TouchbarSlider), new PropertyMetadata(string.Empty));


        /// <summary> Get or set the current value for a TouchbarButton. </summary>
        public int Number
        {
            get => this.number;
            set
            {
                this.TextBlock.Text = $"{value} {this.Unit}";
                this.number = value;
            }
        }
        private int number;


        #endregion


        //@Construct
        /// <summary>
        /// Construct a TouchbarButton.
        /// </summary>
        public TouchbarButton()
        {
            this.InitializeComponent();
            this.RootBorder.PointerEntered += (s, e) =>
            {
                this._vsClickMode = ClickMode.Hover;
                this.VisualState = this.VisualState;//State
            };
            this.RootBorder.PointerPressed += (s, e) =>
            {
                this._vsClickMode = ClickMode.Press;
                this.VisualState = this.VisualState;//State
            };
            this.RootBorder.PointerExited += (s, e) =>
            {
                this._vsClickMode = ClickMode.Release;
                this.VisualState = this.VisualState;//State
            };
                       
            this.RootBorder.Tapped += (s, e) =>
            {
                if (this._vsIsSelected)
                {
                    this.GroupType = TouchbarType.None;
                }
                else
                {
                    this.GroupType = this.Type;
                }
            };
        }
    }
}