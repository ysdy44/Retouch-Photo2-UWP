using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    class NullTool:Tool
    {
        public NullTool()
        {
           // base.Type = null;
            base.Icon = null;
            base.WorkIcon = null;
            base.Page = null;
        }


        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }


        public override void Start(Vector2 point)
        {
        }
        public override void Delta(Vector2 point)
        {
        }
        public override void Complete(Vector2 point)
        {
        }

        public override void Draw(CanvasDrawingSession ds)
        {
        }
    }
}
