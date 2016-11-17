using System.Windows.Forms;

namespace MagickaSpellMacro
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            KeyboardManager.i.iniciar();

            MacroManager.i.iniciar();

            Application.Run();
        }
    }
}