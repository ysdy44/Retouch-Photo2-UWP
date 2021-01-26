using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Operates;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Operates"/>.
    /// </summary>
    public sealed partial class OperateMenu : Expander, IMenu 
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;


        //@Content     
        public override UIElement MainPage => this.OperateMainPage;
        OperateMainPage OperateMainPage = new OperateMainPage();


        //@Construct
        /// <summary>
        /// Initializes a OperateMenu. 
        /// </summary>
        public OperateMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Operates"/>.
    /// </summary>
    public sealed partial class OperateMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = resource.GetString("/Menus/Operate");

            this.Button.ToolTip.Closed += (s, e) => this.OperateMainPage.IsOpen = false;
            this.Button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSecondPage) return;
                if (this.State != ExpanderState.Overlay) return;

                this.OperateMainPage.IsOpen = true;
            };
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Operate;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Operates.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }
    

    /// <summary>
    /// MainPag of <see cref = "OperateMenu"/>.
    /// </summary>
    public sealed partial class OperateMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "OperateMenu" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "OperateMenu.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(OperateMenu), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a OperateMainPage. 
        /// </summary>
        public OperateMainPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructTransform();
            this.ConstructArrange();
            this.ConstructHorizontally();
            this.ConstructVertically();
        }
    }

    /// <summary>
    /// MainPag of <see cref = "OperateMenu"/>.
    /// </summary>
    public sealed partial class OperateMainPage : UserControl
    {

        //DataContext
        private void ConstructDataContext(object dataContext, string path, DependencyProperty dp)
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
            this.HorizontallySpaceToolTip.Content = resource.GetString("/Operates/Horizontally_Space");
            this.HorizontallySpaceButton.Content = new HorizontallySpaceIcon();

            this.VerticallyTextBlock.Text = resource.GetString("/Operates/Vertically");
            this.TopToolTip.Content = resource.GetString("/Operates/Vertically_Top");
            this.TopButton.Content = new TopIcon();
            this.MiddleToolTip.Content = resource.GetString("/Operates/Vertically_Middle");
            this.MiddleButton.Content = new MiddleIcon();
            this.BottomToolTip.Content = resource.GetString("/Operates/Vertically_Bottom");
            this.BottomButton.Content = new BottomIcon();
            this.VerticallySpaceToolTip.Content = resource.GetString("/Operates/Vertically_Space");
            this.VerticallySpaceButton.Content = new VerticallySpaceIcon();
        }

    }

    /// <summary>
    /// MainPag of <see cref = "OperateMenu"/>.
    /// </summary>
    public sealed partial class OperateMainPage : UserControl
    {

        //Transform
        private void ConstructTransform()
        {

            this.FlipHorizontalButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateScale(-1, 1, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.FlipVerticalButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateScale(1, -1, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.RotateLeftButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(-FanKit.Math.PiOver2, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.RotateRightButton.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(FanKit.Math.PiOver2, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };

        }

        //Arrange
        private void ConstructArrange()
        {

            this.MoveBackButton.Click += (s, e) =>
            {
                if (this.Mode != ListViewSelectionMode.Single) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory("Layers arrange", this.ViewModel.LayerageCollection);
                this.ViewModel.HistoryPush(history);

                Layerage destination = this.SelectionViewModel.SelectionLayerage;
                IList<Layerage> parentsChildren = this.ViewModel.LayerageCollection.GetParentsChildren(destination);
                if (parentsChildren.Count < 2) return;

                parentsChildren.Remove(destination);
                parentsChildren.Add(destination);

                LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.BackOneButton.Click += (s, e) =>
            {
                if (this.Mode != ListViewSelectionMode.Single) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory("Layers arrange", this.ViewModel.LayerageCollection);
                this.ViewModel.HistoryPush(history);

                Layerage destination = this.SelectionViewModel.SelectionLayerage;
                IList<Layerage> parentsChildren = this.ViewModel.LayerageCollection.GetParentsChildren(destination);
                if (parentsChildren.Count < 2) return;

                int index = parentsChildren.IndexOf(destination);
                index++;

                if (index < 0) index = 0;
                if (index > parentsChildren.Count - 1) index = parentsChildren.Count - 1;

                parentsChildren.Remove(destination);
                parentsChildren.Insert(index, destination);

                LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.ForwardOneButton.Click += (s, e) =>
            {
                if (this.Mode != ListViewSelectionMode.Single) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory("Layers arrange", this.ViewModel.LayerageCollection);
                this.ViewModel.HistoryPush(history);

                Layerage destination = this.SelectionViewModel.SelectionLayerage;
                IList<Layerage> parentsChildren = this.ViewModel.LayerageCollection.GetParentsChildren(destination);
                if (parentsChildren.Count < 2) return;

                int index = parentsChildren.IndexOf(destination);
                index--;

                if (index < 0) index = 0;
                if (index > parentsChildren.Count - 1) index = parentsChildren.Count - 1;

                parentsChildren.Remove(destination);
                parentsChildren.Insert(index, destination);

                LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.MoveFrontButton.Click += (s, e) =>
            {
                if (this.Mode != ListViewSelectionMode.Single) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory("Layers arrange", this.ViewModel.LayerageCollection);
                this.ViewModel.HistoryPush(history);

                Layerage destination = this.SelectionViewModel.SelectionLayerage;
                IList<Layerage> parentsChildren = this.ViewModel.LayerageCollection.GetParentsChildren(destination);
                if (parentsChildren.Count < 2) return;

                parentsChildren.Remove(destination);
                parentsChildren.Insert(0, destination);

                LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

        }

        //Horizontally
        private void ConstructHorizontally()
        {
            this.LeftButton.Click += (s, e) => this.TransformAlign(BorderMode.MinX, Orientation.Horizontal);
            this.CenterButton.Click += (s, e) => this.TransformAlign(BorderMode.CenterX, Orientation.Horizontal);
            this.RightButton.Click += (s, e) => this.TransformAlign(BorderMode.MaxX, Orientation.Horizontal);
            this.HorizontallySpaceButton.Click += (s, e) => this.TransformSapce(Orientation.Horizontal);
        }

        //Vertical
        private void ConstructVertically()
        {
            this.TopButton.Click += (s, e) => this.TransformAlign(BorderMode.MinY, Orientation.Vertical);
            this.MiddleButton.Click += (s, e) => this.TransformAlign(BorderMode.CenterY, Orientation.Vertical);
            this.BottomButton.Click += (s, e) => this.TransformAlign(BorderMode.MaxY, Orientation.Vertical);
            this.VerticallySpaceButton.Click += (s, e) => this.TransformSapce(Orientation.Vertical);
        }

    }

    /// <summary>
    /// MainPag of <see cref = "OperateMenu"/>.
    /// </summary>
    public sealed partial class OperateMainPage : UserControl
    {
        
        private void TransformAlign(BorderMode borderMode, Orientation orientation)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.Single:
                    {
                        float positionValue = this.ViewModel.CanvasTransformer.GetBorderValue(borderMode);
                        this.TransformAlign(positionValue, borderMode, orientation);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        Transformer transformer = this.Transformer;
                        float positionValue = transformer.GetBorderValue(borderMode);
                        this.TransformAlign(positionValue, borderMode, orientation);
                    }
                    break;
            }
        }
                
        private void TransformAlign(float positionValue, BorderMode borderMode, Orientation orientation)
        {
            //History
            LayersTransformAddHistory history = new LayersTransformAddHistory("Transform");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                Transformer transformer = layerage.GetActualTransformer();
                float value = transformer.GetBorderValue(borderMode);

                float distance = positionValue - value;
                if (distance == 0) return;
                Vector2 vector = orientation == Orientation.Horizontal ?
                    new Vector2(distance, 0) :
                    new Vector2(0, distance);
                
                this.SelectionViewModel.SetLayerageValueWithChildren(layerage, (layerage2) =>
                {
                    ILayer layer = layerage2.Self;

                    //History
                    history.PushTransform(layer, vector);

                    //Refactoring
                    layer.IsRefactoringTransformer = true;
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsTransformer();
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.CacheTransform();
                    layer.TransformAdd(vector);
                });

            });
            //Refactoring
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate}
        }


       ///////////////////////////////


        private void TransformSapce(Orientation orientation)
        {
            if (this.Mode != ListViewSelectionMode.Multiple) return;

            IEnumerable<Layerage> layerages = this.SelectionViewModel.SelectionLayerages;
            int count = layerages.Count();
            if (count < 3) return;

            this.TransformSapce(layerages, count, orientation);
        }

        /// <summary>
        /// Border: 
        ///  Between previous and current
        /// 
        ///  Min            Center           Max             Min            Center           Max             Min            Center           Max
        ///    |-------------o-------------|                  |-------------o-------------|                  |-------------o-------------|
        ///    |__________Length_________|     space    |__________Length_________|     space    |__________Length_________|
        /// 
        /// </summary>
        private void TransformSapce(IEnumerable<Layerage> layerages, int count, Orientation orientation)
        {
            //Layerage, Min, Center, Max, Length
            var borders = orientation == Orientation.Horizontal ?
                from layerage in layerages select this._getBorderX(layerage) :
                from layerage in layerages select this._getBorderY(layerage);

            float min = borders.Min(border => border.Min);//Min
            float max = borders.Max(border => border.Max);//Max

            float lengthSum = borders.Sum(border => border.Length);//Sum of Length
            float space = ((max - min) - lengthSum) / (count - 1);//Between [ previous.Max ] and [ current.Min ].


            //History
            LayersTransformAddHistory history = new LayersTransformAddHistory("Transform");
            

            float postionMin = min;//[ previous.Min ] + [ previous.Length ] + space.
            var orderedBorders = borders.OrderBy(border => border.Min);

            foreach (var border in orderedBorders)
            {
                Layerage layerage = border.Layerage;

                float distance = postionMin - border.Min;
                postionMin += border.Length + space;//Sum

                if (distance == 0) continue;
                Vector2 vector = orientation == Orientation.Horizontal ?
                    new Vector2(distance, 0) :
                    new Vector2(0, distance);

                //Selection
                this.SelectionViewModel.SetLayerageValueWithChildren(layerage, (layerage2) =>
                {
                    ILayer layer = layerage2.Self;

                    //History
                    history.PushTransform(layer, vector);

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.CacheTransform();
                    layer.TransformAdd(vector);
                });
            }

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }

        private (Layerage Layerage, float Min, float Center, float Max, float Length) _getBorderX(Layerage layerage)
        {
            Transformer transformer = layerage.GetActualTransformer();

            float min = transformer.MinX;
            float center = transformer.Center.X;
            float max = transformer.MaxX;

            return (layerage, min, center, max, max - min);
        }
        private (Layerage Layerage, float Min, float Center, float Max, float Length) _getBorderY(Layerage layerage)
        {
            Transformer transformer = layerage.GetActualTransformer();

            float min = transformer.MinY;
            float center = transformer.Center.Y;
            float max = transformer.MaxY;

            return (layerage, min, center, max, max - min);
        }

    }
}