using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        /// <summary>
        /// Initializes a SharpenEffectPage. 
        /// </summary>
        public SharpenEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructSharpen_Amount();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/Sharpen");

            this.AmountTextBlock.Text = resource.GetString("/Effects/Sharpen_Amount");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.Sharpen;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new SharpenIcon()
        };
        
        public void Reset()
        {
            this.AmountSlider.Value = 0;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect sharpen");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = layer.Effect.Sharpen_Amount;
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Effect.Sharpen_Amount = previous;
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Effect.Sharpen_Amount = 0;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.Sharpen_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.AmountSlider.Value = effect.Sharpen_Amount;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {

        private void ConstructButton()
        {
            this.Button.Toggled += (isOn) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect sharpen");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.Sharpen_IsOn;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.Sharpen_IsOn = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.Sharpen_IsOn = isOn;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructSharpen_Amount()
        {
            //Radius
            this.AmountSlider.Value = 0;
            this.AmountSlider.Minimum = 0;
            this.AmountSlider.Maximum = 10;
            this.AmountSlider.ValueChangeStarted += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheSharpen();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                float amount = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.Effect.Sharpen_Amount = amount;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AmountSlider.ValueChangeCompleted += (s, value) =>
            {
                float amount = (float)value;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect sharpen");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingSharpen_Amount;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.Sharpen_Amount = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.Sharpen_Amount = amount;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }

    }
}