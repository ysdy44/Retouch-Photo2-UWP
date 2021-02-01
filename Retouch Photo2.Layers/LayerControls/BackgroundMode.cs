// Core:              
// Referenced:   
// Difficult:         
// Only:              
// Complete:      
namespace Retouch_Photo2.Layers
{
    /// <summary> 
    /// Background mode of <see cref="LayerControl"/>.
    /// </summary>
    public enum BackgroundMode
    {
        /// <summary> Not selected. </summary>
        UnSelected,
        /// <summary> Selected. </summary>
        Selected,

        /// <summary> Parents were selected. </summary>
        ParentsSelected,
        /// <summary> Child is selected. </summary>
        ChildSelected,

    }
}