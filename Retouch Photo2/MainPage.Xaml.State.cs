using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainPage" />. 
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        private bool IsLoading { set => this.LoadingControl.IsActive = value; }
    
        /// <summary> State of <see cref="MainPage"/>. </summary>
        public MainPageState State
        {
            get => this.state;
            set
            {
                this.RadiusAnimaPanel.CenterContent = this.GetCenterContent(value);

                bool isNone = value == MainPageState.None;

                this.SetInitialVisibility(isNone);

                if (isNone)
                {
                    this.SetTitleVisibility(true);
                }
                else
                {
                    bool isMain = value == MainPageState.Main;
                    this.SetTitleVisibility(isMain);
                    this.ForeachPhotoFileList(isMain);
                }

                this.state = value;
            }
        }
        private MainPageState state;


        MainControl MainControl = new MainControl();
        PicturesControl PicturesControl = new PicturesControl();
        SaveControl SaveControl = new SaveControl();
        ShareControl ShareControl = new ShareControl();
        DeleteControl DeleteControl = new DeleteControl();
        DuplicateControl DuplicateControl = new DuplicateControl();


        private UserControl GetCenterContent(MainPageState value)
        {
            switch (value)
            {
                case MainPageState.Main: return this.MainControl;

                case MainPageState.Pictures: return this.PicturesControl;

                case MainPageState.Save: return this.SaveControl;
                case MainPageState.Share: return this.ShareControl;

                case MainPageState.Delete: return this.DeleteControl;
                case MainPageState.Duplicate: return this.DuplicateControl;

                default: return null;
            }
        }

        private void SetInitialVisibility(bool isShow)
        {
            if (isShow)
            {
                this.InitialControl.Visibility = Visibility.Visible;
                this.GridView.Visibility = Visibility.Collapsed;
                this.RadiusAnimaPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.InitialControl.Visibility = Visibility.Collapsed;
                this.GridView.Visibility = Visibility.Visible;
                this.RadiusAnimaPanel.Visibility = Visibility.Visible;
            }
        }

        private void SetTitleVisibility(bool isShow)
        {
            if (isShow)
            {
                this.RefreshButton.Visibility = Visibility.Visible;
                this.SettingButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.RefreshButton.Visibility = Visibility.Collapsed;
                this.SettingButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ForeachPhotoFileList(bool isSelect)
        {
            if (isSelect)
            {
                foreach (Photo item in this.Photos)
                {
                    item.SelectMode = null;
                }
            }
            else
            {
                foreach (Photo item in this.Photos)
                {
                    item.SelectMode = false;
                }
            }
        }



        private async Task Refresh()
        {
            IEnumerable<StorageFile> orderedPhotos =await FileUtil.FindPhoto2pkFile();
           
            //Refresh, when the count is not equal.
            if (orderedPhotos.Count() != this.Photos.Count)
            {
                this.Photos.Clear(); //Notify
                this.GridView.Children.Clear();

                foreach (StorageFile storageFile in orderedPhotos)
                {
                    // [StorageFile] --> [Photo]
                    Photo photo = new Photo(storageFile, ApplicationData.Current.LocalFolder.Path);

                    if (photo != null)
                    {
                        this.Photos.Add(photo); //Notify
                        this.GridView.Children.Add(photo.Instance);
                    }
                }
            }

            bool isZero = (this.Photos.Count == 0);
            this.State = isZero ? MainPageState.None : MainPageState.Main;//State
        }

    }
}