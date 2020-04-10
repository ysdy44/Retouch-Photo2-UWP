using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
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


        //InitialControl
        private void ConstructInitialControl()
        {
            this.InitialAddButton.Tapped += (s, e) => this.ShowAddDialog();
            this.InitialPhotoButton.Tapped += async (s, e) => await this.NewFromPicture(PickerLocationId.PicturesLibrary);
            this.InitialDestopButton.Tapped += async (s, e) => await this.NewFromPicture(PickerLocationId.Desktop);
        }


        //Head & Select
        private void ConstructSelectHead()
        {
            this.MainLayout.SelectCheckBox.Unchecked += (s, e) => this.SelectAllProjectViewItems(SelectMode.None);
            this.MainLayout.SelectCheckBox.Checked += (s, e) =>
            {
                this.SelectAllProjectViewItems(SelectMode.UnSelected);

                this.RefreshSelectCount();
            };

            //Head
            this.RefreshButton.Tapped += async (s, e) => await this.LoadAllProjectViewItems();
            this.SettingButton.Tapped += (s, e) => this.Frame.Navigate(typeof(SettingPage));//Navigate     

            //Select
            this.SelectAllButton.Tapped += (s, e) =>
            {
                bool isAnyUnSelected = this.ProjectViewItems.Any(p => p.SelectMode == SelectMode.UnSelected);
                SelectMode mode = isAnyUnSelected ? SelectMode.Selected : SelectMode.UnSelected;
                this.SelectAllProjectViewItems(mode);

                this.RefreshSelectCount();
            };
        }


        //AddDialog
        private void ConstructAddDialog()
        {
            this.AddDialog.CloseButton.Click += (sender, args) => this.HideAddDialog();
            this.AddDialog.PrimaryButton.Click += (sender, args) =>
            {
                this.HideAddDialog();

                BitmapSize size = this.AddSizePicker.Size;
                this.NewFromSize(size);
            };
        }
        private void ShowAddDialog()
        {
            this.MainLayout.MainPageState = MainPageState.Dialog;
            this.AddDialog.Show();
        }
        private void HideAddDialog()
        {
            if (this.ProjectViewItems.Count == 0)
                this.MainLayout.MainPageState = MainPageState.Initial;
            else
                this.MainLayout.MainPageState = MainPageState.Main;

            this.AddDialog.Hide();
        }


        //PicturesControl
        private void ConstructPicturesControl()
        {
            this.PicturesPhotoButton.Tapped += async (s, e) => await this.NewFromPicture(PickerLocationId.PicturesLibrary);
            this.PicturesDestopButton.Tapped += async (s, e) => await this.NewFromPicture(PickerLocationId.Desktop);
            this.PicturesCancelButton.Tapped += (s, e) => this.MainLayout.MainPageState = MainPageState.Main;
        }


        //RenameDialog
        private void ShowRenameDialog(ProjectViewItem item)
        {
            this.MainLayout.MainPageState = MainPageState.Dialog;

            this.RenameDialog.Show();

            this.RenameTextBox.Text = item.Name;
            this.RenameDialog.PrimaryButton.Click -= async (sender, args) => await this.RenameProjectViewItem(item);
            this.RenameDialog.PrimaryButton.Click += async (sender, args) => await this.RenameProjectViewItem(item);
        }
        private void HideRenameDialog()
        {
            this.RenameTipTextBlock.Visibility = Visibility.Collapsed;

            this.MainLayout.MainPageState = MainPageState.Rename;

            this.RenameDialog.Hide();
        }


        //DeleteControl
        private void ConstructDeleteControl()
        {
            //Delete
            this.DeleteCancelButton.Tapped += (s, e) => this.MainLayout.MainPageState = MainPageState.Main;
            this.DeleteOKButton.Tapped += async (s, e) =>
            {
                this.LoadingControl.IsActive = true;

                IEnumerable<ProjectViewItem> items = from i in this.ProjectViewItems where i.SelectMode == SelectMode.Selected select i;
                await this.DeleteProjectViewItems(items.ToList());

                this.LoadingControl.IsActive = false;

                if (this.ProjectViewItems.Count == 0)
                    this.MainLayout.MainPageState = MainPageState.Initial;
                else
                    this.MainLayout.MainPageState = MainPageState.Main;
            };
        }


        //DuplicateControl
        private void ConstructDuplicateControl()
        {
            //Duplicate
            this.DuplicateCancelButton.Tapped += (s, e) => this.MainLayout.MainPageState = MainPageState.Main;
            this.DuplicateOKButton.Tapped += async (s, e) =>
            {
                this.LoadingControl.IsActive = true;

                IEnumerable<ProjectViewItem> items = from i in this.ProjectViewItems where i.SelectMode == SelectMode.Selected select i;
                await this.DuplicateProjectViewItems(items.ToList());

                this.LoadingControl.IsActive = false;

                this.MainLayout.MainPageState = MainPageState.Main;
            };
        }

    }
}