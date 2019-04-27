using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Controls.LayerControls.GeometryControls;
using Retouch_Photo2.Library;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Models.Layers.GeometryLayers
{
    public class CurveLayer : GeometryLayer
    {
        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public static readonly string Type = "Curve";

        public List<Node> Nodes;
        public CanvasGeometry NodesGeometry;

        protected CurveLayer()
        {
            base.Name = CurveLayer.Type;
            base.Icon = new CurveControl();
        }
        
        /// <summary> Reset this NodesGeometry by Nodes. </summary>
        public void ResetNodesGeometryByNodes()=>this.NodesGeometry = CurveLayer.GetNodesGeometry(this.ViewModel.CanvasDevice, this.Nodes);
        /// <summary> Reset this Transformer by NodesGeometry. </summary>
        public void ResetTransformerByNodesGeometry() => base.Transformer = CurveLayer.GetTransformer(this.NodesGeometry);


        public override void LayerOnNavigatedFrom() 
        {
            // Reset the layer's  transformer
            Matrix3x2 matrix = base.Transformer.Matrix;
            foreach (Node node in this.Nodes)
            {
                node.Vector = Vector2.Transform(node.Vector, matrix);
                node.LeftControl = Vector2.Transform(node.LeftControl, matrix);
                node.RightControl = Vector2.Transform(node.RightControl, matrix);
            }
            this.ResetNodesGeometryByNodes();
            this.ResetTransformerByNodesGeometry();
        }

        protected override CanvasGeometry GetGeometry(Matrix3x2 canvasToVirtualMatrix)
        {
            return this.NodesGeometry.Transform(base.Transformer.Matrix).Transform(canvasToVirtualMatrix);
        }

        public static CurveLayer CreateFromPoint(ICanvasResourceCreator creator, Vector2 startPoint, Vector2 endPoint, Color color)
        {
            List<Node> nodes = new List<Node>
            {
                new Node(startPoint),
                new Node(endPoint),
            };

            return new CurveLayer
            {
                Transformer = Transformer.CreateFromVector(startPoint, endPoint),
                StrokeBrush = new Brush
                {
                    Type = BrushType.Color,
                    Color = color
                },
                Nodes = nodes,
                NodesGeometry = CurveLayer.GetNodesGeometry(creator, nodes)
            };
        }


        public static Transformer GetTransformer(CanvasGeometry nodesGeometry)
        {
            Rect rect = nodesGeometry.ComputeBounds();

            Vector2 leftTop = new Vector2((float)rect.Left, (float)rect.Top);
            Vector2 rightBottom = new Vector2((float)rect.Right, (float)rect.Bottom);

            return Transformer.CreateFromVector(leftTop, rightBottom);
        }

        public static CanvasGeometry GetNodesGeometry(ICanvasResourceCreator creator, List<Node> nodes, CanvasFigureLoop figureLoop= CanvasFigureLoop.Open)
        {
            using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(creator))
            {
                pathBuilder.BeginFigure(nodes.First().Vector);

                //Nodes
                for (int i = 0; i < nodes.Count - 1; i++)//0 to 9
                {
                    if (nodes[i].IsSmooth == false)                    
                        pathBuilder.AddLine(nodes[i + 1].Vector);                    
                    else                    
                        pathBuilder.AddCubicBezier(nodes[i].LeftControl, nodes[i + 1].RightControl, nodes[i + 1].Vector);                    
                }

                //Geometry
                pathBuilder.EndFigure(figureLoop);
                return CanvasGeometry.CreatePath(pathBuilder);
            }
        }

    }
}
