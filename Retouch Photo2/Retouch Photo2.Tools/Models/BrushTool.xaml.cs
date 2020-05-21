﻿using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public sealed partial class BrushTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        FillOrStroke FillOrStroke { get => this.SelectionViewModel.FillOrStroke; set => this.SelectionViewModel.FillOrStroke = value; }

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Construct
        public BrushTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructShowControl();

            //FillOrStroke
            this.FillOrStrokeComboBox.FillOrStrokeChanged += (s, fillOrStroke) =>
            {
                this.FillOrStroke = fillOrStroke;
                this.ViewModel.Invalidate(); //Invalidate
            };

            //BrushType
            this.ConstructFillImage();
            this.ConstructStrokeImage();
        }


        //ShowControl
        private void ConstructShowControl()
        {
            this.ShowControl.Tapped += (s, e) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillShow();
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeShow();
                        break;
                }
            };


            this.StopsPicker.StopsChanged += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChanged(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChanged(array);
                        break;
                }
            };
            this.StopsPicker.StopsChangeStarted += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChangeStarted(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChangeStarted(array);
                        break;
                }
            };
            this.StopsPicker.StopsChangeDelta += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChangeDelta(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChangeDelta(array);
                        break;
                }
            };
            this.StopsPicker.StopsChangeCompleted += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChangeCompleted(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChangeCompleted(array);
                        break;
                }
            };


            this.ExtendComboBox.ExtendChanged += (s, extend) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillExtendChanged(extend);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeExtendChanged(extend);
                        break;
                }
            };
        }


        public void OnNavigatedTo() => this.SelectionViewModel.SetModeStyle();
        public void OnNavigatedFrom() { }

    }

    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public sealed partial class BrushTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/Brush");

            this.FillOrStrokeTextBlock.Text = resource.GetString("/Tools/Brush_FillOrStroke");
            this.BrushTypeTextBlock.Text = resource.GetString("/Tools/Brush_Type");
            this.BrushTextBlock.Text = resource.GetString("/Tools/Brush_Brush");

            this.ExtendTextBlock.Text = resource.GetString("/Tools/Brush_Extend");
        }


        //@Content
        public ToolType Type => ToolType.Brush;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new BrushIcon();
        readonly ToolButton _button = new ToolButton(new BrushIcon());


        BrushHandleMode OperateMode = BrushHandleMode.None;


        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            //Snap
            if (this.IsSnap) this.ViewModel.VectorBorderSnapStarted(this.SelectionViewModel.Transformer);

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillStarted(startingPoint, point);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeStarted(startingPoint, point);
                    break;
            }

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            //Snap
            if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillDelta(canvasStartingPoint, canvasPoint);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeDelta(canvasStartingPoint, canvasPoint);
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            //Snap
            if (this.IsSnap)
            {
                canvasPoint = this.Snap.Snap(canvasPoint);
                this.Snap.Default();
            }

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillDelta(canvasStartingPoint, canvasPoint);
                    this.FillComplete();
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeDelta(canvasStartingPoint, canvasPoint);
                    this.StrokeComplete();
                    break;
            }

            this.OperateMode = BrushHandleMode.None;
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);


        public void Draw(CanvasDrawingSession drawingSession)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            //Draw
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Color accentColor = this.ViewModel.AccentColor;

            //Snapping
            if (this.IsSnap) this.Snap.Draw(drawingSession, matrix);

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.Fill.Draw(drawingSession, matrix, accentColor);
                    break;
                case FillOrStroke.Stroke:
                    this.Stroke.Draw(drawingSession, matrix, accentColor);
                    break;
            }
        }

    }
}