using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase : ILayer
    {

        private ILayer parents;
        public ILayer Parents
        {
            get => this.parents;
            set
            {
                int depth = (value == null) ? 0 : value.Control.Depth + 1; //+1

                this.Control.Depth = depth;
                foreach (ILayer child in this.Children)
                {
                    child.Control.Depth = depth + 1; //+1
                }

                this.parents = value;
            }
        }

        public IList<ILayer> Children { get; set; } = new List<ILayer>();


        public void Add(ILayer layer) => this._add(layer, null);

        public void AddRange(IList<ILayer> layers) => this._add(null, layers);

        private void _add(ILayer layer, IList<ILayer> layers)
        {
            bool isZero = this.Children.Count == 0;
            ExpandMode expandMode = isZero ? ExpandMode.Expand : this.ExpandMode;
            bool isSelected = this.SelectMode.ToBool();

            if (layer != null)
            {
                layer.Parents = this;
                this.Children.Add(layer);

                if (isSelected) layer.SelectMode = SelectMode.ParentsSelected;
            }
            else if (layers != null)
            {
                foreach (ILayer child in layers)
                {
                    child.Parents = this;
                    this.Children.Add(child);

                    if (isSelected) child.SelectMode = SelectMode.ParentsSelected;
                }
            }

            if (isZero) this.SelectMode = SelectMode.Selected;
            this.ExpandMode = expandMode;
        }


        public void Disengage(LayerCollection layerCollection)
        {
            if (this.Parents == null)
            {
                layerCollection.RootLayers.Remove(this);
            }
            else
            {
                this.Parents.Children.Remove(this);

                bool isZero = this.Parents.Children.Count == 0;
                if (isZero)
                {
                    this.Parents.ExpandMode = ExpandMode.NoChildren;
                }
            }
        }

    }
}