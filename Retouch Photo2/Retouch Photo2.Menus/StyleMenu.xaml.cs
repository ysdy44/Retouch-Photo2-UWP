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
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus
{
    internal class StyleShowControlCategory : ObservableCollection<StyleShowControl>
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
    /// Menu of <see cref = "Retouch_Photo2.Styles"/>.
    /// </summary>
    public sealed partial class StyleMenu : Expander
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        IEnumerable<StyleShowControl> SelectedControls => from i in this.GridView.SelectedItems where (i is StyleShowControl) select (i as StyleShowControl);


        private string Untitled = "Untitled";
        private ObservableCollection<StyleShowControlCategory> ControlCategorys { get; set; } = new ObservableCollection<StyleShowControlCategory>();


        internal StyleShowControlCategory SelectedControlCategory
        {
            get => this.selectedControlCategory;
            set
            {
                this.selectedControlCategory = value;
                this.Button.IsEnabled = value != null;
                this.Control.Content = value?.Name;
                this.GridView.ItemsSource = value;
            }
        }
        private StyleShowControlCategory selectedControlCategory;


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
            base.Loaded += (s, e) => this.ConstructLanguages();
            this.MoreButton.Tapped += (s, e) => this.MoreFlyout.ShowAt(this.MoreButton);

            this.Loaded += async (s, e) =>
            {
                if (this.SelectedControlCategory == null)
                {
                    IEnumerable<StyleCategory> styleCategorys = await Retouch_Photo2.XML.ConstructStylesFile();
                    if (styleCategorys == null) return;

                    this.AddStyleCategorys(styleCategorys);
                }
            };


            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this.Button);
            this.ListView.ItemClick += (s, e) =>
            {
                this.Flyout.Hide();
                if (e.ClickedItem is StyleShowControlCategory controlCategory)
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
                            if (e.ClickedItem is StyleShowControl control)
                            {
                                Transformer transformer = this.SelectionViewModel.Transformer;
                                IStyle style = control.Style2;

                                IStyle style3 = style.Clone();
                                style3.CacheTransform();
                                style3.DeliverBrushPoints(transformer);
                                this.SelectionViewModel.SetStyle(style3);

                                this.MethodViewModel.ILayerChanged<IStyle>
                                (
                                    set: (layer) =>
                                    {
                                        Transformer transformer2 = layer.Transform.Transformer;
                                        IStyle style2 = style.Clone();
                                        style2.CacheTransform();
                                        style2.DeliverBrushPoints(transformer);
                                        layer.Style = style2;

                                        this.SelectionViewModel.StandardStyleLayer = layer;
                                    },

                                    type: HistoryType.LayersProperty_SetStyle,
                                    getUndo: (layer) => layer.Style,
                                    setUndo: (layer, previous) => layer.Style = previous.Clone()
                                );
                            }
                        }
                        break;
                    case MainPageState.Rename:
                        {
                            if (e.ClickedItem is StyleShowControl control)
                            {
                                string name = control.Style2.Name;
                                string placeholderText = string.IsNullOrEmpty(name) ? this.Untitled : name;
                                string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(placeholderText);
                                if (string.IsNullOrEmpty(rename)) return;

                                control.Style2.Name = rename;
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


            this.AddStyleButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    Transformer transformer = layer.Transform.Transformer;
                    IStyle style = layer.Style;
                    this.AddControl(style, transformer, this.Untitled);
                    await this.Save();
                }
            };

            this.RenameStyleButton.Tapped += (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.State = MainPageState.Rename;
            };
            this.RenameStyleCancelButton.Tapped += (s, e) => this.State = MainPageState.None;

            this.DeleteStyleButton.Tapped += (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.State = MainPageState.Delete;
            };
            this.DeleteStyleOKButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.DeleteControls(this.SelectedControls.ToArray());
                await this.Save();
                this.State = MainPageState.None;
            };
            this.DeleteCancelButton.Tapped += (s, e) => this.State = MainPageState.None;


            this.AddStyleCategoryButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.AddStyleShowControlCategory(this.Untitled);
                await this.Save();
            };
            this.RenameStyleCategoryButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.RenameSelectedControlCategory();
                await this.Save();
            };
            this.DeleteStyleCategoryButton.Tapped += async (s, e) =>
            {
                this.MoreFlyout.Hide();
                this.RemoveSelectedControlCategory();
                await this.Save();
            };
        }

        private async Task Save()
        {
            await XML.SaveStylesFile(this.ToStyleCategorys());
        }

    }

    public sealed partial class StyleMenu : Expander
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


        public void AddStyleCategorys(IEnumerable<StyleCategory> styleCategorys)
        {
            foreach (StyleCategory styleCategory in styleCategorys)
            {
                StyleShowControlCategory controlategory = new StyleShowControlCategory
                {
                    Name = styleCategory.Name
                };
                foreach (IStyle style in styleCategory.Styles)
                {
                    controlategory.Add(new StyleShowControl
                    {
                        Style2 = style
                    });
                }
                this.ControlCategorys.Add(controlategory);
            }

            this.SelectedControlCategory = this.ControlCategorys.FirstOrDefault();
        }

        public IEnumerable<StyleCategory> ToStyleCategorys()
        {
            return
            (
                from controlCategory
                in this.ControlCategorys
                select new StyleCategory
                {
                    Name = controlCategory.Name,
                    Styles =
                    (
                        from control
                        in controlCategory
                        select control.Style2
                    )
                }
            );
        }

    }

    public sealed partial class StyleMenu : Expander
    {

        public void AddControl(IStyle style, Transformer transformer, string rename)
        {
            if (string.IsNullOrEmpty(rename)) return;

            IStyle style2 = style.Clone();
            style2.Name = rename;
            style2.CacheTransform();
            style2.OneBrushPoints(transformer);

            if (this.SelectedControlCategory is StyleShowControlCategory controlCategory)
            {
                controlCategory.Add(new StyleShowControl
                {
                    Style2 = style2
                });
            }
            else
            {
                StyleShowControlCategory controlCategory2 = new StyleShowControlCategory
                {
                    Name = rename
                };
                controlCategory2.Add(new StyleShowControl
                {
                    Style2 = style2
                });
                this.ControlCategorys.Add(controlCategory2);

                this.SelectedControlCategory = this.ControlCategorys.LastOrDefault();
            }
        }

        public void DeleteControls(StyleShowControl[] controls)// You can not remove an StyleShowControl by an IEnumerable
        {
            if (this.SelectedControlCategory is StyleShowControlCategory controlCategory)
            {
                foreach (StyleShowControl control in controls)
                {
                    if (controlCategory.Contains(control))
                    {
                        controlCategory.Remove(control);
                    }
                }
            }
        }


        public void AddStyleShowControlCategory(string rename)
        {
            if (string.IsNullOrEmpty(rename)) return;

            StyleShowControlCategory controlCategory = new StyleShowControlCategory { Name = rename };
            this.ControlCategorys.Add(controlCategory);
            this.SelectedControlCategory = controlCategory;
        }

        public async void RenameSelectedControlCategory()
        {
            if (this.SelectedControlCategory is StyleShowControlCategory controlCategory)
            {
                string rename = await Retouch_Photo2.DrawPage.ShowRenameFunc(controlCategory.Name);
                if (string.IsNullOrEmpty(rename)) return;

                controlCategory.Name = rename;
            }
        }

        public void RemoveSelectedControlCategory()
        {
            if (this.SelectedControlCategory is StyleShowControlCategory controlCategory)
            {
                this.ControlCategorys.Remove(controlCategory);
            }
            this.SelectedControlCategory = this.ControlCategorys.FirstOrDefault();
        }
    }
}