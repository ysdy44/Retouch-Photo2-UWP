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
        public InvertAdjustment Adjustment { get; set; }

        //@Construct
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
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Invert");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.Invert;
        public FrameworkElement Icon { get; } = new InvertIcon();
        public FrameworkElement Self => null;
        public string Text { get; private set; }

        public IAdjustment GetNewAdjustment() => new InvertAdjustment();

        public void Reset() { }
        public void Follow(InvertAdjustment adjustment) { }
    }
}