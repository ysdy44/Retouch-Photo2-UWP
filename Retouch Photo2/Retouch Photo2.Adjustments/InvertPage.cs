// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              
// Complete:      ★★★
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "InvertAdjustment"/>.
    /// </summary>
    public sealed partial class InvertPage : IAdjustmentPage
    {
        //@Construct
        /// <summary>
        /// Initializes a InvertPage. 
        /// </summary>
        public InvertPage()
        {
            this.ConstructStrings();
            InvertAdjustment.GenericText = this.Text;
            InvertAdjustment.GenericPage = this;
        }
    }

    /// <summary>
    /// Page of <see cref = "InvertAdjustment"/>.
    /// </summary>
    public sealed partial class InvertPage : IAdjustmentPage
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("Adjustments_Invert");
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