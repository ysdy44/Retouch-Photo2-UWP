using Retouch_Photo2.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {        
        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Tools.ITool" />. </summary>
        public ITool Tool
        {
            get => this.tool ;
            set
            {
                //The current tool becomes the active tool.
                ITool oldTool = this.tool;
                oldTool.OnNavigatedFrom();

                //The current tool does not become an active tool.
                ITool newTool = value;
                newTool.OnNavigatedTo();

                this.tool = value;
                this.OnPropertyChanged(nameof(this.Tool));//Notify 
            }
        }
        private ITool tool;

        /// <summary> TransformerTool. </summary>
        public ITransformerTool TransformerTool { get; set; } 

        /// <summary> Tools. </summary>
        public IList<ITool> Tools { get; set; } = new List<ITool>();


        /// <summary> Touchbar's control. </summary>
        public UIElement TouchbarControl
        {

            get => this.touchbarControl;
            set
            {
                this.touchbarControl = value;
                this.OnPropertyChanged(nameof(this.TouchbarControl));//Notify 
            }
        }
        private UIElement touchbarControl;
    }
}