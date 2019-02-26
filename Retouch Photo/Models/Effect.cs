using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.ComponentModel;
using Windows.UI.Xaml;
using System.Xml.Linq;
using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models.Layers;
using Windows.Foundation;
using Retouch_Photo.ViewModels;
using Retouch_Photo.Models.Layers.GeometryLayers;
using Windows.Graphics.Effects;
using System.Numerics;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using Microsoft.Graphics.Canvas.UI;
using Retouch_Photo.Models.Effects;
using Retouch_Photo.Models.Blends;
using Retouch_Photo.Pages.EffectPages;

namespace Retouch_Photo.Models
{

    public abstract class Effect
    {
        /// <summary> 是否开启 </summary>
        private bool isOn;
        public bool IsOn
        {
            get => isOn;
            set
            {
                if (this.Button != null)
                {
                    this.Button.IsEnabled = value;
                    this.Button.Opacity = value ? 1.0 : 0.5;
                }

                if (this.ToggleSwitch != null)
                    if (this.ToggleSwitch.IsOn != value)
                        this.ToggleSwitch.IsOn = value;

                this.isOn = value;
            }
        }

        public EffectType Type;
        public FrameworkElement Icon;
        public FrameworkElement Page;

        /// <summary> 设置参数 </summary>
        public abstract void Set(EffectManager effectManager);
        /// <summary> 重置参数 </summary>
        public abstract void Reset(EffectManager effectManager);
        /// <summary> 给当前类的页面来赋值 </summary>
        public abstract void SetPage(EffectManager effectManager);

        #region Control

        public ToggleSwitch ToggleSwitch;
        public void ToggleSwitch_Loaded(object sender, RoutedEventArgs e) => this.ToggleSwitch = (ToggleSwitch)sender;        

        public Button Button;
        public void Button_Loaded(object sender, RoutedEventArgs e) => this.Button = (Button)sender;

        public void Open(bool isOn)
        {
            this.IsOn = isOn;

            if (this.ToggleSwitch != null)
                this.ToggleSwitch.IsEnabled = true;
        }
        public void Close()
        {
            this.IsOn = false;

            if (this.ToggleSwitch != null)
                this.ToggleSwitch.IsEnabled = false;
        }

        #endregion

    }

    public class EffectManager
    {
        public float BlurAmount;
        public bool GaussianBlurEffectIsOn;
        
        public ICanvasImage Render(ICanvasImage image)
        {
            if (this.GaussianBlurEffectIsOn) image = new Microsoft.Graphics.Canvas.Effects.GaussianBlurEffect
            {
                Source = image,
                BlurAmount = this.BlurAmount
            };

            return image;
        }
    }

    public enum EffectType
    {
        /// <summary> 高斯模糊 </summary>
        GaussianBlur,

        /// <summary> 外部投影 </summary>
        OuterShadow,
        /// <summary> 内部投影 </summary>
        InnerShadow,

        /// <summary> 外部发光 </summary>
        OuterGlow,
        /// <summary> 内部发光 </summary>
        InnerGlow,

        /// <summary> 轮廓 </summary>
        Outline,
        /// <summary> 3D </summary>
        ThreeDimensional,
        /// <summary> 斜面/凸出 </summary>
        BevelEmboss,

        /// <summary> 颜色覆盖 </summary>
        ColorOverlay,
        /// <summary> 渐变覆盖 </summary>
        GradientOverlay,
    }
    
}
