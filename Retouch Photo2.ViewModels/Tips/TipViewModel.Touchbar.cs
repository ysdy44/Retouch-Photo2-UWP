using Retouch_Photo2.Tools;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels.Tips
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {

        /// <summary> Touchbar type. </summary>
        public TouchbarType TouchbarType
        {
            get => this.touchbarType;
            set
            {
                if (this.touchbarType == value) return;

                ITouchbar touchbar = this.GetTouchbar(value);

                if (touchbar == null)
                    this.TouchbarControl = null;
                else
                    this.TouchbarControl = touchbar.Self;

                this.touchbarType = value;
                this.OnPropertyChanged(nameof(this.TouchbarType));//Notify 
            }
        }
        private TouchbarType touchbarType;

        /// <summary> Touchbar control. </summary>
        public UserControl TouchbarControl
        {

            get => this.touchbarControl;
            set
            {
                this.touchbarControl = value;
                this.OnPropertyChanged(nameof(this.TouchbarControl));//Notify 
            }
        }
        private UserControl touchbarControl;

        private ITouchbar GetTouchbar(TouchbarType type)
        {
            switch (type)
            {
                case TouchbarType.None: return null;
                case TouchbarType.StrokeWidth: return this.StrokeWidthTouchbar;
                case TouchbarType.ViewRadian: return this.ViewRadianTouchbar;
                case TouchbarType.ViewScale: return this.ViewScaleTouchbar;
                case TouchbarType.AcrylicTintOpacity: return this.AcrylicTintOpacityTouchbar;
                case TouchbarType.AcrylicBlurAmount: return this.AcrylicBlurAmountTouchbar;
            }
            return null;
        }


        /// <summary> StrokeWidthTouchbar. </summary>
        public ITouchbar StrokeWidthTouchbar;

        /// <summary> ViewRadianTouchbar. </summary>
        public ITouchbar ViewRadianTouchbar;
        /// <summary> ViewScaleTouchbar. </summary>
        public ITouchbar ViewScaleTouchbar;

        /// <summary> AcrylicTintOpacityTouchbar. </summary>
        public ITouchbar AcrylicTintOpacityTouchbar;
        /// <summary> AcrylicBlurAmountTouchbar. </summary>
        public ITouchbar AcrylicBlurAmountTouchbar;

    }
}