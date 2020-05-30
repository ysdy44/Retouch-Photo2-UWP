using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Styles;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    public sealed partial class StyleMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        //@Construct
        public StyleMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();

            this.Loaded += async (s, e) =>
            {
                if (this.StylesGridView.ItemsSource == null)
                {
                    IEnumerable<StyleCategory> StyleCategorys = await Retouch_Photo2.XML.ConstructStylesFile();
                    if (StyleCategorys != null)
                    {
                        StyleCategory styleCategory = StyleCategorys.FirstOrDefault();
                        if (styleCategory != null)
                        {
                            IEnumerable<Retouch_Photo2.Styles.Style> Styles = styleCategory.Styles;
                            this.StylesGridView.ItemsSource = Styles.ToList();
                        }
                    }
                }
            };

            this.StylesGridView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Retouch_Photo2.Styles.Style item)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set style");

                    //Selection
                    Transformer transformer = this.SelectionViewModel.Transformer;
                    this.SelectionViewModel.SetValue((layerage) =>
                    {
                        ILayer layer = layerage.Self;

                        //History
                        var previous = layer.Style.Clone();
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            layer.Style = previous.Clone();
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        Transformer transformer2 = layer.Transform.Transformer;
                        Style style2 = item.Clone();
                        style2.CacheTransform();
                        style2.DeliverBrushPoints(transformer2);
                        layer.Style = style2;

                        transformer = transformer2;
                        this.SelectionViewModel.StyleLayerage = layerage;
                    });
                    {
                        Style style = item.Clone();
                        style.CacheTransform();
                        style.DeliverBrushPoints(transformer);
                        this.SelectionViewModel.SetStyle(style);
                    }

                    //History
                    this.ViewModel.HistoryPush(history);

                    this.ViewModel.Invalidate();//Invalidate
                }
            };

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
            this._Expander.CurrentTitle = resource.GetString("/Menus/Style");
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