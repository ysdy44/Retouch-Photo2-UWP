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
    public enum InvalidateMode
    {
        None,
        Thumbnail,
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

            this.MatrixTransformer.Width = project.Width;
            this.MatrixTransformer.Height = project.Height;

            this.Layers.Clear();
            foreach (Layer layer in project.Layers)
            {
                this.Layers.Add(layer);
            }
        }


        /// <summary> Retouch_Photo2's the only <see cref = "Microsoft.Graphics.Canvas.CanvasDevice" />. </summary>
        public CanvasDevice CanvasDevice { get; } = new CanvasDevice();


        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Library.MatrixTransformer" />. </summary>
        public MatrixTransformer MatrixTransformer = new MatrixTransformer();



        /// <summary> Retouch_Photo2's the only <see cref = "ViewModel.Text" />. </summary>
        private string text;
        public string Text
        {
            get => this.text;
            set
            {
                this.text = value;
                this.OnPropertyChanged(nameof(this.Text));//Notify 
            }
        }
               

        //Notify 
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
               
    }
}