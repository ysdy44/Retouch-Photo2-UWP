using Windows.Storage;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// ID of <see cref="Photo"/>.
    /// </summary>
    public struct Photocopier
    {     
        /// <summary> Gets or sets <see cref="StorageFile.Name"/>. </summary>
        public string Name;
        /// <summary> Gets or sets <see cref="StorageFile.FileType"/>. </summary>
        public string FileType;
        /// <summary> Gets or sets <see cref="StorageFile.FolderRelativeId"/>. </summary>
        public string FolderRelativeId;

        //@Override
        /// <summary>
        /// Returns a String representing this <see cref="Photo"/> instance.
        /// </summary>
        /// <returns> The string representation. </returns>
        public override string ToString() => this.FolderRelativeId;

    }
}