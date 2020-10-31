/*
    Faça um programa que, dado um diretório com arquivos de texto no formato .txt,
    calcule as seguintes estatísticas para cada arquivo. Número de palavras (ok),
    número de vogais (ok), número de consoantes (ok), palavra que apareceu mais 
    vezes no arquivo (ok), vogal mais frequente (ok), consoantes mais frequente (ok).
    Além disso, para cada arquivo do diretório, o programa deverá gerar um novo arquivo,
    contendo o conteúdo do arquivo original escrito em letras maiúsculas (ok). 
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
            // pasta criada manulamente com alguns arquivos txt e textos de alguns sites dentro deles
            string diretorio1 = @"c:\temp\";

            // pasta para os arquivos com maiúsculas (evita que eles interfiram quando rodamos o programa novamente)
            string diretorio2 = diretorio1 + @"Upper\";
            
            // cria diretório para guardar os arquivos com maiúsculas
            if (!Directory.Exists(diretorio2))
            {
                Directory.CreateDirectory(diretorio2);
            }
           
            // lê o nome dos arquivos de texto do diretório
            string[] arquivos = Directory.GetFiles(diretorio1, "*.txt");

            if (arquivos.Length == 0)
            {
                Console.WriteLine("Nenhum arquivo .txt encontrado, finalizando o programa.");
                return;
            }

            // cria um vetor de strings para guardar o texto de cada arquivo
            string[] textos = new string[arquivos.Length];
            // e prepara um vetor para guardar os arquivos modificados
            string[] nomeArquivosUpper = new string[arquivos.Length];

            // lendo o texto de cada arquivo e criando os arquivos para a versão com as maiúsculas
            for (int i = 0; i < arquivos.Length; i++)
            {
                StreamReader sr = new StreamReader(arquivos[i]);
                textos[i] = sr.ReadToEnd().Trim().ToUpper();
                nomeArquivosUpper[i] = $"{i+1} - upper.txt";
            }                                

            // cada thread vai trabalhar com um arquivo difente
            Thread[] threads = new Thread[arquivos.Length];

            // usamos dicionários para guardar vogais, consoantes e palavras
            // se a letra não é vogal, guardamos no dicionário de consoantes
            var vogaisDict = new Dictionary<char, int>()
            {
                ['A'] = 0, ['E'] = 0, ['I'] = 0, ['O'] = 0, ['U'] = 0
            };
            var consoantesDict = new Dictionary<char, int>();
            var palavrasDict = new Dictionary<string, int>();

            // caracteres normelmente utilizados para separar as palavras
            char[] separadores = { ' ', ',', '.', ':', '!', '?', '\t', '\n', '\r' };

            // a ideia é utilizar as threads para preencher os dicionários, depois fazemos os cálculos
            for (int i = 0; i < arquivos.Length; i++)
            {
                // cada thread trabalha com o texto de um arquivo
                threads[i] = new Thread(() => 
                {
                    // percorremos as palavras e internamente as letras
                    foreach (var palavra in textos[i].Split(separadores))
                    {
                        ContaPalavras(palavrasDict, palavra);

                        foreach (var caractere in palavra)
                        {
                            ContaLetras(vogaisDict, consoantesDict, caractere);
                        }
                    }

                    // criando e gravando os arquivos com maiúsculas
                    string arquivo = diretorio2 + nomeArquivosUpper[i];
                    File.WriteAllText(arquivo, textos[i]);
                });
                threads[i].Start();
                threads[i].Join();
            }                      
            
            VogalMaisRepetida(vogaisDict);
            ConsoanteMaisRepetida(consoantesDict);
            PalavraMaisRepetida(palavrasDict);
        }

        static void ContaPalavras(Dictionary<string, int> palavrasDict, string palavra)
        {
            // aqui contamos as palavras - caso ela não esteja no dicionário, 
            // é acrescida como chave e o valor inicial definido como 1 - mesma ideia para os caracteres
            if (palavrasDict.ContainsKey(palavra))
            {
                palavrasDict[palavra]++;
            }
            else if (palavra != "")
            {
                palavrasDict[palavra] = 1;
            }
        }

        static void ContaLetras(Dictionary<char, int> vogaisDict,
                                Dictionary<char, int> consoantesDict,
                                char caractere)
        {
            // caso o caractere esteja contido no dicionário de vogais acrescentamos
            // um no campo com a chave equivalente caso não esteja verificamos se está
            // no intervalo entre 'a' e 'z' não precisamos nos preocupar no 'else' se é
            // vogal ou não porque já teria entrado no 'if' inicial
            if (vogaisDict.ContainsKey(caractere))
            {
                vogaisDict[caractere]++;
            }
            else if (caractere >= 'A' && caractere <= 'Z')
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

        // stackoverflow hacks - não sei se valeu a pela, código ficou enxuto mas menos legível nessas funções
        static void VogalMaisRepetida(Dictionary<char, int> vogaisDict)
        {
            Console.WriteLine($"\nTotal de vogais: { vogaisDict.Sum(x => x.Value) }" +
                $"\nVogal mais repetida: '{ vogaisDict.Aggregate((l, r) => l.Value > r.Value ? l : r).Key }'" +
                $" - { vogaisDict.Values.Max() } repetições");
        }

        static void ConsoanteMaisRepetida(Dictionary<char, int> consoantesDict)
        {
            Console.WriteLine($"\nTotal de consoantes: { consoantesDict.Sum(x => x.Value) }" +
                $"\nConsoante mais repetida: '{ consoantesDict.Aggregate((l, r) => l.Value > r.Value ? l : r).Key }'" +
                $" - { consoantesDict.Values.Max() } repetições");
        }

        static void PalavraMaisRepetida(Dictionary<string, int> palavrasDict)
        {
            Console.WriteLine($"\nTotal de palavras: { palavrasDict.Sum(x => x.Value) }" +
                $"\nPalavra mais repetida: '{ palavrasDict.Aggregate((l, r) => l.Value > r.Value ? l : r).Key }'" +
                $" - { palavrasDict.Values.Max() } repetições");
        }
    }
}