using Retouch_Photo2.Tools.Models;
using FanKit;

namespace Retouch_Photo2.Tools.Pages.ViewPages
{
    /// <summary>
    /// Converter of <see cref = "ViewTool"/>.
    /// </summary>
    public static class ViewConverter
    { 
        /*         
        [1] y = x * π / 180

        [2] -π<y<π , -180<x<180
        [3] y=1 , x=-9

        [4] value is x , radian is y
        [5] radian = value * π / 180
        [6] value = radian * 180 / π
        */
        public const double RadianDefult = 0;
        public const double RadianMinimum = -180;
        public const double RadianMaximum = 180;
        public static double RadianToValue(float radian) => radian * 180.0f / Math.Pi;
        public static float ValueToRadian(double value) => (float)value * Math.Pi / 180.0f;
        public static string RadianToString(float radian) => ((int)(radian * 180.0f / Math.Pi)).ToString() + "º";


        /*         
        [1] y = b/(c-x)

        [2] b = 10 , c = 1
        [3] 0.1<y<10 , -99<x<0
        [4] y=1 , x=-9

        [5] value is x , scale is y
        [6] scale = 10 / (1 - value)
        [7] value = 1 - 10 / scale
        */
        public const double ScaleDefult = -9;
        public const double ScaleMinimum = -99;
        public const double ScaleMaximum = 0;
        public static double ScaleToValue(float scale) => 1 - 10 / scale;
        public static float ValueToScale(double value) => (float)(10 / (1 - value));
        public static string ScaleToString(float scale) => ((int)(scale * 100)).ToString() + "%";
    }
}
