using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsSquare => this.SettingViewModel.IsSquare;

        Layerage MezzanineLayerage;


        /// <summary>
        /// Occurs when the operation begins. 
        /// </summary>
        /// <param name="createLayer">
        /// <summary>
        /// Function of how to crate a layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <returns> The created layer. </returns>
        /// </param>
        /// <param name="startingPoint"> The starting pointer. </param>
        /// <param name="point"> The pointer. </param>
        public void Started(Func<CanvasDevice, Transformer, ILayer> createLayer, Vector2 startingPoint, Vector2 point)
        {
            if (this.TransformerTool.Started(startingPoint, point)) return;//TransformerTool

            //Transformer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            //Snap         
            if (this.IsSnap) this.ViewModel.VectorBorderSnapInitiate(this.SelectionViewModel.GetFirstSelectedLayerage());

            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory("Add layer", this.ViewModel.LayerageCollection);
            this.ViewModel.HistoryPush(history);

            //Selection
            Transformer transformer = new Transformer(canvasStartingPoint, canvasPoint, this.IsCenter, this.IsSquare);

            //Mezzanine
            ILayer layer = createLayer(this.ViewModel.CanvasDevice, transformer);
            Layerage layerage = layer.ToLayerage();
            LayerBase.Instances.Add(layer);

            //Mezzanine
            this.MezzanineLayerage = layerage;
            LayerageCollection.Mezzanine(this.ViewModel.LayerageCollection, this.MezzanineLayerage);

            //History
            this.ViewModel.MethodSelectedNone();

            //Text
            this.ViewModel.SetTipTextWidthHeight(transformer);
            this.ViewModel.TipTextVisibility = Visibility.Visible;
     
            //Selection
            this.Transformer = transformer;
            this.SelectionViewModel.SetModeExtended();
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
                ILayer mezzanineLayer = this.MezzanineLayerage.Self;
                mezzanineLayer.Transform = new Transform(transformer);
                mezzanineLayer.Style.DeliverBrushPoints(transformer);
                //Refactoring
                mezzanineLayer.IsRefactoringRender = true;
                this.MezzanineLayerage.RefactoringParentsRender();

                this.ViewModel.SetTipTextWidthHeight(transformer);//Text
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
                    ILayer mezzanineLayer = this.MezzanineLayerage.Self;
                    mezzanineLayer.Transform = new Transform(transformer);
                    mezzanineLayer.IsSelected = true;
                    //Refactoring
                    mezzanineLayer.IsRefactoringRender = true;
                    mezzanineLayer.IsRefactoringIconRender = true;

                    //Selection
                    this.SelectionViewModel.SetModeSingle(this.MezzanineLayerage);
                    LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                    LayerageCollection.ArrangeLayersBackground(this.ViewModel.LayerageCollection);
                    
                    this.MezzanineLayerage = null;
                    this.ViewModel.TipTextVisibility = Visibility.Collapsed;//Text
                    this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                }
                /*
                else
                {
                    LayerageCollection.RemoveMezzanineLayer(this.ViewModel.LayerageCollection, this.MezzanineLayer);//Mezzanine

                    //Selection
                    this.SelectionViewModel.SetModeNone();
                    LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                    LayerageCollection.ArrangeLayersBackgroundLayerageCollection(this.ViewModel.LayerageCollection);
                }
                 */
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