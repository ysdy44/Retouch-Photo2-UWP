using FanKit.Transformers;
using System;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// ID of <see cref="ILayer"/>.
    /// </summary>
    public partial class Layerage : IGetActualTransformer
    {


        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="action"> action </param>
        public void SetValue(Action<Layerage> action)
        {
            action(this);
        }

        /// <summary>
        /// Set value with group layerage's children.
        /// </summary>
        /// <param name="action"> action </param>
        public void SetValueWithChildrenOnlyGroup(Action<Layerage> action)
        {
            action(this);

            ILayer layer = this.Self;
            if (layer.Type == LayerType.Group)
            {
                foreach (Layerage child in this.Children)
                {
                    child.SetValueWithChildrenOnlyGroup(action);
                }
            }
        }
        /// <summary>
        /// Set value with children.
        /// </summary>
        /// <param name="action"> action </param>
        public void SetValueWithChildren(Action<Layerage> action)
        {
            action(this);

            if (this.Children.Count != 0)
            {
                foreach (Layerage child in this.Children)
                {
                    child.SetValueWithChildren(action);
                }
            }
        }

    }
}
