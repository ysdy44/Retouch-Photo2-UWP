using Retouch_Photo.Models;
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
using static Retouch_Photo.Library.TransformController;

namespace Retouch_Photo.Controls
{
    public sealed partial class TransformerControl : UserControl
    {


        #region DependencyProperty


        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(TransformerControl), new PropertyMetadata(null, (sender, e) =>
        {
            TransformerControl con = (TransformerControl)sender;

            if (e.NewValue is Layer layer)
            {
                con.NavigatorEnable(true);
                con.NavigatorSet(layer.Transformer);
            }
            else
            {
                con.NavigatorEnable(false);
                con.NavigatorClear();
            }
        }));

        public Transformer Transformer
        {
            get { return (Transformer)GetValue(TransformerProperty); }
            set { SetValue(TransformerProperty, value); }
        }
        public static readonly DependencyProperty TransformerProperty = DependencyProperty.Register(nameof(Transformer), typeof(Transformer), typeof(TransformerControl), new PropertyMetadata(null, (sender, e) =>
        {
            TransformerControl con = (TransformerControl)sender;

            if (e.NewValue is Transformer value)
            {
                if (e.OldValue is Transformer oldValue)
                    con.NavigatorSet(value, oldValue);
                else
                    con.NavigatorSet(value);
            }
            else
            {
                con.NavigatorSet(new Transformer());
            }
        }));

        #endregion


        public TransformerControl()
        {
            this.InitializeComponent();

            this.WPicker.Minimum = int.MinValue;
            this.WPicker.Maximum = int.MaxValue;
            this.WPicker.ValueChange += (sender, value) => this.Navigator((m) => m.Transformer.XScale = value / 100);

            this.HPicker.Minimum = int.MinValue;
            this.HPicker.Maximum = int.MaxValue;
            this.HPicker.ValueChange += (sender, value) => this.Navigator((m) => m.Transformer.YScale = value / 100);

            this.XPicker.Minimum = int.MinValue;
            this.XPicker.Maximum = int.MaxValue;
            this.XPicker.ValueChange += (sender, value) => this.Navigator((m) => m.Transformer.Postion.X = value);

            this.YPicker.Minimum = int.MinValue;
            this.YPicker.Maximum = int.MaxValue;
            this.YPicker.ValueChange += (sender, value) => this.Navigator((m) => m.Transformer.Postion.Y = value);

            this.RPicker.Minimum = -(int)(Transformer.PI * 180);
            this.RPicker.Maximum = (int)(Transformer.PI * 180);
            this.RPicker.ValueChange += (sender, value) => this.Navigator((m) => m.Transformer.Radian = value * Transformer.PI/ 180f );

            this.SPicker.Minimum = -(int)(Transformer.PI * 180);
            this.SPicker.Maximum = (int)(Transformer.PI * 180);
            this.SPicker.ValueChange += (sender, value) => this.Navigator((m) => m.Transformer.Skew = value * Transformer.PI/ 180f );
        }

        private void Navigator(Action<Layer> action)
        {
            if (this.Layer == null) return;

            action(this.Layer);

            App.ViewModel.Invalidate();
        }


        private void NavigatorSet(Transformer value)
        {
            this.WPicker.Value = (int)(value.XScale * 100f);
            this.HPicker.Value = (int)(value.YScale * 100f);

            this.XPicker.Value = (int)(value.Postion.X);
            this.YPicker.Value = (int)(value.Postion.Y);

            this.RPicker.Value = (int)(value.Radian / Transformer.PI * 180f);
            this.SPicker.Value = (int)(value.Skew / Transformer.PI * 180f);
        }
        private void NavigatorSet(Transformer newValue, Transformer oldValue)
        {
            if (oldValue.XScale != newValue.XScale) this.WPicker.Value = (int)(newValue.XScale * 100f);
            if (oldValue.YScale != newValue.YScale) this.HPicker.Value = (int)(newValue.YScale * 100f);

            if (oldValue.Postion.X != newValue.Postion.X) this.XPicker.Value = (int)(newValue.Postion.X);
            if (oldValue.Postion.Y != newValue.Postion.Y) this.YPicker.Value = (int)(newValue.Postion.Y);

            if (oldValue.Radian != newValue.Radian) this.RPicker.Value = (int)(newValue.Radian / Transformer.PI * 180f);
            if (oldValue.Skew != newValue.Skew) this.SPicker.Value = (int)(newValue.Skew / Transformer.PI * 180f);
        }
        private void NavigatorClear()
        {
            this.WPicker.Value = 0;
            this.HPicker.Value = 0;

            this.XPicker.Value = 0;
            this.YPicker.Value = 0;

            this.RPicker.Value = 0;
            this.SPicker.Value = 0;
        }
        private void NavigatorEnable(bool enable)
        {
            this.WPicker.IsEnabled = enable;
            this.HPicker.IsEnabled = enable;

            this.XPicker.IsEnabled = enable;
            this.YPicker.IsEnabled = enable;

            this.RPicker.IsEnabled = enable;
            this.SPicker.IsEnabled = enable;
        }

    }
}
