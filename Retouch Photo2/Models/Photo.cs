using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.Storage;

namespace Retouch_Photo2.Models
{
    public class Photo : INotifyPropertyChanged
    {

        /// <summary>名称 </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string name;

        /// <summary>描述 </summary>
        public string Describe
        {
            get => describe;
            set
            {
                describe = value;
                OnPropertyChanged(nameof(Describe));
            }
        }
        private string describe;

        
        /// <summary>缩略图 </summary>
        public Uri Uri
        {
            get => uri;
            set
            {
                uri = value;
                OnPropertyChanged(nameof(Uri));
            }
        }
        private Uri uri;
        
        /// <summary>路径 </summary>
        public string Path;
        
        /// <summary>日期 </summary>
        public DateTimeOffset Time;
                

        /// <summary>图片变暗 </summary>
        public Visibility DarkenVisibility
        {
            get => darkenVisibility;
            set
            {
                darkenVisibility = value;
                OnPropertyChanged(nameof(DarkenVisibility));
            }
        }
        private Visibility darkenVisibility = Visibility.Collapsed;

        /// <summary>指针事件 </summary>
        public void Entered(object sender, PointerRoutedEventArgs e) => DarkenVisibility = Visibility.Visible;
        public void Exited(object sender, PointerRoutedEventArgs e) => DarkenVisibility = Visibility.Collapsed;

        /// <summary>
        /// Create a Photo from  StorageFile
        /// </summary>
        /// <param name="flie">Photo's File</param>
        /// <param name="FolderPath">Path</param>
        /// <returns></returns>
        public static Photo CreatePhoto(StorageFile flie, string FolderPath)
        {
            if (flie.FileType == ".photo")
            {

                DateTimeOffset time = flie.DateCreated;
                //添加
                return new Photo()
                {
                    Name = flie.DisplayName,
                    Time = time,
                    Describe = time.Year.ToString() + "." + time.Month.ToString() + "." + time.Day.ToString(),
                    Uri = new Uri(FolderPath + "/" + flie.DisplayName + ".png", UriKind.Relative),
                    Path = flie.Path,
                };
            }
            else return null;
        }

        public async static Task<IOrderedEnumerable<StorageFile>> CreatePhotoFiles(StorageFolder folder)
        { 
            //获取所有文件
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
           
            //文件夹按照时间排序
            IOrderedEnumerable<StorageFile> orderedFiles = files.OrderByDescending(file => file.DateCreated);

            return orderedFiles;
        }
        



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
