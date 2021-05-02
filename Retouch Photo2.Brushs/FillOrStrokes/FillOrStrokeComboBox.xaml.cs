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
    internal class FillOrStrokeListViewItem : ListViewItem
    {
        //public string Name { get; set; }
        public FillOrStroke FillOrStroke { get; set; }
        public int Index { get; set; }
        public VirtualKey Key { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// ComboBox of <see cref="FillOrStroke"/>.
    /// </summary>
    public sealed partial class FillOrStrokeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when fill or stroke change. </summary>
        public event EventHandler<FillOrStroke> FillOrStrokeChanged;
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.Flyout.Closed += value; remove => this.Flyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.Flyout.Opened += value; remove => this.Flyout.Opened -= value; }


        //@Group
        private readonly IDictionary<FillOrStroke, FillOrStrokeListViewItem> ItemDictionary = new Dictionary<FillOrStroke, FillOrStrokeListViewItem>();
        private readonly IDictionary<VirtualKey, FillOrStrokeListViewItem> KeyDictionary = new Dictionary<VirtualKey, FillOrStrokeListViewItem>();


        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get => this.fillOrStroke;
            set
            {
                FillOrStrokeListViewItem item = this.ItemDictionary[value];
                this.Control.Content = item.Title;
                this.fillOrStroke = value;
            }
        }
        private FillOrStroke fillOrStroke;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a FillOrStrokeComboBox. 
        /// </summary>
        public FillOrStrokeComboBox()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ConstructStrings();

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is FillOrStrokeListViewItem item)
                    {
                        FillOrStroke type = item.FillOrStroke;
                        this.FillOrStrokeChanged?.Invoke(this, type); //Delegate
                        this.Flyout.Hide();
                    }
                }
            };
            this.ListView.KeyDown += (s, e) =>
            {
                VirtualKey key = e.OriginalKey;
                if (this.KeyDictionary.ContainsKey(key) == false) return;

                FillOrStrokeListViewItem item = this.KeyDictionary[key];
                item.Focus(FocusState.Programmatic);
                this.ListView.SelectedIndex = item.Index;
            };
            this.Flyout.Opened += (s, e) =>
            {
                FillOrStrokeListViewItem item = this.ItemDictionary[this.FillOrStroke];
                item.Focus(FocusState.Programmatic);
                this.ListView.SelectedIndex = item.Index;
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            foreach (var kv in this.ItemDictionary)
            {
                FillOrStroke type = kv.Key;
                FillOrStrokeListViewItem item = kv.Value;
                string title = resource.GetString($"Tools_{type}");

                item.Title = title;
            }
        }


        //@Group
        private void InitializeDictionary()
        {
            foreach (object child in this.ListView.Items)
            {
                if (child is FillOrStrokeListViewItem item)
                {
                    FillOrStroke type = item.FillOrStroke;
                    VirtualKey key = item.Key;

                    this.ItemDictionary.Add(type, item);
                    if (key != default) this.KeyDictionary.Add(key, item);
                }
            }
        }

    }
}