using System;
using System.Collections.Generic;

namespace SortMergeJoin
{
    public class Operador
    {
        private Tabela T1 { get; set; }
        private Tabela T2 { get; set; }
        private string Key1 { get; set; }
        private string Key2 { get; set; }

        public Operador(Tabela t1, Tabela t2, string key1, string key2)
        {
            T1 = t1;
            T2 = t2;
            Key1 = key1;
            Key2 = key2;
        }
    }
}