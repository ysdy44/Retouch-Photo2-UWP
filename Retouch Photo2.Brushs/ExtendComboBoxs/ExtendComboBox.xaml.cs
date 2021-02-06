// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs.ExtendIcons;
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

        //@Group
        /// <summary> Occurs when group change. </summary>
        private EventHandler<CanvasEdgeBehavior> Group;
        

        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            set
            {
                this._vsFillOrStroke = value;
                this.Invalidate();//Invalidate
            }
        }

        /// <summary> Gets or sets the fill. </summary>
        public IBrush Fill
        {
            set
            {
                this._vsFill = value;
                this.Invalidate();//Invalidate
            }
        }

        /// <summary> Gets or sets the stroke. </summary>
        public IBrush Stroke
        {
            set
            {
                this._vsStroke = value;
                this.Invalidate();//Invalidate
            }
        }

        /// <summary> Gets or sets the edge behavior. </summary>
        public CanvasEdgeBehavior Extend
        {
            get  => (CanvasEdgeBehavior)base.GetValue(ExtendProperty);
            set => base.SetValue(ExtendProperty, value);
        }
        /// <summary> Identifies the <see cref = "ExtendComboBox.Extend" /> dependency property. </summary>
        public static readonly DependencyProperty ExtendProperty = DependencyProperty.Register(nameof(Extend), typeof(CanvasEdgeBehavior), typeof(ExtendComboBox), new PropertyMetadata(CanvasEdgeBehavior.Clamp, (sender, e) =>
        {
            ExtendComboBox control = (ExtendComboBox)sender;

            if (e.NewValue is CanvasEdgeBehavior value)
            {
                control.Group?.Invoke(control, value);
            }
        }));


        #endregion

        //@VisualState
        FillOrStroke _vsFillOrStroke;
        IBrush _vsFill;
        IBrush _vsStroke;
        /// <summary>
        /// Invalidate. 
        /// </summary>
        public void Invalidate()
        {
            switch (this._vsFillOrStroke)
            {
                case FillOrStroke.Fill:
                    if (this._vsFill != null) this.Extend = this._vsFill.Extend;
                    break;
                case FillOrStroke.Stroke:
                    if (this._vsStroke != null) this.Extend = this._vsStroke.Extend;
                    break;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a ExtendComboBox. 
        /// </summary>
        public ExtendComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Click += (s, e) => this.Flyout.ShowAt(this);
        }

    }

    /// <summary>
    /// Represents the combo box that is used to select CanvasEdgeBehavior.
    /// </summary>
    public sealed partial class ExtendComboBox : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructGroup(this.ClampButton, resource.GetString("Tools_Brush_Extend_Clamp"), new ClampIcon(), CanvasEdgeBehavior.Clamp);
            this.ConstructGroup(this.WrapButton, resource.GetString("Tools_Brush_Extend_Wrap"), new WrapIcon(), CanvasEdgeBehavior.Wrap);
            this.ConstructGroup(this.MirrorButton, resource.GetString("Tools_Brush_Extend_Mirror"), new MirrorIcon(), CanvasEdgeBehavior.Mirror);
        }
        
        //Group
        private void ConstructGroup(Button button, string text, UserControl icon, CanvasEdgeBehavior behavior)
        {
            void group(CanvasEdgeBehavior groupCanvasEdgeBehavior)
            {
                if (groupCanvasEdgeBehavior == behavior)
                {
                    button.IsEnabled = false;

                    this.Button.Content = text;
                }
                else button.IsEnabled = true;
            }

            //NoneButton
            group(this.Extend);

            //Buttons
            button.Content = text;
            button.Tag = icon;
            button.Click += (s, e) =>
            {
                this.ExtendChanged?.Invoke(this, behavior); //Delegate
                this.Flyout.Hide();
            };

            //Group
            this.Group += (s, e) => group(e);
        }

    }
}