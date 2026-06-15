using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Domain.SeedWorks
{
    public class Result
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; } = string.Empty;
        public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

        protected readonly List<string> _errors = new();

        public static Result Ok(string message = "")
            => new() { Success = true, Message = message };

        public static Result Fail(string message)
            => new() { Success = false, Message = message };

        public static Result Fail(IEnumerable<string> errors)
        {
            var result = new Result
            {
                Success = false,
                Message = "Ocorreram erros de validação."
            };

            result._errors.AddRange(errors);
            return result;
        }
    }

    public class Result<T> : Result
    {
        public T? Data { get; protected set; }

        public static Result<T> Ok(T data, string message = "")
            => new() { Data = data, Success = true, Message = message };

        public new static Result<T> Fail(string message)
            => new() { Success = false, Message = message };

        public new static Result<T> Fail(IEnumerable<string> errors)
        {
            var result = new Result<T>
            {
                Success = false,
                Message = "Ocorreram erros de validação."
            };

            result._errors.AddRange(errors);
            return result;
        }
    }
}
