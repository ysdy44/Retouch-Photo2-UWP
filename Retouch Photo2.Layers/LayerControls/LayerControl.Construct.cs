using Microsoft.Graphics.Canvas;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer control.
    /// </summary>
    public partial class LayerControl : UserControl
    {

        //Strings
        private void ConstructStrings(ILayer layer)
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Type = resource.GetString($"Layers_{layer.Type}");
        }

        private void ConstructIcon(CanvasDevice customDevice2)
        {
            //IconCanvasControl
            this.IconCanvasControl.UseSharedDevice = true;
            this.IconCanvasControl.CustomDevice = customDevice2;
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
                LayerManager.ItemClick?.Invoke(layer);//Delegate
                e.Handled = true;
            };
            this.RightTapped += (s, e) =>
            {
                LayerManager.RightTapped?.Invoke(layer);//Delegate
                e.Handled = true;
            };
            this.Holding += (s, e) =>
            {
                LayerManager.RightTapped?.Invoke(layer);//Delegate
                e.Handled = true;
            };
            this.DoubleTapped += (s, e) =>
            {
                LayerManager.RightTapped?.Invoke(layer);//Delegate
                e.Handled = true;
            };
        }

        private void ConstructButton(ILayer layer)
        {
            this.VisualButton.Tapped += (s, e) =>
            {
                LayerManager.VisibilityChanged?.Invoke(layer);//Delegate
                e.Handled = true;
            };
            this.VisualButton.RightTapped += (s, e) =>
            {
                LayerManager.VisibilityChanged?.Invoke(layer);//Delegate
                e.Handled = true;
            };
            this.VisualButton.Holding += (s, e) => e.Handled = true;
            this.VisualButton.DoubleTapped += (s, e) => e.Handled = true;


            this.ExpanedButton.Tapped += (s, e) =>
            {
                LayerManager.IsExpandChanged?.Invoke(layer);//Delegate   
                e.Handled = true;
            };
            this.ExpanedButton.RightTapped += (s, e) =>
            {
                LayerManager.IsExpandChanged?.Invoke(layer);//Delegate   
                e.Handled = true;
            };
            this.ExpanedButton.Holding += (s, e) => e.Handled = true;
            this.ExpanedButton.DoubleTapped += (s, e) => e.Handled = true;


            this.SelectedButton.Tapped += (s, e) =>
            {
                LayerManager.IsSelectedChanged?.Invoke(layer);//Delegate   
                e.Handled = true;
            };
            this.SelectedButton.RightTapped += (s, e) =>
            {
                LayerManager.IsSelectedChanged?.Invoke(layer);//Delegate   
                e.Handled = true;
            };
            this.SelectedButton.Holding += (s, e) => e.Handled = true;
            this.SelectedButton.DoubleTapped += (s, e) => e.Handled = true;
        }

        private void ConstructManipulation(ILayer layer)
        {
            this.ManipulationStarted += (s, e) =>
            {
                LayerManager.IsOverlay = true;
                LayerManager.DragItemsStarted?.Invoke(layer, this.ManipulationMode);//Delegate     
            };
            this.ManipulationCompleted += (s, e) =>
            {
                if (LayerManager.IsOverlay)
                {
                    LayerManager.DragItemsCompleted?.Invoke();//Delegate

                    LayerManager.IsOverlay = false;
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
                if (LayerManager.IsOverlay)
                {
                    Point position = e.GetCurrentPoint(this).Position;
                    OverlayMode overlayMode = this.GetOverlay(position.Y);

                    this.OverlayMode = overlayMode;
                    LayerManager.DragItemsDelta?.Invoke(layer, overlayMode);//Delegate
                }
            };
            this.PointerExited += (s, e) => this.OverlayMode = OverlayMode.None;
            this.PointerReleased += (s, e) => this.OverlayMode = OverlayMode.None;
        }

    }
}