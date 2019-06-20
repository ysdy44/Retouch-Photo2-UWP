using Retouch_Photo2.Library;

namespace ViewModels
{
    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel
    {

        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Library.CanvasTransformer" />. </summary>
        public CanvasTransformer CanvasTransformer { get; } = new CanvasTransformer();


        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s radian. </summary>
        public float CanvasRadian
        {
            get => this.CanvasTransformer.Radian;
            set
            {
                this.CanvasTransformer.Radian = value;
                this.OnPropertyChanged(nameof(this.CanvasRadian));//Notify 
            }
        }


        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s scale. </summary>
        public float CanvasScale
        {
            get => this.CanvasTransformer.Scale;
            set
            {
                this.CanvasTransformer.Scale = value;
                this.OnPropertyChanged(nameof(this.CanvasScale));//Notify 
            }
        }
        

    }
}