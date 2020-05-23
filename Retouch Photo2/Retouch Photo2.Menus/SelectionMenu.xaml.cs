using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Selections.CombineIcons;
using Retouch_Photo2.Selections.EditIcons;
using Retouch_Photo2.Selections.GroupIcons;
using Retouch_Photo2.Selections.SelectIcons;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{        
    public class Clipboard
    {
        ViewModel ViewModel => App.ViewModel;


        /// <summary> The single copyed layerage. </summary>
        public Layerage Layerage { get; private set; }

        /// <summary> The all copyed layerages. </summary> 
        public IEnumerable<Layerage> Layerages { get; private set; }


        public ListViewSelectionMode SelectionMode { get; set; }
        public bool CanPaste => this.SelectionMode == ListViewSelectionMode.Single || this.SelectionMode == ListViewSelectionMode.Multiple;

        public void SetModeNone()
        {
            this.Layerage = null;
            this.Layerages = null;
            this.SelectionMode = ListViewSelectionMode.None;
        }
        public void SetModeSingle(Layerage layerage)
        {
            this.Layerage = layerage.Clone();
            this.Layerages = null;
            this.SelectionMode = ListViewSelectionMode.Single;
        }
        public void SetModeMultiple(IEnumerable<Layerage> layerage)
        {
            this.Layerage = null;
            this.Layerages = from layer in layerage select layer.Clone();
            this.SelectionMode = ListViewSelectionMode.Multiple;
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionMenu" />. 
    /// </summary>
    public sealed partial class SelectionMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

       Clipboard Clipboard = new Clipboard();

        //@Construct
        public SelectionMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();
            
            this.ConstructEdit();
            this.ConstructSelect();
            this.ConstructGroup();
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionMenu" />. 
    /// </summary>
    public sealed partial class SelectionMenu : UserControl, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/Selection");

            this.EditTextBlock.Text = resource.GetString("/Selections/Edit");
            this.CutButton.Content = resource.GetString("/Selections/Edit_Cut");
            this.CutButton.Tag = new CutIcon();
            this.DuplicateButton.Content = resource.GetString("/Selections/Edit_Duplicate");
            this.DuplicateButton.Tag = new DuplicateIcon();
            this.CopyButton.Content = resource.GetString("/Selections/Edit_Copy");
            this.CopyButton.Tag = new CopyIcon();
            this.PasteButton.Content = resource.GetString("/Selections/Edit_Paste");
            this.PasteButton.Tag = new PasteIcon();
            this.ClearButton.Content = resource.GetString("/Selections/Edit_Clear");
            this.ClearButton.Tag = new ClearIcon();

            this.GroupTextBlock.Text = resource.GetString("/Selections/Group");
            this.GroupButton.Content = resource.GetString("/Selections/Group_Group");
            this.GroupButton.Tag = new GroupIcon();
            this.UnGroupButton.Content = resource.GetString("/Selections/Group_UnGroup");
            this.UnGroupButton.Tag = new UnGroupIcon();
            this.ReleaseButton.Content = resource.GetString("/Selections/Group_Release");
            this.ReleaseButton.Tag = new ReleaseIcon();

            this.SelectTextBlock.Text = resource.GetString("/Selections/Select");
            this.AllButton.Content = resource.GetString("/Selections/Select_All");
            this.AllButton.Tag = new AllIcon();
            this.DeselectButton.Content = resource.GetString("/Selections/Select_Deselect");
            this.DeselectButton.Tag = new DeselectIcon();
            this.InvertButton.Content = resource.GetString("/Selections/Select_Invert");
            this.InvertButton.Tag = new InvertIcon();
            
            this.CombineTextBlock.Text = resource.GetString("/Selections/Combine");
            this.AddButton.Content = resource.GetString("/Selections/Combine_Add");
            this.AddButton.Tag = new AddIcon();
            this.SubtractButton.Content = resource.GetString("/Selections/Combine_Subtract");
            this.SubtractButton.Tag = new SubtractIcon();
            this.IntersectButton.Content = resource.GetString("/Selections/Combine_Intersect");
            this.IntersectButton.Tag = new IntersectIcon();
            this.DivideButton.Content = resource.GetString("/Selections/Combine_Divide");
            this.DivideButton.Tag = new DivideIcon();
            this.CombineButton.Content = resource.GetString("/Selections/Combine_Combine");
            this.CombineButton.Tag = new CombineIcon();
        }

        //Menu
        public MenuType Type => MenuType.Selection;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Selections.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionMenu" />. 
    /// </summary>
    public sealed partial class SelectionMenu : UserControl, IMenu
    {

        //Edit
        private void ConstructEdit()
        {
            Layerage CloneLayerage(Layerage source)
            {
                Layerage layerage = source;
                ILayer layer = layerage.Self;

                ILayer clone = layer.Clone(this.ViewModel.CanvasDevice);
                Layerage layerageClone = clone.ToLayerage();
                clone.Control.ConstructLayerControl(layerageClone);
                Layer.Instances.Add(clone);

                return layerageClone;
            }

            this.CutButton.Click += (s, e) =>
            {
                //Selection
                switch (this.ViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None: this.Clipboard.SetModeNone(); return;
                    case ListViewSelectionMode.Single: this.Clipboard.SetModeSingle(this.ViewModel.Layerage); break;
                    case ListViewSelectionMode.Multiple: this.Clipboard.SetModeMultiple(this.ViewModel.Layerages); break;
                }
                this.PasteButton.IsEnabled = this.Clipboard.CanPaste;//PasteButton

                //History
                this.ViewModel.HistoryPushLayeragesHistory("Cut layers");

                LayerageCollection.RemoveAllSelectedLayers(this.ViewModel.LayerageCollection);//Remove

                this.ViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.DuplicateButton.Click += (s, e) =>
            {       
                //Selection
                switch (this.ViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None: return;
                    case ListViewSelectionMode.Single:
                        {
                            //History
                            this.ViewModel.HistoryPushLayeragesHistory("Duplicate layers");

                            Layerage layerage = this.ViewModel.Layerage;
                            Layerage layerageClone = CloneLayerage(layerage);

                            LayerageCollection.Mezzanine(this.ViewModel.LayerageCollection, layerageClone);
                        }
                        break;
                    case ListViewSelectionMode.Multiple:
                        {
                            //History
                            this.ViewModel.HistoryPushLayeragesHistory("Duplicate layers");

                            IEnumerable<Layerage> layerages = this.ViewModel.Layerages;

                            IEnumerable<Layerage> layerageClones =
                                from layerage
                                in layerages
                                select CloneLayerage(layerage);

                            LayerageCollection.MezzanineRange(this.ViewModel.LayerageCollection, layerageClones);
                        }
                        break;
                }
                
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerageCollection);
                this.ViewModel.SetMode(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate                          
            };

            this.CopyButton.Click += (s, e) =>
            {
                //Selection
                switch (this.ViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None: this.Clipboard.SetModeNone(); break;
                    case ListViewSelectionMode.Single: this.Clipboard.SetModeSingle(this.ViewModel.Layerage); break;
                    case ListViewSelectionMode.Multiple: this.Clipboard.SetModeMultiple(this.ViewModel.Layerages); break;
                }
                this.PasteButton.IsEnabled = this.Clipboard.CanPaste;//PasteButton
            };

            this.PasteButton.Click += (s, e) =>
            {
                //Selection
                switch (this.Clipboard.SelectionMode)
                {
                    case ListViewSelectionMode.None: return;
                    case ListViewSelectionMode.Single:
                        {
                            //History
                            this.ViewModel.HistoryPushLayeragesHistory("Paste layers");

                            Layerage layerage = this.Clipboard.Layerage;
                            Layerage layerageClone = CloneLayerage(layerage);

                            LayerageCollection.Mezzanine(this.ViewModel.LayerageCollection, layerageClone);
                        }
                        break;
                    case ListViewSelectionMode.Multiple:
                        {
                            //History
                            this.ViewModel.HistoryPushLayeragesHistory("Duplicate layers");

                            IEnumerable<Layerage> layerages = this.Clipboard.Layerages;

                            IEnumerable<Layerage> layerageClones =
                                from layerage
                                in layerages
                                select CloneLayerage(layerage);

                            LayerageCollection.MezzanineRange(this.ViewModel.LayerageCollection, layerageClones);
                        }
                        break;
                }

                this.ViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection

                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerageCollection);

                this.ViewModel.Invalidate();//Invalidate        
            };

            this.ClearButton.Click += (s, e) =>
            {
                //History
                this.ViewModel.HistoryPushLayeragesHistory("Clear layers");

                LayerageCollection.RemoveAllSelectedLayers(this.ViewModel.LayerageCollection);//Remove

                this.ViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };


        }


        //Select
        private void ConstructSelect()
        {
            void setAllIsSelected(bool isSelected)
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set is selected");

                //Selection
                foreach (Layerage layerage in this.ViewModel.LayerageCollection.RootLayerages)
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.IsSelected;
                    if (previous != isSelected)
                    {
                        history.UndoActions.Push(() =>
                        {
                            ILayer layer2 = layerage.Self;

                            layer2.IsSelected = previous;
                        });
                    }

                    layer.IsSelected = isSelected;
                }

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            }

            this.AllButton.Click += (s, e) => setAllIsSelected(true);

            this.DeselectButton.Click += (s, e) => setAllIsSelected(false);

            this.InvertButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set is selected");

                //Selection
                foreach (Layerage layerage in this.ViewModel.LayerageCollection.RootLayerages)
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.IsSelected;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.IsSelected = previous;
                    });

                    layer.IsSelected = !layer.IsSelected;
                }

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Group
        private void ConstructGroup()
        {

            this.GroupButton.Click += (s, e) =>
            {
                //History
                this.ViewModel.HistoryPushLayeragesHistory("Group layers");

                LayerageCollection.GroupAllSelectedLayers(this.ViewModel.LayerageCollection);

                this.ViewModel.SetMode(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.UnGroupButton.Click += (s, e) =>
            {
                //History
                this.ViewModel.HistoryPushLayeragesHistory("UnGroup layers");

                LayerageCollection.UnGroupAllSelectedLayer(this.ViewModel.LayerageCollection);

                this.ViewModel.SetMode(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.ReleaseButton.Click += (s, e) =>
            {
                //History
                this.ViewModel.HistoryPushLayeragesHistory("Release layers");

                //Selection
                this.ViewModel.SetValue((layerage) =>
                {
                    LayerageCollection.ReleaseGroupLayer(this.ViewModel.LayerageCollection, layerage);
                });

                this.ViewModel.SetMode(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Combine
        private void ConstructCombine()
        {
            this.AddButton.Click += (s, e) =>
            {
            };
            this.SubtractButton.Click += (s, e) =>
            {
            };
            this.IntersectButton.Click += (s, e) =>
            {
            };
            this.DivideButton.Click += (s, e) =>
            {
            };
            this.CombineButton.Click += (s, e) =>
            {
            };
        }

    }
}