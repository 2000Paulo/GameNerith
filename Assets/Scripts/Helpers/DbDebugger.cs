using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DbHelpers 
{
    public static class DbDebugger
    {
        public static void DebugObject(object Objeto, string sPropriedadesDesejadas = "",   string sTitulo = "", bool mostraTipoDoValor = true, bool verificacaoRestrita = false) {
            var aPropriedades = new Dictionary<string, object>();
            var aDebug = new List<string>();
            var sDebugFinal = "";

            var aPropriedadesDesejadas = new List<string>();
            if (sPropriedadesDesejadas != "") {
                string[] aPropriedadesDesejadasSplit = sPropriedadesDesejadas.Split("#");
                foreach (string sPropriedadeDesejada in aPropriedadesDesejadasSplit) {
                    string sPropriedadeDesejadaFormatada = sPropriedadeDesejada.Trim().ToLower();
                    if (sPropriedadeDesejadaFormatada != "") {
                        aPropriedadesDesejadas.Add(sPropriedadeDesejadaFormatada);
                    }
                }
            }

            var oInstancia = Objeto.GetType();
            var oPropriedadesTransform = oInstancia.GetProperties();

            string sHeader = "------ [DEBUG : START] " + Objeto + " ------";
            string sFooter = "------ [DEBUG : FIM] " + Objeto + " ------";
            if (sTitulo != "") {
                sHeader = "------ [DEBUG : START] " + sTitulo + " ------";
                sFooter = "------ [DEBUG : FIM] " + sTitulo + " ------";
            }

            aDebug.Add(sHeader);
            foreach (PropertyInfo oPropriedade in oPropriedadesTransform){
                try {
                    var sNome = oPropriedade.Name.ToString().Trim();
                    var sValor = oPropriedade.GetValue(Objeto, null);
                    var sDebug = "[Propriedade] " + sNome + " = " + sValor;
                    if (mostraTipoDoValor) {
                        var sTipoDoValor = sValor.GetType();
                        sDebug += " [Tipo] " + sTipoDoValor;
                    }

                    if (
                        sPropriedadesDesejadas != ""
                    ) {
                        bool estaEntreAsPropriedadesDesejadas = false;
                        if (verificacaoRestrita == true) {
                            estaEntreAsPropriedadesDesejadas = aPropriedadesDesejadas.Contains(sNome.ToLower());
                        } else {
                            foreach (string sPropriedadeDesejada in aPropriedadesDesejadas){
                                bool encontrado = sNome.ToLower().Contains(sPropriedadeDesejada);
                                if (encontrado) {
                                    estaEntreAsPropriedadesDesejadas = true;
                                    break;
                                }
                            }
                        }

                        bool jaFoiAdicionada = aDebug.Contains(sDebug);
                        
                        if (
                            estaEntreAsPropriedadesDesejadas &&
                            jaFoiAdicionada == false
                        ) {
                            aDebug.Add(sDebug);
                            aPropriedades[sNome] = sValor;
                        }
                    } else {
                        aDebug.Add(sDebug);
                        aPropriedades[sNome] = sValor;
                    }

                } catch {  }
            }
            aDebug.Add(sFooter);

            foreach (string sPropriedade in aDebug) {
                sDebugFinal += sPropriedade + "\n\n";
            }

              Debug.Log(sDebugFinal);
        }
    }
}
