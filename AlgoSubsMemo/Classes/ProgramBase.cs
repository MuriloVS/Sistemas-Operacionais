using System;
using System.Collections.Generic;
using System.IO;

namespace AlgoSubsMemo.Classes
{
    internal class ProgramBase
    {
        public static List<string> linhas = new List<string>();        
        public static List<Processo> processos;

        // método que lê o arquivo de entrada
        public static void LeArquivo()
        {
            // Console.Write("Informe o caminho completo do arquivo: ");
            // string caminho = Console.ReadLine();
            string caminho1 = "../../../processos.txt"; // Visual Studio 2019
            string caminho2 = "processos.txt"; // VS Code
            bool sucesso = true;

            try
            {
                StreamReader sr = new StreamReader(caminho1);

                string linha;

                while ((linha = sr.ReadLine()) != null)
                {
                    linhas.Add(linha);
                }
            }
            catch (Exception f)
            {
                sucesso = false;
            }

            if (!sucesso)
            {
                try
                {
                    StreamReader sr = new StreamReader(caminho2);

                    string linha;

                    while ((linha = sr.ReadLine()) != null)
                    {
                        linhas.Add(linha);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        // cria processos a partir do arquivo de entrada
        public static void CriaProcessos()
        {
            string[] baseProcessos = new string[linhas.Count];

            for (int i = 0; i < linhas.Count; i++)
            {
                baseProcessos[i] = linhas[i];
            }

            // criando os processos - cada linha é dividida e os palavras são os parêmetros da chamada da função
            processos = new List<Processo>(linhas.Count);
            char separador = '|';
            string[] temp;

            foreach (var linha in linhas)
            {
                temp = linha.Split(separador);

                processos.Add(new Processo(int.Parse(temp[0]), int.Parse(temp[1]), temp[2]));
            }
        }

        public static void MostraProcessos()
        {
            foreach (var processo in processos)
            {
                Console.WriteLine(processo.ToString());
            }
        }
    }
}
