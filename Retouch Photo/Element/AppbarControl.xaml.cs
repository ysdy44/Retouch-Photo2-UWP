using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Element
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
            this.MoreButton.Tapped += (sender, e) => this.Flyout.ShowAt(this.MoreButton);

            //Primry Button
            this.AddButton.Tapped += (sender, e) => this.Operate(this.AddButtonTapped, e);
            this.PicturesButton.Tapped += (sender, e) => this.Operate(this.PicturesButtonTapped, e);
            this.SaveButton.Tapped += (sender, e) => this.Operate(this.SaveButtonTapped, e);
            this.ShareButton.Tapped += (sender, e) => this.Operate(this.ShareButtonTapped, e);
            this.DeleteButton.Tapped += (sender, e) => this.Operate(this.DeleteButtonTapped, e);
            this.DuplicateButton.Tapped += (sender, e) => this.Operate(this.DuplicateButtonTapped, e);
            this.FolderButton.Tapped += (sender, e) => this.Operate(this.FolderButtonTapped, e);
            this.MoveButton.Tapped += (sender, e) => this.Operate(this.MoveButtonTapped, e);

            //Second Buttons
            this.SecondAddButton.Tapped += (sender, e) => this.OperatHide(this.AddButtonTapped, e);
            this.SecondPicturesButton.Tapped += (sender, e) => this.OperatHide(this.PicturesButtonTapped, e);
            this.SecondSaveButton.Tapped += (sender, e) => this.OperatHide(this.SaveButtonTapped, e);
            this.SecondShareButton.Tapped += (sender, e) => this.OperatHide(this.ShareButtonTapped, e);
            this.SecondDeleteButton.Tapped += (sender, e) => this.OperatHide(this.DeleteButtonTapped, e);
            this.SecondDuplicateButton.Tapped += (sender, e) => this.OperatHide(this.DuplicateButtonTapped, e);
            this.SecondFolderButton.Tapped += (sender, e) => this.OperatHide(this.FolderButtonTapped, e);
            this.SecondMoveButton.Tapped += (sender, e) => this.OperatHide(this.MoveButtonTapped, e);
        }

        private void Operate(TappedEventHandler handler, TappedRoutedEventArgs e) => handler?.Invoke(this, e);
        private void OperatHide(TappedEventHandler handler, TappedRoutedEventArgs e)
        {
            this.Flyout.Hide();
            handler?.Invoke(this, e);
        }
    }
}
