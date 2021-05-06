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
                    this.ListView.SelectedItem = null;
                    this.Styles.Clear();
                }
                else
                {
                    this.Styles.Clear();
                    foreach (IStyle style in value.Styles)
                    {
                        this.Styles.Add(style);
                    }
                    this.ListView.SelectedItem = value.Name;
                }
            }
            get
            {
                if (this.ListView.SelectedItem is string name)
                {
                    return this.StyleCategorys.FirstOrDefault(c => c.Name == name);
                }
                else
                {
                    return null;
                }
            }
        }

        IList<StyleCategory> StyleCategorys;
        readonly ObservableCollection<string> StyleCategoryNames = new ObservableCollection<string>();
        readonly ObservableCollection<IStyle> Styles = new ObservableCollection<IStyle>();


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
                    this.SelectedStyleCategory = this.StyleCategorys.FirstOrDefault(c => c.Name == name);
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
                                 await this.RenameStyle(style, this.SelectedStyleCategory);
                                 this.State = MainPageState.Main;
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
            if (styleCategory == null) return;

            IStyle style2 = style.Clone();
            style2.CacheTransform();
            style2.OneBrushPoints(transformer);

            string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(this.Untitled);
            if (string.IsNullOrEmpty(rename)) return;

            style2.Name = rename;
            styleCategory.Styles.Add(style2);
            await this.Save();

            this.Styles.Add(style2);
        }

        private async Task RenameStyle(IStyle style, StyleCategory styleCategory)
        {
            if (styleCategory == null) return;

            IStyle style2 = style.Clone();
            string placeholderText = string.IsNullOrEmpty(style.Name) ? this.Untitled : style.Name;
            style2.Name = await Retouch_Photo2.DrawPage.ShowRenameFunc(placeholderText);

            if (styleCategory.Styles.Contains(style))
            {
                int index = styleCategory.Styles.IndexOf(style);
                styleCategory.Styles[index] = style2;
            }
            if (this.Styles.Contains(style))
            {
                int index = this.Styles.IndexOf(style);
                this.Styles[index] = style2;
            }

            await this.Save();
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
                if (this.Styles.Contains(style))
                {
                    this.Styles.Remove(style);
                }
            }

            await this.Save();
        }



        private async Task AddStyleCategory()
        {
            string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(this.Untitled);
            if (string.IsNullOrEmpty(rename)) return;

            this.StyleCategorys.Add(new StyleCategory
            {
                Name = rename
            });
            this.Styles.Clear();

            this.StyleCategoryNames.Add(rename);
            this.ListView.SelectedItem = rename;

            await this.Save();
        }

        private async Task RenameStyleCategory(StyleCategory styleCategory)
        {
            if (styleCategory == null) return;

            string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(this.Untitled);
            if (string.IsNullOrEmpty(rename)) return;

            styleCategory.Name = rename;

            this.StyleCategoryNames.Clear();
            foreach (StyleCategory styleCategory2 in this.StyleCategorys)
            {
                this.StyleCategoryNames.Add(styleCategory2.Name);
            }
            this.ListView.SelectedItem = rename;

            await this.Save();
        }

        private async Task DeleteStyleCategory(StyleCategory styleCategory)
        {
            if (styleCategory == null) return;

            this.StyleCategorys.Remove(styleCategory);
            this.StyleCategoryNames.Remove(styleCategory.Name);

            await this.Save();

            this.SelectedStyleCategory = this.StyleCategorys.FirstOrDefault();
        }

    }
}