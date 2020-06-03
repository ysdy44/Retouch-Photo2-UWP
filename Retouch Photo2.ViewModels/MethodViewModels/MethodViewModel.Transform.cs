using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System.ComponentModel;
using System.Numerics;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some methods of the application
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        public void MethodTransformMultiplies(Matrix3x2 matrix)
        {
            //History
            LayersTransformMultipliesHistory history = new LayersTransformMultipliesHistory("Transform");

            //Selection 
            this.CacheTransformer();
            Transformer transformer = this.StartingTransformer * matrix;
            this.Transformer = transformer;
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                history.PushTransform(layer);

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsTransformer();
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

                //Refactoring
                layer.IsRefactoringRender = true;
                layerage.RefactoringParentsRender();
                layer.TransformMultiplies(matrix);
            });

            this.Invalidate();//Invalidate
        }

        public void MethodTransformMultipliesComplete(Transformer transformer)
        {
            //History
            LayersTransformMultipliesHistory history = new LayersTransformMultipliesHistory("Transform");

            //Selection
            this.Transformer = transformer;
            Matrix3x2 matrix = Transformer.FindHomography(this.StartingTransformer, transformer);
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                history.PushStartingTransform(layer);

                //Refactoring
                layer.IsRefactoringTransformer = true;
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsTransformer();
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.TransformMultiplies(matrix);
            });

            //History
            this.HistoryPush(history);

            this.Invalidate(InvalidateMode.HD);//Invalidate
        }

        

        public void MethodTransformAdd(Vector2 vector)
        {
            //History
            LayersTransformAddHistory history = new LayersTransformAddHistory("Move");

            //Selection
            this.CacheTransformer();
            this.Transformer = Transformer.Add(this.StartingTransformer, vector);
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;

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

                //Refactoring
                layer.IsRefactoringRender = true;
                layerage.RefactoringParentsRender();
                layer.TransformAdd(vector);
            });

            this.Invalidate();//Invalidate
        }
    
        public void MethodTransformAddComplete(Vector2 vector)
        {
            //History
            LayersTransformAddHistory history = new LayersTransformAddHistory("Move");

            //Selection
            Transformer transformer = Transformer.Add(this.StartingTransformer, vector);
            this.Transformer = transformer;
            this.SetValueWithChildren((layerage) =>
            {
                ILayer layer = layerage.Self;
                
                //History
                history.PushTransform(layer, vector);

                //Refactoring
                layer.IsRefactoringTransformer = true;
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsTransformer();
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.TransformAdd(vector);
            });

            //History
            this.HistoryPush(history);

            this.Invalidate(InvalidateMode.HD);//Invalidate
        }
               

    }
}