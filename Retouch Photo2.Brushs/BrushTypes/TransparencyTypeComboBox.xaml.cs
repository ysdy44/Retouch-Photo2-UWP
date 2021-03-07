// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Retouch_Photo2.Brushs.TransparencyTypeIcons;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// ComboBox of Transparency<see cref="BrushType"/>
    /// </summary>
    public sealed partial class TransparencyTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when type change. </summary>
        public EventHandler<BrushType> TypeChanged;

        //@Group
        /// <summary> Occurs when group change. </summary>
        private EventHandler<BrushType> Group;


        #region DependencyProperty


        /// <summary> Gets or sets the transparency. </summary>
        public IBrush Transparency
        {
            get  => (IBrush)base.GetValue(TransparencyProperty);
            set => base.SetValue(TransparencyProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Fill" /> dependency property. </summary>
        public static readonly DependencyProperty TransparencyProperty = DependencyProperty.Register(nameof(Transparency), typeof(IBrush), typeof(TransparencyTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            TransparencyTypeComboBox control = (TransparencyTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                control.Type = value.Type;
            }
            else
            {
                control.Type = BrushType.None;
            }
        }));

        /// <summary> Gets or sets the transparency type. </summary>
        public BrushType Type
        {
            get => this.type;
            set
            {
                this.Group?.Invoke(this, value);//Delegate
                this.type = value;
            }
        }
        private BrushType type = BrushType.None;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TransparencyTypeComboBox. 
        /// </summary>
        public TransparencyTypeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Click += (s, e) => this.Flyout.ShowAt(this);
        }

    }

    /// <summary>
    /// ComboBox of Transparency<see cref="BrushType"/>
    /// </summary>
    public sealed partial class TransparencyTypeComboBox : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructGroup(this.NoneButton, resource.GetString("Tools_Brush_Type_None"), new NoneIcon(), BrushType.None);

            this.ConstructGroup(this.LinearGradientButton, resource.GetString("Tools_Brush_Type_LinearGradient"), new LinearGradientIcon(), BrushType.LinearGradient);
            this.ConstructGroup(this.RadialGradientButton, resource.GetString("Tools_Brush_Type_RadialGradient"), new RadialGradientIcon(), BrushType.RadialGradient);
            this.ConstructGroup(this.EllipticalGradientButton, resource.GetString("Tools_Brush_Type_EllipticalGradient"), new EllipticalGradientIcon(), BrushType.EllipticalGradient);
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
            group(this.Type);

            //Buttons
            button.Content = text;
            button.Tag = icon;
            button.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, type);//Delegate

                this.Flyout.Hide();
            };

            //Group
            this.Group += (s, e) => group(e);
        }

    }
}