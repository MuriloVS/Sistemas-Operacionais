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
            for (int i = 0; i < limites.Length; i++)
            {
                limites[i] = matA.Linha / numThreads * i;
            }

            // tratando o caso em que o número de threads não é múltiplo no número de linhas de 'A'
            limites[numThreads] += matA.Linha % numThreads;
            int inicio, fim, x = 0;

            // criamos threads que fazem a multiplicação de linhas por colunas de acordo com o número de threads
            // exemplo: se temos 6 linhas na matriz A e 3 threads, cada thread fará a operção de duas linhas
            // olhando a função de multiplicação fica mais fácil de entender
            for (int i = 0; i < numThreads; i++)
            {
                inicio = limites[x];
                fim = limites[x + 1];                

                threads[x] = new Thread(()=> MultiplicaMatriz(matA, matB, matC, inicio, fim));
                threads[x].Start();
                threads[x].Join();

                x++;
            }

            Console.WriteLine();
            matA.MostraMatriz();
            matB.MostraMatriz();
            matC.MostraMatriz();           
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
