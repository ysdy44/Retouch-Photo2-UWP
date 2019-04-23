using Retouch_Photo2.Blends;
using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls.LayersControls
{
    public sealed partial class BlendControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        //Delegate
        public delegate void IndexChangedHandler(int index);
        public event IndexChangedHandler IndexChanged = null;

        #region DependencyProperty

        public int BlendIndex
        {
            get { return (int)GetValue(BlendIndexProperty); }
            set { SetValue(BlendIndexProperty, value); }
        }
        public static readonly DependencyProperty BlendIndexProperty = DependencyProperty.Register(nameof(BlendIndex), typeof(Layer), typeof(BlendControl), new PropertyMetadata(0, (sender, e) =>
        {
            BlendControl con = (BlendControl)sender;

            if (e.NewValue is int value)
            {
                if (value < 0) return;
                if (value >= con.ComboBox.Items.Count) return;

                if (con.SelectedIndex == value) return;

                con.SelectedIndex = value;
            }
        }));

        #endregion

        public int SelectedIndex
        {
            get=>this.ComboBox.SelectedIndex;
            set=>this.ComboBox.SelectedIndex=value;
        }

        public BlendControl()
        {
            this.InitializeComponent();

            this.ComboBox.Loaded += (sender, e) =>
            {
                this.ComboBox.ItemsSource = Blend.BlendList;

                if (this.SelectedIndex < 0) this.SelectedIndex = 0;
            };

            this.ComboBox.SelectionChanged += (sender, e) =>
            {
                int index = this.SelectedIndex;
                if (this.BlendIndex == index) return;

                this.BlendIndex = index;
                this.IndexChanged?.Invoke(index); //Delegate
            };
        }
    }
}
