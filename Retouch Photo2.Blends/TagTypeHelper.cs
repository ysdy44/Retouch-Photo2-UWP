using Windows.UI;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Provides static TagType rendering method.
    /// </summary>
    public static class TagTypeHelper
    {
        
        /// <summary>
        /// Turn TagType into color.
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns></returns>
        public static Color TagConverter(TagType tagType)
        {
            switch (tagType)
            {
                case TagType.None: return Colors.Transparent;
                case TagType.Red: return Colors.LightCoral;
                case TagType.Orange: return Colors.Orange;
                case TagType.Yellow: return Colors.Yellow;
                case TagType.Green: return Colors.YellowGreen;
                case TagType.Blue: return Colors.SkyBlue;
                case TagType.Purple: return Colors.Plum;

                default: return Colors.LightGray;
            }
        }

    }
}