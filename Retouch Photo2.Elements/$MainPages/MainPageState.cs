// Core:              
// Referenced:   
// Difficult:         
// Only:              
// Complete:      
namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// State of <see cref="MainPage"/>. 
    /// </summary>
    public enum MainPageState
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Initial page( 0 project). </summary>
        Initial,

        /// <summary> Main page. </summary>
        Main,

        /// <summary> Show dialog. </summary>
        Dialog,

        /// <summary> Add a pictures project. </summary>
        Pictures,
        /// <summary> Rename a project. </summary>
        Rename,

        /// <summary> Delete project(s). </summary>
        Delete,
        /// <summary> Duplicate project(s). </summary>
        Duplicate,
    }
}