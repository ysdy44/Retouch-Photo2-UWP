using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


namespace Retouch_Photo.Controls
{
    public sealed partial class AppbarControl : UserControl
    {
        //Delegate
        public event TappedEventHandler AddButtonTapped;
        public event TappedEventHandler PicturesButtonTapped;

        public event TappedEventHandler SaveButtonTapped;
        public event TappedEventHandler ShareButtonTapped;

        public event TappedEventHandler DeleteButtonTapped;
        public event TappedEventHandler DuplicateButtonTapped;

        public event TappedEventHandler FolderButtonTapped;
        public event TappedEventHandler MoveButtonTapped;


        public AppbarControl()
        {
            this.InitializeComponent();
        }

        //Primry Button

        private void AddButton_Tapped(object sender, TappedRoutedEventArgs e) => this.AddButtonTapped?.Invoke(this, e);
        private void PicturesButton_Tapped(object sender, TappedRoutedEventArgs e) => this.PicturesButtonTapped?.Invoke(this, e);

        private void SaveButton_Tapped(object sender, TappedRoutedEventArgs e) => this.SaveButtonTapped?.Invoke(this, e);
        private void ShareButton_Tapped(object sender, TappedRoutedEventArgs e) => this.ShareButtonTapped?.Invoke(this, e);

        private void DeleteButton_Tapped(object sender, TappedRoutedEventArgs e) => this.DeleteButtonTapped?.Invoke(this, e);
        private void DuplicateButton_Tapped(object sender, TappedRoutedEventArgs e) => this.DuplicateButtonTapped?.Invoke(this, e);

        private void FolderButton_Tapped(object sender, TappedRoutedEventArgs e) => this.FolderButtonTapped?.Invoke(this, e);
        private void MoveButton_Tapped(object sender, TappedRoutedEventArgs e) => this.MoveButtonTapped?.Invoke(this, e);
        
        private void MoreButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Flyout.ShowAt(this.MoreButton);



        //Second Buttons

        private void SecondAddButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Flyout.Hide();
            this.AddButtonTapped?.Invoke(this, e);
        }
        private void SecondPicturesButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Flyout.Hide();
            this.PicturesButtonTapped?.Invoke(this, e);
        }

        private void SecondSaveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Flyout.Hide();
            this.SaveButtonTapped?.Invoke(this, e);
        }
        private void SecondShareButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Flyout.Hide();
            this.ShareButtonTapped?.Invoke(this, e);
        }

        private void SecondDeleteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Flyout.Hide();
            this.DeleteButtonTapped?.Invoke(this, e);
        }
        private void SecondDuplicateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Flyout.Hide();
            this.DuplicateButtonTapped?.Invoke(this, e);
        }

        private void SecondFolderButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Flyout.Hide();
            this.FolderButtonTapped?.Invoke(this, e);
        }
        private void SecondMoveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Flyout.Hide();
            this.MoveButtonTapped?.Invoke(this, e);
        }

    }
}
