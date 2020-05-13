namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Device layout.
    /// </summary>
    public struct DeviceLayout
    {
        public static DeviceLayout Default = new DeviceLayout
        {
            IsAdaptive = true,
            PhoneMaxWidth = 600,
            PadMaxWidth = 900,
            FallBackType = DeviceLayoutType.PC,
        };


        /// <summary> Adapt to screen width. </summary>
        public bool IsAdaptive;
        public int PhoneMaxWidth;
        public int PadMaxWidth;
        /// <summary> Fall back. </summary>
        public DeviceLayoutType FallBackType;


        /// <summary>
        /// Get the actual device type based on width
        /// </summary>
        /// <param name="width"> The width. </param>
        /// <returns> The product <see cref="DeviceLayoutType"/>. </returns>
        public DeviceLayoutType GetActualType(double width)
        {
            if (this.IsAdaptive == false) return this.FallBackType;

            if (width > this.PadMaxWidth) return DeviceLayoutType.PC;
            if (width > this.PhoneMaxWidth) return DeviceLayoutType.Pad;
            return DeviceLayoutType.Phone;
        }
    }
}