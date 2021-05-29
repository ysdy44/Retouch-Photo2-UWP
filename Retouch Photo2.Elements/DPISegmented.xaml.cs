// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Dots-per-inch (DPI).
    /// </summary>
    public enum DPI
    {
        /// <summary> 72 </summary>
        DPI72 = 72,
        /// <summary> 96 </summary>
        DPI96 = 96,
        /// <summary> 144 </summary>
        DPI144 = 144,
        /// <summary> 192 </summary>
        DPI192 = 192,
        /// <summary> 300 </summary>
        DPI300 = 300,
        /// <summary> 400 </summary>
        DPI400 = 400,
    }

    /// <summary>
    /// Segmented of <see cref="Retouch_Photo2.Elements.DPI"/>.
    /// </summary>
    public sealed partial class DPISegmented : UserControl
    {

        //@VisualState
        private DPI _vsDPI;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsDPI)
                {
                    case DPI.DPI72: return this.DPI72;
                    case DPI.DPI96: return this.DPI96;
                    case DPI.DPI144: return this.DPI144;
                    case DPI.DPI192: return this.DPI192;
                    case DPI.DPI300: return this.DPI300;
                    case DPI.DPI400: return this.DPI400;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        /// <summary> Gets or sets the dots-per-inch (DPI). </summary>
        public DPI DPI
        {
            get => this._vsDPI;
            set
            {
                this._vsDPI = value;
                this.VisualState = this.VisualState; // State
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a DPIComboBox.
        public DPISegmented()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState; // State
            this.DPI72Button.Tapped += (s, e) => this.DPI = DPI.DPI72;
            this.DPI96Button.Tapped += (s, e) => this.DPI = DPI.DPI96;
            this.DPI144Button.Tapped += (s, e) => this.DPI = DPI.DPI144;
            this.DPI192Button.Tapped += (s, e) => this.DPI = DPI.DPI192;
            this.DPI300Button.Tapped += (s, e) => this.DPI = DPI.DPI300;
            this.DPI400Button.Tapped += (s, e) => this.DPI = DPI.DPI400;
        }

    }
}