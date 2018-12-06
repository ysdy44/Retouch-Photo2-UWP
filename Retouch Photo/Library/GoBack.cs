using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retouch_Photo.Library
{
    public class GoBack
    {
        public string Text = "给ViewModel的CanvasControl赋值，需要提前实例化DrawPage再返回，没有实际意义的字符串";

        /// <summary>
        /// 是否经过了Frame的Goback
        /// （这个方法返回一次true后永远返回false，避免重复）
        /// 
        ///   private void Page_Loaded(object sender, RoutedEventArgs e)
        ///   {
        ///       if (this.ViewModel.IsGoBack) this.Frame.Navigate(typeof(MainPage));            
        ///   }
        ///</summary>
        public bool IsGoBack
        {
            get
            {
                if (isGoBack)
                {
                    isGoBack = false;
                    return true;
                }
                else return false;
            }
            set => isGoBack = value;
        }
        private bool isGoBack = false;



        /// <summary>
        /// 判断文字是否相同，来结束GoBack
        /// （避免多次GoBack的判断语句）
        ///</summary>
        /// 
        /// protected override void OnNavigatedTo(NavigationEventArgs e)//当前页面成为活动页面
        /// {
        ///      if (e.Parameter is string text)
        ///      {
        ///            if (this.ViewModel.HadGoBack(text)) return;
        ///      }
        /// }
        /// <param name="text"></param>
        /// <returns></returns>
        public bool HadGoBack(string text)
        {
            if (text == Text)
            {
                IsGoBack = true;
                return true;
            }
            else return false;
        }



    }
}
