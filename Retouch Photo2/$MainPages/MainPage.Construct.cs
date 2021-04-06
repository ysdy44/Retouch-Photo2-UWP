using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class MainPage : Page
    {

        //private string DisplayName = "Retouch Photo2";
        private string Untitled = "Untitled";
        private string DocumentationLink = "https://github.com/ysdy44/Retouch-Photo2-UWP-Documentation/blob/master/README.md";


        //FlowDirection
        private void ConstructFlowDirection()
        {
            bool isRightToLeft = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;

            base.FlowDirection = isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            //this.DisplayName = resource.GetString("$DisplayName");
            this.Untitled = resource.GetString("$Untitled");
            this.DocumentationLink = resource.GetString("$DocumentationLink");

            this.Head.Title = resource.GetString("$MainPage_Title");
            {
                this.Head.LeftButtonToolTip = resource.GetString("$MainPage_DocumentationTip");
                this.Head.RightButtonToolTip = resource.GetString("$MainPage_SettingTip");
            }

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
                this.AddDialog.Title = resource.GetString("$MainPage_NewDialog_Title");
                this.AddDialog.SecondaryButtonText = resource.GetString("$MainPage_NewDialog_Close");
                this.AddDialog.PrimaryButtonText = resource.GetString("$MainPage_NewDialog_Primary");
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
                this.RenameDialog.SecondaryButtonText = resource.GetString("$MainPage_RenameDialog_Close");
                this.RenameDialog.PrimaryButtonText = resource.GetString("$MainPage_RenameDialog_Primary");
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


        //InitialControl
        private void ConstructInitialControl()
        {
            this.InitialSampleButton.Click += async (s, e) =>
            {
                await FileUtil.SaveSampleFile();

                //Projects 
                foreach (StorageFolder folder in await FileUtil.FIndAllZipFolders())
                {
                    // [StorageFolder] --> [projectViewItem]
                    IProjectViewItem project = await FileUtil.ConstructProjectViewItem(folder);
                    this.Items.Add(project);
                }
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
            this.AddDialog.SecondaryButtonClick += (s, e) => this.HideAddDialog();
            this.AddDialog.PrimaryButtonClick += (s, e) =>
            {
                this.HideAddDialog();

                BitmapSize size = this.SizePicker.Size;
                this.NewFromSize(size);
            };
        }
        private void ShowAddDialog() => this.AddDialog.Show();
        private void HideAddDialog()
        {
            if (this.Items.Count == 0)
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
            this.RenameDialog.SecondaryButtonClick += (s, e) => this.HideRenameDialog();
            this.RenameDialog.PrimaryButtonClick += async (s, e) =>
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

                await this.DeleteProjectViewItems(this.SelectedItems);

                this.LoadingControl.State = LoadingState.None;
                this.LoadingControl.IsActive = false;

                if (this.Items.Count == 0)
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

                await this.DuplicateProjectViewItems(this.SelectedItems);

                this.LoadingControl.State = LoadingState.None;
                this.LoadingControl.IsActive = false;

                this.MainLayout.State = MainPageState.Main;
            };
        }

    }
}