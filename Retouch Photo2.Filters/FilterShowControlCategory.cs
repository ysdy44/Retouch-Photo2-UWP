// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      ★
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Text;

namespace Retouch_Photo2.Filters
{
    /// <summary>
    /// Controls of <see cref="FilterCategory"/>
    /// </summary>
    public class FilterShowControlCategory : ObservableCollection<FilterShowControl>
    {

        /// <summary> Gets the title. </summary>
        public string Title { get; private set; } = string.Empty;
        public FontWeight Weight { get; private set; } = FontWeights.Normal;

        /// <summary> <see cref="FilterCategory.Name"/> </summary>
        public string Name { get; set; }

        /// <summary> <see cref="FilterCategory.Strings"/> </summary>
        public IDictionary<string, string> Strings { get; private set; }

        //@Construct
        /// <summary>
        /// Initializes a FilterShowControlCategory. 
        /// </summary>
        public FilterShowControlCategory() : base() { }
        /// <summary>
        /// Initializes a FilterShowControlCategory. 
        /// </summary>
        /// <param name="filterCategory"> The source <see cref="FilterCategory"/>. </param>
        public FilterShowControlCategory(FilterCategory filterCategory) : base
        (
            from filter
            in filterCategory.Filters
            select new FilterShowControl
            {
                Filter = filter
            }
        )
        {
            this.Name = filterCategory.Name;
            this.Strings = filterCategory.Strings;
        }

        public void Rename(string language)
        {
            this.Title = Retouch_Photo2.Elements.XML.CreateString(this.Name, this.Strings, language);
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Title)));//Notify
            this.Weight = this.Strings == null ? FontWeights.Normal : FontWeights.Bold;
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Weight)));//Notify
        }

        public FilterCategory ToFilterCategory() => new FilterCategory
        {
            Name = this.Name,
            Strings = this.Strings,
            Filters =
            (
                from control
                in this
                select control.Filter
            )
        };

    }
}