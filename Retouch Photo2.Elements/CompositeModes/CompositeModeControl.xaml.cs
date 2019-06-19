using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements
{
    /// <summary> Control of <see cref = "CompositeMode" />. </summary>
    public sealed partial class CompositeModeControl : UserControl
    {
        //@Converter
        private SolidColorBrush NewToBackgroundConverter(CompositeMode mode) => (mode == Retouch_Photo2.Elements.CompositeMode.New) ? this.AccentColor : this.UnAccentColor;
        private SolidColorBrush NewToForegroundConverter(CompositeMode mode) => (mode == Retouch_Photo2.Elements.CompositeMode.New) ? this.CheckColor : this.UnCheckColor;

        private SolidColorBrush AddToBackgroundConverter(CompositeMode mode) => (mode == Retouch_Photo2.Elements.CompositeMode.Add) ? this.AccentColor : this.UnAccentColor;
        private SolidColorBrush AddToForegroundConverter(CompositeMode mode) => (mode == Retouch_Photo2.Elements.CompositeMode.Add) ? this.CheckColor : this.UnCheckColor;

        private SolidColorBrush SubtractToBackgroundConverter(CompositeMode mode) => (mode == Retouch_Photo2.Elements.CompositeMode.Subtract) ? this.AccentColor : this.UnAccentColor;
        private SolidColorBrush SubtractToForegroundConverter(CompositeMode mode) => (mode == Retouch_Photo2.Elements.CompositeMode.Subtract) ? this.CheckColor : this.UnCheckColor;

        private SolidColorBrush IntersectToBackgroundConverter(CompositeMode mode) => (mode == Retouch_Photo2.Elements.CompositeMode.Intersect) ? this.AccentColor : this.UnAccentColor;
        private SolidColorBrush IntersectToForegroundConverter(CompositeMode mode) => (mode == Retouch_Photo2.Elements.CompositeMode.Intersect) ? this.CheckColor : this.UnCheckColor;
        

        #region DependencyProperty
        
        /// <summary> Mode of <see cref = "CompositeModeControl" />. </summary>
        public Retouch_Photo2.Elements.CompositeMode Mode
        {
            get { return (Retouch_Photo2.Elements.CompositeMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "CompositeModeControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(Retouch_Photo2.Elements.CompositeMode), typeof(CompositeModeControl), new PropertyMetadata(Retouch_Photo2.Elements.CompositeMode.New));

        #endregion


        //@Construct
        public CompositeModeControl()
        {
            this.InitializeComponent();

            this.NewSegmented.Tapped += (s, e) => this.Mode = Retouch_Photo2.Elements.CompositeMode.New;
            this.AddSegmented.Tapped += (s, e) => this.Mode = Retouch_Photo2.Elements.CompositeMode.Add;
            this.SubtractSegmented.Tapped += (s, e) => this.Mode = Retouch_Photo2.Elements.CompositeMode.Subtract;
            this.IntersectSegmented.Tapped += (s, e) => this.Mode = Retouch_Photo2.Elements.CompositeMode.Intersect;
        }         
    }
}
