// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Adjustments.Pages;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Filters;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Adjustments.IAdjustment"/>.
    /// </summary>
    public sealed partial class AdjustmentMenu : Expander, IMenu
    {

        //@Content
        public bool IsOpen { set { } }
        public override UIElement MainPage => this.AdjustmentMainPage;

        readonly AdjustmentMainPage AdjustmentMainPage = new AdjustmentMainPage();


        //@Construct
        /// <summary>
        /// Initializes a AdjustmentMenu. 
        /// </summary>
        public AdjustmentMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.AdjustmentMainPage.IsSecondPageChanged += (s, isSecondPage) => this.Back();
            this.AdjustmentMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.Page != secondPage) this.Page = secondPage;
                this.IsSecondPage = true;
                this.Title = (string)title;
                this.ResetButtonVisibility = Visibility.Visible;
            };
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Adjustments.IAdjustment"/>.
    /// </summary>
    public sealed partial class AdjustmentMenu : Expander, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = resource.GetString("Menus_Adjustment");
        }

        //Menu      
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Adjustment;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            Content = new Retouch_Photo2.Adjustments.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset()
        {
            this.AdjustmentMainPage.Reset();
        }

    }

    /// <summary>
    /// MainPage of <see cref = "AdjustmentMenu"/>.
    /// </summary>
    public sealed partial class AdjustmentMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Delegate
        /// <summary> Occurs when is-second-page change. </summary>
        public event EventHandler<bool> IsSecondPageChanged;
        /// <summary> Occurs when second-page change. </summary>
        public event EventHandler<UIElement> SecondPageChanged;

        //@Content
        /// <summary> Filter ListView. </summary>
        public ListView FilterListView = new ListView
        {
            MinHeight = 165,
            MaxHeight = 300
        };

        private readonly IEnumerable<IAdjustmentPage> AdjustmentPages = new List<IAdjustmentPage>()
        {
            new GrayPage(),
            new InvertPage(),
            new ExposurePage(),
            new BrightnessPage(),
            new SaturationPage(),
            new HueRotationPage(),
            new ContrastPage(),
            new TemperaturePage(),
            new HighlightsAndShadowsPage(),
            new GammaTransferPage(),
            new VignettePage(),
        };


        //@VisualState
        IList<IAdjustment> _vsAdjustments;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsAdjustments == null)
                {
                    this.TextBlock.Visibility = Visibility.Collapsed;
                    return this.Disable;
                }

                this.TextBlock.Text = this._vsAdjustments.Count.ToString();
                this.TextBlock.Visibility = Visibility.Visible;

                if (this._vsAdjustments.Count == 0)
                {
                    return this.ZeroAdjustments;
                }
                else
                {
                    this.InvalidateItemsControl();//Invalidate
                    return this.Adjustments;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        /// <summary>
        ///  Invalidate Adjustment ItemsControl.
        /// </summary>
        private void InvalidateItemsControl()
        {
            if (this._vsAdjustments == null) return;

            this.ListView.ItemsSource = null;
            this.ListView.ItemsSource = this._vsAdjustments;
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "AdjustmentMainPage" />'s filter. </summary>
        public Filter Filter
        {
            get => (Filter)base.GetValue(FilterProperty);
            set => base.SetValue(FilterProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdjustmentMainPage.Filter" /> dependency property. </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(Filter), typeof(AdjustmentMainPage), new PropertyMetadata(null, (sender, e) =>
        {
            AdjustmentMainPage control = (AdjustmentMainPage)sender;

            if (e.NewValue is Filter value)
            {
                control._vsAdjustments = value.Adjustments;
                control.VisualState = control.VisualState;//State
            }
            else
            {
                control._vsAdjustments = null;
                control.VisualState = control.VisualState;//State
            }

            control.IsSecondPageChanged?.Invoke(control, false);//Delegate
        }));


        #endregion


        /// <summary> Gets or sets the current adjustment page. </summary>
        public IAdjustmentPage AdjustmentPage;

        //@Construct
        /// <summary>
        /// Initializes a AdjustmentMainPage. 
        /// </summary>
        public AdjustmentMainPage()
        {
            this.InitializeComponent();
            this.ConstructDataContext
            (
                dataContext: this.SelectionViewModel,
                path: nameof(this.SelectionViewModel.Filter),
                dp: AdjustmentMainPage.FilterProperty
            );
            this.ConstructStrings();

            if (this.AdjustmentPageListView.ItemsSource == null)
            {
                this.ConstructAdjustment();
            }

            this.Loaded += async (s, e) =>
            {
                if (this.FilterListView.ItemsSource == null)
                {
                    IEnumerable<FilterCategory> filterCategorys = await Retouch_Photo2.XML.ConstructFiltersFile();
                    this.ConstructFilter(filterCategorys);
                }

                this.VisualState = this.VisualState;//State
            };

            AdjustmentCommand.Edit = this.EditAction;
            AdjustmentCommand.Remove = this.RemoveAction;
        }


        private void EditAction(IAdjustment adjustment)
        {
            if (adjustment.PageVisibility == Visibility.Collapsed) return;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                this.AdjustmentPage = adjustment.Page;
                int index = layer.Filter.Adjustments.IndexOf(adjustment);
                adjustment.Page.Index = index;
                adjustment.Page.Follow();

                object title = adjustment.Text;
                UIElement secondPage = adjustment.Page.Self;
                this.SecondPageChanged?.Invoke(title, secondPage);//Delegate
            }
        }
        private void RemoveAction(IAdjustment adjustment)
        {
            this.FilterRemove(adjustment);
        }

    }

    /// <summary>
    /// MainPage of <see cref = "AdjustmentMenu"/>.
    /// </summary>
    public sealed partial class AdjustmentMainPage : UserControl
    {

        //DataContext
        private void ConstructDataContext(object dataContext, string path, DependencyProperty dp)
        {
            this.DataContext = dataContext;

            // Create the binding description.
            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(path)
            };

            // Attach the binding to the target.
            this.SetBinding(dp, binding);
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ZeroTextBlock.Text = resource.GetString("Menus_Adjustment_ZeroTip");
            this.DisableTextBlock.Text = resource.GetString("Menus_Adjustment_DisableTip");

            this.AddButton.Content = resource.GetString("Menus_Adjustment_Add");
            this.FilterButton.Content = resource.GetString("Menus_Adjustment_Filters");


            foreach (IAdjustmentPage adjustmentPage in this.AdjustmentPages)
            {
                AdjustmentType type = adjustmentPage.Type;

                ResourceDictionary resources = new ResourceDictionary
                {
                    //@Template
                    Source = new Uri($@"ms-appx:///Retouch Photo2.Adjustments\Icons\{type}Icon.xaml")
                };
                adjustmentPage.Icon = resources[$"{type}Icon"] as ControlTemplate;
            }

        }

        public void Reset()
        {
            if (this.AdjustmentPage is IAdjustmentPage adjustmentPage)
            {
                adjustmentPage.Reset();
                this.ViewModel.Invalidate();//Invalidate
            }
        }
    }

    /// <summary>
    /// MainPage of <see cref = "AdjustmentMenu"/>.
    /// </summary>
    public sealed partial class AdjustmentMainPage : UserControl
    {

        //Filter
        private void ConstructFilter(IEnumerable<FilterCategory> filterCategorys)
        {
            this.FilterListView.IsItemClickEnabled = true;
            this.FilterListView.SelectionMode = ListViewSelectionMode.Single;

            this.FilterListView.ItemTemplate = this.FilterDataTemplate;
            this.FilterListView.ItemContainerStyle = this.FilterItemStyle;

            this.FilterListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Filter filter)
                {
                    this.FilterChanged(filter);
                }
            };

            if (filterCategorys != null)
            {
                FilterCategory filterCategory = filterCategorys.FirstOrDefault();
                if (filterCategory != null)
                {
                    IEnumerable<Filter> filters = filterCategory.Filters;
                    this.FilterListView.ItemsSource = filters.ToList();
                }
            }

            this.FilterButton.Click += (s, e) =>
            {
                object title = this.FilterButton.Content;
                UIElement secondPage = this.FilterListView;
                this.SecondPageChanged?.Invoke(title, secondPage);//Delegate
            };
        }

        //Adjustment
        private void ConstructAdjustment()
        {
            this.AddButton.Click += (s, e) => this.AdjustmentPageFlyout.ShowAt(this.AddButton);

            this.AdjustmentPageListView.ItemsSource = this.AdjustmentPages;

            this.AdjustmentPageListView.ItemClick += (s, e) =>
            {
                this.AdjustmentPageFlyout.Hide();

                if (e.ClickedItem is IAdjustmentPage item)
                {
                    this.FilterAdd(item);
                }
            };
        }

    }

    /// <summary>
    /// MainPage of <see cref = "AdjustmentMenu"/>.
    /// </summary>
    public sealed partial class AdjustmentMainPage : UserControl
    {

        /// <summary>
        /// Get the data context of the Grid.
        /// </summary>
        /// <param name="sender"> Button. </param>
        private IAdjustment GetGridDataContext(object sender)
        {
            if (sender is Button button)
            {
                if (button.Parent is Grid rootGrid)
                {
                    if (rootGrid.DataContext is IAdjustment adjustment)
                    {
                        return adjustment;
                    }
                }
            }

            return null;
        }



        private void FilterAdd(IAdjustmentPage adjustmentPage)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetFilter);

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = layer.Filter.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Filter = previous.Clone();
                };


                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Filter.Adjustments.Add(adjustmentPage.GetNewAdjustment());

                this._vsAdjustments = layer.Filter.Adjustments;
            });

            //History
            this.ViewModel.HistoryPush(history);


            this.VisualState = this.VisualState;//State

            this.InvalidateItemsControl();//Invalidate
            this.ViewModel.Invalidate();//Invalidate     
        }


        private void FilterRemove(IAdjustment removeAdjustment)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetFilter);

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;


                var previous = layer.Filter.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Filter = previous.Clone();
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Filter.Adjustments.Remove(removeAdjustment);

                this._vsAdjustments = layer.Filter.Adjustments;
            });

            //History
            this.ViewModel.HistoryPush(history);


            this.VisualState = this.VisualState;//State

            this.InvalidateItemsControl();//Invalidate
            this.ViewModel.Invalidate();//Invalidate     
        }


        private void FilterChanged(Filter filter)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetFilter);

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = layer.Filter.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Filter = previous.Clone();
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Filter = filter.Clone();

                this._vsAdjustments = layer.Filter.Adjustments;
            });

            //History
            this.ViewModel.HistoryPush(history);


            this.VisualState = this.VisualState;//State

            this.InvalidateItemsControl();//Invalidate
            this.ViewModel.Invalidate();//Invalidate     
        }

    }
}