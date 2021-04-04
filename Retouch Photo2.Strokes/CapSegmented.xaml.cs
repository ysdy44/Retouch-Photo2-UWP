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
    /// Segmented of <see cref="CanvasCapStyle"/>.
    /// </summary>
    public sealed partial class CapSegmented : UserControl
    {

        //@Delegate
        /// <summary> Occurs when cap change. </summary>
        public EventHandler<CanvasCapStyle> CapChanged;

        //@VisualState
        CanvasCapStyle _vsCap;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsCap)
                {
                    case CanvasCapStyle.Flat: return this.FlatState;
                    case CanvasCapStyle.Square: return this.SquareState;
                    case CanvasCapStyle.Round: return this.RoundState;
                    case CanvasCapStyle.Triangle: return this.TriangleState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or set cap of <see cref = "CapSegmented" />. </summary>
        public CanvasCapStyle Cap
        {
            get => (CanvasCapStyle)base.GetValue(CapProperty);
            set => base.SetValue(CapProperty, value);
        }
        /// <summary> Identifies the <see cref = "CapSegmented.Cap" /> dependency property. </summary>
        public static readonly DependencyProperty CapProperty = DependencyProperty.Register(nameof(Cap), typeof(CanvasCapStyle), typeof(CapSegmented), new PropertyMetadata(CanvasCapStyle.Flat, (sender, e) =>
        {
            CapSegmented control = (CapSegmented)sender;

            if (e.NewValue is CanvasCapStyle value)
            {
                control._vsCap = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        /// <summary> IsOpen of <see cref = "CapSegmented" />. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "CapSegmented.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(CapSegmented), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a CapSegmented. 
        /// </summary>
        public CapSegmented()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.Flat.Click += (s, e) => this.CapChanged?.Invoke(this, CanvasCapStyle.Flat);//Delegate
            this.Square.Click += (s, e) => this.CapChanged?.Invoke(this, CanvasCapStyle.Square);//Delegate
            this.Round.Click += (s, e) => this.CapChanged?.Invoke(this, CanvasCapStyle.Round);//Delegate
            this.Triangle.Click += (s, e) => this.CapChanged?.Invoke(this, CanvasCapStyle.Triangle);//Delegate

            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            if (ToolTipService.GetToolTip(this.Flat) is ToolTip toolTip0)
            {
                toolTip0.Content = resource.GetString($"Strokes_Cap_Flat");
            }
            if (ToolTipService.GetToolTip(this.Square) is ToolTip toolTip1)
            {
                toolTip1.Content = resource.GetString($"Strokes_Cap_Square");
            }
            if (ToolTipService.GetToolTip(this.Round) is ToolTip toolTip2)
            {
                toolTip2.Content = resource.GetString($"Strokes_Cap_Round");
            }
            if (ToolTipService.GetToolTip(this.Triangle) is ToolTip toolTip3)
            {
                toolTip3.Content = resource.GetString($"Strokes_Cap_Triangle");
            }
        }
    }
}