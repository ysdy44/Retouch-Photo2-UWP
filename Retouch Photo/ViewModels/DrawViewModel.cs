using System.Numerics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Library;
using Retouch_Photo.Models;
using Retouch_Photo.Controls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.Controls.ToolControls;
using Microsoft.Graphics.Canvas.Effects;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;
using Retouch_Photo.ViewModels.ToolViewModels;
using Microsoft.Graphics.Canvas.UI;

namespace Retouch_Photo.ViewModels
{
    public class DrawViewModel : INotifyPropertyChanged
    {

        /// <summary>画布控件</summary>
        public CanvasControl CanvasControl;
        public void Invalidate(bool isDottedLineRender = false, bool isLayerRender = false, bool? isThumbnail = null)
        {
            this.RenderLayer.RenderTarget= this.RenderLayer.GetRender(this.CanvasControl, this.Transformer.CanvasToVirtualMatrix);

            if (isThumbnail == true) this.CanvasControl.DpiScale = 0.5f;
            else if (isThumbnail == false) this.CanvasControl.DpiScale = 1.0f;

          this.CanvasControl.Invalidate();
        }
        public void InvalidateWithJumpedQueueLayer(Layer jumpedQueueLayer, bool? isThumbnail = null)
        {
            this.RenderLayer.RenderTarget = this.RenderLayer.GetRenderWithJumpedQueueLayer(this.CanvasControl, this.Transformer.CanvasToVirtualMatrix, jumpedQueueLayer);

            if (isThumbnail == true) this.CanvasControl.DpiScale = 0.5f;
            else if (isThumbnail == false) this.CanvasControl.DpiScale = 1.0f;

          this.CanvasControl.Invalidate();
        }


        /// <summary> 初始化CanvasControl, 也是可以绑定它的CreateResources事件</summary>
        public void InitializeCanvasControl(CanvasControl control)
        {
            if (this.CanvasControl != null) return;

            /*
            Dpi标准为=96

            我的Surface Book：
            CanvasControl.Dpi = 240;
            CanvasControl.DpiScale = 1;
            CanvasControl.ConvertPixelsToDips(240) = 96;
            CanvasControl.ConvertDipsToPixels(96, CanvasDpiRounding.Round) = 240;

            修改DpiScale为=0.4后：
            CanvasControl.Dpi = 96;
            CanvasControl.DpiScale = 0.4;
            CanvasControl.ConvertPixelsToDips(240) = 240;
            CanvasControl.ConvertDipsToPixels(96, CanvasDpiRounding.Round) = 96;

            可见
            CanvasControl.DpiScale = 96 / CanvasControl.Dpi;
            可以使DPI为标准的96，避免了位图的像素被缩放的问题
          （比如，在高分辨率的设备上，100 * 100的位图可能占用更多比如240 * 240的像素）

            在绘制之前，将DpiScale设为比1.0低的数，可以节省性能
            （注：如果数字太小或太大会崩溃）
            CanvasBitmap类在初始化时，它的DPI会和参数里的CanvasControl的DPI保持一致，请将它手动设为96.0f
             */
            //static float DefultDpi = 96.0f;
            //control.DpiScale = 96.0f / control.Dpi; 


            this.CanvasControl = control;
        }




        /// <summary>重新加载ViewModel，可以多次调用</summary>
        /// <param name="project">Project类型</param>
        public void LoadFromProject(Project project)
        {
            this.Transformer.LoadFromProject(project);

            /////////////////////////////////////////////////////////////////////////////////////
            
            this.RenderLayer.LoadFromProject(this.CanvasControl, project);
            this.RenderLayer.Layers.CollectionChanged += (s, e) =>
            {
                this.Invalidate(isLayerRender: true);
                this.SelectedIndex = this.RenderLayer.Index;
            };

            Layer.Remove += (Layer layer) =>
            {
                 this.RenderLayer.Remove(layer);
            };
 
            /////////////////////////////////////////////////////////////////////////////////////

            if (project.Tool < this.Tools.Count) this.Tool = this.Tools[project.Tool];

            /////////////////////////////////////////////////////////////////////////////////////

            this.Invalidate(true, true);
        }


        /// <summary>可以返回</summary>
        public GoBack GoBack = new GoBack();


        /// <summary>变形金刚(并不</summary>
        public Transformer Transformer = new Transformer();

        public MarqueeMode MarqueeMode = MarqueeMode.None;

        /// <summary>渲染图层</summary>
        public RenderLayer RenderLayer = new RenderLayer();
         


        #region Index & Tool

        
        /// <summary>控件选定索引</summary>      
        public int SelectedIndex
        {
            get=>selectedIndex;            
            set
            {
                selectedIndex = value < this.RenderLayer.Layers.Count ? value : this.RenderLayer.Layers.Count - 1;

                OnPropertyChanged(nameof(SelectedIndex));
            }
        }
        private int selectedIndex=-1;


        public Color Color = Color.FromArgb(255, 214, 214, 214);

