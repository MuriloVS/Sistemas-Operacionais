using System;
using AlgoSubsMemo.Classes;

namespace AlgoSubsMemo.Algoritmos
{
    class MRU
    {
        public void Run(Processo processo)
        {
            // variável auxiliar para contar a quanto tempo a posição não é usada
            int[] posicaoUsada = new int[processo.NumeroMolduras]; 

            // duas variáveis utilizadas no início, quando ainda temos molduras não preenchidas
            int indice = 0;
            int controle = 0;

            // utilizada quando a moldura já está cheia e é necesssário trocar uma página
            int indiceMRU = 0;
            
            int trocas = 0;

            foreach (var pagina in processo.Paginas)
            {
                // INÍCIO parte inicial - moldura vazia
                if ((controle < processo.NumeroMolduras) && !processo.Molduras.Contains(pagina))
                {
                    //Console.WriteLine($"Pagina = {pagina}");

                    processo.Molduras.Insert(indice, pagina);                                        
                    controle++;                    

                    for (int i = 0; i < controle; i++)
                    {                       
                        posicaoUsada[i]++;
                        //Console.Write($"{posicaoUsada[i]} ");
                    }
                    //Console.WriteLine();

                    posicaoUsada[indice] = 0;
                    indiceMRU = AchaMRU(posicaoUsada);
                    indice++;
                }
                // FIM parte inicial - moldura vazia

                // INÍCIO parte inicial - moldura semi preenchida, página repetida
                else if (processo.Molduras.Contains(pagina))
                {
                    //Console.WriteLine($"Pagina = {pagina}");

                    for (int i = 0; i < controle; i++)
                    {
                        posicaoUsada[i]++;
                        //Console.Write($"{posicaoUsada[i]} ");
                    }
                    //Console.WriteLine();

                    posicaoUsada[processo.Molduras.IndexOf(pagina)] = 0;
                }
                // FIM parte inicial - moldura semi preenchida, página repetida

                // INÍCIO moldura preenchida, usar índice da moldura MRU 
                else
                {
                    //Console.WriteLine($"Pagina = {pagina}");
                    
                    trocas++;                   
                    indiceMRU = AchaMRU(posicaoUsada);

                    processo.Molduras.RemoveAt(indiceMRU);
                    processo.Molduras.Insert(indiceMRU, pagina);
                    
                    for (int i = 0; i < controle; i++)
                    {
                        posicaoUsada[i]++;
                        //Console.Write($"{posicaoUsada[i]} ");
                    }
                    //Console.WriteLine();

                    posicaoUsada[indiceMRU] = 0;
                    indice++;
                }
                // FIM moldura preenchida, usar índice da moldura MRU

                //foreach (var mold in processo.Molduras)
                //{
                //    Console.WriteLine($"Moldura: { mold }");
                //}
                //Console.WriteLine("=============");
            }

            Console.WriteLine($"Trocas = {trocas}");
        }

        // retorna o índice do MRU na moldura
        public int AchaMRU(int[] posicoes)
        {
            int max = -1;
            int index = -1;

            for (int i = 0; i < posicoes.Length; i++)
            {
                if (posicoes[i] > max)
                {
                    max = posicoes[i];
                    index = i;
                }
            }

            return index;
        }
    }
}
