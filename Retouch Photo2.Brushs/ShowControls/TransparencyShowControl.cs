// Core:              ★★
// Referenced:   ★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// A control used to show a transparency.
    /// </summary>
    public sealed partial class TransparencyShowControl : ShowControl
    {

        #region DependencyProperty


        /// <summary> Gets or sets the transparency. </summary>
        public IBrush Transparency
        {
            set
            {
                this._vsTransparency = value;
                this.Invalidate(); // Invalidate
            }
        }


        #endregion

        //@VisualState
        IBrush _vsTransparency;
        /// <summary>
        /// Invalidate.
        /// </summary>
        public void Invalidate()
        {
            this.RectangleFill = base.ToBrush(this._vsTransparency);
        }


        //@Construct
        /// <summary>
        /// Initializes a TransparencyShowControl. 
        /// </summary>
        public TransparencyShowControl()
        {
            this.InitializeComponent();
        }
    }
}