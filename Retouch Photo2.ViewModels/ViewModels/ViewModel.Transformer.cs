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
        #region Transition


        Vector2 _sourcePosition = new Vector2(100, 100);
        float _sourceScale = 0.2f;

        Vector2 _destinationPosition;
        float _destinationScale;


        public void TransitionSource(Point postion, double actualWidth, double actualHeight)
        {
            float left = (float)postion.X;
            float top = (float)postion.Y;
            float width = (float)actualWidth;
            float height = (float)actualHeight;

            this._sourcePosition.X = left + width / 2.0f;
            this._sourcePosition.Y = top + height / 2.0f;
            this._sourceScale = this._getTransitionScale(width, height);
        }


        public void TransitionDestination(Vector2 offset, float controlWidth, float controlHeight)
        {
            this._destinationPosition.X = offset.X + controlWidth / 2.0f;
            this._destinationPosition.Y = offset.Y + controlHeight / 2.0f;
            this._destinationScale = this._getTransitionScale(controlWidth, controlHeight);
        }


        private float _getTransitionScale(float width, float height)
        {
            float widthScale = width / this.CanvasTransformer.Width;
            float heightScale = height / this.CanvasTransformer.Height;

            float scale = System.Math.Min(widthScale, heightScale);
            return scale;
        }

        public void Transition(float value)
        {
            if (value == 0.0f)
            {
                this.CanvasTransformer.Position = this._sourcePosition;
                this.CanvasTransformer.Scale = this._sourceScale;
            }
            else if (value == 1.0f)
            {
                this.CanvasTransformer.Position = this._destinationPosition;
                this.CanvasTransformer.Scale = this._destinationScale;
            }
            else
            {
                float minusValue = 1.0f - value;

                Vector2 position = this._sourcePosition * minusValue + this._destinationPosition * value;
                this.CanvasTransformer.Position = position;

                float scale = this._sourceScale * minusValue + this._destinationScale * value;
                this.CanvasTransformer.Scale = scale;
            }

            this.CanvasTransformer.ReloadMatrix();
        }


        #endregion


        #region CanvasTransformer


        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Library.CanvasTransformer" />. </summary>
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


        #endregion

    }
}