using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Pages
{
    public sealed partial class MenuLayout : UserControl
    {
        //Postion
        Size ControlSize;
               
        private Vector2 postion;
        public Vector2 Postion
        {
            get => this.postion;
            set
            {
                Canvas.SetLeft(this, (value.X > (App.ViewModel.MatrixTransformer.ControlWidth - this.ControlSize.Width)) ? (App.ViewModel.MatrixTransformer.ControlWidth - this.ControlSize.Width) : (value.X < 0) ? 0 : value.X);
                Canvas.SetTop(this, (value.Y > (App.ViewModel.MatrixTransformer.ControlHeight - this.ControlSize.Height)) ? (App.ViewModel.MatrixTransformer.ControlHeight - this.ControlSize.Height) : (value.Y < 0) ? 0 : value.Y);
            }
        }


        //Label
        private bool label;
        public bool Label
        {
            get => label;
            set
            {
                this.LabelIcon.Glyph = value ? "\uE141" : "\uE196";
                this.ContentBorderl.Visibility = value ? Visibility.Visible : Visibility.Collapsed;

                label = value;
            }
        }

        //Content
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        public UIElement CenterContent { get => this.ContentBorderl.Child; set => this.ContentBorderl.Child = value; }


        public MenuLayout()
        {
            this.InitializeComponent();
            this.Loaded += (sender, e) => this.Label = false;
            this.SizeChanged += (sender, e) => this.ControlSize = e.NewSize;

            //Postion 
            this.TitlePanel.ManipulationMode = ManipulationModes.All;
            this.TitlePanel.ManipulationStarted += (sender, e) => this.postion = new Vector2((float)Canvas.GetLeft(this), (float)Canvas.GetTop(this));
            this.TitlePanel.ManipulationDelta += (sender, e) => this.Postion = this.postion += e.Delta.Translation.ToVector2();
            this.TitlePanel.ManipulationCompleted += (sender, e) => { };

            //Label
            this.LabelButton.Tapped+=(sender, e) => this.Label = !this.Label;
        }

        
        //Content
        private void ContentBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.ContentRectangleFrameHeight.Value = e.NewSize.Height;
            this.ContentRectangleStoryboard.Begin();
        }
    }
}
