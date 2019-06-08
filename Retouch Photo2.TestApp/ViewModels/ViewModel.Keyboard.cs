using Windows.System;
using Windows.UI.Xaml;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel
    {

        public ViewModel()
        {
            Window.Current.CoreWindow.KeyUp += (s, e) =>
            {
                VirtualKey key = e.VirtualKey;
                this.KeyUp(key);
                this.KeyUpAndDown(key);
            };
            Window.Current.CoreWindow.KeyDown += (s, e) =>
            {
                VirtualKey key = e.VirtualKey;
                this.KeyDown(key);
                this.KeyUpAndDown(key);
            };
        }


        /// <summary> keyboard's the **SHIFT** key. </summary>
        public bool KeyShift
        {
            get => this.keyShift;
            set
            {
                this.keyShift = value;
                this.OnPropertyChanged(nameof(this.KeyShift));//Notify 
            }
        }
        private bool keyShift;

        /// <summary> keyboard's the **CTRL** key. </summary>
        private bool keyCtrl;
        public bool KeyCtrl
        {
            get => this.keyCtrl;
            set
            {
                this.keyCtrl = value;
                this.OnPropertyChanged(nameof(this.KeyCtrl));//Notify 
            }
        }

        /// <summary> keyboard's the **ALT** key. </summary>
        private bool keyAlt;
        public bool KeyAlt
        {
            get => this.keyAlt;
            set
            {
                this.keyAlt = value;
                this.OnPropertyChanged(nameof(this.KeyAlt));//Notify 
            }
        }


        private void KeyDown(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.Control:
                    this.KeyCtrl = true;
                    break;

                case VirtualKey.Shift:
                    this.KeyShift = true;
                    break;

                case VirtualKey.Delete:
                    break;

                default:
                    break;
            }
        }


        private void KeyUp(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.Control:
                    this.KeyCtrl = false;
                    break;

                case VirtualKey.Shift:
                    this.KeyShift = false;
                    break;

                default:
                    break;
            }
        }
        
        private void KeyUpAndDown(VirtualKey key)
        {
            if (this.KeyCtrl == false && this.KeyShift == false)
            {
            }
            else if (this.KeyCtrl == false && this.KeyShift)
            {
            }
            else if (this.KeyCtrl && this.KeyShift == false)
            {
            }
            else //if (this.KeyCtrl && this.KeyShift)
            {
            }
        }

    }
}