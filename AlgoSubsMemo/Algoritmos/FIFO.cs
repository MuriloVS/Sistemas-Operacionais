using System;
using AlgoSubsMemo.Classes;

namespace AlgoSubsMemo.Algoritmos
{
    class FIFO
    {
        public void Run(Processo processo)
        {
            int fistIn = 0;
            int trocas = 0;
            int controle = 0;

            foreach (var page in processo.Paginas)
            {
                Console.WriteLine($"pagina = {page}");
                if (!processo.Molduras.Contains(page))
                {
                    if (processo.Molduras.Count < processo.NumeroMolduras)
                    {                        
                        processo.Molduras.Insert(controle, page);
                        controle++;
                    }                   
                    else
                    {
                        processo.Molduras.RemoveAt(fistIn);                        
                        processo.Molduras.Insert(fistIn, page);
                        fistIn++;
                        trocas++;
                        controle = 0;
                    }

                    if (fistIn == processo.NumeroMolduras)
                    {
                        fistIn = 0;
                    }
                }

                foreach (var mold in processo.Molduras)
                {
                    Console.WriteLine($"Moldura: { mold }");
                }
                Console.WriteLine("=============");
            }


            
            Console.WriteLine($"\nTrocas = { trocas }");
        }
    }
}
