using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Elements.PatternGridTypeIcons;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Represents the contorl that is used to select grid, horizontal or vertical.
    /// </summary>
    public sealed partial class PatternGridTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when type change. </summary>
        public EventHandler<PatternGridType> TypeChanged;


        #region DependencyProperty


        /// <summary> Gets or sets the grid, horizontal or vertical. </summary>
        public PatternGridType GridType
        {
            get => (PatternGridType)base.GetValue(GridTypeProperty);
            set => base.SetValue(GridTypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "PatternGridTypeComboBox.GridType" /> dependency property. </summary>
        public static readonly DependencyProperty GridTypeProperty = DependencyProperty.Register(nameof(GridType), typeof(PatternGridType), typeof(PatternGridTypeComboBox), new PropertyMetadata(PatternGridType.Grid, (sender, e) =>
        {
            PatternGridTypeComboBox control = (PatternGridTypeComboBox)sender;

            if (e.NewValue is PatternGridType value)
            {
                control._vsType = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        #endregion


        //@VisualState
        PatternGridType _vsType;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsType)
                {
                    case PatternGridType.Grid: return this.Grid;
                    case PatternGridType.Horizontal: return this.Horizontal;
                    case PatternGridType.Vertical: return this.Vertical;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        /// <summary>
        /// Initializes a PatternGridTypeComboBox. 
        /// </summary>
        public PatternGridTypeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Click += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.GridButton.Content = resource.GetString("/ToolElements/PatternGrid_Grid");
            this.GridButton.Tag = new GridIcon();
            this.GridButton.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, PatternGridType.Grid); //Delegate
                this.Flyout.Hide();
            };

            this.HorizontalButton.Content = resource.GetString("/ToolElements/PatternHorizontal_Horizontal");
            this.HorizontalButton.Tag = new HorizontalIcon();
            this.HorizontalButton.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, PatternGridType.Horizontal); //Delegate
                this.Flyout.Hide();
            };

            this.VerticalButton.Content = resource.GetString("/ToolElements/PatternVertical_Vertical");
            this.VerticalButton.Tag = new VerticalIcon();
            this.VerticalButton.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, PatternGridType.Vertical); //Delegate
                this.Flyout.Hide();
            };
        }

    }
}
