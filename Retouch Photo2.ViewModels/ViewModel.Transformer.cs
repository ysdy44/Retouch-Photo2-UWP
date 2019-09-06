using FanKit.Transformers;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel
    {
        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Library.CanvasTransformer" />. </summary>
        public CanvasTransformer CanvasTransformer { get; } = new CanvasTransformer();


        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s radian. </summary>
        public float CanvasTransformerRadian => this.CanvasTransformer.Radian;
        public void NotifyCanvasTransformerRadian() => this.OnPropertyChanged(nameof(this.CanvasTransformerRadian));//Notify 
    
        
        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s scale. </summary>
        public float CanvasTransformerScale => this.CanvasTransformer.Scale;
        public void NotifyCanvasTransformerScale() => this.OnPropertyChanged(nameof(this.CanvasTransformerScale));//Notify 

    }
}