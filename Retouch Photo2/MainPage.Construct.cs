using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Imaging;
using Windows.Storage;
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
        private string Title = "Retouch Photo2";
        private string Untitled = "Untitled";
        private string DocumentationLink = "https://github.com/ysdy44/Retouch-Photo2-UWP-Documentation/blob/master/README.md";

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Title = resource.GetString("/resources/Title");
            this.Untitled = resource.GetString("/resources/Untitled");
            this.DocumentationLink = resource.GetString("/resources/DocumentationLink");


            this.TitleTextBlock.Text = this.Title;

            this.SelectItemsTextBlock.Text = resource.GetString("/$MainPage/Select_Items");
            this.SelectAllButton.Content = resource.GetString("/$MainPage/Select_All");

            this.InitialTextBlock.Text = resource.GetString("/$MainPage/Initial_Tip");
            this.InitialSampleTextBlock.Text = resource.GetString("/$MainPage/Initial_Sample");
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

            //DeviceLayout
            this.SettingViewModel.ConstructLayersHeight();
            
            //MenuType
            this.SettingViewModel.ConstructMenuType(this.TipViewModel.Menus);

            //Key
            this.SettingViewModel.ConstructKey();
            if (this.SettingViewModel.Move == null)
            {
                this.SettingViewModel.Move += (moveMode) =>
                {
                    switch (moveMode)
                    {
                        case MoveMode.None: return;
                        case MoveMode.Left: this.ViewModel.CanvasTransformer.Position += new Vector2(50, 0); break;
                        case MoveMode.Up: this.ViewModel.CanvasTransformer.Position += new Vector2(0, 50); break;
                        case MoveMode.Right: this.ViewModel.CanvasTransformer.Position -= new Vector2(50, 0); break;
                        case MoveMode.Down: this.ViewModel.CanvasTransformer.Position -= new Vector2(0, 50); break;
                    }
                    this.ViewModel.CanvasTransformer.ReloadMatrix();
                    this.ViewModel.Invalidate();//Invalidate
                };
            }
            if (this.SettingViewModel.Edit == null)
            {
                this.SettingViewModel.Edit += (editMode) =>
                {
                    switch (editMode)
                    {
                        case EditMode.None: return;

                        case EditMode.Cut: this.MethodViewModel.MethodEditCut(); break;
                        case EditMode.Duplicate: this.MethodViewModel.MethodEditDuplicate(); break;
                        case EditMode.Copy: this.MethodViewModel.MethodEditCopy(); break;
                        case EditMode.Paste: this.MethodViewModel.MethodEditPaste(); break;
                        case EditMode.Clear: this.MethodViewModel.MethodEditClear(); break;

                        case EditMode.All: this.MethodViewModel.MethodSelectAll(); break;
                        case EditMode.Deselect: this.MethodViewModel.MethodSelectDeselect(); break;
                        case EditMode.Invert: this.MethodViewModel.MethodSelectInvert(); break;

                        case EditMode.Group: this.MethodViewModel.MethodGroupGroup(); break;
                        case EditMode.UnGroup: this.MethodViewModel.MethodGroupUnGroup(); break;
                        case EditMode.Release: this.MethodViewModel.MethodGroupRelease(); break;

                        case EditMode.Undo: this.MethodViewModel.MethodEditUndo(); break;
                    }
                };
            }


        }


        //InitialControl
        private void ConstructInitialControl()
        {
            this.InitialSampleButton.Click += async (s, e) =>
            {
                await FileUtil.ConstructSampleFile();
                await this.LoadAllProjectViewItems();
            };
            this.InitialAddButton.Click += (s, e) => this.ShowAddDialog();
            this.InitialPhotoButton.Click += async (s, e) => await this.NewFromPicture(PickerLocationId.PicturesLibrary);
            this.InitialDestopButton.Click += async (s, e) => await this.NewFromPicture(PickerLocationId.Desktop);
        }


        //DragAndDrop
        private void ConstructDragAndDrop()
        {
            this.AllowDrop = true;
            this.Drop += async (s, e) =>
            {
                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {
                    IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();
                    if (items == null) return;
                    IStorageItem item = items.FirstOrDefault();
                    if (item == null) return;

                    await this.NewFromPicture(item);
                }
            };
            this.DragOver += (s, e) =>
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
                //e.DragUIOverride.Caption = App.resourceLoader.GetString("DropAcceptable_");//可以接受的图片
                e.DragUIOverride.IsCaptionVisible = e.DragUIOverride.IsContentVisible = e.DragUIOverride.IsGlyphVisible = true;
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
            this.MainLayout.State = MainPageState.Dialog;
            this.AddDialog.Show();
        }
        private void HideAddDialog()
        {
            if (this.MainLayout.Count == 0)
                this.MainLayout.State = MainPageState.Initial;
            else
                this.MainLayout.State = MainPageState.Main;

            this.AddDialog.Hide();
        }


        //PicturesControl
        private void ConstructPicturesControl()
        {
            this.PicturesPhotoButton.Click += async (s, e) => await this.NewFromPicture(PickerLocationId.PicturesLibrary);
            this.PicturesDestopButton.Click += async (s, e) => await this.NewFromPicture(PickerLocationId.Desktop);
            this.PicturesCloseButton.Click += (s, e) => this.MainLayout.State = MainPageState.Main;
        }


        //RenameDialog
        string _rename;
        private void ConstructRenameDialog()
        {
            this.RenameDialog.CloseButton.Click += (sender, args) => this.HideRenameDialog();
            this.RenameDialog.PrimaryButton.Click += async (sender, args) =>
            {
                await this.RenameProjectViewItem(this._rename, this.RenameTextBox.Text);
            };
        }
        private void ShowRenameDialog(IProjectViewItem item)
        {
            this.MainLayout.State = MainPageState.Dialog;

            this.RenameDialog.Show();

            this._rename = item.Name;
            this.RenameTextBox.Text = item.Name;
            this.RenameTextBox.Focus(FocusState.Keyboard);
            this.RenameTextBox.SelectAll();
        }
        private void HideRenameDialog()
        {
            this.RenameTipTextBlock.Visibility = Visibility.Collapsed;

            this.MainLayout.State = MainPageState.Rename;

            this.RenameDialog.Hide();
        }


        //DeleteControl
        private void ConstructDeleteControl()
        {
            //Delete
            this.DeleteCloseButton.Click += (s, e) => this.MainLayout.State = MainPageState.Main;
            this.DeletePrimaryButton.Click += async (s, e) =>
            {
                this.LoadingControl.IsActive = true;

                await this.DeleteProjectViewItems(this.MainLayout.SelectedItems);

                this.LoadingControl.IsActive = false;

                if (this.MainLayout.Count == 0)
                    this.MainLayout.State = MainPageState.Initial;
                else
                    this.MainLayout.State = MainPageState.Main;
            };
        }


        //DuplicateControl
        private void ConstructDuplicateControl()
        {
            //Duplicate
            this.DuplicateCloseButton.Click += (s, e) => this.MainLayout.State = MainPageState.Main;
            this.DuplicatePrimaryButton.Click += async (s, e) =>
            {
                this.LoadingControl.IsActive = true;

                await this.DuplicateProjectViewItems(this.MainLayout.SelectedItems);

                this.LoadingControl.IsActive = false;

                this.MainLayout.State = MainPageState.Main;
            };
        }

    }
}