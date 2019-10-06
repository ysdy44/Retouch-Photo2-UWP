using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionControl" />. 
    /// </summary>
    public sealed partial class SelectionControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public MenuTitle MenuTitle => this._MenuTitle;


        /// <summary> The single copyed layer. </summary>
        public ILayer Layer;
        
        /// <summary> The all copyed layers. </summary> 
        public List<ILayer> Layers = new List<ILayer>();


        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "SelectionControl" />'s selection mode. </summary>
        public ListViewSelectionMode Mode
        {
            get { return (ListViewSelectionMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "SelectionControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(ListViewSelectionMode), typeof(SelectionControl), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            SelectionControl con = (SelectionControl)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
                switch (value)
                {
                    case ListViewSelectionMode.None:
                        {
                            //Edit
                            con.CutButton.IsEnabled = false;
                            con.CopyButton.IsEnabled = false;
                            con.RemoveButton.IsEnabled = false;

                            //Select
                            con.InvertButton.IsEnabled = false;
                        }
                        break;
                    case ListViewSelectionMode.Single:
                    case ListViewSelectionMode.Multiple:
                        {
                            //Edit
                            con.CutButton.IsEnabled = true;
                            con.CopyButton.IsEnabled = true;
                            con.RemoveButton.IsEnabled = true;

                            //Select
                            con.InvertButton.IsEnabled = true;
                        }
                        break;
                }
            }
        }));

        #endregion


        //@Construct
        public SelectionControl()
        {
            this.InitializeComponent();

            #region Edit


            this.CutButton.Tapped += (s, e) =>
            {
                //Selection
                switch (this.SelectionViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None:
                        {
                            this.Layer = null;
                            this.Layers = null;
                        }
                        break;
                    case ListViewSelectionMode.Single:
                        {
                            this.Layer = this.SelectionViewModel.Layer.Clone(this.ViewModel.CanvasDevice);
                            this.Layers = null;
                        }
                        break;
                    case ListViewSelectionMode.Multiple:
                        {
                            this.Layer = null;

                            this.Layers.Clear();
                            foreach (ILayer layer in this.SelectionViewModel.Layers)
                            {
                                ILayer cloneLayer = layer.Clone(this.ViewModel.CanvasDevice);
                                this.Layers.Add(cloneLayer);
                            }
                        }
                        break;
                }

                this.PasteButton.IsEnabled = (this.Layer != null || this.Layers != null);//PasteButton

                this.ViewModel.Layers.RemoveAllSelectedLayers();//Remove
                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };
            this.CopyButton.Tapped += (s, e) =>
            {
                //Selection
                switch (this.SelectionViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None:
                        {
                            this.Layer = null;
                            this.Layers = null;
                        }
                        break;
                    case ListViewSelectionMode.Single:
                        {
                            this.Layer = this.SelectionViewModel.Layer.Clone(this.ViewModel.CanvasDevice);
                            this.Layers = null;
                        }
                        break;
                    case ListViewSelectionMode.Multiple:
                        {
                            this.Layer = null;

                            this.Layers.Clear();
                            foreach (ILayer layer in this.SelectionViewModel.Layers)
                            {
                                ILayer cloneLayer = layer.Clone(this.ViewModel.CanvasDevice);
                                this.Layers.Add(cloneLayer);
                            }
                        }
                        break;
                }

                this.PasteButton.IsEnabled = (this.Layer != null || this.Layers != null);//PasteButton
            };
            this.PasteButton.Tapped += (s, e) =>
            {
                if (this.Layer == null && this.Layers == null)//None
                {
                }
                else if (this.Layer != null && this.Layers == null)//Single
                {
                    ILayer cloneLayer = this.Layer.Clone(this.ViewModel.CanvasDevice);//Clone
                    this.ViewModel.Layers.MezzanineOnFirstSelectedLayer(cloneLayer);//Mezzanine

                    this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                    cloneLayer.SelectMode = SelectMode.Selected;
                    this.SelectionViewModel.SetModeSingle(cloneLayer);//Selection
                    this.ViewModel.Invalidate();//Invalidate
                }
                else if (this.Layer == null && this.Layers != null)//Multiple
                {
                    IList<ILayer> cloneLayers = new List<ILayer>();
                    foreach (ILayer child in this.Layers)
                    {
                        ILayer cloneLayer = child.Clone(this.ViewModel.CanvasDevice);//Clone
                        cloneLayer.SelectMode = SelectMode.Selected;
                        cloneLayers.Add(cloneLayer);
                    }

                    this.ViewModel.Layers.MezzanineRangeOnFirstSelectedLayer(cloneLayers);//Mezzanine

                    this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                    this.SelectionViewModel.SetModeMultiple(cloneLayers);//Selection
                    this.ViewModel.Invalidate();//Invalidate        
                }
            };
            this.RemoveButton.Tapped += (s, e) =>
            {
                int count = this.ViewModel.Layers.RootLayers.Count;
                if (count == 0) return;

                this.ViewModel.Layers.RemoveAllSelectedLayers();

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };



            #endregion


            #region Select


            this.AllButton.Tapped += (s, e) =>
            {
                int count = this.ViewModel.Layers.RootLayers.Count;
                if (count == 0) return;

                //Selection
                foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
                {
                    layer.SelectMode =  SelectMode.Selected;
                }

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };
            this.DeselectButton.Tapped += (s, e) =>
            {
                int count = this.ViewModel.Layers.RootLayers.Count;
                if (count == 0) return;

                //Selection
                foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
                {
                    layer.SelectMode = SelectMode.UnSelected;
                }

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };


            this.InvertButton.Tapped += (s, e) =>
            { 
            };


            #endregion

        }

    }
}