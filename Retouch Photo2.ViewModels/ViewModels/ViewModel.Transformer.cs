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
        /// <summary> Notify <see cref="CanvasTransformerRadian"/>. </summary>
        public void NotifyCanvasTransformerRadian() => this.OnPropertyChanged(nameof(this.CanvasTransformerRadian));//Notify 
        /// <summary> Set <see cref="CanvasTransformerRadian"/>. </summary>
        public void SetCanvasTransformerRadian(float radian)
        {
            this.CanvasTransformer.Radian = radian;
            this.CanvasTransformer.ReloadMatrix();

            this.NotifyCanvasTransformerRadian();//Notify
        }


        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s scale. </summary>
        public float CanvasTransformerScale => this.CanvasTransformer.Scale;
        /// <summary> Notify <see cref="CanvasTransformerScale"/>. </summary>
        public void NotifyCanvasTransformerScale() => this.OnPropertyChanged(nameof(this.CanvasTransformerScale));//Notify 
        /// <summary> Set <see cref="CanvasTransformerScale"/>. </summary>
        public void SetCanvasTransformerScale(float scale)
        {
            this.CanvasTransformer.Scale = scale;
            this.CanvasTransformer.ReloadMatrix();

            this.NotifyCanvasTransformerScale();//Notify
        }

    }
}