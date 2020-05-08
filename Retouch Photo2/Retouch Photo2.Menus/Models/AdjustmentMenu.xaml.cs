using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Adjustments.Pages;
using Retouch_Photo2.Elements;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


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


        /// <summary> Gets or sets <see cref = "AdjustmentMenu" />'s adjustment manager. </summary>
        public AdjustmentManager AdjustmentManager
        {
            get { return (AdjustmentManager)GetValue(AdjustmentManagerProperty); }
            set { SetValue(AdjustmentManagerProperty, value); }
        }
        /// <summary> Identifies the <see cref = "AdjustmentMenu.AdjustmentManager" /> dependency property. </summary>
        public static readonly DependencyProperty AdjustmentManagerProperty = DependencyProperty.Register(nameof(AdjustmentManager), typeof(AdjustmentManager), typeof(AdjustmentMenu), new PropertyMetadata(null, (sender, e) =>
        {
            AdjustmentMenu con = (AdjustmentMenu)sender;

            con._Expander.IsSecondPage = false;

            if (e.NewValue is AdjustmentManager value)
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
                 path: nameof(this.SelectionViewModel.AdjustmentManager),
                 dp: AdjustmentMenu.AdjustmentManagerProperty
            );
            this.ConstructStrings();
            this.ConstructMenu();

            AdjustmentManager.Invalidate += () => this.ViewModel.Invalidate();
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
                    IEnumerable<Filter> source = await FileUtil.ConstructFilterFile();
                    this.FiltersListView.ItemsSource = source.ToList();
                }

                this.VisualState = this.VisualState;//State
            };

            this.AddButton.Tapped += (s, e) => this.AdjustmentPageFlyout.ShowAt(this.AddButton);
            this.FiltersButton.Tapped += (s, e) =>
            {
                this.AdjustmentPageOrFilters = false;
                this._Expander.IsSecondPage = true;
            };
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
                    this.SelectionViewModel.SetValue((layer) =>
                    {
                        IAdjustment _new = item.GetNewAdjustment();
                        layer.AdjustmentManager.Adjustments.Add(_new);//Add

                        this._vsAdjustments = layer.AdjustmentManager.Adjustments;
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
                    this.SelectionViewModel.SetValue((layer) =>
                    {
                        layer.AdjustmentManager.Adjustments.Clear();//Clear
                        layer.AdjustmentManager.Adjustments.AddRange(clones);//Add

                        this._vsAdjustments = layer.AdjustmentManager.Adjustments;
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


        //@DataTemplate
        /// <summary> DataTemplate's EditButton Tapped. </summary>
        private void EditButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AdjustmentMenu.GetGridDataContext(sender, out IAdjustment adjustment);

            if (adjustment == null) return;
            if (adjustment.PageVisibility == Visibility.Collapsed) return;

            this.AdjustmentPageOrFilters = true;
            this._Expander.IsSecondPage = true;

            this.Adjustment = adjustment;
        }
        /// <summary> DataTemplate's RemoveButton Tapped. </summary>
        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AdjustmentMenu.GetGridDataContext(sender, out IAdjustment adjustment);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.AdjustmentManager.Adjustments.Remove(adjustment);//Remove

                this._vsAdjustments = layer.AdjustmentManager.Adjustments;
                this.VisualState = this.VisualState;//State

                this.InvalidateItemsControl();//Invalidate
                this.ViewModel.Invalidate();//Invalidate   
                return;
            });
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

            this._button.ToolTip.Content = resource.GetString("/Menus/Adjustment");
            this._Expander.Title = resource.GetString("/Menus/Adjustment");

            this.ZeroTextBlock.Text = resource.GetString("/Menus/Adjustment_ZeroTip");
            this.DisableTextBlock.Text = resource.GetString("/Menus/Adjustment_DisableTip");

            this.AddButton.Content = resource.GetString("/Menus/Adjustment_Add");
            this.FiltersButton.Content = resource.GetString("/Menus/Adjustment_Filters");
        }

        //@Delegate
        public Action Move { get; set; }
        public Action Closed { get; set; }
        public Action Opened { get; set; }


        //@Content
        public MenuType Type => MenuType.Adjustment;
        public FlyoutPlacementMode PlacementMode { get; set; } = FlyoutPlacementMode.Bottom;
        public Point Postion { get; set; }
        public FrameworkElement Layout => this;
        public FrameworkElement Button => this._button;
        private MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Adjustments.Icon()
        };

        public MenuState State
        {
            get => this.state;
            set
            {
                this._button.State = value;
                this._Expander.State = value;
                MenuHelper.SetMenuState(value, this);
                this.state = value;
            }
        }
        private MenuState state;


        //@Construct  
        public void ConstructMenu()
        {
            this.State = MenuState.Hide;
            this.Button.Tapped += (s, e) => this.State = MenuHelper.GetState(this.State);
            this._Expander.CloseButton.Tapped += (s, e) => this.State = MenuState.Hide;
            this._Expander.StateButton.Tapped += (s, e) => this.State = MenuHelper.GetState2(this.State);
            this._Expander.ResetButton.Tapped += (s, e) => this.Reset();
            this._Expander.BackButton.Tapped += (s, e) => this._Expander.IsSecondPage = false;
            MenuHelper.ConstructTitleGrid(this._Expander.TitleGrid, this);
        }


    }
}