using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using System;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Retouch_Photo2's the only <see cref = "ColorMenu" />. 
    /// </summary>
    public sealed partial class ColorMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;


        //@Construct
        public ColorMenu()
        {
            this.InitializeComponent();
            this._button.CenterContent = new ColorEllipse
             (
                  dataContext: this.ViewModel,
                  path: nameof(this.ViewModel.Color),
                  dp: ColorEllipse.ColorProperty
             );
            this.ConstructStrings();
            this.ConstructMenu();

            this.ConstructColor1();
            this.ConstructColor2();
        }



        private void ConstructColor1()
        {
            this.ColorPicker.ColorChanged += (s, value) =>
            {
                switch (this.ViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.ViewModel.MethodFillColorChanged(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.ViewModel.MethodStrokeColorChanged(value);
                        break;
                }
            };
        }

        private void ConstructColor2()
        {
            //Color
            this.ColorPicker.ColorChangeStarted += (s, value) =>
            {
                switch (this.ViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.ViewModel.MethodFillColorChangeStarted(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.ViewModel.MethodStrokeColorChangeStarted(value);
                        break;
                }
            };
            this.ColorPicker.ColorChangeDelta += (s, value) =>
            {
                switch (this.ViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.ViewModel.MethodFillColorChangeDelta(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.ViewModel.MethodStrokeColorChangeDelta(value);
                        break;
                }
            };
            this.ColorPicker.ColorChangeCompleted += (s, value) =>
            {
                switch (this.ViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.ViewModel.MethodFillColorChangeCompleted(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.ViewModel.MethodStrokeColorChangeCompleted(value);
                        break;
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

            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/Color");
        }

        //Menu
        public MenuType Type => MenuType.Transformer;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton();

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }
}
