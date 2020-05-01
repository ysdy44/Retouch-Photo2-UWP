using Retouch_Photo2.Controls;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Button of <see cref = "SelectionViewModel.FillBrush" />'s Color.
    /// </summary>
    public sealed partial class FillColorEllipseButton : UserControl
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public FillColorEllipseButton()
        {
            this.Content = new ColorEllipse
            (
                dataContext: this.SelectionViewModel,
                path: nameof(this.SelectionViewModel.FillBrush),
                dp: ColorEllipse.BrushProperty
            );

            this.Tapped += (s, e) =>
            {
                if (this.Parent is FrameworkElement placementTarget)
                {
                    DrawPage.FillColorShowAt(placementTarget);
                }
                else
                {
                    DrawPage.FillColorShowAt(this);
                }
            };
        }
    }
}