using Retouch_Photo2.Brushs;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {

        #region ColorPicker


        //@Static
        /// <summary>
        /// Displays the fill-color flyout relative to the specified element.
        /// </summary>
        /// <param name="FrameworkElement"> The element to be used as the target for the location of the flyout. </param>
        public static Action<FrameworkElement> FillColorShowAt;
        /// <summary>
        /// Displays the stroke-color flyout relative to the specified element.
        /// </summary>
        /// <param name="FrameworkElement"> The element to be used as the target for the location of the flyout. </param>
        public static Action<FrameworkElement> StrokeColorShowAt;


        //FillColor
        private void ConstructColorFlyout()
        {
            DrawPage.FillColorShowAt += (FrameworkElement placementTarget) =>
            {
                this.FillColorPicker.Color = this.SelectionViewModel.FillBrush.Color;
                this.FillColorFlyout.ShowAt(placementTarget);
            };
            DrawPage.StrokeColorShowAt += (FrameworkElement placementTarget) =>
            {
                this.StrokeColorPicker.Color = this.SelectionViewModel.StrokeBrush.Color;
                this.StrokeColorFlyout.ShowAt(placementTarget);
            };

            this.FillColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetFillColor(value);
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.SelectionViewModel.Color = value;
                        break;
                }

                this.ViewModel.Invalidate();//Invalidate
            };
            this.StrokeColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetStrokeColor(value);
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        this.SelectionViewModel.Color = value;
                        break;
                }

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        #endregion



    }
}