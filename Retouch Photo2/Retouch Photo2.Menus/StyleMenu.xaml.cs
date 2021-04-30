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
using Windows.ApplicationModel.Resources;

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
            this.ConstructStrings();
            this.MoreButton.Tapped += (s, e) => this.MoreFlyout.ShowAt(this.MoreButton);

            this.Loaded += async (s, e) =>
            {
                if (this.StyleCategorys == null)
                {
                    IEnumerable<StyleCategory> styleCategorys = await Retouch_Photo2.XML.ConstructStylesFile();
                    this.StyleCategorys = styleCategorys.ToList();
                    if (styleCategorys != null)
                    {
                        foreach (StyleCategory styleCategory in styleCategorys)
                        {
                            this.StyleCategoryNames.Add(styleCategory.Name);
                        }

                        StyleCategory styleCategory2 = styleCategorys.First();
                        this.ListView.SelectedItem = styleCategory2.Name;
                        foreach (IStyle style in styleCategory2.Styles)
                        {
                            this.Styles.Add(style);
                        }
                    }
                }
            };

            this.Button.Tapped += (s, e) => this.StyleCategoryNamesFlyout.ShowAt(this.Button);
            this.ListView.ItemClick += (s, e) =>
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
                    this.SetStyle(item);
                }
            };

            this.AddStyleButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    IStyle style = layer.Style.Clone();
                    Transformer transformer = layer.Transform.Transformer;
                    await this.AddStyle(style, transformer);
                }
            };

            this.DeleteStyleButton.Tapped += (s, e) =>
            {
                this.MoreFlyout.Hide();
                VisualStateManager.GoToState(this, this.DeleteStyleState.Name, false);
            };
            this.DeleteStyleOKButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                if (this.ListView.SelectedItem is string name)
                {
                    await this.DeleteStyle(this.GridView.SelectedItems, name);
                }
            };
            this.DeleteStyleCancelButton.Tapped += (s, e) =>
            {
                this.MoreFlyout.Hide();
                VisualStateManager.GoToState(this, this.Normal.Name, false);
            };
        }

        private async Task Save()
        {
            await XML.SaveStylesFile(this.StyleCategorys);
        }

    }

    public sealed partial class StyleMenu : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.StyleGroupHeader.Content = "Style";
            this.AddStyleControl.Content = "Add style";
            this.RenameStyleControl.Content = "Rename style";
            this.DeleteStyleControl.Content = "Delete style";

            this.StyleCategoryGroupHeader.Content = "Style category";
            this.AddStyleCategoryControl.Content = "Add style category";
            this.RenameStyleCategoryControl.Content = "Rename style category";
            this.DeleteStyleCategoryControl.Content = "Delete style category";

            this.MoreToolTip.Content = resource.GetString("Menus_More");
        }

        private void SetStyle(IStyle style)
        {
            Transformer transformer = this.SelectionViewModel.Transformer;

            this.MethodViewModel.ILayerChanged<Retouch_Photo2.Styles.IStyle>
            (
                set: (layer) =>
                {
                    Transformer transformer2 = layer.Transform.Transformer;
                    IStyle style2 = style.Clone();
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

            IStyle style3 = style.Clone();
            style3.CacheTransform();
            style3.DeliverBrushPoints(transformer);
            this.SelectionViewModel.SetStyle(style3);
        }

        private async Task AddStyle(IStyle style, Transformer transformer)
        {
            IStyle style2 = style.Clone();
            style2.CacheTransform();
            style2.OneBrushPoints(transformer);

            StyleCategory styleCategory = this.StyleCategorys.FirstOrDefault(c => c.Name == "Custom");
            if (styleCategory != null)
            {
                styleCategory.Styles.Add(style2);
                await this.Save();
            }
            else
            {
                styleCategory = new StyleCategory
                {
                    Name = "Custom",
                    Styles = new List<IStyle>
                    {
                        style2
                    }
                };

                this.StyleCategorys.Add(styleCategory);
                this.StyleCategoryNames.Add(styleCategory.Name);
                this.ListView.SelectedItem = styleCategory.Name;
                await this.Save();
            }

            this.Styles.Clear();
            foreach (IStyle style3 in styleCategory.Styles)
            {
                this.Styles.Add(style3);
            }
        }

        private async Task DeleteStyle(IList<object> styles, string name)
        {
            StyleCategory styleCategory = this.StyleCategorys.FirstOrDefault(c => c.Name == name);
            if (styleCategory == null) return;

            foreach (object item in styles)
            {
                if (item is IStyle style)
                {
                    if (styleCategory.Styles.Contains(style))
                    {
                        styleCategory.Styles.Remove(style);
                    }
                    if (this.Styles.Contains(style))
                    {
                        this.Styles.Remove(style);
                    }
                }
            }

            await this.Save();
        }

    }
}