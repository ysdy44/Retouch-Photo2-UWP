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
using Retouch_Photo2.Library;
using Retouch_Photo2.Models;
using Retouch_Photo2.Controls;
using Microsoft.Graphics.Canvas.Effects;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;
using Microsoft.Graphics.Canvas.UI;
using Windows.System;
using Windows.UI.Core;
using static Retouch_Photo2.Library.HomographyController;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.Tools;
using Retouch_Photo2.Models.Layers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Models.Layers.GeometryLayers;

namespace Retouch_Photo2.ViewModels
{
    public partial class DrawViewModel : INotifyPropertyChanged
    {
 
        public DrawViewModel()
        {
            Window.Current.CoreWindow.KeyUp += (s, e) =>
            {
                this.KeyUp(s, e);
                this.KeyUpAndDown(s, e);
            };
            Window.Current.CoreWindow.KeyDown += (s, e) =>
            {
                this.KeyDown(s, e);
                this.KeyUpAndDown(s, e);
            };
        }

        /// <summary>重新加载ViewModel，可以多次调用</summary>
        /// <param name="project">Project类型</param>
        public void LoadFromProject(Project project)
        {
            if (project == null) return;

            this.MatrixTransformer.LoadFromProject(project);

            this.RenderLayer.LoadFromProject(this.CanvasDevice, project);
            this.RenderLayer.Layers.CollectionChanged += (ssender, e) => this.Invalidate();

            int index = (project.Tool >= Tool.ToolList.Count) || (project.Tool < 0) ? 0 : project.Tool;
            this.Tool = Tool.ToolList[(ToolType)index];
            ToolControl.SetIndex(index);
        }



        /// <summary>标尺线</summary>   
        public bool IsRuler
        {
            get => this.MatrixTransformer.IsRuler;
            set
            {
                this.MatrixTransformer.IsRuler = value;
                this.OnPropertyChanged(nameof(this.IsRuler));//Notify 
            }
        }
        /// <summary>矩阵变换</summary>
        public MatrixTransformer MatrixTransformer = new MatrixTransformer();



        /// <summary>渲染图层</summary>
        public RenderLayer RenderLayer = new RenderLayer();
        /// <summary>控件选定索引</summary>      
        public int Index
        {
            get => this.RenderLayer.Index;
            set=>this.RenderLayer.Index = value;
        }
        /// <summary>当前图层</summary>     
        public Layer Layer
        {
            get => this.RenderLayer.Layer;
            set => this.RenderLayer.Layer = value;
        }
        public void SetLayer(Layer layer)
        {
            //Transformer
            if (layer == null)
                this.Transformer = null;
            else
                this.Transformer = layer.Transformer;

            //Acrylic
            this.SetAcrylicLayer(layer);

            //Geometry
            this.SetGeometryLayer(layer);

            //Line
            this.SetLineLayer(layer);

            //To
            if (layer != null)
                layer.LayerOnNavigatedTo();

            //Form
            if (this.RenderLayer.Layer != null)
                this.RenderLayer.Layer.LayerOnNavigatedFrom();

            this.RenderLayer.Layer = layer;

            this.OnPropertyChanged(nameof(this.Index));//Notify 
            this.OnPropertyChanged(nameof(this.Layer));//Notify 
        }


        /// <summary>几何图层</summary>     
        public GeometryLayer GeometryLayer;
        /// <summary>曲线图层</summary>     
        public CurveLayer CurveLayer;
        /// <summary>设置几何图层</summary>     
        private void SetGeometryLayer(Layer layer)
        {
            if (layer == null)
            {
                //.Geometry
                this.GeometryLayer = null;
                this.OnPropertyChanged(nameof(this.GeometryLayer));//Notify 
                //Brush
                this.Brush = null;
                this.OnPropertyChanged(nameof(this.Brush));//Notify 
                //Curve
                this.CurveLayer = null;
                this.OnPropertyChanged(nameof(this.CurveLayer));//Notify 
            }

            if (layer is GeometryLayer geometryLayer)
            {
                //.Geometry
                this.GeometryLayer = geometryLayer;
                this.OnPropertyChanged(nameof(this.GeometryLayer));//Notify 

                //StrokeWidth
                this.StrokeWidth = geometryLayer.StrokeWidth;
                this.OnPropertyChanged(nameof(StrokeWidth));

                //Brush
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill: this.SetBrush(geometryLayer.FillBrush); break;
                    case FillOrStroke.Stroke: this.SetBrush(geometryLayer.StrokeBrush); break;
                }

                //Curve
                if (layer is CurveLayer curveLayer) this.CurveLayer = curveLayer;
                else this.CurveLayer = null;
                this.OnPropertyChanged(nameof(this.CurveLayer));//Notify 
            }
        }


