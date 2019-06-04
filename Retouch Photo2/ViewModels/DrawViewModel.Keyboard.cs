using Retouch_Photo2.Library;
using Retouch_Photo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;

namespace Retouch_Photo2.ViewModels
{
    public partial class DrawViewModel 
    {

        private bool keyShift;
        public bool KeyShift
        {
            get => this.keyShift;
            set
            {
                this.keyShift = value;
                this.OnPropertyChanged(nameof(this.KeyShift));//Notify 
            }
        }

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


        public void KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Control:
                    this.KeyCtrl = true;
                    break;

                case VirtualKey.Shift:
                    this.KeyShift = true;
                    break;

                case VirtualKey.Delete:
                    Layer layer = this.Layer;
                    if (layer != null)
                    {
                        this.RenderLayer.Remove(layer);
                        this.Tool.ToolOnNavigatedTo();
                    }
                    this.SetLayer(null);
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
                    this.KeyCtrl = false;
                    break;

                case VirtualKey.Shift:
                    this.KeyShift = false;
                    break;

                default:
                    break;
            }
            this.KeyUpAndDown(sender, args);
        }


        public void KeyUpAndDown(CoreWindow sender, KeyEventArgs args)
        {
            if (this.KeyCtrl == false && this.KeyShift == false)
                this.MarqueeMode = MarqueeMode.None;
            else if (this.KeyCtrl == false && this.KeyShift)
                this.MarqueeMode = MarqueeMode.Square;
            else if (this.KeyCtrl && this.KeyShift == false)
                this.MarqueeMode = MarqueeMode.Center;
            else //if (this.KeyCtrl && this.KeyShift)
                this.MarqueeMode = MarqueeMode.SquareAndCenter;
        }
        
    }
}
