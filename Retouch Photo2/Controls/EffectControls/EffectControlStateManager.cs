namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Manager of <see cref="EffectControlState"/>. 
    /// </summary>
    public class EffectControlStateManager
    {
        public bool IsEdit;

        public bool ExistEffect;

        /// <summary>
        /// Return status based on propertys.
        /// </summary>
        public EffectControlState GetState()
        {
            if (this.ExistEffect == false) return EffectControlState.Disable;
            if (this.IsEdit) return EffectControlState.Edit;

            return EffectControlState.Effects;
        }
    }
}