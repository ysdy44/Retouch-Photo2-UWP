using Retouch_Photo.Tools.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo.Tools
{
    public abstract class Tool
    {
        public ToolType Type;

        public FrameworkElement Icon;
        public FrameworkElement WorkIcon;

        public ToolPage Page;

        public ToolViewModel ViewModel;

        public static List<Tool> ToolList = new List<Tool>
        {
             new CursorTool(),
             new ViewTool(),
             new FloodSetectTool(),
             new SelectionBrushTool(),

             new PaintBrushTool(),
             new WatercolorPenTool(),
             new PencilTool(),
             new EraseBrushTool(),

             new PenTool(),
             new RectangleTool(),
             new EllipseTool(),
             new GeometryTool(),

             new AcrylicTool(),
             new LineTool(),
        };
    }
}
