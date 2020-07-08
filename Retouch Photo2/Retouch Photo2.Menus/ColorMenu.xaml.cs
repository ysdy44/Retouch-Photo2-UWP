using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "HSVColorPickers.ColorPicker"/>.
    /// </summary>
    public sealed partial class ColorMenu : Expander, IMenu 
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        ColorMainPage ColorMainPage = new ColorMainPage();


        //@Construct
        /// <summary>
        /// Initializes a ColorMenu. 
        /// </summary>
        public ColorMenu()
        {
            this.InitializeComponent();
            this.Button.CenterContent = new ColorEllipse
            (
                 dataContext: this.SelectionViewModel,
                 path: nameof(this.SelectionViewModel.Color),
                 dp: ColorEllipse.ColorProperty
            );
            this.ConstructStrings();

            this.MainPage = this.ColorMainPage;
        }

    }

    /// <summary>
    /// Menu of <see cref = "HSVColorPickers.ColorPicker"/>.
    /// </summary>
    public sealed partial class ColorMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            base.Title = resource.GetString("/Menus/Color");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Transformer;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton();
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }



    /// <summary>
    /// MainPage of <see cref="ColorMenu"/>.
    /// </summary>
    public sealed partial class ColorMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Construct
        /// <summary>
        /// Initializes a ColorMainPage. 
        /// </summary>
        public ColorMainPage()
        {
            this.InitializeComponent();

            this.ConstructColor1();
            this.ConstructColor2();
        }


        private void ConstructColor1()
        {
            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            if (this.ColorPicker.HexPicker is TextBox textBox)
            {
                //textBox.IsEnabled = false;
                //this.ColorFlyout.Opened += (s, e) => textBox.IsEnabled = true;
                //this.ColorFlyout.Closed += (s, e) => textBox.IsEnabled = false;
                textBox.GotFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = false;
                textBox.LostFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = true;
            }

            this.ColorPicker.ColorChanged += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.MethodViewModel.MethodFillColorChanged(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.MethodViewModel.MethodStrokeColorChanged(value);
                        break;
                }
            };
        }

        private void ConstructColor2()
        {
            //Color
            this.ColorPicker.ColorChangeStarted += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.MethodViewModel.MethodFillColorChangeStarted(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.MethodViewModel.MethodStrokeColorChangeStarted(value);
                        break;
                }
            };
            this.ColorPicker.ColorChangeDelta += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.MethodViewModel.MethodFillColorChangeDelta(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.MethodViewModel.MethodStrokeColorChangeDelta(value);
                        break;
                }
            };
            this.ColorPicker.ColorChangeCompleted += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.MethodViewModel.MethodFillColorChangeCompleted(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.MethodViewModel.MethodStrokeColorChangeCompleted(value);
                        break;
                }
            };
        }

    }
}