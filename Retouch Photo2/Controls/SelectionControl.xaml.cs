using Windows.UI.Xaml.Controls;
using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
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
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        /// <summary> The single copyed layer.  </summary>
        public Layer Layer { get; private set; }


        /// <summary> The all copyed layers.  </summary>
        public IEnumerable<Layer> Layers { get; private set; }


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

            #region Edit


            this.CutButton.RootButton.Tapped += (s, e) =>
            {
                //Edit
                this.Layer = null;
                this.Layers = null;

                //Selection
                switch (this.SelectionViewModel.Mode)
                {
                    case ListViewSelectionMode.None:
                        break;
                    case ListViewSelectionMode.Single:                        
                            this.Layer = this.SelectionViewModel.Layer.Clone(this.ViewModel.CanvasDevice);                        
                        break;
                    case ListViewSelectionMode.Multiple:
                            this.Layers = from layer in this.SelectionViewModel.Layers select layer.Clone(this.ViewModel.CanvasDevice);                        
                        break;
                }
                
                this.ViewModel.RemoveLayers();//Remove

                this.SelectionViewModel.SetModeNone();//Selection
                this.ViewModel.Invalidate();//Invalidate
            };
            this.CopyButton.RootButton.Tapped += (s, e) =>
            {
                //Edit
                this.Layer = null;
                this.Layers = null;

                //Selection
                switch (this.SelectionViewModel.Mode)
                {
                    case ListViewSelectionMode.None:
                        break;
                    case ListViewSelectionMode.Single:
                        this.Layer = this.SelectionViewModel.Layer.Clone(this.ViewModel.CanvasDevice);
                        break;
                    case ListViewSelectionMode.Multiple:
                        this.Layers = from layer in this.SelectionViewModel.Layers select layer.Clone(this.ViewModel.CanvasDevice);
                        break;
                }
            };
            this.PasteButton.RootButton.Tapped += (s, e) =>
            {
                int index = this.MezzanineViewModel.GetfFrstIndex(this.ViewModel.Layers);
                                
                if (this.Layer == null && this.Layers == null)//None
                {

                }
                else if (this.Layer != null && this.Layers == null)//Single
                {
                    //Clone
                    Layer cloneLayer = this.Layer.Clone(this.ViewModel.CanvasDevice);

                    //Insert
                    this.ViewModel.Layers.Insert(index, cloneLayer);

                    this.SelectionViewModel.SetModeSingle(cloneLayer);//Selection
                }
                else if (this.Layer == null && this.Layers != null)//Multiple
                {
                    foreach (Layer layer in this.Layers)
                    {
                        //Insert
                        this.ViewModel.Layers.Insert(index, layer.Clone(this.ViewModel.CanvasDevice));//Insert
                    }

                    this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                }

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RemoveButton.RootButton.Tapped += (s, e) =>
            {
                this.ViewModel.RemoveLayers();//Remove

                this.SelectionViewModel.SetModeNone();//Selection
                this.ViewModel.Invalidate();//Invalidate
            };



            #endregion


            #region Select


            this.AllButton.RootButton.Tapped += (s, e) =>
            {
                int count = this.ViewModel.Layers.Count;
                if (count == 0) return;

                //Selection
                foreach (Layer layer in this.ViewModel.Layers)
                {
                    layer.IsChecked = true;
                }

                this.SelectionViewModel.SetModeMultiple(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };
            this.DeselectButton.RootButton.Tapped += (s, e) =>
            {
                int count = this.ViewModel.Layers.Count;
                if (count == 0) return;
                
                //Selection
                foreach (Layer layer in this.ViewModel.Layers)
                {
                    layer.IsChecked = false;
                }

                this.SelectionViewModel.SetModeNone();//Selection
                this.ViewModel.Invalidate();//Invalidate
            };


            this.InvertButton.RootButton.Tapped += (s, e) =>
            {
                int count = this.ViewModel.Layers.Count;
                if (count == 0) return;

                //Selection
                foreach (Layer layer in this.ViewModel.Layers)
                {
                    layer.IsChecked = !layer.IsChecked;
                }

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion

        }

    }
}