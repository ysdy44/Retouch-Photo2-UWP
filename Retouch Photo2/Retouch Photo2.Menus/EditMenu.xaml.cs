using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Edits.CombineIcons;
using Retouch_Photo2.Edits.EditIcons;
using Retouch_Photo2.Edits.GroupIcons;
using Retouch_Photo2.Edits.SelectIcons;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{        
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "EditMenu" />. 
    /// </summary>
    public sealed partial class EditMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        //@Construct
        public EditMenu()
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
    /// Retouch_Photo2's the only <see cref = "EditMenu" />. 
    /// </summary>
    public sealed partial class EditMenu : UserControl, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/Edit");

            this.EditTextBlock.Text = resource.GetString("/Edits/Edit");
            this.CutButton.Content = resource.GetString("/Edits/Edit_Cut");
            this.CutButton.Tag = new CutIcon();
            this.DuplicateButton.Content = resource.GetString("/Edits/Edit_Duplicate");
            this.DuplicateButton.Tag = new DuplicateIcon();
            this.CopyButton.Content = resource.GetString("/Edits/Edit_Copy");
            this.CopyButton.Tag = new CopyIcon();
            this.PasteButton.Content = resource.GetString("/Edits/Edit_Paste");
            this.PasteButton.Tag = new PasteIcon();
            this.ClearButton.Content = resource.GetString("/Edits/Edit_Clear");
            this.ClearButton.Tag = new ClearIcon();

            this.GroupTextBlock.Text = resource.GetString("/Edits/Group");
            this.GroupButton.Content = resource.GetString("/Edits/Group_Group");
            this.GroupButton.Tag = new GroupIcon();
            this.UnGroupButton.Content = resource.GetString("/Edits/Group_UnGroup");
            this.UnGroupButton.Tag = new UnGroupIcon();
            this.ReleaseButton.Content = resource.GetString("/Edits/Group_Release");
            this.ReleaseButton.Tag = new ReleaseIcon();

            this.SelectTextBlock.Text = resource.GetString("/Edits/Select");
            this.AllButton.Content = resource.GetString("/Edits/Select_All");
            this.AllButton.Tag = new AllIcon();
            this.DeselectButton.Content = resource.GetString("/Edits/Select_Deselect");
            this.DeselectButton.Tag = new DeselectIcon();
            this.InvertButton.Content = resource.GetString("/Edits/Select_Invert");
            this.InvertButton.Tag = new InvertIcon();
            
            this.CombineTextBlock.Text = resource.GetString("/Edits/Combine");
            this.AddButton.Content = resource.GetString("/Edits/Combine_Add");
            this.AddButton.Tag = new AddIcon();
            this.SubtractButton.Content = resource.GetString("/Edits/Combine_Subtract");
            this.SubtractButton.Tag = new SubtractIcon();
            this.IntersectButton.Content = resource.GetString("/Edits/Combine_Intersect");
            this.IntersectButton.Tag = new IntersectIcon();
            this.DivideButton.Content = resource.GetString("/Edits/Combine_Divide");
            this.DivideButton.Tag = new DivideIcon();
            this.CombineButton.Content = resource.GetString("/Edits/Combine_Combine");
            this.CombineButton.Tag = new CombineIcon();
        }

        //Menu
        public MenuType Type => MenuType.Edit;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Edits.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "EditMenu" />. 
    /// </summary>
    public sealed partial class EditMenu : UserControl, IMenu
    {

        //Edit
        private void ConstructEdit()
        {
            this.CutButton.Click += (s, e) => this.MethodViewModel.MethodEditCut();
            this.DuplicateButton.Click += (s, e) => this.MethodViewModel.MethodEditDuplicate();
            this.CopyButton.Click += (s, e) => this.MethodViewModel.MethodEditCopy();
            this.PasteButton.Click += (s, e) => this.MethodViewModel.MethodEditPaste();
            this.ClearButton.Click += (s, e) => this.MethodViewModel.MethodEditClear();
        }


        //Select
        private void ConstructSelect()
        {
            this.AllButton.Click += (s, e) => this.MethodViewModel.MethodSelectAll();
            this.DeselectButton.Click += (s, e) => this.MethodViewModel.MethodSelectDeselect();
            this.InvertButton.Click += (s, e) => this.MethodViewModel.MethodSelectInvert();
        }


        //Group
        private void ConstructGroup()
        {
            this.GroupButton.Click += (s, e) => this.MethodViewModel.MethodGroupGroup();
            this.UnGroupButton.Click += (s, e) => this.MethodViewModel.MethodGroupUnGroup();
            this.ReleaseButton.Click += (s, e) => this.MethodViewModel.MethodGroupRelease();
        }


        //Combine
        private void ConstructCombine()
        {
            this.AddButton.Click += (s, e) => { };
            this.SubtractButton.Click += (s, e) => { };
            this.IntersectButton.Click += (s, e) => { };
            this.DivideButton.Click += (s, e) => { };
            this.CombineButton.Click += (s, e) => { };
        }

    }
}