using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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

namespace Retouch_Photo.Controls
{
    public sealed partial class OperateControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        #region DependencyProperty

        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(OperateControl), new PropertyMetadata(null, (sender, e) =>
        {
            OperateControl con = (OperateControl)sender;

            if (e.NewValue is Layer value)
            {
                con.Initialize(value);
            }
            else
            {
                con.Initialize(null);
           }
        }));

        #endregion


        public OperateControl()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Initialize all button
        /// </summary>
        public void Initialize(Layer layer)
        {
            bool isEnabled = !(layer == null);

            //Transform
            this.InitializeTransform(isEnabled);

            //Arrange
            if (this.ViewModel.RenderLayer.Layers.Count < 2 || !isEnabled || this.ViewModel.SelectedIndex == -1)
                this.InitializeArrange(false);
            else
                this.InitializeArrange(0, this.ViewModel.RenderLayer.Layers.Count - 1, this.ViewModel.SelectedIndex);

            //Align Horizontal            
            this.InitializeAlignHorizontal(isEnabled);

            //Align Vertical            
            this.InitializeAlignVertical(isEnabled);
        }

        /// <summary>
        /// Operating on the layer
        /// </summary>
        /// <param name="action"> Operating </param>
        private void Operate(Action<Layer> action)
        {
            if (this.Layer == null) return;

            action(this.Layer);

            App.ViewModel.Invalidate();
        }


        #region Transform


        private void InitializeTransform(bool isEnabled)
        {
            this.FlipHorizontalButton.ButtonIsEnabled = isEnabled;
            this.FlipVerticalButton.ButtonIsEnabled = isEnabled;
            this.RotateLeftButton.ButtonIsEnabled = isEnabled;
            this.RotateRightButton.ButtonIsEnabled = isEnabled;
        }


        private void FlipHorizontalButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.XScale = -layer.Transformer.XScale
        );
        private void FlipVerticalButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.YScale =-layer.Transformer.YScale
        );
        private void RotateLeftButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.Radian += Transformer.PiHalf
        );
        private void RotateRightButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.Radian -= Transformer.PiHalf
        );


        #endregion


        #region Arrange


        private void InitializeArrange(bool isEnabled)
        {
            this.ArrangeMoveBackButton.ButtonIsEnabled = isEnabled;
            this.ArrangeBackOneButton.ButtonIsEnabled = isEnabled;
            this.ArrangeForwardOneButton.ButtonIsEnabled = isEnabled;
            this.ArrangeMoveFrontButton.ButtonIsEnabled = isEnabled;
        }
        private void InitializeArrange(int min, int max, int index)
        {
            this.ArrangeMoveBackButton.ButtonIsEnabled = !(index == max);
            this.ArrangeBackOneButton.ButtonIsEnabled = (index < max);
            this.ArrangeForwardOneButton.ButtonIsEnabled = (index > min);
            this.ArrangeMoveFrontButton.ButtonIsEnabled = !(index == min);
        }


        private void ArrangeMoveBackButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) =>
            {            
                this.ViewModel.RenderLayer.Layers.Remove(layer);
                this.ViewModel.RenderLayer.Layers.Add(layer);

                this.ViewModel.CurrentLayer = layer;
                this.Initialize(layer);
            }
        );
        private void ArrangeBackOneButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) =>
            {
                int index = this.ViewModel.RenderLayer.Layers.IndexOf(layer);
                this.ViewModel.RenderLayer.Layers.Remove(layer);
                this.ViewModel.RenderLayer.Layers.Insert(index+1, layer);

                this.ViewModel.CurrentLayer = layer;
                this.Initialize(layer);
            }
        );
        private void ArrangeForwardOneButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) =>
            {
                int index = this.ViewModel.RenderLayer.Layers.IndexOf(layer);
                this.ViewModel.RenderLayer.Layers.Remove(layer);
                this.ViewModel.RenderLayer.Layers.Insert(index - 1, layer);

                this.ViewModel.CurrentLayer = layer;
                this.Initialize(layer);
            }
        );
        private void ArrangeMoveFrontButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) =>
            {
                this.ViewModel.RenderLayer.Layers.Remove(layer);
                this.ViewModel.RenderLayer.Layers.Insert(0, layer);

                this.ViewModel.CurrentLayer = layer;
                this.Initialize(layer);
            }
        );


        #endregion


        #region Align Horizontal

        //Initialize
        private void InitializeAlignHorizontal(bool isEnabled)
        {
            this.AlignLeftButton.ButtonIsEnabled = isEnabled;
            this.AlignCenterButton.ButtonIsEnabled = isEnabled;
            this.AlignRightButton.ButtonIsEnabled = isEnabled;
            this.AlignSymmetryHorizontallyButton.ButtonIsEnabled = isEnabled;
        }


        private void AlignLeftButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.Postion.X += -layer.Transformer.TransformMinX(layer.Transformer.Matrix)
        );
        private void AlignCenterButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.Postion.X += this.ViewModel.MatrixTransformer.Width / 2 - layer.Transformer.TransformCenter(layer.Transformer.Matrix).X
        );
        private void AlignRightButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.Postion.X += this.ViewModel.MatrixTransformer.Width - layer.Transformer.TransformMaxX(layer.Transformer.Matrix)
        );
        private void AlignSymmetryHorizontallyButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.Postion.X = -this.Layer.Transformer.Postion.X
        );


        #endregion


        #region Align Vertical


        private void InitializeAlignVertical(bool isEnabled)
        {
            this.AlignTopButton.ButtonIsEnabled = isEnabled;
            this.AlignMiddleButton.ButtonIsEnabled = isEnabled;
            this.AlignBottomButton.ButtonIsEnabled = isEnabled;
            this.AlignSymmetryVerticallyButton.ButtonIsEnabled = isEnabled;
        }


        private void AlignTopButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.Postion.Y += -layer.Transformer.TransformMinY(layer.Transformer.Matrix)
        );
        private void AlignMiddleButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.Postion.Y += this.ViewModel.MatrixTransformer.Height / 2 - layer.Transformer.TransformCenter(layer.Transformer.Matrix).Y
        );
        private void AlignBottomButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.Postion.Y += this.ViewModel.MatrixTransformer.Height - layer.Transformer.TransformMaxY(layer.Transformer.Matrix)
        );
        private void AlignSymmetryVerticallyButton_ButtonTapped(object sender, TappedRoutedEventArgs e) => this.Operate
        (
            (Layer layer) => layer.Transformer.Postion.Y = -this.Layer.Transformer.Postion.Y
        );
        

        #endregion
         
    }
}
