using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Library;
using Retouch_Photo2.TestApp.Tools.Controls;
using Retouch_Photo2.TestApp.ViewModels;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Tools.Models
{
    /// <summary> Mode of <see cref="CursorTool"/>. </summary>
    internal enum CursorMode
    {
        /// <summary> The layer as the SelectionLayer. </summary>
        New,
        /// <summary> Add the layer to the SelectionLayers. </summary>
        Add,
        /// <summary> Subtract the layer to the SelectionLayers. </summary>
        Subtract
    }

    /// <summary>
    /// <see cref="Tool"/>'s CursorTool .
    /// </summary>
    public class CursorTool : Tool
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        /// <summary> Scaling around the center. </summary>
        bool IsCenter => this.ViewModel.KeyCtrl;
        /// <summary> Maintain a ratio when scaling. </summary>
        bool IsRatio => this.ViewModel.KeyShift;
        /// <summary> Step Frequency when spinning. </summary>
        bool IsStepFrequency => this.ViewModel.KeyAlt;


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


        public CursorTool()
        {
            base.Type = ToolType.Cursor;
            base.Icon = new CursorControl();
            base.ShowIcon = new CursorControl();
            base.Page = null;
        }


         public   TransformerMode ContainsNodeMode(Vector2 point, Transformer transformer, Matrix3x2 matrix, bool disabledRadian = false)
        {
            if (App.ViewModel.Text.Length > 10)
            {
                App.ViewModel.Text = string.Empty;
            }
            else
            {
                App.ViewModel.Text = App.ViewModel.Text + "O";
            }

            return Transformer.ContainsNodeMode(point, transformer, matrix, disabledRadian);
        }

        //Cursor
        /// <summary> <see cref = "CursorTool.Started" />'s method. </summary>
        public bool CursorStarted(Vector2 startingPoint)
        {
            switch (this.ViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    return false;

                case ListViewSelectionMode.Single:
                    {
                        Transformer transformer = this.ViewModel.SelectionLayer.TransformerMatrix.Destination;

                        //TransformerMode
                        this.startingTransformer = transformer;
                        this.TransformerMode = this.ContainsNodeMode
                        (
                            startingPoint, transformer,
                            this.ViewModel.CanvasTransformer.GetMatrix()
                        );
                        if (this.TransformerMode == TransformerMode.None) return false;

                        this.ViewModel.SelectionLayer.TransformerMatrix.OldDestination = this.ViewModel.SelectionLayer.TransformerMatrix.Destination;
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                        return true;
                    }

                case ListViewSelectionMode.Multiple:
                    {
                        Transformer transformer = this.ViewModel.Transformer;

                        //TransformerMode
                        this.startingTransformer = transformer;
                        this.TransformerMode = this.ContainsNodeMode
                        (
                            startingPoint, transformer,
                            this.ViewModel.CanvasTransformer.GetMatrix()
                        );
                        if (this.TransformerMode == TransformerMode.None) return false;

                        foreach (Layer selectionLayer in this.ViewModel.SelectionLayers)
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

            switch (this.ViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    return false;

                case ListViewSelectionMode.Single:
                    {
                        //Transformer
                        this.ViewModel.Transformer = Transformer.Controller
                        (
                            this.TransformerMode, startingPoint, point, this.startingTransformer,
                            this.ViewModel.CanvasTransformer.GetInverseMatrix(),
                            this.IsRatio, this.IsCenter, this.IsStepFrequency
                        );

                        //Matrix
                        Matrix3x2 matrix = Transformer.FindHomography(this.startingTransformer, this.ViewModel.Transformer);
                        Transformer oldDestination = this.ViewModel.SelectionLayer.TransformerMatrix.OldDestination;
                        this.ViewModel.SelectionLayer.TransformerMatrix.Destination = Transformer.Multiplies(oldDestination, matrix);

                        this.ViewModel.Invalidate();//Invalidate
                        return true;
                    }

                case ListViewSelectionMode.Multiple:
                    {
                        //Transformer
                        this.ViewModel.Transformer = Transformer.Controller
                        (
                            this.TransformerMode, startingPoint, point, this.startingTransformer,
                            this.ViewModel.CanvasTransformer.GetInverseMatrix(),
                            this.IsRatio, this.IsCenter, this.IsStepFrequency
                        );

                        //Matrix
                        Matrix3x2 matrix = Transformer.FindHomography(this.startingTransformer, this.ViewModel.Transformer);
                        foreach (Layer selectionLayer in this.ViewModel.SelectionLayers)
                        {
                            Transformer oldDestination = selectionLayer.TransformerMatrix.OldDestination;
                            selectionLayer.TransformerMatrix.Destination = Transformer.Multiplies(oldDestination, matrix); ;
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

            switch (this.ViewModel.SelectionMode)
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
                        this.ViewModel.SetSelectionMode(this.ViewModel.Layers);//Transformer

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

                        return true;
                    }

                default:
                    return false;
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Select a new layer and sets layer's Checked.
        /// </summary>
        /// <param name="point"> canvas-starting-point </param>
        private void SelectLayer(Vector2 point)
        {
            Vector2 canvasStartingPoint = Vector2.Transform(point, this.ViewModel.CanvasTransformer.GetInverseMatrix());

            bool selected = false;
            foreach (Layer layer in this.ViewModel.Layers)
            {
                if (selected == false)
                {
                    if (layer.IsVisual)
                    {
                        Transformer layerTransformer = layer.TransformerMatrix.Destination;
                        bool layerInQuadrangle = Transformer.InQuadrangle(canvasStartingPoint, layerTransformer);

                        if (layerInQuadrangle)
                        {
                            selected = true;

                            layer.IsChecked = true;
                            this.TransformerMode = TransformerMode.Translation;//TransformerMode
                            this.ViewModel.SetSelectionModeSingle(layer);//Transformer

                            continue;
                        }
                    }
                }

                layer.IsChecked = false;
                continue;
            }

            this.ViewModel.Invalidate();//Invalidate
        }

        /// <summary>
        /// Add the layer to the SelectionLayers. 
        /// </summary>
        /// <param name="point"> canvas-starting-point </param>
        /// <param name="isAdd"> <see cref = "CursorMode.Add" /> or <see cref = "CursorMode.Subtract" /> </param>
        private void AddLayer(Vector2 point, bool isAdd = true)
        {
            foreach (Layer layer in this.ViewModel.Layers)
            {
                if (layer.IsVisual)
                {
                    Transformer layerTransformer = layer.TransformerMatrix.Destination;
                    bool layerInQuadrangle = Transformer.InQuadrangle(point, layerTransformer);

                    if (layerInQuadrangle)
                    {
                        layer.IsChecked = isAdd;
                        break;
                    }
                }
            }

            this.TransformerMode = TransformerMode.None;//TransformerMode
            this.ViewModel.SetSelectionMode(this.ViewModel.Layers);//Transformer

            this.ViewModel.Invalidate();//Invalidate
        }

        /// <summary>
        /// <see cref = "CursorTool.Starting" />'s method.
        /// </summary>
        /// <param name="point"> Input point. </param>
        /// <param name="transformer"> transformer </param>
        private void SingleAndMultipleStarting(Vector2 point, Transformer transformer)
        {
            switch (this.CursorMode)
            {
                case CursorMode.New:
                    {
                        //TransformerMode
                        this.TransformerMode = this.ContainsNodeMode
                        (
                            point, transformer,
                            this.ViewModel.CanvasTransformer.GetMatrix()
                        );

                        if (this.TransformerMode == TransformerMode.None)
                        {
                            this.SelectLayer(point);
                        }
                    }
                    break;

                case CursorMode.Add:
                    this.AddLayer(point, true);
                    break;

                case CursorMode.Subtract:
                    this.AddLayer(point, false);
                    break;
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////


        //@Override
        public override void Starting(Vector2 point)
        {
            switch (this.ViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    {
                        this.TransformerMode = TransformerMode.None;//TransformerMode
                        this.SelectLayer(point);
                    }
                    break;
                case ListViewSelectionMode.Single:
                    this.SingleAndMultipleStarting(point, this.ViewModel.SelectionLayer.TransformerMatrix.Destination);
                    break;
                case ListViewSelectionMode.Multiple:
                    this.SingleAndMultipleStarting(point, this.ViewModel.Transformer);
                    break;
            }
        }
        public override void Started(Vector2 startingPoint, Vector2 point) => this.CursorStarted(startingPoint);
        public override void Delta(Vector2 startingPoint, Vector2 point) => this.CursorDelta(startingPoint, point);
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) => this.CursorComplete();

        public override void Draw(CanvasDrawingSession ds) { }

    }
}