﻿// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using FanKit.Transformers;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    internal class ArrowTailTypeListViewItem : ListViewItem
    {
        //public string Name { get; set; }
        public GeometryArrowTailType Type { get; set; }
        public int Index { get; set; }
        public VirtualKey Key { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// Represents the contorl that is used to select none or arrow.
    /// </summary>
    public sealed partial class ArrowTailTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when type changed. </summary>
        public event EventHandler<GeometryArrowTailType> TypeChanged;
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.Flyout.Closed += value; remove => this.Flyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.Flyout.Opened += value; remove => this.Flyout.Opened -= value; }


        //@Group
        private readonly IDictionary<GeometryArrowTailType, ArrowTailTypeListViewItem> ItemDictionary = new Dictionary<GeometryArrowTailType, ArrowTailTypeListViewItem>();
        private readonly IDictionary<VirtualKey, ArrowTailTypeListViewItem> KeyDictionary = new Dictionary<VirtualKey, ArrowTailTypeListViewItem>();


        #region DependencyProperty


        /// <summary> Gets or sets the none or arrow. </summary>
        public GeometryArrowTailType Type
        {
            get => this.type;
            set
            {
                ArrowTailTypeListViewItem item = this.ItemDictionary[value];
                this.Control.Content = item.Title;
                this.type = value;
            }
        }
        private GeometryArrowTailType type;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ArrowTailTypeComboBox. 
        /// </summary>
        public ArrowTailTypeComboBox()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ConstructStrings();

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is ArrowTailTypeListViewItem item)
                    {
                        GeometryArrowTailType type = item.Type;
                        this.TypeChanged?.Invoke(this, type); // Delegate
                        this.Flyout.Hide();
                    }
                }
            };
            this.ListView.KeyDown += (s, e) =>
            {
                VirtualKey key = e.OriginalKey;
                if (this.KeyDictionary.ContainsKey(key) == false) return;

                ArrowTailTypeListViewItem item = this.KeyDictionary[key];
                item.Focus(FocusState.Programmatic);
                this.ListView.SelectedIndex = item.Index;
            };
            this.Flyout.Opened += (s, e) =>
            {
                ArrowTailTypeListViewItem item = this.ItemDictionary[this.Type];
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
                GeometryArrowTailType type = kv.Key;
                ArrowTailTypeListViewItem item = kv.Value;
                string title = resource.GetString($"Tools_GeometryArrow_ArrowTail_{type}");

                item.Title = title;
            }
        }


        //@Group
        private void InitializeDictionary()
        {
            foreach (object child in this.ListView.Items)
            {
                if (child is ArrowTailTypeListViewItem item)
                {
                    GeometryArrowTailType type = item.Type;
                    VirtualKey key = item.Key;

                    this.ItemDictionary.Add(type, item);
                    if (key != default) this.KeyDictionary.Add(key, item);
                }
            }
        }

    }
}