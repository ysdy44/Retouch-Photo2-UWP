// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// ComboBox of <see cref="CanvasEdgeBehavior"/>.
    /// </summary>
    public sealed partial class ExtendComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when extend change. </summary>
        public EventHandler<CanvasEdgeBehavior> ExtendChanged;

        //@VisualState
        CanvasEdgeBehavior _vsExtend;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsExtend)
                {
                    case CanvasEdgeBehavior.Clamp: return this.ClampState;
                    case CanvasEdgeBehavior.Wrap: return this.WrapState;
                    case CanvasEdgeBehavior.Mirror: return this.MirrorState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets the edge behavior. </summary>
        public CanvasEdgeBehavior Extend
        {
            get => (CanvasEdgeBehavior)base.GetValue(ExtendProperty);
            set => base.SetValue(ExtendProperty, value);
        }
        /// <summary> Identifies the <see cref = "ExtendComboBox.Extend" /> dependency property. </summary>
        public static readonly DependencyProperty ExtendProperty = DependencyProperty.Register(nameof(Extend), typeof(CanvasEdgeBehavior), typeof(ExtendComboBox), new PropertyMetadata(CanvasEdgeBehavior.Clamp, (sender, e) =>
        {
            ExtendComboBox control = (ExtendComboBox)sender;

            if (e.NewValue is CanvasEdgeBehavior value)
            {
                control._vsExtend = value;
                control.VisualState = control.VisualState;//State
            }
        }));

        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get => this.fillOrStroke;
            set
            {
                switch (value)
                {
                    case FillOrStroke.Fill: this.Extend = this.Fill?.Extend ?? CanvasEdgeBehavior.Clamp; break;
                    case FillOrStroke.Stroke: this.Extend = this.Stroke?.Extend ?? CanvasEdgeBehavior.Clamp; break;
                }

                this.fillOrStroke = value;
            }
        }
        private FillOrStroke fillOrStroke;

        /// <summary> Gets or sets the fill. </summary>
        public IBrush Fill
        {
            get => this.fill;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.Extend = value?.Extend ?? CanvasEdgeBehavior.Clamp;
                        break;
                }
                this.fill = value;
            }
        }
        private IBrush fill;

        /// <summary> Gets or sets the stroke. </summary>
        public IBrush Stroke
        {
            get => this.stroke;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        this.Extend = value?.Extend ?? CanvasEdgeBehavior.Clamp;
                        break;
                }
                this.stroke = value;
            }
        }
        private IBrush stroke;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ExtendComboBox. 
        /// </summary>
        public ExtendComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ClampItem.Tapped += (s, e) =>
            {
                this.ExtendChanged?.Invoke(this, CanvasEdgeBehavior.Clamp);//Delegate
                this.Flyout.Hide();
            };
            this.WrapItem.Tapped += (s, e) =>
            {
                this.ExtendChanged?.Invoke(this, CanvasEdgeBehavior.Wrap);//Delegate
                this.Flyout.Hide();
            };
            this.MirrorItem.Tapped += (s, e) =>
            {
                this.ExtendChanged?.Invoke(this, CanvasEdgeBehavior.Mirror);//Delegate
                this.Flyout.Hide();
            };

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Clamp.Content = resource.GetString($"Tools_Brush_Extend_Clamp");
            this.Wrap.Content = resource.GetString($"Tools_Brush_Extend_Wrap");
            this.Mirror.Content = resource.GetString($"Tools_Brush_Extend_Mirror");
        }
    }
}