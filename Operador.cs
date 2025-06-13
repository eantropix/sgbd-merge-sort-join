using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SortMergeJoin
{
    public class Operador
    {
        public int TotalIO = 0;
        public int TotalTup = 0;
        private readonly string caminhoTabela1;
        private readonly string caminhoTabela2;
        private readonly string chave1;
        private readonly string chave2;
        private readonly string caminhoSaida;
        private MergeSortExternoTabelas sorter1;
        private MergeSortExternoTabelas sorter2;

        public Operador(string caminhoTabela1, string caminhoTabela2, string chave1, string chave2, string caminhoSaida = "resultado_join.csv")
        {
            this.caminhoTabela1 = caminhoTabela1;
            this.caminhoTabela2 = caminhoTabela2;
            this.chave1 = chave1;
            this.chave2 = chave2;
            this.caminhoSaida = caminhoSaida;
            this.sorter1 = new MergeSortExternoTabelas();
            this.sorter2 = new MergeSortExternoTabelas();
        }

        public void Executar()
        {
            using (var reader1 = new StreamReader(caminhoTabela1))
            using (var reader2 = new StreamReader(caminhoTabela2))
            {
                var header1 = reader1.ReadLine().Split(',');
                var header2 = reader2.ReadLine().Split(',');

                int index1 = Array.IndexOf(header1, chave1);
                int index2 = Array.IndexOf(header2, chave2);

                if (index1 == -1 || index2 == -1)
                    throw new ArgumentException("Chave não encontrada em um dos arquivos.");

                var tabela_1 = new Tabela(caminhoTabela1, index1, true);
                var tabela_2 = new Tabela(caminhoTabela2, index2, true);
                TotalIO += sorter1.Ordenar(tabela_1, "tabela_1_ordenada.csv");
                TotalIO += sorter2.Ordenar(tabela_2, "tabela_2_ordenada.csv");
            }

            using (var reader1 = new StreamReader("tabela_1_ordenada.csv"))
            using (var reader2 = new StreamReader("tabela_2_ordenada.csv"))
            using (var writer = new StreamWriter(caminhoSaida))
            {
                var header1 = reader1.ReadLine().Split(',');
                var header2 = reader2.ReadLine().Split(',');

                int index1 = Array.IndexOf(header1, chave1);
                int index2 = Array.IndexOf(header2, chave2);

                if (index1 == -1 || index2 == -1)
                    throw new ArgumentException("Chave não encontrada em um dos arquivos.");

                writer.WriteLine(string.Join(",", header1.Concat(header2)));

                string linha1 = reader1.ReadLine();
                string linha2 = reader2.ReadLine();

                string[] tupla1 = linha1?.Split(',');
                string[] tupla2 = linha2?.Split(',');

                while (linha1 != null && linha2 != null)
                {
                    string chaveVal1 = tupla1[index1];
                    string chaveVal2 = tupla2[index2];

                    int comp = string.Compare(chaveVal1, chaveVal2, StringComparison.Ordinal);

                    if (comp < 0)
                    {
                        linha1 = reader1.ReadLine();
                        tupla1 = linha1?.Split(',');
                        TotalIO++;
                    }
                    else if (comp > 0)
                    {
                        linha2 = reader2.ReadLine();
                        tupla2 = linha2?.Split(',');
                        TotalIO++;
                    }
                    else
                    {
                        var chaveCorrente = chaveVal1;
                        var buffer = new List<string[]>();

                        while (linha2 != null && tupla2[index2] == chaveCorrente)
                        {
                            buffer.Add(tupla2);
                            linha2 = reader2.ReadLine();
                            tupla2 = linha2?.Split(',');
                            TotalIO++;
                        }

                        foreach (var b in buffer)
                        {
                            writer.WriteLine(string.Join(",", tupla1.Concat(b)));
                            TotalIO++;
                            TotalTup++;
                        }

                        linha1 = reader1.ReadLine();
                        TotalIO++;
                        tupla1 = linha1?.Split(',');
                    }
                }
            }
        }

        public int NumIOExecutados()
        {
            return TotalIO;
        }

        public int NumTuplasGeradas()
        {
            return TotalTup;
        }
    }
}