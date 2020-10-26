/*
    Faça um programa que, dado um diretório com arquivos de texto no formato .txt,
    calcule as seguintes estatísticas para cada arquivo. Número de palavras (?),
    número de vogais (ok), número de consoantes (ok), palavra que apareceu mais vezes no arquivo (?),
    vogal mais frequente (ok), consoantes mais frequente (ok). Além disso, para cada arquivo do diretório,
    o programa deverá gerar um novo arquivo, contendo o conteúdo do arquivo original escrito em letras maiúsculas (?). 
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

            // char[] separadores = { ' ', ',', '.', ':', '!', '?', '\t', '\n' };

            // string[][] words = new string[textos.Length][];

            // var dicPalavras = new Dictionary<string, int>();            

            Thread[] threads = new Thread[arquivos.Length];

            // usamos dois dicionários para guardar vogais e consoantes
            var vogaisDict = new Dictionary<char, int>()
            {
                ['a'] = 0, ['e'] = 0, ['i'] = 0, ['o'] = 0, ['u'] = 0
            };
            var consoantesDict = new Dictionary<char, int>();           

            for (int i = 0; i < arquivos.Length; i++)
            {
                threads[i] = new Thread(() => 
                {
                    foreach (var caractere in textos[i])
                    {
                        // caso o caractere esteja contido no dicionário de vogais acrescentamos um no campo com a chave equivalente
                        // caso não esteja verificamos se está no intervalo entre 'a' e 'z'
                        // não precisamos nos preocupar no else se é vogal ou não porque já teria entrado no if inicial
                        if (vogaisDict.ContainsKey(caractere))
                        {
                            vogaisDict[caractere]++;
                        }
                        else if (caractere >= 'a' && caractere <= 'z')
                        {
                            if (consoantesDict.ContainsKey(caractere))
                            {
                                consoantesDict[caractere]++;
                            }
                            else
                            {
                                consoantesDict[caractere] = 1;
                            }                            
                        }                       
                       

                        /*foreach (var texto in textos)
                        {
                            foreach (var palavra in texto.Split(separadores))
                            {
                                Console.WriteLine(palavra);
                            }
                        }*/
                    }
                });
                threads[i].Start();
                threads[i].Join();
            }

            int vogais = -1, consoantes = -1;
            char vogalMaisRepetida = ' ', consoanteMaisRepetida = ' ';            

            foreach (var vogal in vogaisDict)
            {
                Console.WriteLine($"chave {vogal.Key} valor {vogal.Value}");                
                if (vogal.Value > vogais)
                {
                    vogais = vogal.Value;
                    vogalMaisRepetida = vogal.Key;
                }
            }

            foreach (var consoante in consoantesDict)
            {
                Console.WriteLine($"chave {consoante.Key} valor {consoante.Value}");
                if (consoante.Value > consoantes)
                {
                    consoantes = consoante.Value;
                    consoanteMaisRepetida = consoante.Key;
                }
            }

            Console.WriteLine($"\nTotal de vogais:" +
                $" { vogaisDict.Sum(x => x.Value) }" +
                $"\nVogal mais repetida: '{ vogalMaisRepetida }'" +
                $" - { vogais } repetições");

            Console.WriteLine($"\nTotal de consoantes:" +
                $" { consoantesDict.Sum(x => x.Value) }" +
                $"\nConsoante mais repetida: '{ consoanteMaisRepetida }'" +
                $" - { consoantes } repetições");

        }
    }
}

