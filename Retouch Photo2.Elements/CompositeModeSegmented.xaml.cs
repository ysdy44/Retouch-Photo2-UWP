using FanKit.Transformers;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Represents the segmented that is used to select marquee composite mode type.
    /// </summary>
    public sealed partial class CompositeModeSegmented : UserControl
    {

        //@VisualState
        MarqueeCompositeMode _vsMode;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsMode)
                {
                    case MarqueeCompositeMode.New: return this.New;
                    case MarqueeCompositeMode.Add: return this.Add;
                    case MarqueeCompositeMode.Subtract: return this.Subtract;
                    //case MarqueeCompositeMode.Intersect: return this.Intersect;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Mode of <see cref = "CompositeModeSegmented" />. </summary>
        public MarqueeCompositeMode Mode
        {
            get  => (MarqueeCompositeMode)base.GetValue(ModeProperty);
            set => base.SetValue(ModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "CompositeModeSegmented.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(MarqueeCompositeMode), typeof(CompositeModeSegmented), new PropertyMetadata(MarqueeCompositeMode.New,(sender,e)=>
        {
            CompositeModeSegmented control = (CompositeModeSegmented)sender;

            if (e.NewValue is MarqueeCompositeMode value)
            {
                control._vsMode = value;
                control.VisualState = control.VisualState;//State
            }
        }));
               

        /// <summary> IsOpen of <see cref = "CompositeModeSegmented" />. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "CompositeModeSegmented.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(CompositeModeSegmented), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a CompositeModeSegmented. 
        /// </summary>
        public CompositeModeSegmented()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State

            this.NewButton.Click += (s, e) => this.Mode = MarqueeCompositeMode.New;
            this.AddButton.Click += (s, e) => this.Mode = MarqueeCompositeMode.Add;
            this.SubtractButton.Click += (s, e) => this.Mode = MarqueeCompositeMode.Subtract;
            //this.IntersectButton.Click += (s, e) => this.Mode = MarqueeCompositeMode.Intersect;
        }
        
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.NewToolTip.Content = resource.GetString("/ToolElements/CompositeMode_New");
            this.AddToolTip.Content = resource.GetString("/ToolElements/CompositeMode_Add");
            this.SubtractToolTip.Content = resource.GetString("/ToolElements/CompositeMode_Subtract");
            //this.IntersectToolTip.Content = resource.GetString("/ToolElements/CompositeMode_Intersect");
        }

    }
}