using Retouch_Photo2.Brushs;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System.ComponentModel;
using Windows.UI;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some methods of the application
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        public void MethodFillColorChanged(Color value)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set fill");

            //Selection
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.Color = value;
                    break;
            }

            this.Fill = BrushBase.ColorBrush(value);
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Fill.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Fill = previous.Clone();
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layer.Style.Fill = BrushBase.ColorBrush(value);

                this.StandStyleLayerage = layerage;
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }

        public void MethodFillColorChangeStarted(Color value)
        {
            //Selection
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;
                layer.Style.CacheFill();
            });

            this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }

        public void MethodFillColorChangeDelta(Color value)
        {
            //Selection
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //Refactoring
                layer.IsRefactoringRender = true;
                layerage.RefactoringParentsRender();
                layer.Style.Fill = BrushBase.ColorBrush(value);
            });

            this.Invalidate();//Invalidate         
        }

        public void MethodFillColorChangeCompleted(Color value)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set fill");

            //Selection
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.Color = value;
                    break;
            }

            this.Fill = BrushBase.ColorBrush(value);
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.StartingFill.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Fill = previous.Clone();
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Style.Fill = BrushBase.ColorBrush(value);

                this.StandStyleLayerage = layerage;
            });

            //History
            this.HistoryPush(history);

            this.Invalidate(InvalidateMode.HD);//Invalidate 
        }

               


        public void MethodStrokeColorChanged(Color value)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set stroke");

            //Selection
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Stroke:
                    this.Color = value;
                    break;
            }
            this.Stroke = BrushBase.ColorBrush(value);
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Stroke.Clone();
                history.UndoAction += () =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Style.Stroke = previous.Clone();
                };

                layer.Style.Stroke = BrushBase.ColorBrush(value);
                this.StandStyleLayerage = layerage;
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }

        public void MethodStrokeColorChangeStarted(Color value)
        {
            //Selection
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;
                layer.Style.CacheStroke();
            });

            this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }

        public void MethodStrokeColorChangeDelta(Color value)
        {
            //Selection
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.Style.Stroke = BrushBase.ColorBrush(value);
            });

            this.Invalidate();//Invalidate         
        }

        public void MethodStrokeColorChangeCompleted(Color value)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set stroke");

            //Selection
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Stroke:
                    this.Color = value;
                    break;
            }

            this.Stroke = BrushBase.ColorBrush(value);
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.StartingStroke.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Stroke = previous.Clone();
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layer.Style.Stroke = BrushBase.ColorBrush(value);

                this.StandStyleLayerage = layerage;
            });

            //History
            this.HistoryPush(history);

            this.Invalidate(InvalidateMode.HD);//Invalidate 
        }


    }
}
