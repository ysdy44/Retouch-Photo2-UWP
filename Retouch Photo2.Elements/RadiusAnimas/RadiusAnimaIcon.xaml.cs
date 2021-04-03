// Core:              ★★
// Referenced:   
// Difficult:         ★
// Only:              ★★
// Complete:      ★★
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// The shadow icon of the control will also follow the animation, 
    /// if you change the width of the contents of the control.
    /// </summary>  
    [TemplatePart(Name = nameof(RootGrid), Type = typeof(Grid))]
    [ContentProperty(Name = nameof(Content))]
    public sealed partial class RadiusAnimaIcon : ContentControl
    {
        //@Delegate
        /// <summary> Occurs when a pointer enters the hit test area of this element. </summary>
        public event RoutedEventHandler Toggled;


        Grid RootGrid;


        //@Construct
        /// <summary>
        /// Initializes a RadiusAnimaIcon.
        /// </summary>
        public RadiusAnimaIcon()
        {
            this.DefaultStyleKey = typeof(RadiusAnimaIcon);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.RootGrid != null)
            {
                this.RootGrid.Tapped -= this.RootGrid_Tapped;
                this.RootGrid.PointerEntered -= this.RootGrid_PointerEntered;
            }
            this.RootGrid = base.GetTemplateChild(nameof(RootGrid)) as Grid;
            if (this.RootGrid != null)
            {
                this.RootGrid.Tapped += this.RootGrid_Tapped;
                this.RootGrid.PointerEntered += this.RootGrid_PointerEntered;
            }
        }

        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Toggled?.Invoke(this, e);//Delegate 
        }
        private void RootGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                this.Toggled?.Invoke(this, e);//Delegate
            }
        }

    }
}