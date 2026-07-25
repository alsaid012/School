using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.TeacherSubjects;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Services
{
    public class TeacherSubjectService : ITeacherSubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TeacherSubjectService> _logger;

        public TeacherSubjectService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<TeacherSubjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }


        /// <summary>
        /// 📋 الحصول على جميع الروابط بين المعلمين والمواد
        /// </summary>
        public async Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetAllAsync()
        {
            try
            {
                var teacherSubjects = await _unitOfWork.TeacherSubjects.GetAllAsync();
                var dtos = new List<TeacherSubjectDto>();

                foreach (var item in teacherSubjects)
                {
                    var dto = _mapper.Map<TeacherSubjectDto>(item);

                    // ✅ جلب اسم المعلم
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(item.TeacherId);
                    if (teacher != null)
                    {
                        var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                        dto.TeacherName = user?.FullName ?? teacher.TeacherCode;
                        dto.TeacherCode = teacher.TeacherCode;
                    }

                    // ✅ جلب اسم المادة والصف
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(item.SubjectId);
                    if (subject != null)
                    {
                        dto.SubjectName = subject.SubjectName;
                        dto.SubjectCode = subject.SubjectCode;

                        var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
                        dto.GradeLevelName = gradeLevel?.GradeName;
                    }

                    dtos.Add(dto);
                }

                _logger.LogInformation("تم جلب {Count} رابط", dtos.Count);
                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Ok(dtos, "تم جلب الروابط بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetAllAsync");
                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Fail("حدث خطأ أثناء جلب الروابط", statusCode: 500);
            }
        }

        //public async Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetAllAsync()
        //{
        //    try
        //    {
        //        var teacherSubjects = await _unitOfWork.TeacherSubjects.GetAllAsync();
        //        var dtos = _mapper.Map<IEnumerable<TeacherSubjectDto>>(teacherSubjects);

        //        foreach (var dto in dtos)
        //        {
        //            var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId);
        //            var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);

        //            dto.TeacherName = teacher?.User?.FullName ?? string.Empty;
        //            dto.TeacherCode = teacher?.TeacherCode ?? string.Empty;
        //            dto.SubjectName = subject?.SubjectName ?? string.Empty;
        //            dto.SubjectCode = subject?.SubjectCode;

        //            if (subject != null)
        //            {
        //                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
        //                dto.GradeLevelName = gradeLevel?.GradeName;
        //            }
        //        }

        //        return ResponseDto<IEnumerable<TeacherSubjectDto>>.Ok(dtos, "تم جلب الروابط بنجاح");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ أثناء جلب جميع الروابط");
        //        return ResponseDto<IEnumerable<TeacherSubjectDto>>.Fail("حدث خطأ", statusCode: 500);
        //    }
        //}

        public async Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetByTeacherIdAsync(int teacherId)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
                if (teacher == null)
                {
                    return ResponseDto<IEnumerable<TeacherSubjectDto>>.NotFound("المعلم غير موجود");
                }

                var teacherSubjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.TeacherId == teacherId);
                var dtos = _mapper.Map<IEnumerable<TeacherSubjectDto>>(teacherSubjects);

                foreach (var dto in dtos)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
                    dto.SubjectName = subject?.SubjectName ?? string.Empty;
                    dto.SubjectCode = subject?.SubjectCode;
                    
                    if (subject != null)
                    {
                        var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
                        dto.GradeLevelName = gradeLevel?.GradeName;
                    }
                }

                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Ok(dtos, "تم جلب الروابط بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الروابط للمعلم {TeacherId}", teacherId);
                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على روابط مادة معينة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetBySubjectIdAsync(int subjectId)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);
                if (subject == null)
                    return ResponseDto<IEnumerable<TeacherSubjectDto>>.NotFound("المادة غير موجودة");

                var teacherSubjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.SubjectId == subjectId);
                var dtos = new List<TeacherSubjectDto>();

                foreach (var item in teacherSubjects)
                {
                    var dto = _mapper.Map<TeacherSubjectDto>(item);

                    // ✅ جلب اسم المعلم
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(item.TeacherId);
                    if (teacher != null)
                    {
                        var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                        dto.TeacherName = user?.FullName ?? teacher.TeacherCode;
                        dto.TeacherCode = teacher.TeacherCode;
                    }

                    // ✅ جلب اسم المادة والصف
                    dto.SubjectName = subject.SubjectName;
                    dto.SubjectCode = subject.SubjectCode;

                    var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
                    dto.GradeLevelName = gradeLevel?.GradeName;

                    dtos.Add(dto);
                }

                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Ok(dtos, "تم جلب الروابط بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetBySubjectIdAsync للمادة {SubjectId}", subjectId);
                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }
        //public async Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetBySubjectIdAsync(int subjectId)
        //{
        //    try
        //    {
        //        var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);
        //        if (subject == null)
        //        {
        //            return ResponseDto<IEnumerable<TeacherSubjectDto>>.NotFound("المادة غير موجودة");
        //        }

        //        var teacherSubjects = await _unitOfWork.TeacherSubjects
        //            .FindAsync(ts => ts.SubjectId == subjectId);
        //        var dtos = _mapper.Map<IEnumerable<TeacherSubjectDto>>(teacherSubjects);

        //        foreach (var dto in dtos)
        //        {
        //            var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId);
        //            dto.TeacherName = teacher?.User?.FullName ?? string.Empty;
        //            dto.TeacherCode = teacher?.TeacherCode ?? string.Empty;
        //        }

        //        return ResponseDto<IEnumerable<TeacherSubjectDto>>.Ok(dtos, "تم جلب الروابط بنجاح");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ أثناء جلب الروابط للمادة {SubjectId}", subjectId);
        //        return ResponseDto<IEnumerable<TeacherSubjectDto>>.Fail("حدث خطأ", statusCode: 500);
        //    }
        //}

        public async Task<ResponseDto<IEnumerable<TeacherSubjectLookupDto>>> GetLookupAsync(int? teacherId = null)
        {
            try
            {
                IEnumerable<TeacherSubject> teacherSubjects;

                if (teacherId.HasValue)
                {
                    teacherSubjects = await _unitOfWork.TeacherSubjects
                        .FindAsync(ts => ts.TeacherId == teacherId.Value);
                }
                else
                {
                    teacherSubjects = await _unitOfWork.TeacherSubjects.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<TeacherSubjectLookupDto>>(teacherSubjects);

                foreach (var dto in dtos)
                {
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId);
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
                    
                    dto.TeacherName = teacher?.User?.FullName ?? string.Empty;
                    dto.SubjectName = subject?.SubjectName ?? string.Empty;
                    
                    if (subject != null)
                    {
                        var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
                        dto.GradeLevelName = gradeLevel?.GradeName;
                    }
                }

                return ResponseDto<IEnumerable<TeacherSubjectLookupDto>>.Ok(dtos, "تم جلب الروابط للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الروابط للقوائم");
                return ResponseDto<IEnumerable<TeacherSubjectLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على رابط بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<TeacherSubjectDto>> GetByIdAsync(int id)
        {
            try
            {
                var teacherSubject = await _unitOfWork.TeacherSubjects.GetByIdAsync(id);
                if (teacherSubject == null)
                    return ResponseDto<TeacherSubjectDto>.NotFound("الرابط غير موجود");

                var dto = _mapper.Map<TeacherSubjectDto>(teacherSubject);

                // ✅ جلب اسم المعلم
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherSubject.TeacherId);
                if (teacher != null)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                    dto.TeacherName = user?.FullName ?? teacher.TeacherCode;
                    dto.TeacherCode = teacher.TeacherCode;
                }

                // ✅ جلب اسم المادة والصف
                var subject = await _unitOfWork.Subjects.GetByIdAsync(teacherSubject.SubjectId);
                if (subject != null)
                {
                    dto.SubjectName = subject.SubjectName;
                    dto.SubjectCode = subject.SubjectCode;

                    var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
                    dto.GradeLevelName = gradeLevel?.GradeName;
                }

                return ResponseDto<TeacherSubjectDto>.Ok(dto, "تم جلب الرابط بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByIdAsync للرابط {Id}", id);
                return ResponseDto<TeacherSubjectDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }
        //public async Task<ResponseDto<TeacherSubjectDto>> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        var teacherSubject = await _unitOfWork.TeacherSubjects.GetByIdAsync(id);
        //        if (teacherSubject == null)
        //        {
        //            return ResponseDto<TeacherSubjectDto>.NotFound("الرابط غير موجود");
        //        }

        //        var dto = _mapper.Map<TeacherSubjectDto>(teacherSubject);

        //        var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherSubject.TeacherId);
        //        var subject = await _unitOfWork.Subjects.GetByIdAsync(teacherSubject.SubjectId);

        //        dto.TeacherName = teacher?.User?.FullName ?? string.Empty;
        //        dto.TeacherCode = teacher?.TeacherCode ?? string.Empty;
        //        dto.SubjectName = subject?.SubjectName ?? string.Empty;
        //        dto.SubjectCode = subject?.SubjectCode;

        //        if (subject != null)
        //        {
        //            var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
        //            dto.GradeLevelName = gradeLevel?.GradeName;
        //        }

        //        return ResponseDto<TeacherSubjectDto>.Ok(dto, "تم جلب الرابط بنجاح");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ أثناء جلب الرابط {Id}", id);
        //        return ResponseDto<TeacherSubjectDto>.Fail("حدث خطأ", statusCode: 500);
        //    }
        //}

        public async Task<ResponseDto<TeacherSubjectDto>> CreateAsync(CreateTeacherSubjectDto createDto)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(createDto.TeacherId);
                if (teacher == null)
                {
                    return ResponseDto<TeacherSubjectDto>.Fail("المعلم غير موجود");
                }

                var subject = await _unitOfWork.Subjects.GetByIdAsync(createDto.SubjectId);
                if (subject == null)
                {
                    return ResponseDto<TeacherSubjectDto>.Fail("المادة غير موجودة");
                }

                if (await _unitOfWork.TeacherSubjects
                    .AnyAsync(ts => ts.TeacherId == createDto.TeacherId && ts.SubjectId == createDto.SubjectId))
                {
                    return ResponseDto<TeacherSubjectDto>.Fail("هذا المعلم يدرس هذه المادة بالفعل");
                }

                var teacherSubject = _mapper.Map<TeacherSubject>(createDto);
                teacherSubject.CreatedAt = DateTime.Now;
                teacherSubject.IsActive = true;

                var created = await _unitOfWork.TeacherSubjects.AddAsync(teacherSubject);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<TeacherSubjectDto>(created);
                _logger.LogInformation("تم إنشاء رابط جديد بين المعلم {TeacherId} والمادة {SubjectId}", 
                    createDto.TeacherId, createDto.SubjectId);

                return ResponseDto<TeacherSubjectDto>.Ok(dto, "تم إنشاء الرابط بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء رابط جديد");
                return ResponseDto<TeacherSubjectDto>.Fail("حدث خطأ أثناء إنشاء الرابط", statusCode: 500);
            }
        }

        public async Task<ResponseDto<TeacherSubjectDto>> UpdateAsync(int id, UpdateTeacherSubjectDto updateDto)
        {
            try
            {
                var teacherSubject = await _unitOfWork.TeacherSubjects.GetByIdAsync(id);
                if (teacherSubject == null)
                {
                    return ResponseDto<TeacherSubjectDto>.NotFound("الرابط غير موجود");
                }

                _mapper.Map(updateDto, teacherSubject);
                teacherSubject.UpdatedAt = DateTime.Now;

                await _unitOfWork.TeacherSubjects.UpdateAsync(teacherSubject);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<TeacherSubjectDto>(teacherSubject);
                _logger.LogInformation("تم تحديث الرابط {Id}", id);

                return ResponseDto<TeacherSubjectDto>.Ok(dto, "تم تحديث الرابط بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث الرابط {Id}", id);
                return ResponseDto<TeacherSubjectDto>.Fail("حدث خطأ أثناء تحديث الرابط", statusCode: 500);
            }
        }

        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var teacherSubject = await _unitOfWork.TeacherSubjects.GetByIdAsync(id);
                if (teacherSubject == null)
                {
                    return ResponseDto.NotFound("الرابط غير موجود");
                }

                await _unitOfWork.TeacherSubjects.DeleteAsync(teacherSubject);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف الرابط {Id}", id);
                return ResponseDto.Ok("تم حذف الرابط بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف الرابط {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف الرابط", statusCode: 500);
            }
        }

        public async Task<ResponseDto<bool>> IsExistsAsync(int teacherId, int subjectId)
        {
            try
            {
                var exists = await _unitOfWork.TeacherSubjects
                    .AnyAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);
                return ResponseDto<bool>.Ok(exists, exists ? "الرابط موجود" : "الرابط غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من وجود الرابط");
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }
    }
}

