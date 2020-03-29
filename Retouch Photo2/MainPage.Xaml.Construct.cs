using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainPage" />. 
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //SettingViewModel
        private async Task ConstructSettingViewModel()
        {
            //Setting
            SettingViewModel setting = null;

            try
            {
                setting = await SettingViewModel.CreateFromLocalFile();
            }
            catch (Exception)
            {
            }

            if (setting != null)
            {
                this.SettingViewModel = setting;
            }

            ElementTheme theme = this.SettingViewModel.ElementTheme;
            ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);
        }


        //MainPage
        private void ConstructMainPage()
        {
            //Initial
            this.AddButton.Tapped += (s, e) => this.ShowAddDialog();
            this.PhotoButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.PicturesLibrary);
            this.DestopButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.Desktop);

            //Main
            this.MainControl.AddButton.Tapped += (s, e) => this.ShowAddDialog();
            this.MainControl.PicturesButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Pictures);
            this.MainControl.SaveButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Save);
            this.MainControl.ShareButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Share);
            this.MainControl.DeleteButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Delete);
            this.MainControl.DuplicateButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Duplicate);
            this.MainControl.FolderButton.Tapped += (s, e) => this.ShowFolderDialog();
            this.MainControl.MoveButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Move);

            //Second
            this.MainControl.SecondAddButton.Tapped += (s, e) => this.ShowAddDialog();
            this.MainControl.SecondPicturesButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Pictures);
            this.MainControl.SecondSaveButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Save);
            this.MainControl.SecondShareButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Share);
            this.MainControl.SecondDeleteButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Delete);
            this.MainControl.SecondDuplicateButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Duplicate);
            this.MainControl.SecondFolderButton.Tapped += (s, e) => this.ShowFolderDialog();
            this.MainControl.SecondMoveButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Move);

            //Pictures
            this.PicturesControl.PhotoButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.PicturesLibrary);
            this.PicturesControl.DestopButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.Desktop);
            this.PicturesControl.CancelButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Main);

            //Save
            this.SaveControl.CancelButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Main);

            //Share
            this.ShareControl.CancelButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Main);

            //Delete
            this.DeleteControl.CancelButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Main);

            //Duplicate
            this.DuplicateControl.CancelButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Main);
        }
        private void SetMainPageState(MainPageState pageState)
        {
            this._vsIsInitialVisibility = false;
            this._vsState = pageState;
            this.VisualState = this.VisualState;//State
        }
         

        //RightFlyout
        int _tempRightIndex;   
        private void ConstructRightFlyout()
        {
            this.RightSaveButton.Tapped += (s, e) => this.SetRightFlyoutState(MainPageState.Save);
            this.RightShareButton.Tapped += (s, e) => this.SetRightFlyoutState(MainPageState.Share);
            this.RightDeleteButton.Tapped += (s, e) => this.SetRightFlyoutState(MainPageState.Delete);
            this.RightDuplicateButton.Tapped += (s, e) => this.SetRightFlyoutState(MainPageState.Duplicate);

            this.RightFlyout.Closed += (s, e) =>
            {
                int temp = this._tempRightIndex;
                this._tempRightIndex = -1;

                if (temp >= 0)
                {
                    if (temp < this.Photos.Count)
                    {
                        switch (this._vsState)
                        {
                            case MainPageState.Save:
                            case MainPageState.Share:
                            case MainPageState.Delete:
                            case MainPageState.Duplicate:
                                this.Photos[temp].Control.SelectMode = PhotoSelectMode.Selected;
                                break;

                            default:
                                this.Photos[temp].Control.SelectMode = PhotoSelectMode.None;
                                break;
                        }
                    }
                }
            };
        }
        private void SetRightFlyoutState(MainPageState state)
        {
            this._vsIsInitialVisibility = false;
            this._vsState = state;
            this.VisualState = this.VisualState;//State

            this.RightFlyout.Hide();
        }


        //AddDialog
        private void ConstructAddDialog()
        {
            this.AddDialog.CloseButton.Click += (sender, args) =>
            {
                this.SetMainPageState(MainPageState.Main);

                this.AddDialog.Hide();
            };

            this.AddDialog.PrimaryButton.Click += (sender, args) =>
            {
                this.SetMainPageState(MainPageState.Main);

                this.AddDialog.Hide();

                BitmapSize size = this.AddSizePicker.Size;

                this.NewProjectFromSize(size);
            };
        }
        private void ShowAddDialog()
        {
            this._vsIsInitialVisibility = false;
            this._vsState = MainPageState.Add;
            this.VisualState = this.VisualState;//State

            this.AddDialog.Show();
        }


        //FolderDialog
        private void ConstructFolderDialog() { }
        private void ShowFolderDialog() { }
               
    }
}