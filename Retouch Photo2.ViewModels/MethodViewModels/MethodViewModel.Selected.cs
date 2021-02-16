using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        public void MethodSelectedNone()
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetIsSelected);

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.IsSelected == true)
                {
                    //History
                    var previous = layer.IsSelected;
                    history.UndoAction += () =>
                    {
                        layer.IsSelected = previous;
                    };

                    layer.IsSelected = false;
                }
            });

            //History
            this.HistoryPush(history);

            this.SetModeNone();//Selection
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate     
        }

        public void MethodSelectedNot(Layerage selectedLayerage)
        {
            ILayer selectedLayer = selectedLayerage.Self;

            //History 
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetIsSelected);

            var previous = selectedLayer.IsSelected;
            history.UndoAction += () =>
            {
                selectedLayer.IsSelected = previous;
            };

            selectedLayer.IsSelected = !selectedLayer.IsSelected;

            //History 
            this.HistoryPush(history);

            this.SetMode();//Selection
            //LayerManager.ArrangeLayersBackgroundItemClick(selectedLayerage);
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate
        }



        public void MethodSelectedNew(Layerage selectedLayerage)
        {
            ILayer selectedLayer = selectedLayerage.Self;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetIsSelected);

            if (selectedLayer.IsSelected == false)
            {
                var previous = selectedLayer.IsSelected;
                history.UndoAction += () =>
                {
                    selectedLayer.IsSelected = previous;
                };

                selectedLayer.IsSelected = true;
            }

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer != selectedLayer)
                {
                    if (layer.IsSelected == true)
                    {
                        //History
                        var previous = layer.IsSelected;
                        history.UndoAction += () =>
                        {
                            layer.IsSelected = previous;
                        };

                        layer.IsSelected = false;
                    }
                }
            });

            //History
            this.HistoryPush(history);

            this.SetModeSingle(selectedLayerage);//Selection
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate     
        }

        public void MethodSelectedAdd(Layerage selectedLayerage)
        {
            ILayer selectedLayer = selectedLayerage.Self;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetIsSelected);

            if (selectedLayer.IsSelected == false)
            {
                //History
                var previous = selectedLayer.IsSelected;
                history.UndoAction += () =>
                {
                    selectedLayer.IsSelected = previous;
                };

                selectedLayer.IsSelected = true;
            }

            //History
            this.HistoryPush(history);

            this.SetMode();//Selection
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate
        }

        public void MethodSelectedSubtract(Layerage selectedLayerage)
        {
            ILayer selectedLayer = selectedLayerage.Self;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetIsSelected);

            if (selectedLayerage.Self.IsSelected == true)
            {
                //History
                var previous = selectedLayer.IsSelected;
                history.UndoAction += () =>
                {
                    selectedLayer.IsSelected = previous;
                };

                selectedLayer.IsSelected = false;
            }

            //History
            this.HistoryPush(history);

            this.SetMode();//Selection
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate
        }

        /*
    public void MethodSelectedIntersect(Layerage selectedLayerage)
    {
        ILayer selectedLayer = selectedLayerage.Self;

        //History
        LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetIsSelected);

        //Selection
        this.SetValue((layerage) =>
        {
            if (layerage != selectedLayerage)
            {
                ILayer layer = layerage.Self;

                if (layer.IsSelected == true)
                {
                    //History
                    var previous = selectedLayer.IsSelected;
                    history.UndoAction += () =>
                    {
                        selectedLayer.IsSelected = previous;
                    };

                    layer.IsSelected = false;
                }
            }
        });

        //History
        this.HistoryPush(history);

        this.SetModeSingle(selectedLayerage);//Selection
        LayerManager.ArrangeLayersBackground();
        this.Invalidate();//Invalidate
    }
        */


    }
}