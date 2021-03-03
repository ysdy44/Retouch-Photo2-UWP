using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class MainLayout : UserControl
    {

        #region DependencyProperty

        /// <summary> Gets or sets whether the <see cref = "MainLayout" /> orientation for DeviceLayoutType. </summary>
        public Orientation DeviceLayoutTypeOrientation
        {
            get => (Orientation)base.GetValue(DeviceLayoutTypeOrientationProperty);
            set => base.SetValue(DeviceLayoutTypeOrientationProperty, value);
        }
        /// <summary> Identifies the <see cref = "MainLayout.DeviceLayoutTypeOrientation" /> dependency property. </summary>
        public static readonly DependencyProperty DeviceLayoutTypeOrientationProperty = DependencyProperty.Register(nameof(DeviceLayoutTypeOrientation), typeof(Orientation), typeof(MainLayout), new PropertyMetadata(Orientation.Horizontal));

        #endregion


        /// <summary> 
        /// Gets or sets the device layout type. 
        /// </summary>
        public DeviceLayoutType DeviceLayoutType
        {
            set
            {
                if (this._GridView.ItemsPanelRoot is ItemsWrapGrid itemsWrapGrid)
                {
                    switch (value)
                    {
                        case DeviceLayoutType.Phone:
                            itemsWrapGrid.ItemWidth = 140;
                            itemsWrapGrid.ItemHeight = 140;
                            this.DeviceLayoutTypeOrientation = Orientation.Vertical;
                            break;
                        case DeviceLayoutType.Pad:
                            itemsWrapGrid.ItemWidth = 166;
                            itemsWrapGrid.ItemHeight = 156;
                            this.DeviceLayoutTypeOrientation = Orientation.Horizontal;
                            break;
                        case DeviceLayoutType.PC:
                            itemsWrapGrid.ItemWidth = 200;
                            itemsWrapGrid.ItemHeight = 186;

                            this.DeviceLayoutTypeOrientation = Orientation.Horizontal;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //@VisualState
        MainPageState _vsState = MainPageState.None;
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
                    case MainPageState.Pictures: return this.Pictures;
                    case MainPageState.Rename: return this.Rename;
                    case MainPageState.Delete: return this.Delete;
                    case MainPageState.Duplicate: return this.Duplicate;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, true);
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