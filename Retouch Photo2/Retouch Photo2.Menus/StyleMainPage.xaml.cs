// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Styles;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// MainPage of <see cref = "StyleMenu"/>.
    /// </summary>
    public sealed partial class StyleMainPage : UserControl
    {

        //@ViewModel
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