using FanKit.Transformers;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some methods of the application
    /// </summary>
    public partial class ViewModel
    {

        /// <summary> Gets or sets the canvas transformer. </summary>
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