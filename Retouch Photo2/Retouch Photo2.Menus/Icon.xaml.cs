// Core:              ★★
// Referenced:   ★★★
// Difficult:         ★
// Only:              ★★
// Complete:      ★★
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Icon of <see cref="Retouch_Photo2.Menus"/>.
    /// </summary>
    public sealed partial class Icon : UserControl
    {

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "Icon" />'s Type. </summary>
        public MenuType Type
        {
            get => (MenuType)base.GetValue(TypeProperty);
            set => base.SetValue(TypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "Icon.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(MenuType), typeof(Icon), new PropertyMetadata(MenuType.None, (sender, e) =>
        {
            Icon control = (Icon)sender;

            if (e.NewValue is MenuType value)
            {
                control.Content = control.GetIcon(value);
            }
        }));


        #endregion

        //@Construct
        /// <summary>
        /// Initializes a Icon. 
        /// </summary>
        public Icon()
        {
            this.InitializeComponent();
        }

        private Control GetIcon(MenuType type)
        {
            switch (type)
            {
                case MenuType.None: return null;

                case MenuType.Edit: return new Retouch_Photo2.Edits.Icon();
                case MenuType.Operate: return new Retouch_Photo2.Operates.Icon();

                case MenuType.Adjustment: return new Retouch_Photo2.Adjustments.Icon();
                case MenuType.Effect: return new Retouch_Photo2.Effects.Icon();

                case MenuType.Text: return new Retouch_Photo2.Texts.Icon();
                case MenuType.Stroke: return new Retouch_Photo2.Strokes.Icon();

                case MenuType.Style: return new Retouch_Photo2.Styles.Icon();
                case MenuType.Filter: return new Retouch_Photo2.Filters.Icon();

                case MenuType.History: return new Retouch_Photo2.Historys.Icon();
                case MenuType.Transformer: return new FanKit.Transformers.Icon();

                case MenuType.Layer: return new Retouch_Photo2.Layers.Icon();
              
                case MenuType.Pantone: return new PantoneIcon();
                case MenuType.Color: return new ColorIcon();

                default: return null;
            }
        }

    }
}