using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Brushs;
using Retouch_Photo.Controls.LayerControls.GeometryControls;
using Retouch_Photo.Library;
using Retouch_Photo.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Models.Layers.GeometryLayers
{
    public class CurveLayer : GeometryLayer
    {
        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public static readonly string Type = "Curve";

        public List<Node> Nodes=new List<Node>();

        protected CurveLayer()
        {
            base.Name = CurveLayer.Type;
            base.Icon = new CurveControl();
        }

        protected override CanvasGeometry GetGeometry(ICanvasResourceCreator creator, Matrix3x2 canvasToVirtualMatrix)
        {
            using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(creator))
            {
                Node first = this.Nodes.First();
                pathBuilder.BeginFigure(first.VectorTransform);

                //Nodes
                for (int i = 0; i < this.Nodes.Count - 1; i++)//0 to 9
                {
                    if (this.Nodes[i].IsSmooth == false)                    
                        pathBuilder.AddLine(this.Nodes[i + 1].VectorTransform);                    
                    else                    
                        pathBuilder.AddCubicBezier(this.Nodes[i].LeftControlTransform, this.Nodes[i + 1].RightControlTransform, this.Nodes[i + 1].VectorTransform);
                }

                //Geometry
                pathBuilder.EndFigure(CanvasFigureLoop.Open);
                return CanvasGeometry.CreatePath(pathBuilder);
            }
        }

        public static CurveLayer CreateFromPoint(ICanvasResourceCreator creator, Vector2 startPoint, Vector2 endPoint,Color color)
        {
            return new CurveLayer
            {
                Transformer = Transformer.CreateFromVector(startPoint, endPoint),
                StrokeBrush = new Brush
                {
                    Type = BrushType.Color,
                    Color = color
                },
                Nodes = new List<Node>
                {
                    new Node(startPoint),
                    new Node(endPoint),                    
                }
            };
        }
    }

}
