using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Construct
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
            this.ConstructMenu();
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

            this.Button.ToolTip.Content = resource.GetString("/Menus/Color");
            this.Expander.Title = resource.GetString("/Menus/Color");
        }

        //Menu
        public MenuType Type => MenuType.Transformer;
        public IExpanderButton Button { get; } = new MenuButton();
        public IExpander Expander => this._Expander;
        public ExpanderState State
        {
            set
            {
                this.Button.State = value;
                this.Expander.State = value;
            }
        }
        public FrameworkElement Self => this;

        public void ConstructMenu()
        {
            this._Expander.Button = this.Button.Self;

            this.Button.StateChanged += (state) => this.State = state;
            this.Expander.StateChanged += (state) => this.State = state;
        }
    }
}
