// Core:              ★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using System;
using System.Windows.Input;

namespace Retouch_Photo2.Adjustments
{
    public class AdjustmentCommand : ICommand
    {
        //@Static
        public static Action<IAdjustment> Edit { get; set; }
        public static Action<IAdjustment> Remove { get; set; }


        private readonly Action action;
        public AdjustmentCommand(Action action)
        {
            this.action = action;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}