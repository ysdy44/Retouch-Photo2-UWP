using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
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
        TipViewModel TipViewModel => App.TipViewModel;

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
                

        //@Content
        public MenuTitle MenuTitle => this._MenuTitle;
        

        //@VisualState
        bool _vsIsHorizontal;
        bool _vsIsVertically;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsHorizontal) return this.Horizontal;
                if (this._vsIsVertically) return this.Vertically;

                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        

        //@Converter
        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsOverlayExpanded;
        public bool IsOverlayExpanded { private get; set; }
        

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
                            con.FlipHorizontalButton.IsEnabled = con.FlipVerticalButton.IsEnabled = con.RotateLeftButton.IsEnabled = con.RotateRightButton.IsEnabled = false;
                            //Arrange
                            con.ArrangeMoveBackButton.IsEnabled = con.ArrangeBackOneButton.IsEnabled = con.ArrangeForwardOneButton.IsEnabled = con.ArrangeMoveFrontButton.IsEnabled = false;
                            //Align Horizontal
                            con.AlignLeftButton.IsEnabled = con.AlignCenterButton.IsEnabled = con.AlignRightButton.IsEnabled = con.AlignSymmetryHorizontallyButton.IsEnabled = false;
                            //Align Vertical
                            con.AlignTopButton.IsEnabled = con.AlignMiddleButton.IsEnabled = con.AlignBottomButton.IsEnabled = con.AlignSymmetryVerticallyButton.IsEnabled = false;
                        }
                        break;
                    case ListViewSelectionMode.Single:
                        {
                            //Transform
                            con.FlipHorizontalButton.IsEnabled = con.FlipVerticalButton.IsEnabled = con.RotateLeftButton.IsEnabled = con.RotateRightButton.IsEnabled = true;
                            //Arrange
                            con.ArrangeMoveBackButton.IsEnabled = con.ArrangeBackOneButton.IsEnabled = con.ArrangeForwardOneButton.IsEnabled = con.ArrangeMoveFrontButton.IsEnabled = true;
                            //Align Horizontal
                            con.AlignLeftButton.IsEnabled = con.AlignCenterButton.IsEnabled = con.AlignRightButton.IsEnabled = con.AlignSymmetryHorizontallyButton.IsEnabled = true;
                            //Align Vertical
                            con.AlignTopButton.IsEnabled = con.AlignMiddleButton.IsEnabled = con.AlignBottomButton.IsEnabled = con.AlignSymmetryVerticallyButton.IsEnabled = true;
                        }
                        break;
                    case ListViewSelectionMode.Multiple:
                        {
                            //Transform
                            con.FlipHorizontalButton.IsEnabled = con.FlipVerticalButton.IsEnabled = con.RotateLeftButton.IsEnabled = con.RotateRightButton.IsEnabled = true;
                            //Arrange
                            con.ArrangeMoveBackButton.IsEnabled = con.ArrangeBackOneButton.IsEnabled = con.ArrangeForwardOneButton.IsEnabled = con.ArrangeMoveFrontButton.IsEnabled = false;
                            //Align Horizontal
                            con.AlignLeftButton.IsEnabled = con.AlignCenterButton.IsEnabled = con.AlignRightButton.IsEnabled = con.AlignSymmetryHorizontallyButton.IsEnabled = true;
                            //Align Vertical
                            con.AlignTopButton.IsEnabled = con.AlignMiddleButton.IsEnabled = con.AlignBottomButton.IsEnabled = con.AlignSymmetryVerticallyButton.IsEnabled = true;
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

            //Menu
            this._MenuTitle.BackButton.Tapped += (s, e) =>
            {
                this._vsIsHorizontal = false;
                this._vsIsVertically = false;
                this.VisualState = this.VisualState;//State
            };

            #region Transform


            this.FlipHorizontalButton.Tapped += (s, e) =>
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

            this.FlipVerticalButton.Tapped += (s, e) =>
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

            this.RotateLeftButton.Tapped += (s, e) =>
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

            this.RotateRightButton.Tapped += (s, e) =>
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


            #region Arrange


            this.ArrangeMoveBackButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode== ListViewSelectionMode.Single)
                {
                    ILayer destination = this.SelectionViewModel.Layer;

                    IList<ILayer> parentsChildren = (destination.Parents == null) ?
                        this.ViewModel.Layers.RootLayers :
                        destination.Parents.Children;

                    parentsChildren.Remove(destination);
                    parentsChildren.Add(destination);

                    this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

            this.ArrangeBackOneButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.Single)
                {
                    ILayer destination = this.SelectionViewModel.Layer;

                    IList<ILayer> parentsChildren = (destination.Parents == null) ?
                        this.ViewModel.Layers.RootLayers :
                        destination.Parents.Children;

                    int index = parentsChildren.IndexOf(destination);
                    index++;

                    parentsChildren.Remove(destination);
                    parentsChildren.Insert(index, destination);

                    this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

            this.ArrangeForwardOneButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.Single)
                {
                    ILayer destination = this.SelectionViewModel.Layer;

                    IList<ILayer> parentsChildren = (destination.Parents == null) ?
                        this.ViewModel.Layers.RootLayers :
                        destination.Parents.Children;

                    int index = parentsChildren.IndexOf(destination);
                    index--;

                    parentsChildren.Remove(destination);
                    parentsChildren.Insert(index, destination);

                    this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

            this.ArrangeMoveFrontButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.Single)
                {
                    ILayer destination = this.SelectionViewModel.Layer;

                    IList<ILayer> parentsChildren = (destination.Parents == null) ?
                        this.ViewModel.Layers.RootLayers :
                        destination.Parents.Children;

                    parentsChildren.Remove(destination);
                    parentsChildren.Insert(0, destination);

                    this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();
                    this.ViewModel.Invalidate();//Invalidate
                }
            };


            #endregion


            #region Align Horizontal


            this.AlignLeftButton.Tapped += (s, e) =>
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

            this.AlignCenterButton.Tapped += (s, e) =>
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

            this.AlignRightButton.Tapped += (s, e) =>
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

            this.AlignSymmetryHorizontallyButton.Tapped += (s, e) =>
            {
                this._vsIsHorizontal = true;
                this.VisualState = this.VisualState;//State
            };
            

            #endregion


            #region Align Vertical


            this.AlignTopButton.Tapped += (s, e) =>
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

            this.AlignMiddleButton.Tapped += (s, e) =>
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

            this.AlignBottomButton.Tapped += (s, e) =>
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

            this.AlignSymmetryVerticallyButton.Tapped += (s, e) =>
            {
                this._vsIsVertically = true;
                this.VisualState = this.VisualState;//State
            };

            #endregion

        }
    }
}