#region MyRegion
//using AutoMapper;
//using Microsoft.Extensions.Logging;
//using SchoolERP.Application.DTOs.Common;
//using SchoolERP.Application.DTOs.TeacherSubjects;
//using SchoolERP.Application.Interfaces;
//using SchoolERP.Application.Interfaces.Services;
//using SchoolERP.Domain.Entities;

//namespace SchoolERP.Application.Services
//{
//    /// <summary>
//    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
//    /// 🔗  خدمة ربط المعلم بالمادة (TeacherSubjectService)
//    /// 📌  الوظيفة: تنفيذ عمليات إدارة ربط المعلم بالمادة
//    /// 📦  الاستخدام: في TeacherSubjectsController
//    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
//    /// </summary>
//    public class TeacherSubjectService : ITeacherSubjectService
//    {
//        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly ILogger<TeacherSubjectService> _logger;

//        #endregion

//        #region ════════════════════════════════════ البناء ════════════════════════════════════

//        public TeacherSubjectService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TeacherSubjectService> logger)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//            _logger = logger;
//        }

//        #endregion

//        #region ════════════════════════════════════ جلب البيانات ════════════════════════════════════

//        /// <summary>
//        /// 📋 الحصول على جميع الروابط بين المعلمين والمواد
//        /// </summary>
//        public async Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetAllAsync()
//        {
//            try
//            {
//                var teacherSubjects = await _unitOfWork.TeacherSubjects.GetAllAsync();
//                var dtos = _mapper.Map<IEnumerable<TeacherSubjectDto>>(teacherSubjects);

