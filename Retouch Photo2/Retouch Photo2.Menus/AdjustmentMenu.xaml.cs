using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Adjustments.Pages;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "AdjustmentMenu" />. 
    /// </summary>
    public sealed partial class AdjustmentMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@VisualState
        List<IAdjustment> _vsAdjustments;
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
        

        private IAdjustment adjustment;
        public IAdjustment Adjustment
        {
            get => this.adjustment;
            set
            {
                if (this.adjustment == value) return;

                if (value == null)
                {
                    if (this.adjustment != null) this.adjustment.Close();
                    this.AdjustmentPageBorder.Child = null;
                }
                else
                {
                    if (this.AdjustmentPageBorder.Child != value.Page.Self)
                    {
                        this.AdjustmentPageBorder.Child = value.Page.Self;
                    }

                    value.Follow();
                }

                this.adjustment = value;
            }
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

            if (e.NewValue is Filter value)
            {
                con._vsAdjustments = value.Adjustments;
            }
            else
            {
                con._vsAdjustments = null;
            }

            con.VisualState = con.VisualState;//State
        }));


        #endregion

        public bool AdjustmentPageOrFilters
        {
            set
            {
                this.AdjustmentPageBorder.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.FiltersListView.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
            }
        }


        //@Construct
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

            Filter.Invalidate += () => this.ViewModel.Invalidate();
            this.ConstructAdjustmentPageListView();
            this.ConstructFilterListView();

            this.Loaded += async (s, e) =>
            {
                if (this.AdjustmentPageListView.ItemsSource == null)
                {
                    this.AdjustmentPageListView.ItemsSource = new List<IAdjustmentPage>()
                    {
                         GrayAdjustment.GrayPage,
                         InvertAdjustment.InvertPage,

                         ExposureAdjustment.ExposurePage,
                         BrightnessAdjustment.BrightnessPage,
                         SaturationAdjustment.SaturationPage,
                         HueRotationAdjustment.HueRotationPage,
                         ContrastAdjustment.ContrastPage,
                         TemperatureAdjustment.TemperaturePage,

                         HighlightsAndShadowsAdjustment.HighlightsAndShadowsPage,
                         GammaTransferAdjustment.GammaTransferPage,
                         VignetteAdjustment.VignettePage,
                    };
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
            AdjustmentMenu.GetGridDataContext(sender, out IAdjustment adjustment);

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
            AdjustmentMenu.GetGridDataContext(sender, out IAdjustment adjustment);

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.Filter.Adjustments.Remove(adjustment);//Remove

                this._vsAdjustments = layer.Filter.Adjustments;
                this.VisualState = this.VisualState;//State

                this.InvalidateItemsControl();//Invalidate
                this.ViewModel.Invalidate();//Invalidate   
                return;
            });
        }

    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "AdjustmentMenu" />. 
    /// </summary>
    public sealed partial class AdjustmentMenu : UserControl, IMenu
    {    
        //DataContext
        public void ConstructDataContext(object dataContext, string path, DependencyProperty dp)
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
        public MenuType Type => MenuType.Adjustment;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Adjustments.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Reset = this.Reset;
            this._Expander.Initialize();
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "AdjustmentMenu" />. 
    /// </summary>
    public sealed partial class AdjustmentMenu : UserControl, IMenu
    {

        private void ConstructAdjustmentPageListView()
        {
            this.AdjustmentPageListView.ItemClick += (s, e) =>
            {
                this.AdjustmentPageFlyout.Hide();

                if (e.ClickedItem is IAdjustmentPage item)
                {
                    //Selection
                    this.SelectionViewModel.SetValue((layerage) =>
                    {
                        ILayer layer = layerage.Self;

                        IAdjustment _new = item.GetNewAdjustment();
                        layer.Filter.Adjustments.Add(_new);//Add

                        this._vsAdjustments = layer.Filter.Adjustments;
                        this.VisualState = this.VisualState;//State

                        this.InvalidateItemsControl();//Invalidate
                        this.ViewModel.Invalidate();//Invalidate
                        return;
                    });
                }
            };
        }

        private void ConstructFilterListView()
        {
            this.FiltersListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Filter filter)
                {
                    IEnumerable<IAdjustment> clones = from a in filter.Adjustments select a.Clone();

                    //Selection
                    this.SelectionViewModel.SetValue((layerage) =>
                    {
                        ILayer layer = layerage.Self;

                        layer.Filter.Adjustments.Clear();//Clear
                        layer.Filter.Adjustments.AddRange(clones);//Add

                        this._vsAdjustments = layer.Filter.Adjustments;
                        this.VisualState = this.VisualState;//State

                        this.InvalidateItemsControl();//Invalidate
                        this.ViewModel.Invalidate();//Invalidate     
                        return;
                    });
                }
            };
        }


        private void Reset()
        {
            if (this.Adjustment == null) return;

            this.Adjustment.Reset();
            this.ViewModel.Invalidate();
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


        //@Static
        /// <summary>
        /// Get the data context of the Grid.
        /// </summary>
        /// <param name="sender"> Button. </param>
        /// <param name="adjustment"> DataContext. </param>
        public static void GetGridDataContext(object sender, out IAdjustment adjustment)
        {
            if (sender is Button button)
            {
                if (button.Parent is Grid rootGrid)
                {
                    if (rootGrid.DataContext is IAdjustment adjustment2)
                    {
                        adjustment = adjustment2;
                        return;
                    }
                }
            }

            adjustment = null;
        }

    }
}