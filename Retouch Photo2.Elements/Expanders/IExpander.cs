using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Interface of <see cref="Expander">.
    /// </summary>
    public interface IExpander
    {
        /// <summary> Occurs when the position changes. </summary>
        Action Move { get; set; }
        /// <summary> Occurs when the flyout showed. </summary>
        Action Show { get; set; }
        /// <summary> Occurs when the flyout closed. </summary>
        Action Closed { get; set; }
        /// <summary> Occurs when the flyout opened. </summary>
        Action Opened { get; set; }

        /// <summary> Occurs when the state changes. </summary>
        Action<ExpanderState> StateChanged { get; set; }

        
        /// <summary> Gets or sets the title. </summary>
        string Title { get; set; }
        /// <summary> Gets or sets the title grid. </summary>
        FrameworkElement TitleGrid { get; }
        /// <summary> Gets or sets the reset button. </summary>
        FrameworkElement ResetButton { get; }

        /// <summary> Gets or sets the main child. </summary>
        UIElement MainPage { get; set; }
        /// <summary> Gets or sets the second child. </summary>
        UIElement SecondPage { get; set; }
               

        /// <summary> Gets or sets a value that declares whether the status is "Second". </summary>
        bool IsSecondPage { get; set; }
        /// <summary> Gets or sets the state. </summary>
        ExpanderState State { get; set; }
        
        /// <summary> Gets IMenu's placement mode. </summary>
        FlyoutPlacementMode PlacementMode { get; set; }
        /// <summary> Gets IMenu's placement mode. </summary>
        Point Postion { get; set; }
    }
}