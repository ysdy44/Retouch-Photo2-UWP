// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Styles;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Styles"/>.
    /// </summary>
    public sealed partial class StyleMenu : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        IList<StyleCategory> StyleCategorys;


        //@Construct
        /// <summary>
        /// Initializes a StyleMenu. 
        /// </summary>
        public StyleMenu()
        {
            this.InitializeComponent();

            this.Loaded += async (s, e) =>
            {
                if (this.CollectionViewSource.Source == null)
                {
                    IList<StyleCategory> styleCategorys = (await Retouch_Photo2.XML.ConstructStylesFile()).ToList();
                    if (styleCategorys != null)
                    {
                        this.CollectionViewSource.Source = styleCategorys;
                        this.StyleCategorys = styleCategorys;
                    }
                }
            };

            this.StylesGridView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is IStyle item)
                {
                    Transformer transformer = this.SelectionViewModel.Transformer;

                    this.MethodViewModel.ILayerChanged<Retouch_Photo2.Styles.IStyle>
                    (
                        set: (layer) =>
                        {
                            Transformer transformer2 = layer.Transform.Transformer;
                            IStyle style2 = item.Clone();
                            style2.CacheTransform();
                            style2.DeliverBrushPoints(transformer2);
                            layer.Style = style2;

                            transformer = transformer2;
                            this.SelectionViewModel.StandardStyleLayer = layer;
                        },

                        type: HistoryType.LayersProperty_SetStyle,
                        getUndo: (layer) => layer.Style,
                        setUndo: (layer, previous) => layer.Style = previous.Clone()
                    );

                    IStyle style = item.Clone();
                    style.CacheTransform();
                    style.DeliverBrushPoints(transformer);
                    this.SelectionViewModel.SetStyle(style);
                }
            };

        }

        private async Task Save()
        {
            await XML.SaveStylesFile(this.StyleCategorys);
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.SelectionViewModel.SelectionLayerage is ILayer layer)
            {
                IStyle style = layer.Style.Clone();

                StyleCategory styleCategory = this.StyleCategorys.FirstOrDefault(c => c.Name == "Custom");
                if (styleCategory != null)
                {
                    styleCategory.Styles.Add(style);
                    await this.Save();
                }
                else
                {
                    this.StyleCategorys.Add(new StyleCategory
                    {
                        Name = "Custom",
                        Styles = new List<IStyle>
                        {
                            style
                        }
                    });
                    await this.Save();
                }
            }
        }
    }
}