// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Filters;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Adjustments"/>.
    /// </summary>
    public sealed partial class AdjustmentMenu : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        public ItemCollection AdjustmentPages => this.AdjustmentPageListView.Items;


        //@VisualState
        bool _vsIsEnabled = false;
        bool _vsIsZeroAdjustments = true;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsEnabled == false) return this.Disable;
                if (this._vsIsZeroAdjustments) return this.ZeroAdjustments;
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        /// <summary>
        ///  Invalidate Adjustment ItemsControl.
        /// </summary>
        private void InvalidateItemsControl()
        {
            this.ListView.ItemsSource = null;
            if (this.Filter != null) this.ListView.ItemsSource = this.Filter.Adjustments;
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "AdjustmentMenu" />'s filter. </summary>
        public Filter Filter
        {
            get => (Filter)base.GetValue(FilterProperty);
            set => base.SetValue(FilterProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdjustmentMenu.Filter" /> dependency property. </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(Filter), typeof(AdjustmentMenu), new PropertyMetadata(null, (sender, e) =>
        {
            AdjustmentMenu control = (AdjustmentMenu)sender;

            if (e.NewValue is Filter value)
            {
                control._vsIsEnabled = true;
                control._vsIsZeroAdjustments = value.Adjustments.Count == 0;
                control.VisualState = control.VisualState;//State
            }
            else
            {
                control._vsIsEnabled = false;
                control.VisualState = control.VisualState;//State
            }

            control.InvalidateItemsControl();//Invalidate
            control.SplitView.IsPaneOpen = true;
        }));


        #endregion


        /// <summary> Gets or sets the current adjustment page. </summary>
        public IAdjustmentPage AdjustmentPage;


        //@Construct
        /// <summary>
        /// Initializes a AdjustmentMenu. 
        /// </summary>
        public AdjustmentMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            AdjustmentCommand.Edit = this.EditAction;
            AdjustmentCommand.Remove = this.RemoveAction;

            base.SizeChanged += (s, e) => this.SplitView.OpenPaneLength = e.NewSize.Width;
            base.Loaded += async (s, e) =>
            {
                if (this.CollectionViewSource.Source == null)
                {
                    IEnumerable<FilterCategory> categorys = await Retouch_Photo2.XML.ConstructFiltersFile();
                    if (categorys != null)
                    {
                        this.CollectionViewSource.Source = categorys;
                    }
                }
                this.VisualState = this.VisualState;//State
            };


            this.CloseButton.Click += (s, e) =>
            {
                this.ContentPresenter.Content = null;
                this.SplitView.IsPaneOpen = true;
            };


            this.FilterButton.Click += (s, e) => this.FilterFlyout.ShowAt(this.FilterButton);
            this.FilterListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Filter filter)
                {
                    this.FilterChanged(filter);
                }
            };

            this.AddButton.Click += (s, e) => this.AdjustmentPageFlyout.ShowAt(this.AddButton);
            this.AdjustmentPageListView.ItemClick += (s, e) =>
            {
                this.AdjustmentPageFlyout.Hide();

                if (e.ClickedItem is IAdjustmentPage item)
                {
                    this.FilterAdd(item);
                }
            };
        }


        private void EditAction(IAdjustment adjustment)
        {
            if (adjustment.PageVisibility == Visibility.Collapsed) return;

            IAdjustmentPage effectPage = adjustment.Page;
            if (effectPage == null) return;
            this.AdjustmentPage = adjustment.Page;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                int index = layer.Filter.Adjustments.IndexOf(adjustment);
                effectPage.Index = index;
                effectPage.Follow();
            }

            this.ContentPresenter.Content = effectPage.Self;
            this.SplitView.IsPaneOpen = false;
        }
        private void RemoveAction(IAdjustment adjustment)
        {
            this.FilterRemove(adjustment);
        }

    }

    public sealed partial class AdjustmentMenu : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ZeroTextBlock.Text = resource.GetString("Menus_Adjustment_ZeroTip");
            this.DisableTextBlock.Text = resource.GetString("Menus_Adjustment_DisableTip");

            this.AddButton.Content = resource.GetString("Menus_Adjustment_Add");
            this.FilterButton.Content = resource.GetString("Menus_Adjustment_Filters");

            this.CloseButton.Content = resource.GetString("Menus_Close");
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

                this._vsIsZeroAdjustments = layer.Filter.Adjustments.Count == 0;
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

                this._vsIsZeroAdjustments = layer.Filter.Adjustments.Count == 0;
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

                this._vsIsZeroAdjustments = layer.Filter.Adjustments.Count == 0;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.VisualState = this.VisualState;//State

            this.InvalidateItemsControl();//Invalidate
            this.ViewModel.Invalidate();//Invalidate     
        }

    }
}