//                foreach (var dto in dtos)
//                {
//                    // جلب اسم المعلم
//                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId);
//                    if (teacher != null)
//                    {
//                        var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
//                        dto.TeacherName = user?.FullName ?? string.Empty;
//                        dto.TeacherCode = teacher.TeacherCode;
//                    }

//                    // جلب اسم المادة والصف
//                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
//                    if (subject != null)
//                    {
//                        dto.SubjectName = subject.SubjectName;
//                        dto.SubjectCode = subject.SubjectCode;

//                        var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
//                        dto.GradeLevelName = gradeLevel?.GradeName;
//                    }
//                }

//                _logger.LogInformation("تم جلب {Count} رابط", dtos.Count());
//                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Ok(dtos, "تم جلب الروابط بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ في GetAllAsync");
//                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Fail("حدث خطأ أثناء جلب الروابط", statusCode: 500);
//            }
//        }

//        /// <summary>
//        /// 🔍 الحصول على رابط بواسطة المعرف
//        /// </summary>
//        public async Task<ResponseDto<TeacherSubjectDto>> GetByIdAsync(int id)
//        {
//            try
//            {
//                var teacherSubject = await _unitOfWork.TeacherSubjects.GetByIdAsync(id);
//                if (teacherSubject == null)
//                    return ResponseDto<TeacherSubjectDto>.NotFound("الرابط غير موجود");

