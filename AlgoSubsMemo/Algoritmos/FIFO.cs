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
            int controle = 0;

            foreach (var pagina in processo.Paginas)
            {
                // Console.WriteLine($"pagina = {page}");

                // começa verificando se a página não está na moldura
                if (!processo.Molduras.Contains(pagina))
                {
                    // insere a página em uma moldura vazia
                    if (processo.Molduras.Count < processo.NumeroMolduras)
                    {                        
                        processo.Molduras.Insert(controle, pagina);
                        controle++;
                    }
                    // insere a página no índice do primeiro processo a entrar na moldura
                    // este índice é atualizado conforme as trocas são feitas
                    else
                    {
                        trocas++;
                        processo.Molduras.RemoveAt(fistIn);                        
                        processo.Molduras.Insert(fistIn, pagina);
                        fistIn++;                        
                        controle = 0;
                    }

                    // reseta o índice para a primeira posição da moldura
                    if (fistIn == processo.NumeroMolduras)
                    {
                        fistIn = 0;
                    }
                }

                //foreach (var mold in processo.Molduras)
                //{
                //    Console.WriteLine($"Moldura: { mold }");
                //}
                //Console.WriteLine("=============");
            }

            //Console.Write($"FIFO: { trocas } ");
            return trocas;
        }
    }
}
