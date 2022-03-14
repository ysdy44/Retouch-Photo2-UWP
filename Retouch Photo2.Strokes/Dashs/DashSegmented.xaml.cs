// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas.Geometry;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Strokes
{
    /// <summary>
    /// Represents the segmented that is used to select dash style.
    /// </summary>
    public sealed partial class DashSegmented : UserControl
    {

        //@Delegate
        /// <summary> Occurs when dash changed. </summary>
        public event EventHandler<CanvasDashStyle> DashChanged;

        //@VisualState
        CanvasDashStyle _vsDash;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsDash)
                {
                    case CanvasDashStyle.Solid: return this.SolidState;
                    case CanvasDashStyle.Dash: return this.DashState;
                    case CanvasDashStyle.Dot: return this.DotState;
                    case CanvasDashStyle.DashDot: return this.DashDotState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or set ash style of <see cref = "DashSegmented" />. </summary>
        public CanvasDashStyle Dash
        {
            get => (CanvasDashStyle)base.GetValue(DashProperty);
            set => base.SetValue(DashProperty, value);
        }
        /// <summary> Identifies the <see cref = "DashSegmented.Dash" /> dependency property. </summary>
        public static readonly DependencyProperty DashProperty = DependencyProperty.Register(nameof(Dash), typeof(CanvasDashStyle), typeof(DashSegmented), new PropertyMetadata(CanvasDashStyle.Solid, (sender, e) =>
        {
            DashSegmented control = (DashSegmented)sender;

            if (e.NewValue is CanvasDashStyle value)
            {
                control._vsDash = value;
                control.VisualState = control.VisualState; // State
            }
        }));


        /// <summary> IsOpen of <see cref = "DashSegmented" />. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "DashSegmented.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(DashSegmented), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a DashSegmented. 
        /// </summary>
        public DashSegmented()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.Solid.Tapped += (s, e) => this.DashChanged?.Invoke(this, CanvasDashStyle.Solid); // Delegate
            this.Dash2.Tapped += (s, e) => this.DashChanged?.Invoke(this, CanvasDashStyle.Dash); // Delegate
            this.Dot.Tapped += (s, e) => this.DashChanged?.Invoke(this, CanvasDashStyle.Dot); // Delegate
            this.DashDot.Tapped += (s, e) => this.DashChanged?.Invoke(this, CanvasDashStyle.DashDot); // Delegate

            this.Loaded += (s, e) => this.VisualState = this.VisualState; // State
        }


        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            if (ToolTipService.GetToolTip(this.Solid) is ToolTip toolTip0)
            {
                toolTip0.Content = resource.GetString($"Strokes_Dash_Solid");
            }
            if (ToolTipService.GetToolTip(this.Dash2) is ToolTip toolTip1)
            {
                toolTip1.Content = resource.GetString($"Strokes_Dash_Dash");
            }
            if (ToolTipService.GetToolTip(this.Dot) is ToolTip toolTip2)
            {
                toolTip2.Content = resource.GetString($"Strokes_Dash_Dot");
            }
            if (ToolTipService.GetToolTip(this.DashDot) is ToolTip toolTip3)
            {
                toolTip3.Content = resource.GetString($"Strokes_Dash_DashDot");
            }
        }
    }
}