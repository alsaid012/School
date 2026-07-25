using System.Collections.Generic;

namespace SchoolERP.Application.DTOs.Common
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔧  مساعد الاستجابة (Response Helper)
    /// 📌  الوظيفة: دوال مساعدة لإنشاء استجابات موحدة
    /// 📦  الاستخدام: في جميع الـ Controllers
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public static class ResponseHelper
    {
        /// <summary>
        /// إنشاء استجابة ناجحة مع بيانات
        /// </summary>
        public static ResponseDto<T> Ok<T>(T data, string? message = null)
        {
            return new ResponseDto<T>
            {
                Success = true,
                StatusCode = 200,
                Message = message ?? "تمت العملية بنجاح",
                Data = data
            };
        }

        /// <summary>
        /// إنشاء استجابة ناجحة بدون بيانات
        /// </summary>
        public static ResponseDto<object> Ok(string? message = null)
        {
            return new ResponseDto<object>
            {
                Success = true,
                StatusCode = 200,
                Message = message ?? "تمت العملية بنجاح"
            };
        }

        /// <summary>
        /// إنشاء استجابة فاشلة
        /// </summary>
        public static ResponseDto<T> Fail<T>(string message, List<string>? errors = null, int statusCode = 400)
        {
            return new ResponseDto<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors ?? new List<string>(),
                Data = default
            };
        }

        /// <summary>
        /// إنشاء استجابة خطأ (NotFoundException)
        /// </summary>
        public static ResponseDto<T> NotFound<T>(string message = "العنصر غير موجود")
        {
            return new ResponseDto<T>
            {
                Success = false,
                StatusCode = 404,
                Message = message,
                Errors = new List<string> { message }
            };
        }

        /// <summary>
        /// إنشاء استجابة غير مصرح بها
        /// </summary>
        public static ResponseDto<T> Unauthorized<T>(string message = "غير مصرح بالدخول")
        {
            return new ResponseDto<T>
            {
                Success = false,
                StatusCode = 401,
                Message = message,
                Errors = new List<string> { message }
            };
        }

        /// <summary>
        /// إنشاء استجابة خطأ من Exception
        /// </summary>
        public static ResponseDto<T> Exception<T>(string message, int statusCode = 500)
        {
            return new ResponseDto<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    }
}