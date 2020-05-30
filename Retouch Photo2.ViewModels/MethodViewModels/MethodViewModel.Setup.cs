using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        public void MethodSetup(BitmapSize bitmapSize)
        {
            if (this.CanvasTransformer == bitmapSize) return;
            Vector2 scale = this.CanvasTransformer.GetScale(bitmapSize);

            //History
            LayersSetupHistory history = new LayersSetupHistory("Set canvas size", this.CanvasTransformer);

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
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.CacheTransform();
                    layer.TransformMultiplies(Matrix3x2.CreateScale(scale));
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
            LayersSetupHistory history = new LayersSetupHistory("Set canvas size", this.CanvasTransformer);

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
                    history.PushTransform(layer);

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
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