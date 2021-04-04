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
    /// ComboBox of Transparency<see cref="BrushType"/>
    /// </summary>
    public sealed partial class TransparencyTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when type change. </summary>
        public EventHandler<BrushType> TypeChanged;

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
                    case BrushType.LinearGradient: return this.LinearGradientState;
                    case BrushType.RadialGradient: return this.RadialGradientState;
                    case BrushType.EllipticalGradient: return this.EllipticalGradientState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets the transparency. </summary>
        public IBrush Transparency
        {
            get => (IBrush)base.GetValue(TransparencyProperty);
            set => base.SetValue(TransparencyProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Fill" /> dependency property. </summary>
        public static readonly DependencyProperty TransparencyProperty = DependencyProperty.Register(nameof(Transparency), typeof(IBrush), typeof(TransparencyTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            TransparencyTypeComboBox control = (TransparencyTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                control._vsType = value.Type;
                control.VisualState = control.VisualState;//State
            }
            else
            {
                control._vsType = BrushType.None;
                control.VisualState = control.VisualState;//State
            }
        }));

        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TransparencyTypeComboBox. 
        /// </summary>
        public TransparencyTypeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.None.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, BrushType.None);//Delegate
                this.Flyout.Hide();
            };
            this.LinearGradient.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, BrushType.LinearGradient);//Delegate
                this.Flyout.Hide();
            };
            this.RadialGradient.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, BrushType.RadialGradient);//Delegate
                this.Flyout.Hide();
            };
            this.EllipticalGradient.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, BrushType.EllipticalGradient);//Delegate
                this.Flyout.Hide();
            };

            this.Button.Click += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.None.Content = resource.GetString($"Tools_Brush_Type_None");
            this.LinearGradient.Content = resource.GetString($"Tools_Brush_Type_LinearGradient");
            this.RadialGradient.Content = resource.GetString($"Tools_Brush_Type_RadialGradient");
            this.EllipticalGradient.Content = resource.GetString($"Tools_Brush_Type_EllipticalGradient");
        }
    }
}