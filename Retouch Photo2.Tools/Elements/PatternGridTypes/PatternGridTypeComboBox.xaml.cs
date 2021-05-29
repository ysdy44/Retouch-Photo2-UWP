// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Retouch_Photo2.Layers.Models;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    internal class PatternGridTypeListViewItem : ListViewItem
    {
        //public string Name { get; set; }
        public PatternGridType Type { get; set; }
        public int Index { get; set; }
        public VirtualKey Key { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// Represents the contorl that is used to select grid, horizontal or vertical.
    /// </summary>
    public sealed partial class PatternGridTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when type change. </summary>
        public event EventHandler<PatternGridType> TypeChanged;
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.Flyout.Closed += value; remove => this.Flyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.Flyout.Opened += value; remove => this.Flyout.Opened -= value; }


        //@Group
        private readonly IDictionary<PatternGridType, PatternGridTypeListViewItem> ItemDictionary = new Dictionary<PatternGridType, PatternGridTypeListViewItem>();
        private readonly IDictionary<VirtualKey, PatternGridTypeListViewItem> KeyDictionary = new Dictionary<VirtualKey, PatternGridTypeListViewItem>();


        #region DependencyProperty


        /// <summary> Gets or sets the grid, horizontal or vertical. </summary>
        public PatternGridType Type
        {
            get => this.type;
            set
            {
                PatternGridTypeListViewItem item = this.ItemDictionary[value];
                this.Control.Content = item.Title;
                this.type = value;
            }
        }
        private PatternGridType type;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a PatternGridTypeComboBox. 
        /// </summary>
        public PatternGridTypeComboBox()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ConstructStrings();

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is PatternGridTypeListViewItem item)
                    {
                        PatternGridType type = item.Type;
                        this.TypeChanged?.Invoke(this, type); // Delegate
                        this.Flyout.Hide();
                    }
                }
            };
            this.ListView.KeyDown += (s, e) =>
            {
                VirtualKey key = e.OriginalKey;
                if (this.KeyDictionary.ContainsKey(key) == false) return;

                PatternGridTypeListViewItem item = this.KeyDictionary[key];
                item.Focus(FocusState.Programmatic);
                this.ListView.SelectedIndex = item.Index;
            };
            this.Flyout.Opened += (s, e) =>
            {
                PatternGridTypeListViewItem item = this.ItemDictionary[this.Type];
                item.Focus(FocusState.Programmatic);
                this.ListView.SelectedIndex = item.Index;
            };
        }


        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            foreach (var kv in this.ItemDictionary)
            {
                PatternGridType type = kv.Key;
                PatternGridTypeListViewItem item = kv.Value;
                string title = resource.GetString($"Tools_PatternGrid_{type}");

                item.Title = title;
            }
        }


        //@Group
        private void InitializeDictionary()
        {
            foreach (object child in this.ListView.Items)
            {
                if (child is PatternGridTypeListViewItem item)
                {
                    PatternGridType type = item.Type;
                    VirtualKey key = item.Key;

                    this.ItemDictionary.Add(type, item);
                    if (key != default) this.KeyDictionary.Add(key, item);
                }
            }
        }

    }
}