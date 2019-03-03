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
                    //this.Button.Opacity = value ? 1.0 : 0.5;
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
        
        /// <summary> 得到相应的项 </summary>
        public abstract EffectItem GetItem(EffectManager effectManager);
        /// <summary> 给当前类的页面来赋值 </summary>
        public abstract void SetPage(EffectManager effectManager);
        /// <summary> 重置参数 </summary>
        public abstract void Reset(EffectManager effectManager);

        #region Control

        ToggleSwitch ToggleSwitch;
        public bool ToggleSwitchIsOn=>this.ToggleSwitch.IsOn;
        public void ToggleSwitch_Loaded(object sender, RoutedEventArgs e) => this.ToggleSwitch = (ToggleSwitch)sender;        

        Button Button;
        public void Button_Loaded(object sender, RoutedEventArgs e) => this.Button = (Button)sender;

        #endregion

    }


    public abstract class EffectItem
    {
        public bool IsOn;
        public abstract ICanvasImage Render(ICanvasImage image);
    }
    public class EffectManager
    {
        public GaussianBlurEffectItem GaussianBlurEffectItem = new GaussianBlurEffectItem(); 
        public DirectionalBlurEffectItem DirectionalBlurEffectItem = new DirectionalBlurEffectItem();
        public OuterShadowEffectItem OuterShadowEffectItem = new OuterShadowEffectItem();

        public OutlineEffectItem OutlineEffectItem = new OutlineEffectItem();

        public EmbossEffectItem EmbossEffectItem = new EmbossEffectItem();
        public StraightenEffectItem StraightenEffectItem = new StraightenEffectItem();
        

        public ICanvasImage Render(ICanvasImage image)
        {
            if (this.GaussianBlurEffectItem.IsOn) image = this.GaussianBlurEffectItem.Render(image);
            if (this.DirectionalBlurEffectItem.IsOn) image = this.DirectionalBlurEffectItem.Render(image);
            if (this.OuterShadowEffectItem.IsOn) image = this.OuterShadowEffectItem.Render(image);

            if (this.OutlineEffectItem.IsOn) image = this.OutlineEffectItem.Render(image);

            if (this.EmbossEffectItem.IsOn) image = this.EmbossEffectItem.Render(image);
            if (this.StraightenEffectItem.IsOn) image = this.StraightenEffectItem.Render(image);

            return image;
        }
    }


    public enum EffectType
    {
        /// <summary> 高斯模糊 </summary>
        GaussianBlur,
        /// <summary> 定向模糊 </summary>
        DirectionalBlur,

        /// <summary> 外部投影 </summary>
        OuterShadow,

        /// <summary> 轮廓 </summary>
        Outline,

        /// <summary> 浮雕 </summary>
        Emboss,
        /// <summary> 拉直 </summary>
        Straighten
    }


    /*
     
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


    */
}
