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
