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
using Retouch_Photo2.Layers;
using Retouch_Photo2.TestApp.ViewModels;


namespace Retouch_Photo2.TestApp.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public sealed partial class LayerControl : UserControl
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;
 
        //@Converter
        private double OpacityToValueConverter(float opacity) => opacity * 100.0d;
        private float ValueToOpacityConverter(double value) => (float)value / 100.0f;

        private double BoolToOpacityConverter(bool isChecked) => isChecked ? 1.0 : 0.4;


        //@Construct
        public LayerControl()
        {
            this.InitializeComponent();


            this.OpacitySlider.ValueChanged += (s, e) =>
            {
                float opacity = this.ValueToOpacityConverter(e.NewValue);

                this.ViewModel.SelectionOpacity = opacity;
                this.ViewModel.Selection.SetLayer((layer)=>layer.Opacity= opacity);//Selection

                this.ViewModel.Invalidate();//Invalidate
            };

            this.VisualButton.Tapped += (s, e) =>
            {
                bool value = !this.ViewModel.SelectionIsVisual;

                this.ViewModel.SelectionIsVisual = value;
                this.ViewModel.Selection.SetLayer((layer) => layer.IsVisual = value);//Selection

                this.ViewModel.Invalidate();//Invalidate
            };


        }


    }
}
