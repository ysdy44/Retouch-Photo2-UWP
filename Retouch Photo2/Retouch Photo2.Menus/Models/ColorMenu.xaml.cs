using Retouch_Photo2.Brushs;
using Retouch_Photo2.ViewModels;
using System;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Retouch_Photo2's the only <see cref = "ColorMenu" />. 
    /// </summary>
    public sealed partial class ColorMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Construct
        public ColorMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();

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

                if (this.SelectionViewModel.SelectionMode != ListViewSelectionMode.None)
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



    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TransformerMenu" />. 
    /// </summary>
    public sealed partial class ColorMenu : UserControl, IMenu
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Menus/Color");
            this._Expander.Title = resource.GetString("/Menus/Color");
        }

        //@Delegate
        public Action Move { get; set; }
        public Action Closed { get; set; }
        public Action Opened { get; set; }


        //@Content
        public MenuType Type => MenuType.Transformer;
        public FlyoutPlacementMode PlacementMode { get; set; } = FlyoutPlacementMode.Bottom;
        public Point Postion { get; set; }
        public FrameworkElement Layout => this;
        public FrameworkElement Button => this._button;
        private MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Menus.Icons.ColorIcon()
        };
        
        public MenuState State
        {
            get => this.state;
            set
            {
                this._button.State = value;
                this._Expander.State = value;
                MenuHelper.SetMenuState(value, this);
                this.state = value;
            }
        }
        private MenuState state;


        //@Construct  
        public void ConstructMenu()
        {
            this.State = MenuState.Hide;
            this.Button.Tapped += (s, e) => this.State = MenuHelper.GetState(this.State);
            this._Expander.CloseButton.Tapped += (s, e) => this.State = MenuState.Hide;
            this._Expander.StateButton.Tapped += (s, e) => this.State = MenuHelper.GetState2(this.State);
            MenuHelper.ConstructTitleGrid(this._Expander.TitleGrid, this);
        }


    }
}
