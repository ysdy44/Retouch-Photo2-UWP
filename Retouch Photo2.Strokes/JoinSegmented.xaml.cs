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
    /// Represents the segmented that is used to select line join.
    /// </summary>
    public sealed partial class JoinSegmented : UserControl
    {

        //@Delegate
        /// <summary> Occurs when join change. </summary>
        public EventHandler<CanvasLineJoin> JoinChanged;

        //@VisualState
        CanvasLineJoin _vsJoin;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsJoin)
                {
                    case CanvasLineJoin.Miter: return this.MiterState;
                    case CanvasLineJoin.Bevel: return this.BevelState;
                    case CanvasLineJoin.Round: return this.RoundState;
                    case CanvasLineJoin.MiterOrBevel: return this.MiterOrBevelState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or set join of <see cref = "JoinSegmented" />. </summary>
        public CanvasLineJoin Join
        {
            get => (CanvasLineJoin)base.GetValue(JoinProperty);
            set => base.SetValue(JoinProperty, value);
        }
        /// <summary> Identifies the <see cref = "JoinSegmented.Join" /> dependency property. </summary>
        public static readonly DependencyProperty JoinProperty = DependencyProperty.Register(nameof(Join), typeof(CanvasLineJoin), typeof(JoinSegmented), new PropertyMetadata(CanvasLineJoin.Miter, (sender, e) =>
        {
            JoinSegmented control = (JoinSegmented)sender;

            if (e.NewValue is CanvasLineJoin value)
            {
                control._vsJoin = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        /// <summary> IsOpen of <see cref = "JoinSegmented" />. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "JoinSegmented.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(JoinSegmented), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a JoinSegmented. 
        /// </summary>
        public JoinSegmented()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.Miter.Click += (s, e) => this.JoinChanged?.Invoke(this, CanvasLineJoin.Miter);//Delegate
            this.Bevel.Click += (s, e) => this.JoinChanged?.Invoke(this, CanvasLineJoin.Bevel);//Delegate
            this.Round.Click += (s, e) => this.JoinChanged?.Invoke(this, CanvasLineJoin.Round);//Delegate
            this.MiterOrBevel.Click += (s, e) => this.JoinChanged?.Invoke(this, CanvasLineJoin.MiterOrBevel);//Delegate

            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            if (ToolTipService.GetToolTip(this.Miter) is ToolTip toolTip0)
            {
                toolTip0.Content = resource.GetString($"Strokes_Join_Miter");
            }
            if (ToolTipService.GetToolTip(this.Bevel) is ToolTip toolTip1)
            {
                toolTip1.Content = resource.GetString($"Strokes_Join_Bevel");
            }
            if (ToolTipService.GetToolTip(this.Round) is ToolTip toolTip2)
            {
                toolTip2.Content = resource.GetString($"Strokes_Join_Round");
            }
            if (ToolTipService.GetToolTip(this.MiterOrBevel) is ToolTip toolTip3)
            {
                toolTip3.Content = resource.GetString($"Strokes_Join_MiterOrBevel");
            }
        }
    }
}