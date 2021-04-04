// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Operates"/>.
    /// </summary>
    public sealed partial class OperateMenu : UserControl
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
        /// Initializes a OperateMenu. 
        /// </summary>
        public OperateMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.Transform_FlipHorizontal.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateScale(-1, 1, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };
            this.Transform_FlipVertical.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateScale(1, -1, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };
            this.Transform_RotateLeft.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(-FanKit.Math.PiOver2, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };
            this.Transform_RotateRight.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(FanKit.Math.PiOver2, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.Arrange_MoveBack.Click += (s, e) => this.MoveBack();
            this.Arrange_BackOne.Click += (s, e) => this.BackOne();
            this.Arrange_ForwardOne.Click += (s, e) => this.ForwardOne();
            this.Arrange_MoveFront.Click += (s, e) => this.MoveFront();

            this.Horizontally_Left.Click += (s, e) => this.TransformAlign(BorderMode.MinX, Orientation.Horizontal);
            this.Horizontally_Center.Click += (s, e) => this.TransformAlign(BorderMode.CenterX, Orientation.Horizontal);
            this.Horizontally_Right.Click += (s, e) => this.TransformAlign(BorderMode.MaxX, Orientation.Horizontal);
            this.Horizontally_HorizontallySpace.Click += (s, e) => this.TransformSapce(Orientation.Horizontal);

            this.Vertically_Top.Click += (s, e) => this.TransformAlign(BorderMode.MinY, Orientation.Vertical);
            this.Vertically_Middle.Click += (s, e) => this.TransformAlign(BorderMode.CenterY, Orientation.Vertical);
            this.Vertically_Bottom.Click += (s, e) => this.TransformAlign(BorderMode.MaxY, Orientation.Vertical);
            this.Vertically_VerticallySpace.Click += (s, e) => this.TransformSapce(Orientation.Vertical);
        }
    }

    public sealed partial class OperateMenu : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            foreach (UIElement child in this.LayoutRoot.Children)
            {
                if (child is Button button)
                {
                    if (ToolTipService.GetToolTip(button) is ToolTip toolTip)
                    {
                        toolTip.Content = resource.GetString($"Operates_{button.Name}");
                    }
                }
                if (child is TextBlock textBlock)
                {
                    textBlock.Text = resource.GetString($"Operates_{textBlock.Name}");
                }
            }
        }


        private void MoveBack()
        {
            if (this.Mode != ListViewSelectionMode.Single) return;

            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_LayersArrange);
            this.ViewModel.HistoryPush(history);

            Layerage destination = this.SelectionViewModel.SelectionLayerage;
            Layerage parents = LayerManager.GetParentsChildren(destination);
            if (parents.Children.Count < 2) return;

            parents.Children.Remove(destination);
            parents.Children.Add(destination);

            LayerManager.ArrangeLayers();
            this.ViewModel.Invalidate();//Invalidate
        }
        private void BackOne()
        {
            if (this.Mode != ListViewSelectionMode.Single) return;

            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_LayersArrange);
            this.ViewModel.HistoryPush(history);

            Layerage destination = this.SelectionViewModel.SelectionLayerage;
            Layerage parents = LayerManager.GetParentsChildren(destination);
            if (parents.Children.Count < 2) return;

            int index = parents.Children.IndexOf(destination);
            index++;

            if (index < 0) index = 0;
            if (index > parents.Children.Count - 1) index = parents.Children.Count - 1;

            parents.Children.Remove(destination);
            parents.Children.Insert(index, destination);

            LayerManager.ArrangeLayers();
            this.ViewModel.Invalidate();//Invalidate
        }
        private void ForwardOne()
        {
            if (this.Mode != ListViewSelectionMode.Single) return;

            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_LayersArrange);
            this.ViewModel.HistoryPush(history);

            Layerage destination = this.SelectionViewModel.SelectionLayerage;
            Layerage parents = LayerManager.GetParentsChildren(destination);
            if (parents.Children.Count < 2) return;

            int index = parents.Children.IndexOf(destination);
            index--;

            if (index < 0) index = 0;
            if (index > parents.Children.Count - 1) index = parents.Children.Count - 1;

            parents.Children.Remove(destination);
            parents.Children.Insert(index, destination);

            LayerManager.ArrangeLayers();
            this.ViewModel.Invalidate();//Invalidate
        }
        private void MoveFront()
        {
            if (this.Mode != ListViewSelectionMode.Single) return;

            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_LayersArrange);
            this.ViewModel.HistoryPush(history);

            Layerage destination = this.SelectionViewModel.SelectionLayerage;
            Layerage parents = LayerManager.GetParentsChildren(destination);
            if (parents.Children.Count < 2) return;

            parents.Children.Remove(destination);
            parents.Children.Insert(0, destination);

            LayerManager.ArrangeLayers();
            this.ViewModel.Invalidate();//Invalidate
        }


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
            LayersTransformAddHistory history = new LayersTransformAddHistory(HistoryType.LayersTransformAdd_Move);

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

                layerage.SetValueWithChildren((layerage2) =>
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
    }

    public sealed partial class OperateMenu : UserControl
    {

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
            LayersTransformAddHistory history = new LayersTransformAddHistory(HistoryType.LayersTransformAdd_Move);


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
                layerage.SetValueWithChildren((layerage2) =>
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