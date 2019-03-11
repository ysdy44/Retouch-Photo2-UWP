using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;



namespace Retouch_Photo.Pickers
{
    public sealed partial class SwatchesPicker : UserControl, IPicker
    {
        //Delegate
        public event ColorChangeHandler ColorChange = null;

        private Color color = Color.FromArgb(255, 255, 255, 255);
        public Color GetColor() => color;
        public void SetColor(Color value) => color = value;


        private bool isMultiSelect;
        public bool IsMultiSelect
        {
            get => isMultiSelect;
            set
            {
                if (value)
                {
                    this.AddButton.IsEnabled = false;

                    this.GridView.CanReorderItems = true;
                    this.GridView.IsSwipeEnabled = true;
                    this.GridView.AllowDrop = true;

                    this.GridView.IsItemClickEnabled = false;
                    this.GridView.SelectionMode = ListViewSelectionMode.Multiple;
                }
                else
                {
                    this.AddButton.IsEnabled = true;

                    this.GridView.CanReorderItems = false;
                    this.GridView.IsSwipeEnabled = false;
                    this.GridView.AllowDrop = false;

                    this.GridView.IsItemClickEnabled = true;
                    this.GridView.SelectionMode = ListViewSelectionMode.Single;
                }
                isMultiSelect = value;
            }
        }

