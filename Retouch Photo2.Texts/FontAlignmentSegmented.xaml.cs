// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas.Text;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Texts
{
    /// <summary>
    /// Segmented of <see cref="CanvasHorizontalAlignment"/>.
    /// </summary>
    public sealed partial class FontAlignmentSegmented : UserControl
    {

        //@Delegate
        /// <summary> Occurs when alignment change. </summary>
        public EventHandler<CanvasHorizontalAlignment> AlignmentChanged;

        //@VisualState
        CanvasHorizontalAlignment _vsAlignment;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsAlignment)
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


        /// <summary> Alignment of <see cref = "FontAlignmentSegmented" />. </summary>
        public CanvasHorizontalAlignment Alignment
        {
            get => (CanvasHorizontalAlignment)base.GetValue(AlignmentProperty);
            set => base.SetValue(AlignmentProperty, value);
        }
        /// <summary> Identifies the <see cref = "FontAlignmentSegmented.Alignment" /> dependency property. </summary>
        public static readonly DependencyProperty AlignmentProperty = DependencyProperty.Register(nameof(Alignment), typeof(CanvasHorizontalAlignment), typeof(FontAlignmentSegmented), new PropertyMetadata(CanvasHorizontalAlignment.Left, (sender, e) =>
        {
            FontAlignmentSegmented control = (FontAlignmentSegmented)sender;

            if (e.NewValue is CanvasHorizontalAlignment value)
            {
                control._vsAlignment = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        /// <summary> IsOpen of <see cref = "FontAlignmentSegmented" />. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "FontAlignmentSegmented.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(FontAlignmentSegmented), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a FontAlignmentSegmented. 
        /// </summary>
        public FontAlignmentSegmented()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.Left.Click += (s, e) => this.AlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Left);//Delegate
            this.Center.Click += (s, e) => this.AlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Center);//Delegate
            this.Right.Click += (s, e) => this.AlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Right);//Delegate
            this.Justified.Click += (s, e) => this.AlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Justified);//Delegate

            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            if (ToolTipService.GetToolTip(this.Left) is ToolTip toolTip0)
            {
                toolTip0.Content = resource.GetString($"Texts_FontAlignment_Left");
            }
            if (ToolTipService.GetToolTip(this.Center) is ToolTip toolTip1)
            {
                toolTip1.Content = resource.GetString($"Texts_FontAlignment_Center");
            }
            if (ToolTipService.GetToolTip(this.Right) is ToolTip toolTip2)
            {
                toolTip2.Content = resource.GetString($"Texts_FontAlignment_Right");
            }
            if (ToolTipService.GetToolTip(this.Justified) is ToolTip toolTip3)
            {
                toolTip3.Content = resource.GetString($"Texts_FontAlignment_Justified");
            }
        }
    }
}