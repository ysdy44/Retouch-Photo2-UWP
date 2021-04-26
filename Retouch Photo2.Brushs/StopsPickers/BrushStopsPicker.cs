namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Brush for <see cref="StopsPicker"/>.
    /// </summary>
    public class BrushStopsPicker : StopsPicker
    {

        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get => this.fillOrStroke;
            set
            {
                switch (value)
                {
                    case FillOrStroke.Fill: if (this.Fill != null) base.SetArray(this.Fill.Stops); break;
                    case FillOrStroke.Stroke: if (this.Stroke != null) base.SetArray(this.Stroke.Stops); break;
                }

                this.fillOrStroke = value;
            }
        }
        private FillOrStroke fillOrStroke;

        /// <summary> Gets or sets the fill. </summary>
        public IBrush Fill
        {
            get => this.fill;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        if (value != null) base.SetArray(value.Stops);
                        break;
                }
                this.fill = value;
            }
        }
        private IBrush fill;

        /// <summary> Gets or sets the stroke. </summary>
        public IBrush Stroke
        {
            get => this.stroke;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        if (value != null) base.SetArray(value.Stops);
                        break;
                }
                this.stroke = value;
            }
        }
        private IBrush stroke;


        #endregion

    }
}