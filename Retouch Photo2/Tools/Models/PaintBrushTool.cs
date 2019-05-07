using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.EllipticalGradient;
using Retouch_Photo2.Brushs.LinearGradient;
using Retouch_Photo2.Brushs.RadialGradient;
using Retouch_Photo2.Models.Layers;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.ITools;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Tools.Models
{
    public class PaintBrushTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        readonly IClickedTool IClickedTool;

        public PaintBrushTool()
        {
            base.Type = ToolType.PaintBrush;
            base.Icon = new PaintBrushControl();
            base.WorkIcon = new PaintBrushControl();
            base.Page = new PaintBrushPage();

            this.IClickedTool = new IClickedTool
            (
                start: (point) =>
                {
                    //Transformer
                    if (this.ViewModel.Brush != null)
                    {
                        Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;

                        switch (this.ViewModel.Brush.Type)
                        {
                            case BrushType.None:
                                {
                                    this.ViewModel.Brush.Type = BrushType.LinearGradient;
                                    if (this.ViewModel.Transformer is Transformer transformer)
                                    {
                                        this.LinearGradientInitialize(this.ViewModel.Brush.LinearGradientManager, transformer, point);//LinearGradient
                                    }
                                }
                                return true;

                            case BrushType.Color:
                                {
                                    this.ViewModel.Brush.Type = BrushType.LinearGradient;
                                    if (this.ViewModel.Transformer is Transformer transformer)
                                    {
                                        this.LinearGradientInitialize(this.ViewModel.Brush.LinearGradientManager, transformer, point);//LinearGradient
                                    }
                                }
                                return true;

                            case BrushType.LinearGradient:
                                {
                                    IGradientManager manager = this.ViewModel.Brush.LinearGradientManager;
                                    manager.Start(point, matrix);
                                }
                                break;

                            case BrushType.RadialGradient:
                                {
                                    IGradientManager manager = this.ViewModel.Brush.RadialGradientManager;
                                    manager.Start(point, matrix);
                                }
                                return true;

                            case BrushType.EllipticalGradient:
                                {
                                    IGradientManager manager = this.ViewModel.Brush.EllipticalGradientManager;
                                    manager.Start(point, matrix);
                                }
                                return true;

                            case BrushType.Image:
                                return true;

                            default:
                                return true;
                        }
                    }
                    return true;
                },
                delta: (point) =>
                {
                    //Transformer      
                    GeometryLayer geometryLayer = this.ViewModel.GeometryLayer;
                    if (geometryLayer != null)
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;
                        
                        switch (this.ViewModel.Brush.Type)
                        {
                            case BrushType.None:
                                return true;

                            case BrushType.Color:
                                return true;

                            case BrushType.LinearGradient:
                                {
                                    IGradientManager manager = this.ViewModel.Brush.LinearGradientManager;
                                    manager.Delta(point, inverseMatrix);
                                }
                                break;

                            case BrushType.RadialGradient:
                                {
                                    IGradientManager manager = this.ViewModel.Brush.RadialGradientManager;
                                    manager.Delta(point, inverseMatrix);
                                }
                                return true;

                            case BrushType.EllipticalGradient:
                                {
                                    IGradientManager manager = this.ViewModel.Brush.EllipticalGradientManager;
                                    manager.Delta(point, inverseMatrix);
                                }
                                return true;

                            case BrushType.Image:
                                return true;

                            default:
                                break;
                        }
                    }
                    return true;
                },
                complete: (point) =>
                {
                    //Transformer      
                    GeometryLayer geometryLayer = this.ViewModel.GeometryLayer;
                    if (geometryLayer != null)
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;

                        switch (this.ViewModel.Brush.Type)
                        {
                            case BrushType.None:
                                break;

                            case BrushType.Color:
                                break;

                            case BrushType.LinearGradient:
                                {
                                    IGradientManager manager = this.ViewModel.Brush.LinearGradientManager;
                                    manager.Complete();
                                }
                                break;

                            case BrushType.RadialGradient:
                                {
                                    IGradientManager manager = this.ViewModel.Brush.RadialGradientManager;
                                    manager.Complete();
                                }
                                break;

                            case BrushType.EllipticalGradient:
                                {
                                    IGradientManager manager = this.ViewModel.Brush.EllipticalGradientManager;
                                    manager.Complete();
                                }
                                break;

                            case BrushType.Image:
                                break;

                            default:
                                break;
                        }
                    }
                    return true;
                }
            );
        }

        public override void Start(Vector2 point) => this.IClickedTool.Start(point);
        public override void Delta(Vector2 point) => this.IClickedTool.Delta(point);
        public override void Complete(Vector2 point) => this.IClickedTool.Complete(point);

        public override void Draw(CanvasDrawingSession ds)
        {
            //Transformer      
            GeometryLayer geometryLayer = this.ViewModel.GeometryLayer;
            if (geometryLayer == null) return;
            else
            {
                Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;
                geometryLayer.Draw(ds, matrix);

                switch (this.ViewModel.Brush.Type)
                {
                    case BrushType.None:
                        break;

                    case BrushType.Color:
                        break;

                    case BrushType.LinearGradient:
                        {
                            IGradientManager manager = this.ViewModel.Brush.LinearGradientManager;
                            manager.Draw(ds, matrix);
                        }
                        break;

                    case BrushType.RadialGradient:
                        {
                            IGradientManager manager = this.ViewModel.Brush.RadialGradientManager;
                            manager.Draw(ds, matrix);
                        }
                        break;

                    case BrushType.EllipticalGradient:
                        {
                            IGradientManager manager = this.ViewModel.Brush.EllipticalGradientManager;
                            manager.Draw(ds, matrix);
                        }
                        break;

                    case BrushType.Image:
                        break;

                    default:
                        break;
                }

                this.ViewModel.Invalidate();
            }
        }


        private void LinearGradientInitialize(LinearGradientManager manager, Transformer transformer, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            manager.StartPoint = canvasPoint;
            manager.EndPoint = canvasPoint;

            manager.Type = LinearGradientType.EndPoint;

            this.ViewModel.SetBrush(BrushType.LinearGradient);
        }
    }
}
