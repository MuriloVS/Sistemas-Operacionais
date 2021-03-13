/*Diferentes algoritmos de substituição de página podem ser empregados para escolher
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

namespace AlgoSubsMemo
{
    class Program : ProgramBase
    {
        static void Main(string[] args)
        {
            LeArquivo();
            CriaProcessos();
            MostraProcessos();
            
            var fifo = new FIFO();
            var mru = new MRU();

            foreach (var processo in processos)
            {
                fifo.Run(processo);
                processo.Molduras.Clear();
                mru.Run(processo);
                processo.Molduras.Clear();
            }
        }
    }
}
