using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Image of <see cref="ImageRe">.
    /// </summary>
    public partial class ImageRe
    {

        //@Static
        /// <summary> Collection images instances. </summary>
        public static ObservableCollection<ImageRe> Instances = new ObservableCollection<ImageRe>();
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

            ImageRe.Instances.Add(imageRe);//Images
        }


        /// <summary>
        /// Find the first ImageRe by ImageStr.
        /// </summary>
        /// <param name="imageStr"> The source ImageStr</param>
        /// <returns> The ImageRe. </returns>
        public static ImageRe FindFirstImage(ImageStr imageStr)
        {
            string id = imageStr.FolderRelativeId;
            return ImageRe.Instances.FirstOrDefault(i => i.FolderRelativeId == id);
        }

    }
}
