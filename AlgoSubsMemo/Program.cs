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

            //FIFO
            var fifo = new FIFO();

            foreach (var processo in processos)
            {
                fifo.Run(processo);
            }
            
            
        }
    }
}
