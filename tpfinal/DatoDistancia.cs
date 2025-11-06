using System;
using System.Collections.Generic;

namespace tpfinal
{
    [Serializable]
    public class DatoDistancia
    {
        public int distancia { get; set; }
        public string texto { get; set; } // La cadena principal para comparar
        public string descripcion { get; set; } // Datos adicionales

        public DatoDistancia(int distancia, string texto, string descripcion)
        {
            this.distancia = distancia;
            this.texto = texto;
            this.descripcion = descripcion;
        }

        public override string ToString()
        {
            if (texto != null)
            {
                // Muestra la distancia (al padre) y el texto.
                return "(" + this.distancia + ") " + this.texto;
            }
            else
            {
                return "";
            }
        }
    }
}