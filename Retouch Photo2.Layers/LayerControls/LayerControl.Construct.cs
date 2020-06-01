using Microsoft.Graphics.Canvas;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer control.
    /// </summary>
    public partial class LayerControl : UserControl
    {

        private void ConstructIcon(CanvasDevice customDevice)
        {
            //IconCanvasControl
            this.IconCanvasControl.UseSharedDevice = true;
            this.IconCanvasControl.CustomDevice = customDevice;
            this.IconCanvasControl.Draw += (s, arge) =>
            {
                if (this.IconRender == null) return;

                arge.DrawingSession.DrawImage(this.IconRender);
            };
        }

        private void ConstructTapped(ILayer layer)
        {
            this.Tapped += (s, e) =>
            {
                LayerageCollection.ItemClick?.Invoke(layer);//Delegate
                e.Handled = true;
            };

            this.RightTapped += (s, e) =>
            {
                LayerageCollection.RightTapped?.Invoke(layer);//Delegate
                e.Handled = true;
            };
            this.Holding += (s, e) =>
            {
                LayerageCollection.RightTapped?.Invoke(layer);//Delegate
                e.Handled = true;
            };
            this.DoubleTapped += (s, e) =>
            {
                LayerageCollection.RightTapped?.Invoke(layer);//Delegate
                e.Handled = true;
            };
        }

        private void ConstructButton(ILayer layer)
        {
            this.VisualButton.Tapped += (s, e) =>
            {
                LayerageCollection.VisibilityChanged?.Invoke(layer);//Delegate
                e.Handled = true;
            };
            this.ExpanedButton.Tapped += (s, e) =>
            {
                LayerageCollection.IsExpandChanged?.Invoke(layer);//Delegate   
                e.Handled = true;
            };
            this.SelectedButton.Tapped += (s, e) =>
            {
                LayerageCollection.IsSelectedChanged?.Invoke(layer);//Delegate   
                e.Handled = true;
            };
        }

        private void ConstructManipulation(ILayer layer)
        {
            this.ManipulationStarted += (s, e) =>
            {
                LayerageCollection.IsOverlay = true;
                LayerageCollection.DragItemsStarted?.Invoke(layer, this.ManipulationMode);//Delegate     
            };
            this.ManipulationCompleted += (s, e) =>
            {
                if (LayerageCollection.IsOverlay)
                {
                    LayerageCollection.DragItemsCompleted?.Invoke();//Delegate

                    LayerageCollection.IsOverlay = false;
                    this.OverlayMode = OverlayMode.None;
                }
            };
        }

        private void ConstructPointer(ILayer layer)
        {
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;

            this.PointerMoved += (s, e) =>
            {
                if (LayerageCollection.IsOverlay)
                {
                    Point position = e.GetCurrentPoint(this).Position;
                    OverlayMode overlayMode = this.GetOverlay(position.Y);

                    this.OverlayMode = overlayMode;
                    LayerageCollection.DragItemsDelta?.Invoke(layer, overlayMode);//Delegate
                }
            };
            this.PointerExited += (s, e) => this.OverlayMode = OverlayMode.None;
            this.PointerReleased += (s, e) => this.OverlayMode = OverlayMode.None;
        }

    }
}