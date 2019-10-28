namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a TagType from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created TagType. </returns>
        public static TagType CreateTagType(string type)
        {
            switch (type)
            {
                case "None": return TagType.None;
                case "Red": return TagType.Red;
                case "Orange": return TagType.Orange;
                case "Yellow": return TagType.Yellow;
                case "Green": return TagType.Green;
                case "Blue": return TagType.Blue;
                case "Purple": return TagType.Purple;

                default: return TagType.None;
            }
        }

    }
}