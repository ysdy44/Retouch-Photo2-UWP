// Core:              ★★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★★★
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// <see cref = "MainPage" />'s layout. 
    /// </summary>
    public sealed partial class MainLayout : UserControl
    {

        //@Content     
        /// <summary> GridView. </summary>
        public GridView GridView => this._GridView;

        /// <summary> InitialBorder's Child. </summary>
        public UIElement InitialChild { get => this.InitialBorder.Child; set => this.InitialBorder.Child = value; }

        /// <summary> HeadBorder's Child. </summary>
        public UIElement HeadChild { get => this.HeadBorder.Child; set => this.HeadBorder.Child = value; }
        /// <summary> SelectBorder's Child. </summary>
        public UIElement SelectChild { get => this.SelectBorder.Child; set => this.SelectBorder.Child = value; }

        /// <summary> MainBorder's Child. </summary>
        public UIElement MainChild { get => this.MainBorder.Child; set => this.MainBorder.Child = value; }
        /// <summary> PicturesBorder's Child. </summary>
        public UIElement PicturesChild { get => this.PicturesBorder.Child; set => this.PicturesBorder.Child = value; }
        /// <summary> RenameBorder's Child. </summary>
        public UIElement RenameChild { get => this.RenameBorder.Child; set => this.RenameBorder.Child = value; }
        /// <summary> SaveBorder's Child. </summary>
        public UIElement SaveChild { get => this.SaveBorder.Child; set => this.SaveBorder.Child = value; }
        /// <summary> ShareBorder's Child. </summary>
        public UIElement ShareChild { get => this.ShareBorder.Child; set => this.ShareBorder.Child = value; }
        /// <summary> DeleteBorder's Child. </summary>
        public UIElement DeleteChild { get => this.DeleteBorder.Child; set => this.DeleteBorder.Child = value; }
        /// <summary> DuplicateBorder's Child. </summary>
        public UIElement DuplicateChild { get => this.DuplicateBorder.Child; set => this.DuplicateBorder.Child = value; }


        /// <summary> Gets all items. </summary>
        public ObservableCollection<IProjectViewItem> Items { get; private set; } = new ObservableCollection<IProjectViewItem>();
        /// <summary> Gets all selected items. </summary>
        public IEnumerable<IProjectViewItem> SelectedItems => from i in this.Items where i.SelectMode == SelectMode.Selected select i;
        /// <summary> Gets the count of items. </summary>
        public int Count => this.Items.Count;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "MainLayout" />'s selected count. </summary>
        public int SelectedCount
        {
            get => (int)base.GetValue(SelectedCountProperty);
            set => base.SetValue(SelectedCountProperty, value);
        }
        /// <summary> Identifies the <see cref = "MainLayout.SelectedCount" /> dependency property. </summary>
        public static readonly DependencyProperty SelectedCountProperty = DependencyProperty.Register(nameof(SelectedCount), typeof(int), typeof(MainLayout), new PropertyMetadata(0));


        /// <summary> Gets or sets <see cref = "MainLayout" />'s selected is enable. </summary>
        public bool SelectedIsEnabled
        {
            get => (bool)base.GetValue(SelectedIsEnabledProperty);
            set => base.SetValue(SelectedIsEnabledProperty, value);
        }
        /// <summary> Identifies the <see cref = "MainLayout.SelectedCount" /> dependency property. </summary>
        public static readonly DependencyProperty SelectedIsEnabledProperty = DependencyProperty.Register(nameof(SelectedIsEnabled), typeof(bool), typeof(MainLayout), new PropertyMetadata(false));


        /// <summary>
        /// Refresh the selected count.
        /// </summary>
        public void RefreshSelectCount()
        {
            int count = this.Items.Count(p => p.SelectMode == SelectMode.Selected);
            this.SelectedCount = count;

            bool isEnable = (count != 0);
            this.SelectedIsEnabled = isEnable;
        }


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a MainLayout.
        /// </summary>
        public MainLayout()
        {
            this.InitializeComponent();
            this._GridView.ItemsSource = this.Items;
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State

            this.SelectCheckBox.Unchecked += (s, e) => this.SelectAll(SelectMode.None);
            this.SelectCheckBox.Checked += (s, e) =>
            {
                this.SelectAll(SelectMode.UnSelected);

                this.RefreshSelectCount();
            };
        }

    }
}