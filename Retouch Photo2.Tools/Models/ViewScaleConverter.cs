// Core:              ★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★★
namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// Converter of <see cref = "ViewTool"/>.
    /// </summary>
    public static class ViewScaleConverter
    {
        /*         
        [1] y = b/(c-x)

        [2] b = 10 , c = 1
        [3] 0.1<y<10 , -99<x<0
        [4] y=1 , x=-9

        [5] value is x , scale is y
        [6] scale = 10 / (1 - value)
        [7] value = 1 - 10 / scale
        */ 


        // Number 
        public const int MinNumber = 10;
        public const int MaxNumber =1000;

        public static int ScaleToNumber(float scale) => (int)(scale * 100.0f);
        public static float NumberToScale(int number) => number / 100.0f;


        // Value
        private const double DefultValue = -9;
        public const double MinValue = -99;
        public const double MaxValue = 0;

        public static double ScaleToValue(float scale) => 1f - 10f / scale;
        public static float ValueToScale(double value) => (float)(10f / (1f - value));
    }
}
