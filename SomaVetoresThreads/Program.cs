using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SomaVetoresThreads
{
    class Program
    {
        static long total = 0;
       
        static void Main(string[] args)
        {
            Console.WriteLine("Tamanho do vetor: ");
            long tamanho = long.Parse(Console.ReadLine());
            // long tamanho = 100_000_000;

            long[] v1 = new long[tamanho];
            long[] v2 = new long[tamanho];

            Random rnd = new Random();

            // preenchendo os vetores com números randômicos (1 - 10)           
            for (long x = 0; x < tamanho; x++)
            {
                v1[x] = rnd.Next(11);
                v2[x] = rnd.Next(11);
            }
            
            Console.WriteLine("Informe o número de threads: ");
            int threads = int.Parse(Console.ReadLine());
            // int threads = Environment.ProcessorCount;

            // qual o tamanho de cada divisão do vetor original
            long diferenca = tamanho / threads;

            // guardamos os limites (índices) dos vetores
            // esses valores são usados pelas threads
            long[] limite = new long[threads + 1];
            for (int x = 0; x <= threads; x++)
            {
                limite[x] = diferenca * x;
            }

            Task[] tarefas = new Task[threads];
            long inicio, fim;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int x = 0; x < threads; x++)
            {
                // quando passamos os limites diretamente podemos ter erro de índice
                // porque funções lambda pegam uma referência à variável
                inicio = limite[x];
                fim = limite[x + 1];
                tarefas[x] = Task.Run(() => SomaVetores(v1, v2, inicio, fim));
                // sem esse wait temos erro nas somas - não sei o porquê...
                tarefas[x].Wait(1);
            }

            // "garante" que todas as tarefas vão ser terminadas
            Task.WaitAll(tarefas);
            sw.Stop();

            Console.WriteLine($"\n\nteste = { v1.Sum() + v2.Sum() }");
            Console.WriteLine($"total = { total } - { sw.ElapsedMilliseconds }ms");
        }

        private static void SomaVetores(long[] v1, long[] v2, long inicio, long fim)
        {            
            for (long x = inicio; x < fim; x++)
            {
                // todas as threads/tarefas modificam soma
                // é necessário garantir que elas não o façam ao mesmo tempo
                Interlocked.Add(ref total, v1[x] + v2[x]);
                // Console.Write($"Thread={Thread.CurrentThread.ManagedThreadId} ");
            }
        }
    }
}
