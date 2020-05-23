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
{            //TODO：
             /*
    public class Clipboard
    {
        ViewModel ViewModel => App.ViewModel;


        /// <summary> The single copyed layer. </summary>
        public Layerage CloneLayer => this.Layer?.Clone(this.ViewModel.CanvasDevice);
        private Layerage Layer;

        /// <summary> The all copyed layers. </summary> 
        public IEnumerable<Layerage> CloneLayers => this.Layers == null ? null : (from layer in this.Layers select layer.Clone(this.ViewModel.CanvasDevice));
        private IEnumerable<Layerage> Layers;


        public ListViewSelectionMode SelectionMode { get; set; }
        public bool CanPaste => this.SelectionMode == ListViewSelectionMode.Single || this.SelectionMode == ListViewSelectionMode.Multiple;

        public void SetModeNone()
        {
            this.Layer = null;
            this.Layers = null;
            this.SelectionMode = ListViewSelectionMode.None;
        }
        public void SetModeSingle(Layerage layer)
        {
            LayerBase layer2 = layer.Self;
            this.Layer = layer2.Clone(this.ViewModel.CanvasDevice);
            this.Layers = null;
            this.SelectionMode = ListViewSelectionMode.Single;
        }
        public void SetModeMultiple(IEnumerable<Layerage> layers)
        {
            this.Layer = null;
            this.Layers = from layer in layers select layer.Clone(this.ViewModel.CanvasDevice);
            this.SelectionMode = ListViewSelectionMode.Multiple;
        }
    }
              */

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionMenu" />. 
    /// </summary>
    public sealed partial class SelectionMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

     //   Clipboard Clipboard = new Clipboard();

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
            //TODO：
            /*
            this.CutButton.Click += (s, e) =>
            {
                //Selection
                switch (this.ViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None: this.Clipboard.SetModeNone(); break;
                    case ListViewSelectionMode.Single: this.Clipboard.SetModeSingle(this.ViewModel.Layer); break;
                    case ListViewSelectionMode.Multiple: this.Clipboard.SetModeMultiple(this.ViewModel.Layers); break;
                }
                this.PasteButton.IsEnabled = this.Clipboard.CanPaste;//PasteButton

                LayerCollection.RemoveAllSelectedLayers(this.ViewModel.LayerCollection);//Remove

                this.ViewModel.SetMode(this.ViewModel.LayerCollection);//Selection

                LayerCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);

                this.ViewModel.Invalidate();//Invalidate
            };

            this.DuplicateButton.Click += (s, e) =>
            {          
                         

                //Selection
                switch (this.ViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None: break;
                    case ListViewSelectionMode.Single: LayerCollection.Mezzanine(this.ViewModel.LayerCollection, this.ViewModel.Layer.Clone(this.ViewModel.CanvasDevice)); break;
                    case ListViewSelectionMode.Multiple: LayerCollection.MezzanineRange(this.ViewModel.LayerCollection, from layer in this.ViewModel.Layers select layer.Clone(this.ViewModel.CanvasDevice)); break;
                }
                
                LayerCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);

                this.ViewModel.SetMode(this.ViewModel.LayerCollection);

                this.ViewModel.Invalidate();//Invalidate
                          
            };

            this.CopyButton.Click += (s, e) =>
            {
                //Selection
                switch (this.ViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None: this.Clipboard.SetModeNone(); break;
                    case ListViewSelectionMode.Single: this.Clipboard.SetModeSingle(this.ViewModel.Layer); break;
                    case ListViewSelectionMode.Multiple: this.Clipboard.SetModeMultiple(this.ViewModel.Layers); break;
                }
                this.PasteButton.IsEnabled = this.Clipboard.CanPaste;//PasteButton
            };

            this.PasteButton.Click += (s, e) =>
            {
                //Selection
                switch (this.Clipboard.SelectionMode)
                {
                    case ListViewSelectionMode.None: break;
                    case ListViewSelectionMode.Single: LayerCollection.Mezzanine(this.ViewModel.LayerCollection, this.Clipboard.CloneLayer); break;
                    case ListViewSelectionMode.Multiple: LayerCollection.MezzanineRange(this.ViewModel.LayerCollection, this.Clipboard.CloneLayers); break;
                }

                this.ViewModel.SetMode(this.ViewModel.LayerCollection);//Selection

                LayerCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);

                this.ViewModel.Invalidate();//Invalidate        
            };

            this.ClearButton.Click += (s, e) =>
            {
                LayerCollection.RemoveAllSelectedLayers(this.ViewModel.LayerCollection);//Remove

                this.ViewModel.SetMode(this.ViewModel.LayerCollection);//Selection

                LayerCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);

                this.ViewModel.Invalidate();//Invalidate
            };


             */
        }


        //Select
        private void ConstructSelect()
        {

            this.AllButton.Click += (s, e) =>
            {
                //Selection
                this.ViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.IsSelected = false;
                });
                foreach (Layerage layer in this.ViewModel.LayerCollection.RootLayerages)
                {
                    layer.Self.IsSelected = true;
                }

                this.ViewModel.SetMode(this.ViewModel.LayerCollection);//Selection
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.DeselectButton.Click += (s, e) =>
            {
                //Selection
                foreach (Layerage layer in this.ViewModel.LayerCollection.RootLayerages)
                {
                    layer.Self.IsSelected = false;
                }

                this.ViewModel.SetMode(this.ViewModel.LayerCollection);//Selection
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
            
        }


        //Group
        private void ConstructGroup()
        {

            this.GroupButton.Click += (s, e) =>
            {
                LayerageCollection.GroupAllSelectedLayers(this.ViewModel.LayerCollection);

                this.ViewModel.SetMode(this.ViewModel.LayerCollection);
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.UnGroupButton.Click += (s, e) =>
            {
                LayerageCollection.UnGroupAllSelectedLayer(this.ViewModel.LayerCollection);

                this.ViewModel.SetMode(this.ViewModel.LayerCollection);
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.ReleaseButton.Click += (s, e) =>
            {
                //Selection
                this.ViewModel.SetValue((layerage) =>
                {
                    LayerageCollection.ReleaseGroupLayer(this.ViewModel.LayerCollection, layerage);
                });

                this.ViewModel.SetMode(this.ViewModel.LayerCollection);
                LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
                LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
        }

    }
}