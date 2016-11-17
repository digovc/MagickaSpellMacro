using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;

namespace MagickaSpellMacro
{
    public class Macro
    {
        #region Constantes

        private const char CHR_KEY_SEPARATOR = ' ';
        private const int INT_KEY_INTERVALO = 50;
        private const int INT_MOUSE_EVENTF_LEFT_DOWN = 0x02;
        private const int INT_MOUSE_EVENTF_LEFT_UP = 0x04;
        private const int INT_MOUSE_EVENTF_MIDDLE_DOWN = 0x20;
        private const int INT_MOUSE_EVENTF_MIDDLE_UP = 0x0040;
        private const int INT_MOUSE_EVENTF_RIGHT_DOWN = 0x08;
        private const int INT_MOUSE_EVENTF_RIGHT_UP = 0x10;

        private const string STR_KEY_CLICK_MIDDLE_DOWN = "md";
        private const string STR_KEY_CLICK_MIDDLE_UP = "mu";
        private const string STR_KEY_CLICK_RIGHT_DOWN = "rd";
        private const string STR_KEY_CLICK_RIGHT_UP = "ru";
        private const string STR_KEY_SHIFT_DOWN = "sd";
        private const string STR_KEY_SHIFT_UP = "su";
        private const string STR_KEY_TIME = "tm";

        #endregion Constantes

        #region Atributos

        private bool _booCancelar;
        private VirtualKeyCode _enmVirtualKeyCode = VirtualKeyCode.NONAME;
        private int _intNumTecla;
        private List<string> _lstStrKey;
        private string _strMacro;

        public VirtualKeyCode enmVirtualKeyCode
        {
            get
            {
                if (_enmVirtualKeyCode != VirtualKeyCode.NONAME)
                {
                    return _enmVirtualKeyCode;
                }

                _enmVirtualKeyCode = this.getEnmVirtualKeyCode();

                return _enmVirtualKeyCode;
            }
        }

        private bool booCancelar
        {
            get
            {
                return _booCancelar;
            }

            set
            {
                _booCancelar = value;
            }
        }

        private int intNumTecla
        {
            get
            {
                return _intNumTecla;
            }

            set
            {
                _intNumTecla = value;
            }
        }

        private List<string> lstStrKey
        {
            get
            {
                if (_lstStrKey != null)
                {
                    return _lstStrKey;
                }

                _lstStrKey = this.getLstStrKey();

                return _lstStrKey;
            }
        }

        private string strMacro
        {
            get
            {
                return _strMacro;
            }

            set
            {
                _strMacro = value;
            }
        }

        #endregion Atributos

        #region Construtores

        public Macro(int intNumTecla, string strMacro)
        {
            this.intNumTecla = intNumTecla;
            this.strMacro = strMacro;
        }

        #endregion Construtores

        #region Métodos

        public void cancelar()
        {
            Console.WriteLine(string.Format("Cancelando a macro {0}...", this.intNumTecla));

            this.booCancelar = true;
        }

        public void executar()
        {
            this.booCancelar = false;

            if (this.lstStrKey == null)
            {
                return;
            }

            Console.WriteLine(string.Format("Executando a macro {0} ({1}).", this.intNumTecla, this.strMacro));

            foreach (string strKey in this.lstStrKey)
            {
                if (this.booCancelar)
                {
                    break;
                }

                this.executar(strKey);
            }

            this.executarFinalizar();
        }

        private void esperar(int intTempo = INT_KEY_INTERVALO)
        {
            if (intTempo < INT_KEY_INTERVALO)
            {
                intTempo = INT_KEY_INTERVALO;
            }

            int i = (intTempo / 10);
            int t = 0;

            while (true)
            {
                if (this.booCancelar)
                {
                    return;
                }

                if (t >= intTempo)
                {
                    return;
                }

                Thread.Sleep(i * 10);

                t += (i * 10);
            }
        }

