using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    public sealed partial class Control : UserControl
    {
        //Delegate
        public delegate void BlendTypeChangedHandler(BlendType type);
        public event BlendTypeChangedHandler TypeChanged = null;

        #region DependencyProperty

        
        public BlendType BlendType
        {
            get { return (BlendType)GetValue(BlendTypeProperty); }
            set { SetValue(BlendTypeProperty, value); }
        }

        public static readonly DependencyProperty BlendTypeProperty = DependencyProperty.Register(nameof(BlendType), typeof(BlendType), typeof(Control), new PropertyMetadata(BlendType.Normal, (sender, e) =>
        {
            Control con = (Control)sender;

            if (e.NewValue is BlendType value)
            {
                int index = (int)value;

                if (index < 0) return;
                if (index >= con.ComboBox.Items.Count) return;

                if (con.ComboBox.SelectedIndex == index) return;

                con.ComboBox.SelectedIndex = index;
            }
        }));
        

        #endregion


        public Control()
        {
            this.InitializeComponent();

            this.ComboBox.Loaded += (sender, e) =>
            {
                this.ComboBox.ItemsSource = Blend.BlendList;

                if (this.ComboBox.SelectedIndex < 0) this.ComboBox.SelectedIndex = 0;
            };

            this.ComboBox.SelectionChanged += (sender, e) =>
            {
                int index = this.ComboBox.SelectedIndex;
                BlendType type = (BlendType)index;

                if (this.BlendType == type) return;
                this.TypeChanged?.Invoke(type); //Delegate
            };
        }
    }
}
