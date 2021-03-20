/* Diferentes algoritmos de substituição de página podem ser empregados para escolher
 * qual página deve ser substituída da memória. O objetivo deste trabalho é comparar 
 * o desempenho dos algoritmos FIFO, Menos Recentemente Usada (MRU), Não usada
 * frequentemente (NUF) com relação ao algoritmo ótimo em termos de número de troca
 * de páginas.
 * 
 * O programa receberá como entrada um arquivo contendo múltiplas linhas, sendo
 * uma linha referente a cada caso de teste. O formato da linha é o seguinte
 * 
 * Número de molduras de página na memória|número de páginas do processo|sequência em que as páginas são acessadas
 * 
 * Para cada caso de teste, o programa deverá produzir como saída uma linha contendo:
 * 
 * Número de trocas de página no algoritmo FIFO|
 * Número de trocas de página no algoritmo MRU|
 * Número de trocas de página no algoritmo NUF|
 * Número de trocas de página no algoritmo ótimo|
 * nome do algoritmo com desempenho mais próximo do ótimo 
 */


using AlgoSubsMemo.Algoritmos;
using AlgoSubsMemo.Classes;
using System;
using System.Text;
using System.Linq;

namespace AlgoSubsMemo
{
    class Program : ProgramBase
    {
        static void Main(string[] args)
        {
            LeArquivo();
            CriaProcessos();
            //MostraProcessos();
            
            var fifo = new FIFO();
            var mru = new MRU();
            var nuf = new NUF();
            var optimum = new OPTIMUM();

            int[] trocas = new int[3];
            int min;
            int melhor;
            StringBuilder algoritmos = new StringBuilder();
            
            foreach (var processo in processos)
            {              
                trocas[0] = fifo.Run(processo);                
                processo.Molduras.Clear();
                trocas[1] = mru.Run(processo);
                processo.Molduras.Clear();
                trocas[2] = nuf.Run(processo);
                processo.Molduras.Clear();
                melhor = optimum.Run(processo);

                min = trocas.Min();

                for (int i = 0; i < trocas.Length; i++)
                {
                    if (trocas[i] == min)
                    {
                        if (i == 0)
                        {
                            algoritmos.Append("FIFO ");
                        }
                        else if (i == 1)
                        {
                            algoritmos.Append(" MRU ");
                        }
                        else
                        {
                            algoritmos.Append(" NUF");
                        }
                    }
                }

                Console.WriteLine($"FIFO: {trocas[0]} | MRU: {trocas[1]} | NUF: {trocas[2]} | Ótimo: {melhor} | Melhor(es): {algoritmos}");

                algoritmos.Clear();
            }
        }
    }
}
