using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Effects;
using Windows.UI;

namespace Retouch_Photo.Models.Layers
{
    public class AcrylicLayer : Layer
    {
         
        public static string ID = "AcrylicLayer";

        public VectorRect Rect;
        public float TintOpacity = 0.8f;
        public Color TintColor = Color.FromArgb(255, 255, 255, 255);

        public override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 matrix)
        { 
            return new CropEffect
            {
                SourceRectangle=Rect.Transform(matrix).ToRect(),
                Source=image
             }; 
        } 
        public override VectorRect GetBoundRect(ICanvasResourceCreator creator)
        {
            return this.Rect;
        }

         

    }
}
