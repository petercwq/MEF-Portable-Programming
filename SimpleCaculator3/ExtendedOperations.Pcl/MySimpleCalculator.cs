using System;
using System.Collections.Generic;
using System.Composition;
using ExtendedInterfaces.Pcl;

namespace ExtendedOperations.Pcl
{
    [Export(typeof(ICalculator))]
    class MySimpleCalculator : ICalculator
    {
        [ImportMany]
        public IEnumerable<IOperation> OperationsTest { get; set; }

        private readonly IEnumerable<IOperation> operations;

        private readonly IMessageHandler handler;

        [ImportingConstructor]
        public MySimpleCalculator([Import(AllowDefault = true)] IMessageHandler handler, [ImportMany] IEnumerable<IOperation> operations)
        {
            this.handler = handler;
            this.operations = operations;
        }

        public String Calculate(String input)
        {
            int left;
            int right;
            Char operation;
            int fn = FindFirstNonDigit(input); //finds the operator
            if (fn < 0) return "Could not parse command.";

            try
            {
                //separate out the operands
                left = int.Parse(input.Substring(0, fn));
                right = int.Parse(input.Substring(fn + 1));
            }
            catch
            {
                return "Could not parse command.";
            }

            operation = input[fn];

            foreach (var i in operations)
            {
                if (i.Symbol.Equals(operation)) return i.Operate(left, right).ToString();
            }
            return "Operation Not Found!";
        }

        private int FindFirstNonDigit(String s)
        {

            for (int i = 0; i < s.Length; i++)
            {
                if (!(Char.IsDigit(s[i]))) return i;
            }
            return -1;
        }
    }
}
