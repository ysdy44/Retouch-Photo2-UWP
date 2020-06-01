using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Linq;
using System.Numerics;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Text;

namespace Retouch_Photo2.TestApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        CanvasOperator CanvasOperator = new CanvasOperator();



        Node Node = null;
        bool isLaeft = false;

        public BlankPage1()
        {
            this.InitializeComponent();

            this.CanvasOperator.DestinationControl = this.canvas;

            this.CanvasOperator.Single_Start += (eee) =>
            {
                this.Node = null;
                this.Button.Content = "";
                foreach (Node item in this.NodeCollection)
                {
                    if (FanKit.Math.InNodeRadius(item.LeftControlPoint, eee))
                    {
                        this.Node = item;
                        this.Button.Content = "Has";
                        this.isLaeft = true;
                        this.canvas.Invalidate();
                        return;
                    }
                    if (FanKit.Math.InNodeRadius(item.RightControlPoint, eee))
                    {
                        this.Node = item;
                        this.Button.Content = "Has";
                        this.isLaeft = false;
                        this.canvas.Invalidate();
                        return;
                    }
                }
            };
            this.CanvasOperator.Single_Delta += (eee) =>
            {
                if (this.Node !=null)
                {
                    if (this.isLaeft)
                    {
                        this.Node.LeftControlPoint = eee;
                        this.CreatGeometry();
                        this.canvas.Invalidate();
                    }
                    else
                    {
                        this.Node.RightControlPoint = eee;
                        this.CreatGeometry();
                        this.canvas.Invalidate();
                    }
                }
            };
            this.CanvasOperator.Single_Complete += (eee) =>
            {

            };
        }





        private void canvas_Draw_1(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.FillGeometry(this.CanvasGeometry, Windows.UI.Colors.DarkSlateGray);
       //     foreach (CanvasTriangleVertices item in CanvasTriangleVertices)
     //       {
     //           args.DrawingSession.DrawLine(item.Vertex1, item.Vertex2, Colors.Red);
     //           args.DrawingSession.DrawLine(item.Vertex2, item.Vertex3, Colors.Green);
       //         args.DrawingSession.DrawLine(item.Vertex3, item.Vertex1, Colors.DodgerBlue);
      //      }

            args.DrawingSession.DrawNodeCollection(this.NodeCollection);
         }

        private void canvas_CreateResources_1(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            //  CanvasGeometry canvasGeometry = CanvasGeometry.CreateCircle(this.canvas, new Vector2(400, 400), 300);
            // CanvasGeometry canvasGeometry = CanvasGeometry.CreateRoundedRectangle(this.canvas, 400, 400, 300, 300, 55, 55);
            //     CanvasGeometry canvasGeometry = CanvasGeometry.CreateText(new CanvasTextLayout(sender, "AA", new CanvasTextFormat { FontSize = 333, }, 900, 222) { });
            CanvasGeometry canvasGeometry = TransformerGeometry.CreateDount(sender, new Transformer(new Vector2(300, 300), new Vector2(600, 600)), 0.5f);


            canvasGeometry.Simplify(CanvasGeometrySimplification.CubicsAndLines);
            if (this.IsStroke) canvasGeometry = canvasGeometry.Stroke(40);

            this.CanvasTriangleVertices = canvasGeometry.Tessellate();
            this.CanvasGeometry = canvasGeometry;

            this.NodeCollection.Clear();
            canvasGeometry.SendPathTo(this.NodeCollection);
        }



        private void CreatGeometry()
        {
            //Counterclockwise
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(canvas);
            pathBuilder.SetFilledRegionDetermination(this.NodeCollection.FilledRegionDetermination);
            pathBuilder.SetSegmentOptions( this.NodeCollection.FigureSegmentOptions);

            Node preview = this.NodeCollection.Last(node => node.Type == NodeType.Node);
            for (int i = 0; i < this.NodeCollection.Count; i++)
            {
                Node current = this.NodeCollection[i];

                switch (current.Type)
                {
                    case NodeType.BeginFigure:
                            pathBuilder.BeginFigure(current.Point, current.FigureFill);
                        break;
                    case NodeType.Node:
                        {
                            //pathBuilder.AddCubicBezier(preview.RightControlPoint, current.LeftControlPoint, current.Point);

                            if (current.IsSmooth && preview.IsSmooth)
                                pathBuilder.AddCubicBezier(preview.RightControlPoint, current.LeftControlPoint, current.Point);
                            else if (current.IsSmooth && preview.IsSmooth == false)
                                pathBuilder.AddCubicBezier(preview.Point, current.LeftControlPoint, current.Point);
                            else if (current.IsSmooth == false && preview.IsSmooth)
                                pathBuilder.AddCubicBezier(preview.RightControlPoint, current.Point, current.Point);
                            else
                                pathBuilder.AddLine(current.Point);

                            preview = current;
                        }
                        break;
                    case NodeType.EndFigure:
                        pathBuilder.EndFigure(current.FigureLoop);
                        break;
                }
            }

            CanvasGeometry canvasGeometry = CanvasGeometry.CreatePath(pathBuilder);

            if (this.IsStroke) canvasGeometry = canvasGeometry.Stroke(40);

            this.CanvasTriangleVertices = canvasGeometry.Tessellate();
            this.CanvasGeometry = canvasGeometry;

            this.TextBlock.Text = this.NodeCollection.StringBuilder.ToString();
        }




        CanvasTriangleVertices[] CanvasTriangleVertices;
        CanvasGeometry CanvasGeometry;
        NodeCollection NodeCollection = new NodeCollection();

        bool IsStroke = false;
        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.IsStroke = !this.IsStroke;
            this.CreatGeometry();
            this.canvas.Invalidate();
        }
    }
}
