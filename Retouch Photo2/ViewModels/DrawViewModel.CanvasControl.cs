using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Retouch_Photo2.Models;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{

    public class CanvasControlManger
    {
        readonly CanvasControl CanvasControl;

        public CanvasControlManger(CanvasControl sender) => this.CanvasControl = sender;

        /// <summary> 获取或设置应用于此控件的 dpi 的缩放因子。 </summary>
        public float DpiScale
        {
            get => this.CanvasControl.DpiScale;
            set => this.CanvasControl.DpiScale = value;
        }

        /// <summary> 指示需要重新绘制载化管控制的内容。在不久之后引发的 "绘制" 事件中调用 "无效" 结果。 </summary>
        public void Invalidate() => this.CanvasControl.Invalidate();
    }

    public partial class DrawViewModel : INotifyPropertyChanged
    { 

        /// <summary> 画布管理 </summary>
        public CanvasControlManger CanvasManger;
        /// <summary> 画布设备 </summary>
        public CanvasDevice CanvasDevice { get; } = new CanvasDevice();

        /// <summary>
        ///  Indicates that the contents of the CanvasControl need to be redrawn. 
        ///  Calling Invalidate results in the Draw event being raised shortly afterward.
        /// </summary>
        /// <param name="isThumbnail"> draw thumbnails? </param>
        public void Invalidate(bool? isThumbnail = null)
        {
            if (this.CanvasManger == null) return;

            this.RenderLayer.RenderTarget = this.RenderLayer.GetRender
            (
                this.MatrixTransformer.CanvasToVirtualMatrix,
                this.MatrixTransformer.Width,
                this.MatrixTransformer.Height,
                this.MatrixTransformer.Scale
            );

            if (isThumbnail == true) this.CanvasManger.DpiScale = 0.5f;
            else if (isThumbnail == false) this.CanvasManger.DpiScale = 1.0f;

            this.CanvasManger.Invalidate();


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
        }

        public void InvalidateWithJumpedQueueLayer(Layer jumpedQueueLayer, bool? isThumbnail = null)
        {
            this.RenderLayer.RenderTarget = this.RenderLayer.GetRenderWithJumpedQueueLayer
            (
                jumpedQueueLayer,
                this.MatrixTransformer.CanvasToVirtualMatrix,
                this.MatrixTransformer.Width,
                this.MatrixTransformer.Height,
                this.MatrixTransformer.Scale
            );

            if (isThumbnail == true) this.CanvasManger.DpiScale = 0.5f;
            else if (isThumbnail == false) this.CanvasManger.DpiScale = 1.0f;

            this.CanvasManger.Invalidate();
        }
		 

    }
}
