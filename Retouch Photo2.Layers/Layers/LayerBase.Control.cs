using Windows.UI.Xaml;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase 
    {

        public ILayerControl Control { get; set; }


        #region TagType

        private TagType tagType;
        public TagType TagType
        {
            get => this.tagType;
            set
            {
                this.Control.SetTagType(value);
                this.tagType = value;
            }
        }


        #endregion


        #region OverlayMode


        /// <summary> Gets or sets <see cref = "LayerControl" />'s overlay show status. </summary>
        public OverlayMode OverlayMode
        {
            get => this.overlayMode;
            set
            {
                if (this.overlayMode == value) return;
                this.Control.SetOverlayMode(value);
                this.overlayMode = value;
            }
        }
        private OverlayMode overlayMode;


        #endregion


        #region ExpandMode


        public ExpandMode ExpandMode
        {
            get => this.expandMode;
            set
            {
                this.Control.SetExpandMode(value);

                switch (value)
                {
                    case ExpandMode.Expand:
                        {
                            //Recursive
                            foreach (ILayer child in this.Children)
                            {
                                // If the parent layer is expaned, the children is visible
                                if (child.ExpandMode == ExpandMode.UnExpand)
                                    child.ExpandMode = ExpandMode.Expand;

                                child.Control.Self.Visibility = Visibility.Visible;
                            }
                        }
                        break;
                    case ExpandMode.UnExpand:
                        {
                            //Recursive
                            foreach (ILayer child in this.Children)
                            {
                                // The children must be collapsed.
                                if (child.ExpandMode == ExpandMode.Expand)
                                    child.ExpandMode = ExpandMode.UnExpand;

                                child.Control.Visibility = Visibility.Collapsed;
                            }
                        }
                        break;
                }

                this.expandMode = value;
            }
        }
        private ExpandMode expandMode;


        public void Expaned()
        {
            switch (this.ExpandMode)
            {
                case ExpandMode.UnExpand:
                    this.ExpandMode = ExpandMode.Expand;
                    break;
                case ExpandMode.Expand:
                    this.ExpandMode = ExpandMode.UnExpand;
                    break;
            }
        }


        #endregion


        #region SelectMode

        
        public SelectMode SelectMode
        {
            get => this.selectedMode;
            set
            {
                SelectMode oldMode = this.selectedMode;
                if (value == oldMode) return;
                this.selectedMode = value;

                this.Control.SetSelectMode(value);

                switch (value)
                {
                    case SelectMode.UnSelected:
                        switch (oldMode)
                        {
                            case SelectMode.ParentsSelected:
                                this._childrenToUnSelected();
                                break;
                            case SelectMode.Selected:
                            case SelectMode.ChildSelected:
                                this._parentsFromChildSelectedToUnSelected();
                                this._childrenToUnSelected();
                                break;
                        }
                        break;
                    case SelectMode.Selected:
                        switch (oldMode)
                        {
                            case SelectMode.UnSelected:
                                this._parentsToChildSelected();
                                this._childrenToParentsSelected();
                                break;
                            case SelectMode.ParentsSelected:
                            case SelectMode.ChildSelected:
                                this._childrenToParentsSelected();
                                break;
                        }
                        break;
                    case SelectMode.ParentsSelected:
                        switch (oldMode)
                        {
                            case SelectMode.UnSelected:
                            case SelectMode.ChildSelected:
                                this._childrenToParentsSelected();
                                break;
                        }
                        break;
                    case SelectMode.ChildSelected:
                        switch (oldMode)
                        {
                            case SelectMode.Selected:
                                this._parentsToChildSelected();
                                this._childrenToUnSelected();
                                break;
                            case SelectMode.UnSelected:
                            case SelectMode.ParentsSelected:
                                this._parentsToChildSelected();
                                break;
                        }
                        break;
                }

            }
        }
        private SelectMode selectedMode;


        public void Selected()
        {
            switch (this.SelectMode)
            {
                case SelectMode.Selected:
                    this.SelectMode = SelectMode.UnSelected;
                    break;
                case SelectMode.UnSelected:
                case SelectMode.ChildSelected:
                    this.SelectMode = SelectMode.Selected;
                    break;
            }
        }


        #endregion


        #region SelectMode Turn To

        
        // When the current layer is not selected, 
        // Parents also need to re-judge whether they have a child selected
        private void _parentsFromChildSelectedToUnSelected()
        {
            //Recursive
            if (this.Parents != null)
            {
                if (this.Parents.SelectMode == SelectMode.ChildSelected)
                {
                    bool anySelected = this.Parents.Children.Any(child =>
                    child != this && child.SelectMode != SelectMode.UnSelected);

                    if (anySelected == false)
                    {
                        this.Parents.SelectMode = SelectMode.UnSelected;
                    }
                }
            }
        }

        private void _childrenToUnSelected()
        {
            //Recursive
            foreach (ILayer child in this.Children)
            {
                child.SelectMode = SelectMode.UnSelected;
            }
        }

        private void _parentsToChildSelected()
        {
            //Recursive
            if (this.Parents != null)
            {
                this.Parents.SelectMode = SelectMode.ChildSelected;
            }
        }

        private void _childrenToParentsSelected()
        {
            //Recursive       
            foreach (ILayer child in this.Children)
            {
                child.SelectMode = SelectMode.ParentsSelected;
            }
        }


        #endregion

    }
}