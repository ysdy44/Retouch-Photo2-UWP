using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Library;
using Retouch_Photo2.TestApp.Tools.Controls;
using Retouch_Photo2.TestApp.ViewModels;
using System.Numerics;
using Retouch_Photo2.TestApp.Tools.Pages;

using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Tools.Models
{
    /// <summary> Mode of <see cref="CursorTool"/>. </summary>
    internal enum CursorMode
    {
        /// <summary> The layer as the layer. </summary>
        New,
        /// <summary> Add the layer to the layers. </summary>
        Add,
        /// <summary> Subtract the layer to the layers. </summary>
        Subtract
    }

    /// <summary>
    /// <see cref="Tool"/>'s CursorTool .
    /// </summary>
    public class CursorTool : Tool
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;
               
        Transformer startingTransformer;
        TransformerMode TransformerMode;
        CursorMode CursorMode
        {
            get
            {
                if (this.ViewModel.KeyShift)
                    return CursorMode.Add;

                if (this.ViewModel.KeyCtrl)
                    return CursorMode.Subtract;

                return CursorMode.New;
            }
        }
        
        //@Construct
        public CursorTool()
        {
            base.Type = ToolType.Cursor;
            base.Icon = new CursorControl();
            base.ShowIcon = new CursorControl();
            base.Page = new CursorPage();
        }


        //Cursor
        /// <summary> <see cref = "CursorTool.Starting" />'s method. </summary>
        public bool CursorStarting(Vector2 point)
        {
            //SelectionMode
            switch (this.ViewModel.Selection.Mode)
            {
                case ListViewSelectionMode.None:
                    {
                        this.TransformerMode = TransformerMode.None;//TransformerMode
                        bool isSelect = this.SelectLayer(point);
                        if (isSelect) return true;
                    }
                    break;
                case ListViewSelectionMode.Single:
                    {
                        Transformer transformer = this.ViewModel.Selection.Layer.TransformerMatrix.Destination;
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        this.TransformerMode = Transformer.ContainsNodeMode(point, transformer, matrix);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        Transformer transformer = this.ViewModel.Selection.Transformer;
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        this.TransformerMode = Transformer.ContainsNodeMode(point, transformer, matrix);
                    }
                    break;
            }



            //CursorMode
            switch (this.CursorMode)
            {
                case CursorMode.New:
                    {
                        if (this.TransformerMode == TransformerMode.None)
                        {
                            bool isSelect = this.SelectLayer(point);
                            if (isSelect) return true;
                        }
                    }
                    break;
                case CursorMode.Add:
                    {
                        if (this.TransformerMode == TransformerMode.None || this.TransformerMode == TransformerMode.Translation)
                        {
                            bool isAdd = this.AddLayer(point, true);
                            if (isAdd) return true;
                        }
                    }
                    break;
                case CursorMode.Subtract:
                    {
                        if (this.TransformerMode == TransformerMode.None || this.TransformerMode == TransformerMode.Translation)
                        {
                            bool isAdd = this.AddLayer(point, false);
                            if (isAdd) return true;
                        }
                    }
                    break;
            }



            //TransformerMode
            if (this.TransformerMode == TransformerMode.None)
            {
                this.ViewModel.SelectionNone();//Transformer
                this.ViewModel.Invalidate();//Invalidate
                return true;
            }

            return false;
        }
        /// <summary> <see cref = "CursorTool.Started" />'s method. </summary>
        public bool CursorStarted(Vector2 startingPoint, bool isSetTransformerMode= true)
        {
            switch (this.ViewModel.Selection.Mode)
            {
                case ListViewSelectionMode.None:
                    return false;

                case ListViewSelectionMode.Single:
                    {
                        Transformer transformer = this.ViewModel.Selection.Layer.TransformerMatrix.Destination;
                        this.startingTransformer = transformer;

                        if (isSetTransformerMode)
                        {
                            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                            this.TransformerMode = Transformer.ContainsNodeMode(startingPoint, transformer, matrix);
                            if (this.TransformerMode == TransformerMode.None) return false;
                        }

                        this.ViewModel.Selection.Layer.TransformerMatrix.OldDestination = this.ViewModel.Selection.Layer.TransformerMatrix.Destination;
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                        return true;
                    }

                case ListViewSelectionMode.Multiple:
                    {
                        Transformer transformer = this.ViewModel.Selection.Transformer;
                        this.startingTransformer = transformer;

                        if (isSetTransformerMode)
                        {
                            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                            this.TransformerMode = Transformer.ContainsNodeMode(startingPoint, transformer, matrix);
                            if (this.TransformerMode == TransformerMode.None) return false;
                        }

                        foreach (Layer selectionLayer in this.ViewModel.Selection.Layers)
                        {
                            Transformer destination = selectionLayer.TransformerMatrix.Destination;
                            selectionLayer.TransformerMatrix.OldDestination = destination;
                        }
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                        return true;
                    }

                default:
                    return false;
            }
        }
        /// <summary> <see cref = "CursorTool.Delta" />'s method. </summary>
        public bool CursorDelta(Vector2 startingPoint, Vector2 point)
        {
            if (this.TransformerMode == TransformerMode.None) return false;

            switch (this.ViewModel.Selection.Mode)
            {
                case ListViewSelectionMode.None:
                    return false;

                case ListViewSelectionMode.Single:
                    {
                        //Transformer
                        this.ViewModel.Selection.Transformer = Transformer.Controller
                        (
                            this.TransformerMode, startingPoint, point, this.startingTransformer,
                            this.ViewModel.CanvasTransformer.GetInverseMatrix(),
                            this.ViewModel.KeyIsRatio, this.ViewModel.KeyIsCenter, this.ViewModel.KeyIsStepFrequency
                        );

                        //Matrix
                        Matrix3x2 matrix = Transformer.FindHomography(this.startingTransformer, this.ViewModel.Selection.Transformer);
                        Transformer oldDestination = this.ViewModel.Selection.Layer.TransformerMatrix.OldDestination;
                        this.ViewModel.Selection.Layer.TransformerMatrix.Destination = Transformer.Multiplies(oldDestination, matrix);

                        this.ViewModel.Invalidate();//Invalidate
                        return true;
                    }

                case ListViewSelectionMode.Multiple:
                    {
                        //Transformer
                        this.ViewModel.Selection.Transformer = Transformer.Controller
                        (
                            this.TransformerMode, startingPoint, point, this.startingTransformer,
                            this.ViewModel.CanvasTransformer.GetInverseMatrix(),
                            this.ViewModel.KeyIsRatio, this.ViewModel.KeyIsCenter, this.ViewModel.KeyIsStepFrequency
                        );

                        //Matrix
                        Matrix3x2 matrix = Transformer.FindHomography(this.startingTransformer, this.ViewModel.Selection.Transformer);
                        foreach (Layer selectionLayer in this.ViewModel.Selection.Layers)
                        {
                            Transformer oldDestination = selectionLayer.TransformerMatrix.OldDestination;
                            selectionLayer.TransformerMatrix.Destination = Transformer.Multiplies(oldDestination, matrix);
                        }

                        this.ViewModel.Invalidate();//Invalidate
                        return true;
                    }

                default:
                    return false;
            }
        }
        /// <summary> <see cref = "CursorTool.Complete" />'s method. </summary>
        public bool CursorComplete()
        {
            if (this.TransformerMode == TransformerMode.None) return false;

            this.TransformerMode = TransformerMode.None;//TransformerMode

            switch (this.ViewModel.Selection.Mode)
            {
                case ListViewSelectionMode.None:
                    return false;

                case ListViewSelectionMode.Single:
                    {
                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

                        return true;
                    }

                case ListViewSelectionMode.Multiple:
                    {
                        this.ViewModel.SetSelection();//Transformer

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

                        return true;
                    }

                default:
                    return false;
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Select a new layer and sets the layer's Checked.
        /// </summary>
        /// <param name="point"> point </param>
        /// <returns> Return **false** if you do not select to any layer. </returns>
        private bool SelectLayer(Vector2 point)
        {
            Vector2 canvasStartingPoint = Vector2.Transform(point, this.ViewModel.CanvasTransformer.GetInverseMatrix());
            Layer selectedLayer = this.ViewModel.Layers.LastOrDefault((layer) =>
            {
                layer.IsChecked = false;

                if (layer.IsVisual)
                {
                    Transformer layerTransformer = layer.TransformerMatrix.Destination;
                    bool layerInQuadrangle = Transformer.InQuadrangle(canvasStartingPoint, layerTransformer);
                    if (layerInQuadrangle) return true;
                }

                return false;
            });

            if (selectedLayer == null) return false;
            selectedLayer.IsChecked = true;

            this.TransformerMode = TransformerMode.Translation;//TransformerMode
            this.ViewModel.SelectionSingle(selectedLayer);//Transformer
            this.ViewModel.Invalidate();//Invalidate

            return true;
        }

        /// <summary>
        /// Add the layer to the layers. 
        /// </summary>
        /// <param name="point"> point </param>
        /// <param name="isAdd"> <see cref = "CursorMode.Add" /> or <see cref = "CursorMode.Subtract" /> </param>
        /// <returns> Return **false** if you do not select to any layer. </returns>
        private bool AddLayer(Vector2 point, bool isAdd = true)
        {
            Vector2 canvasStartingPoint = Vector2.Transform(point, this.ViewModel.CanvasTransformer.GetInverseMatrix());
            Layer selectedLayer = this.ViewModel.Layers.FirstOrDefault((layer) =>
            {
                if (layer.IsVisual)
                {
                    Transformer layerTransformer = layer.TransformerMatrix.Destination;
                    bool layerInQuadrangle = Transformer.InQuadrangle(canvasStartingPoint, layerTransformer);
                    if (layerInQuadrangle) return true;
                }

                return false;
            });

            if (selectedLayer == null) return false;
            selectedLayer.IsChecked = isAdd;

            this.TransformerMode = TransformerMode.None;//TransformerMode
            this.ViewModel.SetSelection();//Transformer
            this.ViewModel.Invalidate();//Invalidate

            return true;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////


        //@Override
        public override void Starting(Vector2 point) => this.CursorStarting(point);
        public override void Started(Vector2 startingPoint, Vector2 point) => this.CursorStarted(startingPoint, false);
        public override void Delta(Vector2 startingPoint, Vector2 point) => this.CursorDelta(startingPoint, point);
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) => this.CursorComplete();

        public override void Draw(CanvasDrawingSession ds) { }

    }
}