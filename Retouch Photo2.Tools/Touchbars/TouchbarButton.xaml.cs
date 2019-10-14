using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    public sealed partial class TouchbarButton : UserControl
    {
        //@Delegate  
        public EventHandler<bool> Toggle;


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


        /// <summary> Gets or sets the slider of <see cref = "TouchbarButton" />. </summary>
        public TouchbarSlider TouchbarSlider { get; set; }

        /// <summary> Gets or sets the selected state of <see cref = "TouchbarButton" />. </summary>
        public bool IsSelected
        {
            get => this._vsIsSelected;
            set
            {
                this._vsIsSelected=value;
                this.VisualState = this.VisualState;//State
            }
        }


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
            this.PointerEntered += (s, e) =>
            {
                this._vsClickMode = ClickMode.Hover;
                this.VisualState = this.VisualState;//State
            };
            this.PointerPressed += (s, e) =>
            {
                this._vsClickMode = ClickMode.Press;
                this.VisualState = this.VisualState;//State
            };
            this.PointerReleased += (s, e) =>
            {
                this._vsClickMode = ClickMode.Release;
                this.VisualState = this.VisualState;//State
            };
            this.PointerExited += (s, e) =>
            {
                this._vsClickMode = ClickMode.Release;
                this.VisualState = this.VisualState;//State
            };
                       
            this.Tapped += (s, e) =>
            {
                if (this._vsIsSelected)
                {
                    this.Toggle?.Invoke(this, false);//Delegate
                }
                else
                {
                    this.Toggle?.Invoke(this, true);//Delegate
                }
            };
        }
    }
}