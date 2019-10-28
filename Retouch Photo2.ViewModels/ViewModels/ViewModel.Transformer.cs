using FanKit.Transformers;
using System.Numerics;
using Windows.Foundation;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />.
    /// </summary>
    public partial class ViewModel
    {

        /// <summary> Whether the page transitions when the page navigate. </summary>
        public bool IsTransition { get; set; } = false;
        /// <summary> Retouch_Photo2's the only <see cref = "FanKit.Transformers.CanvasTransformer" />. </summary>
        public CanvasTransformer CanvasTransformer { get; } = new CanvasTransformer();


        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s radian. </summary>
        public float CanvasTransformerRadian => this.CanvasTransformer.Radian;
        public void NotifyCanvasTransformerRadian() => this.OnPropertyChanged(nameof(this.CanvasTransformerRadian));//Notify 
        public void SetCanvasTransformerRadian(float radian)
        {
            this.CanvasTransformer.Radian = radian;
            this.CanvasTransformer.ReloadMatrix();

            this.NotifyCanvasTransformerRadian();//Notify
            this.Invalidate();//Invalidate
        }


        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s scale. </summary>
        public float CanvasTransformerScale => this.CanvasTransformer.Scale;
        public void NotifyCanvasTransformerScale() => this.OnPropertyChanged(nameof(this.CanvasTransformerScale));//Notify 
        public void SetCanvasTransformerScale(float scale)
        {
            this.CanvasTransformer.Scale = scale;
            this.CanvasTransformer.ReloadMatrix();

            this.NotifyCanvasTransformerScale();//Notify
            this.Invalidate();//Invalidate
        }

    }
}