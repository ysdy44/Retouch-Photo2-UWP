using FanKit.Transformers;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Represents the contorl that is used to select none or arrow.
    /// </summary>
    public sealed partial class ArrowTailTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when type change. </summary>
        public event EventHandler<GeometryArrowTailType> TypeChanged;
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.Flyout.Closed += value; remove => this.Flyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.Flyout.Opened += value; remove => this.Flyout.Opened -= value; }


        //@VisualState
        GeometryArrowTailType _vsType;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsType)
                {
                    case GeometryArrowTailType.None: return this.NoneState;
                    case GeometryArrowTailType.Arrow: return this.ArrowState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        #region DependencyProperty


        /// <summary> Gets or sets the none or arrow. </summary>
        public GeometryArrowTailType ArrowTailType
        {
            get => (GeometryArrowTailType)base.GetValue(ArrowTailTypeProperty);
            set => base.SetValue(ArrowTailTypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "ArrowTailTypeComboBox.ArrowTailType" /> dependency property. </summary>
        public static readonly DependencyProperty ArrowTailTypeProperty = DependencyProperty.Register(nameof(ArrowTailType), typeof(GeometryArrowTailType), typeof(ArrowTailTypeComboBox), new PropertyMetadata(GeometryArrowTailType.None, (sender, e) =>
        {
            ArrowTailTypeComboBox control = (ArrowTailTypeComboBox)sender;

            if (e.NewValue is GeometryArrowTailType value)
            {
                control._vsType = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ArrowTailTypeComboBox. 
        /// </summary>
        public ArrowTailTypeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.NoneItem.Tapped += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, GeometryArrowTailType.None); //Delegate
                this.Flyout.Hide();
            };
            this.ArrowItem.Tapped += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, GeometryArrowTailType.Arrow); //Delegate
                this.Flyout.Hide();
            };

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.None.Content = resource.GetString($"Tools_GeometryArrow_ArrowTail_None");
            this.Arrow.Content = resource.GetString($"Tools_GeometryArrow_ArrowTail_Arrow");
        }
    }
}
