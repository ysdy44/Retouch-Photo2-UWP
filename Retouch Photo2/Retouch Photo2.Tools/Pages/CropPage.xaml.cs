using FanKit.Transformers;
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
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

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
                });

                this.SelectionViewModel.IsCrop = false;//Selection
                this.ViewModel.Invalidate();//Invalidate
            };
            this.FitButton.Tapped += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer.TransformManager.IsCrop)
                    {
                        Transformer cropTransformer = layer.TransformManager.CropDestination;
                        layer.TransformManager.Destination = cropTransformer;
                        layer.TransformManager.IsCrop = false;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            
            this.MoreButton.Tapped += (s, e) => this.Flyout.ShowAt(this);
        }
    }
}