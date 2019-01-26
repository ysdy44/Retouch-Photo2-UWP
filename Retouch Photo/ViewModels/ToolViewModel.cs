using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels
{

    public interface IToolViewModel
    {
        void Start(Vector2 point, Layer layer);
        void Delta(Vector2 point, Layer layer);
        void Complete(Vector2 point, Layer layer);

        void Draw(CanvasDrawingSession ds, Layer layer);
    }

    public abstract class ToolViewModel
    {
        public abstract void Start(Vector2 point);
        public abstract void Delta(Vector2 point);
        public abstract void Complete(Vector2 point);

        public abstract void Draw(CanvasDrawingSession ds);

        //@Override
        /// <summary> 当前页面成为活动页面 </summary>
        public abstract void ToolOnNavigatedTo();
        /// <summary> 当前页面不再成为活动页面 </summary>
        public abstract void ToolOnNavigatedFrom();
    }
}
