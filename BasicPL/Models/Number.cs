using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BasicPL.Models
{
    public class Number
    {
        public dynamic Value;
        public Position Start, End;
        public Context context;

        public Number(dynamic value)
        {
            if (value is Number)
                Value = value.Value;
            else
                Value = value;

            context = new Context("NULL CONTEXT");
        }

        public Number AddBy(dynamic other)
        {
            return new Number(Value + other).SetContext(context);
        }

        public Number SubBy(dynamic other)
        {
            return new Number(Value - other).SetContext(context);
        }

        public Number MulBy(dynamic other)
        {
            return new Number(Value * other).SetContext(context);
        }

        public Number DivBy(dynamic other)
        {
            try
            {
                return new Number(Value / other).SetContext(context);
            }
            catch (DivideByZeroException ex)
            {
                throw new RuntimeException(ex.Message, Start, End, context);
            }

        }

        public Number PowBy(dynamic other)
        {
            var ret = MathF.Pow(Value, other);
            return new Number(ret).SetContext(context);
        }

        public Number SetPos(Position start, Position end)
        {
            Start = start;
            End = end;
            return this;
        }

        public override string ToString()
        {
            return $"{Value}";
        }

        public Number SetContext(Context ctx)
        {
            context = ctx;
            return this;
        }

        public Number GetCompEQ(dynamic value)
        {
            return new Number(Value == value ? 1 : 0).SetContext(context);
        }

        public Number GetCompNE(dynamic value)
        {
            return new Number(Value != value ? 1 : 0).SetContext(context);
        }

        public Number GetCompLT(dynamic value)
        {
            return new Number(Value < value ? 1 : 0).SetContext(context);
        }

        public Number GetCompGT(dynamic value)
        {
            return new Number(Value > value ? 1 : 0).SetContext(context);
        }

        public Number GetCompLTE(dynamic value)
        {
            return new Number(Value <= value ? 1 : 0).SetContext(context);
        }

        public Number GetCompGTE(dynamic value)
        {
            return new Number(Value >= value ? 1 : 0).SetContext(context);
        }

        public Number AndBy(dynamic value)
        {
            return new Number((Value == 1 ? true : false) && (value == 1 ? true : false) ? 1 : 0).SetContext(context);
        }

        public Number OrBy(dynamic value)
        {
            return new Number((Value == 1 ? true : false) || (value == 1 ? true : false) ? 1 : 0).SetContext(context);
        }

        public Number Not()
        {
            return new Number(Value == 0 ? 1 : 0).SetContext(context);
        }
    }
}
