using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Styles;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Styles.IStyle"/>.
    /// </summary>
    public sealed partial class StyleMenu : Expander, IMenu 
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content     
        public override UIElement MainPage => this.StyleMainPage;
        StyleMainPage StyleMainPage = new StyleMainPage();


        //@Construct
        /// <summary>
        /// Initializes a StyleMenu. 
        /// </summary>
        public StyleMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }
            
    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Styles.IStyle"/>.
    /// </summary>
    public sealed partial class StyleMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = resource.GetString("/Menus/Style");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Style;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Styles.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }
    

    /// <summary>
    /// MainPage of <see cref = "StyleMenu"/>.
    /// </summary>
    public sealed partial class StyleMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Construct
        /// <summary>
        /// Initializes a StyleMainPage. 
        /// </summary>
        public StyleMainPage()
        {
            this.InitializeComponent();

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
                            IEnumerable<Retouch_Photo2.Styles.IStyle> Styles = styleCategory.Styles;
                            this.StylesGridView.ItemsSource = Styles.ToList();
                        }
                    }
                }
            };

            this.StylesGridView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Retouch_Photo2.Styles.IStyle item)
                {
                    Transformer transformer = this.SelectionViewModel.Transformer;

                    this.MethodViewModel.ILayerChanged<Retouch_Photo2.Styles.IStyle>
                    (
                        set: (layer) =>
                        {
                            Transformer transformer2 = layer.Transform.Transformer;
                            Retouch_Photo2.Styles.IStyle style2 = item.Clone();
                            style2.CacheTransform();
                            style2.DeliverBrushPoints(transformer2);
                            layer.Style = style2;

                            transformer = transformer2;
                            this.SelectionViewModel.StandStyleLayer = layer;
                        },

                        historyTitle: "Set style",
                        getHistory: (layer) => layer.Style,
                        setHistory: (layer, previous) => layer.Style = previous.Clone()
                    );

                    Retouch_Photo2.Styles.IStyle style = item.Clone();
                    style.CacheTransform();
                    style.DeliverBrushPoints(transformer);
                    this.SelectionViewModel.SetStyle(style);
                }
            };

        }
    }
}