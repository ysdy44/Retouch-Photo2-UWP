using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Adjustments.Pages;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "AdjustmentControl" />. 
    /// </summary>
    public sealed partial class AdjustmentControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        

        //@Content
        public MenuTitle MenuTitle => this._MenuTitle;


        /// <summary> Manager of <see cref="AdjustmentControlState"/>. </summary>
        AdjustmentControlStateManager Manager = new AdjustmentControlStateManager();
        /// <summary> State of <see cref="AdjustmentControl"/>. </summary>
        AdjustmentControlState State
        {
            set
            {
                switch (value)
                {
                    case AdjustmentControlState.None: VisualStateManager.GoToState(this, this.Normal.Name, false); break;
                    case AdjustmentControlState.Disable: VisualStateManager.GoToState(this, this.Disable.Name, false); break;

                    case AdjustmentControlState.ZeroAdjustments: VisualStateManager.GoToState(this, this.ZeroAdjustments.Name, false); break;
                    case AdjustmentControlState.Adjustments: VisualStateManager.GoToState(this, this.Adjustments.Name, false); break;
                        
                    case AdjustmentControlState.Edit: VisualStateManager.GoToState(this, this.Edit.Name, false); break;
                    case AdjustmentControlState.Filters: VisualStateManager.GoToState(this, this.Filters.Name, false); break;
                }
            }
        }


        private IAdjustmentPage page;
        public IAdjustmentPage Page
        {
            get => this.page;
            set
            {
                if (value == null)
                {
                    if (this.page != null) this.page.Close();
                    this.AdjustmentPageBorder.Child = null;
                }
                else this.AdjustmentPageBorder.Child = value.Page;

                this.page = value;
            }
        }
        public List<IAdjustmentPage> PageList = new List<IAdjustmentPage>()
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


        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "AdjustmentControl" />'s adjustment manager. </summary>
        public AdjustmentManager AdjustmentManager
        {
            get { return (AdjustmentManager)GetValue(AdjustmentManagerProperty); }
            set { SetValue(AdjustmentManagerProperty, value); }
        }
        /// <summary> Identifies the <see cref = "AdjustmentControl.AdjustmentManager" /> dependency property. </summary>
        public static readonly DependencyProperty AdjustmentManagerProperty = DependencyProperty.Register(nameof(AdjustmentManager), typeof(AdjustmentManager), typeof(AdjustmentControl), new PropertyMetadata(null, (sender, e) =>
        {
            AdjustmentControl con = (AdjustmentControl)sender;

            con.Manager.IsEdit = false;
            con.Manager.IsFilter = false;
            con.Manager.Adjustments = null;

            if (e.NewValue is AdjustmentManager value)
            {
                con.Manager.Adjustments = value.Adjustments;
            }

            con.State = con.Manager.GetState();//State
        }));

        #endregion


        //@Construct
        public AdjustmentControl()
        {
            this.InitializeComponent();
            this.State = AdjustmentControlState.Disable;
            this.Loaded += async (s, e) =>
            {
                if (this.AdjustmentPageListView.ItemsSource == null)
                {
                    this.AdjustmentPageListView.ItemsSource = this.PageList;
                }

                if (this.FilterListView.ItemsSource == null)
                {
                    IEnumerable<Filter> source =await FilterHelper.GetFilterSource();
                    this.FilterListView.ItemsSource = source.ToList();
                }
            };


            //Adjustment
            AdjustmentManager.Invalidate = () => this.ViewModel.Invalidate(); 


            //Button
            this.AddButton.Tapped += (s, e) => this.AdjustmentPageFlyout.ShowAt(this.AddButton);
            this.FilterButton.Tapped += (s, e) =>
            {
                this.Manager.IsFilter = true;
                this.State = this.Manager.GetState();//State
            };


            //Menu
            this._MenuTitle.ResetButton.Tapped += (s, e) =>
            {
                if (this.Page == null) return;

                this.Page.Reset();
                this.ViewModel.Invalidate();
            };
            this._MenuTitle.BackButton.Tapped += (s, e) =>
            {
                this.Page = null;

                this.Manager.IsEdit = false;
                this.Manager.IsFilter = false;
                this.State = this.Manager.GetState();//State
            };


            //ListView
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

                        this.Manager.Adjustments = layer.AdjustmentManager.Adjustments;
                        this.State = this.Manager.GetState();//State

                        this.InvalidateItemsControl();//Invalidate
                        this.ViewModel.Invalidate();//Invalidate
                        return;
                    });
                }
            };


            //ListView
            this.FilterListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Filter filter)
                {
                    IEnumerable<IAdjustment> clones = from a in filter.Adjustments select a.Clone();

                    //Selection
                    this.SelectionViewModel.SetValue((layer) =>
                    {
                        layer.AdjustmentManager.Adjustments.Clear();//Clear
                        layer.AdjustmentManager.Adjustments.AddRange(clones);//Add
                        
                        this.Manager.Adjustments = layer.AdjustmentManager.Adjustments;
                        this.State = this.Manager.GetState();//State

                        this.InvalidateItemsControl();//Invalidate
                        this.ViewModel.Invalidate();//Invalidate     
                        return;
                    });
                }
            };
        }


        //@DataTemplate
        /// <summary> DataTemplate's EditButton Tapped. </summary>
        private void EditButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AdjustmentControl.GetGridDataContext(sender, out IAdjustment adjustment);

            if (adjustment == null) return;
            if (adjustment.PageVisibility == Visibility.Collapsed) return;

            this.Manager.IsEdit = true;
            this.State = this.Manager.GetState();//State

            AdjustmentType type = adjustment.Type;
            IAdjustmentPage adjustmentPage = this.PageList.First(page => page.Type == type);

            this.Page = adjustmentPage;
            this.Page.SetAdjustment(adjustment);
        }
        /// <summary> DataTemplate's RemoveButton Tapped. </summary>
        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AdjustmentControl.GetGridDataContext(sender, out IAdjustment adjustment);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.AdjustmentManager.Adjustments.Remove(adjustment);//Remove

                this.Manager.Adjustments = layer.AdjustmentManager.Adjustments;
                this.State = this.Manager.GetState();//State

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
            if (this.Manager.Adjustments == null) return;

            this.ItemsControl.ItemsSource = null;
            this.ItemsControl.ItemsSource = this.Manager.Adjustments;
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