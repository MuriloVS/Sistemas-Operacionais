/*
    Faça um programa que multiplique duas matrizes A e B,
    cujos dimensões são MxN e NxP, onde M pode ou não ser igual a P. (ok)
    O tamanho das matrizes (ok) e o número de threads (?) devem ser informados pelo usuário.
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

            /* ainda não tem utilidade
            int numThreads;
            do
            {
                Console.Write("\nNúmero de Threads (deve ser  múltiplo do número de linhas da matriz A): ");
                numThreads = int.Parse(Console.ReadLine());
            } while (linA % numThreads != 0);*/
            

            var matA = new Matrix(linA, colALinB);
            var matB = new Matrix(colALinB, colB);
           
            PreencheMatriz(matA);
            PreencheMatriz(matB);
            matA.MostraMatriz();
            matB.MostraMatriz();
            var matC = new Matrix(matA.linha, matB.coluna);
            MultiplicaMatriz(matA, matB, matC);
            matC.MostraMatriz();
           
        }

        public static void MultiplicaMatriz(Matrix A, Matrix B, Matrix C)
        {
            for (int linha = 0; linha < A.linha; linha++)
            {
                for (int coluna = 0; coluna < B.coluna; coluna++)
                {                   
                    Thread t = new Thread(() =>
                    {
                        for (int i = 0; i < A.coluna; i++)
                        {
                            C.matriz[linha, coluna] += A.matriz[linha, i] * B.matriz[i, coluna];
                        }
                    });

                    t.Start();
                    t.Join();
                }
            }
        }

        public static void PreencheMatriz(Matrix Z)
        {
            Random rnd = new Random();

            for (int i = 0; i < Z.linha; i++)
            {
                for (int j = 0; j < Z.coluna; j++)
                {
                    Z.matriz[i, j] = rnd.Next(1, 10);
                }
            }
        }


        public struct Matrix
        {
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
        }
    }
}
