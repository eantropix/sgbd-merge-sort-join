using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SortMergeJoin
{
    public class MergeSortExternoTabelas
    {
        public int TotalIO = 0;

        public MergeSortExternoTabelas(){}

        public int Ordenar(Tabela tabela, string caminhoSaida)
        {
            int tamanho = tabela.Tamanho();
            if (tamanho <= 1) return 0;

            string arquivoOrdenado = MergeSort(tabela, 0, tamanho - 1);

            using (var reader = new StreamReader(arquivoOrdenado))
            using (var writer = new StreamWriter(caminhoSaida))
            {
                tabela.EscreverCabecalho(writer);
                while (!reader.EndOfStream)
                {
                    writer.WriteLine(reader.ReadLine());
                }
            }

            File.Delete(arquivoOrdenado);

            return TotalIO;
        }

        private string MergeSort(Tabela tabela, int inicio, int fim)
        {
            if (inicio == fim)
            {
                return GetPartialFile(tabela, inicio, fim);
            }

            int meio = (inicio + fim) / 2;

            string leftFile = MergeSort(tabela, inicio, meio);
            string rightFile = MergeSort(tabela, meio + 1, fim);

            string mergedFile = Merge(tabela, leftFile, rightFile);
            
            File.Delete(leftFile);
            File.Delete(rightFile);

            return mergedFile;
        }

        private string Merge(Tabela tabela, string leftFile, string rightFile)
        {
            string outputFile = Path.GetTempFileName();

            using (StreamWriter sw = new StreamWriter(outputFile))
            using (StreamReader leftReader = new StreamReader(leftFile))
            using (StreamReader rightReader = new StreamReader(rightFile))
            {
                Tupla left = ReadNextTupla(leftReader, tabela);
                Tupla right = ReadNextTupla(rightReader, tabela);

                while (left != null && right != null)
                {
                    if (CompararTuplas(left, right) <= 0)
                    {
                        Tabela.EscreverTupla(sw, left);
                        left = ReadNextTupla(leftReader, tabela);
                        TotalIO += 2;
                    }
                    else
                    {
                        Tabela.EscreverTupla(sw, right);
                        right = ReadNextTupla(rightReader, tabela);
                        TotalIO += 2;
                    }
                }

                while (left != null)
                {
                    Tabela.EscreverTupla(sw, left);
                    left = ReadNextTupla(leftReader, tabela);
                    TotalIO += 2;
                }

                while (right != null)
                {
                    Tabela.EscreverTupla(sw, right);
                    right = ReadNextTupla(rightReader, tabela);
                    TotalIO += 2;
                }
            }

            return outputFile;
        }

        private int CompararTuplas(Tupla a, Tupla b)
        {
            return string.Compare(a.Chave, b.Chave, StringComparison.Ordinal);
        }

        private Tupla ReadNextTupla(StreamReader reader, Tabela tabela)
        {
            string line = reader.ReadLine();
            if (line == null) return null;
            TotalIO++;
            var valores = line.Split(new[] { tabela.separador }, StringSplitOptions.None);
            return new Tupla(valores, tabela.indiceChave);
        }

        private string GetPartialFile(Tabela tabela, int inicio, int fim)
        {
            string partialFile = Path.GetTempFileName();

            using (StreamWriter sw = new StreamWriter(partialFile))
            {
                for (int i = inicio; i <= fim; i++)
                {
                    Tupla tupla = tabela.LerElemento(i);
                    Tabela.EscreverTupla(sw, tupla);
                    TotalIO += 2;
                }
            }

            return partialFile;
        }
    }
}