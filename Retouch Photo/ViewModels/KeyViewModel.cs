using Retouch_Photo.Library;
using Retouch_Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;

namespace Retouch_Photo.ViewModels
{
    public class KeyViewModel
    {

        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        
        public void KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Control:
                    this.ViewModel.KeyCtrl = true;
                    break;

                case VirtualKey.Shift:
                    this.ViewModel.KeyShift = true;
                    break;

                case VirtualKey.Delete:
                    Layer layer = this.ViewModel.CurrentLayer;
                    if (layer != null)
                    {
                        this.ViewModel.RenderLayer.Remove(layer);
                        this.ViewModel.Tool.ViewModel.ToolOnNavigatedTo();
                    }
                    this.ViewModel.CurrentLayer = null;
                    break;

                default:
                    break;
            }
            this.KeyUpAndDown(sender, args);
        }


        public void KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Control:
                    this.ViewModel.KeyCtrl = false;
                    break;

                case VirtualKey.Shift:
                    this.ViewModel.KeyShift = false;
                    break;

                default:
                    break;
            }
            this.KeyUpAndDown(sender, args);
        }


        public void KeyUpAndDown(CoreWindow sender, KeyEventArgs args)
        {
            if (this.ViewModel.KeyCtrl == false && this.ViewModel.KeyShift == false)
                this.ViewModel.MarqueeMode = MarqueeMode.None;
            else if (this.ViewModel.KeyCtrl == false && this.ViewModel.KeyShift)
                this.ViewModel.MarqueeMode = MarqueeMode.Square;
            else if (this.ViewModel.KeyCtrl && this.ViewModel.KeyShift == false)
                this.ViewModel.MarqueeMode = MarqueeMode.Center;
            else //if (this.ViewModel.KeyCtrl && this.ViewModel.KeyShift)
                this.ViewModel.MarqueeMode = MarqueeMode.SquareAndCenter;
        }
        
    }
}
