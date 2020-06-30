using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Adjustments.Pages;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Filters;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
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
    public sealed partial class AdjustmentMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


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

            this.ItemsControl.ItemsSource = null;
            this.ItemsControl.ItemsSource = this._vsAdjustments;
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "AdjustmentMenu" />'s filter. </summary>
        public Filter Filter
        {
            get { return (Filter)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }
        /// <summary> Identifies the <see cref = "AdjustmentMenu.Filter" /> dependency property. </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(Filter), typeof(AdjustmentMenu), new PropertyMetadata(null, (sender, e) =>
        {
            AdjustmentMenu con = (AdjustmentMenu)sender;

            con._Expander.IsSecondPage = false;
            con._Expander.CurrentTitle = con._Expander.Title;

            if (e.NewValue is Filter value)
            {
                con._vsAdjustments = value.Adjustments;
                con.VisualState = con.VisualState;//State
            }
            else
            {
                con._vsAdjustments = null;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion


        /// <summary> Gets or sets the current adjustment. </summary>
        public IAdjustment Adjustment
        {
            get => this.adjustment;
            set
            {
                if (this.adjustment == value) return;

                if (value == null)
                {
                    if (this.adjustment != null)
                    {
                        this.adjustment.Close();
                    }
                    this.AdjustmentPageBorder.Child = null;
                }
                else
                {
                    if (this.AdjustmentPageBorder.Child != value.Page)
                    {
                        this.AdjustmentPageBorder.Child = value.Page;
                    }

                    value.Follow();
                }

                this.adjustment = value;
            }
        }
        private IAdjustment adjustment;

        /// <summary> Sets the state of the menu page. </summary>
        public bool AdjustmentPageOrFilters
        {
            set
            {
                this.AdjustmentPageBorder.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.FiltersListView.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a AdjustmentMenu. 
        /// </summary>
        public AdjustmentMenu()
        {
            this.InitializeComponent();
            this.ConstructDataContext
            (
                 dataContext: this.SelectionViewModel,
                 path: nameof(this.SelectionViewModel.Filter),
                 dp: AdjustmentMenu.FilterProperty
            );
            this.ConstructStrings();
            this.ConstructMenu();


            this.Loaded += async (s, e) =>
            {
                if (this.AdjustmentPageListView.ItemsSource == null)
                {
                    this.AdjustmentPageListView.ItemsSource = this.GetAdjustmentPages().ToList();
                }

                if (this.FiltersListView.ItemsSource == null)
                {
                    IEnumerable<FilterCategory> filterCategorys = await Retouch_Photo2.XML.ConstructFiltersFile();
                    if (filterCategorys != null)
                    {
                        FilterCategory filterCategory = filterCategorys.FirstOrDefault();
                        if (filterCategory != null)
                        {
                            IEnumerable<Filter> filters = filterCategory.Filters;
                            this.FiltersListView.ItemsSource = filters.ToList();
                        }
                    }
                }

                this.VisualState = this.VisualState;//State
            };


            this.AdjustmentPageListView.ItemClick += (s, e) =>
            {
                this.AdjustmentPageFlyout.Hide();

                if (e.ClickedItem is IAdjustmentPage item)
                {
                    this.FilterAdd(item);
                }
            };
            this.FiltersListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Filter filter)
                {
                    this.FilterChanged(filter);
                }
            };


            this.AddButton.Click += (s, e) => this.AdjustmentPageFlyout.ShowAt(this.AddButton);
            this.FiltersButton.Click += (s, e) =>
            {
                this.AdjustmentPageOrFilters = false;
                this._Expander.IsSecondPage = true;
                this._Expander.CurrentTitle = (string)this.FiltersButton.Content;
            };
        }


        //@DataTemplate
        /// <summary> DataTemplate's EditButton Tapped. </summary>
        private void EditButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IAdjustment adjustment = this.GetGridDataContext(sender);
            if (adjustment == null) return;
            if (adjustment.PageVisibility == Visibility.Collapsed) return;

            this.AdjustmentPageOrFilters = true;
            this._Expander.IsSecondPage = true;
            this._Expander.ResetButtonVisibility = Visibility.Visible;
            this._Expander.CurrentTitle = adjustment.Text;

            this.Adjustment = adjustment;
        }
        /// <summary> DataTemplate's RemoveButton Tapped. </summary>
        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IAdjustment adjustment = this.GetGridDataContext(sender);
            if (adjustment == null) return;

            this.FilterRemove(adjustment);
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Adjustments.IAdjustment"/>.
    /// </summary>
    public sealed partial class AdjustmentMenu : UserControl, IMenu
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

            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/Adjustment");

            this.ZeroTextBlock.Text = resource.GetString("/Menus/Adjustment_ZeroTip");
            this.DisableTextBlock.Text = resource.GetString("/Menus/Adjustment_DisableTip");

            this.AddButton.Content = resource.GetString("/Menus/Adjustment_Add");
            this.FiltersButton.Content = resource.GetString("/Menus/Adjustment_Filters");
        }

        //Menu      
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Adjustment;
        /// <summary> Gets the expander. </summary>
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Adjustments.Icon()
        };

        private void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Reset = this.Reset;
            this._Expander.Initialize();
        }

        private void Reset()
        {
            if (this.Adjustment == null) return;

            this.Adjustment.Reset();
            this.ViewModel.Invalidate();//Invalidate
        }
    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Adjustments.IAdjustment"/>.
    /// </summary>
    public sealed partial class AdjustmentMenu : UserControl, IMenu
    {

        //@Generic
        private IEnumerable<IAdjustmentPage> GetAdjustmentPages()
        {
            var grayPage = new GrayPage();
            GrayAdjustment.GenericPage = grayPage;
            yield return grayPage;

            var invertPage = new InvertPage();
            InvertAdjustment.GenericPage = invertPage;
            yield return invertPage;

            var exposurePage = new ExposurePage();
            ExposureAdjustment.GenericPage = exposurePage;
            yield return exposurePage;

            var brightnessPage = new BrightnessPage();
            BrightnessAdjustment.GenericPage = brightnessPage;
            yield return brightnessPage;

            var saturationPage = new SaturationPage();
            SaturationAdjustment.GenericPage = saturationPage;
            yield return saturationPage;

            var hueRotationPage = new HueRotationPage();
            HueRotationAdjustment.GenericPage = hueRotationPage;
            yield return hueRotationPage;

            var contrastPage = new ContrastPage();
            ContrastAdjustment.GenericPage = contrastPage;
            yield return contrastPage;

            var temperaturePage = new TemperaturePage();
            TemperatureAdjustment.GenericPage = temperaturePage;
            yield return temperaturePage;

            var highlightsAndShadowsPage = new HighlightsAndShadowsPage();
            HighlightsAndShadowsAdjustment.GenericPage = highlightsAndShadowsPage;
            yield return highlightsAndShadowsPage;

            var gammaTransferPage = new GammaTransferPage();
            GammaTransferAdjustment.GenericPage = gammaTransferPage;
            yield return gammaTransferPage;

            var vignettePage = new VignettePage();
            VignetteAdjustment.GenericPage = vignettePage;
            yield return vignettePage;
        }


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
            LayersPropertyHistory history = new LayersPropertyHistory("Set layer filter");

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
            LayersPropertyHistory history = new LayersPropertyHistory("Set layer filter");

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
            LayersPropertyHistory history = new LayersPropertyHistory("Set layer filter");

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