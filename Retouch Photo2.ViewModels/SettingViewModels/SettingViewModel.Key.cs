using FanKit.Transformers;
using Retouch_Photo2.Elements;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    public partial class SettingViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the all KeyboardAccelerator. </summary>   
        public IList<KeyboardAccelerator2> KeyboardAccelerators { get; set; }


        //@Construct
        /// <summary>
        /// Unregiste the key.
        /// </summary>
        public void UnregisteKey()
        {
            //@Focus 
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            Window.Current.CoreWindow.KeyUp -= this.CoreWindow_KeyUp;
            Window.Current.CoreWindow.KeyDown -= this.CoreWindow_KeyDown;
        }
        /// <summary>
        /// Registe the key.
        /// </summary>
        public void RegisteKey()
        {
            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            Window.Current.CoreWindow.KeyUp += this.CoreWindow_KeyUp;
            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;
        }


        private void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs e)
        {
            if (BackRequestedExtension.DialogIsShow) return;
            if (BackRequestedExtension.LayoutIsShow) return;

            switch (e.VirtualKey)
            {
                case VirtualKey.Shift: if (this.KeyShift) this.KeyShift = this.IsRatio = this.IsSquare = false; break;
                case VirtualKey.Space: if (this.KeySpace) this.KeySpace = this.IsSnapToTick = this.IsWheelToRotate = false; break;
                case VirtualKey.Control: if (this.KeyCtrl) this.KeyCtrl = this.IsCenter = false; break;
                default: break;
            }
            this.KeyUpAndDown();


            if (this.KeyboardAccelerators != null)
            {
                foreach (KeyboardAccelerator2 key in this.KeyboardAccelerators)
                {
                    if (key.IsEnabled == false && e.VirtualKey == key.Key)
                    {
                        key.IsEnabled = true;
                    }
                }
            }
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            if (BackRequestedExtension.DialogIsShow) return;
            if (BackRequestedExtension.LayoutIsShow) return;

            switch (e.VirtualKey)
            {
                case VirtualKey.Shift: if (this.KeyShift == false) this.KeyShift = this.IsRatio = this.IsSquare = true; break;
                case VirtualKey.Space: if (this.KeySpace == false) this.KeySpace = this.IsSnapToTick = this.IsWheelToRotate = true; break;
                case VirtualKey.Control: if (this.KeyCtrl == false) this.KeyCtrl = this.IsCenter = true; break;
                default: break;
            }
            this.KeyUpAndDown();


            if (this.KeyboardAccelerators != null)
            {
                foreach (KeyboardAccelerator2 key in this.KeyboardAccelerators)
                {
                    if (key.IsEnabled && e.VirtualKey == key.Key)
                    {
                        switch (key.Modifiers)
                        {
                            case VirtualKeyModifiers2.None: key.IsEnabled = false; key.Invoked?.Invoke(); break;
                            case VirtualKeyModifiers2.Shift: if (this.KeyShift) { key.IsEnabled = false; key.Invoked?.Invoke(); } break;
                            case VirtualKeyModifiers2.Space: if (this.KeySpace) { key.IsEnabled = false; key.Invoked?.Invoke(); } break;
                            case VirtualKeyModifiers2.Control: if (this.KeyCtrl) { key.IsEnabled = false; key.Invoked?.Invoke(); } break;
                            default: break;
                        }
                    }
                }
            }
        }



        private void KeyUpAndDown()
        {
            if (this.KeyCtrl == false && this.KeyShift == false)
            {
                this.CompositeMode = MarqueeCompositeMode.New;//CompositeMode
                this.ControlPointMode = SelfControlPointMode.None;//ControlPointMode 
            }
            else if (this.KeyCtrl == false && this.KeyShift)
            {
                this.CompositeMode = MarqueeCompositeMode.Add;//CompositeMode
                this.ControlPointMode = SelfControlPointMode.Angle;//ControlPointMode 
            }
            else if (this.KeyCtrl && this.KeyShift == false)
            {
                this.CompositeMode = MarqueeCompositeMode.Subtract;//CompositeMode
                this.ControlPointMode = SelfControlPointMode.Length;//ControlPointMode 
            }
            else //if (this.KeyCtrl && this.KeyShift)       
            {
                this.CompositeMode = MarqueeCompositeMode.New;//CompositeMode
                this.ControlPointMode = SelfControlPointMode.None;//ControlPointMode 
            }
            //else //if (this.KeyCtrl && this.KeyShift)       
            {
                //this.CompositeMode = MarqueeCompositeMode.Intersect;//CompositeMode
            }
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
        private bool keyShift = false;

        /// <summary> keyboard's the **CTRL** key. </summary>
        public bool KeyCtrl
        {
            get => this.keyCtrl;
            set
            {
                this.keyCtrl = value;
                this.OnPropertyChanged(nameof(this.KeyCtrl));//Notify 
            }
        }
        private bool keyCtrl = false;

        /// <summary> keyboard's the **Space** key. </summary>
        public bool KeySpace
        {
            get => this.keySpace;
            set
            {
                this.keySpace = value;
                this.OnPropertyChanged(nameof(this.KeySpace));//Notify 
            }
        }
        private bool keySpace = false;

    }
}