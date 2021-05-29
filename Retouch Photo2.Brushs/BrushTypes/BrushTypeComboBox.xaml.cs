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
    internal class BrushTypeListViewItem : ListViewItem
    {
        //public string Name { get; set; }
        public BrushType Type { get; set; }
        public int Index { get; set; }
        public VirtualKey Key { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// ComboBox of <see cref="BrushType"/>
    /// </summary>
    public sealed partial class BrushTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when fill type change. </summary>
        public event EventHandler<BrushType> FillTypeChanged;
        /// <summary> Occurs when stroke type change. </summary>
        public event EventHandler<BrushType> StrokeTypeChanged;
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.Flyout.Closed += value; remove => this.Flyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.Flyout.Opened += value; remove => this.Flyout.Opened -= value; }


        //@Group
        private readonly IDictionary<BrushType, BrushTypeListViewItem> ItemDictionary = new Dictionary<BrushType, BrushTypeListViewItem>();
        private readonly IDictionary<VirtualKey, BrushTypeListViewItem> KeyDictionary = new Dictionary<VirtualKey, BrushTypeListViewItem>();


        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get => this.fillOrStroke;
            set
            {
                switch (value)
                {
                    case FillOrStroke.Fill:
                        this.Type = this.FillType;
                        break;
                    case FillOrStroke.Stroke:
                        this.Type = this.StrokeType;
                        break;
                }
                this.fillOrStroke = value;
            }
        }
        private FillOrStroke fillOrStroke = FillOrStroke.Fill;


        /// <summary> Gets or sets the fill. </summary>
        public IBrush Fill
        {
            get => (IBrush)base.GetValue(FillProperty);
            set => base.SetValue(FillProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Fill" /> dependency property. </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(IBrush), typeof(BrushTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BrushTypeComboBox control = (BrushTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                control.FillType = value.Type;
            }
            else
            {
                control.FillType = BrushType.None;
            }
        }));

        /// <summary> Gets or sets the fill type. </summary>
        public BrushType FillType
        {
            get => this.fillType;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.Type = value;
                        break;
                }
                this.fillType = value;
            }
        }
        private BrushType fillType = BrushType.None;


        /// <summary> Gets or sets the stroke. </summary>
        public IBrush Stroke
        {
            get => (IBrush)base.GetValue(StrokeProperty);
            set => base.SetValue(StrokeProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Stroke" /> dependency property. </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(IBrush), typeof(BrushTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BrushTypeComboBox control = (BrushTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                control.StrokeType = value.Type;
            }
            else
            {
                control.StrokeType = BrushType.None;
            }
        }));

        /// <summary> Gets or sets the stroke type. </summary>
        public BrushType StrokeType
        {
            get => this.strokeType;
            set
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        this.Type = value;
                        break;
                }
                this.strokeType = value;
            }
        }
        private BrushType strokeType = BrushType.None;


        /// <summary> Gets or sets the type.</summary>
        public BrushType Type
        {
            get => (BrushType)base.GetValue(TypeProperty);
            set => base.SetValue(TypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(BrushType), typeof(BrushTypeComboBox), new PropertyMetadata(BrushType.None, (sender, e) =>
        {
            BrushTypeComboBox control = (BrushTypeComboBox)sender;

            if (e.NewValue is BrushType value)
            {
                BrushTypeListViewItem item = control.ItemDictionary[value];
                control.Control.Content = item.Title;
            }
        }));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a BrushTypeComboBox. 
        /// </summary>
        public BrushTypeComboBox()
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
                        switch (this.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                this.FillTypeChanged?.Invoke(this, type); // Delegate
                                break;
                            case FillOrStroke.Stroke:
                                this.StrokeTypeChanged?.Invoke(this, type); // Delegate
                                break;
                        }
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