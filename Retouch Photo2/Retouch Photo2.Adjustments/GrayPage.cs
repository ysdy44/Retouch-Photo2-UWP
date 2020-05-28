using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "GrayAdjustment"/>.
    /// </summary>
    public sealed partial class GrayPage : IAdjustmentGenericPage<GrayAdjustment>
    {
        //@Generic
        public GrayAdjustment Adjustment { get; set; }

        //@Construct
        public GrayPage()
        {
            this.ConstructStrings();
        }
    }

    /// <summary>
    /// Page of <see cref = "GrayAdjustment"/>.
    /// </summary>
    public sealed partial class GrayPage : IAdjustmentGenericPage<GrayAdjustment>
    {
        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Gray");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.Gray;
        public FrameworkElement Icon { get; } = new GrayIcon();
        public FrameworkElement Self => null;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new GrayAdjustment();


        public void Reset() { }
        public void Follow(GrayAdjustment adjustment) { }
    }
}