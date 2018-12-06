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
               
        /// <summary>重新加载ViewModel</summary>
        /// <param name="project">Project类型</param>
        /// <param name="canvasWidth">控件宽度</param>
        /// <param name="canvasHeight">控件高度</param>
        public void LoadFromProject(Project project, float canvasWidth, float canvasHeight)
        {
            this.Width = project.Width;
            this.Height = project.Height;

            this.Scale = canvasWidth / this.Width / 8.0f * 7.0f;
            if (this.Scale < 0.1f) this.Scale = 0.1f;
            this.Position.X = canvasWidth / 2.0f;
            this.Position.Y = canvasHeight / 2.0f;

            this.Layers.Clear();
            foreach (Layer layer in project.Layers) this.Layers.Add(layer);
            this.Layers.CollectionChanged += (s, e) => this.Invalidate();
                        
            this.MarqueeSelection =new CanvasRenderTarget(this.CanvasControl, project.Width, project.Height);
            this.MarqueeTool.Complete += () =>
            {
                this.MarqueeTool.Render(this.CanvasControl,  this.MarqueeSelection);

                this.DottedLine.Render(this.CanvasControl, this.MarqueeSelection, this.GetMatrix());
            };


            this.GrayWhiteGrid = this.ToGrayWhiteGrid(this.CanvasControl, project.Width, project.Height);

            if (project.Tool < this.Tools.Count) this.Tool = this.Tools[project.Tool];
        }



        #region Canvas & Marquee


        /// <summary>画布控件</summary>
        public CanvasControl CanvasControl;
        public void Invalidate(bool isRender=false)
        {
            if (isRender) this.DottedLine.Render(this.CanvasControl, this.MarqueeSelection, this.GetMatrix());


            this.CanvasControl.Invalidate();
            this.Text =
                " X:" +
               ((int)this.Position.X).ToString()
               + " Y:" +
               ((int)this.Position.Y).ToString()
               + " Scale:" +
                this.Scale.ToString();
        }
        /// <summary>虚线</summary>
        public DottedLine DottedLine;

        /// <summary>选区</summary>
        public CanvasRenderTarget MarqueeSelection;
        public MarqueeTool MarqueeTool = new MarqueeTool();

        /// <summary> 初始化CanvasControl</summary>
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

             */
            //static float DefultDpi = 96.0f;
            control.DpiScale = 96.0f / control.Dpi;


            this.CanvasControl = control;
            this.DottedLine = new DottedLine(control);
        }


        #endregion


        #region GoBack


        public string GoBackText = "给ViewModel的CanvasControl赋值，需要提前实例化DrawPage再返回，没有实际意义的字符串";

        /// <summary>
        /// 是否经过了Frame的Goback
        /// （这个方法返回一次true后永远返回false，避免重复）
        /// 
        ///   private void Page_Loaded(object sender, RoutedEventArgs e)
        ///   {
        ///       if (this.ViewModel.IsGoBack) this.Frame.Navigate(typeof(MainPage));            
        ///   }
        ///</summary>
        public bool IsGoBack
        {
            get
            {
                if (isGoBack)
                {
                    isGoBack = false;
                    return true;
                }
                else return false;
            }
            set => isGoBack = value;
        }
        private bool isGoBack = false;



        /// <summary>
        /// 判断文字是否相同，来结束GoBack
        /// （避免多次GoBack的判断语句）
        ///</summary>
        /// 
        /// protected override void OnNavigatedTo(NavigationEventArgs e)//当前页面成为活动页面
        /// {
        ///      if (e.Parameter is string text)
        ///      {
        ///            if (this.ViewModel.HadGoBack(text)) return;
        ///      }
        /// }
        /// <param name="text"></param>
        /// <returns></returns>
        public bool HadGoBack(string text)
        {
            if (text == GoBackText)
            {
                IsGoBack = true;
                return true;
            }
            else return false;
        }

        #endregion


        #region Scale & Position


        /// <summary>宽度</summary>
        public float Width = 1000.0f;
        /// <summary>高度</summary>
        public float Height = 1000.0f;

        /// <summary>缩放</summary>
        public float Scale = 1.0f;
        /// <summary>位置</summary>
        public Vector2 Position;

        /// <summary>变换矩阵</summary>
        public Matrix3x2 GetMatrix() =>
            Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
            Matrix3x2.CreateScale(this.Scale) *
            Matrix3x2.CreateTranslation(this.Position);
        public Matrix3x2 GetInversionMatrix() =>
            Matrix3x2.CreateTranslation(-this.Position) *
            Matrix3x2.CreateScale(1 / this.Scale) *
            Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);


        #endregion


        #region Tool & 


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
            Page = new ToolPenPage(),
            ViewModel = new ToolPenViewModel(),
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






        #region Layer & Render


        /// <summary>所有图层</summary>      
        public ObservableCollection<Layer> Layers
        {
            get => layers;
            set
            {
                layers = value;
                OnPropertyChanged(nameof(Layers));
            }
        }
        private ObservableCollection<Layer> layers = new ObservableCollection<Layer>();


        /// <summary>生成渲染</summary>      
        public ICanvasImage ToRender(ICanvasImage image)
        {
            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                image = Layer.Render(this.Layers[i], image);
            }
            return image;
        }

        /// <summary>灰白网格</summary>
        public ICanvasImage GrayWhiteGrid;
        public ICanvasImage ToGrayWhiteGrid(ICanvasResourceCreator creator, int width, int height)
        {
            return new CropEffect//裁切成画布大小
            {
                SourceRectangle = new Rect(0, 0, Width, Height),
                Source = new DpiCompensationEffect//根据DPI适配
                {
                    Source = new ScaleEffect//缩放
                    {
                        Scale = new Vector2(8, 8),
                        InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                        Source = new BorderEffect//无限延伸图片
                        {
                            ExtendX = CanvasEdgeBehavior.Wrap,
                            ExtendY = CanvasEdgeBehavior.Wrap,
                            Source = CanvasBitmap.CreateFromColors
                            (
                               resourceCreator: creator,
                               widthInPixels: 2,
                               heightInPixels: 2,
                               colors: new Color[] //从数组创建2x2图片
                               {
                                   Color.FromArgb(255, 233, 233, 233),Colors.White,
                                   Colors.White,Color.FromArgb(255, 233, 233, 233),
                               }
                            )
                        }
                    }
                }
            };
        }


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
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
