using System;
using System.Collections.Generic;

namespace SchoolERP.Application.DTOs.Common
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📨  نموذج الاستجابة الموحد (Response DTO)
    /// 📌  الوظيفة: توحيد شكل الاستجابة لجميع الـ API Endpoints
    /// 📦  الاستخدام: في جميع الـ Controllers
    /// ⚠️  ملاحظة: هذا الكلاس للبيانات فقط، الدوال المساعدة في مكان آخر
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    /// <typeparam name="T">نوع البيانات المرسلة</typeparam>
    public class ResponseDto<T>
    {
        /// <summary>
        /// هل العملية ناجحة؟
        /// </summary>
        /// <example>true</example>
        public bool Success { get; set; } = true;

        /// <summary>
        /// كود الحالة (HTTP Status Code)
        /// </summary>
        /// <example>200</example>
        public int? StatusCode { get; set; }

        /// <summary>
        /// رسالة توضيحية للعملية
        /// </summary>
        /// <example>تمت العملية بنجاح</example>
        public string? Message { get; set; }

        /// <summary>
        /// البيانات المرسلة (يمكن أن تكون أي نوع)
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// قائمة الأخطاء (في حالة الفشل)
        /// </summary>
        /// <example>["اسم المستخدم مطلوب", "كلمة المرور غير صحيحة"]</example>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// وقت الاستجابة (Timestamp)
        /// </summary>
        /// <example>2024-01-01T12:00:00</example>
        public DateTime Timestamp { get; set; } = DateTime.Now;




        public static ResponseDto<T> Ok(T data, string? message = null)
        {
            return new ResponseDto<T>
            {
                Success = true,
                StatusCode = 200,
                Message = message ?? "تمت العملية بنجاح",
                Data = data
            };
        }

        public static ResponseDto<T> Fail(string message, List<string>? errors = null, int statusCode = 400)
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

        public static ResponseDto<T> NotFound(string message = "العنصر غير موجود")
        {
            return new ResponseDto<T>
            {
                Success = false,
                StatusCode = 404,
                Message = message,
                Errors = new List<string> { message }
            };
        }

        public static ResponseDto<T> Unauthorized(string message = "غير مصرح بالدخول")
        {
            return new ResponseDto<T>
            {
                Success = false,
                StatusCode = 401,
                Message = message,
                Errors = new List<string> { message }
            };
        }

    }
    /// <summary>
    /// 📨  نموذج استجابة بدون بيانات
    /// </summary>
    public class ResponseDto
    {
        public bool Success { get; set; } = true;
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    
     public static ResponseDto Ok(string? message = null)
        {
            return new ResponseDto
            {
                Success = true,
                StatusCode = 200,
                Message = message ?? "تمت العملية بنجاح"
            };
        }

        public static ResponseDto Fail(string message, List<string>? errors = null, int statusCode = 400)
        {
            return new ResponseDto
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

        public static ResponseDto NotFound(string message = "العنصر غير موجود")
        {
            return new ResponseDto
            {
                Success = false,
                StatusCode = 404,
                Message = message,
                Errors = new List<string> { message }
            };
        }

        public static ResponseDto Unauthorized(string message = "غير مصرح بالدخول")
        {
            return new ResponseDto
            {
                Success = false,
                StatusCode = 401,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    } 
}