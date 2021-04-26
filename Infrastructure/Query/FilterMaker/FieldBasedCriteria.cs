using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query.FilterMaker
{
    public interface ICriteria
    {
        string Render();
    }
    public abstract class FieldBasedCriteria : ICriteria
    {
        protected FieldBasedCriteria(string fieldName, List<string> filterValues, OperatorEnum @operator)
        {
            this.FieldName = fieldName;
            this.FilterValues = filterValues;
            this.Operator = @operator;
        }
        protected string FieldName { get; private set; }
        protected List<string> FilterValues { get; set; }
        protected OperatorEnum Operator { get; set; }

        public string Render()
        {

            var result = DoRender();
            return result;
        }
        protected abstract string DoRender();
        public abstract void FillParameters(IDbCommand command);


    }

    public sealed class DateCriteria : FieldBasedCriteria
    {
        public DateCriteria(string fieldName, List<string> filterValues, OperatorEnum @operator) : base(fieldName, filterValues, @operator)
        {
        }

        public override void FillParameters(IDbCommand command)
        {
            IDbDataParameter param = null;
            switch (Operator)
            {
                case OperatorEnum.Equals:
                case OperatorEnum.GreaterThan:
                case OperatorEnum.NotEquals:
                case OperatorEnum.LessThan:
                    param = command.CreateParameter();
                    param.ParameterName = $"@{FieldName}";
                    param.Value = DateTime.Parse(FilterValues[0]);
                    param.DbType = DbType.DateTime;
                    command.Parameters.Add(param);
                    break;
                default:
                    //TODO:
                    throw new InvalidOperationException();
            }

        }

        protected override string DoRender()
        {
            string result = string.Empty;
            switch (Operator)
            {
                case OperatorEnum.Equals:
                    result = $"{FieldName}=@{FieldName}";
                    break;
                case OperatorEnum.NotEquals:
                    result = $"{FieldName}<>@{FieldName}";
                    break;
                case OperatorEnum.GreaterThan:
                    result = $"{FieldName}>@{FieldName}";
                    break;
                case OperatorEnum.LessThan:
                    result = $"{FieldName}<@{FieldName}";
                    break;
                default:
                    //TODO:
                    throw new InvalidOperationException();
            }
            return result;
        }
    }
    public sealed class StringCriteria : FieldBasedCriteria
    {
        public StringCriteria(string fieldName, List<string> filterValues, OperatorEnum @operator) : base(fieldName, filterValues, @operator)
        {
        }

        public override void FillParameters(IDbCommand command)
        {
            IDbDataParameter param = null;
            switch (Operator)
            {
                case OperatorEnum.Equals:
                case OperatorEnum.Contains:
                case OperatorEnum.NotEquals:
                    param = command.CreateParameter();
                    param.ParameterName = $"@{FieldName}";
                    param.Value = FilterValues[0];
                    param.DbType = DbType.String;
                    command.Parameters.Add(param);
                    break;
                default:
                    //TODO:
                    throw new InvalidOperationException();
            }
        }

        protected override string DoRender()
        {
            string result = string.Empty;
            switch (Operator)
            {
                case OperatorEnum.Equals:
                    result = $"{FieldName}=@{FieldName}";
                    break;
                case OperatorEnum.NotEquals:
                    result = $"{FieldName}<>@{FieldName}";
                    break;
                case OperatorEnum.Contains:
                    result = $"{FieldName} like %@{FieldName}%";
                    break;
                default:
                    //TODO:
                    throw new InvalidOperationException();
            }
            return result;
        }
    }
    public abstract class NumberCriteria : FieldBasedCriteria
    {
        protected NumberCriteria(string fieldName, List<string> filterValues, OperatorEnum @operator) : base(fieldName, filterValues, @operator)
        {
        }
        protected abstract DbType DataType { get; }
        protected abstract  object ParseData(string str);
        public override void FillParameters(IDbCommand command)
        {
            IDbDataParameter param = null;
            switch (Operator)
            {
                case OperatorEnum.Equals:
                case OperatorEnum.Contains:
                case OperatorEnum.NotEquals:
                    param = command.CreateParameter();
                    param.ParameterName = $"@{FieldName}";
                    param.Value = ParseData(FilterValues[0]);
                    param.DbType = DataType;
                    command.Parameters.Add(param);
                    break;
                default:
                    //TODO:
                    throw new InvalidOperationException();
            }
        }

        protected override string DoRender()
        {
            string result = string.Empty;
            switch (Operator)
            {
                case OperatorEnum.Equals:
                    result = $"{FieldName}=@{FieldName}";
                    break;
                case OperatorEnum.NotEquals:
                    result = $"{FieldName}<>@{FieldName}";
                    break;
                case OperatorEnum.GreaterThan:
                    result = $"{FieldName}>@{FieldName}";
                    break;
                case OperatorEnum.LessThan:
                    result = $"{FieldName}<@{FieldName}";
                    break;
                default:
                    //TODO:
                    throw new InvalidOperationException();
            }
            return result;
        }
    }

    public sealed class IntCriteria : NumberCriteria
    {
        public IntCriteria(string fieldName, List<string> filterValues, OperatorEnum @operator) : base(fieldName, filterValues, @operator)
        {
        }

        protected override DbType DataType => DbType.Int32;

        protected override object ParseData(string str)
        {
            return int.Parse(str);
        }
    }
    public sealed class DecimalCriteria : NumberCriteria
    {
        public DecimalCriteria(string fieldName, List<string> filterValues, OperatorEnum @operator) : base(fieldName, filterValues, @operator)
        {
        }

        protected override DbType DataType => DbType.Decimal;

        protected override object ParseData(string str)
        {
            return decimal.Parse(str);
        }
    }
    public enum OperatorEnum
    {
        Equals,
        NotEquals,
        Contains,
        GreaterThan,
        LessThan,
        NotContains,
        StartsWith,
        EndsWith
    }
    public sealed class AndCriteria : ICriteria
    {
        ICriteria left;
        ICriteria right;
        public AndCriteria(ICriteria left, ICriteria right)
        {
            this.left = left;
            this.right = right;
        }

        public string Render()
        {
            return $"({left.Render()} and {right.Render()})";
        }
    }
    public sealed class OrCriteria : ICriteria
    {
        ICriteria left;
        ICriteria rigth;
        public OrCriteria(ICriteria left, ICriteria right)
        {
            this.left = left;
            this.rigth = right;
        }

        public string Render()
        {
            return $"({left.Render()} or {rigth.Render()})";
        }
    }
}
