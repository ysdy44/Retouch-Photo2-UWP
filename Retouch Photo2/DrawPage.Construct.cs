using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();
            
            this.LoadingControl.Text = resource.GetString("/$DrawPage/Loading");

            this.DocumentButton.Content = resource.GetString("/$DrawPage/Document");

            this.ExportButton.Text = resource.GetString("/$DrawPage/Export");
            this.ExportToolTip.Content = resource.GetString("/$DrawPage/Export_ToolTip");
            //this.UndoButton.Text = resourceLoader.GetString("/$DrawPage/Undo");
            //this.UndoToolTip.Content = resourceLoader.GetString("/$DrawPage/Undo_ToolTip");
            //this.RedoButton.Text = resourceLoader.GetString("/$DrawPage/Redo");
            //this.RedoToolTip.Content = resourceLoader.GetString("/$DrawPage/Redo_ToolTip");
            this.SetupButton.Text = resource.GetString("/$DrawPage/Setup");
            this.SetupToolTip.Content = resource.GetString("/$DrawPage/Setup_ToolTip");
            this.RulerButton.Text = resource.GetString("/$DrawPage/Ruler");
            this.RulerToolTip.Content = resource.GetString("/$DrawPage/Ruler_ToolTip");
            this.FullScreenButton.Text = resource.GetString("/$DrawPage/FullScreen");
            this.FullScreenToolTip.Content = resource.GetString("/$DrawPage/FullScreen_ToolTip");
            this.TipButton.Text = resource.GetString("/$DrawPage/Tip");

            this.SetupDialog.Title = resource.GetString("/$DrawPage/SetupDialog_Title");
            this.SetupDialog.CloseButton.Content = resource.GetString("/$DrawPage/SetupDialog_Close");
            this.SetupDialog.PrimaryButton.Content = resource.GetString("/$DrawPage/SetupDialog_Primary");
            this.SetupSizePicker.WidthText = resource.GetString("/$DrawPage/SetupSizePicker_Width");
            this.SetupSizePicker.HeightText = resource.GetString("/$DrawPage/SetupSizePicker_Height");
            
            this.ExportDialog.Title = resource.GetString("/$DrawPage/ExportDialog_Title");
            this.ExportDialog.CloseButton.Content = resource.GetString("/$DrawPage/ExportDialog_Close");
            this.ExportDialog.PrimaryButton.Content = resource.GetString("/$DrawPage/ExportDialog_Primary");
        }


        //ViewModel
        private void ConstructViewModel()
        {
            this.MainCanvasControl.ConstructViewModel();
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


        //Save
        private void ConstructExportDialog()
        {
            this.ExportDialog.CloseButton.Click += (sender, args) => this.ExportDialog.Hide();

            this.ExportDialog.PrimaryButton.Click += async (_, __) =>
            {
                this.ExportDialog.Hide();

                this.LoadingControl.IsActive = true;
                bool isSuccesful = await this.Export();
                this.LoadingControl.IsActive = false;

                this.ViewModel.TextVisibility = Visibility.Visible;
                this.ViewModel.Text = isSuccesful ? "  ✔  " : "  ❌  ";
                await Task.Delay(2000);
                this.ViewModel.TextVisibility = Visibility.Collapsed;
            };
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

    }
}