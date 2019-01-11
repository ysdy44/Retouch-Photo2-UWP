using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using Retouch_Photo.Models;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Collections;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using Retouch_Photo.ViewModels;
using Windows.Graphics.Imaging;
using Windows.Storage;

namespace Retouch_Photo.Pages
{
    public sealed partial class DrawPage : Page
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        public DrawPage()
        {
            this.InitializeComponent();
            
            //Selection
            this.SelectionFlyout.Opened += (sender, e) => this.SelectionToggleButton.IsChecked = true;
            this.SelectionFlyout.Closed += (sender, e) => this.SelectionToggleButton.IsChecked = false;
            this.SelectionToggleButton.Tapped += (sender, e) =>
            {
                this.SelectionFlyout.ShowAt(this.SelectionToggleButton);
                this.SelectionControl.Initialize();
            }; 
           //Operate
            this.OperateFlyout.Opened += (sender, e) => this.OperateToggleButton.IsChecked = true;
            this.OperateFlyout.Closed += (sender, e) => this.OperateToggleButton.IsChecked = false;
            this.OperateToggleButton.Tapped += (sender, e) =>
            {
                this.OperateFlyout.ShowAt(this.OperateToggleButton);
            };             
           //Others
            this.OthersFlyout.Opened += (sender, e) => this.OthersToggleButton.IsChecked = true;
            this.OthersFlyout.Closed += (sender, e) => this.OthersToggleButton.IsChecked = false;
            this.OthersToggleButton.Tapped += (sender, e) =>
            {
                this.OthersFlyout.ShowAt(this.OthersToggleButton);
                this.OthersControl.Initialize();
            };
            //Color
            this.ColorPicker.ColorChange += (sender, color) =>
            {
                this.ViewModel.Color = color;

                Layer layer = this.ViewModel.CurrentLayer;
                if (layer != null)
                {
                    layer.ColorChanged(color);
                    layer.Invalidate();
                    this.ViewModel.Invalidate();
                }
            };
            this.ColorButton.Tapped += (sender, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = this.ViewModel.Color;
            };

            //SizeChanged
            this.MainCanvasControl.SizeChanged += (s, e) => this.ViewModel.MatrixTransformer.ControlSizeChanged(e.NewSize);
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.ViewModel.GoBack.IsGoBack) base.Frame.Navigate(typeof(MainPage));
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)//当前页面成为活动页面
        {
            if (e.Parameter is string text)
            {
                if (App.ViewModel.GoBack.HadGoBack(text))
                {
                    return;
                }
            }
            
            if (e.Parameter is XDocument document)   this.GetProject(Project.CreateFromXDocument(this.ViewModel.CanvasControl, document));

            if (e.Parameter is BitmapSize pixels)   this.GetProject(Project.CreateFromSize(this.ViewModel.CanvasControl, pixels));
            
            if (e.Parameter is StorageFile file) this.GetProject(await Project.CreateFromFileAsync(this.ViewModel.CanvasControl, file));
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)//当前页面不再成为活动页面
        {
        }

        public void GetProject(Project project)
        {
            this.LoadingControl.Visibility = Visibility.Visible;//Loading
            if (project == null)
            {
                base.Frame.GoBack();
                return;
            }
            this.ViewModel.LoadFromProject(project);
            this.LoadingControl.Visibility = Visibility.Collapsed;//Loading
        }


        private async void PopupButton_Tapped(object sender, TappedRoutedEventArgs e)  => await this.WelcomeContentDialog.ShowAsync(ContentDialogPlacement.InPlace);//ContentDialogPlacement.InPlace
        private async void Back_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.LoadingControl.Visibility = Visibility.Visible;//Loading

            await Task.Delay(333);

            this.LoadingControl.Visibility = Visibility.Collapsed;//Loading
            this.Frame.GoBack();
        }

        private void SaveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
        } 
    }




    public class PeopleSource : IIncrementalSource<string>
    {
        private readonly List<string> _people = new List<string>();

        public async Task<IEnumerable<string>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<string> result = new List<string>();
            for (int i = 1; i <= 20; i++)
            {
                int dss = pageIndex * pageSize + i;
                var p = "Person ：" + dss.ToString();
                result.Add(p);
            }
            await Task.Delay(10000);
            return result;
        }
    }


     

} 
