using Retouch_Photo2.Brushs.BrushTypeIcons;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents the combo box that is used to select brush type.
    /// </summary>
    public sealed partial class BrushTypeComboBox : UserControl
    {

        //@Delegate
        public EventHandler<BrushType> FillTypeChanged;
        public EventHandler<BrushType> StrokeTypeChanged;

        //@Group
        private EventHandler<BrushType> Group;

        //@Content
        public Storyboard EaseStoryboard => this._EaseStoryboard;

        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get { return (FillOrStroke)GetValue(FillOrStrokeProperty); }
            set { SetValue(FillOrStrokeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty FillOrStrokeProperty = DependencyProperty.Register(nameof(FillOrStroke), typeof(FillOrStroke), typeof(BrushTypeComboBox), new PropertyMetadata(FillOrStroke.Fill, (sender, e) =>
        {
            BrushTypeComboBox con = (BrushTypeComboBox)sender;

            if (e.NewValue is FillOrStroke value)
            {
                switch (value)
                {
                    case FillOrStroke.Fill:
                        con.Group?.Invoke(con, con.FillType);//Delegate
                        break;
                    case FillOrStroke.Stroke:
                        con.Group?.Invoke(con, con.StrokeType);//Delegate
                        break;
                }
            }
        }));


        /// <summary> Gets or sets the fill-brush. </summary>
        public IBrush FillBrush
        {
            get { return (IBrush)GetValue(FillBrushProperty); }
            set { SetValue(FillBrushProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.FillBrush" /> dependency property. </summary>
        public static readonly DependencyProperty FillBrushProperty = DependencyProperty.Register(nameof(FillBrush), typeof(IBrush), typeof(BrushTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BrushTypeComboBox con = (BrushTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                con.FillType = value.Type;
            }
            else
            {
                con.FillType = BrushType.None;
            }
        }));

        public BrushType FillType
        {
            get => this.fillType;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.Group?.Invoke(this, value);//Delegate
                        break;
                }
                this.fillType = value;
            }
        }
        private BrushType fillType = BrushType.None;


        /// <summary> Gets or sets the stroke-brush. </summary>
        public IBrush StrokeBrush
        {
            get { return (IBrush)GetValue(StrokeBrushProperty); }
            set { SetValue(StrokeBrushProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.StrokeBrush" /> dependency property. </summary>
        public static readonly DependencyProperty StrokeBrushProperty = DependencyProperty.Register(nameof(StrokeBrush), typeof(IBrush), typeof(BrushTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BrushTypeComboBox con = (BrushTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                con.StrokeType = value.Type;
            }
            else
            {
                con.StrokeType = BrushType.None;
            }
        }));

        public BrushType StrokeType
        {
            get => this.strokeType;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        this.Group?.Invoke(this, value);//Delegate
                        break;
                }
                this.strokeType = value;
            }
        }
        private BrushType strokeType = BrushType.None;


        #endregion


        //@Construct
        public BrushTypeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
        }

    }

    /// <summary>
    /// Represents the combo box that is used to select brush type.
    /// </summary>
    public sealed partial class BrushTypeComboBox : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructGroup(this.NoneButton, resource.GetString("/ToolElements/BrushType_None"), new NoneIcon(), BrushType.None);

            this.ConstructGroup(this.ColorButton, resource.GetString("/ToolElements/BrushType_Color"), new ColorIcon(), BrushType.Color);

            this.ConstructGroup(this.LinearGradientButton, resource.GetString("/ToolElements/BrushType_LinearGradient"), new LinearGradientIcon(), BrushType.LinearGradient);
            this.ConstructGroup(this.RadialGradientButton, resource.GetString("/ToolElements/BrushType_RadialGradient"), new RadialGradientIcon(), BrushType.RadialGradient);
            this.ConstructGroup(this.EllipticalGradientButton, resource.GetString("/ToolElements/BrushType_EllipticalGradient"), new EllipticalGradientIcon(), BrushType.EllipticalGradient);

            this.ConstructGroup(this.ImageButton, resource.GetString("/ToolElements/BrushType_Image"), new ImageIcon(), BrushType.Image);
        }

        //Group
        private void ConstructGroup(Button button, string text, UserControl icon, BrushType type)
        {
            void group(BrushType groupType)
            {
                if (groupType == type)
                {
                    button.IsEnabled = false;

                    this.Button.Content = text;
                }
                else button.IsEnabled = true;
            }

            //NoneButton
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    group(this.FillType);
                    break;
                case FillOrStroke.Stroke:
                    group(this.StrokeType);
                    break;
            }

            //Buttons
            button.Content = text;
            button.Tag = icon;
            button.Tapped += (s, e) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillTypeChanged?.Invoke(this, type);//Delegate
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeTypeChanged?.Invoke(this, type);//Delegate
                        break;
                }
                this.Flyout.Hide();
            };

            //Group
            this.Group += (s, e) => group(e);
        }

    }
}