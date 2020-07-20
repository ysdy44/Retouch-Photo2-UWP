using FanKit;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// Converter of <see cref = "ViewTool"/>.
    /// </summary>
    public static class ViewRadianConverter
    {
        /*         
        [1] y = x * π / 180

        [2] -π<y<π , -180<x<180
        [3] y=1 , x=-9

        [4] value is x , radian is y
        [5] radian = value * π / 180
        [6] value = radian * 180 / π
        */


        //Number 
        public const int DefultNumber = 0;
        public const int MinNumber = -180;
        public const int MaxNumber = 180;

        public static int RadianToNumber(float radian) => (int)(radian * 180.0f / Math.Pi);
        public static float NumberToRadian(int number) => number * Math.Pi / 180.0f;


        //Value
        public const double DefultValue = 0;
        public const double MinValue = -180;
        public const double MaxValue = 180;

        public static double RadianToValue(float radian) => radian * 180.0f / Math.Pi;
        public static float ValueToRadian(double value) => (float)value * Math.Pi / 180.0f;
    }
}