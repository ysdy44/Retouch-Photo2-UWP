// Core:              ★★
// Referenced:   ★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// A control used to show a brush.
    /// </summary>
    public sealed partial class BrushShowControl : ShowControl
    {

        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            set
            {
                this._vsFillOrStroke = value;
                this.Invalidate();//Invalidate
            }
        }

        /// <summary> Gets or sets the fill. </summary>
        public IBrush Fill
        {
            set
            {
                this._vsFill = value;
                this.Invalidate();//Invalidate
            }
        }

        /// <summary> Gets or sets the stroke. </summary>
        public IBrush Stroke
        {
            set
            {
                this._vsStroke = value;
                this.Invalidate();//Invalidate
            }
        }


        #endregion

        //@VisualState
        FillOrStroke _vsFillOrStroke;
        IBrush _vsFill;
        IBrush _vsStroke;
        /// <summary>
        /// Invalidate.
        /// </summary>
        public void Invalidate()
        {
            switch (this._vsFillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.RectangleFill = base.ToBrush(this._vsFill);
                    break;
                case FillOrStroke.Stroke:
                    this.RectangleFill = base.ToBrush(this._vsStroke);
                    break;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a BrushShowControl. 
        /// </summary>
        public BrushShowControl()
        {
            this.InitializeComponent();
        }
    }
}