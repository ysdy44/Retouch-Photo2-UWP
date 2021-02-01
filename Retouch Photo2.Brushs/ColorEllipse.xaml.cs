// Core:              ★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★★
// Complete:      ★
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a ellipse used to display a color.
    /// </summary>
    public sealed partial class ColorEllipse : UserControl
    {

        #region DependencyProperty


        /// <summary> Gets or sets the brush. </summary>
        public IBrush Brush
        {
            get  => (IBrush)base.GetValue(BrushProperty);
            set => base.SetValue(BrushProperty, value);
        }
        /// <summary> Identifies the <see cref = "ColorEllipse.Brush" /> dependency property. </summary>
        public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(nameof(Brush), typeof(IBrush), typeof(ColorEllipse), new PropertyMetadata(null, (sender, e) =>
        {
            ColorEllipse control = (ColorEllipse)sender;

            if (e.NewValue is IBrush value)
            {
                if (value.Type == BrushType.Color)
                {
                    control.Color = value.Color;
                }
            }
        }));

        /// <summary> Gets or sets the color. </summary>
        public Color Color
        {
            get => (Color)base.GetValue(ColorProperty);
            set => base.SetValue(ColorProperty, value);
        }
        /// <summary> Identifies the <see cref = "ColorEllipse.Color" /> dependency property. </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorEllipse), new PropertyMetadata(Colors.LightGray));


        #endregion

        //@Construct
        /// <summary>
        /// Initializes a ColorEllipse. 
        /// </summary>
        public ColorEllipse()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Initializes a ColorEllipse. 
        /// </summary>
        /// <param name="dataContext">  Gets or sets the data context for FrameworkElement to participate in data binding. </param>
        /// <param name="path"> The path string that constructs the path of the binding source property. </param>
        /// <param name="dp"> The dependency property identifier for the property that is bound by the data. </param>
        public ColorEllipse(object dataContext, string path, DependencyProperty dp) : this()
        {
            this.ConstructDataContext(dataContext, path, dp);
        }


        //DataContext
        /// <summary>
        /// Initializes a DataContext. 
        /// </summary>
        /// <param name="dataContext">  Gets or sets the data context for FrameworkElement to participate in data binding. </param>
        /// <param name="path"> The path string that constructs the path of the binding source property. </param>
        /// <param name="dp"> The dependency property identifier for the property that is bound by the data. </param>
        private void ConstructDataContext(object dataContext, string path, DependencyProperty dp)
        {
            this.DataContext = dataContext;

            // Create the binding description.
            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(path)
            };

            // Attach the binding to the target.
            this.SetBinding(dp, binding);
        }

    }
}