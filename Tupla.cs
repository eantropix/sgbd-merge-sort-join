using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SortMergeJoin
{
    public class Tupla
    {
        public string[] Valores { get; private set; }
        public string Chave { get; private set; }
        public int IndiceChave { get; private set; }

        public Tupla(string[] valores, int indiceChave)
        {
            Valores = valores;
            IndiceChave = indiceChave;
            Chave = valores[indiceChave];
        }

        public string this[int index] => Valores[index];
    }
}