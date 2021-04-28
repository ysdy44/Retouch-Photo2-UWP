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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

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
        readonly ObservableCollection<string> StyleCategoryNames = new ObservableCollection<string>();
        readonly ObservableCollection<IStyle> Styles = new ObservableCollection<IStyle>();


        //@Construct
        /// <summary>
        /// Initializes a StyleMenu. 
        /// </summary>
        public StyleMenu()
        {
            this.InitializeComponent();

            this.Loaded += async (s, e) =>
            {
                if (this.StyleCategorys == null)
                {
                    IEnumerable<StyleCategory> styleCategorys = await Retouch_Photo2.XML.ConstructStylesFile();
                    this.StyleCategorys = styleCategorys.ToList();
                    if (styleCategorys != null)
                    {
                        StyleCategory styleCategory = styleCategorys.First();

                        foreach (StyleCategory styleCategory2 in styleCategorys)
                        {
                            this.StyleCategoryNames.Add(styleCategory2.Name);
                        }
                        this.StyleCategoryNamesListView.SelectedItem = styleCategory.Name;

                        foreach (IStyle style in styleCategory.Styles)
                        {
                            this.Styles.Add(style);
                        }
                    }
                }
            };

            //StyleCategoryNames
            this.StyleCategoryNamesButton.Tapped += (s, e) => this.StyleCategoryNamesFlyout.ShowAt(this.StyleCategoryNamesButton);
            this.StyleCategoryNamesListView.ItemClick += (s, e) =>
           {
               this.StyleCategoryNamesFlyout.Hide();
               if (e.ClickedItem is string name)
               {
                   if (this.StyleCategoryNames.Contains(name))
                   {
                       StyleCategory styleCategory = this.StyleCategorys.First(c => c.Name == name);

                       this.Styles.Clear();
                       foreach (IStyle style in styleCategory.Styles)
                       {
                           this.Styles.Add(style);
                       }
                   }
               }
           };

            this.GridView.ItemClick += (s, e) =>
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

            this.AddButton.Tapped += async (s, e) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    Transformer transformer = layer.Transform.Transformer;
                    IStyle style = layer.Style.Clone();
                    style.CacheTransform();
                    style.OneBrushPoints(transformer);

                    StyleCategory styleCategory = this.StyleCategorys.FirstOrDefault(c => c.Name == "Custom");
                    if (styleCategory != null)
                    {
                        styleCategory.Styles.Add(style);
                        await this.Save();
                    }
                    else
                    {
                        styleCategory = new StyleCategory
                        {
                            Name = "Custom",
                            Styles = new List<IStyle>
                            {
                                style
                            }
                        };

                        this.StyleCategorys.Add(styleCategory);
                        this.StyleCategoryNames.Add(styleCategory.Name);
                        this.StyleCategoryNamesListView.SelectedItem = styleCategory.Name;
                        await this.Save();
                    }

                    this.Styles.Clear();
                    foreach (IStyle style2 in styleCategory.Styles)
                    {
                        this.Styles.Add(style2);
                    }
                }
            };

            this.WritableButton.Tapped += (s, e) =>
            {
                VisualStateManager.GoToState(this, this.Writable.Name, false);
            };

            this.WritableOKButton.Tapped += (s, e) =>
            {


            };

            this.WritableCancelButton.Tapped += (s, e) =>
            {
                VisualStateManager.GoToState(this, this.Normal.Name, false);
            };
        }

        private async Task Save()
        {
            await XML.SaveStylesFile(this.StyleCategorys);
        }

    }
}