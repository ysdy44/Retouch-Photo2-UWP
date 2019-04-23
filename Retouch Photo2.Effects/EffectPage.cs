using Retouch_Photo2.Effects.Pages;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    /// <summary> An <see cref = "Effect"/> corresponds to a <see cref = "EffectPage" />. </summary>
    public abstract class EffectPage : Page
    {
        public EffectType Type;
        public Control Control;

        /// <summary> Manager of effects. </summary>
        public EffectManager EffectManager;

        /// <summary> Is the effect turned on or off?. </summary>
        public abstract bool GetIsOn(EffectManager manager);
        /// <summary> Turn effects on or off </summary>
        public abstract void SetIsOn(EffectManager manager, bool isOn);
        /// <summary> Assignment the current <see cref = "EffectManager"/>. </summary>
        public abstract void SetManager(EffectManager manager);

        /// <summary> Call this method, when the <see cref = "EffectPage" /> navigated. </summary>
        public void Close() => this.EffectManager = null;
        /// <summary> Make <see cref = "Effect"/> and <see cref = "EffectPage" /> back to initial state. </summary>
        public abstract void Reset();


        //@static
        public static List<EffectPage> PageList = new List<EffectPage>()
        {
            new GaussianBlurPage(),
            new DirectionalBlurPage(),
            new SharpenPage(),
            new OuterShadowPage(),

            new OutlinePage(),

            new EmbossPage(),
            new StraightenPage(),
        };
    }
}
