using System;
using System.Threading;
using System.Threading.Tasks;

namespace SomaVetoresThreads
{
    class Program
    {
        private static long total = 0;
        private static long[] v1;
        private static long[] v2;

        static void Main(string[] args)
        {
            Console.WriteLine("Tamanho do vetor: ");
            long tamanho = int.Parse(Console.ReadLine());

            v1 = new long[tamanho];
            v2 = new long[tamanho];

            Random rnd = new Random();

            // preenchendo os vetores com números randômicos
            // e calculando o total para comparar com o resultado das threads
            long teste = 0;
            for (int x = 0; x < tamanho; x++)
            {
                v1[x] = rnd.Next(11);
                v2[x] = rnd.Next(11);
                teste += v1[x] + v2[x];
            }
            
            Console.WriteLine("Informe o número de threads: ");
            //int threads = int.Parse(Console.ReadLine());
            int threads = Environment.ProcessorCount;

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
            for (int x = 0; x < threads; x++)
            {
                // quando passamos os limites diretamente temos erro de índice
                inicio = limite[x];
                fim = limite[x + 1];
                tarefas[x] = Task.Run(() => Soma(v1, v2, inicio, fim));
                // sem esse wait temos erro nas somas - não sei o porquê...
                tarefas[x].Wait(1);
            }

            // "garante" que todas as threads vão terminar antes do programa sair
            Task.WaitAll(tarefas);

            Console.WriteLine($"\n\nteste = { teste }");
            Console.WriteLine($"total = { total }");
        }

        private static void Soma(long[] v1, long[] v2, long start, long end)
        {            
            for (long y = start; y < end; y++)
            {
                // todas as threads modificam soma
                // é necessário garantir que elas não o façam ao mesmo tempo
                Interlocked.Add(ref total, v1[y] + v2[y]);
                // Console.Write($"Thread={Thread.CurrentThread.ManagedThreadId} ");
            }
        }
    }
}
