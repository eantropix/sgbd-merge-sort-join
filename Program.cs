using System;
using SortMergeJoin;

public class Program
{
    public static void Main(string[] args)
    {
        var op = new Operador("uva.csv", "vinho.csv", "uva_id", "uva_id");
        op.Executar();

        Console.WriteLine($"#IOs: {op.NumIOExecutados()}");
        Console.WriteLine($"#Tups: {op.NumTuplasGeradas()}");
    }
}

