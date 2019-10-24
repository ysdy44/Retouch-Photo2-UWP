using Retouch_Photo2.Brushs;
using Retouch_Photo2.ViewModels;
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
                this._MenuTitle.State = value;
                this.ColorPicker.Visibility = (value == MenuState.OverlayNotExpanded) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public FrameworkElement Self => this;

        public UIElement StateButton => this._MenuTitle.StateButton;
        public UIElement CloseButton => this._MenuTitle.CloseButton;
        public UIElement TitlePanel => this._MenuTitle.RootGrid;
               

        //@Construct
        public ColorLayout()
        {
            this.InitializeComponent();

            this.ColorPicker.ColorChange += (s, value) =>
            {
                //FillOrStroke
                this.SelectionViewModel.Color = value;
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.SelectionViewModel.FillColor = value;
                        break;
                    case FillOrStroke.Stroke:
                        this.SelectionViewModel.StrokeColor = value;
                        break;
                }

                if (this.SelectionViewModel.SelectionMode!= ListViewSelectionMode.None)
                {
                    //Selection
                    this.SelectionViewModel.BrushType = BrushType.Color;
                    this.SelectionViewModel.SetValue((layer) =>
                    {
                        //FillOrStroke
                        switch (this.SelectionViewModel.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                layer.StyleManager.FillBrush.Type = BrushType.Color;
                                layer.StyleManager.FillBrush.Color = value;
                                break;
                            case FillOrStroke.Stroke:
                                layer.StyleManager.StrokeBrush.Type = BrushType.Color;
                                layer.StyleManager.StrokeBrush.Color = value;
                                break;
                        }
                    });

                    this.ViewModel.Invalidate();//Invalidate
                }
            };
        }
    }
}