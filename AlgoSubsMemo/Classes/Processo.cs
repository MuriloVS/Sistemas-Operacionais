using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoSubsMemo.Classes
{
    public class Processo
    {
        public int NumeroMolduras { get; set; }
        public List<int> Molduras { get; set; }
        public int NumeroPaginas { get; set; }
        public List<int> Paginas { get; set; }

        public Processo(int molduras, int numeroPaginas, string paginas)
        {
            NumeroMolduras = molduras;
            Molduras = new List<int>(molduras);
            NumeroPaginas = numeroPaginas;
            Paginas = paginas.Split().Select(Int32.Parse).ToList();
        }

        public override string ToString()
        {
            return $"Molduras: { NumeroMolduras }\n" +
                   $"Número de Páginas: { NumeroPaginas }\n" +
                   $"Páginas: { Paginas.ToString() }\n";
        }
    }
}