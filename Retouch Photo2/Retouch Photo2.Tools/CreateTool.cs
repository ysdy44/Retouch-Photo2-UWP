using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ICreateTool"/>'s CreateTool.
    /// </summary>
    public class CreateTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.ViewModel.IsSnap;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsSquare => this.SettingViewModel.IsSquare;

        ILayer MezzanineLayer = null;


        /// <summary>
        /// Occurs when the operation begins. 
        /// </summary>
        /// <param name="createLayer">
        /// <summary>
        /// Function of how to crate a layer.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <returns> The created layer. </returns>
        /// </param>
        /// <param name="startingPoint"> The starting pointer. </param>
        /// <param name="point"> The pointer. </param>
        public void Started(Func<Transformer, ILayer> createLayer, Vector2 startingPoint, Vector2 point)
        {
            if (this.TransformerTool.Started(startingPoint, point)) return;//TransformerTool

            //Transformer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            //Snap         
            if (this.IsSnap) this.ViewModel.VectorBorderSnapStarted(this.SelectionViewModel.GetFirstLayer());

            //Selection
            Transformer transformer = new Transformer(canvasStartingPoint, canvasPoint, this.IsCenter, this.IsSquare);
            this.Transformer = transformer;
            this.SelectionViewModel.SetModeExtended();//Selection

            //Mezzanine
            this.MezzanineLayer = createLayer(transformer);
            LayerCollection.Mezzanine(this.ViewModel.Layers, this.MezzanineLayer);

            //Text
            this.ViewModel.SetTextWidthHeight(transformer);
            this.ViewModel.TextVisibility = Visibility.Visible;
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.Extended)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                //Snap
                if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);

                //Selection
                Transformer transformer = new Transformer(canvasStartingPoint, canvasPoint, this.IsCenter, this.IsSquare);
                this.Transformer = transformer;

                //Mezzanine
                this.MezzanineLayer.Transform = new Transform(transformer);
                this.MezzanineLayer.Style.DeliverBrushPoints(transformer);

                this.ViewModel.SetTextWidthHeight(transformer);//Text
                this.ViewModel.Invalidate();//Invalidate
            }

            if (this.TransformerTool.Delta(startingPoint, point)) return;//TransformerTool
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            if (this.Mode == ListViewSelectionMode.Extended)
            {
                if (isOutNodeDistance)
                {
                    Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                    Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                    Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                    //Snap
                    if (this.IsSnap)
                    {
                        canvasPoint = this.Snap.Snap(canvasPoint);
                        this.Snap.Default();
                    }

                    //Transformer
                    Transformer transformer = new Transformer(canvasStartingPoint, canvasPoint, this.IsCenter, this.IsSquare);
                    this.Transformer = transformer;

                    //Mezzanine
                    this.SelectionViewModel.SetModeSingle(this.MezzanineLayer);//Selection
                    this.MezzanineLayer.Transform = new Transform(transformer);
                    this.MezzanineLayer.IsSelected = true;
                    this.MezzanineLayer = null;
                }
                else LayerCollection.RemoveMezzanineLayer(this.ViewModel.Layers, this.MezzanineLayer);//Mezzanine

                LayerCollection.ArrangeLayersControls(this.ViewModel.Layers);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.ViewModel.TextVisibility = Visibility.Collapsed;//Text
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            }

            if (this.TransformerTool.Complete(startingPoint, point)) return;//TransformerTool
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    this.TransformerTool.Draw(drawingSession); //TransformerTool
                    break;
                case ListViewSelectionMode.Extended:
                    {
                        //Transformer
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        drawingSession.DrawBound(this.Transformer, matrix, this.ViewModel.AccentColor);

                        //Snapping
                        if (this.IsSnap)
                        {
                            this.Snap.Draw(drawingSession, matrix);
                            this.Snap.DrawNode2(drawingSession, matrix);
                        }
                    }
                    break;
            }           
        }

    }
}