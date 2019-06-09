using System.Numerics;

namespace Retouch_Photo2.Library
{
    /// <summary> Define Transformer. </summary>
    public partial struct Transformer
    {  
        /// <summary> Radius of node'. Defult 12. </summary>
        private const float NodeRadius = 12.0f;
        /// <summary> Whether the distance exceeds [NodeRadius]. Defult: 144. </summary>
        public static bool InNodeRadius(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() < 144.0f;// Transformer.NodeRadius * Transformer.NodeRadius;


        /// <summary> Minimum distance between two nodes. Defult 20. </summary>
        private const float NodeDistance = 20.0f;
        /// <summary> Double [NodeDistance]. Defult 40. </summary>
        private const float NodeDistanceDouble = 40.0f;
        /// <summary> Whether the distance'LengthSquared exceeds [NodeDistance]. Defult: 400. </summary>
        public static bool OutNodeDistance(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() > 400.0f;// Transformer.NodeDistance * Transformer.NodeDistance;


        /// <summary> Get outside node. </summary>
        internal static Vector2 OutsideNode(Vector2 nearNode, Vector2 farNode) => nearNode - Vector2.Normalize(farNode - nearNode) * Transformer.NodeDistanceDouble;

        /// <summary>
        /// Point inside the Quadrangle
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="transformer"> Layer's Transformer. </param>
        /// <returns></returns>
        public static bool InQuadrangle(Vector2 point, Transformer transformer) => Transformer.InQuadrangle(point, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom);

        /// <summary>
        /// Point inside the Quadrangle
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <returns></returns>
        public static bool InQuadrangle(Vector2 point, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            float a = (leftTop.X - leftBottom.X) * (point.Y - leftBottom.Y) - (leftTop.Y - leftBottom.Y) * (point.X - leftBottom.X);
            float b = (rightTop.X - leftTop.X) * (point.Y - leftTop.Y) - (rightTop.Y - leftTop.Y) * (point.X - leftTop.X);
            float c = (rightBottom.X - rightTop.X) * (point.Y - rightTop.Y) - (rightBottom.Y - rightTop.Y) * (point.X - rightTop.X);
            float d = (leftBottom.X - rightBottom.X) * (point.Y - rightBottom.Y) - (leftBottom.Y - rightBottom.Y) * (point.X - rightBottom.X);
            return (a > 0 && b > 0 && c > 0 && d > 0) || (a < 0 && b < 0 && c < 0 && d < 0);
        }

        /// <summary>
        /// Gets the radian area filled by the skew node contains the specified point. 
        /// </summary>
        /// <param name="point"> Input point. </param>
        /// <param name="transformer"> Layer's Transformer. </param>
        /// <param name="matrix"></param>
        /// <param name="disabledRadian"> disabled radian </param>
        /// <returns></returns>
        public static TransformerMode ContainsNodeMode(Vector2 point, Transformer transformer, Matrix3x2 matrix, bool disabledRadian = false)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            //Scale2
            if (Transformer.InNodeRadius(leftTop, point)) return TransformerMode.ScaleLeftTop;
            if (Transformer.InNodeRadius(rightTop, point)) return TransformerMode.ScaleRightTop;
            if (Transformer.InNodeRadius(rightBottom, point)) return TransformerMode.ScaleRightBottom;
            if (Transformer.InNodeRadius(leftBottom, point)) return TransformerMode.ScaleLeftBottom;

            //Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            //Scale1
            if (Transformer.InNodeRadius(centerLeft, point)) return TransformerMode.ScaleLeft;
            if (Transformer.InNodeRadius(centerTop, point)) return TransformerMode.ScaleTop;
            if (Transformer.InNodeRadius(centerRight, point)) return TransformerMode.ScaleRight;
            if (Transformer.InNodeRadius(centerBottom, point)) return TransformerMode.ScaleBottom;

            //Outside
            Vector2 outsideLeft = Transformer.OutsideNode(centerLeft, centerRight);
            Vector2 outsideTop = Transformer.OutsideNode(centerTop, centerBottom);
            Vector2 outsideRight = Transformer.OutsideNode(centerRight, centerLeft);
            Vector2 outsideBottom = Transformer.OutsideNode(centerBottom, centerTop);

            //Rotation
            if (disabledRadian == false)
            {
                if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.Rotation;

                //Skew
                //if (Transformer.InNodeRadius(outsideLeft, point)) return TransformerMode.SkewLeft;
                //if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.SkewTop;
                if (Transformer.InNodeRadius(outsideRight, point)) return TransformerMode.SkewRight;
                if (Transformer.InNodeRadius(outsideBottom, point)) return TransformerMode.SkewBottom;
            }

            //Translation
            if (Transformer.InQuadrangle(point, leftTop, rightTop, rightBottom, leftBottom))
            {
                return TransformerMode.Translation;
            }

            return TransformerMode.None;
        }
    }
}
