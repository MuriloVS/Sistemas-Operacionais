using AlgoSubsMemo.Classes;
using System.Collections.Generic;
using System.Linq;

namespace AlgoSubsMemo.Algoritmos
{
    class OPTIMUM
    {
        public int Run(Processo processo)
        {          

            int indice = 0;
            int contadorAux = 0;
            int trocas = 0;
            int paginaMaiorTempo;
            int indicePaginaMaiorTempo;

            foreach (var pagina in processo.Paginas)
            {
                // INÍCIO parte inicial - moldura vazia/semi preenchida, página nova
                if (indice < processo.NumeroMolduras && !processo.Molduras.Contains(pagina))
                {
                    processo.Molduras.Insert(indice, pagina);         
                    indice++;
                    contadorAux++;
                }
                // FIM parte inicial - moldura vazia/semi preenchida, página nova

                // INÍCIO parte inicial - moldura semi preenchida, página repetida
                else if (processo.Molduras.Contains(pagina))
                {         
                    contadorAux++;
                }
                // FIM parte inicial - moldura semi preenchida, página repetida

                // INÍCIO moldura preenchida, encontrar página que será usada daqui a mais tempo
                else
                {
                    //Console.WriteLine($"Pag para alocar: {pagina}");

                    paginaMaiorTempo = AchaMaiorTempo(processo, contadorAux);
                    indicePaginaMaiorTempo = processo.Molduras.IndexOf(paginaMaiorTempo);

                    //Console.WriteLine($"Pag Maior Tempo: {paginaMaiorTempo}");

                    // substituindo na moldura a página que levaria maior tempo a ser necessária novamente
                    processo.Molduras.RemoveAt(indicePaginaMaiorTempo);
                    processo.Molduras.Insert(indicePaginaMaiorTempo, pagina);

                    contadorAux++;
                    trocas++;
                }
                // FIM moldura preenchida, encontrar página que será usada daqui a mais tempo                
            }

            return trocas;
        }

        private int AchaMaiorTempo(Processo processo, int inicio)
        {            
            int tempo = 0;
            var dict = new Dictionary<int, int>();

            foreach (var pagina in processo.Molduras)
            {                
                for (int i = inicio; i < processo.Paginas.Count; i++)
                {
                    tempo++;
                    if (pagina == processo.Paginas[i])
                    {
                        break;
                    }                    
                }
                //Console.WriteLine($"Pagina: {pagina} - Tempo: {tempo}");
                dict.Add(pagina, tempo);
                tempo = 0;
            }

            return dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        }
    }
}
