using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Photos;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    public partial class TransparencyTool : Page, ITool
    {
        //@ViewModel
        IBrush Transparency { get => this.SelectionViewModel.Transparency; set => this.SelectionViewModel.Transparency = value; }


        private void TransparencyStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.Transparency is null) return;

            // Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this.HandleMode = this.Transparency.ContainsHandleMode(startingPoint, matrix);


            if (this.HandleMode == BrushHandleMode.None)
            {
                switch (this.Transparency.Type)
                {
                    case BrushType.None:
                    case BrushType.Color:
                        {
                            this.HandleMode = BrushHandleMode.ToInitializeController;

                            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                            this.Transparency = BrushBase.LinearGradientBrush(canvasStartingPoint, canvasPoint, Colors.Transparent);
                            this.MethodViewModel.StyleChangeStarted(cache: (style) => style.CacheTransparency());
                        }
                        break;
                }
            }


            this.Transparency.CacheTransform();
            this.MethodViewModel.StyleChangeStarted(cache: (style) =>
            {
                style.CacheTransparency();

                style.Transparency = this.Transparency.Clone();
                style.Transparency.CacheTransform();
            });
        }

        private void TransparencyDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            // Selection
            if (this.Transparency is null) return;

            switch (this.HandleMode)
            {
                case BrushHandleMode.ToInitializeController:
                    this.Transparency.InitializeController(canvasStartingPoint, canvasPoint);
                    this.MethodViewModel.StyleChangeDelta(set: (style) => style.Transparency.InitializeController(canvasStartingPoint, canvasPoint));
                    break;

                default:
                    this.Transparency.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);
                    this.MethodViewModel.StyleChangeDelta(set: (style) => style.Transparency.Controller(this.HandleMode, canvasStartingPoint, canvasPoint));
                    break;
            }
        }

        private void TransparencyComplete(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            // Selection
            if (this.Transparency is null) return;
            this.Transparency.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);

            this.MethodViewModel.StyleChangeCompleted
            (
                set: (style) => style.Transparency.Controller(this.HandleMode, canvasStartingPoint, canvasPoint),
                type: HistoryType.LayersProperty_SetStyle_Transparency,
                getUndo: (style) => style.StartingTransparency,
                setUndo: (style, previous) => style.Transparency = previous.Clone()
            );
        }

        private void TransparencyCursor(Vector2 point)
        {
            if (this.Transparency is null) return;

            // Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            BrushHandleMode handleMode = this.Transparency.ContainsHandleMode(point, matrix);

            // Cursor
            switch (handleMode)
            {
                case BrushHandleMode.Center:
                case BrushHandleMode.XPoint:
                case BrushHandleMode.YPoint:
                case BrushHandleMode.ToInitializeController:
                    // Cursor
                    CoreCursorExtension.IsPointerEntered = true;
                    CoreCursorExtension.Cross();
                    break;
                default:
                    // Cursor
                    CoreCursorExtension.IsPointerEntered = false;
                    CoreCursorExtension.Cross();
                    break;
            }
        }

    }


    public partial class TransparencyTool : Page, ITool
    {

        private void ConstructTransparency()
        {
            this.TypeComboBox.TypeChanged += (s, brushType) =>
            {
                this.TransparencyTypeChanged(brushType);
            };
            this.TypeComboBox.Closed += (s, e) => this.SettingViewModel.RegisteKey(); // Setting
            this.TypeComboBox.Opened += (s, e) => this.SettingViewModel.UnregisteKey(); // Setting
        }


        //////////////////////////


        private void TransparencyTypeChanged(BrushType brushType, Photo photo = null)
        {
            if (this.Transparency.Type == brushType) return;


            IBrush brush = null;

            this.MethodViewModel.StyleChanged
            (
                set: (style, transformer) =>
                {
                    style.Transparency.TypeChanged(brushType, transformer, Colors.Transparent, photo);

                    brush = style.Transparency;
                },

                type: HistoryType.LayersProperty_SetStyle_Transparency_Type,
                getUndo: (style) => style.Transparency.Clone(),
                setUndo: (style, previous) => style.Transparency = previous.Clone()
            );

            if ((brush is null) == false)
            {
                this.Transparency = brush.Clone();

                if (brush.Type == BrushType.Color) this.SelectionViewModel.Color = this.Transparency.Color;
            }
        }


        //////////////////////////


        private void TransparencyStopsChanged(CanvasGradientStop[] array)
        {
            this.Transparency.Stops = array.CloneArray();

            this.MethodViewModel.StyleChanged
            (
                set: (style, transformer) => style.Transparency.Stops = array.CloneArray(),
                type: HistoryType.LayersProperty_SetStyle_Transparency,
                getUndo: (style) => style.Transparency.Clone(),
                setUndo: (style, previous) => style.Transparency = previous.Clone()
            );
        }

        private void TransparencyStopsChangeStarted(CanvasGradientStop[] array)
        {
            this.MethodViewModel.StyleChangeStarted(cache: (style) => style.CacheTransparency());
        }
        private void TransparencyStopsChangeDelta(CanvasGradientStop[] array)
        {
            this.Transparency.Stops = array.CloneArray();

            this.MethodViewModel.StyleChangeDelta(set: (style) => style.Transparency.Stops = array.CloneArray());
        }
        private void TransparencyStopsChangeCompleted(CanvasGradientStop[] array)
        {
            this.Transparency.Stops = array.CloneArray();

            this.MethodViewModel.StyleChangeCompleted<IBrush>
            (
                set: (style) => style.Transparency.Stops = array.CloneArray(),
                type: HistoryType.LayersProperty_SetStyle_Transparency,
                getUndo: (style) => style.Transparency.Clone(),
                setUndo: (style, previous) => style.Transparency = previous.Clone()
            );
        }

    }
}