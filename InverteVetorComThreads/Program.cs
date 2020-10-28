/*
   Faça um programa que inverta a ordem dos elementos de um vetor
   de inteiros com N valores. (ok) Por exemplo, se o vetor contiver
   os elementos 1, 2, 3, 4, 5 o vetor de saída deverá ser 5, 4, 3, 2, 1.
   O tamanho do vetor e o número de threads devem ser informados pelo
   usuário. (ok) Os elementos do vetor devem ser gerados de forma aleatória
   pelo programa. (ok) O programa deverá imprimir na tela o vetor de entrada 
   e o vetor de saída. (ok)
*/

using System;
using System.Threading;

namespace InverteVetorComThreads
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Tamanho do vetor: ");
            int tamanho = int.Parse(Console.ReadLine());

            int[] v1 = new int[tamanho];
            int[] v2 = new int[tamanho];

            Random rnd = new Random();

            PreencheVetor(v1, rnd);            

            Console.Write("\nInforme o número de threads: ");
            int numThreads = int.Parse(Console.ReadLine());
            
            // tratando um caso que não faria sentido
            if (numThreads > tamanho)
            {
                numThreads = tamanho;
            }

            // qual o tamanho de cada 'pedaço' do vetor original
            int diferenca = tamanho / numThreads;

            // guardamos os limites (índices) dos vetores
            // esses valores são usados pelas threads
            int[] limite = new int[numThreads + 1];
            for (int x = 0; x <= numThreads; x++)
            {
                limite[x] = diferenca * x;
            }

            // tratando o caso quando o número de threads não é múltiplo do tamanho do vetor
            limite[numThreads] += tamanho % numThreads;

            Thread[] threads = new Thread[numThreads];
            int inicio, fim;
                      
            for (int x = 0; x < numThreads; x++)
            {
                // quando passamos os limites diretamente podemos ter erro de índice
                // porque funções lambda pegam uma referência à variável (e esta varia com 'x')
                inicio = limite[x];
                fim = limite[x + 1];               

                threads[x] = new Thread(() => InverteVetor(v1, v2, inicio, fim));
                
                threads[x].Start();
                // garante que todas as tarefas vão ser terminadas
                threads[x].Join();
            }

            Console.Write("\nVetor Original: ");
            MostraVetor(v1);
            Console.Write("\nVetor Invertido: ");
            MostraVetor(v2);            
        }

        private static void PreencheVetor(int[] v1, Random rnd)
        {
            // preenchendo os vetores com números randômicos (1 a 9)           
            for (int x = 0; x < v1.Length; x++)
            {
                v1[x] = rnd.Next(1, 10);
            }
        }

        private static void InverteVetor(int[] v1, int[] v2, int inicio, int fim)
        {
            // Console.WriteLine($"\nThread={Thread.CurrentThread.ManagedThreadId}\n");
            int ajuste = 0;
            // o cálculo para acertar a posição do v2 precisou ser modificado
            // acredito que usando a variável 'início' do que 'x' fica mais claro para entender
            for (int x = inicio; x < fim; x++)
            {
                v2[v2.Length - inicio - ajuste - 1] = v1[x];
                ajuste++;
            }
        }

        private static void MostraVetor(int[] v)
        {
            Console.WriteLine();
            for (int x = 0; x < v.Length; x++)
            {
                Console.Write($"{v[x]} ");
            }
            Console.WriteLine();
        }
    }
}