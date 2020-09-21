using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace SomaVetoresThreads
{
    class Program
    {
        private static int total = 0;
        private static int[] v1;
        private static int[] v2;

        static void Main(string[] args)
        {
            Console.WriteLine("Tamanho do vetor: ");
            int tamanho = int.Parse(Console.ReadLine());

            v1 = new int[tamanho];
            v2 = new int[tamanho];

            Random rnd = new Random();

            // preenchendo os vetores com números randômicos
            // e calculando o total para comparar com o resultado das threads
            int teste = 0;
            for (int x = 0; x < tamanho; x++)
            {
                v1[x] = rnd.Next(11);
                v2[x] = rnd.Next(11);
                teste += v1[x] + v2[x];
            }

            // por enquanto o número de threads deve ser múltiplo do tamanho do vetor
            Console.WriteLine("Informe o número de threads: ");
            int threads = int.Parse(Console.ReadLine());

            // qual o tamanho de cada divisão do vetor original
            int diferenca = tamanho / threads;

            // guardamos os limites (índices) dos vetores
            // esses valores são usados pelas threads
            int[] limite = new int[threads + 1];
            for (int x = 0; x <= threads; x++)
            {
                limite[x] = diferenca * x;
            }

            Task[] tarefas = new Task[threads];

            for (int x = 0; x < threads; x++)
            {
                tarefas[x] = Task.Run(() => Soma(v1, v2, limite[x], limite[x + 1]));
                // sem este wait temos erro de indíce (o x utilizao não é do início, mas do final)
                tarefas[x].Wait(1);
            }

            // garante que todas as threads vão terminar antes do programa sair
            Task.WaitAll(tarefas);

            Console.WriteLine($"\n\nteste = { teste }");
            Console.WriteLine($"total = { total }");
        }

        private static void Soma(int[] v1, int[] v2, int start, int end)
        {
            //Console.WriteLine(start);
            for (int y = start; y < end; y++)
            {
                total += v1[y] + v2[y];
                // Console.Write($"Thread={Thread.CurrentThread.ManagedThreadId} ");
            }
        }
    }
}
