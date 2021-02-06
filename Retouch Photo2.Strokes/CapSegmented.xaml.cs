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
                    case CanvasCapStyle.Flat: return this.Flat;
                    case CanvasCapStyle.Square: return this.Square;
                    case CanvasCapStyle.Round: return this.Round;
                    case CanvasCapStyle.Triangle: return this.Triangle;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Cap of <see cref = "CapSegmented" />. </summary>
        public CanvasCapStyle Cap
        {
            get  => (CanvasCapStyle)base.GetValue(CapProperty);
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
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FlatToolTip.Content = resource.GetString("Strokes_Cap_Flat");
            this.FlatButton.Click += (s, e) =>
            {
                this.CapChanged?.Invoke(this, CanvasCapStyle.Flat);//Delegate
            };

            this.SquareToolTip.Content = resource.GetString("Strokes_Cap_Square");
            this.SquareButton.Click += (s, e) =>
            {
                this.CapChanged?.Invoke(this, CanvasCapStyle.Square);//Delegate
            };

            this.RoundToolTip.Content = resource.GetString("Strokes_Cap_Round");
            this.RoundButton.Click += (s, e) =>
            {
                this.CapChanged?.Invoke(this, CanvasCapStyle.Round);//Delegate
            };

            this.TriangleToolTip.Content = resource.GetString("Strokes_Cap_Triangle");
            this.TriangleButton.Click += (s, e) =>
            {
                this.CapChanged?.Invoke(this, CanvasCapStyle.Triangle);//Delegate
            };
        }

    }
}