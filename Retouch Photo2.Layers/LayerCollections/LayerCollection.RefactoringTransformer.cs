using FanKit.Transformers;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {
        /// <summary>
        /// Refactoring the transformer.
        /// </summary>
        /// <param name="nodes"> The nodes. </param>
        /// <returns> The transformer. </returns>
        public static Transformer RefactoringTransformer(IList<Node> nodes)
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;

            foreach (Node node in nodes)
            {
                Vector2 vector = node.Point;

                if (left > vector.X) left = vector.X;
                if (top > vector.Y) top = vector.Y;
                if (right < vector.X) right = vector.X;
                if (bottom < vector.Y) bottom = vector.Y;
            }

            return new Transformer(left, top, right, bottom);
        }

        /// <summary>
        /// Refactoring the transformer.
        /// </summary>      
        /// <param name="layers"> The layers. </param>
        /// <returns> The transformer. </returns>
        public static Transformer RefactoringTransformer(IList<ILayer> layers)
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;

            //Foreach
            void aaa(Vector2 vector)
            {
                if (left > vector.X) left = vector.X;
                if (top > vector.Y) top = vector.Y;
                if (right < vector.X) right = vector.X;
                if (bottom < vector.Y) bottom = vector.Y;
            }

            //Foreach
            foreach (ILayer layer in layers)
            {
                Transformer transformer = layer.GetActualDestinationWithRefactoringTransformer;
                aaa(transformer.LeftTop);
                aaa(transformer.RightTop);
                aaa(transformer.RightBottom);
                aaa(transformer.LeftBottom);
            }

            return new Transformer(left, top, right, bottom);
        }
    }
}