        private void executar(string strKey)
        {
            if (string.IsNullOrEmpty(strKey))
            {
                return;
            }

            switch (strKey.ToLower())
            {
                case "1":
                    this.executarMagia(VirtualKeyCode.VK_1);
                    return;

                case "2":
                    this.executarMagia(VirtualKeyCode.VK_2);
                    return;

                case "3":
                    this.executarMagia(VirtualKeyCode.VK_3);
                    return;

                case "4":
                    this.executarMagia(VirtualKeyCode.VK_4);
                    return;

                case "q":
                    this.executarMagia(VirtualKeyCode.VK_W);
                    return;

                case "w":
                    this.executarMagia(VirtualKeyCode.VK_W);
                    return;

                case "e":
                    this.executarMagia(VirtualKeyCode.VK_E);
                    return;

                case "r":
                    this.executarMagia(VirtualKeyCode.VK_R);
                    return;

                case "a":
                    this.executarMagia(VirtualKeyCode.VK_A);
                    return;

                case "s":
                    this.executarMagia(VirtualKeyCode.VK_S);
                    return;

                case "d":
                    this.executarMagia(VirtualKeyCode.VK_D);
                    return;

                case "f":
                    this.executarMagia(VirtualKeyCode.VK_F);
                    return;

                case STR_KEY_CLICK_MIDDLE_DOWN:
                    this.executarMouse(INT_MOUSE_EVENTF_MIDDLE_DOWN);
                    return;

                case STR_KEY_CLICK_MIDDLE_UP:
                    this.executarMouse(INT_MOUSE_EVENTF_MIDDLE_UP);
                    return;

                case STR_KEY_CLICK_RIGHT_DOWN:
                    this.executarMouse(INT_MOUSE_EVENTF_RIGHT_DOWN);
                    return;

                case STR_KEY_CLICK_RIGHT_UP:
                    this.executarMouse(INT_MOUSE_EVENTF_RIGHT_UP);
                    return;

                case STR_KEY_SHIFT_DOWN:
                    this.executarKeyEvent(VirtualKeyCode.LSHIFT, true);
                    return;

                case STR_KEY_SHIFT_UP:
                    this.executarKeyEvent(VirtualKeyCode.LSHIFT, false);
                    return;

                case STR_KEY_TIME:
                    this.esperar(1000);
                    return;
            }
        }

        private void executarFinalizar()
        {
            this.esperar();

            if (this.strMacro.ToLower().Contains(STR_KEY_CLICK_MIDDLE_DOWN))
            {
                mouse_event(INT_MOUSE_EVENTF_MIDDLE_UP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
            }

            if (this.strMacro.ToLower().Contains(STR_KEY_CLICK_RIGHT_DOWN))
            {
                mouse_event(INT_MOUSE_EVENTF_RIGHT_UP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
            }

            if (this.strMacro.ToLower().Contains(STR_KEY_SHIFT_DOWN))
            {
                InputSimulator.SimulateKeyUp(VirtualKeyCode.LSHIFT);
            }

            if (!this.booCancelar)
            {
                this.executarFinalizarBeep();
            }

            Console.WriteLine(string.Format("Macro {0} {1}.", this.intNumTecla, this.booCancelar ? "cancelada" : "finalizada"));

            this.booCancelar = false;

            MacroManager.i.objMacroAtual = null;
        }

        private void executarFinalizarBeep()
        {
            Task.Run(() => this.executarFinalizarBeepTask());
        }

        private void executarFinalizarBeepTask()
        {
            for (int i = 0; i < 3; i++)
            {
                Task.Run(() => Console.Beep(1000, 250));
            }
        }

        private void executarKeyEvent(VirtualKeyCode enmVirtualKeyCode, bool booKeyDown)
        {
            if (booKeyDown)
            {
                InputSimulator.SimulateKeyDown(enmVirtualKeyCode);
            }
            else
            {
                InputSimulator.SimulateKeyUp(enmVirtualKeyCode);
            }

            this.esperar();
        }

        private void executarMagia(VirtualKeyCode enmVirtualKeyCode)
        {
            InputSimulator.SimulateKeyDown(enmVirtualKeyCode);

            this.esperar();

            InputSimulator.SimulateKeyUp(enmVirtualKeyCode);
        }

        private void executarMouse(int intMouseButton)
        {
            mouse_event((uint)intMouseButton, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);

            this.esperar();
            this.esperar();
        }

        private VirtualKeyCode getEnmVirtualKeyCode()
        {
            switch (this.intNumTecla)
            {
                case 2:
                    return VirtualKeyCode.NUMPAD2;

                case 3:
                    return VirtualKeyCode.NUMPAD3;

                case 4:
                    return VirtualKeyCode.NUMPAD4;

                case 5:
                    return VirtualKeyCode.NUMPAD5;

                case 6:
                    return VirtualKeyCode.NUMPAD6;

                case 7:
                    return VirtualKeyCode.NUMPAD7;

                case 8:
                    return VirtualKeyCode.NUMPAD8;

                case 9:
                    return VirtualKeyCode.NUMPAD9;

                default:
                    return VirtualKeyCode.NUMPAD1;
            }
        }

        private List<string> getLstStrKey()
        {
            if (string.IsNullOrEmpty(this.strMacro))
            {
                return null;
            }

            List<string> lstStrResultado = new List<string>();

            foreach (string strKey in this.strMacro.Split(CHR_KEY_SEPARATOR))
            {
                lstStrResultado.Add(strKey);
            }

            return lstStrResultado;
        }

        #endregion Métodos

        #region Eventos

        #endregion Eventos

        #region Externo

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        #endregion Externo
    }
}