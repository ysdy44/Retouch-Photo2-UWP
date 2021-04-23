// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Styles;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Styles"/>.
    /// </summary>
    public sealed partial class StyleMenu : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Construct
        /// <summary>
        /// Initializes a StyleMainPage. 
        /// </summary>
        public StyleMenu()
        {
            this.InitializeComponent();

            this.Loaded += async (s, e) =>
            {
                if (this.CollectionViewSource.Source == null)
                {
                    IEnumerable<StyleCategory> categorys = await Retouch_Photo2.XML.ConstructStylesFile();
                    if (categorys != null)
                    {
                        this.CollectionViewSource.Source = categorys;
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
                            this.SelectionViewModel.StandardStyleLayer = layer;
                        },

                        type: HistoryType.LayersProperty_SetStyle,
                        getUndo: (layer) => layer.Style,
                        setUndo: (layer, previous) => layer.Style = previous.Clone()
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