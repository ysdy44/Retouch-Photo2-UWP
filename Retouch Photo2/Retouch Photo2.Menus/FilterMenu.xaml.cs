// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Filters;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Filters"/>.
    /// </summary>
    public sealed partial class FilterMenu : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        IEnumerable<Filter> SelecteItems => from i in this.GridView.SelectedItems where (i is Filter) select (i as Filter);

        private FilterCategory selectedItemCollection;
        public FilterCategory SelectedItemCollection
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
                    foreach (Filter filter in value.Filters)
                    {
                        this.Items.Add(filter);
                    }
                    this.ListView.SelectedItem = value;

                    this.Control.Content = value.Name;
                    this.Button.IsEnabled = true;

                    this.selectedItemCollection = value;
                }
            }
        }


        IList<FilterCategory> Source;
        readonly ObservableCollection<FilterCategory> ItemCollections = new ObservableCollection<FilterCategory>();
        readonly ObservableCollection<Filter> Items = new ObservableCollection<Filter>();


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
        /// Initializes a FilterMenu. 
        /// </summary>
        public FilterMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.MoreButton.Tapped += (s, e) => this.MoreFlyout.ShowAt(this.MoreButton);

            this.Loaded += async (s, e) =>
            {
                if (this.Source == null)
                {
                    IEnumerable<FilterCategory> filterCategorys = await Retouch_Photo2.XML.ConstructFiltersFile();
                    if (filterCategorys == null) return;

                    this.Source = filterCategorys.ToList();
                    foreach (FilterCategory filterCategory in this.Source)
                    {
                        this.ItemCollections.Add(filterCategory);
                    }

                    this.SelectedItemCollection = filterCategorys.FirstOrDefault();
                }
            };


            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this.Button);
            this.ListView.ItemClick += (s, e) =>
            {
                this.Flyout.Hide();
                if (e.ClickedItem is FilterCategory filterCategory)
                {
                    this.SelectedItemCollection = filterCategory;
                }
            };
            this.GridView.ItemClick += async (s, e) =>
            {
                switch (this.State)
                {
                    case MainPageState.None:
                        {
                            if (e.ClickedItem is Filter filter)
                            {
                                this.Set(filter);
                            }
                        }
                        break;
                    case MainPageState.Rename:
                        {
                            if (e.ClickedItem is Filter filter)
                            {
                                bool result = await this.Rename(filter, this.SelectedItemCollection);
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

                    Filter filter = layer.Filter;
                    Transformer transformer = layer.Transform.Transformer;
                    await this.Add(filter, transformer, this.SelectedItemCollection);
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
            await XML.SaveFiltersFile(this.Source);
        }

    }

    public sealed partial class FilterMenu : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Untitled = resource.GetString("$Untitled");

            this.GroupHeader.Content = resource.GetString("Menus_Filter");
            this.AddControl.Content = resource.GetString("Menus_AddFilter");
            this.RenameControl.Content = resource.GetString("Menus_RenameFilter");
            this.DeleteControl.Content = resource.GetString("Menus_DeleteFilter");

            this.CollectionGroupHeader.Content = resource.GetString("Menus_FilterCategory");
            this.AddCollectionControl.Content = resource.GetString("Menus_AddFilterCategory");
            this.RenameCollectionControl.Content = resource.GetString("Menus_RenameFilterCategory");
            this.DeleteCollectionControl.Content = resource.GetString("Menus_DeleteFilterCategory");

            this.MoreToolTip.Content = resource.GetString("Menus_More");
        }



        private void Set(Filter filter)
        {
            this.MethodViewModel.ILayerChanged<Filter>
            (
                set: (layer) =>
                {
                    Transformer transformer2 = layer.Transform.Transformer;
                    Filter filter2 = filter.Clone();
                    layer.Filter = filter2;
                },

                type: HistoryType.LayersProperty_SetFilter,
                getUndo: (layer) => layer.Filter,
                setUndo: (layer, previous) => layer.Filter = previous.Clone()
            );

            Filter filter3 = filter.Clone();
            this.SelectionViewModel.SetFilter(filter3);
        }

        private async Task Add(Filter filter, Transformer transformer, FilterCategory filterCategory)
        {
            string rename = this.Untitled;
            if (string.IsNullOrEmpty(rename)) return;

            Filter filter2 = filter.Clone();
            filter2.Name = rename;

            if (filterCategory == null)
            {
                FilterCategory filterCategory2 = new FilterCategory
                {
                    Name = rename,
                    Filters =
                    {
                        filter2
                    }
                };
                this.Source.Add(filterCategory2);
                this.ItemCollections.Add(filterCategory2);

                this.SelectedItemCollection = filterCategory2;
            }
            else
            {
                filterCategory.Filters.Add(filter2);

                this.Items.Add(filter2);
            }

            await this.Save();
        }

        private async Task<bool> Rename(Filter filter, FilterCategory filterCategory)
        {
            if (filterCategory == null) return false;

            Filter filter2 = filter.Clone();
            string placeholderText = string.IsNullOrEmpty(filter.Name) ? this.Untitled : filter.Name;

            string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(this.Untitled);
            if (string.IsNullOrEmpty(rename)) return false;

            filter2.Name = rename;
            if (filterCategory.Filters.Contains(filter))
            {
                int index = filterCategory.Filters.IndexOf(filter);
                filterCategory.Filters[index] = filter2;
            }
            if (this.Items.Contains(filter))
            {
                int index = this.Items.IndexOf(filter);
                this.Items[index] = filter2;
            }

            await this.Save();
            return true;
        }

        public async Task Delete(Filter[] filters, FilterCategory filterCategory)// You can not remove an item by an IEnumerable
        {
            if (filterCategory == null) return;

            foreach (Filter filter in filters)
            {
                if (filterCategory.Filters.Contains(filter))
                {
                    filterCategory.Filters.Remove(filter);
                }
                if (this.Items.Contains(filter))
                {
                    this.Items.Remove(filter);
                }
            }

            await this.Save();
        }



        private async Task AddCollection()
        {
            string rename = this.Untitled;
            if (string.IsNullOrEmpty(rename)) return;

            FilterCategory filterCategory = new FilterCategory
            {
                Name = rename
            };
            this.Source.Add(filterCategory);
            this.ItemCollections.Add(filterCategory);

            await this.Save();
            this.SelectedItemCollection = filterCategory;
        }

        private async Task RenameCollection(FilterCategory filterCategory)
        {
            if (filterCategory == null) return;

            string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(this.Untitled);
            if (string.IsNullOrEmpty(rename)) return;

            filterCategory.Name = rename;

            this.ItemCollections.Clear();
            foreach (FilterCategory filterCategory2 in this.Source)
            {
                this.ItemCollections.Add(filterCategory2);
            }

            await this.Save();
            this.SelectedItemCollection = filterCategory;
        }

        private async Task DeleteCollection(FilterCategory filterCategory)
        {
            if (filterCategory == null) return;

            this.Source.Remove(filterCategory);
            this.ItemCollections.Remove(filterCategory);

            await this.Save();
            this.SelectedItemCollection = this.Source.FirstOrDefault();
        }

    }
}