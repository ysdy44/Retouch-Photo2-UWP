using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Controls
{
    public sealed partial class OperateControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        Transformer StartTransformer;

        #region IsEnabled


        //Transform
        bool TransformIsEnabled
        {
            set
            {
                this.FlipHorizontalButton.ButtonIsEnabled = value;
                this.FlipVerticalButton.ButtonIsEnabled = value;
                this.RotateLeftButton.ButtonIsEnabled = value;
                this.RotateRightButton.ButtonIsEnabled = value;
            }
        }

        //Arrange
        bool ArrangeIsEnabled
        {
            set
            {
                this.ArrangeMoveBackButton.ButtonIsEnabled = value;
                this.ArrangeBackOneButton.ButtonIsEnabled = value;
                this.ArrangeForwardOneButton.ButtonIsEnabled = value;
                this.ArrangeMoveFrontButton.ButtonIsEnabled = value;
            }
        }
        private void InitializeArrange(int min, int max, int index)
        {
            this.ArrangeMoveBackButton.ButtonIsEnabled = !(index == max);
            this.ArrangeBackOneButton.ButtonIsEnabled = (index < max);
            this.ArrangeForwardOneButton.ButtonIsEnabled = (index > min);
            this.ArrangeMoveFrontButton.ButtonIsEnabled = !(index == min);
        }

        //Align Horizontal
        bool AlignHorizontalIsEnabled
        {
            set
            {
                this.AlignLeftButton.ButtonIsEnabled = value;
                this.AlignCenterButton.ButtonIsEnabled = value;
                this.AlignRightButton.ButtonIsEnabled = value;
                this.AlignSymmetryHorizontallyButton.ButtonIsEnabled = value;
            }
        }

        //Align Vertical
        bool AlignVerticaIsEnabled
        {
            set
            {
                this.AlignTopButton.ButtonIsEnabled = value;
                this.AlignMiddleButton.ButtonIsEnabled = value;
                this.AlignBottomButton.ButtonIsEnabled = value;
                this.AlignSymmetryVerticallyButton.ButtonIsEnabled = value;
            }
        }


        #endregion

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

            //Transform
            this.FlipHorizontalButton.Tapped += (sender, e) => this.OperateTransform((Layer layer) => Matrix3x2.CreateScale(-1, 1, layer.Transformer.DstCenter));
            this.FlipVerticalButton.Tapped += (sender, e) => this.OperateTransform((Layer layer) => Matrix3x2.CreateScale(1, -1, layer.Transformer.DstCenter));
            this.RotateLeftButton.Tapped += (sender, e) => this.OperateTransform((Layer layer) => Matrix3x2.CreateRotation(-Transformer.PiHalf, layer.Transformer.DstCenter));
            this.RotateRightButton.Tapped += (sender, e) => this.OperateTransform((Layer layer) => Matrix3x2.CreateRotation(Transformer.PiHalf, layer.Transformer.DstCenter));
            
            //Align Horizontal
            this.AlignLeftButton.ButtonTapped += (sender, e) => this.OperateVector((Layer layer) => new Vector2(0 - layer.Transformer.DstMinX, 0));
            this.AlignCenterButton.ButtonTapped += (sender, e) => this.OperateVector((Layer layer) => new Vector2(this.ViewModel.MatrixTransformer.Width / 2 - layer.Transformer.DstCenter.X, 0));
            this.AlignRightButton.ButtonTapped += (sender, e) => this.OperateVector((Layer layer) => new Vector2(this.ViewModel.MatrixTransformer.Width - layer.Transformer.DstMaxX, 0));
            this.AlignSymmetryHorizontallyButton.ButtonTapped += (sender, e) => this.OperateVector((Layer layer) => new Vector2(0, 0));

            //Align Vertical
            this.AlignTopButton.ButtonTapped += (sender, e) => this.OperateVector((Layer layer) => new Vector2(0, 0 - layer.Transformer.DstMinY));
            this.AlignMiddleButton.ButtonTapped += (sender, e) => this.OperateVector((Layer layer) => new Vector2(0, this.ViewModel.MatrixTransformer.Height / 2 - layer.Transformer.DstCenter.Y));
            this.AlignBottomButton.ButtonTapped += (sender, e) => this.OperateVector((Layer layer) => new Vector2(0, this.ViewModel.MatrixTransformer.Height - layer.Transformer.DstMaxY));
            this.AlignSymmetryVerticallyButton.ButtonTapped += (sender, e) => this.OperateVector((Layer layer) => new Vector2(0, 0));

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
        }


        /// <summary> Initialize all button. </summary>
        public void Initialize(Layer layer)
        {
            bool isEnabled = !(layer == null);

            //Transform
            this.TransformIsEnabled = isEnabled;
            //Align Horizontal            
            this.AlignHorizontalIsEnabled = isEnabled;
            //Align Vertical            
            this.AlignVerticaIsEnabled = isEnabled;

            //Arrange
            if (this.ViewModel.RenderLayer.Layers.Count < 2 || !isEnabled || this.ViewModel.SelectedIndex == -1)
                this.ArrangeIsEnabled = false;
            else
                this.InitializeArrange(0, this.ViewModel.RenderLayer.Layers.Count - 1, this.ViewModel.SelectedIndex);
        }

        /// <summary> Operating on the layer. </summary>
        private void OperateArrange(Action<Layer> action) 
        {
            if (this.Layer == null) return;
            this.StartTransformer = this.Layer.Transformer;

            action(this.Layer);//Action

            this.ViewModel.CurrentLayer = this.Layer;
            this.Initialize(this.Layer);
            this.ViewModel.Invalidate();
        }
        /// <summary> Vector operating on the layer. </summary>
        private void OperateVector(Func<Layer, Vector2> action)
        {
            if (this.Layer == null) return;
            this.StartTransformer = this.Layer.Transformer;

            //Action
            Vector2 vector = action(this.Layer);
            this.Layer.Transformer.DstLeftTop = this.StartTransformer.DstLeftTop + vector;
            this.Layer.Transformer.DstRightTop = this.StartTransformer.DstRightTop + vector;
            this.Layer.Transformer.DstRightBottom = this.StartTransformer.DstRightBottom + vector;
            this.Layer.Transformer.DstLeftBottom = this.StartTransformer.DstLeftBottom + vector;

            this.ViewModel.Invalidate();
        }
        /// <summary> Transform operating on the layer. </summary>
        private void OperateTransform(Func<Layer, Matrix3x2> action)
        {
            if (this.Layer == null) return;
            this.StartTransformer = this.Layer.Transformer;

            //Action
            Matrix3x2 matrix = action(this.Layer);
            this.Layer.Transformer.DstLeftTop = Vector2.Transform(this.StartTransformer.DstLeftTop, matrix);
            this.Layer.Transformer.DstRightTop = Vector2.Transform(this.StartTransformer.DstRightTop, matrix);
            this.Layer.Transformer.DstRightBottom = Vector2.Transform(this.StartTransformer.DstRightBottom, matrix);
            this.Layer.Transformer.DstLeftBottom = Vector2.Transform(this.StartTransformer.DstLeftBottom, matrix);

            this.ViewModel.Invalidate();
        }
    }
}
