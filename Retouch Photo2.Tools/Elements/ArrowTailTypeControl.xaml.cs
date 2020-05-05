using FanKit.Transformers;
using Retouch_Photo2.Tools.Elements.ArrowTailTypeIcons;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Represents the contorl that is used to select none or arrow.
    /// </summary>
    public sealed partial class ArrowTailTypeControl : UserControl
    {

        //@Delegate
        public EventHandler<GeometryArrowTailType> ArrowTailTypeChanged;


        #region DependencyProperty


        /// <summary> Gets or sets the None or Arrow. </summary>
        public GeometryArrowTailType ArrowTailType
        {
            get { return (GeometryArrowTailType)GetValue(ArrowTailTypeProperty); }
            set { SetValue(ArrowTailTypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ArrowTailTypeControl.ArrowTailType" /> dependency property. </summary>
        public static readonly DependencyProperty ArrowTailTypeProperty = DependencyProperty.Register(nameof(ArrowTailType), typeof(GeometryArrowTailType), typeof(ArrowTailTypeControl), new PropertyMetadata(GeometryArrowTailType.None, (sender, e) =>
        {
            ArrowTailTypeControl con = (ArrowTailTypeControl)sender;

            if (e.NewValue is GeometryArrowTailType value)
            {
                con._vsArrowTailType = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion


        //@VisualState
        GeometryArrowTailType _vsArrowTailType;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsArrowTailType)
                {
                    case GeometryArrowTailType.None: return this.None;
                    case GeometryArrowTailType.Arrow: return this.Arrow;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        public ArrowTailTypeControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }

        
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.NoneButton.Content = resource.GetString("/ToolElements/ArrowTail_None");
            this.NoneButton.Tag = new NoneIcon();
            this.NoneButton.Tapped += (s, e) =>
            {
                this.ArrowTailTypeChanged?.Invoke(this, GeometryArrowTailType.None); //Delegate
                this.Flyout.Hide();
            };

            this.ArrowButton.Content = resource.GetString("/ToolElements/ArrowTail_Arrow");
            this.ArrowButton.Tag = new ArrowIcon();
            this.ArrowButton.Tapped += (s, e) =>
            {
                this.ArrowTailTypeChanged?.Invoke(this, GeometryArrowTailType.Arrow); //Delegate
                this.Flyout.Hide();
            };
        }

    }
}
