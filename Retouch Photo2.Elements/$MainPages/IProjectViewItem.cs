// Core:              ★★★
// Referenced:   ★★
// Difficult:         ★
// Only:              ★
// Complete:      ★
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Interface of <see cref="IProjectViewItem"/>.
    /// </summary>
    public interface IProjectViewItem
    {

        /// <summary> Gets or sets the name. </summary>
        string Name { get; set; }
        /// <summary> Gets or sets the visibility. </summary>
        Visibility Visibility { get; set; }
        
        /// <summary> Gets or sets the select-mode. </summary>
        SelectMode SelectMode { get; set; }

        /// <summary>
        /// Rename.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="thumbnail"> The thumbnail path. </param>
        void Rename(string name, string thumbnail);

        /// <summary>
        /// Switch the state.
        /// </summary>
        void SwitchState();

        /// <summary>
        /// Refresh image source.
        /// </summary>
        void RefreshImageSource();

        /// <summary>
        /// Get the position and size of the image element relative to the visual element. 
        /// </summary>
        /// <returns> The calculated rect. </returns>
        Rect GetVisualRect(UIElement visual);

    }
}