using System;
using System.Threading;

namespace InverteVetorComThreads
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tamanho do vetor: ");
            int tamanho = int.Parse(Console.ReadLine());
            // long tamanho = 10;

            int[] v1 = new int[tamanho];
            // long[] v2 = new long[tamanho];

            Random rnd = new Random();

            // preenchendo os vetores com números randômicos (1 a 9)           
            for (int x = 0; x < tamanho; x++)
            {
                v1[x] = rnd.Next(10);               
            }

            int numThreads;
            // int numThreads = Environment.ProcessorCount;

            do
            {
                Console.WriteLine("Informe o número de threads (deve ser múltiplo do tamamho do vetor): ");
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
            
            // necessário inverter o vetor com os índices para espelhar o vetor original
            Array.Reverse(limite);

            Thread[] threads = new Thread[numThreads];
            int inicio, fim;
                      
            for (int x = 0; x < numThreads; x++)
            {
                // quando passamos os limites diretamente podemos ter erro de índice
                // porque funções lambda pegam uma referência à variável
                inicio = limite[x];
                fim = limite[x + 1];
                threads[x] = new Thread(() => MostraVetorInvertido(v1, inicio, fim));                

                // iniciando as threads
                threads[x].Start();                
                Thread.Sleep(1);
            }            

            // garante que todas as tarefas vão ser terminadas
            // e começamos pela última para que os vetores fiquem espelhados
            for (int i = numThreads-1; i >= 0; i--)
            {                                               
                threads[i].Join();             
            }
            
            MostraVetor(v1, tamanho);
        }

        private static void MostraVetorInvertido(int[] v, int inicio, int fim)
        {
            // Console.WriteLine($"\nThread={Thread.CurrentThread.ManagedThreadId}\n");
            for (long x = inicio-1; x >= fim; x--)
            {
                Console.Write($"{v[x]} ");
            }
        }

        private static void MostraVetor(int[] v, int tamanho)
        {
            Console.WriteLine("\nOriginal");
            for (long x = 0; x < tamanho; x++)
            {
                Console.Write($"{v[x]} ");
            }
        }
    }
}