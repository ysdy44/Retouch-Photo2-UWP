using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Effects
{
    //delegate
    public delegate void EffectHandler(Effect effect);

    public abstract class Effect
    {
        //delegate
        public delegate void VoidCall();
        public static event VoidCall InvalidateCall = null;
        public static void Invalidate() => Effect.InvalidateCall?.Invoke();


        /// <summary> 是否开启 </summary>
        private bool isOn;
        public bool IsOn
        {
            get => isOn;
            set
            {
                if (this.Button != null)
                {
                    this.Button.IsEnabled = value;
                    //this.Button.Opacity = value ? 1.0 : 0.5;
                }

                if (this.ToggleSwitch != null)
                    if (this.ToggleSwitch.IsOn != value)
                        this.ToggleSwitch.IsOn = value;

                this.isOn = value;
            }
        }

        public EffectType Type;
        public FrameworkElement Icon;
        public FrameworkElement Page;

        /// <summary> 得到相应的项 </summary>
        public abstract EffectItem GetItem(EffectManager effectManager);
        /// <summary> 给当前类的页面来赋值 </summary>
        public abstract void SetPage(EffectManager effectManager);
        /// <summary> 重置参数 </summary>
        public abstract void Reset(EffectManager effectManager);

        #region Control

        public Windows.UI.Xaml.Controls.Button Button { set; protected get; }
        public ToggleSwitch ToggleSwitch { set; protected get; }
        public bool ToggleSwitchIsOn => this.ToggleSwitch.IsOn;
        
        #endregion

    }
}
