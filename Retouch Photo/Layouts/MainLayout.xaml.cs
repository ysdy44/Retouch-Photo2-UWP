using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Layouts
{
    public sealed partial class MainLayout : Page
    {

        #region DependencyProperty

        /// <summary>
        /// <see cref="MainLayout"/>'s IsPaneOpen.
        /// </summary>
        public bool IsPaneOpen
        {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }
        public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register(nameof(IsPaneOpen), typeof(bool), typeof(MainLayout), new PropertyMetadata(false, new PropertyChangedCallback(IsPaneOpenOnChanged)));
        private static void IsPaneOpenOnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is MainLayout con)
            {
                if (e.NewValue is bool value)
                {
                    if (value)
                    {
                        con.BottomBorder.HorizontalAlignment = HorizontalAlignment.Center;
                        con.BottomBorder.BorderThickness = new Thickness(1, 1, 1, 0);
                    }
                    else
                    {
                        con.BottomBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
                        con.BottomBorder.BorderThickness = new Thickness(0, 1, 0, 0);
                    }
                }
            }
        }

        #endregion
        

        public UIElement CenterContent { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }

        public UIElement TopBar { get => this.TopBorder.Child; set => this.TopBorder.Child = value; }
        public UIElement BottomBar { get => this.BottomBorder.Child; set => this.BottomBorder.Child = value; }

        public MainLayout()
        {
            this.InitializeComponent();
        }
    }
}
