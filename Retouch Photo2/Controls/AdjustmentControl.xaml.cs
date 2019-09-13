using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Adjustments.Pages;
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


        private AdjustmentControlState state = AdjustmentControlState.None;
        public AdjustmentControlState State
        {
            set
            {
                if (this.state == value) return;

                this.BackButton.Visibility = this.ResetButton.Visibility = this.Frame.Visibility = (value == AdjustmentControlState.Edit) ? Visibility.Visible : Visibility.Collapsed;
                this.AddButton.Visibility = this.FilterButton.Visibility = (value == AdjustmentControlState.Edit) ? Visibility.Collapsed : Visibility.Visible;
                this.ListView.IsEnabled = this.FilterGridView.IsEnabled = (value == AdjustmentControlState.Null || value == AdjustmentControlState.Adjustments);
                this.Border.Visibility = (value == AdjustmentControlState.Adjustments) ? Visibility.Visible : Visibility.Collapsed;
                this.TextBlock.Visibility = (value == AdjustmentControlState.Null || value == AdjustmentControlState.Disable) ? Visibility.Visible : Visibility.Collapsed;

                this.state = value;
            }
        }

        private IAdjustmentPage page;
        public IAdjustmentPage Page
        {
            get => this.page;
            set
            {
                this.Frame.Child = value.Page;

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
                con.Invalidate(value.Adjustments);
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
                if (this.ListView.ItemsSource == null)
                    this.ListView.ItemsSource = this.PageList;

                if (this.FilterGridView.ItemsSource == null)
                    this.FilterGridView.ItemsSource = (await Filter.GetFilterSource()).ToList();
            };


            //Adjustment
            AdjustmentManager.Invalidate = () => this.ViewModel.Invalidate();


            //Button
            this.FilterButton.Tapped += (s, e) => this.FilterFlyout.ShowAt(this.FilterButton);
            this.AddButton.Tapped += (s, e) => this.PageFlyout.ShowAt((Button)s);


            //Page
            this.BackButton.Tapped += (s, e) =>
            {
                if (this.Page == null) return;

                this.Page.Close();
                this.Page = null;

                this.State = AdjustmentControlState.Adjustments;
            };
            this.ResetButton.Tapped += (s, e) =>
            {
                if (this.Page == null) return;

                this.Page.Reset();
                this.ViewModel.Invalidate();
            };


            // Adjustment
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is IAdjustmentPage item)
                {
                    //Selection
                    this.SelectionViewModel.SetValue((layer) =>
                    {
                        IAdjustment adjustment = item.GetNewAdjustment();

                        layer.AdjustmentManager.Adjustments.Add(adjustment);

                        this.Invalidate(layer.AdjustmentManager.Adjustments);
                    });

                    this.ViewModel.Invalidate();//Invalidate

                    this.PageFlyout.Hide();
                }
            };


            //GridView
            this.FilterGridView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Filter filter)
                {
                    //Selection
                    this.SelectionViewModel.SetValue((layer) =>
                    {
                        layer.AdjustmentManager.Adjustments.Clear();
                        foreach (IAdjustment adjustment in filter.Adjustments)
                        {
                            IAdjustment cloneAdjustment = adjustment.Clone();
                            layer.AdjustmentManager.Adjustments.Add(cloneAdjustment);
                        }
                        this.Invalidate(layer.AdjustmentManager.Adjustments);
                    });

                    this.ViewModel.Invalidate();//Invalidate     
                }
            };
        }


        //@DataTemplate
        /// <summary> DataTemplate's EditButton Tapped. </summary>
        private void EditButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AdjustmentControl.GetGridDataContext(sender, out IAdjustment adjustment);

            if (adjustment == null) return;
            if (adjustment.Visibility == Visibility.Collapsed) return;

            IAdjustmentPage adjustmentPage = this.PageList.First(page => page.Type == adjustment.Type);

            adjustmentPage.SetAdjustment(adjustment);
            this.Page = adjustmentPage;

            this.State = AdjustmentControlState.Edit;
        }
        /// <summary> DataTemplate's RemoveButton Tapped. </summary>
        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AdjustmentControl.GetGridDataContext(sender, out IAdjustment adjustment);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.AdjustmentManager.Adjustments.Remove(adjustment);

                this.Invalidate(layer.AdjustmentManager.Adjustments);
            });

            this.ViewModel.Invalidate();//Invalidate   
        }


        /// <summary>
        ///  Invalidate Adjustment ItemsControl.
        /// </summary>
        /// <param name="adjustments"> The source adjustments. </param>
        public void Invalidate(IEnumerable<IAdjustment> adjustments)
        {
            if (adjustments == null) return;

            this.ItemsControl.ItemsSource = null;
            this.ItemsControl.ItemsSource = adjustments;

            this.State = (this.SelectionViewModel.Layer.AdjustmentManager.Adjustments.Count == 0) ? AdjustmentControlState.Null : AdjustmentControlState.Adjustments;
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