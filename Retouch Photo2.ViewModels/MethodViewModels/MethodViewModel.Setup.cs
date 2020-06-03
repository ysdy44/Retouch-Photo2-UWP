using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System.ComponentModel;
using System.Numerics;
using Windows.Graphics.Imaging;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some methods of the application
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        public void MethodSetup(BitmapSize bitmapSize)
        {
            if (this.CanvasTransformer == bitmapSize) return;
            Vector2 scale = this.CanvasTransformer.GetScale(bitmapSize);
            Matrix3x2 matrix = Matrix3x2.CreateScale(scale);

            //History
            LayersSetupTransformMultipliesHistory history = new LayersSetupTransformMultipliesHistory("Set canvas size", this.CanvasTransformer);

            //CanvasTransformer
            this.CanvasTransformer.BitmapSize = bitmapSize;
            this.CanvasTransformer.ReloadMatrix();

            //LayerageCollection
            foreach (Layerage layerage in this.LayerageCollection.RootLayerages)
            {
                //Selection
                this.SetLayerageValueWithChildren(layerage, (layerage2) =>
                {
                    ILayer layer = layerage2.Self;

                    //History
                    history.PushTransform(layer);

                    //Refactoring
                    layer.IsRefactoringTransformer = true;
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    //layerage.RefactoringParentsTransformer();
                    //layerage.RefactoringParentsRender();
                    //layerage.RefactoringParentsIconRender();
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });
            }

            //History
            this.HistoryPush(history);

            //Selection
            this.SetMode(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }


        public void MethodSetup(BitmapSize bitmapSize, IndicatorMode  indicatorMode)
        {
            if (this.CanvasTransformer == bitmapSize) return;
            Vector2 previousVector = this.CanvasTransformer.GetIndicatorVector(indicatorMode);

            //History
            LayersSetupTransformAddHistory history = new LayersSetupTransformAddHistory("Set canvas size", this.CanvasTransformer);

            //CanvasTransformer
            this.CanvasTransformer.BitmapSize = bitmapSize;
            this.CanvasTransformer.ReloadMatrix();

            Vector2 vector = this.CanvasTransformer.GetIndicatorVector(indicatorMode);
            Vector2 distance = vector - previousVector;

            //LayerageCollection
            foreach (Layerage layerage in this.LayerageCollection.RootLayerages)
            {
                //Selection
                this.SetLayerageValueWithChildren(layerage, (layerage2) =>
                {
                    ILayer layer = layerage2.Self;

                    //History
                    history.PushTransform(layer, distance);

                    //Refactoring
                    layer.IsRefactoringTransformer = true;
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    //layerage.RefactoringParentsTransformer();
                    //layerage.RefactoringParentsRender();
                    //layerage.RefactoringParentsIconRender();
                    layer.CacheTransform();
                    layer.TransformAdd(distance);
                });
            }

            //History
            this.HistoryPush(history);

            //Selection
            this.SetMode(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }

    }
}