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
            int tamanho;
            string entrada;

            do
            {
                Console.Write("Tamanho do vetor: ");
                entrada = Console.ReadLine();
            } while (!int.TryParse(entrada, out tamanho) || tamanho <= 0);

            Console.WriteLine();

            int[] v1 = new int[tamanho];
            int[] v2 = new int[tamanho];

            Random rnd = new Random();

            PreencheVetor(v1, rnd);

            int numThreads;
            do
            {
                Console.Write("Informe o número de threads: ");
                entrada = Console.ReadLine();
            } while (!int.TryParse(entrada, out numThreads) || numThreads <= 0);
            
            // tratando um caso que não faria sentido
            if (numThreads > tamanho)
            {
                numThreads = tamanho;
            }

            // guardamos os limites (índices) dos vetores
            // esses valores são usados pelas threads
            int[] limite = new int[numThreads + 1];
            DefineLimites(limite, tamanho, numThreads);

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
            // Array.Reverse(v2);
            // MostraVetor(v2);
        }

        private static void DefineLimites(int[] limite, int tamanho, int numThreads)
        {
            // qual o tamanho de cada 'pedaço' do vetor original
            // correções são necessárias caso a divisão não tenha resto zero
            int diferenca = tamanho / numThreads;

            for (int x = 0; x < limite.Length; x++)
            {
                limite[x] = diferenca * x;
            }

            // tratando o caso quando o número de threads não é múltiplo do tamanho do vetor
            // a última posição do vetor fica maior que as outras neste caso, mas garante que tudo será executado
            int resto = tamanho % numThreads;
            limite[^1] += resto;

            Console.WriteLine("Antes dos ajustes");
            MostraVetor(limite);

            // a lógica é começar do penúltimo indíce, aquele com maior distância até o próximo
            // acrescentamos um a cada índice, até o terceiro elemento na primeira iteração
            // na segunda iteração ele para no quarto elemento e assim por diante
            // dessa maneira cada thread trabalha com um intervalo parecido
            // caso o resto seja um, não há porque fazer o ajuste
            // exemplo: vetor com a tamanho 20 e 7 threads - resto 6, cinco iterações
            // 0  2  4  6  8  10  12  20 (original)
            // 0  2  5  7  9  11  13  20 (primeira iteração)
            // 0  2  5  8 10  12  14  20 (segunda iteração)
            // 0  2  5  8 11  13  15  20 (terceira iteração)
            // 0  2  5  8 11  14  16  20 (quarta iteração)
            // 0  2  5  8 11  14  17  20 (quinta iteração)
            // no ínicio o inteverlo mínimo era 2 e o máximo era 8 - ao final temos invervalo mínimo 2 e máximo igual a 3
            if (resto > 1)
            {
                // aux é utilizada para que a cada iteração o 'for 'pare antes a cada iteração (terceiro elemento, quarto elemento etc.)
                int aux = 0;
                while (resto > 1)
                {
                    for (int i = limite.Length - 2; i > (1 + aux); i--)
                    {
                        limite[i] += 1;
                    }
                    resto--;
                    aux++;
                }
            }
            Console.WriteLine("Após os ajustes");
            MostraVetor(limite);
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
            for (int x = inicio; x < fim; x++)
            {
                v2[v2.Length - x - 1] = v1[x];
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