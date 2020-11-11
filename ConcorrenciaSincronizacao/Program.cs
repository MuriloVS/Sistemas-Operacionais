/*
  Tarefa: Você foi contratado pelo C3 (Clube do Capitalismo Compulsivo) para desenvolver
  um sistema de gerenciamento de um banco. Sua principal tarefa é desenvolver um sistema
  que evite que múltiplas operações sejam realizadas em uma mesma conta bancária de forma
  simultânea. Assuma que, para cada conta corrente, você possui tanto o seu identificador (ok)
  como o saldo disponível (ok). 

  Crie diversas threads para simular sequências de operações em paralelo e, aleatoriamente,
  defina qual conta receberá a operação, o tipo de operação (crédito ou débito), e o valor
  da operação. (ok) Realize simulações com diferentes números de threads. (ok) Após, assuma que existe
  uma nova operação que realiza a consulta do saldo. (ok) A principal diferença para esta operação
  é que múltiplas threads podem consultar o saldo de uma conta simultaneamente, desde que
  nenhuma outra thread esteja realizando uma operação de crédito ou débito. (ok?) Operações de
  débito e crédito continuam precisando de acesso exclusivo aos registros da conta para
  executarem adequadamente. 
*/

using System;
using System.Threading;

namespace ConcorrenciaSincronizacao
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();

            // somente um cliente foi criado para que fosse mais fácil sobrecarregar as operações das threads
            // o saldo inicial na conta do cliente fica entre 100 e 500
            Cliente cliente = new Cliente(1, rnd.Next(100, 501));

            Console.WriteLine($"Saldo inicial na conta: ");
            cliente.MostraSaldo();
            Console.WriteLine();

            // o número de threads varia a cada vez que o programa é executado
            int numThreads = Environment.ProcessorCount + rnd.Next(0, 11);
            Thread[] threads = new Thread[numThreads];

            for (int i = 0; i < numThreads; i++)
            {
                // gera 0 ou 1
                int op = rnd.Next(2);
                
                if (op == 0)
                {                    
                    threads[i] = new Thread(() => cliente.Saque(rnd.Next(200)));
                    threads[i].Start();
                }
                else
                {
                    threads[i] = new Thread(() => cliente.Deposito(rnd.Next(200)));
                    threads[i].Start();
                }
            }

            // garantindo que as threads completem antes de finalizar o programa
            foreach (var thread in threads)
            {
                thread.Join();
            }
            
            Console.WriteLine("\nSaldo final na conta: ");
            cliente.MostraSaldo();
        }    
    }

    class Cliente
    {
        public int Identificador { get; set; }
        public int Saldo { get; set; }
        private readonly Mutex _mut = new Mutex(false, "Teste");

        public Cliente(int identificador, int saldo)
        {
            Identificador = identificador;
            Saldo = saldo;
        }

        public void MostraSaldo()
        {
            Console.WriteLine($"Saldo = R$ { Saldo },00");
        }

        public void Deposito(int valor)
        {
            try
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} solicitando acesso.");
                _mut.WaitOne();
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} obteve acesso. Realizando depósito de R$ {valor},00.");
                Saldo += valor;
                MostraSaldo();
            }
            finally
            {                
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} finalizou a operação.");                
                _mut.ReleaseMutex();                
            }
        }

        public void Saque(int valor)
        {
            try
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} solicitando acesso.");
                _mut.WaitOne();
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} obteve acesso. Realizando saque de R$ {valor},00.");

                if ((Saldo - valor) >= 0)
                {
                    Saldo -= valor;
                    MostraSaldo();
                }
                else
                {
                    Console.WriteLine("ERRO! Saldo insuficiente, operação cancelada.");
                }
            }
            finally
            {  
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} finalizou a operação.");                
                _mut.ReleaseMutex();                
            }
        }
    }
}
