using Retouch_Photo2.Layers.Models;
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
        public event EventHandler<PatternGridType> TypeChanged;

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
                    case PatternGridType.Grid: return this.GridState;
                    case PatternGridType.Horizontal: return this.HorizontalState;
                    case PatternGridType.Vertical: return this.VerticalState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

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

        //@Construct
        /// <summary>
        /// Initializes a PatternGridTypeComboBox. 
        /// </summary>
        public PatternGridTypeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.GridItem.Tapped += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, PatternGridType.Grid); //Delegate
                this.Flyout.Hide();
            };
            this.HorizontalItem.Tapped += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, PatternGridType.Horizontal); //Delegate
                this.Flyout.Hide();
            };
            this.VerticalItem.Tapped += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, PatternGridType.Vertical); //Delegate
                this.Flyout.Hide();
            };

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Grid.Content = resource.GetString($"Tools_PatternGrid_Grid");
            this.Horizontal.Content = resource.GetString($"Tools_PatternGrid_Horizontal");
            this.Vertical.Content = resource.GetString($"Tools_PatternGrid_Vertical");
        }
    }
}