using System.Numerics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Library;
using Retouch_Photo.Models;
using Retouch_Photo.Controls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.Controls.ToolControls;
using Microsoft.Graphics.Canvas.Effects;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;
using Retouch_Photo.ViewModels.ToolViewModels;
using Microsoft.Graphics.Canvas.UI;
using Windows.System;
using Windows.UI.Core;
using Retouch_Photo.Models.Tools;
namespace Retouch_Photo.Library
{
    public class ShortCutKey
    {
        //delegate
        public delegate void KeyEventHandler2();
        public event KeyEventHandler2 KeyDown;
        public event KeyEventHandler2 KeyUp;
        public event KeyEventHandler2 KeyDownOrUp;


        public ShortCutKey( )
        {      
            Window.Current.Dispatcher.AcceleratorKeyActivated += this.TypedEventHandler;
        }


        public static bool IsKeyDown(VirtualKey virtualKey) => Window.Current.CoreWindow.GetKeyState(virtualKey).HasFlag(CoreVirtualKeyStates.Down);
        public static bool IsKeyUp(VirtualKey virtualKey) => Window.Current.CoreWindow.GetKeyState(virtualKey).HasFlag(CoreVirtualKeyStates.None);


        int bb = 0;
        bool Down = false;
        public void TypedEventHandler(CoreDispatcher core, AcceleratorKeyEventArgs args)
        {
            string s = args.EventType.ToString();
            bool isDown = s.Contains("Down");
            bool isUp = s.Contains("Up");

            if (isDown)
            {
                this.Down = true;
                this.KeyDown?.Invoke();
            }

            if (isUp)
            {
                if (this.Down == true)
                {
                    this.KeyUp?.Invoke();
                    this.Down = false;
                }
            }

            if (isDown||isUp)
            {
                this.KeyDownOrUp?.Invoke();
            }

        }



    }
}
