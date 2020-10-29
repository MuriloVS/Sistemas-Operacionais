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
            Console.Write("Número de linhas da matriz A: ");
            int linA = int.Parse(Console.ReadLine());

            Console.Write("\nNúmero de colunas da matriz A. Este valor também será o número de linhas da matriz B: ");
            int colALinB = int.Parse(Console.ReadLine());

            Console.Write("\nNúmero de linhas da colunas da matriz B: ");
            int colB = int.Parse(Console.ReadLine());

            // a matriz C vai guardar o resultado da multiplicação
            var matA = new Matrix(linA, colALinB, 'A');
            var matB = new Matrix(colALinB, colB, 'B');
            var matC = new Matrix(matA.linha, matB.coluna, 'C');

            Random rnd = new Random();
            
            matA.PreencheMatriz(matA, rnd);
            matB.PreencheMatriz(matB, rnd);

            Console.Write("\nNúmero de Threads: ");
            int numThreads = int.Parse(Console.ReadLine());

            // tratando uma condição que não faria sentido
            if (numThreads > matA.linha)
            {
                numThreads = matA.linha;
            }

            Thread[] threads = new Thread[numThreads];           

            // assim como no exercício do vetor eu preferi guardar os intervalos antes para facilitar a legibilidade
            int[] limites = new int[numThreads + 1];
            for (int i = 0; i < limites.Length; i++)
            {
                limites[i] = matA.linha / numThreads * i;
            }

            // tratando o caso em que o número de threads não é múltiplo no número de linhas de 'A'
            limites[numThreads] += matA.linha % numThreads;
            int inicio, fim, x = 0;

            // criamos threads que fazem a multiplicação de linhas por colunas de acordo com o número de threads
            // exemplo: se temos 6 linhas na matriz A e 3 threads, cada thread fará a operção de duas linhas
            // olhando a função de multiplicação fica mais fácil de entender
            for (int i = 0; i < matA.linha; i += (matA.linha / numThreads))
            {
                inicio = limites[x];
                fim = limites[x + 1];                

                threads[x] = new Thread(()=> MultiplicaMatriz(matA, matB, matC, inicio, fim));
                threads[x].Start();
                threads[x].Join();

                x++;

                // necessário para evitar que o índice ultrapasse o limite do vetor
                // essa correção é para os casos em que mat.Linha/numThreds for diferente de zero
                if (x == numThreads)
                {
                    break;
                }
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
            public Matrix(int linha, int coluna, char nome)
            {                
                this.linha = linha;
                this.coluna = coluna;
                this.nome = nome;
                matriz = new int[this.linha, this.coluna];
            }

            public int linha;
            public int coluna;
            char nome;
            public int[,] matriz;

            public void MostraMatriz()
            {
                Console.WriteLine($"{nome}");
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
