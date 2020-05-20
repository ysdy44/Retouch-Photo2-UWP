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
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers;

namespace Retouch_Photo2.Menus.Models
{
    public sealed partial class StyleMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        //@Construct
        public StyleMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();

            this.Button.Click += (s, e) =>
            {
                this.ItemsControl.ItemsSource = sadasd(this.ViewModel.LayerCollection.RootLayers,0);
            };
        }

        private IEnumerable<string> sadasd(IList<ILayer> layers, int depht)
        {

            foreach (var layer in layers)
            {
                yield return $"{depht}   {layer.Type}  {layer.Control.Self.Visibility}";


                foreach (var child in sadasd( layer.Children, depht+1))
                {
                    yield return child;
                }
            }        
        }
    }

    public sealed partial class StyleMenu : UserControl, IMenu
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = "Style"; //resource.GetString("/Menus/Style");
        }

        //Menu
        public MenuType Type => MenuType.Style;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Styles.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }
}