using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System.ComponentModel;
using System.Numerics;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {


        public void MethodTransformMultiplies(Matrix3x2 matrix)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Transform");

            //Selection 
            this.CacheTransformer();
            Transformer transformer = this.StartingTransformer * matrix;
            this.Transformer = transformer;
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Transform.Destination;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Transform.Destination = previous;
                });

                layer.CacheTransform(); 
                layer.TransformMultiplies(matrix);
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }

        
        public void MethodTransformMultipliesStarted()
        {
            //Selection
            this.CacheTransformer();
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.CacheTransform();
            });

            this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
         
        public void MethodTransformMultipliesDelta(Transformer transformer)
        {
            //Selection
            this.Transformer = transformer;
            Matrix3x2 matrix = Transformer.FindHomography(this.StartingTransformer, transformer);
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.TransformMultiplies(matrix);
            });

            this.Invalidate();//Invalidate
        }

        public void MethodTransformMultipliesComplete(Transformer transformer)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Transform");

            //Selection
            this.Transformer = transformer;
            Matrix3x2 matrix = Transformer.FindHomography(this.StartingTransformer, transformer);
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.TransformMultiplies(matrix);

                //History
                var previous = layer.Transform.StartingDestination;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Transform.Destination = previous;
                });
            });

            //History
            this.HistoryPush(history);

            this.Invalidate(InvalidateMode.HD);//Invalidate
        }



        public void MethodTransformAdd(Vector2 vector)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Move");

            //Selection
            this.CacheTransformer();
            this.Transformer = Transformer.Add(this.StartingTransformer, vector);
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Transform.Destination;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Transform.Destination = previous;
                });

                layer.CacheTransform();
                layer.TransformAdd(vector);
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }


        public void MethodTransformAddStarted()
        {
            //Selection
            this.CacheTransformer();
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.CacheTransform();
            });

            this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }

        public void MethodTransformAddDelta(Vector2 vector)
        {
            //Selection
            Transformer transformer = Transformer.Add(this.StartingTransformer, vector);
            this.Transformer = transformer;
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.TransformAdd(vector);
            });

            this.Invalidate();//Invalidate
        }
    
        public void MethodTransformAddComplete(Vector2 vector)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Move");

            //Selection
            Transformer transformer = Transformer.Add(this.StartingTransformer, vector);
            this.Transformer = transformer;
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Transform.StartingDestination;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Transform.Destination = previous;
                });

                layer.TransformAdd(vector);
            });

            //History
            this.HistoryPush(history);

            this.Invalidate(InvalidateMode.HD);//Invalidate
        }
               

    }
}