//                var dto = _mapper.Map<TeacherSubjectDto>(teacherSubject);

//                // جلب اسم المعلم
//                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherSubject.TeacherId);
//                if (teacher != null)
//                {
//                    var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
//                    dto.TeacherName = user?.FullName ?? string.Empty;
//                    dto.TeacherCode = teacher.TeacherCode;
//                }

//                // جلب اسم المادة والصف
//                var subject = await _unitOfWork.Subjects.GetByIdAsync(teacherSubject.SubjectId);
//                if (subject != null)
//                {
//                    dto.SubjectName = subject.SubjectName;
//                    dto.SubjectCode = subject.SubjectCode;

//                    var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
//                    dto.GradeLevelName = gradeLevel?.GradeName;
//                }

//                return ResponseDto<TeacherSubjectDto>.Ok(dto, "تم جلب الرابط بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ في GetByIdAsync للرابط {Id}", id);
//                return ResponseDto<TeacherSubjectDto>.Fail("حدث خطأ", statusCode: 500);
//            }
//        }

//        #endregion

//        #region ════════════════════════════════════ البحث والفلترة ════════════════════════════════════

//        /// <summary>
//        /// 📋 الحصول على روابط معلم معين
//        /// </summary>
//        public async Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetByTeacherIdAsync(int teacherId)
//        {
//            try
//            {
//                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
//                if (teacher == null)
//                    return ResponseDto<IEnumerable<TeacherSubjectDto>>.NotFound("المعلم غير موجود");

