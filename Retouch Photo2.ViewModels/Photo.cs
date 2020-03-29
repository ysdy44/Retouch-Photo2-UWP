using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// MainPage items's Model Class .
    /// </summary>
    public class Photo
    {

        //@Static
        /// <summary> Occurs when tapped the photo. </summary>
        public static Action<Photo> ItemClick;
        /// <summary> Occurs when right-click input a photo. </summary>
        public static Action<Photo> RightTapped;

        /// <summary> Photo's name. </summary>
        public string Name { get; private set; }
        /// <summary> Photo's describe. </summary>
        public string Describe { get; private set; }
        /// <summary> Photo's thumbnail uri. </summary>
        public Uri Uri { get; private set; }
        /// <summary> Photo's zip file path. </summary>
        public string ZipFilePath { get; private set; }
        /// <summary> Photo's file date. </summary>
        public DateTimeOffset Time { get; private set; }
               
        /// <summary> Instance control. </summary>
        public PhotoControl Control { get; private set; }

        
        //@Construct
        /// <summary>
        /// Construct a Photo from <see cref = "StorageFile" />.
        /// </summary>
        /// <param name="flie">Photo's File</param>
        /// <param name="folderPath">Path</param>
        /// <returns> photo </returns>
        public Photo(StorageFile flie, string folderPath)
        {
            DateTimeOffset time = flie.DateCreated;

            string name = flie.DisplayName;
            string describe = $"{time.Year}.{time.Month}.{time.Day}";
            Uri uri = new Uri($"{folderPath}/{flie.DisplayName}.png", UriKind.Relative);
          
            this.Name = name;
            this.Time = time;
            this.Describe = describe;
            this.Uri = uri;
            this.ZipFilePath = flie.Path;

            this.Control = new PhotoControl
            {
                Titlle = name,
                ImageSource = uri
            };
            this.Control.Tapped += (s, e) => Photo.ItemClick?.Invoke(this);//Delegate
            this.Control.RightTapped += (s, e) => Photo.RightTapped?.Invoke(this);//Delegate
            this.Control.Holding += (s, e) => Photo.RightTapped?.Invoke(this);//Delegate

        }

    }
}