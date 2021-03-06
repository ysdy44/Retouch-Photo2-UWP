﻿using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Photos;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    public partial class BrushTool : Page, ITool
    {
        //@ViewModel
        IBrush Fill { get => this.SelectionViewModel.Fill; set => this.SelectionViewModel.Fill = value; }


        private void FillStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.Fill is null) return;

            // Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this.HandleMode = this.Fill.ContainsHandleMode(startingPoint, matrix);


            if (this.HandleMode == BrushHandleMode.None)
            {
                switch (this.Fill.Type)
                {
                    case BrushType.None:
                    case BrushType.Color:
                        {
                            this.HandleMode = BrushHandleMode.ToInitializeController;

                            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                            this.Fill = BrushBase.LinearGradientBrush(canvasStartingPoint, canvasPoint);
                            this.MethodViewModel.StyleChangeStarted(cache: (style) => style.CacheFill());
                        }
                        break;
                }
            }

            // Cursor
            switch (this.HandleMode)
            {
                case BrushHandleMode.Center:
                case BrushHandleMode.XPoint:
                case BrushHandleMode.YPoint:
                case BrushHandleMode.ToInitializeController:
                    // Cursor
                    CoreCursorExtension.IsManipulationStarted = true;
                    CoreCursorExtension.Cross();
                    break;
            }

            this.Fill.CacheTransform();
            this.MethodViewModel.StyleChangeStarted(cache: (style) =>
            {
                style.CacheFill();

                style.Fill = this.Fill.Clone();
                style.Fill.CacheTransform();
            });
        }

        private void FillDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            // Selection
            if (this.Fill is null) return;

            switch (this.HandleMode)
            {
                case BrushHandleMode.ToInitializeController:
                    this.Fill.InitializeController(canvasStartingPoint, canvasPoint);
                    this.MethodViewModel.StyleChangeDelta(set: (style) => style.Fill.InitializeController(canvasStartingPoint, canvasPoint));
                    break;

                default:
                    this.Fill.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);
                    this.MethodViewModel.StyleChangeDelta(set: (style) => style.Fill.Controller(this.HandleMode, canvasStartingPoint, canvasPoint));
                    break;
            }
        }

        private void FillComplete(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            // Selection
            if (this.Fill is null) return;
            this.Fill.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);
            this.HandleMode = BrushHandleMode.None;

            // Cursor
            CoreCursorExtension.IsManipulationStarted = false;
            CoreCursorExtension.Cross();

            this.MethodViewModel.StyleChangeCompleted
            (
                set: (style) => style.Fill.Controller(this.HandleMode, canvasStartingPoint, canvasPoint),
                type: HistoryType.LayersProperty_SetStyle_Fill,
                getUndo: (style) => style.StartingFill,
                setUndo: (style, previous) => style.Fill = previous.Clone()
            );
        }

        private void FillCursor(Vector2 point)
        {
            if (this.Fill is null) return;

            // Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            BrushHandleMode handleMode = this.Fill.ContainsHandleMode(point, matrix);

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


    public partial class BrushTool : Page, ITool
    {


        private void ConstructFill()
        {
            this.FillBrushButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowFillColorFlyout?.Invoke(this, this.FillBrushButton);
            this.TypeComboBox.FillTypeChanged += async (s, brushType) =>
            {
                if (brushType == BrushType.Image)
                {
                    Photo photo = await Retouch_Photo2.DrawPage.ShowGalleryFunc?.Invoke();

                    this.FillTypeChanged(BrushType.Image, photo);
                }
                else
                {
                    this.FillTypeChanged(brushType);
                }
            };
        }


        //////////////////////////


        private void FillTypeChanged(BrushType brushType, Photo photo = null)
        {
            if (this.Fill.Type == brushType) return;


            IBrush brush = null;

            this.MethodViewModel.StyleChanged
            (
                set: (style, transformer) =>
                {
                    style.Fill.TypeChanged(brushType, transformer, photo);

                    brush = style.Fill;
                },

                type: HistoryType.LayersProperty_SetStyle_Fill_Type,
                getUndo: (style) => style.Fill.Clone(),
                setUndo: (style, previous) => style.Fill = previous.Clone()
            );

            if ((brush is null) == false)
            {
                this.Fill = brush.Clone();

                if (brush.Type == BrushType.Color) this.SelectionViewModel.Color = this.Fill.Color;
            }
        }


        //////////////////////////


        private void FillStopsChanged(CanvasGradientStop[] array)
        {
            this.Fill.Stops = array.CloneArray();

            this.MethodViewModel.StyleChanged
            (
                set: (style, transformer) => style.Fill.Stops = array.CloneArray(),
                type: HistoryType.LayersProperty_SetStyle_Fill,
                getUndo: (style) => style.Fill.Clone(),
                setUndo: (style, previous) => style.Fill = previous.Clone()
            );
        }

        private void FillStopsChangeStarted(CanvasGradientStop[] array)
        {
            this.MethodViewModel.StyleChangeStarted(cache: (style) => style.CacheFill());
        }
        private void FillStopsChangeDelta(CanvasGradientStop[] array)
        {
            this.Fill.Stops = array.CloneArray();

            this.MethodViewModel.StyleChangeDelta(set: (style) => style.Fill.Stops = array.CloneArray());
        }
        private void FillStopsChangeCompleted(CanvasGradientStop[] array)
        {
            this.Fill.Stops = array.CloneArray();

            this.MethodViewModel.StyleChangeCompleted<IBrush>
            (
                set: (style) => style.Fill.Stops = array.CloneArray(),
                type: HistoryType.LayersProperty_SetStyle_Fill,
                getUndo: (style) => style.Fill.Clone(),
                setUndo: (style, previous) => style.Fill = previous.Clone()
            );
        }

        private void FillExtendChanged(CanvasEdgeBehavior extend)
        {
            this.Fill.Extend = extend;
            this.ExtendComboBox.Extend = extend;

            this.MethodViewModel.StyleChanged<CanvasEdgeBehavior>
            (
                set: (style, transformer) => style.Fill.Extend = extend,
                type: HistoryType.LayersProperty_SetStyle_Fill_Extend,
                getUndo: (style) => style.Fill.Extend,
                setUndo: (style, previous) => style.Fill.Extend = previous
            );
        }

    }
}