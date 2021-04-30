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
    /// ComboBox OF <see cref="Retouch_Photo2.Brushs.FillOrStroke"/>.
    /// </summary>
    public sealed partial class FillOrStrokeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when fill or stroke change. </summary>
        public event EventHandler<FillOrStroke> FillOrStrokeChanged;

        //@VisualState
        FillOrStroke _vsFillOrStroke;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsFillOrStroke)
                {
                    case FillOrStroke.Fill: return this.FillState;
                    case FillOrStroke.Stroke: return this.StrokeState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get => (FillOrStroke)base.GetValue(FillOrStrokeProperty);
            set => base.SetValue(FillOrStrokeProperty, value);
        }
        /// <summary> Identifies the <see cref = "FillOrStrokeComboBox.FillOrStroke" /> dependency property. </summary>
        public static readonly DependencyProperty FillOrStrokeProperty = DependencyProperty.Register(nameof(FillOrStroke), typeof(FillOrStroke), typeof(FillOrStrokeComboBox), new PropertyMetadata(FillOrStroke.Fill, (sender, e) =>
        {
            FillOrStrokeComboBox control = (FillOrStrokeComboBox)sender;

            if (e.NewValue is FillOrStroke value)
            {
                control._vsFillOrStroke = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a FillOrStrokeComboBox. 
        /// </summary>
        public FillOrStrokeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.FillItem.Tapped += (s, e) =>
            {
                this.FillOrStrokeChanged?.Invoke(this, FillOrStroke.Fill);//Delegate
                this.Flyout.Hide();
            };
            this.StrokeItem.Tapped += (s, e) =>
            {
                this.FillOrStrokeChanged?.Invoke(this, FillOrStroke.Stroke);//Delegate
                this.Flyout.Hide();
            };

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Fill.Content = resource.GetString($"Tools_Fill");
            this.Stroke.Content = resource.GetString($"Tools_Stroke");
        }
    }
}