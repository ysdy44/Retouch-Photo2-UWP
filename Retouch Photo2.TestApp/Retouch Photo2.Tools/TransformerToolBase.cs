using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Transformers;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ToolBase"/>'s TransformerToolBase .
    /// </summary>
    public partial class TransformerToolBase : ToolBase
    {
        //@ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;
        SelectionViewModel SelectionViewModel => Retouch_Photo2.TestApp.App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => Retouch_Photo2.TestApp.App.KeyboardViewModel;


        //Transformer
        Transformer oldTransformer;
        TransformerMode TransformerMode;


        //@Override
        public override bool Starting(Vector2 point)
        {
            switch (this.SelectionViewModel.Mode)
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
                        Transformer transformer = this.SelectionViewModel.GetTransformer();
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        this.TransformerMode = Transformer.ContainsNodeMode(point, transformer, matrix);

                        //Add
                        switch (this.KeyboardViewModel.CompositeMode)
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

        public override bool Started(Vector2 startingPoint, bool isSetTransformerMode = true)
        {
            if (this.SelectionViewModel.Mode == ListViewSelectionMode.None) return false;

            this.oldTransformer = this.SelectionViewModel.GetTransformer();

            if (isSetTransformerMode)
            {
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                this.TransformerMode = Transformer.ContainsNodeMode(startingPoint, this.oldTransformer, matrix);
                if (this.TransformerMode == TransformerMode.None) return false;
            }

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
            }, true);

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

            return true;
        }

        public override bool Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.SelectionViewModel.Mode == ListViewSelectionMode.None) return false;

            if (this.TransformerMode == TransformerMode.None) return false;

            //Transformer
            this.SelectionViewModel.Transformer = Transformer.Controller
            (
                this.TransformerMode, startingPoint, point, this.oldTransformer,
                this.ViewModel.CanvasTransformer.GetInverseMatrix(),
                this.KeyboardViewModel.IsRatio, this.KeyboardViewModel.IsCenter, this.KeyboardViewModel.IsStepFrequency
            );
            Matrix3x2 matrix = Transformer.FindHomography(this.oldTransformer, this.SelectionViewModel.Transformer);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, matrix);
            }, true);

            this.ViewModel.Invalidate();//Invalidate
            return true;
        }

        public override bool Complete(bool isSingleStarted)
        {
            switch (this.SelectionViewModel.Mode)
            {
                case ListViewSelectionMode.None:
                    {
                        return false;
                    }

                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {

                        switch (this.KeyboardViewModel.CompositeMode)
                        {
                            case CompositeMode.New:
                                {
                                    if (this.TransformerMode == TransformerMode.None)
                                    {
                                        //Selection
                                        this.SelectionViewModel.SetValue((layer) =>
                                        {
                                            layer.IsChecked = false;
                                        });
                                        this.SelectionViewModel.SetModeNone();//Selection
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

               

        /// <summary>
        /// Select a layer from a point,
        /// make it to <see cref = "Selection.Layer" />
        /// and make the <see cref = "TransformerMode" /> to move,
        /// find the layer that makes it unique, and 
        /// </summary>
        /// <param name="point"> point </param>
        /// <returns> Return **false** if you do not select to any layer. </returns>
        private bool SelectLayer(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, matrix);

            Layer selectedLayer = this.ViewModel.Layers.FirstOrDefault((layer) =>
            {
                if (layer.Visibility == Visibility.Visible)
                {
                    bool layerInQuadrangle = Transformer.InQuadrangle(canvasPoint, layer.TransformerMatrix.Destination);
                    if (layerInQuadrangle)
                    {
                        selectedLayer = layer;
                        return true;
                    }
                }
                return false;
            });
            if (selectedLayer == null) return false;


            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.IsChecked = false;
            });
            this.TransformerMode = TransformerMode.Translation;//TransformerMode


            //Selection
            selectedLayer.IsChecked = true;
            this.SelectionViewModel.SetModeSingle(selectedLayer);//Transformer
            this.ViewModel.Invalidate();//Invalidate
            return true;
        }

        /// <summary>
        /// Add the layer to the layers. 
        /// </summary>
        /// <param name="point"> point </param>
        /// <param name="isAdd"> <see cref = "CompositeMode.Add" /> or <see cref = "CompositeMode.Subtract" /> </param>
        /// <returns> Return **false** if you do not select to any layer. </returns>
        private bool AddLayer(Vector2 point, bool isAdd = true)
        {
            Vector2 canvasPoint = Vector2.Transform(point, this.ViewModel.CanvasTransformer.GetInverseMatrix());

            Layer selectedLayer = this.ViewModel.Layers.FirstOrDefault((layer) =>
            {
                if (layer.Visibility == Visibility.Visible)
                {
                    Transformer layerTransformer = layer.TransformerMatrix.Destination;
                    bool layerInQuadrangle = Transformer.InQuadrangle(canvasPoint, layerTransformer);
                    if (layerInQuadrangle) return true;
                }

                return false;
            });

            if (selectedLayer == null) return false;
            selectedLayer.IsChecked = isAdd;

            this.TransformerMode = TransformerMode.None;//TransformerMode
            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Transformer
            this.ViewModel.Invalidate();//Invalidate

            return true;
        }

    }
}