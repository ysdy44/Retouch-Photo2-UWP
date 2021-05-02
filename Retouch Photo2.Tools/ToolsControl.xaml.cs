// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools.Models;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    internal class ToolTypeListViewItem : ListViewItem
    {
        //public string Name { get; set; }
        public ToolType Type { get; set; }
        public int Index { get; set; } = -1;
        public int MoreIndex { get; set; } = -1;
        public VirtualKey Key { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// Represents a tools control, that containing some buttons.
    /// </summary>
    public sealed partial class ToolsControl : UserControl
    {

        //@Delegate
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.MoreFlyout.Closed += value; remove => this.MoreFlyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.MoreFlyout.Opened += value; remove => this.MoreFlyout.Opened -= value; }


        //@Group
        private readonly IDictionary<ToolType, ToolTypeListViewItem> ItemDictionary = new Dictionary<ToolType, ToolTypeListViewItem>();
        private readonly IDictionary<VirtualKey, ToolTypeListViewItem> KeyDictionary = new Dictionary<VirtualKey, ToolTypeListViewItem>();


        #region DependencyProperty


        /// <summary> Gets or sets the assembly type. </summary>
        public Type AssemblyType { get; set; }


        /// <summary> Gets or sets the tool-type. </summary>
        public ToolType Type
        {
            get => (ToolType)base.GetValue(TypeProperty);
            set => base.SetValue(TypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolsControl.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(ToolType), typeof(ToolsControl), new PropertyMetadata(ToolType.Node, (sender, e) =>
        {
            ToolsControl control = (ToolsControl)sender;

            if (e.NewValue is ToolType value)
            {
                ToolTypeListViewItem item = control.ItemDictionary[value];
                item.Focus(FocusState.Programmatic);
                control.ListView.SelectedIndex = item.Index;

                control.Tool = Retouch_Photo2.Tools.XML.CreateTool(control.AssemblyType, value);
            }
        }));


        /// <summary> Gets or sets the tool. </summary>
        public ITool Tool
        {
            get => (ITool)base.GetValue(ToolProperty);
            set => base.SetValue(ToolProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolsControl.Tool" /> dependency property. </summary>
        public static readonly DependencyProperty ToolProperty = DependencyProperty.Register(nameof(Tool), typeof(ITool), typeof(ToolsControl), new PropertyMetadata(new NoneTool(), (sender, e) =>
        {
            ToolsControl control = (ToolsControl)sender;

            //The current tool becomes the active tool.
            if (e.OldValue is ITool oldTool)
            {
                oldTool.OnNavigatedFrom();
            }

            //The current tool does not become an active tool.
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
        /// <summary> Identifies the <see cref = "ToolsControl.IsOpenCore" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenCoreProperty = DependencyProperty.Register(nameof(IsOpenCore), typeof(bool), typeof(ToolsControl), new PropertyMetadata(false));


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
        /// Initializes a ToolsControl. 
        /// </summary>
        public ToolsControl()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ConstructStrings();

            this.More.Click += (s, e) => this.MoreFlyout.ShowAt(this);
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is ToolTypeListViewItem item)
                    {
                        ToolType type = item.Type;
                        this.Type = type;
                    }
                }
            };
            this.MoreListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is ToolTypeListViewItem item)
                    {
                        ToolType type = item.Type;
                        this.Type = type;
                    }
                }
            };
            this.MoreListView.KeyDown += (s, e) =>
            {
                VirtualKey key = e.OriginalKey;
                if (this.KeyDictionary.ContainsKey(key) == false) return;

                ToolTypeListViewItem item = this.KeyDictionary[key];
                item.Focus(FocusState.Programmatic);
                this.MoreListView.SelectedIndex = item.MoreIndex;
            };
            this.MoreFlyout.Opened += (s, e) =>
            {
                ToolTypeListViewItem item = this.ItemDictionary[this.Type];
                item.Focus(FocusState.Programmatic);
                this.MoreListView.SelectedIndex = item.MoreIndex;
            };
        }


        //Strings
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

                    this.ItemDictionary.Add(type, item);
                    if (key != default) this.KeyDictionary.Add(key, item);
                }
            }
        }

    }
}