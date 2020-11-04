/*
    Faça um programa que multiplique duas matrizes A e B, cujas dimensões são
    MxN e NxP, onde M pode ou não ser igual a P. (ok) O tamanho das  matrizes (ok)
    e o número de threads (ok) devem ser informados pelo usuário. Os valores das
    matrizes devem ser gerados de forma aleatória (ok) pelo programa. O programa 
    deverá imprimir na tela as matrizes A e B bem como o resultado da sua multiplicação. (ok)
 */

using System;
using System.Threading;

namespace MatrizesComThreads
{
    class Program
    {
        static void Main(string[] args)
        {
            string entrada;

            int linA;
            do
            {
                Console.Write("Número de linhas da matriz A: ");
                entrada = Console.ReadLine();
            } while (!int.TryParse(entrada, out linA) || linA <= 0);

            Console.WriteLine();

            int colALinB;
            do
            {
                Console.Write("Número de colunas da matriz A. Este valor também será o número de linhas da matriz B: ");
                entrada = Console.ReadLine();
            } while (!int.TryParse(entrada, out colALinB) || colALinB <= 0);

            Console.WriteLine();

            int colB;
            do
            {
                Console.Write("Número de linhas da colunas da matriz B: ");
                entrada = Console.ReadLine();
            } while (!int.TryParse(entrada, out colB) || colB <= 0);
           

            // a matriz C vai guardar o resultado da multiplicação
            var matA = new Matrix(linA, colALinB, 'A');
            var matB = new Matrix(colALinB, colB, 'B');
            var matC = new Matrix(matA.Linha, matB.Coluna, 'C');

            Random rnd = new Random();
            
            matA.PreencheMatriz(matA, rnd);
            matB.PreencheMatriz(matB, rnd);

            Console.WriteLine();

            int numThreads;
            do
            {
                Console.Write("Número de Threads: ");
                entrada = Console.ReadLine();
            } while (!int.TryParse(entrada, out numThreads) || numThreads <= 0);

            // tratando uma condição que não faria sentido
            if (numThreads > matA.Linha)
            {
                numThreads = matA.Linha;
            }

            Thread[] threads = new Thread[numThreads];           

            // assim como no exercício do vetor eu preferi guardar os intervalos antes para facilitar a legibilidade
            int[] limites = new int[numThreads + 1];
            DefineLimites(limites, matA.Linha, numThreads);            
           
            int inicio, fim, x = 0;

            // criamos threads que fazem a multiplicação de linhas por colunas de acordo com o número de threads
            // exemplo: se temos 6 linhas na matriz A e 3 threads, cada thread fará a operção de duas linhas
            // olhando a função de multiplicação fica mais fácil de entender
            for (int i = 0; i < numThreads; i++)
            {
                inicio = limites[i];
                fim = limites[i + 1];                

                threads[i] = new Thread(()=> MultiplicaMatriz(matA, matB, matC, inicio, fim));
                threads[i].Start();
                threads[i].Join();

                x++;
            }

            Console.WriteLine();
            matA.MostraMatriz();
            matB.MostraMatriz();
            matC.MostraMatriz();           
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
            limite[limite.Length - 1] += resto;

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
            for (int i = 0; i < limite.Length; i++)
            {
                Console.Write(limite[i] + " ");
            }
            Console.WriteLine();
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
            for (int i = 0; i < limite.Length; i++)
            {
                Console.Write(limite[i] + " ");
            }
        }

        public static void MultiplicaMatriz(Matrix A, Matrix B, Matrix C, int inicio, int fim)
        {
            for (int linha = inicio; linha < fim; linha++)
            {
                for (int coluna = 0; coluna < B.Coluna; coluna++)
                {
                    for (int i = 0; i < A.Coluna; i++)
                    {
                        C.Matriz[linha, coluna] += A.Matriz[linha, i] * B.Matriz[i, coluna];
                    }
                }
            }
        }

        public struct Matrix
        {
            // construtor deste scruct
            public Matrix(int linha, int coluna, char nome)
            {                
                Linha = linha;
                Coluna = coluna;
                Nome = nome;
                Matriz = new int[Linha, Coluna];
            }

            public int Linha;
            public int Coluna;
            public char Nome;
            public int[,] Matriz;

            public void MostraMatriz()
            {
                Console.WriteLine($"{ Nome }");
                for (int i = 0; i < Linha; i++)
                {
                    for (int j = 0; j < Coluna; j++)
                    {
                        Console.Write($"{ Matriz[i, j] } ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            public void PreencheMatriz(Matrix Z, Random rnd)
            {
                for (int i = 0; i < Z.Linha; i++)
                {
                    for (int j = 0; j < Z.Coluna; j++)
                    {
                        Z.Matriz[i, j] = rnd.Next(1, 10);
                    }
                }
            }
        }
    }
}
