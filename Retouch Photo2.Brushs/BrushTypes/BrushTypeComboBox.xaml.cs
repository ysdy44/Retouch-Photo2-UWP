// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// ComboBox of <see cref="BrushType"/>
    /// </summary>
    public sealed partial class BrushTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when fill type change. </summary>
        public EventHandler<BrushType> FillTypeChanged;
        /// <summary> Occurs when stroke type change. </summary>
        public EventHandler<BrushType> StrokeTypeChanged;

        //@VisualState
        BrushType _vsType;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsType)
                {
                    case BrushType.None: return this.NoneState;
                    case BrushType.Color: return this.ColorState;
                    case BrushType.LinearGradient: return this.LinearGradientState;
                    case BrushType.RadialGradient: return this.RadialGradientState;
                    case BrushType.EllipticalGradient: return this.EllipticalGradientState;
                    case BrushType.Image: return this.ImageState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get => this.fillOrStroke;
            set
            {
                switch (value)
                {
                    case FillOrStroke.Fill:
                        this._vsType = this.FillType;
                        this.VisualState = this.VisualState;//State
                        break;
                    case FillOrStroke.Stroke:
                        this._vsType = this.StrokeType;
                        this.VisualState = this.VisualState;//State
                        break;
                }
                this.fillOrStroke = value;
            }
        }
        private FillOrStroke fillOrStroke = FillOrStroke.Fill;


        /// <summary> Gets or sets the fill. </summary>
        public IBrush Fill
        {
            get => (IBrush)base.GetValue(FillProperty);
            set => base.SetValue(FillProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Fill" /> dependency property. </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(IBrush), typeof(BrushTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BrushTypeComboBox control = (BrushTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                control.FillType = value.Type;
            }
            else
            {
                control.FillType = BrushType.None;
            }
        }));

        /// <summary> Gets or sets the fill type. </summary>
        public BrushType FillType
        {
            get => this.fillType;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this._vsType = value;
                        this.VisualState = this.VisualState;//State
                        break;
                }
                this.fillType = value;
            }
        }
        private BrushType fillType = BrushType.None;


        /// <summary> Gets or sets the stroke. </summary>
        public IBrush Stroke
        {
            get => (IBrush)base.GetValue(StrokeProperty);
            set => base.SetValue(StrokeProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Stroke" /> dependency property. </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(IBrush), typeof(BrushTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BrushTypeComboBox control = (BrushTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                control.StrokeType = value.Type;
            }
            else
            {
                control.StrokeType = BrushType.None;
            }
        }));

        /// <summary> Gets or sets the stroke type. </summary>
        public BrushType StrokeType
        {
            get => this.strokeType;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        this._vsType = value;
                        this.VisualState = this.VisualState;//State
                        break;
                }
                this.strokeType = value;
            }
        }
        private BrushType strokeType = BrushType.None;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a BrushTypeComboBox. 
        /// </summary>
        public BrushTypeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.NoneItem.Tapped += (s, e) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillTypeChanged?.Invoke(this, BrushType.None);//Delegate
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeTypeChanged?.Invoke(this, BrushType.None);//Delegate
                        break;
                }
                this.Flyout.Hide();
            };
            this.ColorItem.Tapped += (s, e) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillTypeChanged?.Invoke(this, BrushType.Color);//Delegate
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeTypeChanged?.Invoke(this, BrushType.Color);//Delegate
                        break;
                }
                this.Flyout.Hide();
            };
            this.LinearGradientItem.Tapped += (s, e) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillTypeChanged?.Invoke(this, BrushType.LinearGradient);//Delegate
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeTypeChanged?.Invoke(this, BrushType.LinearGradient);//Delegate
                        break;
                }
                this.Flyout.Hide();
            };
            this.RadialGradientItem.Tapped += (s, e) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillTypeChanged?.Invoke(this, BrushType.RadialGradient);//Delegate
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeTypeChanged?.Invoke(this, BrushType.RadialGradient);//Delegate
                        break;
                }
                this.Flyout.Hide();
            };
            this.EllipticalGradientItem.Tapped += (s, e) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillTypeChanged?.Invoke(this, BrushType.EllipticalGradient);//Delegate
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeTypeChanged?.Invoke(this, BrushType.EllipticalGradient);//Delegate
                        break;
                }
                this.Flyout.Hide();
            };
            this.ImageItem.Tapped += (s, e) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillTypeChanged?.Invoke(this, BrushType.Image);//Delegate
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeTypeChanged?.Invoke(this, BrushType.Image);//Delegate
                        break;
                }
                this.Flyout.Hide();
            };

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.None.Content = resource.GetString($"Tools_Brush_Type_None");
            this.Color.Content = resource.GetString($"Tools_Brush_Type_Color");
            this.LinearGradient.Content = resource.GetString($"Tools_Brush_Type_LinearGradient");
            this.RadialGradient.Content = resource.GetString($"Tools_Brush_Type_RadialGradient");
            this.EllipticalGradient.Content = resource.GetString($"Tools_Brush_Type_EllipticalGradient");
            this.Image.Content = resource.GetString($"Tools_Brush_Type_Image");
        }
    }
}