﻿using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.SettingPages;
using Retouch_Photo2.ViewModels;
using System;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SettingPage" />. 
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        //Layout
        PhoneLayout PhoneLayout = new PhoneLayout();
        PadLayout PadLayout = new PadLayout();
        PCLayout PCLayout = new PCLayout();
        DeviceLayoutType DeviceLayoutType
        {
            set
            {
                //Layout
                switch (value)
                {
                    case DeviceLayoutType.Adaptive: this.LayoutBorder.Child = this.PCLayout; break;
                    case DeviceLayoutType.Phone: this.LayoutBorder.Child = this.PhoneLayout; break;
                    case DeviceLayoutType.Pad: this.LayoutBorder.Child = this.PadLayout; break;
                    case DeviceLayoutType.PC: this.LayoutBorder.Child = this.PCLayout; break;
                }

                //Adaptive
                bool isAdaptive = (value == DeviceLayoutType.Adaptive);
                this.AdaptiveTextBlock.Opacity = isAdaptive ? 1.0 : 0.6;
                this.AdaptiveGrid.IsEnabled = isAdaptive;
                this.AdaptiveResetButton.IsEnabled = isAdaptive;
            }
        }


        //@Construct
        public SettingPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.BackButton.Tapped += (s, e) => this.Frame.GoBack();

            this.ConstructTheme();
            this.ConstructLayout();
            this.ConstructLayoutAdaptive();

            this.LocalButton.Tapped += async (s, e) =>
            {
                IStorageFolder folder = ApplicationData.Current.LocalFolder;
                await Launcher.LaunchFolderAsync(folder);
            };
        }


        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Theme
            this.NavigatedTheme();

            //Layout
            this.NavigatedLayout();

            //Adaptive            
            this.NavigatedLayoutAdaptive();
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

    }
}