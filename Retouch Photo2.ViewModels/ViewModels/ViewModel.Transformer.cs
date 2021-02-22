using FanKit.Transformers;
using Retouch_Photo2.Tools.Models;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel
    {

        /// <summary> Gets or sets the canvas transformer. </summary>
        public CanvasTransformer CanvasTransformer { get; } = new CanvasTransformer();


        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s radian. </summary>
        public float CanvasTransformerRadian => this.CanvasTransformer.Radian;
        /// <summary> Notify <see cref="CanvasTransformerRadian"/>. </summary>
        public void NotifyCanvasTransformerRadian() => this.OnPropertyChanged(nameof(CanvasTransformerRadian));//Notify 
        /// <summary> Set <see cref="CanvasTransformerRadian"/>. </summary>
        public void SetCanvasTransformerRadian(float radian)
        {
            this.CanvasTransformer.Radian = radian;
            this.CanvasTransformer.ReloadMatrix();

            this.NotifyCanvasTransformerRadian();//Notify
        }
        /// <summary> Left rotate. </summary>
        public void CanvasTransformerLeftRotate(float sweep = 0.1f)
        {
            float radian = this.CanvasTransformer.Radian;
            radian -= sweep;
            if (radian < ViewRadianConverter.MinNumber) radian = ViewRadianConverter.MinNumber;
            this.SetCanvasTransformerRadian(radian);
        }
        /// <summary> Right rotate. </summary>
        public void CanvasTransformerRightRotate(float sweep = 0.1f)
        {
            float radian = this.CanvasTransformer.Radian;
            radian += sweep;
            if (radian > ViewRadianConverter.MaxNumber) radian = ViewRadianConverter.MaxNumber;
            this.SetCanvasTransformerRadian(radian);
        }



        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s scale. </summary>
        public float CanvasTransformerScale => this.CanvasTransformer.Scale;
        /// <summary> Notify <see cref="CanvasTransformerScale"/>. </summary>
        public void NotifyCanvasTransformerScale() => this.OnPropertyChanged(nameof(CanvasTransformerScale));//Notify 
        /// <summary> Set <see cref="CanvasTransformerScale"/>. </summary>
        public void SetCanvasTransformerScale(float scale)
        {
            this.CanvasTransformer.Scale = scale;
            this.CanvasTransformer.ReloadMatrix();

            this.NotifyCanvasTransformerScale();//Notify
        }

    }
}