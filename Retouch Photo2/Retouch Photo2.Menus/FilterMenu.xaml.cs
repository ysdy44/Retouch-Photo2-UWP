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
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch Photo2.Filters"/>.
    /// </summary>
    public sealed partial class FilterMenu : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Construct
        /// <summary>
        /// Initializes a FilterMenu. 
        /// </summary>
        public FilterMenu()
        {
            this.InitializeComponent();
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
            };

            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Filter filter)
                {
                    this.SetFilter(filter);
                }
            };
        }

        private void SetFilter(Filter filter)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetFilter);

            //Selection
            ILayer outermostLayer = null;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;
                if (outermostLayer == null) outermostLayer = layer;

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


                layer.Filter.Adjustments.Clear();
                foreach (IAdjustment adjustment in filter.Adjustments)
                {
                    layer.Filter.Adjustments.Add(adjustment.Clone());
                }
            });

            this.SelectionViewModel.Adjustments.Clear();
            foreach (IAdjustment adjustment in outermostLayer.Filter.Adjustments)
            {
                this.SelectionViewModel.Adjustments.Add(adjustment);
            }

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate     
        }

    }
}