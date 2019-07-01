using Retouch_Photo2.Adjustments.Pages;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// <see cref = "Adjustment"/>'s page.
    /// </summary>
    public abstract class AdjustmentPage : Page
    {
        /// <summary> <see cref = "AdjustmentPage"/>'s type. </summary>
        public AdjustmentType Type;
        /// <summary> <see cref = "AdjustmentPage"/>'s icon. </summary>
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


        //@Static
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
