using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        // FlowDirection
        private void ConstructFlowDirection()
        {
            bool isRightToLeft = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;

            base.FlowDirection = isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        // Strings
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

            this.PresetDocker.Title = resource.GetString("$MainPage_Preset");
            this.AddPresetDialog.Title = resource.GetString("$MainPage_NewDialog_Title");

            this.ClickTipTextBlock.Text = resource.GetString("$MainPage_Select_ClickTip");
            this.AllButton.Content = resource.GetString("$MainPage_Select_All");

            this.InitialTipTextBlock.Text = resource.GetString("$MainPage_InitialTip");
            {
                this.SampleTextBlock.Text = resource.GetString("$MainPage_Initial_Sample");
                this.NewTextBlock.Text = resource.GetString("$MainPage_Initial_New");
                this.PhotoTextBlock.Text = resource.GetString("$MainPage_Initial_Photo");
                this.DestopTextBlock.Text = resource.GetString("$MainPage_Initial_Destop");
            }

            this.NewControl.Content = resource.GetString("$MainPage_New");
            {
                this.AddDialog.Title = resource.GetString("$MainPage_NewDialog_Title");
                this.AddDialog.SecondaryButtonText = resource.GetString("$MainPage_NewDialog_Close");
                this.AddDialog.PrimaryButtonText = resource.GetString("$MainPage_NewDialog_Primary");
                this.SizePicker.WidthText = resource.GetString("$MainPage_SizePicker_Width");
                this.SizePicker.HeightText = resource.GetString("$MainPage_SizePicker_Height");
            }

            this.PicturesControl.Content = resource.GetString("$MainPage_Pictures");
            {
                this.PicturesPhotoControl.Content = resource.GetString("$MainPage_Pictures_Photo");
                this.PicturesDestopControl.Content = resource.GetString("$MainPage_Pictures_Destop");
                this.PicturesCloseControl.Content = resource.GetString("$MainPage_Pictures_Close");
            }

            this.RenameControl.Content = resource.GetString("$MainPage_Rename");
            {
                this.RenameTitleTextBlock.Text = resource.GetString("$MainPage_Rename_Title");
                this.RenameSubtitleTextBlock.Text = resource.GetString("$MainPage_Rename_Subtitle");
                this.RenameCloseControl.Content = resource.GetString("$MainPage_Rename_Close");
                this.RenameDialog.Title = resource.GetString("$MainPage_RenameDialog_Title");
                this.RenameDialog.SecondaryButtonText = resource.GetString("$MainPage_RenameDialog_Close");
                this.RenameDialog.PrimaryButtonText = resource.GetString("$MainPage_RenameDialog_Primary");
                this.RenameTextBox.PlaceholderText = resource.GetString("$MainPage_RenameDialog_TextBox_PlaceholderText");
                this.TextBoxTipTextBlock.Text = resource.GetString("$MainPage_RenameDialog_TextBoxTip");
            }

            this.DeleteControl.Content = resource.GetString("$MainPage_Delete");
            {
                this.DeleteTitleTextBlock.Text = resource.GetString("$MainPage_Delete_Title");
                this.DeleteSubtitleTextBlock.Text = resource.GetString("$MainPage_Delete_Subtitle");
                this.DeleteCloseControl.Content = resource.GetString("$MainPage_Delete_Close");
                this.DeletePrimaryControl.Content = resource.GetString("$MainPage_Delete_Primary");
            }

            this.DuplicateControl.Content = resource.GetString("$MainPage_Duplicate");
            {
                this.DuplicateTitleTextBlock.Text = resource.GetString("$MainPage_Duplicate_Title");
                this.DuplicateSubtitleTextBlock.Text = resource.GetString("$MainPage_Duplicate_Subtitle");
                this.DuplicateCloseControl.Content = resource.GetString("$MainPage_Duplicate_Close");
                this.DuplicatePrimaryControl.Content = resource.GetString("$MainPage_Duplicate_Primary");
            }
        }


        // InitialControl
        private void ConstructInitialControl()
        {
            this.InitialSampleButton.Click += async (s, e) =>
            {
                this.MainLayout.Count = 3;
                this.MainLayout.State = MainPageState.Main;
                await FileUtil.SaveSampleFile();

                // Projects 
                foreach (StorageFolder zipFolder in await FileUtil.FIndAllZipFolders())
                {
                    // [StorageFolder] --> [projectViewItem]
                    IProjectViewItem item = await FileUtil.ConstructProjectViewItem(zipFolder);
                    this.Items.Add(item);
                }
            };
            this.InitialAddButton.Click += (s, e) => this.ShowAddDialog();
            this.InitialPhotoButton.Click += async (s, e) => await this.NewFromPicture(PickerLocationId.PicturesLibrary);
            this.InitialDestopButton.Click += async (s, e) => await this.NewFromPicture(PickerLocationId.Desktop);
        }


        // DragAndDrop
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
                //e.DragUIOverride.Caption = App.resourceLoader.GetString("DropAcceptable_");
                e.DragUIOverride.IsCaptionVisible = e.DragUIOverride.IsContentVisible = e.DragUIOverride.IsGlyphVisible = true;
            };
        }


        // Present
        private async void ConstructPresetGridView()
        {
            if (this.PresetGridView.ItemsSource is null)
            {
                IEnumerable<Project> source = await Retouch_Photo2.XML.ConstructProjectsFile();
                if (source is null) return;

                this.PresetProjects = new ObservableCollection<Project>(source);
                this.PresetGridView.ItemsSource = this.PresetProjects;
            }
        }
        private void ConstructPresetDocker()
        {
            this.Star.Click += (s, e) => this.PresetDocker.Show();
        
            this.PresetDocker.SecondaryButtonClick += (s, e) => this.PresetDocker.Hide();
            this.PresetDocker.PrimaryButtonClick += (s, e) => this.AddPresetDialog.Show();

            this.PresetGridView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Project item)
                {
                    this.PresetDocker.Hide();
                    this.NewFromProject(item.Clone());
                }
            };

            this.AddPresetDialog.SecondaryButtonClick += (s, e) => this.AddPresetDialog.Hide();
            this.AddPresetDialog.PrimaryButtonClick += (s, e) =>
            {
                BitmapSize size = this.PresetSizePicker.Size;

                // Project
                Project project = new Project
                {
                    Width = (int)size.Width,
                    Height = (int)size.Height,
                };

                this.PresetProjects.Add(project);
            };
        }


        // AddDialog
        private void ConstructAddDialog()
        {
            this.AddDialog.SecondaryButtonClick += (s, e) => this.HideAddDialog();
            this.AddDialog.PrimaryButtonClick += (s, e) =>
            {
                this.HideAddDialog();

                BitmapSize size = this.SizePicker.Size;
                this.NewFromProject(new Project(size));
            };
        }
        private void ShowAddDialog() => this.AddDialog.Show();
        private void HideAddDialog()
        {
            this.MainLayout.Count = this.Items.Count;
            this.MainLayout.State = MainPageState.Main;

            this.AddDialog.Hide();
        }


        // PicturesControl
        private void ConstructPicturesControl()
        {
            this.PicturesPhotoButton.Tapped += async (s, e) => await this.NewFromPicture(PickerLocationId.PicturesLibrary);
            this.PicturesDestopButton.Tapped += async (s, e) => await this.NewFromPicture(PickerLocationId.Desktop);
            this.PicturesCloseButton.Tapped += (s, e) => this.MainLayout.State = MainPageState.Main;
        }


        // RenameDialog
        string _rename;
        private void ConstructRenameDialog()
        {
            this.RenameDialog.SecondaryButtonClick += (s, e) => this.HideRenameDialog();
            this.RenameDialog.PrimaryButtonClick += async (s, e) =>
            {
                this.LoadingControl.State = LoadingState.Loading;

                await this.RenameProjectViewItem(this._rename, this.RenameTextBox.Text);

                this.LoadingControl.State = LoadingState.None;
            };

            this.RenameTextBox.Loaded += (s, e) => this.RenameTextBox.Focus(FocusState.Programmatic);
        }
        private void ShowRenameDialog(IProjectViewItem item)
        {
            this.RenameDialog.Show();

            this._rename = item.Name;
            this.RenameTextBox.Text = item.Name;
            this.RenameTextBox.SelectAll();
            this.RenameTextBox.Focus(FocusState.Programmatic);
        }
        private void HideRenameDialog()
        {
            this.TextBoxTipTextBlock.Visibility = Visibility.Collapsed;

            this.MainLayout.State = MainPageState.Rename;

            this.RenameDialog.Hide();
        }


        // DeleteControl
        private void ConstructDeleteControl()
        {
            // Delete
            this.DeleteCloseButton.Tapped += (s, e) => this.MainLayout.State = MainPageState.Main;
            this.DeletePrimaryButton.Tapped += async (s, e) =>
            {
                this.LoadingControl.State = LoadingState.Loading;

                await this.DeleteProjectViewItems(this.SelectedItems.ToArray());

                this.LoadingControl.State = LoadingState.None;

                this.MainLayout.Count = this.Items.Count;
                this.MainLayout.State = MainPageState.Main;
            };
        }


        // DuplicateControl
        private void ConstructDuplicateControl()
        {
            // Duplicate
            this.DuplicateCloseButton.Tapped += (s, e) => this.MainLayout.State = MainPageState.Main;
            this.DuplicatePrimaryButton.Tapped += async (s, e) =>
            {
                this.LoadingControl.State = LoadingState.Loading;

                await this.DuplicateProjectViewItems(this.SelectedItems.ToArray());

                this.LoadingControl.State = LoadingState.None;

                this.MainLayout.State = MainPageState.Main;
            };
        }

    }
}