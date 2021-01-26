using Retouch_Photo2.Brushs.BrushTypeIcons;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// ComboBox of <see cref="BrushType"/>
    /// </summary>
    public sealed partial class BrushTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when fill type change. </summary>
        public EventHandler<BrushType> FillTypeChanged;
        /// <summary> Occurs when stroke type change. </summary>
        public EventHandler<BrushType> StrokeTypeChanged;

        //@Group
        /// <summary> Occurs when group change. </summary>
        private EventHandler<BrushType> Group;


        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get  => (FillOrStroke)base.GetValue(FillOrStrokeProperty);
            set => base.SetValue(FillOrStrokeProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.FillOrStroke" /> dependency property. </summary>
        public static readonly DependencyProperty FillOrStrokeProperty = DependencyProperty.Register(nameof(FillOrStroke), typeof(FillOrStroke), typeof(BrushTypeComboBox), new PropertyMetadata(FillOrStroke.Fill, (sender, e) =>
        {
            BrushTypeComboBox control = (BrushTypeComboBox)sender;

            if (e.NewValue is FillOrStroke value)
            {
                switch (value)
                {
                    case FillOrStroke.Fill:
                        control.Group?.Invoke(control, control.FillType);//Delegate
                        break;
                    case FillOrStroke.Stroke:
                        control.Group?.Invoke(control, control.StrokeType);//Delegate
                        break;
                }
            }
        }));


        /// <summary> Gets or sets the fill. </summary>
        public IBrush Fill
        {
            get  => (IBrush)base.GetValue(FillProperty);
            set => base.SetValue(FillProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Fill" /> dependency property. </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(IBrush), typeof(BrushTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BrushTypeComboBox control = (BrushTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                control.FillType = value.Type;
            }
            else
            {
                control.FillType = BrushType.None;
            }
        }));

        /// <summary> Gets or sets the fill type. </summary>
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


        /// <summary> Gets or sets the stroke. </summary>
        public IBrush Stroke
        {
            get  => (IBrush)base.GetValue(StrokeProperty);
            set => base.SetValue(StrokeProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Stroke" /> dependency property. </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(IBrush), typeof(BrushTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BrushTypeComboBox control = (BrushTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                control.StrokeType = value.Type;
            }
            else
            {
                control.StrokeType = BrushType.None;
            }
        }));

        /// <summary> Gets or sets the stroke type. </summary>
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
        /// <summary>
        /// Initializes a BrushTypeComboBox. 
        /// </summary>
        public BrushTypeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Click += (s, e) => this.Flyout.ShowAt(this);
        }

    }

    /// <summary>
    /// ComboBox of <see cref="BrushType"/>
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
            button.Click += (s, e) =>
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