using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    ///  Represents a transition that can be switched between two states.
    /// </summary>
    public sealed partial class OnOffSwitch : UserControl
    { 
        
        #region DependencyProperty


        /// <summary> Gets or sets whether the status of the <see cref = "OnOffSwitch" /> is "on". </summary>
        public bool IsOn
        {
            get { return (bool)GetValue(IsOnProperty); }
            set { SetValue(IsOnProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OnOffSwitch.IsOn" /> dependency property. </summary>
        public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(nameof(IsOn), typeof(bool), typeof(OnOffSwitch), new PropertyMetadata(false,(sender,e)=>
        {
            OnOffSwitch con = (OnOffSwitch)sender;

            if (e.NewValue is bool value)
            {
                con._vsIsOn = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        /// <summary> Provides the object content that is displayed when the status of the <see cref = "OnOffSwitch" /> is off. </summary>
        public string OffContent
        {
            get { return (string)GetValue(OffContentProperty); }
            set { SetValue(OffContentProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OnOffSwitch.OffContent" /> dependency property. </summary>
        public static readonly DependencyProperty OffContentProperty = DependencyProperty.Register(nameof(OffContent), typeof(string), typeof(OnOffSwitch), new PropertyMetadata(null));


        /// <summary> Provides the object content that is displayed when the status of the <see cref = "OnOffSwitch" /> is on. </summary>
        public string OnContent
        {
            get { return (string)GetValue(OnContentProperty); }
            set { SetValue(OnContentProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OnOffSwitch.OnContent" /> dependency property. </summary>
        public static readonly DependencyProperty OnContentProperty = DependencyProperty.Register(nameof(OnContent), typeof(string), typeof(OnOffSwitch), new PropertyMetadata(null));


        #endregion


        //@VisualState
        bool _vsIsOn;
        public VisualState VisualState
        {
            get => (this._vsIsOn) ? this.On : this.Off;
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        public OnOffSwitch()
        {
            this.InitializeComponent();
            this.Loaded+=(s, e) => this.VisualState = this.VisualState;//State
           
            this.OnBorder.Tapped += (s, e) => this.IsOn = !this.IsOn;
            this.OffBorder.Tapped += (s, e) => this.IsOn = !this.IsOn;
        }
    }
}