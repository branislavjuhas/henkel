using Terminal.Gui; // Terminal.Gui
using NStack; // NStack

namespace Henkel
{
    class Interface
    {
        #region Variables

            public static StatusBar StatusBar = new StatusBar(new StatusItem[] {
                new StatusItem(Key.F2, "~F2~ Export", () => { }),
                new StatusItem(Key.F7, "~F7~ Settings", () => { }),
                new StatusItem(Key.F4 | Key.CtrlMask, "~Ctrl F4~ Exit", () => { }),
            }) { ColorScheme = Colors.TopLevel };

        #endregion

        public static void Initialize()
        {
            // Initialize the interface

            Application.Init();
            Application.Top.Add(StatusBar);
            Application.Run();
        }
    }
}