namespace Retouch_Photo2.Elements.DrawPages
{
    /// <summary>
    /// Manager of <see cref="DrawLayout"/>. 
    /// </summary>
    public class DrawLayoutStateManager
    {
        /// <summary> 
        /// PhoneState of <see cref="DrawLayoutStateManager"/>. 
        /// </summary>
        public enum DrawLayoutPhoneState
        {
            /// <summary> Hide left and right borders. </summary>
            Hided,
            /// <summary> Show left border. </summary>
            ShowLeft,
            /// <summary> Show right border. </summary>
            ShowRight,
        }

        /// <summary> <see cref="DrawLayout.IsFullScreen"/>. </summary>
        public bool IsFullScreen;
        /// <summary> <see cref="DrawLayout.Width"/>. </summary>
        public double Width;
        /// <summary> <see cref="DrawLayout.PhoneState"/>. </summary>
        public DrawLayoutPhoneState PhoneState;

        /// <summary>
        /// Return status based on propertys.
        /// </summary>
        /// <returns> state </returns>
        public DrawLayoutState GetState()
        {
            if (this.IsFullScreen) return DrawLayoutState.FullScreen;

            if (this.Width > 900.0) return DrawLayoutState.PC;
            if (this.Width > 600.0) return DrawLayoutState.Pad;

            switch (this.PhoneState)
            {
                case DrawLayoutPhoneState.Hided: return DrawLayoutState.Phone;
                case DrawLayoutPhoneState.ShowLeft: return DrawLayoutState.PhoneShowLeft;
                case DrawLayoutPhoneState.ShowRight: return DrawLayoutState.PhoneShowRight;
            }

            return DrawLayoutState.None;
        }
    }
}