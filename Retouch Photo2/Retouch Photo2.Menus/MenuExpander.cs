// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus
{
    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    public partial class MenuExpander : Expander
    {

        //@Static
        /// <summary>
        /// Show a flyout with a specific name.
        /// </summary>
        public static void ShowAt(MenuType type, FrameworkElement placementTarget) => MenuExpander.DictionaryCore[type].ShowAt(placementTarget);
        /// <summary>
        /// Gets all MenuExpander.
        /// </summary>
        public static IEnumerable<KeyValuePair<MenuType, MenuExpander>> Dictionary => MenuExpander.DictionaryCore;


        private readonly static IDictionary<MenuType, MenuExpander> DictionaryCore = new Dictionary<MenuType, MenuExpander>();
        private static void Add(MenuType type, MenuExpander expander) => MenuExpander.DictionaryCore.Add(type, expander);
        private static void Remove(MenuType type) => MenuExpander.DictionaryCore.Remove(type);


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "MenuExpander" />'s type. </summary>
        public MenuType Type
        {
            get => (MenuType)base.GetValue(TypeProperty);
            set => base.SetValue(TypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "MenuExpander.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(MenuType), typeof(MenuExpander), new PropertyMetadata(MenuType.None, (sender, e) =>
        {
            MenuExpander control = (MenuExpander)sender;

            if (e.NewValue is MenuType value)
            {
                MenuExpander.Add(value, control);
            }
        }));


        #endregion


        //@Construct     
        /// <summary>
        /// Initializes a MenuExpander. 
        /// </summary>
        public MenuExpander() : base() { }
        ~MenuExpander() => MenuExpander.Remove(this.Type);

    }
}