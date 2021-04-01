using System;
using AlgoSubsMemo.Classes;

namespace AlgoSubsMemo.Algoritmos
{
    class FIFO
    {
        public int Run(Processo processo)
        {
            int fistIn = 0;
            int trocas = 0;
            int indice = 0;

            foreach (var pagina in processo.Paginas)
            {
                // Console.WriteLine($"pagina = {page}");

                // INÍCIO parte inicial - moldura vazia/semi preenchida, página nova
                if (processo.Molduras.Count < processo.NumeroMolduras && !processo.Molduras.Contains(pagina))
                {
                    processo.Molduras.Insert(indice, pagina);
                    indice++;
                    trocas++;
                }
                // FIM parte inicial - moldura vazia/semi preenchida, página nova

                // INÍCIO parte inicial - moldura semi preenchida, página repetida
                else if (processo.Molduras.Contains(pagina))
                {
                    continue;
                }
                // FIM parte inicial - moldura semi preenchida, página repetida

                // INÍCIO moldura preenchida, usar índice da primeira página a entrar na moldura
                else
                {                    
                    processo.Molduras.RemoveAt(fistIn);
                    processo.Molduras.Insert(fistIn, pagina);
                    fistIn++;
                    indice = 0;

                    trocas++;
                }
                // FIM moldura preenchida, usar índice da primeira página a entrar na moldura

                // reseta o índice para a primeira posição da moldura
                if (fistIn == processo.NumeroMolduras)
                {
                    fistIn = 0;
                }

                //foreach (var mold in processo.Molduras)
                //{
                //    Console.WriteLine($"Moldura: { mold }");
                //}
                //Console.WriteLine("=============");
            }

            return trocas;
        }
    }
}
