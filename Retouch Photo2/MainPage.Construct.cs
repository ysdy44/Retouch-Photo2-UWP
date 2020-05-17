using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
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
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Untitled = resource.GetString("/$MainPage/Untitled");

            this.TitleTextBlock.Text = resource.GetString("/$MainPage/Title");

            this.SelectItemsTextBlock.Text = resource.GetString("/$MainPage/Select_Items");
            this.SelectAllButton.Content = resource.GetString("/$MainPage/Select_All");

            this.InitialTextBlock.Text = resource.GetString("/$MainPage/Initial_Tip");
            this.InitialAddTextBlock.Text = resource.GetString("/$MainPage/Initial_Add");
            this.InitialPhotoTextBlock.Text = resource.GetString("/$MainPage/Initial_Photo");
            this.InitialDestopTextBlock.Text = resource.GetString("/$MainPage/Initial_Destop");

            this.AddButton.Content = resource.GetString("/$MainPage/Add");
            this.AddDialog.Title = resource.GetString("/$MainPage/AddDialog_Title");
            this.AddDialog.CloseButton.Content = resource.GetString("/$MainPage/AddDialog_Close");
            this.AddDialog.PrimaryButton.Content = resource.GetString("/$MainPage/AddDialog_Primary");
            this.AddSizePicker.WidthText = resource.GetString("/$MainPage/AddSizePicker_Width");
            this.AddSizePicker.HeightText = resource.GetString("/$MainPage/AddSizePicker_Height");

            this.PicturesButton.Content = resource.GetString("/$MainPage/Pictures");
            this.PicturesPhotoButton.Content = resource.GetString("/$MainPage/Pictures_Photo");
            this.PicturesDestopButton.Content = resource.GetString("/$MainPage/Pictures_Destop");
            this.PicturesCloseButton.Content = resource.GetString("/$MainPage/Pictures_Close");

            this.RenameButton.Content = resource.GetString("/$MainPage/Rename");
            this.RenameRunTitle.Text = resource.GetString("/$MainPage/RenameRun_Title");
            this.RenameRunContent.Text = resource.GetString("/$MainPage/RenameRun_Content");
            this.RenameCloseButton.Content = resource.GetString("/$MainPage/Rename_Close");
            this.RenameDialog.Title = resource.GetString("/$MainPage/RenameDialog_Title");
            this.RenameDialog.CloseButton.Content = resource.GetString("/$MainPage/RenameDialog_Close");
            this.RenameDialog.PrimaryButton.Content = resource.GetString("/$MainPage/RenameDialog_Primary");
            this.RenameTextBox.PlaceholderText = resource.GetString("/$MainPage/RenameDialog_PlaceholderText");
            this.RenameTipTextBlock.Text = resource.GetString("/$MainPage/RenameDialog_TipText");

            this.DeleteButton.Content = resource.GetString("/$MainPage/Delete");
            this.DeleteRunTitle.Text = resource.GetString("/$MainPage/DeleteRun_Title");
            this.DeleteRunContent.Text = resource.GetString("/$MainPage/DeleteRun_Content");
            this.DeleteCloseButton.Content = resource.GetString("/$MainPage/Delete_Close");
            this.DeletePrimaryButton.Content = resource.GetString("/$MainPage/Delete_Primary");

            this.DuplicateButton.Content = resource.GetString("/$MainPage/Duplicate");
            this.DuplicateRunTitle.Text = resource.GetString("/$MainPage/DuplicateRun_Title");
            this.DuplicateRunContent.Text = resource.GetString("/$MainPage/DuplicateRun_Content");
            this.DuplicateCloseButton.Content = resource.GetString("/$MainPage/Duplicate_Close");
            this.DuplicatePrimaryButton.Content = resource.GetString("/$MainPage/Duplicate_Primary");
        }


        //Setting
        private async Task ConstructSetting()
        {
            //Setting
            Setting setting = await XML.ConstructSettingFile();
            if (setting != null)
            {
                this.SettingViewModel.Setting = setting;
            }

            //Theme
            this.SettingViewModel.ConstructTheme();

            //DeviceLayout
            this.SettingViewModel.ConstructDeviceLayout();

            //MenuType
            this.SettingViewModel.ConstructMenuType(this.TipViewModel.Menus);

            //Key
            this.SettingViewModel.ConstructKey();
            if (this.SettingViewModel.Move == null)
            {
                this.SettingViewModel.Move += (value) =>
                {
                    this.ViewModel.CanvasTransformer.Position += value;
                    this.ViewModel.CanvasTransformer.ReloadMatrix();
                    this.ViewModel.Invalidate();//Invalidate
                };
            }
        }


        //InitialControl
        private void ConstructInitialControl()
        {
            this.InitialAddButton.Click += (s, e) => this.ShowAddDialog();
            this.InitialPhotoButton.Click += async (s, e) => await this.NewFromPicture(PickerLocationId.PicturesLibrary);
            this.InitialDestopButton.Click += async (s, e) => await this.NewFromPicture(PickerLocationId.Desktop);
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
            this.RefreshButton.Click += async (s, e) =>
            {
                await this.LoadAllProjectViewItems();
            };
            this.SettingButton.Click += (s, e) => this.Frame.Navigate(typeof(SettingPage));//Navigate     

            //Select
            this.SelectAllButton.Click += (s, e) =>
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
            this.PicturesPhotoButton.Click += async (s, e) => await this.NewFromPicture(PickerLocationId.PicturesLibrary);
            this.PicturesDestopButton.Click += async (s, e) => await this.NewFromPicture(PickerLocationId.Desktop);
            this.PicturesCloseButton.Click += (s, e) => this.MainLayout.MainPageState = MainPageState.Main;
        }


        //RenameDialog
        string _rename;
        private void ConstructRenameDialog()
        {
            this.RenameDialog.CloseButton.Click += (sender, args) => this.HideRenameDialog();
            this.RenameDialog.PrimaryButton.Click += async (sender, args) =>
            {
                await this.RenameProjectViewItem(this._rename, this.RenameTextBox.Name);
            };
        }
        private void ShowRenameDialog(ProjectViewItem item)
        {
            this.MainLayout.MainPageState = MainPageState.Dialog;

            this.RenameDialog.Show();

            this._rename = item.Name;
            this.RenameTextBox.Text = item.Name;
            this.RenameTextBox.Focus(FocusState.Keyboard);
            this.RenameTextBox.SelectAll();
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
            this.DeleteCloseButton.Click += (s, e) => this.MainLayout.MainPageState = MainPageState.Main;
            this.DeletePrimaryButton.Click += async (s, e) =>
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
            this.DuplicateCloseButton.Click += (s, e) => this.MainLayout.MainPageState = MainPageState.Main;
            this.DuplicatePrimaryButton.Click += async (s, e) =>
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