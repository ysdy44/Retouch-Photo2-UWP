using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents the button that is used to combo box item.
    /// </summary>
    public sealed partial class ComboBoxButton : UserControl
    {

        #region DependencyProperty

        
        /// <summary> Gets or sets a value that declares whether the status is "Selected". </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ComboBoxButton.FillOrStroke" /> dependency property. </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(ComboBoxButton), new PropertyMetadata(false, (sender, e) =>
        {
            ComboBoxButton con = (ComboBoxButton)sender;

            if (e.NewValue is bool value)
            {
                con._vsIsSelected = value;
                con._vsClickMode = ClickMode.Release;
                con.VisualState = con.VisualState;//State
            }
        }));

        /// <summary> ContentPresenter's Content. </summary>
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }
     
        /// <summary> Gets or sets text content for TextBlock. </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ComboBoxButton.Text" /> dependency property. </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(ComboBoxButton), new PropertyMetadata(null));


        #endregion

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

        private ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@Construct
        /// <summary>
        /// Construct a ComboBoxButton.
        /// </summary>
        public ComboBoxButton()
        {
            this.InitializeComponent();
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }
        /// <summary>
        /// Construct a ComboBoxButton.
        /// </summary>
        public ComboBoxButton(object centerContent) : this()
        {
            this.ContentPresenter.Content = centerContent;
        }

    }
}