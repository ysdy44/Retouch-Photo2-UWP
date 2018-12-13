﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;

namespace Retouch_Photo.Models.Layers.GeometryLayers
{
    public class RectangularLayer:GeometryLayer
    {
        public static string ID = "RectangularLayer";

        public VectorRect Rect;
         
        public override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image)
        {
            CanvasCommandList command = new CanvasCommandList(creator);

            using (CanvasDrawingSession ds= command.CreateDrawingSession())
            {
                if (this.IsFill) ds.FillRectangle(this.Rect.X, this.Rect.Y, this.Rect.Width, this.Rect.Height, this.FillBrush);
                if (this.IsStroke) ds.DrawRectangle(this.Rect.X, this.Rect.Y, this.Rect.Width, this.Rect.Height, this.StrokeBrush, this.StrokeWidth);
            }

            return new CompositeEffect
            {
                Sources =
                {
                    new CropEffect
                    {
                        SourceRectangle = this.Rect.ToRect(),
                        BorderMode = EffectBorderMode.Hard,
                        Source = new GaussianBlurEffect
                        {
                            BlurAmount = 20.0f,
                            Source = image
                        }
                    },
                    new OpacityEffect
                    {
                        Source=command,
                        Opacity=0.3f
                    },
                }
            };
        }

        public override void CurrentDraw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
        }
        public override VectorRect GetBoundRect(ICanvasResourceCreator creator)
        {
            return this.Rect;
        }

        public static RectangularLayer CreateFromRect(ICanvasResourceCreator creator, VectorRect rect, Color color)
        {
            return new RectangularLayer
            {
                Rect = rect,
                FillBrush = new CanvasSolidColorBrush(creator, color)
            };
        }

    

    }
}
