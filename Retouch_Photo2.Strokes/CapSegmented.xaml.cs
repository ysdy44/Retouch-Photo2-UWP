using Microsoft.Graphics.Canvas.Geometry;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Strokes
{
    /// <summary>
    /// Represents the segmented that is used to select cap style.
    /// </summary>
    public sealed partial class CapSegmented : UserControl
    {

        //@Delegate
        public EventHandler<CanvasCapStyle> CapChanged;

        //@VisualState
        CanvasCapStyle _vsCap;
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
            get { return (CanvasCapStyle)GetValue(CapProperty); }
            set { SetValue(CapProperty, value); }
        }
        /// <summary> Identifies the <see cref = "CapSegmented.Cap" /> dependency property. </summary>
        public static readonly DependencyProperty CapProperty = DependencyProperty.Register(nameof(Cap), typeof(CanvasCapStyle), typeof(CapSegmented), new PropertyMetadata(CanvasCapStyle.Flat, (sender, e) =>
        {
            CapSegmented con = (CapSegmented)sender;

            if (e.NewValue is CanvasCapStyle value)
            {
                con._vsCap = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        /// <summary> IsOpen of <see cref = "CapSegmented" />. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "CapSegmented.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(CapSegmented), new PropertyMetadata(false));


        #endregion


        //@Construct
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

            this.FlatToolTip.Content = resource.GetString("/Strokes/Cap_Flat");
            this.FlatButton.Tapped += (s, e) =>
            {
                this.CapChanged?.Invoke(this, CanvasCapStyle.Flat);//Delegate
            };

            this.SquareToolTip.Content = resource.GetString("/Strokes/Cap_Square");
            this.SquareButton.Tapped += (s, e) =>
            {
                this.CapChanged?.Invoke(this, CanvasCapStyle.Square);//Delegate
            };

            this.RoundToolTip.Content = resource.GetString("/Strokes/Cap_Round");
            this.RoundButton.Tapped += (s, e) =>
            {
                this.CapChanged?.Invoke(this, CanvasCapStyle.Round);//Delegate
            };

            this.TriangleToolTip.Content = resource.GetString("/Strokes/Cap_Triangle");
            this.TriangleButton.Tapped += (s, e) =>
            {
                this.CapChanged?.Invoke(this, CanvasCapStyle.Triangle);//Delegate
            };
        }

    }
}