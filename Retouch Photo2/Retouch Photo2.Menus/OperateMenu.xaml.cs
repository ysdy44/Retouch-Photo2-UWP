using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Operates;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "OperateMenu" />. 
    /// </summary>
    public sealed partial class OperateMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        Transformer Transformer { get => this.ViewModel.Transformer; set => this.ViewModel.Transformer = value; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "OperateMenu" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OperateMenu.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(OperateMenu), new PropertyMetadata(false));


        #endregion
        

        //@Construct
        public OperateMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructToolTip();
            this.ConstructMenu();

            this.ConstructTransform();
            this.ConstructArrange();
            this.ConstructHorizontally();
            this.ConstructVertically();
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "OperateMenu" />. 
    /// </summary>
    public sealed partial class OperateMenu : UserControl, IMenu
    {
        //DataContext
        public void ConstructDataContext(object dataContext, string path, DependencyProperty dp)
        {
            this.DataContext = dataContext;

            // Create the binding description.
            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(path)
            };

            // Attach the binding to the target.
            this.SetBinding(dp, binding);
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();
            
            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/Operate");

            this.TransformTextBlock.Text = resource.GetString("/Operates/Transform");
            this.FlipHorizontalToolTip.Content = resource.GetString("/Operates/Transform_FlipHorizontal");
            this.FlipHorizontalButton.Content = new FlipHorizontalIcon();
            this.FlipVerticalToolTip.Content = resource.GetString("/Operates/Transform_FlipVertical");
            this.FlipVerticalButton.Content = new FlipVerticalIcon();
            this.RotateLeftToolTip.Content = resource.GetString("/Operates/Transform_RotateLeft");
            this.RotateLeftButton.Content = new RotateLeftIcon();
            this.RotateRightToolTip.Content = resource.GetString("/Operates/Transform_RotateRight");
            this.RotateRightButton.Content = new RotateRightIcon();

            this.ArrangeTextBlock.Text = resource.GetString("/Operates/Arrange");
            this.MoveBackToolTip.Content = resource.GetString("/Operates/Arrange_MoveBack");
            this.MoveBackButton.Content = new MoveBackIcon();
            this.BackOneToolTip.Content = resource.GetString("/Operates/Arrange_BackOne");
            this.BackOneButton.Content = new BackOneIcon();
            this.ForwardOneToolTip.Content = resource.GetString("/Operates/Arrange_ForwardOne");
            this.ForwardOneButton.Content = new ForwardOneIcon();
            this.MoveFrontToolTip.Content = resource.GetString("/Operates/Arrange_MoveFront");
            this.MoveFrontButton.Content = new MoveFrontIcon();

            this.HorizontallyTextBlock.Text = resource.GetString("/Operates/Horizontally");
            this.LeftToolTip.Content = resource.GetString("/Operates/Horizontally_Left");
            this.LeftButton.Content = new LeftIcon();
            this.CenterToolTip.Content = resource.GetString("/Operates/Horizontally_Center");
            this.CenterButton.Content = new CenterIcon();
            this.RightToolTip.Content = resource.GetString("/Operates/Horizontally_Right");
            this.RightButton.Content = new RightIcon();
            this.HorizontallySymmetryToolTip.Content = resource.GetString("/Operates/Horizontally_Symmetry");
            this.HorizontallySymmetryButton.Content = new HorizontallySymmetryIcon();

            this.VerticallyTextBlock.Text = resource.GetString("/Operates/Vertically");
            this.TopToolTip.Content = resource.GetString("/Operates/Vertically_Top");
            this.TopButton.Content = new TopIcon();
            this.MiddleToolTip.Content = resource.GetString("/Operates/Vertically_Middle");
            this.MiddleButton.Content = new MiddleIcon();
            this.BottomToolTip.Content = resource.GetString("/Operates/Vertically_Bottom");
            this.BottomButton.Content = new BottomIcon();
            this.VerticallySymmetryToolTip.Content = resource.GetString("/Operates/Vertically_Symmetry");
            this.VerticallySymmetryButton.Content = new VerticallySymmetryIcon();
        }

        //ToolTip
        private void ConstructToolTip()
        {
            this._button.ToolTip.Opened += (s, e) =>
            {
                if (this._Expander.IsSecondPage==false)
                {
                    if (this.Expander.State == ExpanderState.Overlay)
                    {
                        this.IsOpen = true;
                    }
                }
            };
            this._button.ToolTip.Closed += (s, e) =>
            {
                this.IsOpen = false;
            };
        }


        //Menu
        public MenuType Type => MenuType.Operate;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Operates.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "OperateMenu" />. 
    /// </summary>
    public sealed partial class OperateMenu : UserControl, IMenu
    {

        //Transform
        private void ConstructTransform()
        {

            this.FlipHorizontalButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateScale(-1, 1, transformer.Center);
                this.ViewModel.MethodTransformMultiplies(matrix);//Method

            };

            this.FlipVerticalButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateScale(1, -1, transformer.Center);
                this.ViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.RotateLeftButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(-FanKit.Math.PiOver2, transformer.Center);
                this.ViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.RotateRightButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(FanKit.Math.PiOver2, transformer.Center);
                this.ViewModel.MethodTransformMultiplies(matrix);//Method
            };

        }

        //Arrange
        private void ConstructArrange()
        {

            this.MoveBackButton.Click += (s, e) =>
            {
                if (this.ViewModel.SelectionMode == ListViewSelectionMode.Single)
                {
                    Layerage destination = this.ViewModel.Layerage;
                    IList<Layerage> parentsChildren = this.ViewModel.LayerageCollection.GetParentsChildren(destination);
                    if (parentsChildren.Count < 2) return;

                    parentsChildren.Remove(destination);
                    parentsChildren.Add(destination);

                    LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

            this.BackOneButton.Click += (s, e) =>
            {
                if (this.ViewModel.SelectionMode == ListViewSelectionMode.Single)
                {
                    Layerage destination = this.ViewModel.Layerage;
                    IList<Layerage> parentsChildren = this.ViewModel.LayerageCollection.GetParentsChildren(destination);
                    if (parentsChildren.Count < 2) return;

                    int index = parentsChildren.IndexOf(destination);
                    index++;

                    if (index < 0) index = 0;
                    if (index > parentsChildren.Count) index = parentsChildren.Count - 1;

                    parentsChildren.Remove(destination);
                    parentsChildren.Insert(index, destination);

                    LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

            this.ForwardOneButton.Click += (s, e) =>
            {
                if (this.ViewModel.SelectionMode == ListViewSelectionMode.Single)
                {
                    Layerage destination = this.ViewModel.Layerage;                    
                    IList<Layerage> parentsChildren = this.ViewModel.LayerageCollection.GetParentsChildren(destination);                    
                    if (parentsChildren.Count < 2) return;

                    int index = parentsChildren.IndexOf(destination);
                    index--;

                    if (index < 0) index = 0;
                    if (index > parentsChildren.Count) index = parentsChildren.Count - 1;

                    parentsChildren.Remove(destination);
                    parentsChildren.Insert(index, destination);

                    LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

            this.MoveFrontButton.Click += (s, e) =>
            {
                if (this.ViewModel.SelectionMode == ListViewSelectionMode.Single)
                {
                    Layerage destination = this.ViewModel.Layerage;
                    IList<Layerage> parentsChildren = this.ViewModel.LayerageCollection.GetParentsChildren(destination);                    
                    if (parentsChildren.Count < 2) return;

                    parentsChildren.Remove(destination);
                    parentsChildren.Insert(0, destination);

                    LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

        }

        //Horizontally
        private void ConstructHorizontally()
        {

            this.LeftButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0 - transformer.MinX, 0);
                this.ViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.CenterButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(this.ViewModel.CanvasTransformer.Width / 2 - transformer.Center.X, 0);
                this.ViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.RightButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(this.ViewModel.CanvasTransformer.Width - transformer.MaxX, 0);
                this.ViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.HorizontallySymmetryButton.Click += (s, e) =>
            {
                this._Expander.IsSecondPage = true;
            };

        }

        //Vertical
        private void ConstructVertically()
        {

            this.TopButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0, 0 - transformer.MinY);
                this.ViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.MiddleButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0, this.ViewModel.CanvasTransformer.Height / 2 - transformer.Center.Y);
                this.ViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.BottomButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(0, this.ViewModel.CanvasTransformer.Height - transformer.MaxY);
                this.ViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.VerticallySymmetryButton.Click += (s, e) =>
            {
                this._Expander.IsSecondPage = true;
            };

        }        
    }
}