namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Image of <see cref="Photo">.
    /// </summary>
    public partial class Photo
    {
        //@SavePhoto
        /// <summary>
        /// Avoid unwanted pictures being saved.
        /// When that method occous, it is ""True"".
        /// <see cref="Retouch_Photo2.Elements.XML.SavePhotocopier"/>
        /// </summary>
        public bool HasSavePhotocopier = false;

    }
}