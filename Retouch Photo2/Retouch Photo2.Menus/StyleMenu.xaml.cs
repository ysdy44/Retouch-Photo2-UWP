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
using Retouch_Photo2.Elements;

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
        IEnumerable<IStyle> SelectedStyles => from i in this.GridView.SelectedItems where (i is IStyle) select (i as IStyle);
        StyleCategory SelectedStyleCategory
        {
            set
            {
                if (value == null)
                {
                    this.StyleCollection.Clear();
                    this.ListView.SelectedItem = null;

                    this.Control.Content = null;
                    this.Button.IsEnabled = false;
                }
                else
                {
                    this.StyleCollection.Clear();
                    foreach (IStyle style in value.Styles)
                    {
                        this.StyleCollection.Add(style);
                    }
                    this.ListView.SelectedItem = value;

                    this.Control.Content = value.Name;
                    this.Button.IsEnabled = true;
                }
            }
            get
            {
                if (this.ListView.SelectedItem is StyleCategory styleCategory)
                {
                    return styleCategory;
                }
                else
                {
                    return null;
                }
            }
        }

        IList<StyleCategory> StyleCategorys;
        readonly ObservableCollection<StyleCategory> StyleCategoryCollection = new ObservableCollection<StyleCategory>();
        readonly ObservableCollection<IStyle> StyleCollection = new ObservableCollection<IStyle>();


        private string Untitled = "Untitled";


        MainPageState State
        {
            get => this.state;
            set
            {
                switch (value)
                {
                    case MainPageState.Rename:
                        VisualStateManager.GoToState(this, this.Rename.Name, false);
                        break;
                    case MainPageState.Delete:
                        VisualStateManager.GoToState(this, this.Delete.Name, false);
                        break;
                    default:
                        VisualStateManager.GoToState(this, this.Normal.Name, false);
                        break;
                }

                this.state = value;
            }
        }
        MainPageState state = MainPageState.Main;


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
                    if (styleCategorys == null) return;

                    this.StyleCategorys = styleCategorys.ToList();
                    foreach (StyleCategory styleCategory in this.StyleCategorys)
                    {
                        this.StyleCategoryCollection.Add(styleCategory);
                    }

                    this.SelectedStyleCategory = styleCategorys.FirstOrDefault();
                }
            };


            this.Button.Tapped += (s, e) => this.StyleCategoryNamesFlyout.ShowAt(this.Button);
            this.ListView.ItemClick += (s, e) =>
            {
                this.StyleCategoryNamesFlyout.Hide();
                if (e.ClickedItem is StyleCategory styleCategory)
                {
                    this.SelectedStyleCategory = styleCategory;
                }
            };
            this.GridView.ItemClick += async (s, e) =>
             {
                 switch (this.State)
                 {
                     case MainPageState.Main:
                         {
                             if (e.ClickedItem is IStyle style)
                             {
                                 this.SetStyle(style);
                             }
                         }
                         break;
                     case MainPageState.Rename:
                         {
                             if (e.ClickedItem is IStyle style)
                             {
                                 bool result = await this.RenameStyle(style, this.SelectedStyleCategory);
                                 if (result) this.State = MainPageState.Main;
                             }
                         }
                         break;
                     default:
                         break;
                 }
             };


            this.AddStyleButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    IStyle style = layer.Style;
                    Transformer transformer = layer.Transform.Transformer;
                    await this.AddStyle(style, transformer, this.SelectedStyleCategory);
                }
            };

            this.RenameStyleButton.Tapped += (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.State = MainPageState.Rename;
            };
            this.RenameStyleCancelButton.Tapped += (s, e) => this.State = MainPageState.Main;

            this.DeleteStyleButton.Tapped += (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.State = MainPageState.Delete;
            };
            this.DeleteStyleOKButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                await this.DeleteStyle(this.SelectedStyles.ToArray(), this.SelectedStyleCategory);
                this.State = MainPageState.Main;
            };
            this.DeleteStyleCancelButton.Tapped += (s, e) => this.State = MainPageState.Main;


            this.AddStyleCategoryButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                await this.AddStyleCategory();
            };
            this.RenameStyleCategoryButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                await this.RenameStyleCategory(this.SelectedStyleCategory);
            };
            this.DeleteStyleCategoryButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                await this.DeleteStyleCategory(this.SelectedStyleCategory);
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

            this.Untitled = resource.GetString("$Untitled");

            this.StyleGroupHeader.Content = resource.GetString("Menus_Style");
            this.AddStyleControl.Content = resource.GetString("Menus_AddStyle");
            this.RenameStyleControl.Content = resource.GetString("Menus_RenameStyle");
            this.DeleteStyleControl.Content = resource.GetString("Menus_DeleteStyle");

            this.StyleCategoryGroupHeader.Content = resource.GetString("Menus_StyleCategory");
            this.AddStyleCategoryControl.Content = resource.GetString("Menus_AddStyleCategory");
            this.RenameStyleCategoryControl.Content = resource.GetString("Menus_RenameStyleCategory");
            this.DeleteStyleCategoryControl.Content = resource.GetString("Menus_DeleteStyleCategory");

            this.MoreToolTip.Content = resource.GetString("Menus_More");
        }



        private void SetStyle(IStyle style)
        {
            Transformer transformer = this.SelectionViewModel.Transformer;

            this.MethodViewModel.ILayerChanged<IStyle>
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

        private async Task AddStyle(IStyle style, Transformer transformer, StyleCategory styleCategory)
        {
            string rename = this.Untitled;
            if (string.IsNullOrEmpty(rename)) return;

            IStyle style2 = style.Clone();
            style2.Name = rename;
            style2.CacheTransform();
            style2.OneBrushPoints(transformer);

            if (styleCategory == null)
            {
                StyleCategory styleCategory2 = new StyleCategory
                {
                    Name = rename,
                    Styles =
                    {
                        style2
                    }
                };
                this.StyleCategorys.Add(styleCategory2);
                this.StyleCategoryCollection.Add(styleCategory2);

                this.SelectedStyleCategory = styleCategory2;
            }
            else
            {
                styleCategory.Styles.Add(style2);

                this.StyleCollection.Add(style2);
            }

            await this.Save();
        }

        private async Task<bool> RenameStyle(IStyle style, StyleCategory styleCategory)
        {
            if (styleCategory == null) return false;

            IStyle style2 = style.Clone();
            string placeholderText = string.IsNullOrEmpty(style.Name) ? this.Untitled : style.Name;

            string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(this.Untitled);
            if (string.IsNullOrEmpty(rename)) return false;

            style2.Name = rename;
            if (styleCategory.Styles.Contains(style))
            {
                int index = styleCategory.Styles.IndexOf(style);
                styleCategory.Styles[index] = style2;
            }
            if (this.StyleCollection.Contains(style))
            {
                int index = this.StyleCollection.IndexOf(style);
                this.StyleCollection[index] = style2;
            }

            await this.Save();
            return true;
        }

        public async Task DeleteStyle(IStyle[] styles, StyleCategory styleCategory)// You can not remove an item by an IEnumerable
        {
            if (styleCategory == null) return;

            foreach (IStyle style in styles)
            {
                if (styleCategory.Styles.Contains(style))
                {
                    styleCategory.Styles.Remove(style);
                }
                if (this.StyleCollection.Contains(style))
                {
                    this.StyleCollection.Remove(style);
                }
            }

            await this.Save();
        }



        private async Task AddStyleCategory()
        {
            string rename = this.Untitled;
            if (string.IsNullOrEmpty(rename)) return;

            StyleCategory styleCategory = new StyleCategory
            {
                Name = rename
            };
            this.StyleCategorys.Add(styleCategory);
            this.StyleCategoryCollection.Add(styleCategory);

            await this.Save();
            this.SelectedStyleCategory = styleCategory;
        }

        private async Task RenameStyleCategory(StyleCategory styleCategory)
        {
            if (styleCategory == null) return;

            string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(this.Untitled);
            if (string.IsNullOrEmpty(rename)) return;

            styleCategory.Name = rename;

            this.StyleCategoryCollection.Clear();
            foreach (StyleCategory styleCategory2 in this.StyleCategorys)
            {
                this.StyleCategoryCollection.Add(styleCategory2);
            }

            await this.Save();
            this.SelectedStyleCategory = styleCategory;
        }

        private async Task DeleteStyleCategory(StyleCategory styleCategory)
        {
            if (styleCategory == null) return;

            this.StyleCategorys.Remove(styleCategory);
            this.StyleCategoryCollection.Remove(styleCategory);

            await this.Save();
            this.SelectedStyleCategory = this.StyleCategorys.FirstOrDefault();
        }

    }
}