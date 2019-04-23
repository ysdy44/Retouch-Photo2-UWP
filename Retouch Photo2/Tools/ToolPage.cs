using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    public abstract class ToolPage : Page
    {
        //@Override
        /// <summary> 当前页面成为活动页面 </summary>
        public abstract void ToolOnNavigatedTo();
        /// <summary> 当前页面不再成为活动页面 </summary>
        public abstract void ToolOnNavigatedFrom();

        /// <summary>
        /// Passing values between [Tool Model] to [Tool Page].
        /// </summary>
        /// <param name="magicNumbers"> ??? </param>
        public virtual void Communication(int magicNumbers) { }

    }
}
