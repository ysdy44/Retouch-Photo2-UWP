using Retouch_Photo2.Elements;
using Retouch_Photo2.TestApp.ViewModels;
using Retouch_Photo2.Transformers;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s CursorTool .
    /// </summary>
    public partial class CursorTool : Tool
    {
        //Cursor
        /// <summary> <see cref = "CursorTool.Starting" />'s method. </summary>
        public bool CursorStarting(Vector2 point)
        {
            switch (this.ViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    {
                        this.TransformerMode = TransformerMode.None;//TransformerMode
                        bool isSelect = this.SelectLayer(point);
                        if (isSelect) return true;
                        else return false;
                    }


                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        Transformer transformer = this.ViewModel.GetSelectionTransformer();
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        this.TransformerMode = Transformer.ContainsNodeMode(point, transformer, matrix);

                        //Add
                        switch (this.ViewModel.CompositeMode)
                        {
                            case CompositeMode.New:
                            case CompositeMode.Intersect:
                                {
                                    if (this.TransformerMode == TransformerMode.None)
                                    {
                                        bool isSelect = this.SelectLayer(point);

                                        if (isSelect) return true;
                                        else return false;
                                    }
                                }
                                break;
                            case CompositeMode.Add:
                                {
                                    if (this.TransformerMode == TransformerMode.None || this.TransformerMode == TransformerMode.Translation)
                                    {
                                        bool isAdd = this.AddLayer(point, true);
                                        if (isAdd) return true;
                                        else return false;
                                    }
                                }
                                break;
                            case CompositeMode.Subtract:
                                {
                                    if (this.TransformerMode == TransformerMode.None || this.TransformerMode == TransformerMode.Translation)
                                    {
                                        bool isAdd = this.AddLayer(point, false);
                                        if (isAdd) return true;
                                        else return false;
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }

            // if (this.TransformerMode== TransformerMode.None)
            return true;
        }
        /// <summary> <see cref = "CursorTool.Started" />'s method. </summary>
        public bool CursorStarted(Vector2 startingPoint, bool isSetTransformerMode = true)
        {
            if (this.ViewModel.SelectionMode == ListViewSelectionMode.None) return false;

            this.oldTransformer = this.ViewModel.GetSelectionTransformer();

            if (isSetTransformerMode)
            {
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                this.TransformerMode = Transformer.ContainsNodeMode(startingPoint, this.oldTransformer, matrix);
                if (this.TransformerMode == TransformerMode.None) return false;
            }

            //Selection
            this.ViewModel.SelectionSetValue((layer) =>
            {
                layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
            });

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

            return true;
        }
        /// <summary> <see cref = "CursorTool.Delta" />'s method. </summary>
        public bool CursorDelta(Vector2 startingPoint, Vector2 point)
        {
            if (this.ViewModel.SelectionMode == ListViewSelectionMode.None) return false;

            if (this.TransformerMode == TransformerMode.None) return false;

            //Transformer
            this.ViewModel.SelectionTransformer = Transformer.Controller
            (
                this.TransformerMode, startingPoint, point, this.oldTransformer,
                this.ViewModel.CanvasTransformer.GetInverseMatrix(),
                this.ViewModel.KeyIsRatio, this.ViewModel.KeyIsCenter, this.ViewModel.KeyIsStepFrequency
            );
            Matrix3x2 matrix = Transformer.FindHomography(this.oldTransformer, this.ViewModel.SelectionTransformer);

            //Selection
            this.ViewModel.SelectionSetValue((layer) =>
            {
                layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
            });

            this.ViewModel.Invalidate();//Invalidate
            return true;
        }
        /// <summary> <see cref = "CursorTool.Complete" />'s method. </summary>
        public bool CursorComplete(bool isSingleStarted)
        {
            switch (this.ViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    {
                        return false;
                    }

                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {

                        switch (this.ViewModel.CompositeMode)
                        {
                            case CompositeMode.New:
                                {
                                    if (this.TransformerMode == TransformerMode.None)
                                    {
                                        //Selection
                                        this.ViewModel.SelectionSetValue((layer) =>
                                        {
                                            layer.IsChecked = false;
                                        });
                                        this.ViewModel.SetSelectionModeNone();//Selection
                                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

                                        return false;
                                    }
                                }
                                break;
                            case CompositeMode.Add: break;
                            case CompositeMode.Subtract: break;
                            case CompositeMode.Intersect: break;
                        }

                    }
                    break;
            }
                         

            this.TransformerMode = TransformerMode.None;//TransformerMode
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

            return true;
        }
    }
}