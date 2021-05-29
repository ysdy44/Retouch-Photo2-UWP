// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    internal class ExtendListViewItem : ListViewItem
    {
        //public string Name { get; set; }
        public CanvasEdgeBehavior Type { get; set; }
        public int Index { get; set; }
        public VirtualKey Key { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// ComboBox of <see cref="CanvasEdgeBehavior"/>.
    /// </summary>
    public sealed partial class ExtendComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when extend change. </summary>
        public event EventHandler<CanvasEdgeBehavior> ExtendChanged;
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.Flyout.Closed += value; remove => this.Flyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.Flyout.Opened += value; remove => this.Flyout.Opened -= value; }


        //@Group
        private readonly IDictionary<CanvasEdgeBehavior, ExtendListViewItem> ItemDictionary = new Dictionary<CanvasEdgeBehavior, ExtendListViewItem>();
        private readonly IDictionary<VirtualKey, ExtendListViewItem> KeyDictionary = new Dictionary<VirtualKey, ExtendListViewItem>();


        #region DependencyProperty


        /// <summary> Gets or sets the edge behavior. </summary>
        public CanvasEdgeBehavior Extend
        {
            get => this.extend;
            set
            {
                ExtendListViewItem item = this.ItemDictionary[value];
                this.Control.Content = item.Title;
                this.extend = value;
            }
        }
        private CanvasEdgeBehavior extend;

        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get => this.fillOrStroke;
            set
            {
                switch (value)
                {
                    case FillOrStroke.Fill: this.Extend = this.Fill?.Extend ?? CanvasEdgeBehavior.Clamp; break;
                    case FillOrStroke.Stroke: this.Extend = this.Stroke?.Extend ?? CanvasEdgeBehavior.Clamp; break;
                }

                this.fillOrStroke = value;
            }
        }
        private FillOrStroke fillOrStroke;

        /// <summary> Gets or sets the fill. </summary>
        public IBrush Fill
        {
            get => this.fill;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.Extend = value?.Extend ?? CanvasEdgeBehavior.Clamp;
                        break;
                }
                this.fill = value;
            }
        }
        private IBrush fill;

        /// <summary> Gets or sets the stroke. </summary>
        public IBrush Stroke
        {
            get => this.stroke;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        this.Extend = value?.Extend ?? CanvasEdgeBehavior.Clamp;
                        break;
                }
                this.stroke = value;
            }
        }
        private IBrush stroke;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ExtendComboBox. 
        /// </summary>
        public ExtendComboBox()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ConstructStrings();

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is ExtendListViewItem item)
                    {
                        CanvasEdgeBehavior type = item.Type;
                        this.ExtendChanged?.Invoke(this, type); // Delegate
                        this.Flyout.Hide();
                    }
                }
            };
            this.ListView.KeyDown += (s, e) =>
            {
                VirtualKey key = e.OriginalKey;
                if (this.KeyDictionary.ContainsKey(key) == false) return;

                ExtendListViewItem item = this.KeyDictionary[key];
                item.Focus(FocusState.Programmatic);
                this.ListView.SelectedIndex = item.Index;
            };
            this.Flyout.Opened += (s, e) =>
            {
                ExtendListViewItem item = this.ItemDictionary[this.Extend];
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
                CanvasEdgeBehavior type = kv.Key;
                ExtendListViewItem item = kv.Value;
                string title = resource.GetString($"Tools_Brush_Extend_{type}");

                item.Title = title;
            }
        }


        //@Group
        private void InitializeDictionary()
        {
            foreach (object child in this.ListView.Items)
            {
                if (child is ExtendListViewItem item)
                {
                    CanvasEdgeBehavior type = item.Type;
                    VirtualKey key = item.Key;

                    this.ItemDictionary.Add(type, item);
                    if (key != default) this.KeyDictionary.Add(key, item);
                }
            }
        }

    }
}