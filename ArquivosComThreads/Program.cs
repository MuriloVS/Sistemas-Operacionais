/*
    Faça um programa que, dado um diretório com arquivos de texto no formato .txt,
    calcule as seguintes estatísticas para cada arquivo. Número de palavras (ok),
    número de vogais (ok), número de consoantes (ok), palavra que apareceu mais vezes no arquivo (ok),
    vogal mais frequente (ok), consoantes mais frequente (ok). Além disso, para cada arquivo do diretório,
    o programa deverá gerar um novo arquivo, contendo o conteúdo do arquivo original escrito em letras maiúsculas (?). 
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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

            Thread[] threads = new Thread[arquivos.Length];

            // usamos dois dicionários para guardar vogais, consoantes e palavras
            var vogaisDict = new Dictionary<char, int>()
            {
                ['a'] = 0, ['e'] = 0, ['i'] = 0, ['o'] = 0, ['u'] = 0
            };
            var consoantesDict = new Dictionary<char, int>();
            var palavrasDict = new Dictionary<string, int>();

            // para separar as palavras
            char[] separadores = { ' ', ',', '.', ':', '!', '?', '\t', '\n', '\r' };

            string[] stringAux;

            // preenchemos os dicionários para depois podemos verificar os dados solicitados
            for (int i = 0; i < arquivos.Length; i++)
            {
                // cada thread trabalha com o texto de um arquivo
                threads[i] = new Thread(() => 
                {   
                    stringAux = textos[i].Split(separadores);
                    for (int j = 0; j < stringAux.Length; j++)
                    {
                        // aqui contamos as palavras - caso ela não esteja no dicionário 
                        // é acrescida como chave o valor inicial definido como 1 - mesma ideia para os caractes, abaixo
                        if (palavrasDict.ContainsKey(stringAux[j]))
                        {
                            palavrasDict[stringAux[j]]++;
                        }
                        else if (stringAux[j] != "")
                        {
                            palavrasDict[stringAux[j]] = 1;
                        }

                        // aqui contamos os caracteres
                        foreach (var caractere in stringAux[j])
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
                        }
                    } 
                });
                threads[i].Start();
                threads[i].Join();
            }

            int vogais = -1, consoantes = -1, palavras = -1;
            char vogalMaisRepetida = ' ', consoanteMaisRepetida = ' ';
            string palavraMaisRepetida = new string(" ");

            foreach (var vogal in vogaisDict)
            {
                //Console.WriteLine($"vogal {vogal.Key} valor {vogal.Value}");                
                if (vogal.Value > vogais)
                {
                    vogais = vogal.Value;
                    vogalMaisRepetida = vogal.Key;
                }
            }

            foreach (var consoante in consoantesDict)
            {
                //Console.WriteLine($"consoante {consoante.Key} valor {consoante.Value}");
                if (consoante.Value > consoantes)
                {
                    consoantes = consoante.Value;
                    consoanteMaisRepetida = consoante.Key;
                }
            }

            foreach (var palavra in palavrasDict)
            {
                Console.WriteLine($"palavra  '{palavra.Key}' valor {palavra.Value}");
                if (palavra.Value > palavras)
                {
                    palavras = palavra.Value;
                    palavraMaisRepetida = palavraMaisRepetida.Replace(palavraMaisRepetida, palavra.Key);
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

            Console.WriteLine($"\nTotal de palavras:" +
                $" { palavrasDict.Sum(x => x.Value) }" +
                $"\nPalavra mais repetida: '{ palavraMaisRepetida }'" +
                $" - { palavras } repetições");

        }
    }
}

