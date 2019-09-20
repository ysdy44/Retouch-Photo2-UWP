using Retouch_Photo2.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
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

                ITouchbar touchbar = this.Touchbars.FirstOrDefault(t => t.Type == value);

                if (touchbar==null)
                {
                    this.TouchbarControl = null;
                }
                else
                {
                    this.TouchbarControl = touchbar.Self;
                }
 
                this.touchbarType = value;
                this.OnPropertyChanged(nameof(this.TouchbarType));//Notify 
            }
        }
        private TouchbarType touchbarType;

        /// <summary> Touchbar's control. </summary>
        public UIElement TouchbarControl
        {

            get => this.touchbarControl;
            set
            {
                this.touchbarControl = value;
                this.OnPropertyChanged(nameof(this.TouchbarControl));//Notify 
            }
        }
        private UIElement touchbarControl;

        /// <summary> Touchbars. </summary>
        public IList<ITouchbar> Touchbars { get; set; } = new List<ITouchbar>();

    }
}