using Retouch_Photo2.Brushs;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Layouts
{
    public sealed partial class ColorLayout : UserControl, IMenuLayout
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public MenuState State
        {
            set
            {
                this.MenuTitle.State = value;
                this.ColorPicker.Visibility = (value == MenuState.OverlayNotExpanded) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public FrameworkElement Self => this;

        public UIElement StateButton => this.MenuTitle.StateButton;
        public UIElement CloseButton => this.MenuTitle.CloseButton;
        public UIElement TitlePanel => this.MenuTitle.RootGrid;
               

        //@Construct
        public ColorLayout()
        {
            this.InitializeComponent();

            this.ColorPicker.ColorChange += (s, value) =>
            {
                this.SelectionViewModel.Color = value;

                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.SelectionViewModel.FillColor = value;
                        break;
                    case FillOrStroke.Stroke:
                        this.SelectionViewModel.StrokeColor = value;
                        break;
                }

                if (this.SelectionViewModel.Mode == ListViewSelectionMode.None) return;

                this.SelectionViewModel.BrushType = BrushType.Color;

                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        {
                            //Selection
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.FillColor = value;
                            }, true);
                        }
                        break;
                    case FillOrStroke.Stroke:
                        {
                            //Selection
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.StrokeColor = value;
                            }, true);
                        }
                        break;
                }

                this.ViewModel.Invalidate();//Invalidate
            };

        }
    }
}