using FanKit.Transformers;
using Retouch_Photo2.Edits;
using System;
using System.ComponentModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents an ViewModel that contains shortcut, layout and <see cref="ViewModels.Setting"/>.
    /// </summary>
    public partial class SettingViewModel : INotifyPropertyChanged
    {

        //@Delegate  
        /// <summary> Occurs when the canvas position moved. </summary>
        public Action<FlyoutPlacementMode> Move { get; set; }
        /// <summary> Occurs when the conext edited. </summary>
        public Action<EditType> Edit { get; set; }
        /// <summary> Occurs when the conext undoed. </summary>
        public Action<UndoType> Undo { get; set; }


        /// <summary> Whether <see cref="SettingViewModel.ConstructKey"/> is available. </summary>
        public bool KeyIsEnabled = true;


        //@Construct
        /// <summary>
        /// Initializes the key.
        /// </summary>
        public void ConstructKey()
        {
            Window.Current.CoreWindow.KeyDown += (s, e) =>
            {
                if (this.KeyIsEnabled == false) return;
                switch (e.VirtualKey)
                {
                    case VirtualKey.Shift: if (this.KeyShift == false) this.KeyShift = this.IsRatio = this.IsSquare = true; break;
                    case VirtualKey.Control: if (this.KeyCtrl == false) this.KeyCtrl = this.IsCenter = true; break;
                    case VirtualKey.Space: if (this.KeyAlt == false) this.KeyAlt = this.IsStepFrequency = true; break;

                    case VirtualKey.Escape: break;

                    case VirtualKey.Left: this.MoveType = FlyoutPlacementMode.Left; break;
                    case VirtualKey.Up: this.MoveType = FlyoutPlacementMode.Top; break;
                    case VirtualKey.Right: this.MoveType = FlyoutPlacementMode.Right; break;
                    case VirtualKey.Down: this.MoveType = FlyoutPlacementMode.Bottom; break;

                    case VirtualKey.X: if (this.KeyCtrl) this.EditType = EditType.Edit_Cut; break;
                    case VirtualKey.J: if (this.KeyCtrl) this.EditType = EditType.Edit_Duplicate; break;
                    case VirtualKey.C: if (this.KeyCtrl) this.EditType = EditType.Edit_Copy; break;
                    case VirtualKey.V: if (this.KeyCtrl) this.EditType = EditType.Edit_Paste; break;
                    case VirtualKey.Delete: this.EditType = EditType.Edit_Clear; break;
                    case VirtualKey.A: if (this.KeyCtrl) this.EditType = EditType.Select_All; break;
                    case VirtualKey.D: if (this.KeyCtrl) this.EditType = EditType.Select_Deselect; break;
                    case VirtualKey.I: if (this.KeyCtrl) this.EditType = EditType.Select_Invert; break;
                    case VirtualKey.G: if (this.KeyCtrl) this.EditType = EditType.Group_Group; break;
                    case VirtualKey.U: if (this.KeyCtrl) this.EditType = EditType.Group_UnGroup; break;
                    case VirtualKey.R: if (this.KeyCtrl) this.EditType = EditType.Group_Release; break;
                    //case VirtualKey.O: if (this.KeyCtrl) this.EditType = EditType.Combine_Union; break;
                    //case VirtualKey.E: if (this.KeyCtrl) this.EditType = EditType.Combine_Exclude; break;
                    //case VirtualKey.X: if (this.KeyCtrl) this.EditType = EditType.Combine_Xor; break;
                    //case VirtualKey.I: if (this.KeyCtrl) this.EditType = EditType.Combine_Intersect; break;
                    //case VirtualKey.S: if (this.KeyCtrl) this.EditType = EditType.Combine_ExpandStroke; break;

                    case VirtualKey.Z: if (this.KeyCtrl) this.UndoType = UndoType.Undo; break;
                    case VirtualKey.Y: if (this.KeyCtrl) this.UndoType = UndoType.Redo; break;

                    default: break;
                }

                this.KeyUpAndDown();
            };

            Window.Current.CoreWindow.KeyUp += (s, e) =>
            {
                if (this.KeyIsEnabled == false) return;
                switch (e.VirtualKey)
                {
                    case VirtualKey.Shift: this.KeyShift = this.IsRatio = this.IsSquare = false; break;
                    case VirtualKey.Control: this.KeyCtrl = this.IsCenter = false; break;
                    case VirtualKey.Space: this.KeyAlt = this.IsStepFrequency = false; break;

                    case VirtualKey.Escape: this.IsFullScreen = !this.IsFullScreen; break;

                    case VirtualKey.Left:
                    case VirtualKey.Up: 
                    case VirtualKey.Right: 
                    case VirtualKey.Down: this.MoveType = FlyoutPlacementMode.Full; break;

                    case VirtualKey.X: 
                    case VirtualKey.J: 
                    case VirtualKey.C: 
                    case VirtualKey.V: 
                    case VirtualKey.Delete: 
                    case VirtualKey.A:
                    case VirtualKey.D:
                    case VirtualKey.I: 
                    case VirtualKey.G:
                    case VirtualKey.U:
                    case VirtualKey.R: this.EditType = EditType.None; break;
                    //case VirtualKey.O: 
                    //case VirtualKey.E: 
                    //case VirtualKey.X:
                    //case VirtualKey.I: 
                    //case VirtualKey.S: 

                    case VirtualKey.Z: 
                    case VirtualKey.Y: this.EditType = EditType.None; break;

                    default: break;
                }

                this.KeyUpAndDown();
            };
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

        /// <summary> keyboard's the **ALT** key. </summary>
        public bool KeyAlt
        {
            get => this.keyAlt;
            set
            {
                this.keyAlt = value;
                this.OnPropertyChanged(nameof(this.KeyAlt));//Notify 
            }
        }
        private bool keyAlt = false;


        public FlyoutPlacementMode MoveType
        {
            get => this.moveType;
            set
            {
                if (this.moveType == value) return;

                this.moveType = value;
                this.Move?.Invoke(value);//Delegate
            }
        }
        private FlyoutPlacementMode moveType = FlyoutPlacementMode.Full;

        public EditType EditType
        {
            get => this.editType;
            set
            {
                if (this.editType == value) return;

                this.editType = value;
                this.Edit?.Invoke(value);//Delegate
            }
        }
        private EditType editType = EditType.None;

        public UndoType UndoType
        {
            get => this.undoType;
            set
            {
                if (this.undoType == value) return;

                this.undoType = value;
                this.Undo?.Invoke(value);//Delegate
            }
        }
        private UndoType undoType = UndoType.None;
        
    }
}