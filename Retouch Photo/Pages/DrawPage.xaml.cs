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
            this.LayoutBinging(this.SelectionLayout, this.SelectionToggleButton);
            //Operate
            this.LayoutBinging(this.OperateLayout, this.OperateToggleButton);
            //Adjustment
            this.LayoutBinging(this.AdjustmentLayout, this.AdjustmentToggleButton);
            //Effect
            this.LayoutBinging(this.EffectLayout, this.EffectToggleButton);
            //Transformer
            this.LayoutBinging(this.TransformerLayout, this.TransformerToggleButton);
            //Navigator
            this.LayoutBinging(this.NavigatorLayout, this.NavigatorToggleButton);
          
            //Color
            this.ColorButton.Tapped += (sender, e) => this.ColorLayout.ShowAt(this.ColorButton);

            //Layer
            this.LayersControl.FlyoutShow += (control) => this.LayerLayout.ShowAt(control);
            
            this.BackButton.Tapped += (sender, e) => this.Frame.GoBack();
            this.SaveButton.Tapped += (sender, e) => this.Frame.GoBack();
        }


        //Layout
        private void LayoutBinging(MenuLayout layout, ToggleButton button)
        {
            layout.Flyout.Opened += (sender, e) => button.IsChecked = true;
            layout.Flyout.Closed += (sender, e) => button.IsChecked = false;
            button.Tapped += (sender, e) => layout.ShowAt(button);
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)//当前页面成为活动页面
        {
            if (e.Parameter is Project project)
            {
                if (project == null)
                {
                    base.Frame.GoBack();
                    return;
                }

                this.Loaded += (sender, e2) =>
                {

                    this.LoadingControl.Visibility = Visibility.Visible;//Loading
                    this.ViewModel.LoadFromProject(project);//Project
                    this.LoadingControl.Visibility = Visibility.Collapsed;//Loading   

                    this.ViewModel.Invalidate();
                };
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)//当前页面不再成为活动页面
        {
        }

    } 
} 
