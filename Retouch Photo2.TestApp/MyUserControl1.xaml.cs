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

namespace Retouch_Photo2.TestApp
{
    public sealed partial class MyUserControl1 : UserControl
    {

        //@Delegate 
        public EventHandler<Rect> RectChanged;

        public MyUserControl1()
        {
            this.InitializeComponent();
        }

        public Rect Rect => new Rect(this.X.Value, this.Y.Value, this.W.Value, this.H.Value);

        private void X_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.RectChanged?.Invoke(this, this.Rect); //Delegate
        }
        private void Y_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.RectChanged?.Invoke(this, this.Rect); //Delegate
        }

        private void H_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.RectChanged?.Invoke(this, this.Rect); //Delegate
        }
        private void W_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.RectChanged?.Invoke(this, this.Rect); //Delegate
        }

    }
}