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

        public void MethodSetup(BitmapSize size)
        {
            int previousWidth = this.CanvasTransformer.Width;
            int previousHeight = this.CanvasTransformer.Height;
            int width = (int)size.Width;
            int height = (int)size.Height;
            if (previousWidth == width && previousHeight == height) return;


            Matrix3x2 matrix = Matrix3x2.CreateScale((float)width / (float)previousWidth, (float)height / (float)previousHeight);

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set canvas size");

            //CanvasTransformer
            {
                //History
                var previous1 = previousWidth;
                var previous2 = previousHeight;
                history.UndoActions.Push(() =>
                {
                    this.CanvasTransformer.Width = previous1;
                    this.CanvasTransformer.Height = previous2;
                    this.CanvasTransformer.ReloadMatrix();
                });

                this.CanvasTransformer.Width = width;
                this.CanvasTransformer.Height = height;
                this.CanvasTransformer.ReloadMatrix();
            }

            //LayerageCollection
            foreach (Layerage layerage in this.LayerageCollection.RootLayerages)
            {
                //Selection
                this.SetLayerageValueWithChildren(layerage, (layerage2) =>
                {
                    ILayer layer = layerage2.Self;

                    //History
                    var previous = TransformPosition.GetLayer(layer);
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage2.Self;

                        TransformPosition.SetLayer(layer2, previous);
                    });

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


        public void MethodSetup(BitmapSize size, IndicatorMode  indicatorMode)
        {
            int previousWidth = this.CanvasTransformer.Width;
            int previousHeight = this.CanvasTransformer.Height;
            int width = (int)size.Width;
            int height = (int)size.Height;
            if (previousWidth == width && previousHeight == height) return;


            Vector2 previousVector = this.CanvasTransformer.GetIndicatorVector(indicatorMode);

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set canvas size");

            //CanvasTransformer
            {
                //History
                var previous1 = previousWidth;
                var previous2 = previousHeight;
                history.UndoActions.Push(() =>
                {
                    this.CanvasTransformer.Width = previous1;
                    this.CanvasTransformer.Height = previous2;
                    this.CanvasTransformer.ReloadMatrix();
                });

                this.CanvasTransformer.Width = width;
                this.CanvasTransformer.Height = height;
                this.CanvasTransformer.ReloadMatrix();
            }

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
                    var previous = TransformPosition.GetLayer(layer);
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage2.Self;

                        TransformPosition.SetLayer(layer2, previous);
                    });

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