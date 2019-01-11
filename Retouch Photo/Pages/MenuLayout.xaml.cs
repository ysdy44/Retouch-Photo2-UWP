using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Text;
using Windows.Foundation.Metadata;
using System.Globalization;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Retouch_Photo.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Retouch_Photo.ViewModels;

namespace Retouch_Photo.Pages
{
    public sealed partial class MenuLayout : UserControl
    {
        //Postion
        Vector2 ControlSize;

        private Vector2 postion;
        public Vector2 Postion
        {
            get => this.postion;
            set
            {
                float X;
                if (value.X > (App.ViewModel.MatrixTransformer.ControlWidth - this.ControlSize.X)) X = (App.ViewModel.MatrixTransformer.ControlWidth - this.ControlSize.X);
                else if (value.X < 0) X = 0;
                else X = value.X;

                float Y;
                if (value.Y > (App.ViewModel.MatrixTransformer.ControlHeight - this.ControlSize.Y)) Y = (App.ViewModel.MatrixTransformer.ControlHeight - this.ControlSize.Y);
                else if (value.Y < 0) Y = 0;
                else Y = value.Y;

                Canvas.SetLeft(this, X);
                Canvas.SetTop(this, Y);

                postion.X = X;
                postion.Y = Y;
            }
        }


        //Label
        private bool label = true;
        public bool Label
        {
            get => label;
            set
            {
                this.LabelIcon.Glyph = value ? "\uE141" : "\uE196";
                this.ContentPanel.Visibility = value ? Visibility.Visible : Visibility.Collapsed;

                label = value;
            }
        }

        //Layout
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        public UIElement CenterContent { get => this.ContentPanel.Child; set => this.ContentPanel.Child = value; }

        public MenuLayout()
        {
            this.InitializeComponent();
            this.Loaded += (sender, e) => this.Label = true;
        
            //Postion
            this.SizeChanged += (sender, e) => this.ControlSize = e.NewSize.ToVector2();

            this.TitlePanel.ManipulationMode = ManipulationModes.All;
            this.TitlePanel.ManipulationStarted += (sender, e) => this.Postion = new Vector2((float)Canvas.GetLeft(this), (float)Canvas.GetTop(this));
            this.TitlePanel.ManipulationDelta += (sender, e) => this.Postion = new Vector2(this.postion.X + (float)e.Delta.Translation.X, this.postion.Y + (float)e.Delta.Translation.Y);
            this.TitlePanel.ManipulationCompleted += (sender, e) => { };

            //Label
            this.LabelButton.Tapped+=(sender, e) => this.Label = !this.Label;
        }
    }
}
