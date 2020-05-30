using FanKit.Transformers;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some methods of the application
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the snap. </summary>
        public VectorVectorSnap VectorVectorSnap = new VectorVectorSnap();
        /// <summary> Gets or sets the snap. </summary>
        public VectorBorderSnap VectorBorderSnap = new VectorBorderSnap();
        /// <summary> Gets or sets the snap. </summary>
        public BorderBorderSnap BorderBorderSnap = new BorderBorderSnap();


        /// <summary> Initiate the snap. </summary>
        public void VectorVectorSnapInitiate(NodeCollection nodes)
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
        /// <summary> Initiate the snap. </summary>
        public void VectorBorderSnapInitiate(Transformer transformer)
        {
            this.VectorBorderSnap.Destinations = this.GetSnapDestinations(transformer);

            //NodeRadius
            float scale = this.CanvasTransformer.Scale;
            this.VectorBorderSnap.NodeRadius = FanKit.Math.NodeRadius / scale;
        }
        /// <summary> Initiate the snap. </summary>
        public void VectorBorderSnapInitiate(Layerage firstLayer)
        {
            if (firstLayer != null)
            {
                this.VectorBorderSnap.Destinations = this.GetSnapDestinations(firstLayer);
            }

            //NodeRadius
            float scale = this.CanvasTransformer.Scale;
            this.VectorBorderSnap.NodeRadius = FanKit.Math.NodeRadius / scale;
        }
        /// <summary> Initiate the snap. </summary>
        public void BorderBorderSnapInitiate(Layerage firstLayer)
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
            float height = this.CanvasTransformer.Height;
            yield return new TransformerBorder(width, height);


            //Parents
            if (firstLayer.Parents != null)
            {
                Transformer transformer = firstLayer.Parents.Self.Transform.Transformer;
                yield return new TransformerBorder(transformer);
            }


            //Layers
            IList<Layerage> layers = this.LayerageCollection.GetParentsChildren(firstLayer);

            foreach (Layerage layer in layers)
            {
                if (layer.Self.IsSelected == false)
                {
                    Transformer transformer = layer.Self.Transform.Transformer;
                    yield return new TransformerBorder(transformer);
                }
            }
        }

    }
}