using System;
using System.Diagnostics;
using System.Threading;

namespace InverteVetorComThreads
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tamanho do vetor: ");
            int tamanho = int.Parse(Console.ReadLine());
            // int tamanho = 10;

            int[] v1 = new int[tamanho];
            int[] v2 = new int[tamanho];

            Random rnd = new Random();

            // preenchendo os vetores com números randômicos (1 a 9)           
            for (int x = 0; x < tamanho; x++)
            {
                v1[x] = rnd.Next(1, 10);               
            }

            int numThreads;
            // int numThreads = Environment.ProcessorCount;

            do
            {
                Console.WriteLine("Informe o número de threads (deve ser múltiplo do tamanho do vetor): ");
                numThreads = int.Parse(Console.ReadLine());
            } while (tamanho % numThreads != 0);

            // qual o tamanho de cada 'pedaço' do vetor original
            int diferenca = tamanho / numThreads;

            // guardamos os limites (índices) dos vetores
            // esses valores são usados pelas threads
            int[] limite = new int[numThreads + 1];
            for (int x = 0; x <= numThreads; x++)
            {
                limite[x] = diferenca * x;
            }
            
            Thread[] threads = new Thread[numThreads];
            int inicio, fim, start;
                      
            for (int x = 0; x < numThreads; x++)
            {
                // quando passamos os limites diretamente podemos ter erro de índice
                // porque funções lambda pegam uma referência à variável
                inicio = limite[x];
                fim = limite[x + 1];
                start = limite[numThreads - x] - 1;                

                threads[x] = new Thread(() => InverteVetor(v1, v2, inicio, fim, start));
                
                threads[x].Start();
                // garante que todas as tarefas vão ser terminadas
                threads[x].Join();
            }
         
            MostraVetor(v1, tamanho);
            MostraVetor(v2, tamanho);            
        }

        private static void InverteVetor(int[] v1, int[] v2, int inicio, int fim, int start)
        {
            // Console.WriteLine($"\nThread={Thread.CurrentThread.ManagedThreadId}\n");
            int ajuste = 0;
            for (int x = inicio; x < fim; x++)
            {
                v2[start - ajuste] = v1[x];
                ajuste++;
            }
        }

        private static void MostraVetor(int[] v, int tamanho)
        {
            Console.WriteLine();
            for (int x = 0; x < tamanho; x++)
            {
                Console.Write($"{v[x]} ");
            }
            Console.WriteLine();
        }
    }
}