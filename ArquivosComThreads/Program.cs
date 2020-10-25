/*
    Faça um programa que, dado um diretório com arquivos de texto no formato .txt,
    calcule as seguintes estatísticas para cada arquivo. Número de palavras,
    número de vogais (ok), número de consoantes (ok), palavra que apareceu mais vezes no arquivo,
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
            // pasta criada com alguns arquivos txt e textos de alguns sites neles
            string[] arquivos = Directory.GetFiles(@"c:\temp\", "*.txt");
            string[] textos = new string[arquivos.Length];
            

            // lendo o texto de cada arquivo
            for (int i = 0; i < arquivos.Length; i++)
            {
                StreamReader sr = new StreamReader(arquivos[i]);
                textos[i] = sr.ReadToEnd().Trim().ToLower();
            }

            char[] separadores = { ' ', ',', '.', ':', '!', '?', '\t', '\n' };

            string[][] words = new string[textos.Length][];
                        
            // var dicPalavras = new Dictionary<string, int>();            

            Thread[] threads = new Thread[arquivos.Length];

            char[] vogaisVetor = new char[] { 'a', 'e', 'i', 'o', 'u' };
            int vogais = 0, consoantes = 0;

            for (int i = 0; i < arquivos.Length; i++)
            {
                threads[i] = new Thread(() => 
                {
                    foreach (var caractere in textos[i])
                    {
                        Console.WriteLine(caractere);
                        if (vogaisVetor.Contains(caractere))
                        {
                            vogais++;
                        }
                        else if (caractere >= 'a' && caractere <= 'z')
                        {
                            consoantes++;
                        }

                        foreach (var texto in textos)
                        {
                            foreach (var palavra in texto.Split(separadores))
                            {
                                Console.WriteLine(palavra);
                            }
                        }
                    }
                });
                threads[i].Start();
                threads[i].Join();
            }          
            
            Console.WriteLine($"Total de vogais: { vogais }");
            Console.WriteLine($"Total de consoantes: {consoantes}");


            
        }
    }
}
