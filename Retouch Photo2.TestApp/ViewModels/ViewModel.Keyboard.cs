using Windows.System;
using Windows.UI.Xaml;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel
    {

        //@Construct
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

        private void KeyDown(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.Shift:
                    this.KeyShift = true;
                    break;

                case VirtualKey.Control:
                    this.KeyCtrl = true;
                    break;

                case VirtualKey.Space:
                    this.KeyAlt = true;
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
                case VirtualKey.Shift:
                    this.KeyShift = false;
                    break;

                case VirtualKey.Control:
                    this.KeyCtrl = false;
                    break;

                case VirtualKey.Space:
                    this.KeyAlt = false;
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


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        /// <summary> keyboard's the **SHIFT** key. </summary>
        public bool KeyShift
        {
            get => this.keyShift;
            set
            {
                this.KeyIsRatio = value;

                this.keyShift = value;
                this.OnPropertyChanged(nameof(this.KeyShift));//Notify 
            }
        }
        private bool keyShift;

        /// <summary> keyboard's the **CTRL** key. </summary>
        public bool KeyCtrl
        {
            get => this.keyCtrl;
            set
            {
                this.KeyIsCenter = value;
                ///////////////////////////////////////////////
                this.keyCtrl = value;
                this.OnPropertyChanged(nameof(this.KeyCtrl));//Notify 
            }
        }
        private bool keyCtrl;

        /// <summary> keyboard's the **ALT** key. </summary>
        public bool KeyAlt
        {
            get => this.keyAlt;
            set
            {
                this.KeyIsStepFrequency = value;
                this.KeyIsMove = value;

                this.keyAlt = value;
                this.OnPropertyChanged(nameof(this.KeyAlt));//Notify 
            }
        }
        private bool keyAlt;


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary> Scaling around the center (<see cref = "ViewModel.Selection" />). </summary>
        public bool KeyIsCenter
        {
            get => this.keyIsCenter;
            set
            {
                this.keyIsCenter = value;
                this.OnPropertyChanged(nameof(this.KeyIsCenter));//Notify 
            }
        }
        private bool keyIsCenter;

        /// <summary> Maintain a ratio when scaling (<see cref = "ViewModel.Selection" />). </summary>
        public bool KeyIsRatio
        {
            get => this.keyIsRatio;
            set
            {
                this.keyIsRatio = value;
                this.OnPropertyChanged(nameof(this.KeyIsRatio));//Notify 
            }
        }
        private bool keyIsRatio;

        /// <summary> Step Frequency when spinning (<see cref = "ViewModel.Selection" />). </summary>
        public bool KeyIsStepFrequency
        {
            get => this.keyIsStepFrequency;
            set
            {
                this.keyIsStepFrequency = value;
                this.OnPropertyChanged(nameof(this.KeyIsStepFrequency));//Notify 
            }
        }
        private bool keyIsStepFrequency;


        /// <summary> Change <see cref = "ViewModel.CanvasTransformer" />'s Position when moving. </summary>
        public bool KeyIsMove
        {
            get => this.keyIsMove;
            set
            {
                this.keyIsMove = value;
                this.OnPropertyChanged(nameof(this.KeyIsMove));//Notify 
            }
        }
        private bool keyIsMove;


    }
}