// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Adjustments.Pages;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Retouch_Photo2.Menus
{
    internal class AdjustmentTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value is AdjustmentType type)
            {
                IAdjustmentPage adjustmentPage = Retouch_Photo2.Adjustments.XML.CreateAdjustmentPage(typeof(BrightnessPage), type);
                return adjustmentPage.Title;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class AdjustmentIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value is AdjustmentType type)
            {
                IAdjustmentPage adjustmentPage = Retouch_Photo2.Adjustments.XML.CreateAdjustmentPage(typeof(BrightnessPage), type);
                return adjustmentPage.Icon;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Adjustments"/>.
    /// </summary>
    public sealed partial class AdjustmentMenu : MenuExpander
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        public bool MenuIsEnabled { get => this.ListView.IsEnabled; set => this.ListView.IsEnabled = value; }


        //@Construct
        /// <summary>
        /// Initializes a AdjustmentMenu. 
        /// </summary>
        public AdjustmentMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            base.Loaded += (s, e) => this.ConstructLanguages();

            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is ListViewItem item)
                    {
                        string key = item.Name;
                        IAdjustment adjustment = Retouch_Photo2.Adjustments.XML.CreateAdjustment(key);
                        this.Add(adjustment);

                        AdjustmentCommand.Edit(adjustment); // Delegate
                    }
                }
            };
        }
    }

    public sealed partial class AdjustmentMenu : MenuExpander
    {
        
        // Languages
        private void ConstructLanguages()
        {
            if (string.IsNullOrEmpty(ApplicationLanguages.PrimaryLanguageOverride) == false)
            {
                if (ApplicationLanguages.PrimaryLanguageOverride != base.Language)
                {
                    this.ConstructStrings();
                }
            }
        }

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            foreach (object child in this.ListView.Items)
            {
                if (child is ListViewItem item)
                {
                    if (item.Content is ContentControl control)
                    {
                        string key = item.Name;
                        string title = resource.GetString($"Adjustments_{key}");

                        control.Content = title;
                    }
                }
            }
        }


        private void Add(IAdjustment adjustment)
        {
            // History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetFilter);

            // Selection
            ILayer outermostLayer = null;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;
                if (outermostLayer is null) outermostLayer = layer;

                var previous = layer.Filter.Clone();
                history.UndoAction += () =>
                {
                    // Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Filter = previous.Clone();
                };


                // Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Filter.Adjustments.Add(adjustment);
            });
            this.SelectionViewModel.SetFilter(outermostLayer?.Filter);

            // History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate(); // Invalidate     
        }


        /// <summary>
        /// Remove the adjustment.
        /// </summary>
        /// <param name="adjustment"> The removed adjustment. </param>
        public void Remove(IAdjustment removeAdjustment)
        {
            // History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetFilter);

            // Selection
            ILayer outermostLayer = null;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;
                if (outermostLayer is null) outermostLayer = layer;

                var previous = layer.Filter.Clone();
                history.UndoAction += () =>
                {
                    // Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Filter = previous.Clone();
                };

                // Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Filter.Adjustments.Remove(removeAdjustment);
            });
            this.SelectionViewModel.SetFilter(outermostLayer?.Filter);

            // History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate(); // Invalidate     

            AdjustmentCommand.Edit(null); // Delegate
        }

    }
}