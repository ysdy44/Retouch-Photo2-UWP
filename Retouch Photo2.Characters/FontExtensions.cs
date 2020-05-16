using Windows.UI.Text;

namespace Retouch_Photo2.Characters
{
    public static class FontExtensions
    {

        //FontWeight
        public static string ToString(this FontWeight fontWeight)
        {
            ushort weight = fontWeight.Weight;

            if (weight == FontWeights.Black.Weight) return "Black";
            if (weight == FontWeights.Bold.Weight) return "Bold";

            if (weight == FontWeights.ExtraBlack.Weight) return "ExtraBlack";
            if (weight == FontWeights.ExtraBold.Weight) return "ExtraBold";
            if (weight == FontWeights.ExtraLight.Weight) return "ExtraLight";

            if (weight == FontWeights.Light.Weight) return "Light";
            if (weight == FontWeights.Medium.Weight) return "Medium";
            if (weight == FontWeights.Normal.Weight) return "Normal";

            if (weight == FontWeights.SemiBold.Weight) return "SemiBold";
            if (weight == FontWeights.SemiLight.Weight) return "SemiLight";

            if (weight == FontWeights.Thin.Weight) return "Thin";

            return "Normal";
        }

    }
}