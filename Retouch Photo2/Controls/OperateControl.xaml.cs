using FanKit.Transformers;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "OperateControl" />. 
    /// </summary>
    public sealed partial class OperateControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => Retouch_Photo2.App.ViewModel;
        SelectionViewModel SelectionViewModel => Retouch_Photo2.App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => Retouch_Photo2.App.MezzanineViewModel;
        TipViewModel TipViewModel => Retouch_Photo2.App.TipViewModel;
                     

        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "OperateControl" />'s Mode. </summary>
        public ListViewSelectionMode Mode
        {
            get { return (ListViewSelectionMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OperateControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(ListViewSelectionMode), typeof(OperateControl), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            OperateControl con = (OperateControl)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
                switch (value)
                {
                    case ListViewSelectionMode.None:
                        {
                            //Transform
                            con.FlipHorizontalButton.ButtonIsEnabled = false;
                            con.FlipVerticalButton.ButtonIsEnabled = false;
                            con.RotateLeftButton.ButtonIsEnabled = false;
                            con.RotateRightButton.ButtonIsEnabled = false;

                            //Align Horizontal
                            con.AlignLeftButton.ButtonIsEnabled = false;
                            con.AlignCenterButton.ButtonIsEnabled = false;
                            con.AlignRightButton.ButtonIsEnabled = false;
                            con.AlignSymmetryHorizontallyButton.ButtonIsEnabled = false;

                            //Align Vertical
                            con.AlignTopButton.ButtonIsEnabled = false;
                            con.AlignMiddleButton.ButtonIsEnabled = false;
                            con.AlignBottomButton.ButtonIsEnabled = false;
                            con.AlignSymmetryVerticallyButton.ButtonIsEnabled = false;

                            //Arrange
                            con.ArrangeMoveBackButton.ButtonIsEnabled = false;
                            con.ArrangeBackOneButton.ButtonIsEnabled = false;
                            con.ArrangeForwardOneButton.ButtonIsEnabled = false;
                            con.ArrangeMoveFrontButton.ButtonIsEnabled = false;
                        }
                        break;
                    case ListViewSelectionMode.Single:
                    case ListViewSelectionMode.Multiple:
                        {
                            //Transform
                            con.FlipHorizontalButton.ButtonIsEnabled = true;
                            con.FlipVerticalButton.ButtonIsEnabled = true;
                            con.RotateLeftButton.ButtonIsEnabled = true;
                            con.RotateRightButton.ButtonIsEnabled = true;

                            //Align Horizontal
                            con.AlignLeftButton.ButtonIsEnabled = true;
                            con.AlignCenterButton.ButtonIsEnabled = true;
                            con.AlignRightButton.ButtonIsEnabled = true;
                            con.AlignSymmetryHorizontallyButton.ButtonIsEnabled = true;

                            //Align Vertical
                            con.AlignTopButton.ButtonIsEnabled = true;
                            con.AlignMiddleButton.ButtonIsEnabled = true;
                            con.AlignBottomButton.ButtonIsEnabled = true;
                            con.AlignSymmetryVerticallyButton.ButtonIsEnabled = true;

                            //Arrange
                            con.ArrangeMoveBackButton.ButtonIsEnabled = true;
                            con.ArrangeBackOneButton.ButtonIsEnabled = true;
                            con.ArrangeForwardOneButton.ButtonIsEnabled = true;
                            con.ArrangeMoveFrontButton.ButtonIsEnabled = true;
                        }
                        break;
                }
            }
        }));

        #endregion
        

        //@Construct
        public OperateControl()
        {
            this.InitializeComponent();


            #region Transform


            this.FlipHorizontalButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Matrix3x2 matrix = Matrix3x2.CreateScale(-1, 1, transformer.Center);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, matrix);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.FlipVerticalButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Matrix3x2 matrix = Matrix3x2.CreateScale(1, -1, transformer.Center);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, matrix);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.RotateLeftButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Matrix3x2 matrix = Matrix3x2.CreateRotation(Transformer.PiHalf, transformer.Center);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, matrix);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.RotateRightButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Matrix3x2 matrix = Matrix3x2.CreateRotation(-Transformer.PiHalf, transformer.Center);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, matrix);

                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion


            #region Align Horizontal


            this.AlignLeftButton.ButtonTapped += (s, e) =>
            {
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0 - transformer.MinX, 0);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, matrix);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignCenterButton.ButtonTapped += (s, e) =>
            {
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(this.ViewModel.CanvasTransformer.Width / 2 - transformer.Center.X, 0);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, matrix);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignRightButton.ButtonTapped += (s, e) =>
            {
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(this.ViewModel.CanvasTransformer.Width - transformer.MaxX, 0);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, matrix);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignSymmetryHorizontallyButton.ButtonTapped += (s, e) => { };


            #endregion


            #region Align Vertical


            this.AlignTopButton.ButtonTapped += (s, e) =>
            {
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0 , 0- transformer.MinY);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, matrix);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignMiddleButton.ButtonTapped += (s, e) =>
            {
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0,this.ViewModel.CanvasTransformer.Height / 2 - transformer.Center.Y);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, matrix);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignBottomButton.ButtonTapped += (s, e) =>
            {
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0, this.ViewModel.CanvasTransformer.Height - transformer.MaxY);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, matrix);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignSymmetryVerticallyButton.ButtonTapped += (s, e) => { };


            #endregion


            #region Arrange


            this.ArrangeMoveBackButton.ButtonTapped += (s, e) =>
            {

            };

            this.ArrangeBackOneButton.ButtonTapped += (s, e) =>
            {

            };

            this.ArrangeForwardOneButton.ButtonTapped += (s, e) => 
            {

            };

            this.ArrangeMoveFrontButton.ButtonTapped += (s, e) =>
            {

            };


            #endregion

        }
    }
}