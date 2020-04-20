using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Image of <see cref="Photo">.
    /// </summary>
    public partial class Photo
    {

        //@Static
        /// <summary> Collection <see cref="Photo"/>s instances. </summary>
        public static ObservableCollection<Photo> Instances = new ObservableCollection<Photo>();
        /// <summary> FInd a <see cref="Photo"/> form a <see cref="Photocopier"/>. </summary>
        public static Photo FindFirstPhotocopier(Photocopier photocopier)=> Photo.Instances.FirstOrDefault(i => i.FolderRelativeId == photocopier.FolderRelativeId);


        /// <summary>
        /// Check duplicate <see cref="Photo"/>.
        /// If it exists, replace it, or insert it into the <see cref="Photo"/>s.
        /// </summary>
        /// <param name="photo"> The source photo. </param>
        public static void DuplicateChecking(Photo photo)
        {
            foreach (Photo p in Photo.Instances)
            {
                if (p.FolderRelativeId == photo.FolderRelativeId)
                {
                    photo = p;
                    return;
                }
            }

            Photo.Instances.Add(photo);//Photos
        }


        /// <summary>
        /// Find the first <see cref="Photo"/> by <see cref="Photocopier"/>.
        /// </summary>
        /// <param name="photocopier"> The source photocopier</param>
        /// <returns> The product photo. </returns>
        public static Photo FindFirstPhoto(Photocopier photocopier)
        {
            string id = photocopier.FolderRelativeId;
            return Photo.Instances.FirstOrDefault(i => i.FolderRelativeId == id);
        }

    }
}
