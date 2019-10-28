using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Blends
{
    public sealed partial class TagControl : UserControl
    {

        //@Delegate
        public EventHandler<TagType> TagTypeChanged;
        IList<RadioButton> _radioButtons = new List<RadioButton>();


        #region DependencyProperty

        /// <summary> Gets or sets the TagType. </summary>
        public TagType TagType
        {
            get { return (TagType)GetValue(BlendTypeProperty); }
            set { SetValue(BlendTypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TagControl.TagType" /> dependency property. </summary>
        public static readonly DependencyProperty BlendTypeProperty = DependencyProperty.Register(nameof(TagType), typeof(TagType), typeof(TagControl), new PropertyMetadata(TagType.None, (sender, e) =>
        {
            TagControl con = (TagControl)sender;

            if (e.NewValue is TagType value)
            {
                int index = (int)value;
                foreach (RadioButton radioButton in con._radioButtons)
                {
                    if (index== radioButton.TabIndex)
                    {
                        radioButton.IsChecked = true;
                    }
                }
            }
        }));

        #endregion


        //@Construct
        public TagControl()
        {
            this.InitializeComponent();
            Style style = this.Resources["RadioButtonStyle"] as Style;
 
            Array tagTypes = Enum.GetValues(typeof(TagType));
            foreach (TagType tagType in tagTypes)
            {              
                int index = (int)tagType;
                Color color= TagTypeHelper.TagConverter(tagType);

                RadioButton radioButton = new RadioButton
                {
                    Style= style,
                    TabIndex = index,
                    Background = new SolidColorBrush(color),
                };
                radioButton.Tapped += (s, e) =>
                {
                    this.TagTypeChanged?.Invoke(this, tagType); //Delegate
                };

                this._radioButtons.Add(radioButton);
                this.StackPanel.Children.Add(radioButton);
            }
        }
        
    }
}
