using System.Collections.Generic;

namespace AlgoSubsMemo.Classes
{
    public class Processo
    {
        public int Molduras { get; set; }
        public int NumeroPaginas { get; set; }
        public string Paginas { get; set; }

        public Processo(int molduras, int numeroPaginas, string paginas)
        {
            Molduras = molduras;
            NumeroPaginas = numeroPaginas;
            Paginas = paginas;
        }

        public override string ToString()
        {
            return $"Molduras: { Molduras }\n" +
                   $"Número de Páginas: { NumeroPaginas }\n" +
                   $"Páginas: { Paginas }\n";
        }
    }
}