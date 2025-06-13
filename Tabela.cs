using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SortMergeJoin
{
    public class Tabela
    {
        private string caminhoArquivo;
        public int indiceChave;
        private bool temCabecalho;
        public string separador;

        public Tabela(string caminho, int indiceChave = 0, bool temCabecalho = false, string separador = ",")
        {
            this.caminhoArquivo = caminho;
            this.indiceChave = indiceChave;
            this.temCabecalho = temCabecalho;
            this.separador = separador;

            if (!File.Exists(caminho))
            {
                throw new FileNotFoundException("Arquivo não encontrado.");
            }
        }

        public Tupla LerElemento(int indice)
        {
            int currentIndex = 0;
            bool cabecalhoPulado = false;

            using (StreamReader sr = new StreamReader(caminhoArquivo))
            {
                if (temCabecalho && !cabecalhoPulado)
                {
                    sr.ReadLine();
                    cabecalhoPulado = true;
                }

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (currentIndex == indice)
                    {
                        var valores = line.Split(new[] { separador }, StringSplitOptions.None);
                        return new Tupla(valores, indiceChave);
                    }
                    currentIndex++;
                }
            }
            throw new IndexOutOfRangeException("Índice fora dos limites da tabela.");
        }

        public int Tamanho()
        {
            int count = 0;
            bool cabecalhoPulado = false;

            using (StreamReader sr = new StreamReader(caminhoArquivo))
            {
                if (temCabecalho && !cabecalhoPulado)
                {
                    sr.ReadLine();
                    cabecalhoPulado = true;
                }

                while (sr.ReadLine() != null)
                {
                    count++;
                }
            }
            return count;
        }

        public static void EscreverTupla(StreamWriter sw, Tupla tupla)
        {
            sw.WriteLine(string.Join(",", tupla.Valores));
        }

        public void EscreverCabecalho(StreamWriter sw)
        {
            if (!temCabecalho) return;

            using (StreamReader sr = new StreamReader(caminhoArquivo))
            {
                string cabecalho = sr.ReadLine();
                sw.WriteLine(cabecalho);
            }
        }
    }
}