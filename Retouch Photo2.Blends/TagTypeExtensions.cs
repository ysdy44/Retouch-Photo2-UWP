// Core:              
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      
using Windows.UI;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Extensions of <see cref = "TagType" />.
    /// </summary>
    public static class TagTypeExtensions
    {

        /// <summary>
        /// Turn into color.
        /// </summary>
        /// <param name="tagType"> The source tag type. </param>
        /// <returns> The product color. </returns>
        public static Color ToColor(this TagType tagType)
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