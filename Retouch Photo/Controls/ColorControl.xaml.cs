using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
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
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Controls
{
    public sealed partial class ColorControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        #region DependencyProperty

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorControl), new PropertyMetadata(Colors.White,(sender,e)=>
        {          
            ColorControl con = (ColorControl)sender;

            if (con.isColor) return;
       
            if (e.NewValue  is Color value)
            {
                con.ColorPicker.Color = value;
            }
        }));

        #endregion


        public ColorControl()
        {
            this.InitializeComponent();
        }

        bool isColor;
        private void ColorPicker_ColorChange(object sender, Color value)
        {
            isColor = true;
            this.ViewModel.Color = value;

            Layer layer = this.ViewModel.CurrentLayer;
            if (layer != null)
            {
                layer.ColorChanged(value);
                layer.Invalidate();
                this.ViewModel.Invalidate();
            }
            isColor = false;
        }
    }
}
