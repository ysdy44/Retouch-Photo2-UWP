using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Image of <see cref="ImageRe">.
    /// </summary>
    public partial class ImageRe
    {

        //@Static
        /// <summary> Static images instances. </summary>
        public static Stack<ImageRe> Instances = new Stack<ImageRe>();
        /// <summary> FInd a ImageRe form a ImageStr. </summary>
        public static ImageRe FindFirstImageRe(ImageStr imageStr)=> ImageRe.Instances.FirstOrDefault(i => i.FolderRelativeId == imageStr.FolderRelativeId);


        /// <summary>
        /// Check duplicate ImageRe.
        /// If it exists, replace it, or insert it into the Images.
        /// </summary>
        /// <param name="imageRe"> The source ImageRe. </param>
        public static void DuplicateChecking(ImageRe imageRe)
        {
            foreach (ImageRe imageRe2 in ImageRe.Instances)
            {
                if (imageRe2.FolderRelativeId == imageRe.FolderRelativeId)
                {
                    imageRe = imageRe2;
                    return;
                }
            }

            ImageRe.Instances.Push(imageRe);//Images
        }


        public static ImageRe FindFirstImage(ImageStr imageStr)
        {
            string id = imageStr.FolderRelativeId;
            return ImageRe.Instances.FirstOrDefault(i => i.FolderRelativeId == id);
        }

    }
}
