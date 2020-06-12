using FanKit.Transformers;
using System;
using System.ComponentModel;
using Windows.System;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents an ViewModel that contains shortcut, layout and <see cref="ViewModels.Setting"/>.
    /// </summary>
    public partial class SettingViewModel : INotifyPropertyChanged
    {

        //@Delegate  
        /// <summary> Occurs when the canvas position moved. </summary>
        public Action<MoveMode> Move { get; set; }
        /// <summary> Occurs when the conext edited. </summary>
        public Action<EditMode> Edit { get; set; }


        /// <summary> Whether <see cref="SettingViewModel.ConstructKey"/> is available. </summary>
        public bool KeyIsEnabled = true;


        //@Construct
        /// <summary>
        /// Initializes the key.
        /// </summary>
        public void ConstructKey()
        {
            Window.Current.CoreWindow.KeyUp += (s, e) =>
            {
                if (this.KeyIsEnabled == false) return;

                VirtualKey key = e.VirtualKey;
                this.KeyUp(key);
                this.KeyUpAndDown(key);
            };
            Window.Current.CoreWindow.KeyDown += (s, e) =>
            {
                if (this.KeyIsEnabled == false) return;

                VirtualKey key = e.VirtualKey;
                this.KeyDown(key);
                this.KeyUpAndDown(key);
            };
        }

        private void KeyDown(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.Shift: this.SetKeyShift(true); break;
                case VirtualKey.Control: this.SetKeyCtrl(true); break;
                case VirtualKey.Space: this.SetKeyAlt(true); break;


                case VirtualKey.Escape: this.SetKeyEscape(true); break;


                case VirtualKey.Left: this.SetKeyMove(MoveMode.Left); break;
                case VirtualKey.Up: this.SetKeyMove(MoveMode.Up); break;
                case VirtualKey.Right: this.SetKeyMove(MoveMode.Right); break;
                case VirtualKey.Down: this.SetKeyMove(MoveMode.Down); break;


                case VirtualKey.X: this.SetKeyEdit(EditMode.Cut); break;
                case VirtualKey.J: this.SetKeyEdit(EditMode.Duplicate); break;
                case VirtualKey.C: this.SetKeyEdit(EditMode.Copy); break;
                case VirtualKey.V: this.SetKeyEdit(EditMode.Paste); break;
                case VirtualKey.Delete: this.SetKeyEditClear(true); break;

                case VirtualKey.A: this.SetKeyEdit(EditMode.All); break;
                case VirtualKey.D: this.SetKeyEdit(EditMode.Deselect); break;
                case VirtualKey.I: this.SetKeyEdit(EditMode.Invert); break;

                case VirtualKey.G: this.SetKeyEdit(EditMode.Group); break;
                case VirtualKey.U: this.SetKeyEdit(EditMode.UnGroup); break;
                case VirtualKey.R: this.SetKeyEdit(EditMode.Release); break;

                //case VirtualKey.O: this.SetKeyEdit(EditMode.Union); break;
                //case VirtualKey.E: this.SetKeyEdit(EditMode.Exclude); break;
                //case VirtualKey.X: this.SetKeyEdit(EditMode.Xor); break;
                //case VirtualKey.I: this.SetKeyEdit(EditMode.Intersect); break;
                //case VirtualKey.S: this.SetKeyEdit(EditMode.ExpandStroke); break;

                case VirtualKey.Z: this.SetKeyEdit(EditMode.Undo); break;
                case VirtualKey.Y: this.SetKeyEdit(EditMode.Redo); break;


                default: break;
            }
        }

        private void KeyUp(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.Shift: this.SetKeyShift(false); break;
                case VirtualKey.Control: this.SetKeyCtrl(false); break;
                case VirtualKey.Space: this.SetKeyAlt(false); break;

                case VirtualKey.Escape: this.SetKeyEscape(false); break;

                case VirtualKey.Left:
                case VirtualKey.Up:
                case VirtualKey.Right:
                case VirtualKey.Down:
                    this.SetKeyMove(MoveMode.None);
                    break;


                case VirtualKey.X: 
                case VirtualKey.J: 
                case VirtualKey.C: 
                case VirtualKey.V: 

                case VirtualKey.A: 
                case VirtualKey.D:
                case VirtualKey.I: 

                case VirtualKey.G: 
                case VirtualKey.U: 
                case VirtualKey.R: 

                //case VirtualKey.O: 
                //case VirtualKey.E: 
                //case VirtualKey.X:
                //case VirtualKey.I: 
                //case VirtualKey.S:

                case VirtualKey.Z: 
                case VirtualKey.Y:
                    this.SetKeyEdit(EditMode.None);
                    break;

                case VirtualKey.Delete: this.SetKeyEditClear(false); break;


                default: break;
            }
        }

        private void KeyUpAndDown(VirtualKey key)
        {
            if (this.KeyCtrl == false && this.KeyShift == false)
                this.CompositeMode = MarqueeCompositeMode.New;//CompositeMode
            else if (this.KeyCtrl == false && this.KeyShift)
                this.CompositeMode = MarqueeCompositeMode.Add;//CompositeMode
            else if (this.KeyCtrl && this.KeyShift == false)
                this.CompositeMode = MarqueeCompositeMode.Subtract;//CompositeMode
            else //if (this.KeyCtrl && this.KeyShift)       
                this.CompositeMode = MarqueeCompositeMode.Intersect;//CompositeMode
        }
        

        /// <summary> keyboard's the **SHIFT** key. </summary>
        public bool KeyShift;
        private void SetKeyShift(bool value)
        {
            if (this.KeyShift == value) return;

            this.KeyShift = value;
            this.OnPropertyChanged(nameof(this.KeyShift));//Notify 

            //Key
            {
                this.IsRatio = value;
                this.IsSquare = value;
            }
        }
        
        /// <summary> keyboard's the **CTRL** key. </summary>
        public bool KeyCtrl;
        private void SetKeyCtrl(bool value)
        {
            if (this.KeyCtrl == value) return;

            this.KeyCtrl = value;
            this.OnPropertyChanged(nameof(this.KeyCtrl));//Notify 

            //Key
            {
                this.IsCenter = value;
            }
        }
                        
        /// <summary> keyboard's the **ALT** key. </summary>
        public bool KeyAlt;
        private void SetKeyAlt(bool value)
        {
            if (this.KeyAlt == value) return;
            
            this.KeyAlt = value;
            this.OnPropertyChanged(nameof(this.KeyAlt));//Notify 

            //Key
            {
                this.IsStepFrequency = value;
            }
        }
        
        /// <summary> keyboard's the **Escape** key. </summary>
        public bool KeyEscape;
        private void SetKeyEscape(bool value)
        {
            if (this.KeyEscape == value) return;

            this.KeyEscape = value;
            this.OnPropertyChanged(nameof(this.KeyEscape));//Notify 


            //Key
            {
                if (value==false)
                {
                    this.IsFullScreen = !IsFullScreen;
                }
            }
        }
        

        /// <summary> keyboard's the **Left Up Right Down** key. </summary>
        public MoveMode MoveMode;
        private void SetKeyMove(MoveMode value)
        {
            if (this.MoveMode == value) return;

            this.MoveMode = value;
            this.Move?.Invoke(value);//Delegate
        }
        

        /// <summary> keyboard's the **ABCD...** key. </summary>
        public EditMode EditMode;
        private void SetKeyEdit(EditMode value)
        {
            if (value!= EditMode.None)
            {
                if (this.KeyCtrl == false) return;
                if (this.EditMode == value) return;

                this.EditMode = value;
                this.Edit?.Invoke(value);//Delegate
            }
            else
            {
                if (this.EditMode == EditMode.None) return;

                this.EditMode = EditMode.None;
            }
        }
        private void SetKeyEditClear(bool isClear)
        {
            if (isClear)
            {
                if (this.EditMode == EditMode.Clear) return;

                this.EditMode = EditMode.Clear;
                this.Edit?.Invoke(EditMode.Clear);//Delegate
            }
            else
            {
                if (this.EditMode == EditMode.None) return;

                this.EditMode = EditMode.None;
            }   
        }

    }
}