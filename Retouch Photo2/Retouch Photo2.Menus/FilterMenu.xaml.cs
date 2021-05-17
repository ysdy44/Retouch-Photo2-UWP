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
using Windows.UI.Xaml.Controls;
using Windows.Globalization;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Filters"/>.
    /// </summary>
    public sealed partial class FilterMenu : Expander
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        IEnumerable<FilterShowControl> SelectedControls => from i in this.GridView.SelectedItems where (i is FilterShowControl) select (i as FilterShowControl);


        //@Converter
        private bool IsEnableConverter(FilterShowControlCategory value) => value != null;

        private string Untitled = "Untitled";


        #region DependencyProperty


        private ObservableCollection<FilterShowControlCategory> ControlCategorys { get; set; } = new ObservableCollection<FilterShowControlCategory>();


        /// <summary> Gets or sets <see cref = "FilterMenu" />'s SelectedControlCategory. </summary>
        public FilterShowControlCategory SelectedControlCategory
        {
            get => (FilterShowControlCategory)base.GetValue(SelectedControlCategoryProperty);
            set => base.SetValue(SelectedControlCategoryProperty, value);
        }
        /// <summary> Identifies the <see cref = "FilterMenu.SelectedControlCategory" /> dependency property. </summary>
        public static readonly DependencyProperty SelectedControlCategoryProperty = DependencyProperty.Register(nameof(SelectedControlCategory), typeof(FilterShowControlCategory), typeof(FilterMenu), new PropertyMetadata(null));


        #endregion


        //@VisualState
        /// <summary> Gets or set the state. </summary>
        public MainPageState State
        {
            get => this.state;
            set
            {
                this.GridView.SelectionMode = value == MainPageState.Delete ? ListViewSelectionMode.Multiple : ListViewSelectionMode.None;
                this.MainGrid.Visibility = value == MainPageState.None ? Visibility.Visible : Visibility.Collapsed;
                this.RenameGrid.Visibility = value == MainPageState.Rename ? Visibility.Visible : Visibility.Collapsed;
                this.DeleteGrid.Visibility = value == MainPageState.Delete ? Visibility.Visible : Visibility.Collapsed;
                this.state = value;
            }
        }
        private MainPageState state = MainPageState.None;


        //@Construct
        /// <summary>
        /// Initializes a FilterMenu. 
        /// </summary>
        public FilterMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            base.Loaded += (s, e) => this.ConstructLanguages();
            this.MoreButton.Tapped += (s, e) => this.MoreFlyout.ShowAt(this.MoreButton);

            this.Loaded += async (s, e) =>
            {
                if (this.SelectedControlCategory == null)
                {
                    IEnumerable<FilterCategory> filterCategorys = await Retouch_Photo2.XML.ConstructFiltersFile();
                    if (filterCategorys == null) return;

                    foreach (FilterCategory filterCategory in filterCategorys)
                    {
                        FilterShowControlCategory controlCategory = new FilterShowControlCategory(filterCategory);
                        controlCategory.Rename(base.Language);
                        this.ControlCategorys.Add(controlCategory);
                    }
                    this.SelectedControlCategory = this.ControlCategorys.FirstOrDefault();
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

                                Filter filter3 = filter.Clone();
                                this.SelectionViewModel.SetFilter(filter3);

                                this.MethodViewModel.ILayerChanged<Filter>
                                (
                                    set: (layer) => layer.Filter = filter.Clone(),

                                    type: HistoryType.LayersProperty_SetFilter,
                                    getUndo: (layer) => layer.Filter.Clone(),
                                    setUndo: (layer, previous) => layer.Filter = previous.Clone()
                                );
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
            await XML.SaveFiltersFile(from c in this.ControlCategorys select c.ToFilterCategory());
        }

    }

    public sealed partial class FilterMenu : Expander
    {

        //Languages
        private void ConstructLanguages()
        {
            if (string.IsNullOrEmpty(ApplicationLanguages.PrimaryLanguageOverride) == false)
            {
                if (ApplicationLanguages.PrimaryLanguageOverride != base.Language)
                {
                    this.ConstructStrings();
                }
            }
        }

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

    }

    public sealed partial class FilterMenu : Expander
    {

        public void AddControl(Filter filter, string rename)
        {
            if (string.IsNullOrEmpty(rename)) return;

            Filter filter2 = filter.Clone();
            filter2.Name = rename;

            FilterShowControl control = new FilterShowControl
            {
                Filter = filter2
            };

            if (this.SelectedControlCategory is FilterShowControlCategory controlCategory)
            {
                controlCategory.Add(control);
            }
            else
            {
                FilterShowControlCategory controlCategory2 = new FilterShowControlCategory
                {
                    Name = rename
                };
                controlCategory2.Add(control);

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
            controlCategory.Rename(base.Language);
            this.ControlCategorys.Add(controlCategory);
            this.SelectedControlCategory = controlCategory;
        }

        public async void RenameSelectedControlCategory()
        {
            if (this.SelectedControlCategory is FilterShowControlCategory controlCategory)
            {
                string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(controlCategory.Title);
                if (string.IsNullOrEmpty(rename)) return;

                controlCategory.Name = rename;
                controlCategory.Rename(base.Language);
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