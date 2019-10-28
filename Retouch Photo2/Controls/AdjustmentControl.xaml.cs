using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Adjustments.Pages;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.IO;

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


        //@VisualState
        bool _vsIsEdit;
        bool _vsIsFilter;
        List<IAdjustment> _vsAdjustments; 
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsEdit) return this.Edit;
                if (this._vsIsFilter) return this.Filters;

                if (this._vsAdjustments == null) return this.Disable;

                if (this._vsAdjustments.Count == 0) return this.ZeroAdjustments;
                else return this.Adjustments;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
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

            con._vsIsEdit = false;
            con._vsIsFilter = false;

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


        //@Construct
        public AdjustmentControl()
        {
            this.InitializeComponent();
            this.VisualState = this.VisualState;//State

            this.Loaded += async (s, e) =>
            {
                if (this.AdjustmentPageListView.ItemsSource == null)
                {
                    this.AdjustmentPageListView.ItemsSource = this.PageList;
                }

                if (this.FilterListView.ItemsSource == null)
                {
                    StorageFile file = null;
                    bool isLocalFilterExists = await ApplicationLocalTextFileUtility.IsFileExistsInLocalFolder("Filter.xml");

                    if (isLocalFilterExists)
                    {
                        //Read the file from the local folder.
                        file = await ApplicationData.Current.LocalFolder.GetFileAsync("Filter.xml");
                    }
                    else
                    {
                        //Read the file from the package.
                        file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///XMLs/Filter.xml"));

                        //Copy to the local folder.
                        await file.CopyAsync(ApplicationData.Current.LocalFolder);
                    }

                    if (file != null)
                    {
                        Stream stream = await file.OpenStreamForReadAsync();
                        XDocument document = XDocument.Load(stream);

                        IEnumerable<Filter> source = Retouch_Photo2.Adjustments.XML.LoadFilters(document);
                        this.FilterListView.ItemsSource = source.ToList();
                    }
                }
            };


            //Adjustment
            AdjustmentManager.Invalidate = () => this.ViewModel.Invalidate(); 


            //Button
            this.AddButton.Tapped += (s, e) => this.AdjustmentPageFlyout.ShowAt(this.AddButton);
            this.FilterButton.Tapped += (s, e) =>
            {
                this._vsIsFilter = true;
                this.VisualState = this.VisualState;//State
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

                this._vsIsEdit = false;
                this._vsIsFilter = false;
                this.VisualState = this.VisualState;//State
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

                        this._vsAdjustments = layer.AdjustmentManager.Adjustments;
                        this.VisualState = this.VisualState;//State

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
                        
                        this._vsAdjustments = layer.AdjustmentManager.Adjustments;
                        this.VisualState = this.VisualState;//State

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

            this._vsIsEdit = true;
            this.VisualState = this.VisualState;//State

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
}