//                var teacherSubjects = await _unitOfWork.TeacherSubjects
//                    .FindAsync(ts => ts.TeacherId == teacherId);
//                var dtos = _mapper.Map<IEnumerable<TeacherSubjectDto>>(teacherSubjects);

//                foreach (var dto in dtos)
//                {
//                    // جلب اسم المعلم
//                    var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
//                    dto.TeacherName = user?.FullName ?? string.Empty;
//                    dto.TeacherCode = teacher.TeacherCode;

//                    // جلب اسم المادة والصف
//                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
//                    if (subject != null)
//                    {
//                        dto.SubjectName = subject.SubjectName;
//                        dto.SubjectCode = subject.SubjectCode;

//                        var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
//                        dto.GradeLevelName = gradeLevel?.GradeName;
//                    }
//                }

//                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Ok(dtos, "تم جلب الروابط بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ في GetByTeacherIdAsync للمعلم {TeacherId}", teacherId);
//                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Fail("حدث خطأ", statusCode: 500);
//            }
//        }

//        /// <summary>
//        /// 📋 الحصول على روابط مادة معينة
//        /// </summary>
//        public async Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetBySubjectIdAsync(int subjectId)
//        {
//            try
//            {
//                var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);
//                if (subject == null)
//                    return ResponseDto<IEnumerable<TeacherSubjectDto>>.NotFound("المادة غير موجودة");

//                var teacherSubjects = await _unitOfWork.TeacherSubjects
//                    .FindAsync(ts => ts.SubjectId == subjectId);
//                var dtos = _mapper.Map<IEnumerable<TeacherSubjectDto>>(teacherSubjects);

//                foreach (var dto in dtos)
//                {
//                    // جلب اسم المعلم
//                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId);
//                    if (teacher != null)
//                    {
//                        var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
//                        dto.TeacherName = user?.FullName ?? string.Empty;
//                        dto.TeacherCode = teacher.TeacherCode;
//                    }

//                    // جلب اسم المادة والصف
//                    dto.SubjectName = subject.SubjectName;
//                    dto.SubjectCode = subject.SubjectCode;

//                    var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
//                    dto.GradeLevelName = gradeLevel?.GradeName;
//                }

//                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Ok(dtos, "تم جلب الروابط بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ في GetBySubjectIdAsync للمادة {SubjectId}", subjectId);
//                return ResponseDto<IEnumerable<TeacherSubjectDto>>.Fail("حدث خطأ", statusCode: 500);
//            }
//        }

//        #endregion

//        #region ════════════════════════════════════ القوائم المنسدلة ════════════════════════════════════

//        /// <summary>
//        /// 📋 الحصول على الروابط للقوائم المنسدلة
//        /// </summary>
//        public async Task<ResponseDto<IEnumerable<TeacherSubjectLookupDto>>> GetLookupAsync(int? teacherId = null)
//        {
//            try
//            {
//                IEnumerable<TeacherSubject> teacherSubjects;

