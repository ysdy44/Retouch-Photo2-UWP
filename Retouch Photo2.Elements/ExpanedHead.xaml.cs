// Core:              ★★
// Referenced:   
// Difficult:         ★*
// Only:              ★★
// Complete:      ★★
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents a header control
    /// that can be extended 
    /// based on the <see cref="ScrollViewer.VerticalOffset"/>.
    /// </summary>  
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(UnExpaned), GroupName = nameof(CommonStates))]
    [TemplatePart(Name = nameof(Title), Type = typeof(string))]
    [TemplatePart(Name = nameof(LeftButtonContent), Type = typeof(object))]
    [TemplatePart(Name = nameof(RightButtonContent), Type = typeof(object))]
    [TemplatePart(Name = nameof(LeftButtonToolTip), Type = typeof(string))]
    [TemplatePart(Name = nameof(RightButtonToolTip), Type = typeof(string))]
    public sealed class ExpanedHead : Control
    {

        //@Delegate
        /// <summary> Occurs when the tapped the left button. </summary>
        public event TappedEventHandler LeftButtonTapped;
        /// <summary> Occurs when the tapped the right button. </summary>
        public event TappedEventHandler RightButtonTapped;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "ExpanedHead.TitleTextBlock" />'s text. </summary>
        public string Title
        {
            get => (string)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "ExpanedHead.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(ExpanedHead), new PropertyMetadata(null));


        /// <summary> Gets or sets <see cref = "ExpanedHead.LeftButton" />'s content. </summary>
        public object LeftButtonContent
        {
            get => (object)base.GetValue(LeftButtonContentProperty);
            set => base.SetValue(LeftButtonContentProperty, value);
        }
        /// <summary> Identifies the <see cref = "ExpanedHead.RightButtonContent" /> dependency property. </summary>
        public static readonly DependencyProperty LeftButtonContentProperty = DependencyProperty.Register(nameof(LeftButtonContent), typeof(object), typeof(ExpanedHead), new PropertyMetadata(null));


        /// <summary> Gets or sets <see cref = "ExpanedHead.RightButton" />'s right button content. </summary>
        public object RightButtonContent
        {
            get => (object)base.GetValue(RightButtonContentProperty);
            set => base.SetValue(RightButtonContentProperty, value);
        }
        /// <summary> Identifies the <see cref = "ExpanedHead.RightButtonContent" /> dependency property. </summary>
        public static readonly DependencyProperty RightButtonContentProperty = DependencyProperty.Register(nameof(RightButtonContent), typeof(object), typeof(ExpanedHead), new PropertyMetadata(null));


        /// <summary> Gets or sets <see cref = "ExpanedHead.LeftButton" />'s ToolTip. </summary>
        public string LeftButtonToolTip
        {
            get => (string)base.GetValue(LeftButtonToolTipProperty);
            set => base.SetValue(LeftButtonToolTipProperty, value);
        }
        /// <summary> Identifies the <see cref = "ExpanedHead.LeftButtonToolTip" /> dependency property. </summary>
        public static readonly DependencyProperty LeftButtonToolTipProperty = DependencyProperty.Register(nameof(LeftButtonToolTip), typeof(string), typeof(ExpanedHead), new PropertyMetadata(null));


        /// <summary> Gets or sets <see cref = "ExpanedHead.RightButton" />'s ToolTip. </summary>
        public string RightButtonToolTip
        {
            get => (string)base.GetValue(RightButtonToolTipProperty);
            set => base.SetValue(RightButtonToolTipProperty, value);
        }
        /// <summary> Identifies the <see cref = "ExpanedHead.RightButtonToolTip" /> dependency property. </summary>
        public static readonly DependencyProperty RightButtonToolTipProperty = DependencyProperty.Register(nameof(RightButtonToolTip), typeof(string), typeof(ExpanedHead), new PropertyMetadata(null));


        #endregion


        VisualStateGroup CommonStates;
        VisualState Normal;
        VisualState UnExpaned;
        ListViewItem LeftButton;
        ListViewItem RightButton;


        //@VisualState
        bool _vsIsExpaned;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsExpaned) return this.Normal;
                else return this.UnExpaned;
            }
            set => VisualStateManager.GoToState(this, value?.Name ?? "Normal", true);
        }


        //@Construct
        /// <summary>
        /// Initializes a ExpanedHead.
        /// </summary>
        public ExpanedHead()
        {
            this.DefaultStyleKey = typeof(ExpanedHead);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CommonStates = base.GetTemplateChild(nameof(CommonStates)) as VisualStateGroup;
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.UnExpaned = base.GetTemplateChild(nameof(UnExpaned)) as VisualState;

            if (this.LeftButton != null) this.LeftButton.Tapped -= this.LeftButtonTapped;
            this.LeftButton = base.GetTemplateChild(nameof(LeftButton)) as ListViewItem;
            if (this.LeftButton != null) this.LeftButton.Tapped += this.LeftButtonTapped;

            if (this.RightButton != null) this.RightButton.Tapped -= this.RightButtonTapped;
            this.RightButton = base.GetTemplateChild(nameof(RightButton)) as ListViewItem;
            if (this.RightButton != null) this.RightButton.Tapped += this.RightButtonTapped;
        }


        /// <summary>
        /// Gets or sets how long the distance is moving before it Expaned.
        /// </summary>
        public double Distance { get; set; } = 100;
        private double cacheDistance;

        private double previousOfset;

        /// <summary>
        /// Move for Expaned.
        /// </summary>
        /// <param name="offset"> The <see cref="ScrollViewer.VerticalOffset"/>. </param>
        public void Move(double offset)
        {
            if (offset < this.Distance / 2)
            {
                this._vsIsExpaned = true;
                this.VisualState = this.VisualState;//State
                return;
            }

            double changed = offset - this.previousOfset;
            this.previousOfset = offset;

            this.cacheDistance += changed;
            if (this.cacheDistance > this.Distance)
            {
                this.cacheDistance = this.Distance;

                this._vsIsExpaned = false;
                this.VisualState = this.VisualState;//State
            }
            else if (this.cacheDistance < -this.Distance)
            {
                this.cacheDistance = -this.Distance;

                this._vsIsExpaned = true;
                this.VisualState = this.VisualState;//State
            }
        }

    }
}