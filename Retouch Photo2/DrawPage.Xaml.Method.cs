using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //ViewModel
        private void ConstructViewModel()
        {
            this.MainCanvasControl.ConstructViewModel();

            this.ViewModel.TipWidthHeight += (transformer, point, invalidateMode) =>
            {
                //Text
                {
                    Vector2 horizontal = transformer.Horizontal;
                    Vector2 vertical = transformer.Vertical;

                    int width = (int)horizontal.Length();
                    int height = (int)vertical.Length();

                    this.TipWRun.Text = width.ToString();
                    this.TipHRun.Text = height.ToString();
                }

                //Width Height
                {
                    Vector2 offset = this.DrawLayout.FullScreenOffset;
                    double x = offset.X + point.X + 10;
                    double y = offset.Y + point.Y - 50;

                    Canvas.SetLeft(this.TipToolTip, x);
                    Canvas.SetTop(this.TipToolTip, y);
                }

                switch (invalidateMode)
                {
                    case InvalidateMode.None: break;
                    case InvalidateMode.Thumbnail: this.TipToolTip.Visibility = Visibility.Collapsed; break;
                    case InvalidateMode.HD: this.TipToolTip.Visibility = Visibility.Visible; break;
                }
            };
        }
        //KeyboardViewModel
        private void ConstructKeyboardViewModel()
        {
            //Move
            if (this.KeyboardViewModel.Move == null)
            {
                this.KeyboardViewModel.Move += (value) =>
                {
                    this.ViewModel.CanvasTransformer.Position += value;
                    this.ViewModel.CanvasTransformer.ReloadMatrix();
                    this.ViewModel.Invalidate();//Invalidate
                };
            }

            //FullScreen
            if (this.KeyboardViewModel.FullScreenChanged == null)
            {
                this.KeyboardViewModel.FullScreenChanged += (isFullScreen) =>
                {
                    this.IsFullScreen = isFullScreen;
                    this.ViewModel.Invalidate();//Invalidate
                };
            }
        }


        //File
        private void ConstructFileButton()
        {
            this.HomeButton.Tapped += (s, e) =>
            {
                this.FileFlyout.Hide();
                this.NavigatedFrom();
            };
            this.SaveButton.Tapped += (s, e) =>
            {
                this.FileFlyout.Hide();
                if (this.ViewModel.Name == null)
                    this.ShowRenameDialog();
                else
                    this.Save();
            };
            this.RenameButton.Tapped += (s, e) =>
            {
                this.FileFlyout.Hide();
                this.ShowRenameDialog();
            };
            this.ExportButton.Tapped += (s, e) =>
            {
                this.FileFlyout.Hide();
                this.ShowExportDialog();
            };
            this.ShareButton.Tapped += (s, e) =>
            {
                this.FileFlyout.Hide();
                this.ShowShareDialog();
            };
            this.SetupButton.Tapped += (s, e) =>
            {
                this.FileFlyout.Hide();
                this.ShowSetupDialog();
            };
        }
        private void ConstructFileDialog()
        {
            this.ConstructRenameDialog();
            this.ConstructExportDialog();
            this.ConstructShareDialog();
            this.ConstructSetupDialog();
        }


        //Rename
        private void ConstructRenameDialog()
        {
            this.RenameDialog.CloseButton.Click += (sender, args) => this.RenameDialog.Hide();
            
            this.RenameDialog.PrimaryButton.Click += (_, __) =>
            {
                string name = this.TextBox.Text;

                if (name != null)
                {
                    if (name != string.Empty)
                    {
                        bool isExist = this.ViewModel.Photos.Any(p => p.Name == name);

                        if (isExist == false)
                        {
                            this.ViewModel.Name = name;
                            this.Save();
                            this.RenameDialog.Hide();
                            return;
                        }
                    }
                }
            };
        }        
        private void ShowRenameDialog()
        {
            this.RenameDialog.Show();
        }

        //Export
        private void ConstructExportDialog()
        {
            this.ExportDialog.CloseButton.Click += (sender, args) => this.ExportDialog.Hide();

            this.ExportDialog.PrimaryButton.Click += async (_, __) =>
            {
                FormatType type = this.ExportFormatComboBox.Format;

                this.Export(type);
            };
        }
        private void ShowExportDialog()
        {
            this.ExportDialog.Show();
        }

        //Share
        private void ConstructShareDialog()
        {
            this.ShareDialog.CloseButton.Click += (sender, args) => this.ShareDialog.Hide();

            this.ShareDialog.PrimaryButton.Click += async (_, __) =>
            {
                FormatType type = this.ShareFormatComboBox.Format;

                this.Share(type);
            };
        }
        private void ShowShareDialog()
        {
            this.ShareDialog.Show();
        }

        //Setup
        private void ConstructSetupDialog()
        {
            this.SetupDialog.CloseButton.Click += (sender, args) => this.SetupDialog.Hide();

            this.SetupDialog.PrimaryButton.Click += (_, __) =>
            {
                this.SetupDialog.Hide();

                BitmapSize size = this.SetupSizePicker.Size;

                this.ViewModel.CanvasTransformer.Width = (int)size.Width;
                this.ViewModel.CanvasTransformer.Height = (int)size.Height;

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ShowSetupDialog()
        {
            this.SetupDialog.Show();
        }

        //Save
        private async void Save()
        {
            string name = this.ViewModel.Name;
            int width = this.ViewModel.CanvasTransformer.Width;
            int height = this.ViewModel.CanvasTransformer.Height;

            await FileUtil.SaveImageRes();
            await FileUtil.SaveProject(this.ViewModel.Layers.RootLayers, name, width, height);
            await FileUtil.CreateFromDirectory(name);

            Func<Matrix3x2, ICanvasImage> renderAction = this.MainCanvasControl.Render;
            FileUtil.SaveThumbnailAsync(this.ViewModel.CanvasDevice, renderAction, name, width, height);
        }
        //Export
        private async void Export(FormatType type)
        {
            /*        
            CanvasBitmap cb=null;
            IStorageFolder Folder = Application.Current.Exit;


            StorageFile file = await Folder.CreateFileAsync(Name + ".jpg", CreationCollisionOption.GenerateUniqueName);
            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await cb.SaveAsync(fileStream, CanvasBitmapFileFormat.Jpeg);
            }
             */


            switch (type)
            {
                case FormatType.JPEG:
                    break;
                case FormatType.PNG:
                    break;
                case FormatType.BMP:
                    break;
                case FormatType.GIF:
                    break;
                case FormatType.TIFF:
                    break;
            }
        }
        //Share
        private async void Share(FormatType type)
        {

        }
        //Setup
        private void Setup(int width, int height)
        {

        }


        //Navigated
        private void NavigatedTo()
        {
            //Transition
            Vector2 offset = this.DrawLayout.FullScreenOffset;
            float width = this.DrawLayout.CenterChildWidth;
            float height = this.DrawLayout.CenterChildHeight;
            this.ViewModel.CanvasTransformer.TransitionDestination(offset, width, height);

            if (this.ViewModel.IsTransition == false)
            {
                this.NavigatedToComplete();
            }
            else
            {
                this.Transition = 0;
                this.TransitionStoryboard.Begin();//Storyboard
            }
        }
        private void NavigatedToComplete()
        {
            this.ViewModel.CanvasTransformer.Transition(1.0f);
            this.IsFullScreen = false;
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        private async void NavigatedFrom()
        {
            await FileUtil.DeleteCacheAsync();

            this.SelectionViewModel.SetModeNone();
            this.ViewModel.Layers.RootLayers.Clear();
            this.ViewModel.Layers.RootControls.Clear();

            this.IsFullScreen = true;
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

            await Task.Delay(400);
            this.Frame.GoBack();
        }

    }
}