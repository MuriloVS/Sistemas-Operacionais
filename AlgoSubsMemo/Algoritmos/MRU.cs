using System;
using AlgoSubsMemo.Classes;

namespace AlgoSubsMemo.Algoritmos
{
    class MRU
    {
        public void Run(Processo processo)
        {
            // variável auxiliar para contar a quanto tempo a posição não é acessada
            int[] posicaoUsada = new int[processo.NumeroMolduras]; 

            // duas variáveis utilizadas no início, quando ainda temos molduras não preenchidas
            int indice = 0;
            int controle = 0;

            // utilizada quando a moldura já está cheia e é necesssário trocar uma página
            int indiceMRU;
            
            int trocas = 0;

            foreach (var pagina in processo.Paginas)
            {
                // INÍCIO parte inicial - moldura vazia/semi preenchida, página nova
                if ((controle < processo.NumeroMolduras) && !processo.Molduras.Contains(pagina))
                {
                    //Console.WriteLine($"Pagina = {pagina}");

                    processo.Molduras.Insert(indice, pagina);
                    controle++;
                    
                    AumentaTempo(posicaoUsada, controle);
                    //Console.WriteLine();

                    // zera o tempo da página que foi incluída na moldura
                    posicaoUsada[indice] = 0;
                    
                    indice++;
                }
                // FIM parte inicial - moldura vazia/semi preenchida, página nova

                // INÍCIO parte inicial - moldura semi preenchida, página repetida
                else if (processo.Molduras.Contains(pagina))
                {
                    //Console.WriteLine($"Pagina = {pagina}");

                    AumentaTempo(posicaoUsada, controle);
                    //Console.WriteLine();

                    // zera o tempo da página que já está na moldura e foi acessada novamente
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

                    AumentaTempo(posicaoUsada, controle);
                    //Console.WriteLine();

                    // zera o tempo da página trocada
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

        // aumenta em um o "tempo" que as páginas não são acessadas
        private static void AumentaTempo(int[] posicaoUsada, int controle)
        {
            for (int i = 0; i < controle; i++)
            {
                posicaoUsada[i]++;
                //Console.Write($"{posicaoUsada[i]} ");
            }
        }

        // retorna o índice do MRU na moldura
        public int AchaMRU(int[] posicoes)
        {
            int max = posicoes[0];
            int index = 0;

            for (int i = 1; i < posicoes.Length; i++)
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
