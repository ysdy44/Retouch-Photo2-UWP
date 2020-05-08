using Retouch_Photo2.Brushs.FillOrStrokeIcons;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents the combo box that is used to select fill or stroke.
    /// </summary>
    public sealed partial class FillOrStrokeComboBox : UserControl
    {

        //@Delegate
        public EventHandler<FillOrStroke> FillOrStrokeChanged;

        //@Group
        private EventHandler<FillOrStroke> Group;

        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get { return (FillOrStroke)GetValue(FillOrStrokeProperty); }
            set { SetValue(FillOrStrokeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "FillOrStrokeComboBox.FillOrStroke" /> dependency property. </summary>
        public static readonly DependencyProperty FillOrStrokeProperty = DependencyProperty.Register(nameof(FillOrStroke), typeof(FillOrStroke), typeof(FillOrStrokeComboBox), new PropertyMetadata(FillOrStroke.Fill, (sender, e) =>
        {
            FillOrStrokeComboBox con = (FillOrStrokeComboBox)sender;

            if (e.NewValue is FillOrStroke value)
            {
                con.Group?.Invoke(con, value);
            }
        }));


        #endregion


        //@Construct
        public FillOrStrokeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
        }

    }

    /// <summary>
    /// Represents the combo box that is used to select fill or stroke.
    /// </summary>
    public sealed partial class FillOrStrokeComboBox : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructGroup(this.FillButton, resource.GetString("/ToolElements/Fill"), new FillIcon(), FillOrStroke.Fill);
            this.ConstructGroup(this.StrokeButton, resource.GetString("/ToolElements/Stroke"), new StrokeIcon(), FillOrStroke.Stroke);
        }

        //Group
        private void ConstructGroup(Button button, string text, UserControl icon, FillOrStroke fillOrStroke)
        {
            void group(FillOrStroke groupFillOrStroke)
            {
                if (groupFillOrStroke == fillOrStroke)
                {
                    button.IsEnabled = false;

                    this.Button.Content = text;
                }
                else button.IsEnabled = true;
            }

            //NoneButton
            group(this.FillOrStroke);

            //Buttons
            button.Content = text;
            button.Tag = icon;
            button.Tapped += (s, e) =>
            {
                this.FillOrStrokeChanged?.Invoke(this, fillOrStroke); //Delegate
                this.Flyout.Hide();
            };

            //Group
            this.Group += (s, e) => group(e);
        }

    }
}