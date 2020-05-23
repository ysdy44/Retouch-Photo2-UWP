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
                this.ItemsControl.ItemsSource = sadasd(this.ViewModel.LayerCollection.RootLayerages,0);
            };
            this.Button2.Click += (s, e) =>
            {
                this.ItemsControl.ItemsSource = asdsadasdsdsssss();
            };

            this.Coo.Click += (s, e) =>
            {
                List<Layerage> sdasd = new List<Layerage>();
                foreach (var item in this.ViewModel.LayerCollection.RootLayerages)
                {
                    sdasd.Add(item.Clone());
                }
                previous = sdasd;
            };
            this.Re.Click += (s, e) =>
            {
                this.ViewModel.LayerCollection.RootLayerages.Clear();
                foreach (var item in previous)
                {
                    this.ViewModel.LayerCollection.RootLayerages.Add(item.Clone());
                }

                this.ViewModel.Text = previous.Count().ToString();
                this.ViewModel.SetMode(this.ViewModel.LayerCollection);
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);
                this.ViewModel.Invalidate();
            };
        }
        IEnumerable<Layerage> previous;

        private IEnumerable<string> asdsadasdsdsssss()
        {
            foreach (var layer in Layer.Instances)
            {
                ILayer layer2 = layer;
                yield return $"   {layer2.Id}";
            }
        }

        private IEnumerable<string> sadasd(IList<Layerage> layers, int depht)
        {

            foreach (var layer in layers)
            {
                ILayer layer2 = layer.Self;
                yield return $"{depht}  {layer2.Id}";


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