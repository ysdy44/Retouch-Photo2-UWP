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
        /// <summary> Occurs when dash change. </summary>
        public EventHandler<CanvasDashStyle> DashChanged;

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
                    case CanvasDashStyle.Solid: return this.Solid;
                    case CanvasDashStyle.Dash: return this.Dash2;
                    case CanvasDashStyle.Dot: return this.Dot;
                    case CanvasDashStyle.DashDot: return this.DashDot;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Dash style of <see cref = "DashSegmented" />. </summary>
        public CanvasDashStyle Dash
        {
            get  => (CanvasDashStyle)base.GetValue(DashProperty);
            set => base.SetValue(DashProperty, value);
        }
        /// <summary> Identifies the <see cref = "DashSegmented.Dash" /> dependency property. </summary>
        public static readonly DependencyProperty DashProperty = DependencyProperty.Register(nameof(Dash), typeof(CanvasDashStyle), typeof(DashSegmented), new PropertyMetadata(CanvasDashStyle.Solid, (sender, e) =>
        {
            DashSegmented control = (DashSegmented)sender;

            if (e.NewValue is CanvasDashStyle value)
            {
                control._vsDash = value;
                control.VisualState = control.VisualState;//State
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
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.SolidToolTip.Content = resource.GetString("/Strokes/Dash_Solid");
            this.SolidButton.Click += (s, e) =>
            {
                this.DashChanged?.Invoke(this, CanvasDashStyle.Solid);//Delegate
            };

            this.DashToolTip.Content = resource.GetString("/Strokes/Dash_Dash");
            this.DashButton.Click += (s, e) =>
            {
                this.DashChanged?.Invoke(this, CanvasDashStyle.Dash);//Delegate
            };

            this.DotToolTip.Content = resource.GetString("/Strokes/Dash_Dot");
            this.DotButton.Click += (s, e) =>
            {
                this.DashChanged?.Invoke(this, CanvasDashStyle.Dot);//Delegate
            };

            this.DashDotToolTip.Content = resource.GetString("/Strokes/Dash_DashDot");
            this.DashDotButton.Click += (s, e) =>
            {
                this.DashChanged?.Invoke(this, CanvasDashStyle.DashDot);//Delegate
            };
        }

    }
}