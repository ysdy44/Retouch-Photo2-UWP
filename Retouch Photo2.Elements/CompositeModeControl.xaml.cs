using FanKit.Transformers;
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
                con.ModeChanged(value);
            }
        }));

        private void ModeChanged(MarqueeCompositeMode mode)
        {
            bool isNew = (mode == MarqueeCompositeMode.New);
            this.NewSegmented.Background = isNew ? this.AccentColor : this.UnAccentColor;
            this.NewSegmented.Foreground = isNew ? this.CheckColor : this.UnCheckColor;

            bool isAdd = (mode == MarqueeCompositeMode.Add);
            this.AddSegmented.Background = isAdd ? this.AccentColor : this.UnAccentColor;
            this.AddSegmented.Foreground = isAdd ? this.CheckColor : this.UnCheckColor;

            bool isSubtract = (mode == MarqueeCompositeMode.Subtract);
            this.SubtractSegmented.Background = isSubtract ? this.AccentColor : this.UnAccentColor;
            this.SubtractSegmented.Foreground = isSubtract ? this.CheckColor : this.UnCheckColor;

            bool isIntersect = (mode == MarqueeCompositeMode.Intersect);
            this.IntersectSegmented.Background = isIntersect ? this.AccentColor : this.UnAccentColor;
            this.IntersectSegmented.Foreground = isIntersect ? this.CheckColor : this.UnCheckColor;
        }

        #endregion
        
        //@Construct
        public CompositeModeControl()
        {
            this.InitializeComponent();

            this.NewSegmented.Tapped += (s, e) => this.Mode = MarqueeCompositeMode.New;
            this.AddSegmented.Tapped += (s, e) => this.Mode = MarqueeCompositeMode.Add;
            this.SubtractSegmented.Tapped += (s, e) => this.Mode = MarqueeCompositeMode.Subtract;
            this.IntersectSegmented.Tapped += (s, e) => this.Mode = MarqueeCompositeMode.Intersect;
        }         
    }
}