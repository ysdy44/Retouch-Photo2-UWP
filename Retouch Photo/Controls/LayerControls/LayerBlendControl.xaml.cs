using Retouch_Photo.Models;
using Retouch_Photo.Models.Blends;
using Retouch_Photo.ViewModels;
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

namespace Retouch_Photo.Controls.LayerControls
{
    public sealed partial class LayerBlendControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        #region DependencyProperty

        public int BlendIndex
        {
            get { return (int)GetValue(BlendIndexProperty); }
            set { SetValue(BlendIndexProperty, value); }
        }
        public static readonly DependencyProperty BlendIndexProperty = DependencyProperty.Register(nameof(BlendIndex), typeof(Layer), typeof(LayerBlendControl), new PropertyMetadata(null, (sender, e) =>
        {
            LayerBlendControl con = (LayerBlendControl)sender;

            if (e.NewValue is int value)
            {
                if (value <0) return;
                if (value >= con.ComboBox.Items.Count) return;

                if (con.ComboBox.SelectedIndex == value) return;

                con.ComboBox.SelectedIndex = value;
            }
        }));

        #endregion



        public LayerBlendControl()
        {
            this.InitializeComponent();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.ComboBox.ItemsSource = Blend.BlendList;

            this.ComboBox.SelectedIndex = 0;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.BlendIndex == this.ComboBox.SelectedIndex) return;

            this.BlendIndex = this.ComboBox.SelectedIndex;

            this.ViewModel.Invalidate();
        }

    }
}
