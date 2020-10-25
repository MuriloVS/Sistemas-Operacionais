/*
    Faça um programa que multiplique duas matrizes A e B,
    cujos dimensões são MxN e NxP, onde M pode ou não ser igual a P. (ok)
    O tamanho das matrizes (ok) e o número de threads (ok) devem ser informados pelo usuário.
    Os valores das matrizes devem ser gerados de forma aleatória (ok)  pelo programa.
    O programa deverá imprimir na tela as matrizes A e B bem como o resultado da sua multiplicação. (ok)
 */

using System;
using System.Threading;

namespace MatrizesComThreads
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Número de linhas da matriz A: ");
            int linA = int.Parse(Console.ReadLine());

            Console.Write("\nNúmero de colunas da matriz A. Este valor também será o número de linhas da mastriz B: ");
            int colALinB = int.Parse(Console.ReadLine());

            Console.Write("\nNúmero de linhas da colunas da matriz B: ");
            int colB = int.Parse(Console.ReadLine());

            
            int numThreads;
            do
            {
                Console.Write("\nNúmero de Threads (deve ser  múltiplo do número de linhas da matriz A): ");
                numThreads = int.Parse(Console.ReadLine());
            } while (linA % numThreads != 0);

            Thread[] threads = new Thread[numThreads];

            // a matriz C vai guardar o resultado da multiplicação
            var matA = new Matrix(linA, colALinB);
            var matB = new Matrix(colALinB, colB);
            var matC = new Matrix(matA.linha, matB.coluna);

            Random rnd = new Random();
            // necessário passar o random para o scruct, não é possível criar a variável dentro dele
            matA.PreencheMatriz(matA, rnd);
            matB.PreencheMatriz(matB, rnd);

            // assim como no exercício do vetor eu preferi guardar os intervalos antes para facilitar a legibilidade
            int[] limites = new int[numThreads + 1];
            for (int i = 0; i < limites.Length; i++)
            {
                limites[i] = matA.linha / numThreads * i;
            }

            int inicio, fim, x = 0;

            // criamos threads que fazem a multiplicação de uma linha por uma coluna (varia conforme o número de threads)
            for (int i = 0; i < matA.linha; i += (matA.linha / numThreads))
            {                
                inicio = limites[x];
                fim = limites[x + 1];

                threads[x] = new Thread(()=> MultiplicaMatriz(matA, matB, matC, inicio, fim));
                threads[x].Start();
                threads[x].Join();

                x++;
            }

            matA.MostraMatriz();
            matB.MostraMatriz();
            matC.MostraMatriz();           
        }

        public static void MultiplicaMatriz(Matrix A, Matrix B, Matrix C, int inicio, int fim)
        {
            for (int linha = inicio; linha < fim; linha++)
            {
                for (int coluna = 0; coluna < B.coluna; coluna++)
                {
                    for (int i = 0; i < A.coluna; i++)
                    {
                        C.matriz[linha, coluna] += A.matriz[linha, i] * B.matriz[i, coluna];
                    }
                }
            }
        }


        public struct Matrix
        {
            // construtor deste scruct
            public Matrix(int linha, int coluna)
            {
                this.linha = linha;
                this.coluna = coluna;
                matriz = new int[this.linha, this.coluna];
            }

            public int linha;
            public int coluna;
            public int[,] matriz;

            public void MostraMatriz()
            {
                Console.WriteLine();
                for (int i = 0; i < linha; i++)
                {
                    for (int j = 0; j < coluna; j++)
                    {
                        Console.Write($"{matriz[i, j]} ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            public void PreencheMatriz(Matrix Z, Random rnd)
            {
                for (int i = 0; i < Z.linha; i++)
                {
                    for (int j = 0; j < Z.coluna; j++)
                    {
                        Z.matriz[i, j] = rnd.Next(1, 10);
                    }
                }
            }
        }
    }
}
