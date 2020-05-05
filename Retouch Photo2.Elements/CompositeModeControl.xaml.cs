using FanKit.Transformers;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Control of <see cref = "MarqueeCompositeMode" />. 
    /// </summary>
    public sealed partial class CompositeModeControl : UserControl
    {

        #region DependencyProperty


        /// <summary> Mode of <see cref = "CompositeModeControl" />. </summary>
        public MarqueeCompositeMode Mode
        {
            get { return (MarqueeCompositeMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "CompositeModeControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(MarqueeCompositeMode), typeof(CompositeModeControl), new PropertyMetadata(MarqueeCompositeMode.New,(sender,e)=>
        {
            CompositeModeControl con = (CompositeModeControl)sender;

            if (e.NewValue is MarqueeCompositeMode value)
            {
                con._vsIMode = value;
                con.VisualState = con.VisualState;//State
            }
        }));
               

        /// <summary> IsOpen of <see cref = "CompositeModeControl" />. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "CompositeModeControl.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(CompositeModeControl), new PropertyMetadata(false));


        #endregion


        //@VisualState
        MarqueeCompositeMode _vsIMode;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsIMode)
                {
                    case MarqueeCompositeMode.New: return this.New;
                    case MarqueeCompositeMode.Add: return this.Add;
                    case MarqueeCompositeMode.Subtract: return this.Subtract;
                    case MarqueeCompositeMode.Intersect: return this.Intersect;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        public CompositeModeControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State

            this.NewBorder.Tapped += (s, e) => this.Mode = MarqueeCompositeMode.New;
            this.AddBorder.Tapped += (s, e) => this.Mode = MarqueeCompositeMode.Add;
            this.SubtractBorder.Tapped += (s, e) => this.Mode = MarqueeCompositeMode.Subtract;
            this.IntersectBorder.Tapped += (s, e) => this.Mode = MarqueeCompositeMode.Intersect;
        }
        
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.NewToolTip.Content = resource.GetString("/ToolElements/CompositeMode_New");
            this.AddToolTip.Content = resource.GetString("/ToolElements/CompositeMode_Add");
            this.SubtractToolTip.Content = resource.GetString("/ToolElements/CompositeMode_Subtract");
            this.IntersectToolTip.Content = resource.GetString("/ToolElements/CompositeMode_Intersect");
        }

    }
}