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
    /// Segmented of <see cref="CanvasTextDirection"/>.
    /// </summary>
    public sealed partial class DirectionSegmented : UserControl
    {

        //@Delegate
        /// <summary> Occurs when direction change. </summary>
        public EventHandler<CanvasTextDirection> DirectionChanged;

        //@VisualState
        CanvasTextDirection _vsDirection;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {

                switch (this._vsDirection)
                {
                    case CanvasTextDirection.LeftToRightThenTopToBottom: return this.LeftToRightThenTopToBottomState;
                    case CanvasTextDirection.RightToLeftThenTopToBottom: return this.RightToLeftThenTopToBottomState;

                    case CanvasTextDirection.LeftToRightThenBottomToTop: return this.LeftToRightThenBottomToTopState;
                    case CanvasTextDirection.RightToLeftThenBottomToTop: return this.RightToLeftThenBottomToTopState;


                    case CanvasTextDirection.TopToBottomThenLeftToRight: return this.TopToBottomThenLeftToRightState;
                    case CanvasTextDirection.BottomToTopThenLeftToRight: return this.BottomToTopThenLeftToRightState;

                    case CanvasTextDirection.TopToBottomThenRightToLeft: return this.TopToBottomThenRightToLeftState;
                    case CanvasTextDirection.BottomToTopThenRightToLeft: return this.BottomToTopThenRightToLeftState;

                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets the fontvDirection. </summary>
        public CanvasTextDirection Direction
        {
            get => (CanvasTextDirection)base.GetValue(CanvasTextDirectionProperty);
            set => base.SetValue(CanvasTextDirectionProperty, value);
        }
        /// <summary> Identifies the <see cref = "DirectionSegmented.Direction" /> dependency property. </summary>
        public static readonly DependencyProperty CanvasTextDirectionProperty = DependencyProperty.Register(nameof(Direction), typeof(CanvasTextDirection), typeof(DirectionSegmented), new PropertyMetadata(CanvasTextDirection.LeftToRightThenTopToBottom, (sender, e) =>
        {
            DirectionSegmented control = (DirectionSegmented)sender;

            if (e.NewValue is CanvasTextDirection value)
            {
                control._vsDirection = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        /// <summary> Gets or sets the IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "DirectionSegmented.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(DirectionSegmented), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a DirectionSegmented. 
        /// </summary>
        public DirectionSegmented()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.LeftToRightThenTopToBottom.Tapped += (s, e) => this.DirectionChanged?.Invoke(this, CanvasTextDirection.LeftToRightThenTopToBottom);//Delegate
            this.RightToLeftThenTopToBottom.Tapped += (s, e) => this.DirectionChanged?.Invoke(this, CanvasTextDirection.RightToLeftThenTopToBottom);//Delegate

            this.LeftToRightThenBottomToTop.Tapped += (s, e) => this.DirectionChanged?.Invoke(this, CanvasTextDirection.LeftToRightThenBottomToTop);//Delegate
            this.RightToLeftThenBottomToTop.Tapped += (s, e) => this.DirectionChanged?.Invoke(this, CanvasTextDirection.RightToLeftThenBottomToTop);//Delegate


            this.TopToBottomThenLeftToRight.Tapped += (s, e) => this.DirectionChanged?.Invoke(this, CanvasTextDirection.TopToBottomThenLeftToRight);//Delegate
            this.BottomToTopThenLeftToRight.Tapped += (s, e) => this.DirectionChanged?.Invoke(this, CanvasTextDirection.BottomToTopThenLeftToRight);//Delegate

            this.TopToBottomThenRightToLeft.Tapped += (s, e) => this.DirectionChanged?.Invoke(this, CanvasTextDirection.TopToBottomThenRightToLeft);//Delegate
            this.BottomToTopThenRightToLeft.Tapped += (s, e) => this.DirectionChanged?.Invoke(this, CanvasTextDirection.BottomToTopThenRightToLeft);//Delegate

            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            foreach (UIElement child in this.RootGrid.Children)
            {
                if (child is ListViewItem item)
                {
                    if (ToolTipService.GetToolTip(item) is ToolTip toolTip)
                    {
                        string key = item.Name;
                        string title = resource.GetString($"Texts_Direction_{key}");

                        toolTip.Content = title;
                    }
                }
            }
        }
    }
}