        ObservableCollection<SolidColorBrush> Collection = new ObservableCollection<SolidColorBrush>
        {
            new SolidColorBrush(Color.FromArgb(255,0,0,0)),new SolidColorBrush(Color.FromArgb(255,31,31,31)),new SolidColorBrush(Color.FromArgb(255,63,63,63)),new SolidColorBrush(Color.FromArgb(255,95,95,95)),new SolidColorBrush(Color.FromArgb(255,127,127,127)),new SolidColorBrush(Color.FromArgb(255,159,159,159)),new SolidColorBrush(Color.FromArgb(255,191,191,191)), new SolidColorBrush(Color.FromArgb(255,223,223,223)), new SolidColorBrush(Color.FromArgb(255,255,255,255)),
            new SolidColorBrush(Color.FromArgb(255, 255, 192, 203)),new SolidColorBrush(Color.FromArgb(255, 220, 20, 60)),new SolidColorBrush(Color.FromArgb(255, 255, 240, 245)),new SolidColorBrush(Color.FromArgb(255, 219, 112, 147)),new SolidColorBrush(Color.FromArgb(255, 255, 105, 180)),new SolidColorBrush(Color.FromArgb(255, 199, 21, 133)),new SolidColorBrush(Color.FromArgb(255, 218, 112, 214)),new SolidColorBrush(Color.FromArgb(255, 216, 191, 216)),
            new SolidColorBrush(Color.FromArgb(255, 221, 160, 221)),new SolidColorBrush(Color.FromArgb(255, 238, 130, 238)),new SolidColorBrush(Color.FromArgb(255, 255, 0, 255)),new SolidColorBrush(Color.FromArgb(255, 139, 0, 139)),new SolidColorBrush(Color.FromArgb(255, 128, 0, 128)),new SolidColorBrush(Color.FromArgb(255, 186, 85, 211)),new SolidColorBrush(Color.FromArgb(255, 148, 0, 211)),new SolidColorBrush(Color.FromArgb(255, 75, 0, 130)),
            new SolidColorBrush(Color.FromArgb(255, 138, 43, 226)),new SolidColorBrush(Color.FromArgb(255, 147, 112, 219)),new SolidColorBrush(Color.FromArgb(255, 123, 104, 238)),new SolidColorBrush(Color.FromArgb(255, 106, 90, 205)),new SolidColorBrush(Color.FromArgb(255, 72, 61, 139)),new SolidColorBrush(Color.FromArgb(255, 230, 230, 250)),new SolidColorBrush(Color.FromArgb(255, 0, 0, 205)),new SolidColorBrush(Color.FromArgb(255, 25, 25, 112)),
            new SolidColorBrush(Color.FromArgb(255, 0, 0, 139)),new SolidColorBrush(Color.FromArgb(255, 0, 0, 128)),new SolidColorBrush(Color.FromArgb(255, 65, 105, 225)),new SolidColorBrush(Color.FromArgb(255, 100, 149, 237)),new SolidColorBrush(Color.FromArgb(255, 119, 136, 153)),new SolidColorBrush(Color.FromArgb(255, 112, 128, 144)),new SolidColorBrush(Color.FromArgb(255, 30, 144, 255)),new SolidColorBrush(Color.FromArgb(255, 240, 248, 255)),
            new SolidColorBrush(Color.FromArgb(255, 70, 130, 180)),new SolidColorBrush(Color.FromArgb(255, 135, 206, 250)),new SolidColorBrush(Color.FromArgb(255, 135, 206, 235)),new SolidColorBrush(Color.FromArgb(255, 0, 191, 255)),new SolidColorBrush(Color.FromArgb(255, 173, 216, 230)),new SolidColorBrush(Color.FromArgb(255, 176, 216, 230)),new SolidColorBrush(Color.FromArgb(255, 95, 158, 160)),new SolidColorBrush(Color.FromArgb(255, 240, 255, 255)),
            new SolidColorBrush(Color.FromArgb(255, 224, 255, 255)),new SolidColorBrush(Color.FromArgb(255, 175, 238, 238)),new SolidColorBrush(Color.FromArgb(255, 0, 255, 255)),new SolidColorBrush(Color.FromArgb(255, 0, 206, 209)),new SolidColorBrush(Color.FromArgb(255, 47, 79, 79)),new SolidColorBrush(Color.FromArgb(255, 0, 139, 139)),new SolidColorBrush(Color.FromArgb(255, 0, 128, 128)),new SolidColorBrush(Color.FromArgb(255, 72, 209, 204)),
            new SolidColorBrush(Color.FromArgb(255, 32, 178, 170)),new SolidColorBrush(Color.FromArgb(255, 64, 224, 208)),new SolidColorBrush(Color.FromArgb(255, 127, 255, 212)),new SolidColorBrush(Color.FromArgb(255, 102, 205, 170)),new SolidColorBrush(Color.FromArgb(255, 0, 250, 154)),new SolidColorBrush(Color.FromArgb(255, 245, 255, 250)),new SolidColorBrush(Color.FromArgb(255, 0, 255, 127)),new SolidColorBrush(Color.FromArgb(255, 60, 179, 113)),
            new SolidColorBrush(Color.FromArgb(255, 46, 139, 87)),new SolidColorBrush(Color.FromArgb(255, 144, 238, 144)),new SolidColorBrush(Color.FromArgb(255, 152, 251, 152)),new SolidColorBrush(Color.FromArgb(255, 143, 188, 143)),new SolidColorBrush(Color.FromArgb(255, 50, 205, 50)),new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)),new SolidColorBrush(Color.FromArgb(255, 34, 139, 34)),new SolidColorBrush(Color.FromArgb(255, 127, 255, 0)),
            new SolidColorBrush(Color.FromArgb(255, 124, 252, 0)),new SolidColorBrush(Color.FromArgb(255, 173, 255, 47)),new SolidColorBrush(Color.FromArgb(255, 85, 107, 47)),new SolidColorBrush(Color.FromArgb(255, 154, 205, 50)),new SolidColorBrush(Color.FromArgb(255, 107, 142, 35)),new SolidColorBrush(Color.FromArgb(255, 245, 245, 220)),new SolidColorBrush(Color.FromArgb(255, 250, 250, 210)),new SolidColorBrush(Color.FromArgb(255, 255, 255, 240)),
            new SolidColorBrush(Color.FromArgb(255, 255, 255, 224)),new SolidColorBrush(Color.FromArgb(255, 255, 255, 0)),new SolidColorBrush(Color.FromArgb(255, 128, 128, 0)),new SolidColorBrush(Color.FromArgb(255, 189, 183, 107)),new SolidColorBrush(Color.FromArgb(255, 255, 250, 205)),new SolidColorBrush(Color.FromArgb(255, 238, 232, 170)),new SolidColorBrush(Color.FromArgb(255, 240, 230, 140)),new SolidColorBrush(Color.FromArgb(255, 255, 215, 0)),
            new SolidColorBrush(Color.FromArgb(255, 255, 248, 220)),new SolidColorBrush(Color.FromArgb(255, 218, 165, 32)),new SolidColorBrush(Color.FromArgb(255, 184, 134, 11)),new SolidColorBrush(Color.FromArgb(255, 255, 250, 240)),new SolidColorBrush(Color.FromArgb(255, 253, 245, 230)),new SolidColorBrush(Color.FromArgb(255, 245, 222, 179)),new SolidColorBrush(Color.FromArgb(255, 255, 228, 181)),new SolidColorBrush(Color.FromArgb(255, 255, 165, 0)),
            new SolidColorBrush(Color.FromArgb(255, 255, 239, 213)),new SolidColorBrush(Color.FromArgb(255, 255, 235, 205)),new SolidColorBrush(Color.FromArgb(255, 255, 222, 173)),new SolidColorBrush(Color.FromArgb(255, 250, 235, 215)),new SolidColorBrush(Color.FromArgb(255, 210, 180, 140)),new SolidColorBrush(Color.FromArgb(255, 222, 184, 135)),new SolidColorBrush(Color.FromArgb(255, 255, 228, 196)),new SolidColorBrush(Color.FromArgb(255, 255, 140, 0)),
            new SolidColorBrush(Color.FromArgb(255, 250, 240, 230)),new SolidColorBrush(Color.FromArgb(255, 205, 133, 63)),new SolidColorBrush(Color.FromArgb(255, 244, 164, 96)),new SolidColorBrush(Color.FromArgb(255, 210, 105, 30)),new SolidColorBrush(Color.FromArgb(255, 255, 245, 238)),new SolidColorBrush(Color.FromArgb(255, 160, 82, 45)),new SolidColorBrush(Color.FromArgb(255, 255, 160, 122)),new SolidColorBrush(Color.FromArgb(255, 255, 160, 122)),
            new SolidColorBrush(Color.FromArgb(255, 255, 69, 0)),new SolidColorBrush(Color.FromArgb(255, 255, 99, 71)),new SolidColorBrush(Color.FromArgb(255, 255, 228, 225)),new SolidColorBrush(Color.FromArgb(255, 250, 128, 114)),new SolidColorBrush(Color.FromArgb(255, 255, 250, 250)),new SolidColorBrush(Color.FromArgb(255, 240, 128, 128)),new SolidColorBrush(Color.FromArgb(255, 188, 143, 143)),new SolidColorBrush(Color.FromArgb(255, 205, 92, 92)),
            new SolidColorBrush(Color.FromArgb(255, 165, 42, 42)),new SolidColorBrush(Color.FromArgb(255, 178, 34, 34)),new SolidColorBrush(Color.FromArgb(255, 139, 0, 0)),new SolidColorBrush(Color.FromArgb(255, 128, 0, 0)),
        };

        public SwatchesPicker()
        {
            this.InitializeComponent();
        }

        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Collection.RemoveAt(this.GridView.SelectedIndex == -1 ? 0 : this.GridView.SelectedIndex);
        private void AddButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Collection.Insert(0, new SolidColorBrush(this.color));
        private void MultiSelectToggleButton_Checked(object sender, RoutedEventArgs e) => this.IsMultiSelect = true;
        private void MultiSelectToggleButton_Unchecked(object sender, RoutedEventArgs e) => this.IsMultiSelect = false;


        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is SolidColorBrush item)
            {
                this.color = item.Color;
                this.ColorChange?.Invoke(this, this.color);
            }
        }
        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.GridView.SelectedIndex;
            this.RemoveButton.IsEnabled = (index >= 0 && index < Collection.Count);
        }

    }
}
