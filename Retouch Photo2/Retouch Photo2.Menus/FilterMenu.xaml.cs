// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using System.ComponentModel;
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

namespace Retouch_Photo2.Menus
{
    internal class FilterShowControlCategory : ObservableCollection<FilterShowControl>
    {
        public string Name
        {
            get => this.name;
            set
            {
                if (this.name == value) return;

                this.name = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name)));//Notify  
            }
        }
        private string name = string.Empty;
    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Filters"/>.
    /// </summary>
    public sealed partial class FilterMenu : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        IEnumerable<FilterShowControl> SelectedControls => from i in this.GridView.SelectedItems where (i is FilterShowControl) select (i as FilterShowControl);


        private string Untitled = "Untitled";
        private ObservableCollection<FilterShowControlCategory> ControlCategorys { get; set; } = new ObservableCollection<FilterShowControlCategory>();


        internal FilterShowControlCategory SelectedControlCategory
        {
            get => this.selectedControlCategory;
            set
            {
                if (this.selectedControlCategory == value) return;

                this.selectedControlCategory = value;
                this.Button.IsEnabled = value != null;
                this.Control.Content = value?.Name;
                this.GridView.ItemsSource = value;
            }
        }
        private FilterShowControlCategory selectedControlCategory;


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
                if (this.SelectedControlCategory == null)
                {
                    IEnumerable<FilterCategory> filterCategorys = await Retouch_Photo2.XML.ConstructFiltersFile();
                    if (filterCategorys == null) return;

                    this.AddFilterCategorys(filterCategorys);
                }
            };


            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this.Button);
            this.ListView.ItemClick += (s, e) =>
            {
                this.Flyout.Hide();
                if (e.ClickedItem is FilterShowControlCategory controlCategory)
                {
                    this.SelectedControlCategory = controlCategory;
                }
            };
            this.ListView.DragItemsCompleted += async (s, e) => await this.Save();
            this.GridView.ItemClick += async (s, e) =>
            {
                switch (this.State)
                {
                    case MainPageState.None:
                        {
                            if (e.ClickedItem is FilterShowControl control)
                            {
                                Filter filter = control.Filter;
                                this.MethodViewModel.ILayerChanged<Filter>
                                (
                                    set: (layer) => layer.Filter = filter.Clone(),

                                    type: HistoryType.LayersProperty_SetFilter,
                                    getUndo: (layer) => layer.Filter.Clone(),
                                    setUndo: (layer, previous) => layer.Filter = previous.Clone()
                                );

                                Filter filter3 = filter.Clone();
                                this.SelectionViewModel.SetFilter(filter3);
                            }
                        }
                        break;
                    case MainPageState.Rename:
                        {
                            if (e.ClickedItem is FilterShowControl control)
                            {
                                string name = control.Filter.Name;
                                string placeholderText = string.IsNullOrEmpty(name) ? this.Untitled : name;
                                string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(placeholderText);
                                if (string.IsNullOrEmpty(rename)) return;

                                control.Filter.Name = rename;
                                control.Rename();
                                this.State = MainPageState.None;
                            }
                        }
                        break;
                    default:
                        break;
                }
            };
            this.GridView.DragItemsCompleted += async (s, e) => await this.Save();


            this.AddFilterButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    Filter filter = layer.Filter;
                    this.AddControl(filter, this.Untitled);
                    await this.Save();
                }
            };

            this.RenameFilterButton.Tapped += (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.State = MainPageState.Rename;
            };
            this.RenameFilterCancelButton.Tapped += (s, e) => this.State = MainPageState.None;

            this.DeleteFilterButton.Tapped += (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.State = MainPageState.Delete;
            };
            this.DeleteFilterOKButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.DeleteControls(this.SelectedControls.ToArray());
                await this.Save();
                this.State = MainPageState.None;
            };
            this.DeleteCancelButton.Tapped += (s, e) => this.State = MainPageState.None;


            this.AddFilterCategoryButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.AddFilterShowControlCategory(this.Untitled);
                await this.Save();
            };
            this.RenameFilterCategoryButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.RenameSelectedControlCategory();
                await this.Save();
            };
            this.DeleteFilterCategoryButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.RemoveSelectedControlCategory();
                await this.Save();
            };
        }

        private async Task Save()
        {
            await XML.SaveFiltersFile(this.ToFilterCategorys());
        }

    }

    public sealed partial class FilterMenu : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Untitled = resource.GetString("$Untitled");

            this.FilterGroupHeader.Content = resource.GetString("Menus_Filter");
            this.AddFilterControl.Content = resource.GetString("Menus_AddFilter");
            this.RenameFilterControl.Content = resource.GetString("Menus_RenameFilter");
            this.DeleteFilterControl.Content = resource.GetString("Menus_DeleteFilter");

            this.FilterCategoryGroupHeader.Content = resource.GetString("Menus_FilterCategory");
            this.AddFilterCategoryControl.Content = resource.GetString("Menus_AddFilterCategory");
            this.RenameFilterCategoryControl.Content = resource.GetString("Menus_RenameFilterCategory");
            this.DeleteFilterCategoryControl.Content = resource.GetString("Menus_DeleteFilterCategory");

            this.MoreToolTip.Content = resource.GetString("Menus_More");
        }


        public void AddFilterCategorys(IEnumerable<FilterCategory> filterCategorys)
        {
            foreach (FilterCategory filterCategory in filterCategorys)
            {
                FilterShowControlCategory controlategory = new FilterShowControlCategory
                {
                    Name = filterCategory.Name
                };
                foreach (Filter filter in filterCategory.Filters)
                {
                    controlategory.Add(new FilterShowControl
                    {
                        Filter = filter
                    });
                }
                this.ControlCategorys.Add(controlategory);
            }

            this.SelectedControlCategory = this.ControlCategorys.FirstOrDefault();
        }

        public IEnumerable<FilterCategory> ToFilterCategorys()
        {
            return
            (
                from controlCategory
                in this.ControlCategorys
                select new FilterCategory
                {
                    Name = controlCategory.Name,
                    Filters =
                    (
                        from control
                        in controlCategory
                        select control.Filter
                    )
                }
            );
        }

    }

    public sealed partial class FilterMenu : UserControl
    {

        public void AddControl(Filter filter, string rename)
        {
            if (string.IsNullOrEmpty(rename)) return;

            Filter item2 = filter.Clone();
            item2.Name = rename;

            if (this.SelectedControlCategory is FilterShowControlCategory controlCategory)
            {
                controlCategory.Add(new FilterShowControl 
                {
                    Filter = item2 
                });
            }
            else
            {
                FilterShowControlCategory controlCategory2 = new FilterShowControlCategory
                {
                    Name = rename
                };
                controlCategory2.Add(new FilterShowControl
                {
                    Filter = filter
                });
                this.ControlCategorys.Add(controlCategory2);

                this.SelectedControlCategory = this.ControlCategorys.LastOrDefault();
            }
        }

        public void DeleteControls(FilterShowControl[] controls)// You can not remove an FilterShowControl by an IEnumerable
        {
            if (this.SelectedControlCategory is FilterShowControlCategory controlCategory)
            {
                foreach (FilterShowControl control in controls)
                {
                    if (controlCategory.Contains(control))
                    {
                        controlCategory.Remove(control);
                    }
                }
            }
        }


        public void AddFilterShowControlCategory(string rename)
        {
            if (string.IsNullOrEmpty(rename)) return;

            FilterShowControlCategory controlCategory = new FilterShowControlCategory { Name = rename };
            this.ControlCategorys.Add(controlCategory);
            this.SelectedControlCategory = controlCategory;
        }

        public async void RenameSelectedControlCategory()
        {
            if (this.SelectedControlCategory is FilterShowControlCategory controlCategory)
            {
                string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(controlCategory.Name);
                if (string.IsNullOrEmpty(rename)) return;

                controlCategory.Name = rename;
            }
        }

        public void RemoveSelectedControlCategory()
        {
            if (this.SelectedControlCategory is FilterShowControlCategory controlCategory)
            {
                this.ControlCategorys.Remove(controlCategory);
            }
            this.SelectedControlCategory = this.ControlCategorys.FirstOrDefault();
        }
    }
}