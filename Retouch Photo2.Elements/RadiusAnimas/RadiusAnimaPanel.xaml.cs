// Core:              ★★
// Referenced:   
// Difficult:         ★
// Only:              ★★
// Complete:      ★★
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// The shadow panel of the control will also follow the animation, 
    /// if you change the width of the contents of the control.
    /// </summary>
    [TemplatePart(Name = nameof(Storyboard), Type = typeof(Storyboard))]
    [TemplatePart(Name = nameof(Frame), Type = typeof(SplineDoubleKeyFrame))]
    [TemplatePart(Name = nameof(CornerRadius), Type = typeof(CornerRadius))]
    [ContentProperty(Name = nameof(Content))]
    public sealed class RadiusAnimaPanel : ContentControl
    {

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "RadiusAnimaPanel" />'s corner radius. </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)base.GetValue(CornerRadiusProperty);
            set => base.SetValue(CornerRadiusProperty, value);
        }
        /// <summary> Identifies the <see cref = "RadiusAnimaPanel.CornerRadius" /> dependency property. </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(RadiusAnimaPanel), new PropertyMetadata(new CornerRadius(25)));


        #endregion


        Storyboard Storyboard;
        SplineDoubleKeyFrame Frame;
        ContentPresenter ContentPresenter;


        //@Construct
        /// <summary>
        /// Initializes a RadiusAnimaPanel.
        /// </summary>
        public RadiusAnimaPanel()
        {
            this.DefaultStyleKey = typeof(RadiusAnimaPanel);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.Storyboard = base.GetTemplateChild(nameof(Storyboard)) as Storyboard;
            this.Frame = base.GetTemplateChild(nameof(Frame)) as SplineDoubleKeyFrame;

            if ((this.ContentPresenter is null) == false) this.ContentPresenter.SizeChanged -= this.ContentPresenter_SizeChanged;
            this.ContentPresenter = base.GetTemplateChild(nameof(ContentPresenter)) as ContentPresenter;
            if ((this.ContentPresenter is null) == false) this.ContentPresenter.SizeChanged += this.ContentPresenter_SizeChanged;
        }

        private void ContentPresenter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize == e.PreviousSize) return;
            this.Frame.Value = e.NewSize.Width;
            this.Storyboard.Begin();
        }

    }
}