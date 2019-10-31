namespace Retouch_Photo2.Layers
{
    public struct ImageStr
    {     
        /// <summary> Gets or sets <see cref="StorageFile.Name"/>. </summary>
        public string Name;
        /// <summary> Gets or sets <see cref="StorageFile.FileType"/>. </summary>
        public string FileType;
        /// <summary> Gets or sets <see cref="StorageFile.FolderRelativeId"/>. </summary>
        public string FolderRelativeId;

        //@Override
        /// <summary>
        /// Returns a String representing this ImageRe instance.
        /// </summary>
        /// <returns> The string representation. </returns>
        public override string ToString() => this.FolderRelativeId;

    }
}