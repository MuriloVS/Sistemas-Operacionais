/*
  Tarefa: Você foi contratado pelo C3 (Clube do Capitalismo Compulsivo) para desenvolver
  um sistema de gerenciamento de um banco. Sua principal tarefa é desenvolver um sistema
  que evite que múltiplas operações sejam realizadas em uma mesma conta bancária de forma
  simultânea. Assuma que, para cada conta corrente, você possui tanto o seu identificador (ok)
  como o saldo disponível (ok). 

  Crie diversas threads para simular sequências de operações em paralelo e, aleatoriamente,
  defina qual conta receberá a operação, o tipo de operação (crédito ou débito), e o valor
  da operação. Realize simulações com diferentes números de threads. Após, assuma que existe
  uma nova operação que realiza a consulta do saldo. A principal diferença para esta operação
  é que múltiplas threads podem consultar o saldo de uma conta simultaneamente, desde que
  nenhuma outra thread esteja realizando uma operação de crédito ou débito. Operações de
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
            Cliente cliente = new Cliente(1, rnd.Next(100, 1001));
            cliente.MostraSaldo();
            cliente.Deposita(100);
            cliente.MostraSaldo();
            cliente.Saque(500);
            cliente.MostraSaldo();
        }
    }

    class Cliente
    {
        public int Identificador { get; set; }
        public int Saldo { get; set; }
        private Mutex _mut = new Mutex();

        public Cliente(int identificador, int saldo)
        {
            Identificador = identificador;
            Saldo = saldo;
        }

        public void MostraSaldo()
        {
            Console.WriteLine($"Saldo = R$ { Saldo },00");
        }

        public void Deposita(int valor)
        {
            _mut.WaitOne();
            Saldo += valor;
            _mut.ReleaseMutex();
        }

        public void Saque(int valor)
        {
            _mut.WaitOne();
            if ((Saldo - valor) >= 0M)
            {
                Saldo -= valor;
                _mut.ReleaseMutex();
                return;
            }            

            Console.WriteLine("ERRO! Saldo insuficiente!");
        }
    }
}
