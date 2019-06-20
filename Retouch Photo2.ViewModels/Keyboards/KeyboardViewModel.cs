using Windows.System;
using Windows.UI.Xaml;
using System.ComponentModel;
using Retouch_Photo2.Elements;

namespace Retouch_Photo2.ViewModels.Keyboards
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "KeyboardViewModel" />. 
    /// </summary>
    public partial class KeyboardViewModel : INotifyPropertyChanged
    {

        //@Construct
        public KeyboardViewModel()
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
                    this.SetKeyShift(true);
                    this.SetKeyShift(true);
                    break;
                case VirtualKey.Control:
                    this.SetKeyCtrl(true);
                    break;
                case VirtualKey.Space:
                    this.SetKeyAlt(true);
                    break;

                case VirtualKey.Delete:
                    break;

                case VirtualKey.Escape:
                    this.IsFullScreen = !this.IsFullScreen;
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
                    this.SetKeyShift(false);
                    break;
                case VirtualKey.Control:
                    this.SetKeyCtrl(false);
                    break;
                case VirtualKey.Space:
                    this.SetKeyAlt(false);
                    break;

                default:
                    break;
            }
        }

        private void KeyUpAndDown(VirtualKey key)
        {
            if (this.KeyCtrl == false && this.KeyShift == false)
                this.CompositeMode = CompositeMode.New;//CompositeMode
            else if (this.KeyCtrl == false && this.KeyShift)
                this.CompositeMode = CompositeMode.Add;//CompositeMode
            else if (this.KeyCtrl && this.KeyShift == false)
                this.CompositeMode = CompositeMode.Subtract;//CompositeMode
            else //if (this.KeyCtrl && this.KeyShift)       
                this.CompositeMode = CompositeMode.Intersect;//CompositeMode
        }



        /// <summary> keyboard's the **SHIFT** key. </summary>
        public bool KeyShift;
        private void SetKeyShift(bool value)
        {
            this.IsRatio = value;

            this.KeyShift = value;
            this.OnPropertyChanged(nameof(this.KeyShift));//Notify 
        }


        /// <summary> keyboard's the **CTRL** key. </summary>
        public bool KeyCtrl;
        private void SetKeyCtrl(bool value)
        {
            this.IsCenter = value;

            this.KeyCtrl = value;
            this.OnPropertyChanged(nameof(this.KeyCtrl));//Notify 
        }




        /// <summary> keyboard's the **ALT** key. </summary>
        public bool KeyAlt;
        private void SetKeyAlt(bool value)
        {
            this.IsStepFrequency = value;

            this.KeyAlt = value;
            this.OnPropertyChanged(nameof(this.KeyAlt));//Notify 
        }
                    


        //Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="name"> Name of the property used to notify listeners. </param>
        protected void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}