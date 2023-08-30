using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ideabuilder_status
{
    internal class MenuCommands
    {
        //Main Menu Commands
        public static readonly RoutedUICommand File_Exit = new RoutedUICommand
        (
                "Exit",
                "Exit",
                typeof(MenuCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4,ModifierKeys.Alt)
                }
            );
        public static readonly RoutedUICommand Settings = new RoutedUICommand("Preferences", "Preferences", typeof(MenuCommands), new InputGestureCollection());
        public static readonly RoutedUICommand Commands_Refresh = new RoutedUICommand("Refresh", "Refresh", typeof(MenuCommands), new InputGestureCollection());
        public static readonly RoutedUICommand Commands_Pause = new RoutedUICommand("Pause", "Pause", typeof(MenuCommands), new InputGestureCollection());
        public static readonly RoutedUICommand Commands_Resume = new RoutedUICommand("Resume", "Resume", typeof(MenuCommands), new InputGestureCollection());
        public static readonly RoutedUICommand Commands_Cancel = new RoutedUICommand("Cancel", "Cancel", typeof(MenuCommands), new InputGestureCollection());
    }
}
