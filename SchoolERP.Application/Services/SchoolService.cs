using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Departments;
using SchoolERP.Application.DTOs.GradeLevels;
using SchoolERP.Application.DTOs.Schools;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏫  خدمة المدارس (SchoolService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة المدارس
    /// 📦  الاستخدام: في SchoolsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class SchoolService : ISchoolService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SchoolService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public SchoolService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<SchoolService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على المدارس ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع المدارس
        /// </summary>
        public async Task<ResponseDto<IEnumerable<SchoolDto>>> GetAllAsync()
        {
            try
            {
                //var schools = await _unitOfWork.Schools.GetAllAsync();
                var schools = await _unitOfWork.SchoolRepository.GetAllAsync();

                var dtos = _mapper.Map<IEnumerable<SchoolDto>>(schools);
                foreach (var dto in dtos)
                {
                    // جلب اسم الإدارة
                    var department = await _unitOfWork.Departments.GetByIdAsync(dto.DepartmentId);
                    dto.DepartmentName = department?.Name;

                    // جلب اسم المحافظة
                    if (department != null)
                    {
                        var governorate = await _unitOfWork.Governorates.GetByIdAsync(department.GovernorateId);
                        dto.GovernorateName = governorate?.Name;
                    }

                    // جلب عدد الطلاب
                    var students = await _unitOfWork.Students
                        .FindAsync(s => s.User.SchoolId == dto.Id);
                    dto.StudentsCount = students.Count();

                    // جلب عدد المعلمين
                    var teachers = await _unitOfWork.TeacherRepository
                        .FindAsync(t => t.User.SchoolId == dto.Id);
                    dto.TeachersCount = teachers.Count();
                }
                _logger.LogInformation("تم جلب {Count} مدرسة", dtos.Count());
                return ResponseDto<IEnumerable<SchoolDto>>.Ok(dtos, "تم جلب المدارس بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع المدارس");
                return ResponseDto<IEnumerable<SchoolDto>>.Fail("حدث خطأ أثناء جلب المدارس", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المدارس التابعة لإدارة معينة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<SchoolDto>>> GetByDepartmentIdAsync(int departmentId)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
                if (department == null)
                {
                    return ResponseDto<IEnumerable<SchoolDto>>.NotFound("الإدارة غير موجودة");
                }

                //var schools = await _unitOfWork.Schools
                var schools = await _unitOfWork.SchoolRepository
                        .FindAsync(s => s.DepartmentId == departmentId);
                var dtos = _mapper.Map<IEnumerable<SchoolDto>>(schools);

                return ResponseDto<IEnumerable<SchoolDto>>.Ok(dtos, "تم جلب المدارس بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المدارس للإدارة {DepartmentId}", departmentId);
                return ResponseDto<IEnumerable<SchoolDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المدارس للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<SchoolLookupDto>>> GetLookupAsync(int? departmentId = null)
        {
            try
            {
                IEnumerable<School> schools;

                if (departmentId.HasValue)
                {
                    schools = await _unitOfWork.SchoolRepository
                        .FindAsync(s => s.DepartmentId == departmentId.Value);
                }
                else
                {
                    schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<SchoolLookupDto>>(schools);
                return ResponseDto<IEnumerable<SchoolLookupDto>>.Ok(dtos, "تم جلب المدارس للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المدارس للقوائم");
                return ResponseDto<IEnumerable<SchoolLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن مدرسة ════════════════════════════════════

        ///// <summary>
        ///// 🔍 الحصول على مدرسة بواسطة المعرف
        ///// </summary>
        //public async Task<ResponseDto<SchoolDetailsDto>> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        var school = await _unitOfWork.Schools.GetWithDetailsAsync(id);
        //        if (school == null)
        //        {
        //            return ResponseDto<SchoolDetailsDto>.NotFound("المدرسة غير موجودة");
        //        }

        //        var dto = _mapper.Map<SchoolDetailsDto>(school);

        //        // إحصائيات المدرسة
        //        dto.Statistics = new SchoolStatisticsDto
        //        {
        //            TotalStudents = school.Users?.Count(u => u.UserType == UserType.Student) ?? 0,
        //            TotalTeachers = school.Users?.Count(u => u.UserType == UserType.Teacher) ?? 0,
        //            TotalEmployees = school.Users?.Count(u => u.UserType == UserType.Employee) ?? 0,
        //            TotalClassRooms = school.GradeLevels?.Sum(g => g.ClassRooms.Count) ?? 0,
        //            TotalGradeLevels = school.GradeLevels?.Count ?? 0,
        //            TotalAcademicYears = school.AcademicYears?.Count ?? 0
        //        };

        //        return ResponseDto<SchoolDetailsDto>.Ok(dto, "تم جلب المدرسة بنجاح");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ أثناء جلب المدرسة {Id}", id);
        //        return ResponseDto<SchoolDetailsDto>.Fail("حدث خطأ", statusCode: 500);
        //    }
        //}


        /// <summary>
        /// 🔍 الحصول على مدرسة بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<SchoolDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                //var school = await _unitOfWork.Schools.GetByIdAsync(id);
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(id);

                if (school == null)
                    return ResponseDto<SchoolDetailsDto>.NotFound("المدرسة غير موجودة");

                var dto = _mapper.Map<SchoolDetailsDto>(school);

                // جلب الإدارة
                var department = await _unitOfWork.Departments.GetByIdAsync(school.DepartmentId);
                dto.DepartmentName = department?.Name;
                dto.Department = department != null ? new DepartmentLookupDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Code = department.Code
                } : null;

                // جلب المحافظة
                if (department != null)
                {
                    var governorate = await _unitOfWork.Governorates.GetByIdAsync(department.GovernorateId);
                    dto.GovernorateName = governorate?.Name;
                }

                // جلب الصفوف
                var gradeLevels = await _unitOfWork.GradeLevels
                    .FindAsync(g => g.SchoolId == id);
                dto.GradeLevels = _mapper.Map<List<GradeLevelLookupDto>>(gradeLevels);

                // جلب الإحصائيات
                var statisticsResponse = await GetStatisticsAsync(id);
                dto.Statistics = statisticsResponse.Data;

                return ResponseDto<SchoolDetailsDto>.Ok(dto, "تم جلب المدرسة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByIdAsync للمدرسة {Id}", id);
                return ResponseDto<SchoolDetailsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على مدرسة بواسطة الكود
        /// </summary>
        public async Task<ResponseDto<SchoolDto>> GetByCodeAsync(string code)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByCodeAsync(code);
                if (school == null)
                {
                    return ResponseDto<SchoolDto>.NotFound("المدرسة غير موجودة");
                }

                var dto = _mapper.Map<SchoolDto>(school);
                return ResponseDto<SchoolDto>.Ok(dto, "تم جلب المدرسة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المدرسة بالكود {Code}", code);
                return ResponseDto<SchoolDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }
        #region ════════════════════════════════════ الإحصائيات ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصائيات المدرسة
        /// </summary>
        public async Task<ResponseDto<SchoolStatisticsDto>> GetStatisticsAsync(int schoolId)
        {
            try
            {
                //var school = await _unitOfWork.Schools.GetByIdAsync(schoolId);
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);

                if (school == null)
                    return ResponseDto<SchoolStatisticsDto>.NotFound("المدرسة غير موجودة");

                // جلب الإحصائيات
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.User.SchoolId == schoolId);
                var teachers = await _unitOfWork.TeacherRepository
                    .FindAsync(t => t.User.SchoolId == schoolId);
                var employees = await _unitOfWork.EmployeeRepository
                    .FindAsync(e => e.User.SchoolId == schoolId);
                var gradeLevels = await _unitOfWork.GradeLevels
                    .FindAsync(g => g.SchoolId == schoolId);
                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.GradeLevel.SchoolId == schoolId);
                var academicYears = await _unitOfWork.AcademicYears
                    .FindAsync(ay => ay.SchoolId == schoolId);

                var statistics = new SchoolStatisticsDto
                {
                    TotalStudents = students.Count(),
                    TotalTeachers = teachers.Count(),
                    TotalEmployees = employees.Count(),
                    TotalClassRooms = classRooms.Count(),
                    TotalGradeLevels = gradeLevels.Count(),
                    TotalAcademicYears = academicYears.Count()
                };

                return ResponseDto<SchoolStatisticsDto>.Ok(statistics, "تم جلب إحصائيات المدرسة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetStatisticsAsync للمدرسة {SchoolId}", schoolId);
                return ResponseDto<SchoolStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion
        ///// <summary>
        ///// 📊 الحصول على إحصائيات المدرسة
        ///// </summary>
        //public async Task<ResponseDto<SchoolStatisticsDto>> GetStatisticsAsync(int schoolId)
        //{
        //    try
        //    {
        //        var school = await _unitOfWork.Schools.GetWithDetailsAsync(schoolId);
        //        if (school == null)
        //        {
        //            return ResponseDto<SchoolStatisticsDto>.NotFound("المدرسة غير موجودة");
        //        }

        //        var statistics = new SchoolStatisticsDto
        //        {
        //            TotalStudents = school.Users?.Count(u => u.UserType == UserType.Student) ?? 0,
        //            TotalTeachers = school.Users?.Count(u => u.UserType == UserType.Teacher) ?? 0,
        //            TotalEmployees = school.Users?.Count(u => u.UserType == UserType.Employee) ?? 0,
        //            TotalClassRooms = school.GradeLevels?.Sum(g => g.ClassRooms.Count) ?? 0,
        //            TotalGradeLevels = school.GradeLevels?.Count ?? 0,
        //            TotalAcademicYears = school.AcademicYears?.Count ?? 0
        //        };

        //        return ResponseDto<SchoolStatisticsDto>.Ok(statistics, "تم جلب إحصائيات المدرسة");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ أثناء جلب إحصائيات المدرسة {SchoolId}", schoolId);
        //        return ResponseDto<SchoolStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
        //    }
        //}

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء مدرسة جديدة
        /// </summary>
        public async Task<ResponseDto<SchoolDto>> CreateAsync(CreateSchoolDto createDto)
        {
            try
            {
                // التحقق من وجود الإدارة
                var department = await _unitOfWork.Departments.GetByIdAsync(createDto.DepartmentId);
                if (department == null)
                {
                    return ResponseDto<SchoolDto>.Fail("الإدارة غير موجودة");
                }

                // التحقق من وجود كود مكرر
                if (await _unitOfWork.SchoolRepository.IsCodeExistsAsync(createDto.SchoolCode))
                {
                    return ResponseDto<SchoolDto>.Fail($"الكود {createDto.SchoolCode} موجود بالفعل");
                }

                // التحقق من وجود اسم مكرر
                if (await _unitOfWork.SchoolRepository.IsNameExistsAsync(createDto.SchoolName))
                {
                    return ResponseDto<SchoolDto>.Fail($"الاسم {createDto.SchoolName} موجود بالفعل");
                }

                var school = _mapper.Map<School>(createDto);
                school.CreatedAt = DateTime.Now;
                school.IsActive = true;

                var created = await _unitOfWork.SchoolRepository.AddAsync(school);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<SchoolDto>(created);
                _logger.LogInformation("تم إنشاء مدرسة جديدة: {Name}", created.SchoolName);

                return ResponseDto<SchoolDto>.Ok(dto, "تم إنشاء المدرسة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء مدرسة جديدة");
                return ResponseDto<SchoolDto>.Fail("حدث خطأ أثناء إنشاء المدرسة", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات مدرسة
        /// </summary>
        public async Task<ResponseDto<SchoolDto>> UpdateAsync(int id, UpdateSchoolDto updateDto)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(id);
                if (school == null)
                {
                    return ResponseDto<SchoolDto>.NotFound("المدرسة غير موجودة");
                }

                // التحقق من وجود اسم مكرر
                if (!string.IsNullOrEmpty(updateDto.SchoolName) &&
                    await _unitOfWork.SchoolRepository.IsNameExistsAsync(updateDto.SchoolName, id))
                {
                    return ResponseDto<SchoolDto>.Fail($"الاسم {updateDto.SchoolName} موجود بالفعل");
                }

                // التحقق من وجود كود مكرر
                if (!string.IsNullOrEmpty(updateDto.SchoolCode) &&
                    await _unitOfWork.SchoolRepository.IsCodeExistsAsync(updateDto.SchoolCode, id))
                {
                    return ResponseDto<SchoolDto>.Fail($"الكود {updateDto.SchoolCode} موجود بالفعل");
                }

                // التحقق من وجود الإدارة
                if (updateDto.DepartmentId.HasValue)
                {
                    var department = await _unitOfWork.Departments.GetByIdAsync(updateDto.DepartmentId.Value);
                    if (department == null)
                    {
                        return ResponseDto<SchoolDto>.Fail("الإدارة غير موجودة");
                    }
                }

                _mapper.Map(updateDto, school);
                school.UpdatedAt = DateTime.Now;

                await _unitOfWork.SchoolRepository.UpdateAsync(school);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<SchoolDto>(school);
                _logger.LogInformation("تم تحديث المدرسة: {Name}", school.SchoolName);

                return ResponseDto<SchoolDto>.Ok(dto, "تم تحديث المدرسة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث المدرسة {Id}", id);
                return ResponseDto<SchoolDto>.Fail("حدث خطأ أثناء تحديث المدرسة", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف مدرسة (Soft Delete)
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(id);
                if (school == null)
                {
                    return ResponseDto.NotFound("المدرسة غير موجودة");
                }

                // التحقق من وجود طلاب مرتبطين
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.User.SchoolId == id);
                if (students.Any())
                {
                    return ResponseDto.Fail("لا يمكن حذف المدرسة لأنها تحتوي على طلاب مسجلين");
                }

                school.IsDeleted = true;
                school.IsActive = false;
                school.DeletedAt = DateTime.Now;
                school.UpdatedAt = DateTime.Now;

                await _unitOfWork.SchoolRepository.UpdateAsync(school);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف المدرسة: {Name}", school.SchoolName);
                return ResponseDto.Ok("تم حذف المدرسة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف المدرسة {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف المدرسة", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════


        ///// <summary>
        ///// ✅ التحقق من وجود عام دراسي بنفس الاسم
        ///// </summary>
        //public async Task<ResponseDto<bool>> IsNameExistsAsync(int schoolId, string name, int? excludeId = null)
        //{
        //    try
        //    {
        //        var exists = await _unitOfWork.AcademicYears
        //            .IsNameExistsAsync(schoolId, name, excludeId);
        //        return ResponseDto<bool>.Ok(exists, exists ? "الاسم موجود" : "الاسم غير موجود");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ أثناء التحقق من الاسم {Name}", name);
        //        return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
        //    }
        //}



        /// <summary>
        /// ✅ التحقق من وجود مدرسة بنفس الاسم
        /// </summary>
        public async Task<ResponseDto<bool>> IsNameExistsAsync(string name, int? excludeId = null)
        {
            try
            {
                var exists = await _unitOfWork.SchoolRepository.IsNameExistsAsync(name, excludeId);
                return ResponseDto<bool>.Ok(exists, exists ? "الاسم موجود" : "الاسم غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من الاسم {Name}", name);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        /// <summary>
        /// ✅ التحقق من وجود مدرسة بنفس الكود
        /// </summary>
        public async Task<ResponseDto<bool>> IsCodeExistsAsync(string code, int? excludeId = null)
        {
            try
            {
                var exists = await _unitOfWork.SchoolRepository.IsCodeExistsAsync(code, excludeId);
                return ResponseDto<bool>.Ok(exists, exists ? "الكود موجود" : "الكود غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في IsCodeExistsAsync للمدرسة {Code}", code);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion
    }
}