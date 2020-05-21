using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="IMoveTool"/>'s MoveTool.
    /// </summary>
    public partial class MoveTool : IMoveTool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        
        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;

        MarqueeCompositeMode MarqueeCompositeMode => this.SettingViewModel.CompositeMode;
        BorderBorderSnap Snap => this.ViewModel.BorderBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        Transformer StartingTransformer;
            
        public bool Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            bool isMove = this.GetIsClickLayer(canvasStartingPoint);
            if (isMove == false) return false;

            //Snap
            if (this.IsSnap) this.ViewModel.BorderBorderSnapStarted(this.SelectionViewModel.GetFirstLayer());

            //Selection
            this.StartingTransformer = this.Transformer;
            if (this.IsSnap) this.Snap.StartingSource = new TransformerBorder(this.Transformer);
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.CacheTransform();
            });
            
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            return true;
        }

        public bool Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;
            
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Vector2 canvasMove = canvasPoint - canvasStartingPoint;

            //Snap
            if (this.IsSnap) canvasMove = this.Snap.Snap(canvasMove);

            //Selection
            Transformer transformer = Transformer.Add(this.StartingTransformer, canvasMove);
            if (this.IsSnap) this.Snap.Source = new TransformerBorder(transformer);
            this.SelectionViewModel.Transformer = transformer;
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.TransformAdd(canvasMove);
            });

            this.ViewModel.Invalidate();//Invalidate
            return true;
        }
        public bool Complete(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Vector2 canvasMove = canvasPoint - canvasStartingPoint;

            //Snap
            if (this.IsSnap)
            {
                canvasMove = this.Snap.Snap(canvasMove);
                this.Snap.Default();
            }

            //History
            IHistoryBase history = new IHistoryBase("Move");

            //Selection
            Transformer transformer = Transformer.Add(this.StartingTransformer, canvasMove);
            if (this.IsSnap) this.Snap.Source = new TransformerBorder(transformer);
            this.SelectionViewModel.Transformer = transformer;
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.TransformAdd(canvasMove);

                //History
                var previous = layer.Transform.StartingDestination;
                int index = layer.Control.Index;
                history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                Transform.Destination = previous);
            });

            //History
            this.ViewModel.Push(history);

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            return true;
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.Mode == ListViewSelectionMode.None) return;

            //Transformer
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            drawingSession.DrawBoundNodes(this.Transformer, matrix, this.ViewModel.AccentColor, this.SelectionViewModel.DisabledRadian);

            //Snapping
            if (this.IsSnap) this.Snap.Draw(drawingSession, matrix);
        }


        private bool GetIsClickLayer(Vector2 canvasStartingPoint)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.None: break;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        bool isFillContains = this.Transformer.FillContainsPoint(canvasStartingPoint);
                        if (isFillContains) return true;
                    }
                    break;
            }

            //SelectedLayer
            ILayer selectedLayer = this.GetSelectedLayer(canvasStartingPoint);

            if (selectedLayer == null)
            {
                this.ClickeNone();
                return false;
            }
            else
            {
                this.ClickeNew(selectedLayer);
                return true;
            }
        }

    }
}