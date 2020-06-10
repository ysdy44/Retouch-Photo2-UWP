using Retouch_Photo2.Menus;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a <see cref="MenuType"/> from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created <see cref="MenuType"/>. </returns>
        public static MenuType CreateMenuType(string type)
        {
            switch (type)
            {
                case "Debug": return MenuType.Debug;

                case "Edit": return MenuType.Edit;
                case "Operate": return MenuType.Operate;

                case "Adjustment": return MenuType.Adjustment;
                case "Effect": return MenuType.Effect;

                case "Text": return MenuType.Text;

                case "Stroke": return MenuType.Stroke;
                case "Style": return MenuType.Style;

                case "History": return MenuType.History;
                case "Transformer": return MenuType.Transformer;

                case "Layer": return MenuType.Layer;
                case "Color": return MenuType.Color;
                case "Keyboard": return MenuType.Keyboard;

                default: return MenuType.Keyboard;
            }
        }

    }
}