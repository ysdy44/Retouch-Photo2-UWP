using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Adjustments.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Newtonsoft.Json;
using Retouch_Photo2.Adjustments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

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


        private AdjustmentControlState state = AdjustmentControlState.None;
        public AdjustmentControlState State
        {
            set
            {
                if (this.state == value) return;
                
                Visibility edit = (value == AdjustmentControlState.Edit) ? Visibility.Visible : Visibility.Collapsed;
                this.BackButton.Visibility = edit;
                this.ResetButton.Visibility = edit;
                this.Frame.Visibility = edit;

                Visibility edit2 = (value == AdjustmentControlState.Edit) ? Visibility.Collapsed : Visibility.Visible;
                this.AddButton.Visibility = edit2;
                this.FilterButton.Visibility = edit2;

                bool sss = (value == AdjustmentControlState.Null || value == AdjustmentControlState.Adjustments);
                this.PageListView.IsEnabled = sss;
                this.FilterGridView.IsEnabled = sss;

                Visibility adjustments = (value == AdjustmentControlState.Adjustments) ? Visibility.Visible : Visibility.Collapsed;
                this.ItemsControl.Visibility = adjustments;

                Visibility disable = (value == AdjustmentControlState.Null || value == AdjustmentControlState.Disable) ? Visibility.Visible : Visibility.Collapsed;
                this.TextBlock.Visibility = disable;

                this.state = value;
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
                    if (this.page != null)
                    {
                        this.page.Close();
                    }

                    this.Frame.Child = null;
                    this.State = AdjustmentControlState.Adjustments;
                }
                else
                {
                    this.Frame.Child = value.Page;
                    this.State = AdjustmentControlState.Edit;
                }

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

            if (e.NewValue is AdjustmentManager value)
            {
                con.Invalidate(value);
            }
            else
            {
                con.State = AdjustmentControlState.Disable;
            }
        }));

        #endregion


        //@Construct
        public AdjustmentControl()
        {
            this.InitializeComponent();
            this.State = AdjustmentControlState.Disable;
            this.Loaded += async (s, e) =>
            {
                if (this.PageListView.ItemsSource == null)
                {
                    this.PageListView.ItemsSource = this.PageList;
                }

                if (this.FilterGridView.ItemsSource == null)
                {
                    IEnumerable<Filter> source =await FilterHelper.GetFilterSource();
                    this.FilterGridView.ItemsSource = source.ToList();
                }
            };


            //Adjustment
            AdjustmentManager.Invalidate = () => this.ViewModel.Invalidate();


            //Button
            this.FilterButton.Tapped += (s, e) => this.FilterFlyout.ShowAt(this.FilterButton);
            this.AddButton.Tapped += (s, e) => this.PageFlyout.ShowAt(this.AddButton);


            //Page
            this.BackButton.Tapped += (s, e) => this.Page = null;
            this.ResetButton.Tapped += (s, e) => this.Reset(this.Page);


            //ListView
            this.PageListView.ItemClick += (s, e) =>
            {
                this.PageFlyout.Hide();

                if (e.ClickedItem is IAdjustmentPage item)
                {
                    //Selection
                    this.SelectionViewModel.SetValue((layer) =>
                    {
                        IAdjustment _new = item.GetNewAdjustment();
                        layer.AdjustmentManager.Adjustments.Add(_new);//Add

                        this.Invalidate(layer.AdjustmentManager);//Invalidate
                        //this.Edit(_new);

                        this.ViewModel.Invalidate();//Invalidate
                        return;
                    });
                }
            };


            //GridView
            this.FilterGridView.ItemClick += (s, e) =>
            {
                this.FilterFlyout.Hide();

                if (e.ClickedItem is Filter filter)
                {
                    IEnumerable<IAdjustment> clones = from a in filter.Adjustments select a.Clone();

                    //Selection
                    this.SelectionViewModel.SetValue((layer) =>
                    {
                        layer.AdjustmentManager.Adjustments.Clear();//Clear
                        layer.AdjustmentManager.Adjustments.AddRange(clones);//Add
                        
                        this.Invalidate(layer.AdjustmentManager);

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

            this.Edit(adjustment);
        }
        /// <summary> DataTemplate's RemoveButton Tapped. </summary>
        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AdjustmentControl.GetGridDataContext(sender, out IAdjustment adjustment);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.AdjustmentManager.Adjustments.Remove(adjustment);//Remove
                this.Invalidate(layer.AdjustmentManager);//Invalidate

                this.ViewModel.Invalidate();//Invalidate   
                return;
            });
        }


        private void Reset(IAdjustmentPage adjustmentPage)
        {
            if (adjustmentPage == null) return;

            adjustmentPage.Reset();
            this.ViewModel.Invalidate();
        }
        private void Edit(IAdjustment adjustment)
        {
            if (adjustment == null) return;
            if (adjustment.PageVisibility == Visibility.Collapsed) return;

            AdjustmentType type = adjustment.Type;
            IAdjustmentPage adjustmentPage = this.PageList.First(page => page.Type == type);

            this.Page = adjustmentPage;
            this.Page.SetAdjustment(adjustment);
        }
        /// <summary>
        ///  Invalidate Adjustment ItemsControl.
        /// </summary>
        /// <param name="adjustmentManager"> The adjustment-manager. </param>
        public void Invalidate(AdjustmentManager adjustmentManager)
        {
            if (adjustmentManager == null) return;

            this.ItemsControl.ItemsSource = null;
            this.ItemsControl.ItemsSource = adjustmentManager.Adjustments;

            int count = adjustmentManager.Adjustments.Count();
            this.State = (count == 0) ? AdjustmentControlState.Null : AdjustmentControlState.Adjustments;
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