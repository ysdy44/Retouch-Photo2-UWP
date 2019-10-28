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

        /// <summary> <see cref = "Photo" />'s name. </summary>
        public string Name { get; private set; }
        /// <summary> <see cref = "Photo" />'s describe. </summary>
        public string Describe { get; private set; }
        /// <summary> <see cref = "Photo" />'s thumbnail uri. </summary>
        public Uri Uri { get; private set; }
        /// <summary> <see cref = "Photo" />'s file path. </summary>
        public string Path { get; private set; }
        /// <summary> <see cref = "Photo" />'s file date. </summary>
        public DateTimeOffset Time { get; private set; }


        public bool? SelectMode
        {
            get => this.selectMode;
            set
            {
                this.Instance.SetSelectMode(value);
                this.selectMode = value;
            }
        }
        private bool? selectMode = null;

        /// <summary> Instance control. </summary>
        public PhotoControl Instance
        {
            get
            {
                if (this.instance == null) this.instance = new PhotoControl(this);

                return this.instance;
            }
            private set => this.instance = value;
        }
        private PhotoControl instance;

        
        //@Static
        /// <summary> Occurs when tapped the RootGrid. </summary>
        public static EventHandler<Photo> ItemClick;
        /// <summary>
        /// Create a <see cref = "Photo" /> from <see cref = "StorageFile" />.
        /// </summary>
        /// <param name="flie">Photo's File</param>
        /// <param name="folderPath">Path</param>
        /// <returns> photo </returns>
        public static Photo CreatePhoto(StorageFile flie, string folderPath)
        {
            DateTimeOffset time = flie.DateCreated;

            //add a photo class
            return new Photo
            {
                Name = flie.DisplayName,
                Time = time,
                Describe = $"{time.Year}.{time.Month}.{time.Day}",
                Uri = new Uri($"{folderPath}/{flie.DisplayName}.png", UriKind.Relative),
                Path = flie.Path,
            };
        }
        
    }
}