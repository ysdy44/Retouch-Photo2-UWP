using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Retouch_Photo2.Elements.MainPages;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />.
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        public VectorVectorSnap VectorVectorSnap = new VectorVectorSnap();
        public VectorBorderSnap VectorBorderSnap = new VectorBorderSnap();
        public BorderBorderSnap BorderBorderSnap = new BorderBorderSnap();


        public void VectorVectorSnapStarted(NodeCollection nodes)
        {
            this.VectorVectorSnap.Destinations =
                from node
                in nodes
                where node.IsChecked == false
                select node.Point;

            //NodeRadius
            float scale = this.CanvasTransformer.Scale;
            this.VectorVectorSnap.NodeRadius = FanKit.Math.NodeRadius / scale;
        }
        public void VectorBorderSnapStarted(Transformer transformer)
        {
            this.VectorBorderSnap.Destinations = this.GetSnapDestinations(transformer);

            //NodeRadius
            float scale = this.CanvasTransformer.Scale;
            this.VectorBorderSnap.NodeRadius = FanKit.Math.NodeRadius / scale;
        }
        public void VectorBorderSnapStarted(Layerage firstLayer)
        {
            if (firstLayer != null)
            {
                this.VectorBorderSnap.Destinations = this.GetSnapDestinations(firstLayer);
            }

            //NodeRadius
            float scale = this.CanvasTransformer.Scale;
            this.VectorBorderSnap.NodeRadius = FanKit.Math.NodeRadius / scale;
        }
        public void BorderBorderSnapStarted(Layerage firstLayer)
        {
            if (firstLayer != null)
            {
                this.BorderBorderSnap.Destinations = this.GetSnapDestinations(firstLayer);
            }
            
            //NodeRadius
            float scale = this.CanvasTransformer.Scale;
            this.BorderBorderSnap.NodeRadius = FanKit.Math.NodeRadius / scale;
        }
        

        private IEnumerable<TransformerBorder> GetSnapDestinations(Transformer transformer)
        {
            yield return new TransformerBorder(transformer);
        }
        private IEnumerable<TransformerBorder> GetSnapDestinations(Layerage firstLayer)
        {
            //CanvasTransformer
            float width = this.CanvasTransformer.Width;
            float height = this.CanvasTransformer.Width;
            yield return new TransformerBorder(width, height);


            //Parents
            if (firstLayer.Parents != null)
            {
                Transformer transformer = firstLayer.Parents.Self.Transform.Destination;
                yield return new TransformerBorder(transformer);
            }


            //Layers
            IList<Layerage> layers = this.LayerageCollection.GetParentsChildren(firstLayer);

            foreach (Layerage layer in layers)
            {
                if (layer.Self.IsSelected == false)
                {
                    Transformer transformer = layer.Self.Transform.Destination;
                    yield return new TransformerBorder(transformer);
                }
            }
        }


    }
}