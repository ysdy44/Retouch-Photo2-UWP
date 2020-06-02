using Microsoft.Graphics.Canvas;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Dots-per-inch (DPI).
    /// </summary>
    public enum DPI
    {
        DPI72 = 72,
        DPI96 = 96,
        DPI144 = 144,
        DPI192 = 192,
        DPI300 = 300,
        DPI400 = 400,
    }

    /// <summary>
    /// ComboBox of <see cref="Retouch_Photo2.Elements.DPI"/>.
    /// </summary>
    public sealed partial class DPIComboBox : UserControl
    {

        //@VisualState
        DPI _vsDPI;
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

        #region DependencyProperty


        /// <summary> Gets or sets the dots-per-inch (DPI). </summary>
        public DPI DPI
        {
            get { return (DPI)GetValue(DPIProperty); }
            set { SetValue(DPIProperty, value); }
        }
        /// <summary> Identifies the <see cref = "DPIComboBox.DPI" /> dependency property. </summary>
        public static readonly DependencyProperty DPIProperty = DependencyProperty.Register(nameof(DPI), typeof(DPI), typeof(DPIComboBox), new PropertyMetadata(DPI.DPI144, (sender, e) =>
        {
            DPIComboBox con = (DPIComboBox)sender;

            if (e.NewValue is DPI value)
            {
                con._vsDPI = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion

        
        //@Construct
        public DPIComboBox()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.DPI72Button.Click += (s, e) => this.DPI = DPI.DPI72;
            this.DPI96Button.Click += (s, e) => this.DPI = DPI.DPI96;
            this.DPI144Button.Click += (s, e) => this.DPI = DPI.DPI144;
            this.DPI192Button.Click += (s, e) => this.DPI = DPI.DPI192;
            this.DPI300Button.Click += (s, e) => this.DPI = DPI.DPI300;
        }

    }
}