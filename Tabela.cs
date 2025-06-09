using System;
using System.Collections.Generic;

namespace SortMergeJoin
{
    public class Tabela
    {
        private readonly int qtd_pags;
        private readonly int qtd_cols;

        public List<Pagina> Pags { get; set; } = new List<Pagina>();
    }
}