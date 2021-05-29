// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas.Text;
using System;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Texts
{
    /// <summary>
    /// Segmented of <see cref="CanvasHorizontalAlignment"/>.
    /// </summary>
    public sealed partial class HorizontalAlignmentSegmented : UserControl
    {

        //@Delegate
        /// <summary> Occurs when horizontal alignment changed. </summary>
        public event EventHandler<CanvasHorizontalAlignment> HorizontalAlignmentChanged;

        //@VisualState
        CanvasHorizontalAlignment _vsHorizontalAlignment;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsHorizontalAlignment)
                {
                    case CanvasHorizontalAlignment.Left: return this.LeftState;
                    case CanvasHorizontalAlignment.Center: return this.CenterState;
                    case CanvasHorizontalAlignment.Right: return this.RightState;
                    case CanvasHorizontalAlignment.Justified: return this.JustifiedState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets the horizontal alignment. </summary>
        public CanvasHorizontalAlignment HorizontalAlignment2
        {
            get => (CanvasHorizontalAlignment)base.GetValue(HorizontalAlignment2Property);
            set => base.SetValue(HorizontalAlignment2Property, value);
        }
        /// <summary> Identifies the <see cref = "HorizontalAlignmentSegmented.HorizontalAlignment" /> dependency property. </summary>
        public static readonly DependencyProperty HorizontalAlignment2Property = DependencyProperty.Register(nameof(HorizontalAlignment2), typeof(CanvasHorizontalAlignment), typeof(HorizontalAlignmentSegmented), new PropertyMetadata(CanvasHorizontalAlignment.Left, (sender, e) =>
        {
            HorizontalAlignmentSegmented control = (HorizontalAlignmentSegmented)sender;

            if (e.NewValue is CanvasHorizontalAlignment value)
            {
                control._vsHorizontalAlignment = value;
                control.VisualState = control.VisualState; // State
            }
        }));


        /// <summary> Gets or sets the IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "HorizontalAlignmentSegmented.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(HorizontalAlignmentSegmented), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a HorizontalAlignmentSegmented. 
        /// </summary>
        public HorizontalAlignmentSegmented()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            base.Loaded += (s, e) => this.ConstructLanguages();

            this.Left.Tapped += (s, e) => this.HorizontalAlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Left); // Delegate
            this.Center.Tapped += (s, e) => this.HorizontalAlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Center); // Delegate
            this.Right.Tapped += (s, e) => this.HorizontalAlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Right); // Delegate
            this.Justified.Tapped += (s, e) => this.HorizontalAlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Justified); // Delegate

            this.Loaded += (s, e) => this.VisualState = this.VisualState; // State
        }


        // Languages
        private void ConstructLanguages()
        {
            if (string.IsNullOrEmpty(ApplicationLanguages.PrimaryLanguageOverride) == false)
            {
                if (ApplicationLanguages.PrimaryLanguageOverride != base.Language)
                {
                    this.ConstructStrings();
                }
            }
        }

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            if (ToolTipService.GetToolTip(this.Left) is ToolTip toolTip0)
            {
                toolTip0.Content = resource.GetString($"Texts_HorizontalAlignment_Left");
            }
            if (ToolTipService.GetToolTip(this.Center) is ToolTip toolTip1)
            {
                toolTip1.Content = resource.GetString($"Texts_HorizontalAlignment_Center");
            }
            if (ToolTipService.GetToolTip(this.Right) is ToolTip toolTip2)
            {
                toolTip2.Content = resource.GetString($"Texts_HorizontalAlignment_Right");
            }
            if (ToolTipService.GetToolTip(this.Justified) is ToolTip toolTip3)
            {
                toolTip3.Content = resource.GetString($"Texts_HorizontalAlignment_Justified");
            }
        }
    }
}