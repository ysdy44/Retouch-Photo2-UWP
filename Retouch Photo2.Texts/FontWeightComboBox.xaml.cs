// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Texts
{
    internal class FontWeightListViewItem : ListViewItem
    {
        //public string Name { get; set; }
        public FontWeight2 Weight { get; set; }
        public int Index { get; set; }
        public VirtualKey Key { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// Segmented of <see cref="FontWeight2"/>.
    /// </summary>
    public sealed partial class FontWeightComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when weight change. </summary>
        public event EventHandler<FontWeight2> WeightChanged;
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.Flyout.Closed += value; remove => this.Flyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.Flyout.Opened += value; remove => this.Flyout.Opened -= value; }


        //@Group
        private readonly IDictionary<FontWeight2, FontWeightListViewItem> ItemDictionary = new Dictionary<FontWeight2, FontWeightListViewItem>();
        private readonly IDictionary<VirtualKey, FontWeightListViewItem> KeyDictionary = new Dictionary<VirtualKey, FontWeightListViewItem>();


        //@Converter
        private ContentControl TagConverter(FontWeight2 weight2) => new ContentControl
        {
            Template = this.TagTemplate,
            Content = weight2.ToFontWeight().Weight.ToString()
        };


        #region DependencyProperty


        /// <summary> Gets or sets the fontvweight. </summary>
        public FontWeight2 Weight
        {
            get => this.weight;
            set
            {
                FontWeightListViewItem item = this.ItemDictionary[value];
                this.Control.Content = item.Title;
                this.weight = value;
            }
        }
        private FontWeight2 weight;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a FontWeightComboBox. 
        /// </summary>
        public FontWeightComboBox()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ConstructStrings();

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is FontWeightListViewItem item)
                    {
                        FontWeight2 weight = item.Weight;
                        this.WeightChanged?.Invoke(this, weight);//Delegate
                    }
                }
            };
            this.ListView.KeyDown += (s, e) =>
            {
                VirtualKey key = e.OriginalKey;
                if (this.KeyDictionary.ContainsKey(key) == false) return;

                FontWeightListViewItem item = this.KeyDictionary[key];
                item.Focus(FocusState.Programmatic);
                this.ListView.SelectedIndex = item.Index;
            };
            this.Flyout.Opened += (s, e) =>
            {
                FontWeightListViewItem item = this.ItemDictionary[this.Weight];
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
                FontWeight2 weight = kv.Key;
                FontWeightListViewItem item = kv.Value;
                string title = resource.GetString($"Texts_FontWeight_{weight}");

                item.Title = title;
                if (this.Weight == weight) this.Control.Content = title;
            }
        }


        //@Group
        private void InitializeDictionary()
        {
            foreach (object child in this.ListView.Items)
            {
                if (child is FontWeightListViewItem item)
                {
                    FontWeight2 weight = item.Weight;
                    VirtualKey key = item.Key;

                    this.ItemDictionary.Add(weight, item);
                    if (key != default) this.KeyDictionary.Add(key, item);
                }
            }
        }

    }
}