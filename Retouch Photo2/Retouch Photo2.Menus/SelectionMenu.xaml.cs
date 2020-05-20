using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
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


        /// <summary> The single copyed layer. </summary>
        public ILayer CloneLayer => this.Layer?.Clone(this.ViewModel.CanvasDevice);
        private ILayer Layer;

        /// <summary> The all copyed layers. </summary> 
        public IEnumerable<ILayer> CloneLayers => this.Layers == null ? null : (from layer in this.Layers select layer.Clone(this.ViewModel.CanvasDevice));
        private IEnumerable<ILayer> Layers;


        public ListViewSelectionMode SelectionMode { get; set; }
        public bool CanPaste => this.SelectionMode == ListViewSelectionMode.Single || this.SelectionMode == ListViewSelectionMode.Multiple;

        public void SetModeNone()
        {
            this.Layer = null;
            this.Layers = null;
            this.SelectionMode = ListViewSelectionMode.None;
        }
        public void SetModeSingle(ILayer layer)
        {
            this.Layer = layer.Clone(this.ViewModel.CanvasDevice);
            this.Layers = null;
            this.SelectionMode = ListViewSelectionMode.Single;
        }
        public void SetModeMultiple(IEnumerable<ILayer> layers)
        {
            this.Layer = null;
            this.Layers = from layer in layers select layer.Clone(this.ViewModel.CanvasDevice);
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

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
                    case ListViewSelectionMode.None: this.Clipboard.SetModeNone(); break;
                    case ListViewSelectionMode.Single: this.Clipboard.SetModeSingle(this.SelectionViewModel.Layer); break;
                    case ListViewSelectionMode.Multiple: this.Clipboard.SetModeMultiple(this.SelectionViewModel.Layers); break;
                }
                this.PasteButton.IsEnabled = this.Clipboard.CanPaste;//PasteButton

                LayerCollection.RemoveAllSelectedLayers(this.ViewModel.Layers);//Remove

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

                LayerCollection.ArrangeLayersControls(this.ViewModel.Layers);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.DuplicateButton.Click += (s, e) =>
            {
                //Selection
                switch (this.SelectionViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None: break;
                    case ListViewSelectionMode.Single: LayerCollection.Mezzanine(this.ViewModel.Layers, this.SelectionViewModel.Layer.Clone(this.ViewModel.CanvasDevice)); break;
                    case ListViewSelectionMode.Multiple: LayerCollection.MezzanineRange(this.ViewModel.Layers, from layer in this.SelectionViewModel.Layers select layer.Clone(this.ViewModel.CanvasDevice)); break;
                }
                
                LayerCollection.ArrangeLayersControls(this.ViewModel.Layers);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.CopyButton.Click += (s, e) =>
            {
                //Selection
                switch (this.SelectionViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None: this.Clipboard.SetModeNone(); break;
                    case ListViewSelectionMode.Single: this.Clipboard.SetModeSingle(this.SelectionViewModel.Layer); break;
                    case ListViewSelectionMode.Multiple: this.Clipboard.SetModeMultiple(this.SelectionViewModel.Layers); break;
                }
                this.PasteButton.IsEnabled = this.Clipboard.CanPaste;//PasteButton
            };

            this.PasteButton.Click += (s, e) =>
            {
                //Selection
                switch (this.Clipboard.SelectionMode)
                {
                    case ListViewSelectionMode.None: break;
                    case ListViewSelectionMode.Single: LayerCollection.Mezzanine(this.ViewModel.Layers, this.Clipboard.CloneLayer); break;
                    case ListViewSelectionMode.Multiple: LayerCollection.MezzanineRange(this.ViewModel.Layers, this.Clipboard.CloneLayers); break;
                }

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

                LayerCollection.ArrangeLayersControls(this.ViewModel.Layers);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.ViewModel.Invalidate();//Invalidate        
            };

            this.ClearButton.Click += (s, e) =>
            {
                LayerCollection.RemoveAllSelectedLayers(this.ViewModel.Layers);//Remove

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

                LayerCollection.ArrangeLayersControls(this.ViewModel.Layers);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.ViewModel.Invalidate();//Invalidate
            };

        }


        //Select
        private void ConstructSelect()
        {

            this.AllButton.Click += (s, e) =>
            {
                //Selection
                foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
                {
                    layer.IsSelected = true;
                }

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.DeselectButton.Click += (s, e) =>
            {
                //Selection
                foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
                {
                    layer.IsSelected = false;
                }

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.ViewModel.Invalidate();//Invalidate
            };
            
        }


        //Group
        private void ConstructGroup()
        {

            this.GroupButton.Click += (s, e) =>
            {
                LayerCollection.GroupAllSelectedLayers(this.ViewModel.Layers);

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);

                LayerCollection.ArrangeLayersControls(this.ViewModel.Layers);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.UnGroupButton.Click += (s, e) =>
            {
                LayerCollection.UnGroupAllSelectedLayer(this.ViewModel.Layers);

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);

                LayerCollection.ArrangeLayersControls(this.ViewModel.Layers);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.ReleaseButton.Click += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer)=> 
                {
                    LayerCollection.ReleaseGroupLayer(this.ViewModel.Layers, layer);
                });

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);
                
                LayerCollection.ArrangeLayersControls(this.ViewModel.Layers);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

    }
}