using System;

namespace ExtendedInterfaces.Pcl
{
    public interface ICalculator
    {
        String Calculate(String input);
    }

    public interface IOperation
    {
        Char Symbol { get; }
        int Operate(int left, int right);
    }
}
