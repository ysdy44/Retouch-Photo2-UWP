using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.Layers;
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
            this.PhotoButton.Tapped += async (s, e) => await this.NewFromPicture(PickerLocationId.PicturesLibrary);
            this.DestopButton.Tapped += async (s, e) => await this.NewFromPicture(PickerLocationId.Desktop);

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
            this.PicturesControl.PhotoButton.Tapped += async (s, e) => await this.NewFromPicture(PickerLocationId.PicturesLibrary);
            this.PicturesControl.DestopButton.Tapped += async (s, e) => await this.NewFromPicture(PickerLocationId.Desktop);
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


        //ContextFlyout
        private string ContextText { get => this.ContextTextBlock.Text; set => this.ContextTextBlock.Text = value; }
        private void ConstructContextFlyout()
        {
            this.ConstructRenameDialog();
            this.ContextRenameButton.Tapped += (s, e) => this.ShowRenameDialog();

            this.ContextSaveButton.Tapped += (s, e) => this.SetContextFlyoutState(MainPageState.Save);
            this.ContextShareButton.Tapped += (s, e) => this.SetContextFlyoutState(MainPageState.Share);

            this.ContextDeleteButton.Tapped += (s, e) => this.SetContextFlyoutState(MainPageState.Delete);
            this.ContextDuplicateButton.Tapped += (s, e) => this.SetContextFlyoutState(MainPageState.Duplicate);

            this.ContextFlyout.Closed += (s, e) =>
            {
                ProjectViewItem item = this.ProjectViewItems.FirstOrDefault(p => p.Tittle == this.ContextText);
                this.ContextText = string.Empty;

                if (item == null) return;

                switch (this._vsState)
                {
                    case MainPageState.Save:
                    case MainPageState.Share:
                    case MainPageState.Delete:
                    case MainPageState.Duplicate:
                        item.SelectMode = SelectMode.Selected;
                        break;
                    default:
                        item.SelectMode = SelectMode.None;
                        break;
                }
            };
        }
        private void SetContextFlyoutState(MainPageState state)
        {
            this._vsIsInitialVisibility = false;
            this._vsState = state;
            this.VisualState = this.VisualState;//State

            this.ContextFlyout.Hide();
        }


        //RenameDialog
        private void ConstructRenameDialog()
        {
            this.RenameDialog.CloseButton.Click += (sender, args) =>
            {
                this.SetMainPageState(MainPageState.Main);

                this.RenameDialog.Hide();
            };

            this.RenameDialog.PrimaryButton.Click += (sender, args) =>
            {
                this.SetMainPageState(MainPageState.Main);

                this.RenameDialog.Hide();
            };
        }
        private void ShowRenameDialog()
        {
            this._vsIsInitialVisibility = false;
            this._vsState = MainPageState.Dialog;
            this.VisualState = this.VisualState;//State

            this.ContextFlyout.Hide();
            this.RenameDialog.Show();
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

                this.NewFromSize(size);
            };
        }
        private void ShowAddDialog()
        {
            this._vsIsInitialVisibility = false;
            this._vsState = MainPageState.Dialog;
            this.VisualState = this.VisualState;//State

            this.AddDialog.Show();
        }


        //FolderDialog
        private void ConstructFolderDialog() { }
        private void ShowFolderDialog() { }
               
    }
}