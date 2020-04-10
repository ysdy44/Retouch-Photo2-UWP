using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Linq;
using System.Threading.Tasks;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public partial class LayersControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Content
        /// <summary> IndicatorBorder. </summary>
        public Border IndicatorBorder => this._IndicatorBorder;


        //LayerCollection
        ILayer DragSourceLayer;
        ILayer DragDestinationLayer;
        SelectMode DragLayerSelectMode;
        OverlayMode DragLayerOverlayMode;


        //@Construct
        public LayersControl()
        {
            this.InitializeComponent();
            //LayerCollection
            this.ConstructLayerCollection();
            this.ItemsControl.ItemsSource = this.ViewModel.Layers.RootControls;


            this.Tapped += (s, e) =>
            {
                foreach (ILayer child in this.ViewModel.Layers.RootLayers)
                {
                    child.SelectMode = SelectMode.UnSelected;
                }

                this.SelectionViewModel.SetModeNone();//Selection
                this.ViewModel.Invalidate();
            };
            this.RightTapped += (s, e) => this.ShowLayerMenu();
            this.Holding += (s, e) => this.ShowLayerMenu();            
        }

        private void ShowLayerMenu()
        {
            this.TipViewModel.SetMenuState(MenuType.Layer, MenuState.FlyoutHide, MenuState.FlyoutShow);
        }
        private void ShowLayerMenu(ILayer layer)
        {
            Point rootGridPosition = layer.Control.Self.TransformToVisual(this).TransformPoint(new Point());
            Canvas.SetTop(this._IndicatorBorder, rootGridPosition.Y);

            this.ShowLayerMenu();
        }
        
    }
}