        /// <summary>工具</summary>      
        public Tool Tool
        {
            get
            {
                if (tool == null) tool = this.Tools.FirstOrDefault();
                
                return tool;
            }
            set
            {
                tool = value;
                OnPropertyChanged(nameof(Tool));
            }
        }
        private Tool tool;
        

        /// <summary>所有工具</summary>
        public List<Tool> Tools => new List<Tool>
        {            
            new Tool()
        {
            Type = ToolType.Cursor,
            Icon = new ToolCursorControl(),
            WorkIcon = new ToolCursorControl(),
            Page = new ToolCursorPage(),
            ViewModel = new ToolCursorViewModel(),
        },
            new Tool()
        {
            Type = ToolType.View,
            Icon = new ToolViewControl(),
            WorkIcon = new ToolViewControl(),
            Page = new ToolViewPage(),
            ViewModel = new ToolViewViewModel(),
            },
            new Tool()
        {
            Type = ToolType.FloodSetect,
            Icon = new ToolFloodSetectControl(),
            WorkIcon = new ToolFloodSetectControl(),
            Page = new ToolFloodSetectPage(),
            ViewModel = new ToolFloodSetectViewModel(),
        },
            //[临时删除]
            /*
            new Tool()
        {
            Type = ToolType.SelectionBrush,
            Icon = new ToolSelectionBrushControl(),
            WorkIcon = new ToolSelectionBrushControl(),
            Page = new ToolSelectionBrushPage(),
            ViewModel = new ToolSelectionBrushViewModel(),
        },


            
            new Tool()
        {
            Type = ToolType.RectangularMarquee,
            Icon = new ToolRectangularMarqueeControl(),
            WorkIcon = new ToolRectangularMarqueeControl(),
            Page = new ToolRectangularMarqueePage(),
            ViewModel = new ToolRectangularMarqueeViewModel(),
        },
            new Tool()
        {
            Type = ToolType.EllipticalMarquee,
            Icon = new ToolEllipticalMarqueeControl(),
            WorkIcon = new ToolEllipticalMarqueeControl(),
            Page = new ToolEllipticalMarqueePage(),
            ViewModel = new ToolEllipticalMarqueeViewModel(),
        },
            new Tool()
        {
            Type = ToolType.PolygonMarquee,
            Icon = new ToolPolygonMarqueeControl(),
            WorkIcon = new ToolPolygonMarqueeControl(),
            Page = new ToolPolygonMarqueePage(),
            ViewModel = new ToolPolygonMarqueeViewModel(),
        },
            new Tool()
        {
            Type = ToolType.FreeHandMarquee,
            Icon = new ToolFreeHandMarqueeControl(),
            WorkIcon = new ToolFreeHandMarqueeControl(),
            Page = new ToolFreeHandMarqueePage(),
            ViewModel = new ToolFreeHandMarqueeViewModel(),
        },

             */
    
            new Tool()
        {
            Type = ToolType.PaintBrush,
            Icon = new ToolPaintBrushControl(),
            WorkIcon = new ToolPaintBrushControl(),
            Page = new ToolPaintBrushPage(),
            ViewModel = new ToolPaintBrushViewModel(),
        },
            new Tool()
        {
            Type = ToolType.WatercolorPen,
            Icon = new ToolWatercolorPenControl(),
            WorkIcon = new ToolWatercolorPenControl(),
            Page = new ToolWatercolorPenPage(),
            ViewModel = new ToolWatercolorPenViewModel(),
        },
            new Tool()
        {
            Type = ToolType.Pencil,
            Icon = new ToolPencilControl(),
            WorkIcon = new ToolPencilControl(),
            Page = new ToolPencilPage(),
            ViewModel = new ToolPencilViewModel(),
        },
            new Tool()
        {
            Type = ToolType.EraseBrush,
            Icon = new ToolEraseBrushControl(),
            WorkIcon = new ToolEraseBrushControl(),
            Page = new ToolEraseBrushPage(),
            ViewModel = new ToolEraseBrushViewModel(),
        },


            new Tool()
        {
            Type = ToolType.Pen,
            Icon = new ToolPenControl(),
            WorkIcon = new ToolPenControl(),
            Page = new ToolPenPage(),
            ViewModel = new ToolPenViewModel(),
        },
            new Tool()
        {
            Type = ToolType.Rectangle,
            Icon = new ToolRectangleControl(),
            WorkIcon = new ToolRectangleControl(),
            Page = new ToolRectanglePage(),
            ViewModel = new ToolRectangleViewModel(),
        },
            new Tool()
        {
            Type = ToolType.Ellipse,
            Icon = new ToolEllipseControl(),
            WorkIcon = new ToolEllipseControl(),
            Page = new ToolEllipsePage(),
            ViewModel = new ToolEllipseViewModel(),
        },
            new Tool()
        {
            Type = ToolType.Geometry,
            Icon = new ToolGeometryControl(),
            WorkIcon = new ToolGeometryControl(),
            Page = new ToolGeometryPage(),
            ViewModel = new ToolGeometryViewModel(),
        }
            };


        #endregion




        /// <summary>文本</summary>      
        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        private string text;
               

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
