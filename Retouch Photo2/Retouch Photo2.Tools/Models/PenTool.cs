﻿// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public partial class PenTool : ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        Layerage CurveLayerage => this.SelectionViewModel.CurveLayerage;
        CurveLayer CurveLayer => this.SelectionViewModel.CurveLayer;

        VectorVectorSnap Snap => this.ViewModel.VectorVectorSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Content 
        public ToolType Type => ToolType.Pen;
        public ToolGroupType GroupType => ToolGroupType.Tool;
        public string Title => this.GeometryPage.Title;
        public ControlTemplate Icon => this.GeometryPage.Icon;
        public FrameworkElement Page => this.GeometryPage;
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.GeometryPage.IsOpen; set => this.GeometryPage.IsOpen = value; }

        readonly GeometryPage GeometryPage = new GeometryPage(ToolType.Pen);


        NodeCollectionMode Mode = NodeCollectionMode.None;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.CurveLayer == null)
                this.Mode = NodeCollectionMode.Preview;
            else
                this.Mode = NodeCollectionMode.Add;

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewStart(startingPoint);
                    break;
                case NodeCollectionMode.Add:
                    this.AddStart();
                    break;
            }
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewDelta(canvasPoint);//PreviewNode
                    break;
                case NodeCollectionMode.Add:
                    {
                        if (this.CurveLayer != null)
                        {
                            this.AddDelta(canvasPoint);
                        }
                    }
                    break;
            }
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewComplete(canvasStartingPoint, canvasPoint, isOutNodeDistance);//PreviewNode
                    break;
                case NodeCollectionMode.Add:
                    {
                        if (this.CurveLayer != null)
                        {
                            this.AddComplete(canvasPoint);
                        }
                    }
                    break;
            }

            this.Mode = NodeCollectionMode.None;
        }
        public void Clicke(Vector2 point)
        {
            if (this.CurveLayer == null) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.Add:
                    this.AddComplete(canvasPoint);
                    break;
            }
        }

        public void Cursor(Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            switch (this.Mode)
            {
                case NodeCollectionMode.Preview:
                    this.PreviewDraw(drawingSession);
                    break;
                case NodeCollectionMode.Add:
                    {
                        if (this.CurveLayer != null)
                        {
                            this.AddDraw(drawingSession);
                        }
                    }
                    break;
                default:
                    {
                        if (this.CurveLayer != null)
                        {
                            ILayer layer = this.CurveLayer;

                            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

                            drawingSession.DrawLayerBound(layer, matrix, this.ViewModel.AccentColor);
                            drawingSession.DrawNodeCollection(layer.Nodes, matrix, this.ViewModel.AccentColor);
                        }
                    }
                    break;
            }
        }


        private void CreateLayer(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer);
            this.ViewModel.HistoryPush(history);


            //Transformer
            Transformer transformer = new Transformer(canvasPoint, canvasStartingPoint);

            //Layer
            CurveLayer curveLayer = new CurveLayer(canvasStartingPoint, canvasPoint)
            {
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandCurveStyle,
            };
            Layerage curveLayerage = curveLayer.ToLayerage();
            string id = curveLayerage.Id;
            LayerBase.Instances.Add(id, curveLayer);

            //Mezzanine
            LayerManager.Mezzanine(curveLayerage);


            this.SelectionViewModel.SetModeSingle(curveLayerage);//Selection
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            //Refactoring
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }

    }
}