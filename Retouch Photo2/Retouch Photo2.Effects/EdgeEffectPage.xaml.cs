using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Edge_IsOn"/>.
    /// </summary>
    public sealed partial class EdgeEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public EdgeEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructEdge_Amount();
            this.ConstructEdge_Radius();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Edge_IsOn"/>.
    /// </summary>
    public sealed partial class EdgeEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/Edge");

            this.AmountTextBlock.Text = resource.GetString("/Effects/Edge_Amount");
            this.RadiusTextBlock.Text = resource.GetString("/Effects/Edge_Radius");
        }

        //@Content
        public EffectType Type => EffectType.Edge;
        public FrameworkElement Page => this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new EdgeIcon()
        };


        public void Reset()
        {
            this.AmountSlider.Value = 50;
            this.RadiusSlider.Value = 0;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect outline");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous1 = layer.Effect.Edge_Amount;
                var previous2 = layer.Effect.Edge_Radius;
                history.UndoActions.Push(() =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Effect.Edge_Amount = previous1;
                    layer.Effect.Edge_Radius = previous2;
                });

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Effect.Edge_Amount = 0.5f;
                layer.Effect.Edge_Radius = 0.0f;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsButtonTapped = false;
            this.Button.ToggleSwitch.IsOn = effect.Edge_IsOn;
            this.Button.IsButtonTapped = true;
        }
        public void FollowPage(Effect effect)
        {
            this.AmountSlider.Value = effect.Edge_Amount * 100.0f;
            this.RadiusSlider.Value = effect.Edge_Radius;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Edge_IsOn"/>.
    /// </summary>
    public sealed partial class EdgeEffectPage : Page, IEffectPage
    {

        private void ConstructButton()
        {
            this.Button.ToggleSwitch.Toggled += (s, e) =>
            {
                if (this.Button.IsButtonTapped == false) return;
                bool isOn = this.Button.ToggleSwitch.IsOn;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect edge");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.Edge_IsOn;
                    history.UndoActions.Push(() =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.Edge_IsOn = previous;
                    });

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.Edge_IsOn = isOn;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructEdge_Amount()
        {
            //Amount
            this.AmountSlider.Value = 50;
            this.AmountSlider.Minimum = 0;
            this.AmountSlider.Maximum = 100;
            this.AmountSlider.ValueChangeStarted += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;
                    layer.Effect.CacheEdge();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                float amount = (float)value / 100.0f;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.Effect.Edge_Amount = amount;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AmountSlider.ValueChangeCompleted += (s, value) =>
            {
                float amount = (float)value / 100.0f;
                
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set edge effect amount");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingEdge_Amount;
                    history.UndoActions.Push(() =>
                    {     
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.Edge_Amount = previous;
                    });

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.Edge_Amount = amount;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };

        }

        private void ConstructEdge_Radius()
        {
            //Radius
            this.RadiusSlider.Value = 0;
            this.RadiusSlider.Minimum = 0;
            this.RadiusSlider.Maximum = 10;
            this.RadiusSlider.ValueChangeStarted += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;
                    layer.Effect.CacheEdge();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float blurAmount = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.Effect.Edge_Radius = blurAmount;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float blurAmount = (float)value;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set edge effect blur amount");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingEdge_Radius;
                    history.UndoActions.Push(() =>
                    {   
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.Edge_Radius = previous;
                    });

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.Edge_Radius = blurAmount;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };

        }

    }
}