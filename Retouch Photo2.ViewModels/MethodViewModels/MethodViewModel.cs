// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Styles;
using System;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some methods of the application
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        #region ILayer<T>


        /// <summary>
        /// Change T type for ILayer, save history, invalidate canvas.
        /// </summary>
        /// <typeparam name="T"> The T type property. </typeparam>
        /// <param name="set"> The sets of T. </param>
        /// <param name="historyTitle"> The history title. </param>
        /// <param name="getHistory"> The gets of history T. </param>
        /// <param name="setHistory"> The sets of history T. </param>
        public void ILayerChanged<T>
        (
            Action<ILayer> set,
            string historyTitle, Func<ILayer, T> getHistory, Action<ILayer, T> setHistory
        )
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = getHistory(layer);
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    setHistory(layer, previous);
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                set(layer);
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }


        public void ILayerChangeStarted(Action<ILayer> cache)
        {
            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;
                cache(layer);
            });

            this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }

        public void ILayerChangeDelta(Action<ILayer> set)
        {
            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                //Refactoring
                layer.IsRefactoringRender = true;
                layerage.RefactoringParentsRender();
                set(layer);
            });

            this.Invalidate();//Invalidate
        }

        public void ILayerChangeCompleted<T>
        (
            Action<ILayer> set,

            string historyTitle,
            Func<ILayer, T> getHistory,
            Action<ILayer, T> setHistory
        )
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = getHistory(layer);
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    setHistory(layer, previous);
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                set(layer);
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }


        #endregion
        
        

        #region TLayer<T>


        /// <summary>
        /// Change T type for TLayer, save history, invalidate canvas.
        /// </summary>
        /// <typeparam name="T"> The T type property. </typeparam>
        /// <typeparam name="TLayer"> The T type layer. </typeparam>
        /// <param name="layerType"> The layer-type. </param>
        /// <param name="set"> The sets of T. </param>
        /// <param name="historyTitle"> The history title. </param>
        /// <param name="getHistory"> The gets of history T. </param>
        /// <param name="setHistory"> The sets of history T. </param>
        public void TLayerChanged<T, TLayer>
        (
            LayerType layerType,
            Action<TLayer> set,
            string historyTitle, Func<TLayer, T> getHistory, Action<TLayer, T> setHistory
        )
        where TLayer : ILayer
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == layerType)
                {
                    TLayer tLayer = (TLayer)layer;

                    var previous = getHistory(tLayer);
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        tLayer.IsRefactoringRender = true;
                        tLayer.IsRefactoringIconRender = true;
                        setHistory(tLayer, previous);
                    };

                    //Refactoring
                    tLayer.IsRefactoringRender = true;
                    tLayer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    set(tLayer);
                }
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }


        public void TLayerChangeStarted<TLayer>(LayerType layerType, Action<TLayer> cache)
            where TLayer : ILayer
        {
            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == layerType)
                {
                    TLayer tLayer = (TLayer)layer;
                    cache(tLayer);
                }
            });

            this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }

        public void TLayerChangeDelta<TLayer>(LayerType layerType, Action<TLayer> set)
            where TLayer : ILayer
        {
            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == layerType)
                {
                    TLayer tLayer = (TLayer)layer;

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    set(tLayer);
                }
            });

            this.Invalidate();//Invalidate
        }

        public void TLayerChangeCompleted<T, TLayer>
        (
            LayerType layerType,
            Action<TLayer> set,

            string historyTitle,
            Func<TLayer, T> getHistory,
            Action<TLayer, T> setHistory
        )
         where TLayer : ILayer
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == layerType)
                {
                    TLayer tLayer = (TLayer)layer;

                    var previous = getHistory(tLayer);
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        setHistory(tLayer, previous);
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    set(tLayer);
                }
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }


        #endregion
        


        #region Effect<T>


        /// <summary>
        /// Change T type for Effect, save history, invalidate canvas.
        /// </summary>
        /// <typeparam name="T"> The T type property. </typeparam>
        /// <param name="set"> The sets of T. </param>
        /// <param name="historyTitle"> The history title. </param>
        /// <param name="getHistory"> The gets of history T. </param>
        /// <param name="setHistory"> The sets of history T. </param>
        public void EffectChanged<T>
        (
            Action<Effect> set,
            string historyTitle, Func<Effect, T> getHistory, Action<Effect, T> setHistory
        )
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = getHistory(layer.Effect);
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    setHistory(layer.Effect, previous);
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                set(layer.Effect);
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }



        public void EffectChangeStarted(Action<Effect> cache)
        {
            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;
                cache(layer.Effect);
            });

            this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }

        public void EffectChangeDelta(Action<Effect> set)
        {
            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                //Refactoring
                layer.IsRefactoringRender = true;
                layerage.RefactoringParentsRender();
                set(layer.Effect);
            });

            this.Invalidate();//Invalidate
        }

        public void EffectChangeCompleted<T>
        (
            Action<Effect> set,

            string historyTitle,
            Func<Effect, T> getHistory,
            Action<Effect, T> setHistory
        )
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = getHistory(layer.Effect);
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    setHistory(layer.Effect, previous);
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                set(layer.Effect);
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }



        #endregion



        #region TAdjustment<T>


        /// <summary>
        /// Change T type for TAdjustment, save history, invalidate canvas.
        /// </summary>
        /// <typeparam name="T"> The T type property. </typeparam>
        /// <typeparam name="TAdjustment"> The T type layer. </typeparam>
        /// <param name="index"> The adjustment index. </param>
        /// <param name="set"> The sets of T. </param>
        /// <param name="historyTitle"> The history title. </param>
        /// <param name="getHistory"> The gets of history T. </param>
        /// <param name="setHistory"> The sets of history T. </param>
        public void TAdjustmentChanged<T,TAdjustment>
        (
            int index,
            Action<TAdjustment> set,
            string historyTitle, Func<TAdjustment, T> getHistory, Action<TAdjustment, T> setHistory
        )
        where TAdjustment : IAdjustment
        {
            if (this.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (index < 0) return;
                if (index > layer.Filter.Adjustments.Count - 1) return;
                if (layer.Filter.Adjustments[index] is TAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previous1 = getHistory(adjustment);
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is TAdjustment previousAdjustment)
                        {
                            //Refactoring
                            layer.IsRefactoringTransformer = true;
                            layer.IsRefactoringRender = true;
                            setHistory(previousAdjustment, previous1);
                        }
                    };

                    //Refactoring
                    layer.IsRefactoringTransformer = true;
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    set(adjustment);

                    //History
                    this.HistoryPush(history);

                    this.Invalidate(InvalidateMode.HD);//Invalidate
                }
            }
        }


        public void TAdjustmentChangeStarted<TAdjustment>(int index, Action<TAdjustment> cache)
            where TAdjustment : IAdjustment
        {
            if (this.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (layer.Filter.Adjustments[index] is TAdjustment adjustment)
                {
                    cache(adjustment);
                    this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                }
            }
        }

        public void TAdjustmentChangeDelta<TAdjustment>(int index, Action<TAdjustment> set)
        {
            if (this.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (index < 0) return;
                if (index > layer.Filter.Adjustments.Count - 1) return;
                if (layer.Filter.Adjustments[index] is TAdjustment adjustment)
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    set(adjustment);

                    this.Invalidate();//Invalidate
                }
            }
        }

        public void TAdjustmentChangeCompleted<T, TAdjustment>
        (
            int index,
            Action<TAdjustment> set,

            string historyTitle,
            Func<TAdjustment, T> getHistory,
            Action<TAdjustment, T> setHistory
        )
            where TAdjustment : IAdjustment
        {
            if (this.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (index < 0) return;
                if (index > layer.Filter.Adjustments.Count - 1) return;
                if (layer.Filter.Adjustments[index] is TAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previous1 = getHistory(adjustment);
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is TAdjustment previousAdjustment)
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            setHistory(previousAdjustment, previous1);
                        }
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    set(adjustment);

                    //History
                    this.HistoryPush(history);

                    this.Invalidate(InvalidateMode.HD);//Invalidate
                }
            }
        }


        #endregion
        


        #region Style<T>


        /// <summary>
        /// Change T type for Style, save history, invalidate canvas.
        /// </summary>
        /// <typeparam name="T"> The T type property. </typeparam>
        /// <param name="set"> The sets of T. </param>
        /// <param name="historyTitle"> The history title. </param>
        /// <param name="getHistory"> The gets of history T. </param>
        /// <param name="setHistory"> The sets of history T. </param>
        public void StyleChanged<T>
        (
            Action<IStyle, Transformer> set,
            string historyTitle, Func<IStyle, T> getHistory, Action<IStyle, T> setHistory
        )
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

            //Selection
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = getHistory(layer.Style);
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    setHistory(layer.Style, previous);
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                set(layer.Style, layer.Transform.Transformer);
                this.StandStyleLayer = layer;
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }


        public void StyleChangeStarted(Action<IStyle> cache)
        {
            //Selection
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;
                cache(layer.Style);
            });

            this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }

        public void StyleChangeDelta(Action<IStyle> set)
        {
            //Selection
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //Refactoring
                layer.IsRefactoringRender = true;
                layerage.RefactoringParentsRender();
                set(layer.Style);
            });

            this.Invalidate();//Invalidate
        }

        public void StyleChangeCompleted<T>
        (
            Action<IStyle> set,

            string historyTitle,
            Func<IStyle, T> getHistory,
            Action<IStyle, T> setHistory
        )
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

            //Selection
            this.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = getHistory(layer.Style);
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    setHistory(layer.Style, previous);
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                set(layer.Style);
                this.StandStyleLayer = layer;
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }


        #endregion



        #region ITextLayer<T>


        /// <summary>
        /// Change T type for ITextLayer, save history, invalidate canvas.
        /// </summary>
        /// <typeparam name="T"> The T type property. </typeparam>
        /// <param name="set"> The sets of T. </param>
        /// <param name="historyTitle"> The history title. </param>
        /// <param name="getHistory"> The gets of history T. </param>
        /// <param name="setHistory"> The sets of history T. </param>
        public void ITextLayerChanged<T>
        (
            Action<ITextLayer> set,
            string historyTitle, Func<ITextLayer, T> getHistory, Action<ITextLayer, T> setHistory
        )
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(historyTitle);

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = getHistory(textLayer);
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        setHistory(textLayer, previous);
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    set(textLayer);
                }
            });

            //History
            this.HistoryPush(history);

            this.Invalidate();//Invalidate
        }
        

        #endregion



    }
}