//                if (teacherId.HasValue)
//                {
//                    teacherSubjects = await _unitOfWork.TeacherSubjects
//                        .FindAsync(ts => ts.TeacherId == teacherId.Value);
//                }
//                else
//                {
//                    teacherSubjects = await _unitOfWork.TeacherSubjects.GetAllAsync();
//                }

//                var dtos = _mapper.Map<IEnumerable<TeacherSubjectLookupDto>>(teacherSubjects);

//                foreach (var dto in dtos)
//                {
//                    // جلب اسم المعلم
//                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId);
//                    if (teacher != null)
//                    {
//                        var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
//                        dto.TeacherName = user?.FullName ?? string.Empty;
//                    }

//                    // جلب اسم المادة والصف
//                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
//                    if (subject != null)
//                    {
//                        dto.SubjectName = subject.SubjectName;
//                        var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
//                        dto.GradeLevelName = gradeLevel?.GradeName;
//                    }
//                }

//                return ResponseDto<IEnumerable<TeacherSubjectLookupDto>>.Ok(dtos, "تم جلب الروابط للقوائم");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ في GetLookupAsync");
//                return ResponseDto<IEnumerable<TeacherSubjectLookupDto>>.Fail("حدث خطأ", statusCode: 500);
//            }
//        }

//        #endregion

//        #region ════════════════════════════════════ العمليات الأساسية ════════════════════════════════════

//        /// <summary>
//        /// ➕ إنشاء رابط جديد بين معلم ومادة
//        /// </summary>
//        public async Task<ResponseDto<TeacherSubjectDto>> CreateAsync(CreateTeacherSubjectDto createDto)
//        {
//            try
//            {
//                // التحقق من وجود المعلم
//                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(createDto.TeacherId);
//                if (teacher == null)
//                    return ResponseDto<TeacherSubjectDto>.Fail("المعلم غير موجود");

//                // التحقق من وجود المادة
//                var subject = await _unitOfWork.Subjects.GetByIdAsync(createDto.SubjectId);
//                if (subject == null)
//                    return ResponseDto<TeacherSubjectDto>.Fail("المادة غير موجودة");

//                // التحقق من عدم وجود رابط مكرر
//                var existing = await _unitOfWork.TeacherSubjects
//                    .FindAsync(ts => ts.TeacherId == createDto.TeacherId && ts.SubjectId == createDto.SubjectId);
//                if (existing.Any())
//                    return ResponseDto<TeacherSubjectDto>.Fail("هذا المعلم يدرس هذه المادة بالفعل");

//                // إذا كان الرابط أساسي، إلغاء الأساسية من الروابط الأخرى لنفس المعلم
//                if (createDto.IsPrimary)
//                {
//                    var otherPrimary = await _unitOfWork.TeacherSubjects
//                        .FindAsync(ts => ts.TeacherId == createDto.TeacherId && ts.IsPrimary);
//                    foreach (var item in otherPrimary)
//                    {
//                        item.IsPrimary = false;
//                        await _unitOfWork.TeacherSubjects.UpdateAsync(item);
//                    }
//                }

//                var teacherSubject = _mapper.Map<TeacherSubject>(createDto);
//                teacherSubject.CreatedAt = DateTime.Now;
//                teacherSubject.IsActive = true;

//                var created = await _unitOfWork.TeacherSubjects.AddAsync(teacherSubject);
//                await _unitOfWork.CompleteAsync();

//                var dto = _mapper.Map<TeacherSubjectDto>(created);

//                // جلب الأسماء
//                var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
//                dto.TeacherName = user?.FullName ?? string.Empty;
//                dto.TeacherCode = teacher.TeacherCode;
//                dto.SubjectName = subject.SubjectName;
//                dto.SubjectCode = subject.SubjectCode;

//                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
//                dto.GradeLevelName = gradeLevel?.GradeName;

//                _logger.LogInformation("تم إنشاء رابط جديد بين المعلم {TeacherId} والمادة {SubjectId}", createDto.TeacherId, createDto.SubjectId);
//                return ResponseDto<TeacherSubjectDto>.Ok(dto, "تم إنشاء الرابط بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ في CreateAsync");
//                return ResponseDto<TeacherSubjectDto>.Fail("حدث خطأ أثناء إنشاء الرابط", statusCode: 500);
//            }
//        }

