using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Service.Services
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; }
        public bool IsNotFound { get; }
        public string ErrorMessage { get; }
        public object CustomErrorObject { get; private set; }
        public T Data { get; }
        public ServiceResult(bool isSuccess, bool isNotFound, string errorMessage, T data) 
            {
                IsSuccess = isSuccess;
                IsNotFound = isNotFound;
                ErrorMessage = errorMessage;
                Data = data;
            }
        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>(true, false, null, data);
        }
        public static ServiceResult<T> NotFound(string message)
        {
            return new ServiceResult<T>(false, true, message, default);
        }
        public static ServiceResult<T> Error(string message)
        {
            return new ServiceResult<T>(false, false, message, default);
        }
        public static ServiceResult<T> CustomError(object customError)
        {
            return new ServiceResult<T>(false, false, null, default)
            {
                CustomErrorObject = customError
            };
        }
    }
}