        /// <summary>描边宽度</summary>     
        public float StrokeWidth = 1.0f;
        /// <summary>填充或描边</summary>     
        public FillOrStroke FillOrStroke;
        /// <summary>设置填充或描边</summary> 
        public void SetFillOrStroke(FillOrStroke fillOrStroke, Retouch_Photo2.Brushs.Brush brush)
        {
            this.FillOrStroke = fillOrStroke;
            this.OnPropertyChanged(nameof(this.FillOrStroke));//Notify 

            this.Brush = brush;
            this.OnPropertyChanged(nameof(this.Brush));//Notify 
        }
        /// <summary>笔刷</summary>     
        public Retouch_Photo2.Brushs.Brush Brush;
        /// <summary>设置笔刷</summary>     
        public void SetBrush(BrushType type)
        {
            this.Brush.Type = type;
            this.OnPropertyChanged(nameof(this.Brush));
        }
        public void SetBrush(Retouch_Photo2.Brushs.Brush brush)
        {
            this.Brush = brush;
            if (brush.Type == BrushType.Color) this.Color = brush.Color;
            this.OnPropertyChanged(nameof(this.Brush));//Notify 
        }

        
        /// <summary>亚克力透明</summary>     
        public float AcrylicTintOpacity = 0.5f;
        /// <summary>亚克力模糊</summary>    
        public float AcrylicBlurAmount = 12.0f;
        /// <summary>亚克力图层</summary>
        public AcrylicLayer AcrylicLayer;
        /// <summary>设置亚克力图层</summary>     
        private void SetAcrylicLayer(Layer layer)
        {
            if (layer==null)
            {
                this.AcrylicLayer = null;
                this.OnPropertyChanged(nameof(this.AcrylicLayer));//Notify 

                return;
            }

            if (layer is AcrylicLayer acrylicLayer)
            {
                this.AcrylicTintOpacity = acrylicLayer.TintOpacity;
                this.OnPropertyChanged(nameof(this.AcrylicTintOpacity));//Notify 

                this.AcrylicBlurAmount = acrylicLayer.BlurAmount;
                this.OnPropertyChanged(nameof(this.AcrylicBlurAmount));//Notify 

                this.AcrylicLayer = acrylicLayer;
                this.OnPropertyChanged(nameof(this.AcrylicLayer));//Notify 

                return;
            }
        }

        
        /// <summary>线图层</summary>
        public LineLayer LineLayer;
        /// <summary>设置线图层</summary>     
        private void SetLineLayer(Layer layer)
        {
            if (layer == null)
            {
                this.LineLayer = null;
                this.OnPropertyChanged(nameof(LineLayer));

                return;
            }

            if (layer is LineLayer lineLayer)
            {
                this.StrokeWidth = lineLayer.StrokeWidth;
                this.OnPropertyChanged(nameof(StrokeWidth));
                 
                this.LineLayer = lineLayer;
                this.OnPropertyChanged(nameof(LineLayer));

                return;
            }
        }


        /// <summary>笔刷类型</summary>
        public BrushType BrushType;
        /// <summary>设置笔刷类型</summary>
        private void SetBrushType(BrushType brushType)
        {
            this.BrushType = brushType;
            this.OnPropertyChanged(nameof(this.BrushType));//Notify 
        }


        #region Index & Tool


        /// <summary>变换</summary>    
        private Transformer? transformer;
        public Transformer? Transformer
        {
            get => this.transformer;
            set
            {
                this.transformer = value;
                this.OnPropertyChanged(nameof(this.Transformer));//Notify 
            }
        }


        /// <summary>颜色</summary>    
        private Color color = Color.FromArgb(255, 214, 214, 214);
        public Color Color
        {
            get => this.color;
            set
            {
                this.color = value;
                this.OnPropertyChanged(nameof(this.Color));//Notify 
            }
        }

        /// <summary>描边颜色</summary>    
        private Color strokeColor = Color.FromArgb(255, 0, 0, 0);
        public Color StrokeColor
        {
            get => this.strokeColor;
            set
            {
                this.strokeColor = value;
                this.OnPropertyChanged(nameof(this.StrokeColor));//Notify 
            }
        }
        

        /// <summary>工具</summary>    
        private Tool tool = new NullTool();
        public Tool Tool
        {
            get => this.tool;
            set
            {
                this.tool.ToolOnNavigatedFrom();//当前页面不再成为活动页面
                value.ToolOnNavigatedTo();//当前页面成为活动页面

                this.tool = value;
                this.OnPropertyChanged(nameof(this.Tool));//Notify 
            }
        }
        

        #endregion
         

        #region Library


        //Curve
        public CurveNodes CurveNodes = new CurveNodes();
        
        //Transformer
        public TransformerMode TransformerMode = TransformerMode.None;
        public readonly Dictionary<TransformerMode, IController> TransformerDictionary = new Dictionary<TransformerMode, IController>
        {
             {TransformerMode.None,  new NoneController()},
             {TransformerMode.Translation,  new TranslationController()},
             {TransformerMode.Rotation,  new RotationController()},

             {TransformerMode.SkewLeft,  new SkewLeftController()},
             {TransformerMode.SkewTop,  new SkewTopController()},
             {TransformerMode.SkewRight,  new SkewRightController()},
             {TransformerMode.SkewBottom,  new SkewBottomController()},

             {TransformerMode.ScaleLeft,  new ScaleLeftController()},
             {TransformerMode.ScaleTop,  new ScaleTopController()},
             {TransformerMode.ScaleRight,  new ScaleRightController()},
             {TransformerMode.ScaleBottom,  new ScaleBottomController()},

             {TransformerMode.ScaleLeftTop,  new ScaleLeftTopController()},
             {TransformerMode.ScaleRightTop,  new ScaleRightTopController()},
             {TransformerMode.ScaleRightBottom,  new ScaleRightBottomController()},
             {TransformerMode.ScaleLeftBottom,  new ScaleLeftBottomController()},
        };
        
        /// <summary>选框模式</summary>
        public MarqueeMode MarqueeMode
        {
            get => marqueeMode;
            set
            {
                if (this.marqueeMode == value) return;
                this.marqueeMode = value; 
                this.OnPropertyChanged(nameof(this.MarqueeMode));//Notify 
            }
        }
        private MarqueeMode marqueeMode = MarqueeMode.None;


            
        #endregion


        /// <summary> 文本 </summary>      
        public string Text
        {
            get => this.text;
            set
            {
                this.text = value; 
                this.OnPropertyChanged(nameof(this.Text));//Notify 
            }
        }
        private string text;
               
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
