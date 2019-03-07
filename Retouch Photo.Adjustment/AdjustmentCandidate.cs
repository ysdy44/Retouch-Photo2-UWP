using Retouch_Photo.Adjustments.Models;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo.Adjustments
{
    /// <summary>
    /// AdjustmentCandidate: 调整候选人。
    /// 用于生成Adjustment类，和Adjustment类一一对应，并给它提供帮助。
    /// 避免性能浪费，所以不会多次实例化同类型的AdjustmentCandidate
    /// </summary>

    /// 新建Adjustment的方法
    /// [AdjustmentCandidate] --> [Adjustment]
    public abstract class AdjustmentCandidate
    {
        public AdjustmentType Type;
        public FrameworkElement Icon;
        public FrameworkElement Page;

        /// <summary> 生成一个新的Adjustment实例 </summary>
        public abstract Adjustment GetNewAdjustment();
        /// <summary> 给当前类的页面来赋值 </summary>
        public abstract void SetPage(Adjustment adjustment);

        //@static
        public static AdjustmentCandidate GetAdjustmentCandidate(AdjustmentType type) => AdjustmentCandidate.AdjustmentCandidateList.First(e => e.Type == type);
        public static List<AdjustmentCandidate> AdjustmentCandidateList = new List<AdjustmentCandidate>()
        {
            new GrayAdjustmentCandidate(),
            new InvertAdjustmentCandidate(),

            new ExposureAdjustmentCandidate(),
            new BrightnessAdjustmentCandidate(),
            new SaturationAdjustmentCandidate(),
            new HueRotationAdjustmentCandidate(),
            new ContrastAdjustmentCandidate(),
            new TemperatureAdjustmentCandidate(),

            new HighlightsAndShadowsAdjustmentCandidate(),
            new GammaTransferAdjustmentCandidate(),
            new VignetteAdjustmentCandidate(),
        };
    }
}
