using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "CropTool"/>.
    /// </summary>
    public sealed partial class CropPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;       
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }

        //@Construct
        public CropPage()
        {
            this.InitializeComponent();
            this.ResetButton.Tapped += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformManager.IsCrop = false;
                }, true);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }
}