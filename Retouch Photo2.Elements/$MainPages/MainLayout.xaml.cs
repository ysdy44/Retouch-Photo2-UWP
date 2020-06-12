using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary> 
    /// <see cref = "MainPage" />'s layout. 
    /// </summary>
    public sealed partial class MainLayout : UserControl
    {

        //@Content     
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


        //@VisualState
        MainPageState _vsState = MainPageState.Main;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsState)
                {
                    case MainPageState.None: return this.Normal;
                    case MainPageState.Initial: return this.Initial;
                    case MainPageState.Main: return this.Main;
                    case MainPageState.Dialog: return this.Dialog;
                    case MainPageState.Pictures: return this.Pictures;
                    case MainPageState.Rename: return this.Rename;
                    case MainPageState.Delete: return this.Delete;
                    case MainPageState.Duplicate: return this.Duplicate;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        /// <summary>
        /// Gets or sets the mainpage-state.
        /// </summary>
        public MainPageState State
        {
            get => this._vsState;
            set
            {
                this._vsState = value;
                this.VisualState = this.VisualState;//State
            }
        }


        /// <summary>
        /// All ProjectViewItems.
        /// </summary>
        public ObservableCollection<IProjectViewItem> Items { get; private set; } = new ObservableCollection<IProjectViewItem>();

        public IEnumerable<IProjectViewItem> SelectedItems => from i in this.Items where i.SelectMode == SelectMode.Selected select i;

        public int Count => this.Items.Count;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "MainLayout" />'s selected count. </summary>
        public int SelectedCount
        {
            get { return (int)GetValue(SelectedCountProperty); }
            set { SetValue(SelectedCountProperty, value); }
        }
        /// <summary> Identifies the <see cref = "MainLayout.SelectedCount" /> dependency property. </summary>
        public static readonly DependencyProperty SelectedCountProperty = DependencyProperty.Register(nameof(SelectedCount), typeof(int), typeof(MainLayout), new PropertyMetadata(0));


        /// <summary> Gets or sets <see cref = "MainLayout" />'s selected is enable. </summary>
        public bool SelectedIsEnabled
        {
            get { return (bool)GetValue(SelectedIsEnabledProperty); }
            set { SetValue(SelectedIsEnabledProperty, value); }
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
        public MainLayout()
        {
            this.InitializeComponent();
            this.GridView.ItemsSource = this.Items;

            this.Loaded += (s, e) =>
            {
                 this.VisualState = this.VisualState;//State
            };

            this.SelectCheckBox.Unchecked += (s, e) => this.SelectAll(SelectMode.None);
            this.SelectCheckBox.Checked += (s, e) =>
            {
                this.SelectAll(SelectMode.UnSelected);

                this.RefreshSelectCount();
            };
        }

        /// <summary>
        /// Refresh all items select-mode.
        /// </summary>
        public void SelectAllAndDeselectIcon()
        {
            bool isAnyUnSelected = this.Items.Any(p => p.SelectMode == SelectMode.UnSelected);
            SelectMode mode = isAnyUnSelected ? SelectMode.Selected : SelectMode.UnSelected;
            this.SelectAll(mode);

            this.RefreshSelectCount();
        }

        /// <summary>
        /// Refresh all items select-mode.
        /// </summary>
        public void SelectAll(SelectMode selectMode)
        {
            foreach (IProjectViewItem item in this.Items)
            {
                item.SelectMode = selectMode;
            }
        }
               
        /// <summary>
        /// Get a name that doesn't have a rename.
        /// If there are, add the number.
        /// [Untitled] --> [Untitled1]   
        /// </summary>
        /// <param name="name"> The previous name. </param>
        /// <returns> The new name. </returns>
        public string UntitledRenameByRecursive(string name)
        {
            // Is there a re-named item?
            if (this.Items.All(i => i.Name != name))
                return name;

            int num = 0;
            string newName;

            do
            {
                num++;
                newName = $"{name}{num}";
            }
            // Is there a re-named item?
            while (this.Items.Any(i => i.Name == newName));

            return newName;
        }

    }
}