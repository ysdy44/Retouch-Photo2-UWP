// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using FanKit.Transformers;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
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
                    case MarqueeCompositeMode.New: return this.NewState;
                    case MarqueeCompositeMode.Add: return this.AddState;
                    case MarqueeCompositeMode.Subtract: return this.SubtractState;
                    //case MarqueeCompositeMode.Intersect: return this.IntersectState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Mode of <see cref = "CompositeModeSegmented" />. </summary>
        public MarqueeCompositeMode Mode
        {
            get => (MarqueeCompositeMode)base.GetValue(ModeProperty);
            set => base.SetValue(ModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "CompositeModeSegmented.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(MarqueeCompositeMode), typeof(CompositeModeSegmented), new PropertyMetadata(MarqueeCompositeMode.New, (sender, e) =>
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

            this.New.Click += (s, e) => this.Mode = MarqueeCompositeMode.New;
            this.Add.Click += (s, e) => this.Mode = MarqueeCompositeMode.Add;
            this.Subtract.Click += (s, e) => this.Mode = MarqueeCompositeMode.Subtract;
            //this.Intersect.Click += (s, e) => this.Mode = MarqueeCompositeMode.Intersect;

            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            if (ToolTipService.GetToolTip(this.New) is ToolTip toolTip0)
            {
                toolTip0.Content = resource.GetString($"Tools_Cursor_CompositeMode_New");
            }
            if (ToolTipService.GetToolTip(this.Add) is ToolTip toolTip1)
            {
                toolTip1.Content = resource.GetString($"Tools_Cursor_CompositeMode_Add");
            }
            if (ToolTipService.GetToolTip(this.Subtract) is ToolTip toolTip2)
            {
                toolTip2.Content = resource.GetString($"Tools_Cursor_CompositeMode_Subtract");
            }
            //if (ToolTipService.GetToolTip(this.Intersect) is ToolTip toolTip3)
            //{
            //    toolTip3.Content = resource.GetString($"Tools_Cursor_CompositeMode_Intersect");
            //}
        }
    }
}