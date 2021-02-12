using Retouch_Photo2.Edits;
using Retouch_Photo2.Elements;
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
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2
{ 
    public sealed partial class MainPage : Page
    {
        private string DisplayName = "Retouch Photo2";
        private string Untitled = "Untitled";
        private string DocumentationLink = "https://github.com/ysdy44/Retouch-Photo2-UWP-Documentation/blob/master/README.md";

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.DisplayName = resource.GetString("$DisplayName");
            this.Untitled = resource.GetString("$Untitled");
            this.DocumentationLink = resource.GetString("$DocumentationLink");

            this.TitleTextBlock.Text = resource.GetString("$MainPage_Title");
            this.DocumentationTipToolTip.Content = resource.GetString("$MainPage_DocumentationTip");
            this.SettingTipToolTip.Content = resource.GetString("$MainPage_SettingTip");

            this.ClickTipTextBlock.Text = resource.GetString("$MainPage_Select_ClickTip");
            this.AllButton.Content = resource.GetString("$MainPage_Select_All");

            this.InitialTipTextBlock.Text = resource.GetString("$MainPage_InitialTip");
            {
                this.SampleTextBlock.Text = resource.GetString("$MainPage_Initial_Sample");
                this.NewTextBlock.Text = resource.GetString("$MainPage_Initial_New");
                this.PhotoTextBlock.Text = resource.GetString("$MainPage_Initial_Photo");
                this.DestopTextBlock.Text = resource.GetString("$MainPage_Initial_Destop");
            }

            this.NewButton.Content = resource.GetString("$MainPage_New");
            {
                this.NewDialog.Title = resource.GetString("$MainPage_NewDialog_Title");
                this.NewDialog.CloseButton.Content = resource.GetString("$MainPage_NewDialog_Close");
                this.NewDialog.PrimaryButton.Content = resource.GetString("$MainPage_NewDialog_Primary");
                this.SizePicker.WidthText = resource.GetString("$MainPage_SizePicker_Width");
                this.SizePicker.HeightText = resource.GetString("$MainPage_SizePicker_Height");
            }

            this.PicturesButton.Content = resource.GetString("$MainPage_Pictures");
            {
                this.PicturesPhotoButton.Content = resource.GetString("$MainPage_Pictures_Photo");
                this.PicturesDestopButton.Content = resource.GetString("$MainPage_Pictures_Destop");
                this.PicturesCloseButton.Content = resource.GetString("$MainPage_Pictures_Close");
            }

            this.RenameButton.Content = resource.GetString("$MainPage_Rename");
            {
                this.RenameTitleTextBlock.Text = resource.GetString("$MainPage_Rename_Title");
                this.RenameSubtitleTextBlock.Text = resource.GetString("$MainPage_Rename_Subtitle");
                this.RenameCloseButton.Content = resource.GetString("$MainPage_Rename_Close");
                this.RenameDialog.Title = resource.GetString("$MainPage_RenameDialog_Title");
                this.RenameDialog.CloseButton.Content = resource.GetString("$MainPage_RenameDialog_Close");
                this.RenameDialog.PrimaryButton.Content = resource.GetString("$MainPage_RenameDialog_Primary");
                this.TextBox.PlaceholderText = resource.GetString("$MainPage_RenameDialog_TextBox_PlaceholderText");
                this.TextBoxTipTextBlock.Text = resource.GetString("$MainPage_RenameDialog_TextBoxTip");
            }

            this.DeleteButton.Content = resource.GetString("$MainPage_Delete");
            {
                this.DeleteTitleTextBlock.Text = resource.GetString("$MainPage_Delete_Title");
                this.DeleteSubtitleTextBlock.Text = resource.GetString("$MainPage_Delete_Subtitle");
                this.DeleteCloseButton.Content = resource.GetString("$MainPage_Delete_Close");
                this.DeletePrimaryButton.Content = resource.GetString("$MainPage_Delete_Primary");
            }
            
            this.DuplicateButton.Content = resource.GetString("$MainPage_Duplicate");
            {
                this.DuplicateTitleTextBlock.Text = resource.GetString("$MainPage_Duplicate_Title");
                this.DuplicateSubtitleTextBlock.Text = resource.GetString("$MainPage_Duplicate_Subtitle");
                this.DuplicateCloseButton.Content = resource.GetString("$MainPage_Duplicate_Close");
                this.DuplicatePrimaryButton.Content = resource.GetString("$MainPage_Duplicate_Primary");
            }
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
            if (this.SettingViewModel.Move == null) this.SettingViewModel.Move += (moveType) =>
            {
                switch (moveType)
                {
                    case FlyoutPlacementMode.Full: return;
                    case FlyoutPlacementMode.Left: this.ViewModel.CanvasTransformer.Position += new Vector2(50, 0); break;
                    case FlyoutPlacementMode.Top: this.ViewModel.CanvasTransformer.Position += new Vector2(0, 50); break;
                    case FlyoutPlacementMode.Right: this.ViewModel.CanvasTransformer.Position -= new Vector2(50, 0); break;
                    case FlyoutPlacementMode.Bottom: this.ViewModel.CanvasTransformer.Position -= new Vector2(0, 50); break;
                }
                this.ViewModel.CanvasTransformer.ReloadMatrix();
                this.ViewModel.Invalidate();//Invalidate
            };
            if (this.SettingViewModel.Edit == null) this.SettingViewModel.Edit += (editType) =>
            {
                switch (editType)
                {
                    case EditType.None: return;

                    case EditType.Edit_Cut: this.MethodViewModel.MethodEditCut(); break;
                    case EditType.Edit_Duplicate: this.MethodViewModel.MethodEditDuplicate(); break;
                    case EditType.Edit_Copy: this.MethodViewModel.MethodEditCopy(); break;
                    case EditType.Edit_Paste: this.MethodViewModel.MethodEditPaste(); break;
                    case EditType.Edit_Clear: this.MethodViewModel.MethodEditClear(); break;

                    case EditType.Select_All: this.MethodViewModel.MethodSelectAll(); break;
                    case EditType.Select_Deselect: this.MethodViewModel.MethodSelectDeselect(); break;
                    case EditType.Select_Invert: this.MethodViewModel.MethodSelectInvert(); break;

                    case EditType.Group_Group: this.MethodViewModel.MethodGroupGroup(); break;
                    case EditType.Group_UnGroup: this.MethodViewModel.MethodGroupUnGroup(); break;
                    case EditType.Group_Release: this.MethodViewModel.MethodGroupRelease(); break;
                };
                if (this.SettingViewModel.Undo == null) this.SettingViewModel.Undo += (undoType) =>
                {
                    switch (undoType)
                    {
                        case UndoType.Undo: this.MethodViewModel.MethodEditUndo(); break;
                        default: break;
                    }
                };
            };
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
            this.NewDialog.CloseButton.Click += (sender, args) => this.HideAddDialog();
            this.NewDialog.PrimaryButton.Click += (sender, args) =>
            {
                this.HideAddDialog();

                BitmapSize size = this.SizePicker.Size;
                this.NewFromSize(size);
            };
        }
        private void ShowAddDialog()
        {
            this.MainLayout.State = MainPageState.Dialog;
            this.NewDialog.Show();
        }
        private void HideAddDialog()
        {
            if (this.MainLayout.Count == 0)
                this.MainLayout.State = MainPageState.Initial;
            else
                this.MainLayout.State = MainPageState.Main;

            this.NewDialog.Hide();
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
                this.LoadingControl.State = LoadingState.Loading;
                this.LoadingControl.IsActive = true;

                await this.RenameProjectViewItem(this._rename, this.TextBox.Text);

                this.LoadingControl.State = LoadingState.None;
                this.LoadingControl.IsActive = false;
            };
        }
        private void ShowRenameDialog(IProjectViewItem item)
        {
            this.MainLayout.State = MainPageState.Dialog;

            this.RenameDialog.Show();

            this._rename = item.Name;
            this.TextBox.Text = item.Name;
            this.TextBox.Focus(FocusState.Keyboard);
            this.TextBox.SelectAll();
        }
        private void HideRenameDialog()
        {
            this.TextBoxTipTextBlock.Visibility = Visibility.Collapsed;

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
                this.LoadingControl.State = LoadingState.Loading;
                this.LoadingControl.IsActive = true;

                await this.DeleteProjectViewItems(this.MainLayout.SelectedItems);

                this.LoadingControl.State = LoadingState.None;
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
                this.LoadingControl.State = LoadingState.Loading;
                this.LoadingControl.IsActive = true;

                await this.DuplicateProjectViewItems(this.MainLayout.SelectedItems);

                this.LoadingControl.State = LoadingState.None;
                this.LoadingControl.IsActive = false;

                this.MainLayout.State = MainPageState.Main;
            };
        }

    }
}