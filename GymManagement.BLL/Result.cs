using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL
{
	public sealed record Result(bool result, string? error = null, ResultKind kind = ResultKind.Ok)
	{
		public static Result Ok() => new(true);
		public static Result Faild(string message) => new(false, message, ResultKind.Fail);
		public static Result Validation(string message = "Validation Failed") => new(false, message,ResultKind.ValidationFail);
		public static Result NotFound(string message = "Not Found") => new(false, message, ResultKind.NotFound);

	}
	public sealed record Result<T>(bool result, T? model,string? message = null ,  ResultKind kind = ResultKind.Ok)
	{
		public static Result<T> Ok(T model) => new(true , model);
		public static Result<T> Faild(string message) => new(false, default , message, ResultKind.Fail);
		public static Result<T> NotFound(string message = "Not Found") => new(false, default , message , ResultKind.NotFound);

	}


	public enum ResultKind
	{
		Ok,
		Fail,
		ValidationFail,
		NotFound,
		Forbidden,
		Conflict
	}
}
