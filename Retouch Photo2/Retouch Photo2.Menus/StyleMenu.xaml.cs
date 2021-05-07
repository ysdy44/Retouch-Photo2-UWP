// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Styles;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
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
        IEnumerable<IStyle> SelecteItems => from i in this.GridView.SelectedItems where (i is IStyle) select (i as IStyle);

        private StyleCategory selectedItemCollection;
        public StyleCategory SelectedItemCollection
        {
            get => this.selectedItemCollection;
            set
            {
                if (value == null)
                {
                    this.Items.Clear();
                    this.ListView.SelectedItem = null;

                    this.Control.Content = null;
                    this.Button.IsEnabled = false;

                    this.selectedItemCollection = value;
                }
                else
                {
                    this.Items.Clear();
                    foreach (IStyle style in value.Styles)
                    {
                        this.Items.Add(style);
                    }
                    this.ListView.SelectedItem = value;

                    this.Control.Content = value.Name;
                    this.Button.IsEnabled = true;

                    this.selectedItemCollection = value;
                }
            }
        }


        IList<StyleCategory> Source;
        readonly ObservableCollection<StyleCategory> ItemCollections = new ObservableCollection<StyleCategory>();
        readonly ObservableCollection<IStyle> Items = new ObservableCollection<IStyle>();


        private string Untitled = "Untitled";


        //@VisualState
        MainPageState _vsState = MainPageState.None;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsState)
                {
                    case MainPageState.None: return this.Normal;
                    case MainPageState.Rename: return this.RenameState;
                    case MainPageState.Delete: return this.DeleteState;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, true);
        }
        /// <summary> Gets or set the state. </summary>
        public MainPageState State
        {
            get => this._vsState;
            set
            {
                this._vsState = value;
                this.VisualState = this.VisualState;//VisualState
            }
        }


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
                if (this.Source == null)
                {
                    IEnumerable<StyleCategory> styleCategorys = await Retouch_Photo2.XML.ConstructStylesFile();
                    if (styleCategorys == null) return;

                    this.Source = styleCategorys.ToList();
                    foreach (StyleCategory styleCategory in this.Source)
                    {
                        this.ItemCollections.Add(styleCategory);
                    }

                    this.SelectedItemCollection = styleCategorys.FirstOrDefault();
                }
            };


            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this.Button);
            this.ListView.ItemClick += (s, e) =>
            {
                this.Flyout.Hide();
                if (e.ClickedItem is StyleCategory styleCategory)
                {
                    this.SelectedItemCollection = styleCategory;
                }
            };
            this.GridView.ItemClick += async (s, e) =>
             {
                 switch (this.State)
                 {
                     case MainPageState.None:
                         {
                             if (e.ClickedItem is IStyle style)
                             {
                                 this.Set(style);
                             }
                         }
                         break;
                     case MainPageState.Rename:
                         {
                             if (e.ClickedItem is IStyle style)
                             {
                                 bool result = await this.Rename(style, this.SelectedItemCollection);
                                 if (result) this.State = MainPageState.None;
                             }
                         }
                         break;
                     default:
                         break;
                 }
             };


            this.AddButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    IStyle style = layer.Style;
                    Transformer transformer = layer.Transform.Transformer;
                    await this.Add(style, transformer, this.SelectedItemCollection);
                }
            };

            this.RenameButton.Tapped += (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.State = MainPageState.Rename;
            };
            this.RenameCancelButton.Tapped += (s, e) => this.State = MainPageState.None;

            this.DeleteButton.Tapped += (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.State = MainPageState.Delete;
            };
            this.DeleteOKButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                await this.Delete(this.SelecteItems.ToArray(), this.SelectedItemCollection);
                this.State = MainPageState.None;
            };
            this.DeleteCancelButton.Tapped += (s, e) => this.State = MainPageState.None;


            this.AddCollectionButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                await this.AddCollection();
            };
            this.RenameCollectionButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                await this.RenameCollection(this.SelectedItemCollection);
            };
            this.DeleteCollectionButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                await this.DeleteCollection(this.SelectedItemCollection);
            };
        }

        private async Task Save()
        {
            await XML.SaveStylesFile(this.Source);
        }

    }

    public sealed partial class StyleMenu : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Untitled = resource.GetString("$Untitled");

            this.GroupHeader.Content = resource.GetString("Menus_Style");
            this.AddControl.Content = resource.GetString("Menus_AddStyle");
            this.RenameControl.Content = resource.GetString("Menus_RenameStyle");
            this.DeleteControl.Content = resource.GetString("Menus_DeleteStyle");

            this.CollectionGroupHeader.Content = resource.GetString("Menus_StyleCategory");
            this.AddCollectionControl.Content = resource.GetString("Menus_AddStyleCategory");
            this.RenameCollectionControl.Content = resource.GetString("Menus_RenameStyleCategory");
            this.DeleteCollectionControl.Content = resource.GetString("Menus_DeleteStyleCategory");

            this.MoreToolTip.Content = resource.GetString("Menus_More");
        }



        private void Set(IStyle style)
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

        private async Task Add(IStyle style, Transformer transformer, StyleCategory styleCategory)
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
                this.Source.Add(styleCategory2);
                this.ItemCollections.Add(styleCategory2);

                this.SelectedItemCollection = styleCategory2;
            }
            else
            {
                styleCategory.Styles.Add(style2);

                this.Items.Add(style2);
            }

            await this.Save();
        }

        private async Task<bool> Rename(IStyle style, StyleCategory styleCategory)
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
            if (this.Items.Contains(style))
            {
                int index = this.Items.IndexOf(style);
                this.Items[index] = style2;
            }

            await this.Save();
            return true;
        }

        public async Task Delete(IStyle[] styles, StyleCategory styleCategory)// You can not remove an item by an IEnumerable
        {
            if (styleCategory == null) return;

            foreach (IStyle style in styles)
            {
                if (styleCategory.Styles.Contains(style))
                {
                    styleCategory.Styles.Remove(style);
                }
                if (this.Items.Contains(style))
                {
                    this.Items.Remove(style);
                }
            }

            await this.Save();
        }



        private async Task AddCollection()
        {
            string rename = this.Untitled;
            if (string.IsNullOrEmpty(rename)) return;

            StyleCategory styleCategory = new StyleCategory
            {
                Name = rename
            };
            this.Source.Add(styleCategory);
            this.ItemCollections.Add(styleCategory);

            await this.Save();
            this.SelectedItemCollection = styleCategory;
        }

        private async Task RenameCollection(StyleCategory styleCategory)
        {
            if (styleCategory == null) return;

            string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(this.Untitled);
            if (string.IsNullOrEmpty(rename)) return;

            styleCategory.Name = rename;

            this.ItemCollections.Clear();
            foreach (StyleCategory styleCategory2 in this.Source)
            {
                this.ItemCollections.Add(styleCategory2);
            }

            await this.Save();
            this.SelectedItemCollection = styleCategory;
        }

        private async Task DeleteCollection(StyleCategory styleCategory)
        {
            if (styleCategory == null) return;

            this.Source.Remove(styleCategory);
            this.ItemCollections.Remove(styleCategory);

            await this.Save();
            this.SelectedItemCollection = this.Source.FirstOrDefault();
        }

    }
}