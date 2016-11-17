using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;

namespace MagickaSpellMacro
{
    public class MacroManager
    {
        #region Constantes

        private const string DIR_FILE_MACRO = "macro.txt";

        #endregion Constantes

        #region Atributos

        private static MacroManager _i;

        private List<Macro> _lstObjMacro;
        private Macro _objMacroAtual;

        public static MacroManager i
        {
            get
            {
                if (_i != null)
                {
                    return _i;
                }

                _i = new MacroManager();

                return _i;
            }
        }

        public Macro objMacroAtual
        {
            get
            {
                return _objMacroAtual;
            }

            set
            {
                _objMacroAtual = value;
            }
        }

        private List<Macro> lstObjMacro
        {
            get
            {
                if (_lstObjMacro != null)
                {
                    return _lstObjMacro;
                }

                _lstObjMacro = this.getLstObjMacro();

                return _lstObjMacro;
            }

            set
            {
                _lstObjMacro = value;
            }
        }

        #endregion Atributos

        #region Construtores

        private MacroManager()
        {
        }

        #endregion Construtores

        #region Métodos

        internal void iniciar()
        {
            Console.WriteLine("Inicializando serviço.");

            KeyboardManager.i.onKeyDown += this.KeyboardManager_onKeyDown;
        }

        private List<Macro> getLstObjMacro()
        {
            if (!File.Exists(DIR_FILE_MACRO))
            {
                File.WriteAllText(DIR_FILE_MACRO, "q q q q q");
            }

            List<Macro> lstResultado = new List<Macro>();

            foreach (string strMacro in File.ReadAllLines(DIR_FILE_MACRO))
            {
                if (lstResultado.Count >= 9)
                {
                    return lstResultado;
                }

                if (string.IsNullOrEmpty(strMacro))
                {
                    continue;
                }

                lstResultado.Add(new Macro((lstResultado.Count + 1), strMacro));
            }

            return lstResultado;
        }

        private void processarOnKeyDown(Keys enmKey)
        {
            switch (enmKey)
            {
                case Keys.NumPad0:
                    this.objMacroAtual?.cancelar();
                    return;

                case Keys.F5:
                    this.recarregarLstObjMacro();
                    return;
            }

            foreach (Macro objMacro in this.lstObjMacro)
            {
                if (!((int)enmKey).Equals((int)objMacro.enmVirtualKeyCode))
                {
                    continue;
                }

                if (objMacro.Equals(this.objMacroAtual))
                {
                    return;
                }

                this.objMacroAtual?.cancelar();

                this.objMacroAtual = objMacro;

                this.objMacroAtual.executar();
                return;
            }
        }

        private void recarregarLstObjMacro()
        {
            if (!InputSimulator.IsKeyDown(VirtualKeyCode.F5))
            {
                return;
            }

            this.lstObjMacro = null;

            Console.WriteLine("Lista de macros recarregada.");
        }

        #endregion Métodos

        #region Eventos

        private void KeyboardManager_onKeyDown(object sender, Keys enmKey)
        {
            Task.Run(() => this.processarOnKeyDown(enmKey));
        }

        #endregion Eventos
    }
}