//        /// <summary>
//        /// ✏️ تحديث بيانات رابط
//        /// </summary>
//        public async Task<ResponseDto<TeacherSubjectDto>> UpdateAsync(int id, UpdateTeacherSubjectDto updateDto)
//        {
//            try
//            {
//                var teacherSubject = await _unitOfWork.TeacherSubjects.GetByIdAsync(id);
//                if (teacherSubject == null)
//                    return ResponseDto<TeacherSubjectDto>.NotFound("الرابط غير موجود");

//                // إذا كان الرابط أساسي، إلغاء الأساسية من الروابط الأخرى لنفس المعلم
//                if (updateDto.IsPrimary.HasValue && updateDto.IsPrimary.Value)
//                {
//                    var otherPrimary = await _unitOfWork.TeacherSubjects
//                        .FindAsync(ts => ts.TeacherId == teacherSubject.TeacherId && ts.IsPrimary && ts.Id != id);
//                    foreach (var item in otherPrimary)
//                    {
//                        item.IsPrimary = false;
//                        await _unitOfWork.TeacherSubjects.UpdateAsync(item);
//                    }
//                }

//                _mapper.Map(updateDto, teacherSubject);
//                teacherSubject.UpdatedAt = DateTime.Now;

//                await _unitOfWork.TeacherSubjects.UpdateAsync(teacherSubject);
//                await _unitOfWork.CompleteAsync();

//                var dto = _mapper.Map<TeacherSubjectDto>(teacherSubject);

//                // جلب الأسماء
//                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherSubject.TeacherId);
//                if (teacher != null)
//                {
//                    var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
//                    dto.TeacherName = user?.FullName ?? string.Empty;
//                    dto.TeacherCode = teacher.TeacherCode;
//                }

//                var subject = await _unitOfWork.Subjects.GetByIdAsync(teacherSubject.SubjectId);
//                if (subject != null)
//                {
//                    dto.SubjectName = subject.SubjectName;
//                    dto.SubjectCode = subject.SubjectCode;

//                    var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
//                    dto.GradeLevelName = gradeLevel?.GradeName;
//                }

//                _logger.LogInformation("تم تحديث الرابط {Id}", id);
//                return ResponseDto<TeacherSubjectDto>.Ok(dto, "تم تحديث الرابط بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ في UpdateAsync للرابط {Id}", id);
//                return ResponseDto<TeacherSubjectDto>.Fail("حدث خطأ أثناء تحديث الرابط", statusCode: 500);
//            }
//        }

//        /// <summary>
//        /// 🗑️ حذف رابط
//        /// </summary>
//        public async Task<ResponseDto> DeleteAsync(int id)
//        {
//            try
//            {
//                var teacherSubject = await _unitOfWork.TeacherSubjects.GetByIdAsync(id);
//                if (teacherSubject == null)
//                    return ResponseDto.NotFound("الرابط غير موجود");

//                await _unitOfWork.TeacherSubjects.DeleteAsync(teacherSubject);
//                await _unitOfWork.CompleteAsync();

//                _logger.LogInformation("تم حذف الرابط {Id}", id);
//                return ResponseDto.Ok("تم حذف الرابط بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ في DeleteAsync للرابط {Id}", id);
//                return ResponseDto.Fail("حدث خطأ أثناء حذف الرابط", statusCode: 500);
//            }
//        }

//        #endregion

//        #region ════════════════════════════════════ التحقق ════════════════════════════════════

//        /// <summary>
//        /// ✅ التحقق من وجود رابط مكرر
//        /// </summary>
//        public async Task<ResponseDto<bool>> IsExistsAsync(int teacherId, int subjectId)
//        {
//            try
//            {
//                var existing = await _unitOfWork.TeacherSubjects
//                    .FindAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);
//                var exists = existing.Any();
//                return ResponseDto<bool>.Ok(exists, exists ? "الرابط موجود" : "الرابط غير موجود");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ في IsExistsAsync");
//                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
//            }
//        }

//        #endregion
//    }
//}
#endregion