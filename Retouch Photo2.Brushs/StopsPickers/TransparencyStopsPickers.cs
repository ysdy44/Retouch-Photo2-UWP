namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Transparency for <see cref="StopsPicker"/>.
    /// </summary>
    public class TransparencyStopsPickers : StopsPicker
    {

        #region DependencyProperty

        /// <summary> Gets or sets the transparency. </summary>
        public IBrush Transparency
        {
            get => this.transparency;
            set
            {
                if ((value is null) == false) base.SetArray(value.Stops);
                this.transparency = value;
            }
        }
        private IBrush transparency;


        #endregion

    }
}