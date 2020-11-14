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
using System.Collections.Generic;
using System.Threading;

namespace ConcorrenciaSincronizacao
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();

            // somente um cliente foi criado para que fosse mais fácil sobrecarregar as operações das threads
            // o saldo inicial na conta do cliente fica entre 100 e 400
            Cliente cliente = new Cliente(1, rnd.Next(100, 401));

            Console.WriteLine($"Saldo inicial na conta: ");
            cliente.MostraSaldo();
            Console.WriteLine();

            // o número de threads varia cada vez que o programa é executado
            int numThreads = Environment.ProcessorCount + rnd.Next(0, 21);
            Thread[] threads = new Thread[numThreads];


            for (int i = 0; i < numThreads; i++)
            {
                // gera números entre 0 e 2               
                int op = rnd.Next(3);

                if (op == 0)
                {
                    threads[i] = new Thread(() => cliente.Saque(rnd.Next(1, 201)));
                    threads[i].Start();
                }
                else if (op == 1)
                {
                    threads[i] = new Thread(() => cliente.Deposito(rnd.Next(1, 201)));                    
                    threads[i].Start();
                }
                else
                {
                    threads[i] = new Thread(() => cliente.MostraSaldo());
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
        // controle do acesso ao método para mostrar o saldo
        // quando a lista tem algum elemento é porque temos operação de saque/depósito/consulta aguardando
        // o número da thread que inicioou a operação é adicionado no inicio e removido final das operações
        public List<int> Controle = new List<int>();

        public Cliente(int identificador, int saldo)
        {
            Identificador = identificador;
            Saldo = saldo;
        }

        public void MostraSaldo()
        {
            if (Controle.Count > 0)
            {
                try
                {
                    Controle.Add(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} solicitando acesso para mostrar saldo.");                    
                    _mut.WaitOne();
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} obteve acesso." +
                        $" Mostrando o saldo da conta. Saldo = R$ { Saldo.ToString("N2") }" +
                        $"\nThread {Thread.CurrentThread.ManagedThreadId} finalizou a operação.");
                }
                finally
                {
                    Controle.Remove(Thread.CurrentThread.ManagedThreadId);
                    _mut.ReleaseMutex();
                }
            }
            else
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} mostrando o saldo da conta." +
                    $" Saldo = R$ { Saldo.ToString("N2") }." +
                    $"\nThread {Thread.CurrentThread.ManagedThreadId} finalizou a operação.");
            }
        }

        public void Deposito(int valor)
        {            
            try
            {                
                Controle.Add(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} solicitando acesso para realizar um depósito.");
                _mut.WaitOne();
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} obteve acesso." +
                    $" Realizando depósito de R$ { valor.ToString("N2") }.");
                Saldo += valor;
                Console.WriteLine($"Novo saldo = R$ { Saldo.ToString("N2") }");
            }
            finally
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} finalizou a operação.");
                Controle.Remove(Thread.CurrentThread.ManagedThreadId);
                _mut.ReleaseMutex();
            }
        }

        public void Saque(int valor)
        {            
            try
            {                
                Controle.Add(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} solicitando acesso pare realizar um saque.");
                _mut.WaitOne();
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} obteve acesso. " +
                    $"Realizando saque de R$ { valor.ToString("N2") }.");

                if ((Saldo - valor) >= 0)
                {
                    Saldo -= valor;
                    Console.WriteLine($"Novo saldo = R$ { Saldo.ToString("N2") }");
                }
                else
                {
                    Console.WriteLine("ERRO! Saldo insuficiente, operação cancelada.");
                }
            }
            finally
            {                
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} finalizou a operação.");                
                Controle.Remove(Thread.CurrentThread.ManagedThreadId);
                _mut.ReleaseMutex();
            }            
        }
    }
}
