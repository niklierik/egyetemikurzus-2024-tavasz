// See https://aka.ms/new-console-template for more information
using Ora3;

Console.WriteLine("Hello, World!");

StrukturaPelda p = new();

Console.WriteLine(p);

List<int> szamok = new List<int>();
szamok.Add(1);
szamok.Add(2);
szamok.Add(3);
szamok.Add(4);

float[] ints = new float[]
{
    1, 2, 3, 4
};

GenericSum.Sum(szamok);
GenericSum.Sum(ints);


checked
{
    try
    {
        Something();
    }
    catch (OverflowException e)
    {
        Console.WriteLine(e.Message);
    }
}

static void Something()
{
    checked
    {
        int asd = int.MaxValue;
        int variable = int.Parse(Console.ReadLine()!);
        Console.WriteLine(asd + variable);
    }
}