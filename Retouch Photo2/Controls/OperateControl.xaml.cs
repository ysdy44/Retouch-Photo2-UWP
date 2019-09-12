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
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }

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
                            con.FlipHorizontalButton.IsEnabled = false;
                            con.FlipVerticalButton.IsEnabled = false;
                            con.RotateLeftButton.IsEnabled = false;
                            con.RotateRightButton.IsEnabled = false;

                            //Align Horizontal
                            con.AlignLeftButton.IsEnabled = false;
                            con.AlignCenterButton.IsEnabled = false;
                            con.AlignRightButton.IsEnabled = false;
                            con.AlignSymmetryHorizontallyButton.IsEnabled = false;

                            //Align Vertical
                            con.AlignTopButton.IsEnabled = false;
                            con.AlignMiddleButton.IsEnabled = false;
                            con.AlignBottomButton.IsEnabled = false;
                            con.AlignSymmetryVerticallyButton.IsEnabled = false;

                            //Arrange
                            con.ArrangeMoveBackButton.IsEnabled = false;
                            con.ArrangeBackOneButton.IsEnabled = false;
                            con.ArrangeForwardOneButton.IsEnabled = false;
                            con.ArrangeMoveFrontButton.IsEnabled = false;
                        }
                        break;
                    case ListViewSelectionMode.Single:
                    case ListViewSelectionMode.Multiple:
                        {
                            //Transform
                            con.FlipHorizontalButton.IsEnabled = true;
                            con.FlipVerticalButton.IsEnabled = true;
                            con.RotateLeftButton.IsEnabled = true;
                            con.RotateRightButton.IsEnabled = true;

                            //Align Horizontal
                            con.AlignLeftButton.IsEnabled = true;
                            con.AlignCenterButton.IsEnabled = true;
                            con.AlignRightButton.IsEnabled = true;
                            con.AlignSymmetryHorizontallyButton.IsEnabled = true;

                            //Align Vertical
                            con.AlignTopButton.IsEnabled = true;
                            con.AlignMiddleButton.IsEnabled = true;
                            con.AlignBottomButton.IsEnabled = true;
                            con.AlignSymmetryVerticallyButton.IsEnabled = true;

                            //Arrange
                            con.ArrangeMoveBackButton.IsEnabled = true;
                            con.ArrangeBackOneButton.IsEnabled = true;
                            con.ArrangeForwardOneButton.IsEnabled = true;
                            con.ArrangeMoveFrontButton.IsEnabled = true;
                        }
                        break;
                }
            }
        }));


        /// <summary> Gets or sets <see cref = "OperateControl" />'s ToolTip IsOpen. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OperateControl.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(OperateControl), new PropertyMetadata(false));


        #endregion


        //@Construct
        public OperateControl()
        {
            this.InitializeComponent();


            #region Transform


            this.FlipHorizontalButton.RootButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateScale(-1, 1, transformer.Center);

                //Selection
                this.Transformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.FlipVerticalButton.RootButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateScale(1, -1, transformer.Center);

                //Selection
                this.Transformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.RotateLeftButton.RootButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(FanKit.Math.PiOver2, transformer.Center);

                //Selection
                this.Transformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.RotateRightButton.RootButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(-FanKit.Math.PiOver2, transformer.Center);

                //Selection
                this.Transformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion


            #region Align Horizontal


            this.AlignLeftButton.RootButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0 - transformer.MinX, 0);

                //Selection
                this.Transformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignCenterButton.RootButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(this.ViewModel.CanvasTransformer.Width / 2 - transformer.Center.X, 0);

                //Selection
                this.Transformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignRightButton.RootButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(this.ViewModel.CanvasTransformer.Width - transformer.MaxX, 0);

                //Selection
                this.Transformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignSymmetryHorizontallyButton.RootButton.Tapped += (s, e) => { };


            #endregion


            #region Align Vertical


            this.AlignTopButton.RootButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0, 0 - transformer.MinY);

                //Selection
                this.Transformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignMiddleButton.RootButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0, this.ViewModel.CanvasTransformer.Height / 2 - transformer.Center.Y);

                //Selection
                this.Transformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignBottomButton.RootButton.Tapped += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0, this.ViewModel.CanvasTransformer.Height - transformer.MaxY);

                //Selection
                this.Transformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.AlignSymmetryVerticallyButton.RootButton.Tapped += (s, e) => { };


            #endregion


            #region Arrange


            this.ArrangeMoveBackButton.RootButton.Tapped += (s, e) =>
            {

            };

            this.ArrangeBackOneButton.RootButton.Tapped += (s, e) =>
            {

            };

            this.ArrangeForwardOneButton.RootButton.Tapped += (s, e) =>
            {

            };

            this.ArrangeMoveFrontButton.RootButton.Tapped += (s, e) =>
            {

            };


            #endregion

        }
    }
}