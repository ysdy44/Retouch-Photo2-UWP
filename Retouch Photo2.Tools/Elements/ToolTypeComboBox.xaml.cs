// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Models;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    internal class ToolTypeListViewItem : ListViewItem
    {
        //public string Name { get; set; }
        public ToolType Type { get; set; }
        public int Index { get; set; } = -1;
        public VirtualKey Key { get; set; }

        #region DependencyProperty

        /// <summary> Gets or sets the title. </summary>
        public string Title
        {
            get => (string)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolTypeListViewItem.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(ToolTypeListViewItem), new PropertyMetadata(string.Empty));

        #endregion
    }

    /// <summary>
    /// Represents a tools combo box, that containing some buttons.
    /// </summary>
    public sealed partial class ToolTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.MoreFlyout.Closed += value; remove => this.MoreFlyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.MoreFlyout.Opened += value; remove => this.MoreFlyout.Opened -= value; }


        //@Group
        private readonly IDictionary<ToolType, ToolTypeListViewItem> ItemDictionary = new Dictionary<ToolType, ToolTypeListViewItem>();
        private readonly IDictionary<ToolType, ToolTypeListViewItem> MoreItemDictionary = new Dictionary<ToolType, ToolTypeListViewItem>();
        private readonly IDictionary<VirtualKey, ToolTypeListViewItem> KeyDictionary = new Dictionary<VirtualKey, ToolTypeListViewItem>();
        private readonly IDictionary<VirtualKey, ToolTypeListViewItem> MoreKeyDictionary = new Dictionary<VirtualKey, ToolTypeListViewItem>();


        #region DependencyProperty


        /// <summary> Gets or sets the assembly type. </summary>
        public Type AssemblyType { get; set; }


        /// <summary> Gets or sets the layer-type. </summary>
        public LayerType LayerType
        {
            get => (LayerType)base.GetValue(LayerTypeProperty);
            set => base.SetValue(LayerTypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolTypeComboBox.LayerType" /> dependency property. </summary>
        public static readonly DependencyProperty LayerTypeProperty = DependencyProperty.Register(nameof(LayerType), typeof(LayerType), typeof(ToolTypeComboBox), new PropertyMetadata(LayerType.None, (sender, e) =>
        {
            ToolTypeComboBox control = (ToolTypeComboBox)sender;

            if (e.NewValue is LayerType value)
            {
                control.Tool = Retouch_Photo2.Tools.XML.CreateTool(control.AssemblyType, control.GetToolType(control.ToolType, value));
            }
        }));


        /// <summary> Gets or sets the tool-type. </summary>
        public ToolType ToolType
        {
            get => (ToolType)base.GetValue(ToolTypeProperty);
            set => base.SetValue(ToolTypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolTypeComboBox.ToolType" /> dependency property. </summary>
        public static readonly DependencyProperty ToolTypeProperty = DependencyProperty.Register(nameof(ToolType), typeof(ToolType), typeof(ToolTypeComboBox), new PropertyMetadata(ToolType.Node, (sender, e) =>
        {
            ToolTypeComboBox control = (ToolTypeComboBox)sender;

            if (e.NewValue is ToolType value)
            {
                if (control.ItemDictionary.ContainsKey(value))
                {
                    ToolTypeListViewItem item = control.ItemDictionary[value];
                    item.Focus(FocusState.Programmatic);
                    control.ListView.SelectedIndex = item.Index;
                }
                else
                {
                    control.ListView.SelectedIndex = -1;
                }

                control.Tool = Retouch_Photo2.Tools.XML.CreateTool(control.AssemblyType, control.GetToolType(value, control.LayerType));
            }
        }));


        private ToolType GetToolType(ToolType toolType, LayerType layerType)
        {
            if (toolType != ToolType.Cursor) return toolType;

            switch (layerType)
            {
                case LayerType.None: return toolType;
                case LayerType.GeometryRectangle: return ToolType.GeometryRectangle;
                case LayerType.GeometryEllipse: return ToolType.GeometryEllipse;
                case LayerType.Curve: return ToolType.Pen;
                case LayerType.TextFrame: return ToolType.TextFrame;
                case LayerType.TextArtistic: return ToolType.TextArtistic;
                case LayerType.Image: return ToolType.Image;
                //case LayerType.Group: return ToolType.Group;
                case LayerType.PatternGrid: return ToolType.PatternGrid;
                case LayerType.PatternDiagonal: return ToolType.PatternDiagonal;
                case LayerType.PatternSpotted: return ToolType.PatternSpotted;
                case LayerType.GeometryRoundRect: return ToolType.GeometryRoundRect;
                case LayerType.GeometryTriangle: return ToolType.GeometryTriangle;
                case LayerType.GeometryDiamond: return ToolType.GeometryDiamond;
                case LayerType.GeometryPentagon: return ToolType.GeometryPentagon;
                case LayerType.GeometryStar: return ToolType.GeometryStar;
                case LayerType.GeometryCog: return ToolType.GeometryCog;
                case LayerType.GeometryDount: return ToolType.GeometryDount;
                case LayerType.GeometryPie: return ToolType.GeometryPie;
                case LayerType.GeometryCookie: return ToolType.GeometryCookie;
                case LayerType.GeometryArrow: return ToolType.GeometryArrow;
                case LayerType.GeometryCapsule: return ToolType.GeometryCapsule;
                case LayerType.GeometryHeart: return ToolType.GeometryHeart;
                default: return toolType;
            }
        }


        /// <summary> Gets or sets the tool. </summary>
        public ITool Tool
        {
            get => (ITool)base.GetValue(ToolProperty);
            set => base.SetValue(ToolProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolTypeComboBox.Tool" /> dependency property. </summary>
        public static readonly DependencyProperty ToolProperty = DependencyProperty.Register(nameof(Tool), typeof(ITool), typeof(ToolTypeComboBox), new PropertyMetadata(new NoneTool(), (sender, e) =>
        {
            ToolTypeComboBox control = (ToolTypeComboBox)sender;

            // The current tool becomes the active tool.
            if (e.OldValue is ITool oldTool)
            {
                oldTool.OnNavigatedFrom();
            }

            // The current tool does not become an active tool.
            if (e.NewValue is ITool newTool)
            {
                newTool.OnNavigatedTo();
            }
        }));


        /// <summary> Gets or sets the tool-type. </summary>
        public bool IsOpenCore
        {
            get => (bool)base.GetValue(IsOpenCoreProperty);
            set => base.SetValue(IsOpenCoreProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolTypeComboBox.IsOpenCore" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenCoreProperty = DependencyProperty.Register(nameof(IsOpenCore), typeof(bool), typeof(ToolTypeComboBox), new PropertyMetadata(false));


        /// <summary>
        /// Gets or sets the IsOpen.
        /// </summary>
        public bool IsOpen
        {
            set
            {
                switch (this.DeviceLayoutType)
                {
                    case DeviceLayoutType.PC:
                    case DeviceLayoutType.Pad:
                        {
                            this.IsOpenCore = value;
                        }
                        break;
                }
                this.Tool.IsOpen = value;
            }
        }


        /// <summary> Gets or sets the device layout type. </summary>
        public DeviceLayoutType DeviceLayoutType { get; set; }


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ToolTypeComboBox. 
        /// </summary>
        public ToolTypeComboBox()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ConstructStrings();
            base.Loaded += (s, e) => this.ConstructLanguages();

            this.More.Click += (s, e) => this.MoreFlyout.ShowAt(this);
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is ToolTypeListViewItem item)
                    {
                        ToolType type = item.Type;
                        this.ToolType = type;
                    }
                }
            };
            this.ListView.KeyDown += (s, e) =>
           {
               VirtualKey key = e.OriginalKey;
               if (key == VirtualKey.Left || key == VirtualKey.Right)
               {
                   this.ListView.SelectedIndex = -1;
                   this.MoreListView.SelectedIndex = 1;
                   this.MoreListView.Focus(FocusState.Programmatic);
               }
               else if (this.KeyDictionary.ContainsKey(key))
               {
                   ToolTypeListViewItem item = this.KeyDictionary[key];
                   item.Focus(FocusState.Programmatic);
                   this.ListView.SelectedIndex = item.Index;
                   this.MoreListView.SelectedIndex = -1;
               }
           };
            this.MoreListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is ToolTypeListViewItem item)
                    {
                        ToolType type = item.Type;
                        this.ToolType = type;
                    }
                }
            };
            this.MoreListView.KeyDown += (s, e) =>
            {
                VirtualKey key = e.OriginalKey;
                if (key == VirtualKey.Left || key == VirtualKey.Right)
                {
                    this.ListView.SelectedIndex = 0;
                    this.MoreListView.SelectedIndex = -1;
                    this.ListView.Focus(FocusState.Programmatic);
                }
                else if (this.MoreKeyDictionary.ContainsKey(key))
                {
                    ToolTypeListViewItem item = this.MoreKeyDictionary[key];
                    item.Focus(FocusState.Programmatic);
                    this.ListView.SelectedIndex = -1;
                    this.MoreListView.SelectedIndex = item.Index;
                }
            };
            this.MoreFlyout.Opened += (s, e) =>
            {
                if (this.MoreItemDictionary.ContainsKey(this.ToolType))
                {
                    ToolTypeListViewItem item = this.MoreItemDictionary[this.ToolType];
                    item.Focus(FocusState.Programmatic);
                    this.MoreListView.SelectedIndex = item.Index;
                }
                else
                {
                    this.MoreListView.SelectedIndex = -1;
                }
            };
        }
    }

    public sealed partial class ToolTypeComboBox : UserControl
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

            this.MoreToolTip.Content = resource.GetString($"Tools_{this.More.Name}");
            this.Pattern.Content = resource.GetString($"Tools_{this.Pattern.Name}");
            this.Geometry.Content = resource.GetString($"Tools_{this.Geometry.Name}");

            foreach (var kv in this.ItemDictionary)
            {
                ToolType type = kv.Key;
                ToolTypeListViewItem item = kv.Value;
                string title = resource.GetString($"Tools_{type}");

                item.Title = title;
            }
            foreach (var kv in this.MoreItemDictionary)
            {
                ToolType type = kv.Key;
                ToolTypeListViewItem item = kv.Value;
                string title = resource.GetString($"Tools_{type}");

                item.Title = title;
            }
        }


        //@Group
        private void InitializeDictionary()
        {
            foreach (object child in this.ListView.Items)
            {
                if (child is ToolTypeListViewItem item)
                {
                    ToolType type = item.Type;
                    VirtualKey key = item.Key;

                    this.ItemDictionary.Add(type, item);
                    if (key != default) this.KeyDictionary.Add(key, item);
                }
            }

            foreach (object child in this.MoreListView.Items)
            {
                if (child is ToolTypeListViewItem item)
                {
                    ToolType type = item.Type;
                    VirtualKey key = item.Key;

                    this.MoreItemDictionary.Add(type, item);
                    if (key != default) this.MoreKeyDictionary.Add(key, item);
                }
            }
        }

    }
}
