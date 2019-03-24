using Retouch_Photo.Adjustments.Pages;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Adjustments
{
    /// <summary> An <see cref = "Adjustment"/> corresponds to a <see cref = "AdjustmentPage" />. </summary>
    public abstract  class AdjustmentPage : Page
    {
        public AdjustmentType Type;
        public FrameworkElement Icon;

        /// <summary> Return a new <see cref = "Adjustment"/>. </summary>
        public abstract Adjustment GetNewAdjustment();
        /// <summary> Return the current <see cref = "Adjustment"/>. </summary>
        public abstract Adjustment GetAdjustment();
        /// <summary> Assignment the current <see cref = "Adjustment"/>. </summary>
        public abstract void SetAdjustment(Adjustment adjustment);

        /// <summary> Call this method, when the <see cref = "AdjustmentPage" /> navigated. </summary>
        public abstract void Close();
        /// <summary> Make <see cref = "Adjustment"/> and <see cref = "AdjustmentPage" /> back to initial state. </summary>
        public abstract void Reset();


        //@static
        public static AdjustmentPage GetPage(AdjustmentType type) => AdjustmentPage.PageList.First(e => e.Type == type);
        public static List<AdjustmentPage> PageList = new List<AdjustmentPage>()
        {
            new GrayPage(),
            new InvertPage(),

            new ExposurePage(),
            new BrightnessPage(),
            new SaturationPage(),
            new HueRotationPage(),
            new ContrastPage(),
            new TemperaturePage(),

            new HighlightsAndShadowsPage(),
            new GammaTransferPage(),
            new VignettePage(),
        };
    }
}
