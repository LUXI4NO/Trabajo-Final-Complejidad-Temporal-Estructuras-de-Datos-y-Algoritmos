using System;
using System.Collections.Generic;
using System.Text;
using tp1;

namespace tpfinal
{
    public class Estrategia
    {
        // calculo la distancia de Levenshtein entre dos textos
        public int CalcularDistancia(string str1, string str2)
        {
            return Utils.calculateLevenshteinDistance(str1, str2);
        }

        //agrego un nuevo dato al arbol BK
        public void AgregarDato(ArbolGeneral<DatoDistancia> arbol, DatoDistancia dato)
        {
            DatoDistancia datoRaiz = arbol.getDatoRaiz();
            int d = CalcularDistancia(datoRaiz.texto, dato.texto);

            // Si ya existe el dato no hago nada.
            if (d == 0)
            {
                return;
            }

            dato.distancia = d;
            ArbolGeneral<DatoDistancia> hijoConDistancia = null;

            //busco si ya existe un hijo con esa distancia
            foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
            {
                if (hijo.getDatoRaiz().distancia == d)
                {
                    hijoConDistancia = hijo;
                    break;
                }
            }

            // si lo encuentro sigo por esa rama
            if (hijoConDistancia != null)
            {
                AgregarDato(hijoConDistancia, dato);
            }
            else
            {
                //si no, creo un nuevo hijo
                arbol.agregarHijo(new ArbolGeneral<DatoDistancia>(dato));
            }
        }

        //busco en el arbol con un umbral de similitud
        public void Buscar(ArbolGeneral<DatoDistancia> arbol, string elementoABuscar, int umbral, List<DatoDistancia> collected)
        {
            // limito la cantidad de resultados
            if (collected.Count >= 100)
            {
                return;
            }

            DatoDistancia datoRaiz = arbol.getDatoRaiz();
            int d = CalcularDistancia(datoRaiz.texto, elementoABuscar);

            // si esta dentro del umbral lo guardo
            if (d <= umbral)
            {
                collected.Add(datoRaiz);
            }

            // recorro los hijos dentro del rango permitido
            foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
            {
                int k = hijo.getDatoRaiz().distancia;

                if (k >= (d - umbral) && k <= (d + umbral))
                {
                    Buscar(hijo, elementoABuscar, umbral, collected);
                }
            }
        }

        //devuelvo todas las hojas del arbol
        public string Consulta1(ArbolGeneral<DatoDistancia> arbol)
        {
            StringBuilder sb = new StringBuilder();
            List<DatoDistancia> hojas = new List<DatoDistancia>();

            sb.Append("--- hojas del arbol BK ---\n");
            GetHojas(arbol, hojas);

            foreach (DatoDistancia hoja in hojas)
            {
                sb.Append(hoja.ToString() + "\n");
            }

            return sb.ToString();
        }

        // recorro el arbol y guardo las hojas
        private void GetHojas(ArbolGeneral<DatoDistancia> arbol, List<DatoDistancia> hojas)
        {
            if (arbol.esHoja())
            {
                hojas.Add(arbol.getDatoRaiz());
            }
            else
            {
                foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
                {
                    GetHojas(hijo, hojas);
                }
            }
        }

        // muestro los caminos desde la raiz hasta las hojas
        public string Consulta2(ArbolGeneral<DatoDistancia> arbol)
        {
            StringBuilder todosLosCaminos = new StringBuilder();
            List<DatoDistancia> caminoActual = new List<DatoDistancia>();

            todosLosCaminos.Append("--- caminos a las hojas ---\n");
            GetCaminos(arbol, caminoActual, todosLosCaminos);

            return todosLosCaminos.ToString();
        }

        // armo los caminos usando recursividad
        private void GetCaminos(ArbolGeneral<DatoDistancia> arbol, List<DatoDistancia> caminoActual, StringBuilder todosLosCaminos)
        {
            caminoActual.Add(arbol.getDatoRaiz());

            if (arbol.esHoja())
            {
                for (int i = 0; i < caminoActual.Count; i++)
                {
                    todosLosCaminos.Append(caminoActual[i].ToString());
                    if (i < caminoActual.Count - 1)
                    {
                        todosLosCaminos.Append(" -> ");
                    }
                }
                todosLosCaminos.Append("\n");
            }
            else
            {
                foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
                {
                    GetCaminos(hijo, caminoActual, todosLosCaminos);
                }
            }

            // quito el ultimo nodo para seguir con otro camino
            caminoActual.RemoveAt(caminoActual.Count - 1);
        }

        // muestro los datos por nivel usando recorrido BFS
        public string Consulta3(ArbolGeneral<DatoDistancia> arbol)
        {
            StringBuilder sb = new StringBuilder();
            Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>();

            cola.encolar(arbol);
            int nivel = 0;

            sb.Append("--- datos por nivel (BFS) ---\n");

            while (!cola.esVacia())
            {
                sb.Append("\nnivel " + nivel + ":\n");
                int nodosEnNivelActual = cola.cantidadElementos();

                for (int i = 0; i < nodosEnNivelActual; i++)
                {
                    ArbolGeneral<DatoDistancia> nodo = cola.desencolar();
                    sb.Append("- " + nodo.getDatoRaiz().ToString() + "\n");

                    foreach (ArbolGeneral<DatoDistancia> hijo in nodo.getHijos())
                    {
                        cola.encolar(hijo);
                    }
                }

                nivel++;
            }

            return sb.ToString();
        }
    }
}
