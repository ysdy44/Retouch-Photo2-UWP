// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    internal class BlendModeListViewItem : ListViewItem
    {
        //public string Name { get; set; }
        public BlendEffectMode Mode { get; set; }
        public int Index { get; set; }
        public VirtualKey Key { get; set; }

        #region DependencyProperty

        /// <summary> Gets or sets the title. </summary>
        public string Title
        {
            get => (string)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "BlendModeListViewItem.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(BlendModeListViewItem), new PropertyMetadata(string.Empty));

        #endregion
    }

    /// <summary>
    /// ComboBox of <see cref="BlendEffectMode?"/>
    /// </summary>
    public sealed partial class BlendModeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when mode change. </summary>
        public event EventHandler<BlendEffectMode?> ModeChanged;
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.Flyout.Closed += value; remove => this.Flyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.Flyout.Opened += value; remove => this.Flyout.Opened -= value; }


        //@Group
        private readonly IDictionary<BlendEffectMode, BlendModeListViewItem> ItemDictionary = new Dictionary<BlendEffectMode, BlendModeListViewItem>();
        private readonly IDictionary<VirtualKey, BlendModeListViewItem> KeyDictionary = new Dictionary<VirtualKey, BlendModeListViewItem>();



        #region DependencyProperty


        /// <summary> Gets or sets the blend-type. </summary>
        public BlendEffectMode? Mode
        {
            get => this.mode;
            set
            {
                BlendModeListViewItem item =
                    (value is BlendEffectMode mode) ?
                    this.ItemDictionary[mode] :
                    this.None;
                this.Control.Content = item.Title;
                this.mode = value;
            }
        }
        private BlendEffectMode? mode;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a BlendModeComboBox. 
        /// </summary>
        public BlendModeComboBox()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ConstructStrings();
            base.Loaded += (s, e) => this.ConstructLanguages();

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is BlendModeListViewItem item)
                    {
                        if (item == this.None)
                        {
                            this.ModeChanged?.Invoke(this, null); // Delegate
                        }
                        else
                        {
                            BlendEffectMode weight = item.Mode;
                            this.ModeChanged?.Invoke(this, weight); // Delegate
                        }
                    }
                }
            };
            this.ListView.KeyDown += (s, e) =>
            {
                VirtualKey key = e.OriginalKey;
                if (this.KeyDictionary.ContainsKey(key) == false) return;

                BlendModeListViewItem item = this.KeyDictionary[key];
                item.Focus(FocusState.Programmatic);
                this.ListView.SelectedIndex = item.Index;
            };
            this.Flyout.Opened += (s, e) =>
            {
                BlendModeListViewItem item =
                    (this.Mode is BlendEffectMode mode) ?
                    this.ItemDictionary[mode] :
                    this.None;
                item.Focus(FocusState.Programmatic);
                this.ListView.SelectedIndex = item.Index;
            };
        }
    }

    public sealed partial class BlendModeComboBox : UserControl
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

            {
                BlendModeListViewItem item = this.None;
                string title = resource.GetString($"Blends_None");

                item.Title = title;
                if (this.Mode == null) this.Control.Content = title;
            }

            foreach (var kv in this.ItemDictionary)
            {
                BlendEffectMode weight = kv.Key;
                BlendModeListViewItem item = kv.Value;
                string title = resource.GetString($"Blends_{weight}");

                item.Title = title;
                if (this.Mode == weight) this.Control.Content = title;
            }
        }


        //@Group
        private void InitializeDictionary()
        {
            foreach (object child in this.ListView.Items)
            {
                if (child is BlendModeListViewItem item)
                {
                    if (item == this.None) continue;

                    BlendEffectMode weight = item.Mode;
                    VirtualKey key = item.Key;

                    this.ItemDictionary.Add(weight, item);
                    if (key != default) this.KeyDictionary.Add(key, item);
                }
            }
        }

    }
}