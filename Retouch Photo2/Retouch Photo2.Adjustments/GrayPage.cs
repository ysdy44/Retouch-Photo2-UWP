// Core:              
// Referenced:   
// Difficult:         ★★
// Only:              
// Complete:      ★★★
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "GrayAdjustment"/>.
    /// </summary>
    public sealed partial class GrayPage : IAdjustmentPage
    {
        //@Construct
        /// <summary>
        /// Initializes a GrayPage. 
        /// </summary>
        public GrayPage()
        {
            this.ConstructStrings();
        }
    }

    /// <summary>
    /// Page of <see cref = "GrayAdjustment"/>.
    /// </summary>
    public sealed partial class GrayPage : IAdjustmentPage
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("Adjustments_Gray");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.Gray;
        /// <summary> Gets the icon. </summary>
        public ControlTemplate Icon { get => GrayAdjustment.GenericIcon; set => GrayAdjustment.GenericIcon = value; }
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => null;
        /// <summary> Gets the text. </summary>
        public string Text { get => GrayAdjustment.GenericText; private set => GrayAdjustment.GenericText = value; }

        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        public IAdjustment GetNewAdjustment() => new GrayAdjustment();


        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset() { }
        /// <summary>
        /// <see cref="IAdjustmentPage"/>'s value follows the <see cref="IAdjustment"/>.
        /// </summary>
        public void Follow() { }

    }
}