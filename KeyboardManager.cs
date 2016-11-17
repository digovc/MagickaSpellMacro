using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MagickaSpellMacro
{
    public class KeyboardManager
    {
        #region Constantes

        //private const int SW_HIDE = 0;

        private const int INT_WH_KEYBOARD_LL = 13;
        private const int INT_WM_KEYDOWN = 0x0100;

        #endregion Constantes

        #region Atributos

        private static KeyboardManager _i;
        private IntPtr _pIntHookI;

        public static KeyboardManager i
        {
            get
            {
                if (_i != null)
                {
                    return _i;
                }

                _i = new KeyboardManager();

                return _i;
            }
        }

        private IntPtr pIntHookI
        {
            get
            {
                return _pIntHookI;
            }

            set
            {
                _pIntHookI = value;
            }
        }

        #endregion Atributos

        #region Construtores

        private KeyboardManager()
        {
        }

        #endregion Construtores

        #region Métodos

        public void iniciar()
        {
            this.pIntHookI = this.setHook(this.onKeyDownSo);
        }

        private IntPtr setHook(LowLevelKeyboardProc objLowLevelKeyboardProc)
        {
            using (Process objProcess = Process.GetCurrentProcess())
            using (ProcessModule objProcessModule = objProcess.MainModule)
            {
                return SetWindowsHookEx(INT_WH_KEYBOARD_LL, objLowLevelKeyboardProc, GetModuleHandle(objProcessModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int intCode, IntPtr pIntWParam, IntPtr pIntLParam);

        #endregion Métodos

        #region Eventos

        private IntPtr onKeyDownSo(int intCode, IntPtr pIntWParam, IntPtr pIntLParam)
        {
            if ((intCode > -1) && (pIntWParam == (IntPtr)INT_WM_KEYDOWN))
            {
                Keys enmKeys = (Keys)Marshal.ReadInt32(pIntLParam);

                this.onKeyDown?.Invoke(this, enmKeys);
            }

            return CallNextHookEx(pIntHookI, intCode, pIntWParam, pIntLParam);
        }

        public event EventHandler<Keys> onKeyDown;

        #endregion Eventos

        #region Externo

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        #endregion Externo
    }
}