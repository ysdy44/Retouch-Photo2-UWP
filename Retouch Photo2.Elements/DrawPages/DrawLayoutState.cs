namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// State of <see cref="DrawLayout"/>. 
    /// </summary>
    public enum DrawLayoutState
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Full-screen. </summary>
        FullScreen,

        /// <summary> Phone. </summary>
        Phone,
        /// <summary> Phone (Show left border). </summary>
        PhoneShowLeft,
        /// <summary> Phone (Show right border). </summary>
        PhoneShowRight,

        /// <summary> Pad. </summary>
        Pad,

        /// <summary> Person computer. </summary>
        PC,
    }
}