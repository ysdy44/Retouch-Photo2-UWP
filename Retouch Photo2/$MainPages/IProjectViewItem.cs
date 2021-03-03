// Core:              ★★★
// Referenced:   ★★
// Difficult:         ★
// Only:              ★
// Complete:      ★
using Retouch_Photo2.ViewModels;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Retouch_Photo2
{
    /// <summary>
    /// Interface of <see cref="IProjectViewItem"/>.
    /// </summary>
    public interface IProjectViewItem
    {

        /// <summary> Gets or sets the name. </summary>
        string Name { get; set; }
        /// <summary> Gets or sets the thumbnail path. </summary>
        Uri ImageSource { get; set; }
        /// <summary> Gets or sets the project. </summary>
        Project Project { get; set; }
        /// <summary> Gets or sets the rect of image visual area. </summary>
        Rect ImageVisualRect { get; }


        /// <summary> Gets or sets the visibility. </summary>
        Visibility Visibility { get; set; }
        /// <summary> Gets or sets the state of select-mode. </summary>
        bool IsMultiple { get; set; }
        /// <summary> Gets or sets the weather is selected. </summary>
        bool IsSelected { get; set; }


        /// <summary>
        /// Get the position and size of the image element relative to the visual element. 
        /// </summary>
        /// <returns> The calculated widnows. </returns>
        void RenderImageVisualRect(UIElement widnows);

    }
}