using Retouch_Photo2.Brushs;
using System;
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
                switch (this.SelectionViewModel.FillBrush.Type)
                {
                    case BrushType.Color:
                        this.FillColorPicker.Color = this.SelectionViewModel.FillBrush.Color;
                        break;
                }
                this.FillColorFlyout.ShowAt(placementTarget);
            };
            DrawPage.StrokeColorShowAt += (FrameworkElement placementTarget) =>
            {
                switch (this.SelectionViewModel.StrokeBrush.Type)
                {
                    case BrushType.Color:
                        this.StrokeColorPicker.Color = this.SelectionViewModel.StrokeBrush.Color;
                        break;
                }
                this.StrokeColorFlyout.ShowAt(placementTarget);
            };

            this.FillColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetColor(value, FillOrStroke.Fill);

                this.ViewModel.Invalidate();//Invalidate
            };
            this.StrokeColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetColor(value, FillOrStroke.Stroke);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        #endregion
               
    }
}