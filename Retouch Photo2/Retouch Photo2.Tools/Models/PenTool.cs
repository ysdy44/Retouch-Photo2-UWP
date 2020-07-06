using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public partial class PenTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        Layerage CurveLayerage => this.SelectionViewModel.CurveLayerage;
        CurveLayer CurveLayer => this.SelectionViewModel.CurveLayer;

        GeometryTool GeometryTool = new GeometryTool();
        VectorVectorSnap Snap => this.ViewModel.VectorVectorSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Construct
        /// <summary>
        /// Initializes a PenTool. 
        /// </summary>
        public PenTool()
        {
            this.Content = this.GeometryTool;
            this.ConstructStrings();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.GeometryTool.OnNavigatedFrom();

            //Refactoring
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }


        private void CreateLayer(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory("Add layer", this.ViewModel.LayerageCollection);
            this.ViewModel.HistoryPush(history);


            //Transformer
            Transformer transformer = new Transformer(canvasPoint, canvasStartingPoint);

            //Layer
            CurveLayer curveLayer = new CurveLayer(this.ViewModel.CanvasDevice, canvasStartingPoint, canvasPoint)
            {
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandCurveStyle,
            };
            Layerage curveLayerage = curveLayer.ToLayerage();
            LayerBase.Instances.Add(curveLayer);

            //Mezzanine
            LayerageCollection.Mezzanine(this.ViewModel.LayerageCollection, curveLayerage);


            this.SelectionViewModel.SetModeSingle(curveLayerage);//Selection
            LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.ViewModel.LayerageCollection);
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

    }
}