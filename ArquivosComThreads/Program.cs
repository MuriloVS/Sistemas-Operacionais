/*
    Faça um programa que, dado um diretório com arquivos de texto no formato .txt,
    calcule as seguintes estatísticas para cada arquivo. Número de palavras,
    número de vogais, número de consoantes, palavra que apareceu mais vezes no arquivo,
    vogal mais frequente, consoantes mais frequente. Além disso, para cada arquivo do diretório,
    o programa deverá gerar um novo arquivo, contendo o conteúdo do arquivo original escrito em letras maiúsculas. 
*/



using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ArquivoComThreads
{
    class Program
    {
        static void Main(string[] args)
        {
            // string[] separadores = {' ', '.', ':', '!', '?', '\t'};           

            string[] arquivos = Directory.GetFiles(@"c:\temp\", "*.txt");
            string[] textos = new string[arquivos.Length];
            char[] vogais = new char[] { 'a', 'e', 'i', 'o', 'u' };
            Thread[] threads = new Thread[arquivos.Length];

            for (int i = 0; i < arquivos.Length; i++)
            {
                StreamReader sr = new StreamReader(arquivos[i]);
                textos[i] = sr.ReadToEnd();
            }

            int vogal = 0, consoante = 0, inicio;

            for (int i = 0; i < arquivos.Length; i++)
            {
                inicio = i;
                threads[i] = new Thread(() => 
                {
                    foreach (var caractere in textos[inicio].ToLower())
                    {                        
                        if (vogais.Contains(caractere))
                        {
                            vogal++;
                        }
                        else if (caractere >= 'a' && caractere <= 'z')
                        {
                            consoante++;
                        }
                    }
                });
                threads[i].Start();
                threads[i].Join();
            }          
            
            Console.WriteLine($"Total de vogais: { vogal }");
            Console.WriteLine($"Total de consoantes: {consoante}");

        }
    }
}
