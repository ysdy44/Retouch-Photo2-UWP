using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.ComponentModel;
using Windows.UI.Xaml;
using Microsoft.Graphics.Canvas.Effects;
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
using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.Models.Blends;

namespace Retouch_Photo.Models
{
    /// <summary>
    /// Effect: 特效。
    /// 给图层提供特效。
    /// </summary>
    public abstract class Effect
    {
        public EffectType Type;
        public FrameworkElement Icon;
        /// <summary> 是否开启 </summary>
        bool IsOn;

        /// <summary> 重置参数 </summary>
        public abstract void Reset();
        public abstract ICanvasImage GetRender(ICanvasImage image);

    }

    public class EffectManger
    {


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
