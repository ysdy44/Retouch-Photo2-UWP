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
                    case CanvasLineJoin.Miter: return this.Miter;
                    case CanvasLineJoin.Bevel: return this.Bevel;
                    case CanvasLineJoin.Round: return this.Round;
                    case CanvasLineJoin.MiterOrBevel: return this.MiterOrBevel;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Join of <see cref = "JoinSegmented" />. </summary>
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
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.MiterToolTip.Content = resource.GetString("/Strokes/Join_Miter");
            this.MiterButton.Click += (s, e) =>
            {
                this.JoinChanged?.Invoke(this, CanvasLineJoin.Miter);//Delegate
            };

            this.BevelToolTip.Content = resource.GetString("/Strokes/Join_Bevel");
            this.BevelButton.Click += (s, e) =>
            {
                this.JoinChanged?.Invoke(this, CanvasLineJoin.Bevel);//Delegate
            };

            this.RoundToolTip.Content = resource.GetString("/Strokes/Join_Round");
            this.RoundButton.Click += (s, e) =>
            {
                this.JoinChanged?.Invoke(this, CanvasLineJoin.Round);//Delegate
            };

            this.MiterOrBevelToolTip.Content = resource.GetString("/Strokes/Join_MiterOrBevel");
            this.MiterOrBevelButton.Click += (s, e) =>
            {
                this.JoinChanged?.Invoke(this, CanvasLineJoin.MiterOrBevel);//Delegate
            };
        }

    }
}