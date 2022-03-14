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

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// ComboBox of Transparency<see cref="BrushType"/>
    /// </summary>
    public sealed partial class TransparencyTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when type changed. </summary>
        public event EventHandler<BrushType> TypeChanged;
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.Flyout.Closed += value; remove => this.Flyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.Flyout.Opened += value; remove => this.Flyout.Opened -= value; }


        //@Group
        private readonly IDictionary<BrushType, BrushTypeListViewItem> ItemDictionary = new Dictionary<BrushType, BrushTypeListViewItem>();
        private readonly IDictionary<VirtualKey, BrushTypeListViewItem> KeyDictionary = new Dictionary<VirtualKey, BrushTypeListViewItem>();


        #region DependencyProperty


        /// <summary> Gets or sets the transparency. </summary>
        public IBrush Transparency
        {
            get => (IBrush)base.GetValue(TransparencyProperty);
            set => base.SetValue(TransparencyProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransparencyTypeComboBox.Fill" /> dependency property. </summary>
        public static readonly DependencyProperty TransparencyProperty = DependencyProperty.Register(nameof(Transparency), typeof(IBrush), typeof(TransparencyTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            TransparencyTypeComboBox control = (TransparencyTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                control.Type = value.Type;
            }
            else
            {
                control.Type = BrushType.None;
            }
        }));


        /// <summary> Gets or sets the type.</summary>
        public BrushType Type
        {
            get => (BrushType)base.GetValue(TypeProperty);
            set => base.SetValue(TypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransparencyTypeComboBox.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(BrushType), typeof(TransparencyTypeComboBox), new PropertyMetadata(BrushType.None, (sender, e) =>
        {
            TransparencyTypeComboBox control = (TransparencyTypeComboBox)sender;

            if (e.NewValue is BrushType value)
            {
                BrushTypeListViewItem item = control.ItemDictionary[value];
                control.Control.Content = item.Title;
            }
        }));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TransparencyTypeComboBox. 
        /// </summary>
        public TransparencyTypeComboBox()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ConstructStrings();

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is BrushTypeListViewItem item)
                    {
                        BrushType type = item.Type;
                        this.TypeChanged?.Invoke(this, type); // Delegate
                        this.Flyout.Hide();
                    }
                }
            };
            this.ListView.KeyDown += (s, e) =>
            {
                VirtualKey key = e.OriginalKey;
                if (this.KeyDictionary.ContainsKey(key) == false) return;

                BrushTypeListViewItem item = this.KeyDictionary[key];
                item.Focus(FocusState.Programmatic);
                this.ListView.SelectedIndex = item.Index;
            };
            this.Flyout.Opened += (s, e) =>
            {
                BrushTypeListViewItem item = this.ItemDictionary[this.Type];
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
                BrushType type = kv.Key;
                BrushTypeListViewItem item = kv.Value;
                string title = resource.GetString($"Tools_Brush_Type_{type}");

                item.Title = title;
            }
        }


        //@Group
        private void InitializeDictionary()
        {
            foreach (object child in this.ListView.Items)
            {
                if (child is BrushTypeListViewItem item)
                {
                    BrushType type = item.Type;
                    VirtualKey key = item.Key;

                    this.ItemDictionary.Add(type, item);
                    if (key != default) this.KeyDictionary.Add(key, item);
                }
            }
        }

    }
}