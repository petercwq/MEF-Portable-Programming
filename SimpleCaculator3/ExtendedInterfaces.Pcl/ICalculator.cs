using System;

namespace ExtendedInterfaces.Pcl
{
    public interface ICalculator
    {
        String Calculate(String input);
    }

    public interface IOperation
    {
        int Operate(int left, int right);
    }

    public class OperationMetadata
    {
        public char Symbol { get; set; }
    }
}
