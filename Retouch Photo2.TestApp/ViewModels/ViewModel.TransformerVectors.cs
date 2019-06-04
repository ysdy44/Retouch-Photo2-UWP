using Retouch_Photo2.Layers;
using Retouch_Photo2.Library;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel
    {

        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Library.TransformerVectors" />. </summary>
        public TransformerVectors? TransformerVectors;

        /// <summary>
        /// Gets <see cref = "TransformerVectors" /> for all checked layers.
        /// </summary>
        /// <param name="layers"> all layers. </param>
        /// <returns> Returns **null** if there are no checked layers. </returns>
        public TransformerVectors? GetCheckedLayersTransformerVectors(IEnumerable<Layer> layers)
        {
            //Value
            bool existCheckedLayer = false;
            float left = 0, top = 0, right = 0, bottom = 0;

            void aaa(TransformerVectors transformerVectors)
            {
                left = transformerVectors.MinX;
                top = transformerVectors.MinY;
                right = transformerVectors.MaxX;
                bottom = transformerVectors.MaxY;
            }

            void bbb(Vector2 vector)
            {
                if (left > vector.X) left = vector.X;
                if (top > vector.Y) top = vector.Y;
                if (right < vector.X) right = vector.X;
                if (bottom < vector.Y) bottom = vector.Y;
            }


            //Foreach
            foreach (Layer item in layers)
            {
                if (item.IsChecked)
                {
                    if (existCheckedLayer == false)
                    {
                        // Frist checked layer
                        aaa(item.Transformer.DestinationVectors);
                    }
                    else
                    {
                        // Compare other layers
                        bbb(item.Transformer.DestinationVectors.LeftTop);
                        bbb(item.Transformer.DestinationVectors.RightTop);
                        bbb(item.Transformer.DestinationVectors.RightTop);
                        bbb(item.Transformer.DestinationVectors.LeftBottom);
                    }

                    existCheckedLayer = true;
                }
            }


            //Return
            if (existCheckedLayer == false) return null;

            return new TransformerVectors(left, top, right, bottom);
        }

    }
}