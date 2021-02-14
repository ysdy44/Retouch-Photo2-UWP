using System;
using Windows.System;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Specifies the virtual key used to modify another key. 
    /// For example, 
    /// when the Ctrl key is pressed at the same time as the other keys, such as Ctrl-C.
    /// </summary>
    [Flags]
    public enum VirtualKeyModifiers2 : uint
    {
        /// <summary> No virtual keying modifier. </summary>
        None = 0,
        /// <summary> Control virtual key. </summary>
        Control = 1,
        /// <summary> Space virtual key. </summary>
        Space = 2,
        /// <summary> Shift virtual key. </summary>
        Shift = 4,
    }


    /// <summary>
    /// Represents a keyboard shortcut (or accelerator) 
    /// that allows users to perform actions 
    /// using the keyboard instead of navigating the app UI directly or through access keys.
    /// </summary>
    public class KeyboardAccelerator2
    {

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the group value.
        /// </summary>
        public int Group { get; set; }

        /// <summary>
        /// Gets or sets a virtual key for another key that modifies the keyboard shortcut (accelerator).
        /// </summary>
        public VirtualKeyModifiers2 Modifiers { get; set; } = VirtualKeyModifiers2.None;

        /// <summary>
        /// Get or set a virtual key for a keyboard shortcut (accelerator) (used in conjunction with one or more modifier keys).
        /// </summary>
        public VirtualKey Key { get; set; } = VirtualKey.None;

        /// <summary>
        /// Gets or sets whether a keyboard shortcut (accelerator) is available to the user.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// This KeyboardAccelerator key combination occurs when you press it.
        /// </summary>
        public Action Invoked;

        public override string ToString()
        {
            switch (this.Modifiers)
            {
                case VirtualKeyModifiers2.Control: return $"Ctrl + {this.ToStr()}";
                case VirtualKeyModifiers2.Shift: return $"Shift + {this.ToStr()}";
                default: return this.ToStr();
            }
        }
        private string ToStr()
        {
            switch (this.Key)
            {
                case VirtualKey.Left: return "←";
                case VirtualKey.Up: return "↑";
                case VirtualKey.Right: return "→";
                case VirtualKey.Down: return "↓";
                case VirtualKey.Delete: return "Delete";
                case VirtualKey.Escape: return "Esc";
                default: return $"{this.Key}";
            }
        }


    }
}