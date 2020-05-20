using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Selections.EditIcons;
using Retouch_Photo2.Selections.GroupIcons;
using Retouch_Photo2.Selections.SelectIcons;
using Retouch_Photo2.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionMenu" />. 
    /// </summary>
    public sealed partial class SelectionMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        /// <summary> The single copyed layer. </summary>
        public ILayer Layer;
        /// <summary> The all copyed layers. </summary> 
        public List<ILayer> Layers = new List<ILayer>();


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

            this.SelectTextBlock.Text = resource.GetString("/Selections/Select");
            this.AllButton.Content = resource.GetString("/Selections/Select_All");
            this.AllButton.Tag = new AllIcon();
            this.DeselectButton.Content = resource.GetString("/Selections/Select_Deselect");
            this.DeselectButton.Tag = new DeselectIcon();
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

            this.CutButton.Click += (s, e) =>
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

            this.DuplicateButton.Click += (s, e) =>
            {
                IList<ILayer> layers = this.ViewModel.Layers.GetAllSelectedLayers();
                IEnumerable<ILayer> duplicateLayers = from i in layers select i.Clone(this.ViewModel.CanvasDevice);

                this.ViewModel.Layers.MezzanineRangeOnFirstSelectedLayer(duplicateLayers.ToList());
                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.CopyButton.Click += (s, e) =>
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

            this.PasteButton.Click += (s, e) =>
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

            this.ClearButton.Click += (s, e) =>
            {
                int count = this.ViewModel.Layers.RootLayers.Count;
                if (count == 0) return;

                this.ViewModel.Layers.RemoveAllSelectedLayers();

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };

        }


        //Select
        private void ConstructSelect()
        {

            this.AllButton.Click += (s, e) =>
            {
                int count = this.ViewModel.Layers.RootLayers.Count;
                if (count == 0) return;

                //Selection
                foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
                {
                    layer.SelectMode = SelectMode.Selected;
                }

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };

            this.DeselectButton.Click += (s, e) =>
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
            
        }


        //Group
        private void ConstructGroup()
        {

            this.GroupButton.Click += (s, e) =>
            {
                this.ViewModel.Layers.GroupAllSelectedLayers();
                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.UnGroupButton.Click += (s, e) =>
            {
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer.Type == LayerType.Group)
                    {
                        this.ViewModel.Layers.UnGroupLayer(layer);
                    }
                });

                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);
                this.ViewModel.Invalidate();//Invalidate
            };

        }

    }
}