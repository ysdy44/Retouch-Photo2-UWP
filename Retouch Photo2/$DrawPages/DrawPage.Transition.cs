using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //TaskCompletionSource
        private TaskCompletionSource<string> RenameTaskSource;

        private async Task<string> ShowRenameDialogTask(string placeholderText)
        {
            this.RenameDialog.Show();

            this.RenameTextBox.Text = placeholderText;
            this.RenameTextBox.SelectAll();
            this.RenameTextBox.Focus(FocusState.Programmatic);

            this.RenameTaskSource = new TaskCompletionSource<string>();
            string resultName = await this.RenameTaskSource.Task;
            this.RenameTaskSource = null;
            return resultName;
        }

        private void RenameDialogTrySetResult(FrameworkElement element, string name)
        {
            if (this.RenameTaskSource != null && this.RenameTaskSource.Task.IsCanceled == false)
            {
                this.RenameTaskSource.TrySetResult(name);
            }

            this.RenameDialog.Hide();
        }


        //////////////////////////


        //Rename
        private void ConstructRenameDialog()
        {
            this.RenameDialog.SecondaryButtonClick += (s, e) => this.RenameDialogTrySetResult(null, null);
            this.RenameDialog.PrimaryButtonClick += (s, e) =>
            {
                this.RenameDialog.Focus(FocusState.Programmatic);
                string text = this.RenameTextBox.Text;
                this.RenameDialogTrySetResult(this.RenameTextBox, text);
            };
            this.RenameTextBox.Loaded += (s, e) => this.RenameTextBox.Focus(FocusState.Programmatic);
        }


        private async void ShowRenameDialog()
        {
            string placeholderText = this.SelectionViewModel.LayerName;
            string name = await this.ShowRenameDialogTask(placeholderText);
            if (string.IsNullOrEmpty(name)) return;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetName);

            //Selection
            this.SelectionViewModel.LayerName = name;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Name != name)
                {
                    //History
                    var previous = layer.Name;
                    history.UndoAction += () =>
                    {
                        layer.Name = previous;
                    };

                    layer.Name = name;
                }
            });

            //History
            this.ViewModel.HistoryPush(history);
        }


        //////////////////////////


        //Transition
        /// <summary> The transition data. </summary>
        Rect? _lockSourceRect;


        private void RegisteTransition()
        {
            this.LoadingControl.Completed += (s, e) => this.TransitionComplete();

            this.LoadingControl.ValueChanged += (s, e) =>
            {
                float value = (float)e.NewValue;
                this.TransitionDelta(value);
            };
        }


        //Staring
        private void TransitionStaring()
        {
            this.LoadingControl.State = LoadingState.LoadingWithProgress;

            //Destination
            Vector2 destinationPostion = this.SettingViewModel.FullScreenOffset;
            float destinationWidth = this.SettingViewModel.CanvasWidth;
            float destinationHeight = this.SettingViewModel.CanvasHeight;
            this.ViewModel.CanvasTransformer.TransitionDestination(destinationPostion, destinationWidth, destinationHeight);


            if (this._lockSourceRect is Rect data)
            {
                //Source
                this.ViewModel.CanvasTransformer.TransitionSource(data);
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.LoadingControl.Begin();//Storyboard
            }
            else this.TransitionComplete();
        }

        //Delta
        private void TransitionDelta(float value)
        {
            this.ViewModel.CanvasTransformer.Transition(value);
            this.ViewModel.Invalidate(InvalidateMode.None);//Invalidate
        }

        //Complete
        private void TransitionComplete()
        {
            this.LoadingControl.State = LoadingState.None;

            //Transition
            this.ViewModel.CanvasTransformer.Transition(1.0f);

            this.DrawLayout.IsFullScreen = false;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }


    }
}