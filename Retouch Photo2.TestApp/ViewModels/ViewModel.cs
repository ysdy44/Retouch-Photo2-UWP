using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Library;
using Retouch_Photo2.TestApp.Models;
using Retouch_Photo2.TestApp.Tools;
using Retouch_Photo2.TestApp.Tools.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary> Mode of the <see cref = "ViewModel.Invalidate" />. </summary>
    public enum InvalidateMode
    {
        /// <summary> Normal </summary>
        None,
        /// <summary> Thumbnail </summary>
        Thumbnail,
        /// <summary> High-definition </summary>
        HD,
    }

    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Reload <see cref = "ViewModel" /> </summary>
        /// <param name="project"> project </param>
        public void LoadFromProject(Project project)
        {
            if (project == null) return;

            this.CanvasTransformer.Width = project.Width;
            this.CanvasTransformer.Height = project.Height;

            this.Layers.Clear();
            foreach (Layer layer in project.Layers)
            {
                this.Layers.Add(layer);
            }
        }


        /// <summary> Retouch_Photo2's the only <see cref = "Microsoft.Graphics.Canvas.CanvasDevice" />. </summary>
        public CanvasDevice CanvasDevice { get; } = new CanvasDevice();

        /// <summary>
        /// Indicates that the contents of the CanvasControl need to be redrawn.
        /// </summary>
        /// <param name="mode"> invalidate mode </param>
        public void Invalidate(InvalidateMode mode = InvalidateMode.None) => this.InvalidateAction?.Invoke(mode);
        /// <summary> <see cref = "Action" /> of the <see cref = "ViewModel.Invalidate" />. </summary>
        public Action<InvalidateMode> InvalidateAction { private get; set; }


        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Layers.Layer" />s. </summary>
        public ObservableCollection<Layer> Layers { get; } = new ObservableCollection<Layer>();



        /// <summary> Retouch_Photo2's the only <see cref = "ViewModel.Text" />. </summary>
        public string Text
        {
            get => this.text;
            set
            {
                this.text = value;
                this.OnPropertyChanged(nameof(this.Text));//Notify 
            }
        }
        private string text= string.Empty;
        /// <summary> Sets's the <see cref = "ViewModel.Text" />. </summary>
        public void SetText()
        {
            if (App.ViewModel.Text.Length > 44) App.ViewModel.Text = string.Empty;
            else App.ViewModel.Text += "O";
        }



        //Notify 
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}