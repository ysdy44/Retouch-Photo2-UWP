// Core:              ★
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
            this.InitializeComponent();
            this.ConstructStrings();
        }
    }

    public sealed partial class InvertPage : IAdjustmentPage
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Title = resource.GetString("Adjustments_Invert");

            InvertAdjustment.GenericIcon = this.IconContentControl.Template;
            InvertAdjustment.GenericText = this.Title;
            InvertAdjustment.GenericPage = this;
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.Invert;
        /// <summary> Gets the icon. </summary>
        public ControlTemplate Icon => this.IconContentControl.Template;
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Title { get; private set; }

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