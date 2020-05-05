using Retouch_Photo2.Brushs.FillOrStrokeIcons;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents the contorl that is used to select fill or stroke.
    /// </summary>
    public sealed partial class FillOrStrokeControl : UserControl
    {

        //@Delegate
        public EventHandler<FillOrStroke> FillOrStrokeChanged;


        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get { return (FillOrStroke)GetValue(FillOrStrokeProperty); }
            set { SetValue(FillOrStrokeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "FillOrStrokeControl.FillOrStroke" /> dependency property. </summary>
        public static readonly DependencyProperty FillOrStrokeProperty = DependencyProperty.Register(nameof(FillOrStroke), typeof(FillOrStroke), typeof(FillOrStrokeControl), new PropertyMetadata(FillOrStroke.Fill, (sender, e) =>
        {
            FillOrStrokeControl con = (FillOrStrokeControl)sender;

            if (e.NewValue is FillOrStroke value)
            {
                con._vsFillOrStroke = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion


        //@VisualState
        FillOrStroke _vsFillOrStroke;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsFillOrStroke)
                {
                    case FillOrStroke.Fill: return this.Fill;
                    case FillOrStroke.Stroke: return this.Stroke;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        public FillOrStrokeControl()
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

            this.FillButton.Content = resource.GetString("/ToolElements/Fill");
            this.FillButton.Tag = new FillIcon();
            this.FillButton.Tapped += (s, e) =>
            {
                this.FillOrStrokeChanged?.Invoke(this, FillOrStroke.Fill); //Delegate
                this.Flyout.Hide();
            };

            this.StrokeButton.Content = resource.GetString("/ToolElements/Stroke");
            this.StrokeButton.Tag = new StrokeIcon();
            this.StrokeButton.Tapped += (s, e) =>
            {
                this.FillOrStrokeChanged?.Invoke(this, FillOrStroke.Stroke); //Delegate
                this.Flyout.Hide();
            };
        }

    }
}
