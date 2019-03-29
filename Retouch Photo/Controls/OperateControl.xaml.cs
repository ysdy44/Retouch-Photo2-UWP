using Retouch_Photo.Library;
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
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Controls
{
    public sealed partial class OperateControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

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
            /*

                        //Transform
                        this.FlipHorizontalButton.Tapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.XScale = -layer.Transformer.XScale);
                        this.FlipVerticalButton.Tapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.YScale = -layer.Transformer.YScale);
                        this.RotateLeftButton.Tapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.Radian += Transformer.PiHalf);
                        this.RotateRightButton.Tapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.Radian -= Transformer.PiHalf);

                        //Arrange
                        this.ArrangeMoveBackButton.ButtonTapped += (sender, e) => this.OperateArrange((Layer layer) =>
                        {
                            this.ViewModel.RenderLayer.Layers.Remove(layer);
                            this.ViewModel.RenderLayer.Layers.Add(layer);
                        });
                        this.ArrangeBackOneButton.ButtonTapped += (sender, e) => this.OperateArrange((Layer layer) =>
                        {
                            int index = this.ViewModel.RenderLayer.Layers.IndexOf(layer);
                            this.ViewModel.RenderLayer.Layers.Remove(layer);
                            this.ViewModel.RenderLayer.Layers.Insert(index + 1, layer);
                        });
                        this.ArrangeForwardOneButton.ButtonTapped += (sender, e) => this.OperateArrange((Layer layer) =>
                        {
                            int index = this.ViewModel.RenderLayer.Layers.IndexOf(layer);
                            this.ViewModel.RenderLayer.Layers.Remove(layer);
                            this.ViewModel.RenderLayer.Layers.Insert(index - 1, layer);
                        });
                        this.ArrangeMoveFrontButton.ButtonTapped += (sender, e) => this.OperateArrange((Layer layer) =>
                        {
                            this.ViewModel.RenderLayer.Layers.Remove(layer);
                            this.ViewModel.RenderLayer.Layers.Insert(0, layer);
                        });

                        //Align Horizontal
                        this.AlignLeftButton.ButtonTapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.Position.X += -layer.Transformer.TransformMinX(layer.Transformer.Matrix));
                        this.AlignCenterButton.ButtonTapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.Position.X += this.ViewModel.MatrixTransformer.Width / 2 - layer.Transformer.TransformCenter(layer.Transformer.Matrix).X);
                        this.AlignRightButton.ButtonTapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.Position.X += this.ViewModel.MatrixTransformer.Width - layer.Transformer.TransformMaxX(layer.Transformer.Matrix));
                        this.AlignSymmetryHorizontallyButton.ButtonTapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.Position.X = -this.Layer.Transformer.Position.X);

                        //Align Vertical
                        this.AlignTopButton.ButtonTapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.Position.Y += -layer.Transformer.TransformMinY(layer.Transformer.Matrix));
                        this.AlignMiddleButton.ButtonTapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.Position.Y += this.ViewModel.MatrixTransformer.Height / 2 - layer.Transformer.TransformCenter(layer.Transformer.Matrix).Y);
                        this.AlignBottomButton.ButtonTapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.Position.Y += this.ViewModel.MatrixTransformer.Height - layer.Transformer.TransformMaxY(layer.Transformer.Matrix));
                        this.AlignSymmetryVerticallyButton.ButtonTapped += (sender, e) => this.Operate((Layer layer) => layer.Transformer.Position.Y = -this.Layer.Transformer.Position.Y);

                         */
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
        private void OperateArrange(Action<Layer> action) => this.Operate((layer)=>
        {
            action(this.Layer);

            this.ViewModel.CurrentLayer = this.Layer;
            this.Initialize(this.Layer);
        });



        //Transform
        private void InitializeTransform(bool isEnabled)
        {
            this.FlipHorizontalButton.ButtonIsEnabled = isEnabled;
            this.FlipVerticalButton.ButtonIsEnabled = isEnabled;
            this.RotateLeftButton.ButtonIsEnabled = isEnabled;
            this.RotateRightButton.ButtonIsEnabled = isEnabled;
        }

        //Arrange
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
         
       //Align Horizontal
       private void InitializeAlignHorizontal(bool isEnabled)
        {
            this.AlignLeftButton.ButtonIsEnabled = isEnabled;
            this.AlignCenterButton.ButtonIsEnabled = isEnabled;
            this.AlignRightButton.ButtonIsEnabled = isEnabled;
            this.AlignSymmetryHorizontallyButton.ButtonIsEnabled = isEnabled;
        }

        //Align Vertical
        private void InitializeAlignVertical(bool isEnabled)
        {
            this.AlignTopButton.ButtonIsEnabled = isEnabled;
            this.AlignMiddleButton.ButtonIsEnabled = isEnabled;
            this.AlignBottomButton.ButtonIsEnabled = isEnabled;
            this.AlignSymmetryVerticallyButton.ButtonIsEnabled = isEnabled;
        }
               
         
    }
}
