using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Students;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🧑‍🎓  خدمة الطلاب (StudentService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة الطلاب
    /// 📦  الاستخدام: في StudentsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class StudentService : IStudentService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public StudentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على الطلاب ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع الطلاب
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentDto>>> GetAllAsync()
        {
            try
            {
                var students = await _unitOfWork.Students.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<StudentDto>>(students);

                foreach (var dto in dtos)
                {
                    // جلب اسم المستخدم
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;

                    // جلب اسم العام الدراسي
                    var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(dto.AcademicYearId);
                    dto.AcademicYearName = academicYear?.YearName;

                    // جلب اسم الفصل والصف
                    if (dto.ClassRoomId.HasValue)
                    {
                        var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(dto.ClassRoomId.Value);
                        if (classRoom != null)
                        {
                            dto.ClassRoomName = classRoom.ClassName;
                            var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(classRoom.GradeLevelId);
                            dto.GradeLevelName = gradeLevel?.GradeName;
                            dto.GradeLevelId = classRoom.GradeLevelId;
                        }
                    }
                }

                _logger.LogInformation("تم جلب {Count} طالب", dtos.Count());
                return ResponseDto<IEnumerable<StudentDto>>.Ok(dtos, "تم جلب الطلاب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع الطلاب");
                return ResponseDto<IEnumerable<StudentDto>>.Fail("حدث خطأ أثناء جلب الطلاب", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الطلاب في فصل معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentDto>>> GetByClassRoomIdAsync(int classRoomId)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(classRoomId);
                if (classRoom == null)
                {
                    return ResponseDto<IEnumerable<StudentDto>>.NotFound("الفصل غير موجود");
                }

                var students = await _unitOfWork.Students.GetStudentsByClassRoomAsync(classRoomId);
                var dtos = _mapper.Map<IEnumerable<StudentDto>>(students);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<StudentDto>>.Ok(dtos, "تم جلب الطلاب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الطلاب للفصل {ClassRoomId}", classRoomId);
                return ResponseDto<IEnumerable<StudentDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الطلاب في صف معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentDto>>> GetByGradeLevelIdAsync(int gradeLevelId)
        {
            try
            {
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(gradeLevelId);
                if (gradeLevel == null)
                {
                    return ResponseDto<IEnumerable<StudentDto>>.NotFound("الصف غير موجود");
                }

                var students = await _unitOfWork.Students.GetStudentsByGradeLevelAsync(gradeLevelId);
                var dtos = _mapper.Map<IEnumerable<StudentDto>>(students);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<StudentDto>>.Ok(dtos, "تم جلب الطلاب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الطلاب للصف {GradeLevelId}", gradeLevelId);
                return ResponseDto<IEnumerable<StudentDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الطلاب في عام دراسي معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentDto>>> GetByAcademicYearIdAsync(int academicYearId)
        {
            try
            {
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(academicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<IEnumerable<StudentDto>>.NotFound("العام الدراسي غير موجود");
                }

                var students = await _unitOfWork.Students.GetStudentsByAcademicYearAsync(academicYearId);
                var dtos = _mapper.Map<IEnumerable<StudentDto>>(students);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;

                }

                return ResponseDto<IEnumerable<StudentDto>>.Ok(dtos, "تم جلب الطلاب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الطلاب للعام الدراسي {AcademicYearId}", academicYearId);
                return ResponseDto<IEnumerable<StudentDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الطلاب تحت إشراف معلم معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentDto>>> GetByTeacherIdAsync(int teacherId)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
                if (teacher == null)
                {
                    return ResponseDto<IEnumerable<StudentDto>>.NotFound("المعلم غير موجود");
                }

                var students = await _unitOfWork.Students.GetStudentsByTeacherAsync(teacherId);
                var dtos = _mapper.Map<IEnumerable<StudentDto>>(students);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }
                return ResponseDto<IEnumerable<StudentDto>>.Ok(dtos, "تم جلب الطلاب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الطلاب للمعلم {TeacherId}", teacherId);
                return ResponseDto<IEnumerable<StudentDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الطلاب المتخرجين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentDto>>> GetGraduatedStudentsAsync()
        {
            try
            {
                var students = await _unitOfWork.Students.GetGraduatedStudentsAsync();
                var dtos = _mapper.Map<IEnumerable<StudentDto>>(students);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<StudentDto>>.Ok(dtos, "تم جلب الطلاب المتخرجين");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الطلاب المتخرجين");
                return ResponseDto<IEnumerable<StudentDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الطلاب النشطين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentDto>>> GetActiveStudentsAsync()
        {
            try
            {
                var students = await _unitOfWork.Students.GetActiveStudentsAsync();
                var dtos = _mapper.Map<IEnumerable<StudentDto>>(students);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<StudentDto>>.Ok(dtos, "تم جلب الطلاب النشطين");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الطلاب النشطين");
                return ResponseDto<IEnumerable<StudentDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الطلاب للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentLookupDto>>> GetLookupAsync(int? classRoomId = null)
        {
            try
            {
                IEnumerable<Student> students;

                if (classRoomId.HasValue)
                {
                    students = await _unitOfWork.Students
                        .FindAsync(s => s.ClassRoomId == classRoomId.Value);
                }
                else
                {
                    students = await _unitOfWork.Students.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<StudentLookupDto>>(students);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.Id);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<StudentLookupDto>>.Ok(dtos, "تم جلب الطلاب للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الطلاب للقوائم");
                return ResponseDto<IEnumerable<StudentLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن طالب ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على طالب بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<StudentDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(id);

                if (student == null)
                    return ResponseDto<StudentDetailsDto>.NotFound("الطالب غير موجود");

                var dto = _mapper.Map<StudentDetailsDto>(student);

                // جلب المستخدم
                var user = await _unitOfWork.Users.GetByIdAsync(student.UserId);
                if (user != null)
                {
                    dto.FullName = user.FullName;
                }

                // جلب اسم العام الدراسي
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(student.AcademicYearId);
                dto.AcademicYearName = academicYear?.YearName;

                // جلب اسم الفصل والصف
                if (student.ClassRoomId.HasValue)
                {
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(student.ClassRoomId.Value);
                    if (classRoom != null)
                    {
                        dto.ClassRoomName = classRoom.ClassName;
                        var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(classRoom.GradeLevelId);
                        dto.GradeLevelName = gradeLevel?.GradeName;
                        dto.GradeLevelId = classRoom.GradeLevelId;
                    }
                }

                // جلب إحصائيات الطالب
                dto.Statistics = await GetStudentStatisticsAsync(student.Id);

                return ResponseDto<StudentDetailsDto>.Ok(dto, "تم جلب الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الطالب {Id}", id);
                return ResponseDto<StudentDetailsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على طالب بواسطة الكود
        /// </summary>
        public async Task<ResponseDto<StudentDto>> GetByCodeAsync(string studentCode)
        {
            try
            {
                var student = await _unitOfWork.Students.GetStudentByCodeAsync(studentCode);
                if (student == null)
                {
                    return ResponseDto<StudentDto>.NotFound("الطالب غير موجود");
                }

                var dto = _mapper.Map<StudentDto>(student);
                var user = await _unitOfWork.Users.GetByIdAsync(student.UserId);
                dto.FullName = user?.FullName ?? string.Empty;

                return ResponseDto<StudentDto>.Ok(dto, "تم جلب الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الطالب بالكود {StudentCode}", studentCode);
                return ResponseDto<StudentDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 الحصول على إحصائيات الطالب
        /// </summary>
        public async Task<ResponseDto<StudentStatisticsDto>> GetStatisticsAsync(int studentId)
        {
            try
            {
                var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);
                if (student == null)
                {
                    return ResponseDto<StudentStatisticsDto>.NotFound("الطالب غير موجود");
                }

                var statistics = await GetStudentStatisticsAsync(studentId);
                return ResponseDto<StudentStatisticsDto>.Ok(statistics, "تم جلب إحصائيات الطالب");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات الطالب {StudentId}", studentId);
                return ResponseDto<StudentStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 حساب إحصائيات الطالب
        /// </summary>
        private async Task<StudentStatisticsDto> GetStudentStatisticsAsync(int studentId)
        {
            try
            {
                // جلب نتائج الامتحانات
                var examResults = await _unitOfWork.ExamResults
                    .FindAsync(er => er.StudentId == studentId);

                // جلب سجلات الحضور
                var attendances = await _unitOfWork.StudentAttendances
                    .FindAsync(sa => sa.StudentId == studentId);

                var statistics = new StudentStatisticsDto
                {
                    TotalExams = examResults.Count(),
                    AverageScore = examResults.Any() ? (decimal)examResults.Average(r => r.Score) : 0,
                    MaxScore = examResults.Any() ? examResults.Max(r => r.Score) : 0,
                    MinScore = examResults.Any() ? examResults.Min(r => r.Score) : 0,
                    PresentDays = attendances.Count(a => a.Status == Domain.Enums.AttendanceStatus.Present),
                    AbsentDays = attendances.Count(a => a.Status == Domain.Enums.AttendanceStatus.Absent),
                    LateDays = attendances.Count(a => a.Status == Domain.Enums.AttendanceStatus.Late),
                    ExcusedDays = attendances.Count(a => a.Status == Domain.Enums.AttendanceStatus.Excused),
                    AttendancePercentage = attendances.Any() 
                        ? (decimal)attendances.Count(a => a.Status == Domain.Enums.AttendanceStatus.Present) / attendances.Count() * 100 
                        : 0,
                    SubjectsCount = 0, // سيتم حسابه لاحقاً
                    PassedSubjects = examResults.Count(r => r.Score >= 50), // افتراض 50% نجاح
                    FailedSubjects = examResults.Count(r => r.Score < 50),
                    SuccessPercentage = examResults.Any() 
                        ? (decimal)examResults.Count(r => r.Score >= 50) / examResults.Count() * 100 
                        : 0,
                    ClassRank = 0 // سيتم حسابه لاحقاً
                };

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حساب إحصائيات الطالب {StudentId}", studentId);
                return new StudentStatisticsDto();
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء طالب جديد
        /// </summary>
        public async Task<ResponseDto<StudentDto>> CreateAsync(CreateStudentDto createDto)
        {
            try
            {
                // التحقق من وجود المستخدم
                var user = await _unitOfWork.Users.GetByIdAsync(createDto.UserId);
                if (user == null)
                {
                    return ResponseDto<StudentDto>.Fail("المستخدم غير موجود");
                }

                // التحقق من وجود العام الدراسي
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(createDto.AcademicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<StudentDto>.Fail("العام الدراسي غير موجود");
                }

                // التحقق من وجود الفصل
                if (createDto.ClassRoomId.HasValue)
                {
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(createDto.ClassRoomId.Value);
                    if (classRoom == null)
                    {
                        return ResponseDto<StudentDto>.Fail("الفصل غير موجود");
                    }
                }

                // التحقق من وجود كود طالب مكرر
                if (await _unitOfWork.Students.StudentCodeExistsAsync(createDto.StudentCode))
                {
                    return ResponseDto<StudentDto>.Fail($"كود الطالب {createDto.StudentCode} موجود بالفعل");
                }

                var student = _mapper.Map<Student>(createDto);
                student.EnrollmentDate = DateTime.Now;
                student.CreatedAt = DateTime.Now;
                student.IsActive = true;

                var created = await _unitOfWork.Students.AddAsync(student);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<StudentDto>(created);
                _logger.LogInformation("تم إنشاء طالب جديد: {StudentCode}", created.StudentCode);

                return ResponseDto<StudentDto>.Ok(dto, "تم إنشاء الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء طالب جديد");
                return ResponseDto<StudentDto>.Fail("حدث خطأ أثناء إنشاء الطالب", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات طالب
        /// </summary>
        public async Task<ResponseDto<StudentDto>> UpdateAsync(int id, UpdateStudentDto updateDto)
        {
            try
            {
                var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(id);
                if (student == null)
                {
                    return ResponseDto<StudentDto>.NotFound("الطالب غير موجود");
                }

                // التحقق من وجود الفصل
                if (updateDto.ClassRoomId.HasValue)
                {
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(updateDto.ClassRoomId.Value);
                    if (classRoom == null)
                    {
                        return ResponseDto<StudentDto>.Fail("الفصل غير موجود");
                    }
                }

                _mapper.Map(updateDto, student);
                student.UpdatedAt = DateTime.Now;

                await _unitOfWork.Students.UpdateAsync(student);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<StudentDto>(student);

                var user = await _unitOfWork.Users.GetByIdAsync(student.UserId);
                dto.FullName = user?.FullName ?? string.Empty;

                _logger.LogInformation("تم تحديث الطالب: {StudentCode}", student.StudentCode);

                return ResponseDto<StudentDto>.Ok(dto, "تم تحديث الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث الطالب {Id}", id);
                return ResponseDto<StudentDto>.Fail("حدث خطأ أثناء تحديث الطالب", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف طالب (Soft Delete)
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(id);
                if (student == null)
                {
                    return ResponseDto.NotFound("الطالب غير موجود");
                }

                student.IsDeleted = true;
                student.IsActive = false;
                student.DeletedAt = DateTime.Now;
                student.UpdatedAt = DateTime.Now;

                await _unitOfWork.Students.UpdateAsync(student);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف الطالب: {StudentCode}", student.StudentCode);
                return ResponseDto.Ok("تم حذف الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف الطالب {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف الطالب", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود كود طالب
        /// </summary>
        public async Task<ResponseDto<bool>> IsStudentCodeExistsAsync(string studentCode)
        {
            try
            {
                var exists = await _unitOfWork.Students.StudentCodeExistsAsync(studentCode);
                return ResponseDto<bool>.Ok(exists, exists ? "كود الطالب موجود" : "كود الطالب غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من كود الطالب {StudentCode}", studentCode);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion
    }
}