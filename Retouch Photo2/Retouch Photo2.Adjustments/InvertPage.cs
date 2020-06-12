using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "InvertAdjustment"/>.
    /// </summary>
    public sealed partial class InvertPage : IAdjustmentGenericPage<InvertAdjustment>
    {
        //@Generic
        /// <summary> Gets IAdjustment's adjustment. </summary>
        public InvertAdjustment Adjustment { get; set; }

        //@Construct
        /// <summary>
        /// Initializes a InvertPage. 
        /// </summary>
        public InvertPage()
        {
            this.ConstructStrings();
        }
    }

    /// <summary>
    /// Page of <see cref = "InvertAdjustment"/>.
    /// </summary>
    public sealed partial class InvertPage : IAdjustmentGenericPage<InvertAdjustment>
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Invert");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.Invert;
        /// <summary> Gets the icon. </summary>
        public FrameworkElement Icon { get; } = new InvertIcon();
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => null;
        /// <summary> Gets the text. </summary>
        public string Text { get; private set; }

        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        public IAdjustment GetNewAdjustment() => new InvertAdjustment();

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset() { }
        /// <summary>
        /// <see cref="IAdjustmentPage"/>'s value follows the <see cref="IAdjustment"/>.
        /// </summary>
        /// <param name="adjustment"> The adjustment. </param>
        public void Follow(InvertAdjustment adjustment) { }
    }
}