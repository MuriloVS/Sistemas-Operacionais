using AlgoSubsMemo.Classes;
using System;
using System.Collections.Generic;

namespace AlgoSubsMemo.Algoritmos
{
    class NUF
    {
        public int Run(Processo processo)
        {
            // variável para controlar quantas vezes uma página foi acessada
            // lembra que o elemento em si é sempre índice do vetor + 1
            int[] acessoPaginas = new int[processo.NumeroPaginas];
            
            int indice = 0;
            int menosUtilizado;
            int indiceNaMoldura;
            int trocas = 0;

            foreach (var pagina in processo.Paginas)
            {
                // INÍCIO parte inicial - moldura vazia/semi preenchida, página nova
                if (indice < processo.NumeroMolduras && !processo.Molduras.Contains(pagina))
                {                    
                    processo.Molduras.Insert(indice, pagina);                    
                    acessoPaginas[pagina - 1]++;
                    indice++;                   
                }
                // FIM parte inicial - moldura vazia/semi preenchida, página nova

                // INÍCIO parte inicial - moldura semi preenchida, página repetida
                else if (processo.Molduras.Contains(pagina))
                {                    
                    acessoPaginas[pagina-1]++;
                }
                // FIM parte inicial - moldura semi preenchida, página repetida

                // INÍCIO moldura preenchida, usar índice do processo NUF
                else
                {
                    trocas++;

                    menosUtilizado = AchaNUF(acessoPaginas, processo.Molduras) ;                    
                    indiceNaMoldura = processo.Molduras.IndexOf(menosUtilizado);
                    
                    processo.Molduras.RemoveAt(indiceNaMoldura);
                    processo.Molduras.Insert(indiceNaMoldura, pagina);

                    acessoPaginas[pagina-1]++;
                }
                // FIM moldura preenchida, usar índice do processo NUF

                //for (int i = 0; i < acessoPaginas.Length; i++)
                //{
                //    Console.Write($"{acessoPaginas[i]} ");
                //}
                //Console.WriteLine();
            }

            //Console.WriteLine($"| NUF: { trocas }");

            return trocas;
        }

        private int AchaNUF(int[] acessoPaginas, List<int> moldura)
        {
            int menor = int.MaxValue;
            int elemento = -1;

            for (int i = 0; i < acessoPaginas.Length; i++)
            {
                if ((acessoPaginas[i] < menor) && moldura.Contains(i + 1))
                {
                    menor = acessoPaginas[i];
                    elemento = i + 1;
                }
            }

            return elemento;
        }
    }
}
