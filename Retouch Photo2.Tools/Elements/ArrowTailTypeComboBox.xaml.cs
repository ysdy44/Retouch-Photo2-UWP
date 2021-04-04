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
        public EventHandler<GeometryArrowTailType> TypeChanged;

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
                    case GeometryArrowTailType.None: return this.None;
                    case GeometryArrowTailType.Arrow: return this.Arrow;
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

            this.NoneButton.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, GeometryArrowTailType.None); //Delegate
                this.Flyout.Hide();
            };
            this.ArrowButton.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, GeometryArrowTailType.Arrow); //Delegate
                this.Flyout.Hide();
            };

            this.Button.Click += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.NoneButton.Content = resource.GetString($"Tools_GeometryArrow_ArrowTail_None");
            this.ArrowButton.Content = resource.GetString($"Tools_GeometryArrow_ArrowTail_Arrow");
        }
    }
}
