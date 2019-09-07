namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary> 
    /// State of <see cref="MainPage"/>. 
    /// </summary>
    public enum MainPageState
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Main. </summary>
        Main,
        /// <summary> Loading. </summary>
        //Loading,

        /// <summary> Add a blank project. </summary>
        //Add,
        /// <summary> Add a pictures project. </summary>
        Pictures,

        /// <summary> Save project(s). </summary>
        Save,
        /// <summary> Share project(s). </summary>
        Share,

        /// <summary> Delete project(s). </summary>
        Delete,
        /// <summary> Duplicate project(s). </summary>
        Duplicate,

        /// <summary> Create a new Folder. </summary>
        //Folder,
        /// <summary> Move a project into a folder. </summary>
        Move,
    }
}