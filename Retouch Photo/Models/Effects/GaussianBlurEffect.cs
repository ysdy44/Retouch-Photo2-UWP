using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.EffectControls;
using Retouch_Photo.Pages.EffectPages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Models.Effects
{
    public class GaussianBlurEffect : Effect
    {
        public GaussianBlurPage page = new GaussianBlurPage();

        public GaussianBlurEffect()
        {
            base.Type = EffectType.GaussianBlur;
            base.Icon = new GaussianBlurControl();
            base.Page = this.page;
        }

        public override void Set(EffectManager effectManager)
        {
            bool value = base.ToggleSwitch.IsOn;
            base.IsOn = value;
            effectManager.GaussianBlurEffectIsOn = value;
        }
        public override void Reset(EffectManager effectManager)
        {
            this.page.EffectManager = null;

            effectManager.BlurAmount = 0;

            this.SetPage(effectManager);
        }
        public override void SetPage(EffectManager effectManager)
        {
            this.page.EffectManager = effectManager;
        }



    }
}

