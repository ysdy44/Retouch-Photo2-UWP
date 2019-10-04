using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            this.PasteButton.IsEnabled = false;//PasteButton


            this.ExtractButton.IsEnabled = false;
            this.MergeButton.IsEnabled = false;
            this.PixelButton.IsEnabled = false;
            this.FeatherButton.IsEnabled = false;
            this.TransformButton.IsEnabled = false;


            #region Edit


            this.CutButton.Tapped += (s, e) =>
            {
                //TODO: Layer New
                /*


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

                this.ViewModel.RemoveLayers();//Remove

                this.SelectionViewModel.SetModeNone();//Selection
                this.ViewModel.Invalidate();//Invalidate
                */
            };
            this.CopyButton.Tapped += (s, e) =>
            {
                //TODO: Layer New
                /*

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

                */
            };
            this.PasteButton.Tapped += (s, e) =>
            {//TODO: Layer New
             /*

                int index = this.MezzanineViewModel.GetfFrstIndex(this.ViewModel.Layers);
                
                if (this.Layer == null && this.Layers == null)//None
                {
                }
                else if (this.Layer != null && this.Layers == null)//Single
                {
                    ILayer cloneLayer = this.Layer.Clone(this.ViewModel.Layers, this.ViewModel.CanvasDevice);//Clone
                                        
                    this.ViewModel.Layers.Insert(index, cloneLayer);//Insert

                    this.SelectionViewModel.SetModeSingle(cloneLayer);//Selection
                }
                else if (this.Layer == null && this.Layers != null)//Multiple
                {
                    foreach (ILayer layer in this.Layers)
                    {
                        ILayer cloneLayer = layer.Clone(this.ViewModel.Layers, this.ViewModel.CanvasDevice);//Clone
                                                
                        this.ViewModel.Layers.Insert(index, cloneLayer);//Insert
                    }

                    this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                }

                this.ViewModel.Invalidate();//Invalidate
             */
            };
            this.RemoveButton.Tapped += (s, e) =>
            {
                //TODO: Layer New
                /*

                this.ViewModel.RemoveLayers();//Remove

                this.SelectionViewModel.SetModeNone();//Selection
                this.ViewModel.Invalidate();//Invalidate

                */
            };



            #endregion


            #region Select


            this.AllButton.Tapped += (s, e) =>
            {//TODO: Layer New
             /*
                int count = this.ViewModel.Layers.Count;
                if (count == 0) return;

                //Selection
                foreach (ILayer layer in this.ViewModel.Layers)
                {
                    layer.IsChecked = true;
                }

                this.SelectionViewModel.SetModeMultiple(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate

             */
            };
            this.DeselectButton.Tapped += (s, e) =>
            {
                //TODO: Layer New
                /*


                int count = this.ViewModel.Layers.Count;
                if (count == 0) return;
                
                //Selection
                foreach (ILayer layer in this.ViewModel.Layers)
                {
                    layer.IsChecked = false;
                }

                this.SelectionViewModel.SetModeNone();//Selection
                this.ViewModel.Invalidate();//Invalidate
                */
            };


            this.InvertButton.Tapped += (s, e) =>
            {
                //TODO: Layer New
                /*

                int count = this.ViewModel.Layers.Count;
                if (count == 0) return;

                //Selection
                foreach (ILayer layer in this.ViewModel.Layers)
                {
                    layer.IsChecked = !layer.IsChecked;
                }

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate

                */
            };


            #endregion

        }

    }
}