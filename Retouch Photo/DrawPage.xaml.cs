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

namespace Retouch_Photo
{
    public sealed partial class DrawPage : Page
    {


        public DrawPage()
        {
            this.InitializeComponent();
        }



        private async void PopupButton_Tapped(object sender, TappedRoutedEventArgs e) => await this.WelcomeContentDialog.ShowAsync(ContentDialogPlacement.InPlace);
        private void Back_Tapped(object sender, TappedRoutedEventArgs e)=> this.Frame.GoBack();



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
