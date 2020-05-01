using Retouch_Photo2.Controls;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Button of <see cref = "SelectionViewModel.StrokeBrush" />'s Color.
    /// </summary>
    public sealed partial class StrokeColorEllipseButton : UserControl
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public StrokeColorEllipseButton()
        {
            this.Content = new ColorEllipse
            (
                dataContext: this.SelectionViewModel,
                path: nameof(this.SelectionViewModel.StrokeBrush),
                dp: ColorEllipse.BrushProperty
            );

            this.Tapped += (s, e) =>
            {
                if (this.Parent is FrameworkElement placementTarget)
                {
                    DrawPage.StrokeColorShowAt(placementTarget);
                }
                else
                {
                    DrawPage.StrokeColorShowAt(this);
                }
            };
        }
    }
}