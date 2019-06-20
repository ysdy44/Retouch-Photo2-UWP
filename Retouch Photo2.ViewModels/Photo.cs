using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.TestApp.Models
{
    /// <summary>
    /// <see cref="MainPage"/>items's Model Class .
    /// </summary>
    public class Photo : INotifyPropertyChanged
    {

        /// <summary> <see cref = "Photo" />'s name. </summary>
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.OnPropertyChanged(nameof(this.Name));//Notify
            }
        }
        private string name;

        /// <summary> <see cref = "Photo" />'s describe. </summary>
        public string Describe
        {
            get => this.describe;
            set
            {
                this.describe = value;
                this.OnPropertyChanged(nameof(this.Describe));//Notify
            }
        }
        private string describe;


        /// <summary> <see cref = "Photo" />'s thumbnail uri. </summary>
        public Uri Uri
        {
            get => this.uri;
            set
            {
                this.uri = value;
                this.OnPropertyChanged(nameof(this.Uri));//Notify
            }
        }
        private Uri uri;

        /// <summary> <see cref = "Photo" />'s file path. </summary>
        public string Path
        {
            get => this.path;
            set
            {
                this.path = value;
                this.OnPropertyChanged(nameof(this.Path));//Notify
            }
        }
        private string path;

        /// <summary> <see cref = "Photo" />'s file date. </summary>
        public DateTimeOffset Time
        {
            get => this.time;
            set
            {
                this.time = value;
                this.OnPropertyChanged(nameof(this.Time));//Notify
            }
        }
        private DateTimeOffset time;


        /// <summary> Darken the <see cref = "Photo" /> mask's visibility. </summary>
        public Visibility DarkenVisibility
        {
            get => this.darkenVisibility;
            set
            {
                this.darkenVisibility = value;
                this.OnPropertyChanged(nameof(this.DarkenVisibility));//Notify
            }
        }
        private Visibility darkenVisibility = Visibility.Collapsed;

        /// <summary> Darken the <see cref = "Photo" /> mask's visibility. </summary>
        public void Entered(object sender, PointerRoutedEventArgs e) => this.DarkenVisibility = Visibility.Visible;
        public void Exited(object sender, PointerRoutedEventArgs e) => this.DarkenVisibility = Visibility.Collapsed;


        /// <summary>
        /// Create a <see cref = "Photo" /> from <see cref = "StorageFile" />.
        /// </summary>
        /// <param name="flie">Photo's File</param>
        /// <param name="FolderPath">Path</param>
        /// <returns> photo </returns>
        public static Photo CreatePhoto(StorageFile flie, string FolderPath)
        {
            if (flie.FileType == ".photo")
            {
                DateTimeOffset time = flie.DateCreated;

                //add a photo class
                return new Photo()
                {
                    Name = flie.DisplayName,
                    Time = time,
                    Describe =  time.Year.ToString() + "." + time.Month.ToString() + "." + time.Day.ToString(),
                    Uri = new Uri(FolderPath + "/" + flie.DisplayName + ".png", UriKind.Relative),
                    Path = flie.Path,
                };
            }
            else return null;
        }


        //@Static
        /// <summary>
        /// Get <see cref = "StorageFile" /> from the destination <see cref = "StorageFolder" />.
        /// </summary>
        /// <param name="folder"> Destination Folder </param>
        /// <returns> files </returns>
        public async static Task<IOrderedEnumerable<StorageFile>> CreatePhotoFilesFromStorageFolder(StorageFolder folder)
        {
            //get all file.
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();

            //Sort by Time
            IOrderedEnumerable<StorageFile> orderedFiles = files.OrderByDescending(file => file.DateCreated);

            return orderedFiles;
        }



        //Notify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
