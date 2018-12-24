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

            this.MainCanvasControl.SizeChanged += (s, e) => this.ViewModel.MatrixTransformer.ControlSizeChanged(e.NewSize);
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.GoBack.IsGoBack) base.Frame.Navigate(typeof(MainPage));
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)//当前页面成为活动页面
        {
            if (e.Parameter is string text)
            {
                if (this.ViewModel.GoBack.HadGoBack(text)) return;                
            }
            
            if (e.Parameter is XDocument document)
            {
                this.LoadingControl.Visibility = Visibility.Visible;//Loading                
                Project project = Project.CreateFromXDocument(this.ViewModel.CanvasControl, document);
                this.ViewModel.LoadFromProject(project);
                this.LoadingControl.Visibility = Visibility.Collapsed;//Loading
                return;
            }

            if (e.Parameter is BitmapSize pixels)
            {
                this.LoadingControl.Visibility = Visibility.Visible;//Loading
                Project project = Project.CreateFromSize(this.ViewModel.CanvasControl, pixels);
                this.ViewModel.LoadFromProject(project);
                this.LoadingControl.Visibility = Visibility.Collapsed;//Loading
                return;
            }

            if (e.Parameter is StorageFile file)
            {
                this.LoadingControl.Visibility = Visibility.Visible;//Loading
                Project project =await Project.CreateFromFileAsync(this.ViewModel.CanvasControl, file);
                if (project == null)
                {
                    base.Frame.GoBack();
                    return;
                }
                this.ViewModel.LoadFromProject(project);
                this.LoadingControl.Visibility = Visibility.Collapsed;//Loading
                return;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)//当前页面不再成为活动页面
        {
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
