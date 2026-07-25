using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.GradeLevels;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📚  خدمة الصفوف الدراسية (GradeLevelService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة الصفوف الدراسية
    /// 📦  الاستخدام: في GradeLevelsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class GradeLevelService : IGradeLevelService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GradeLevelService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public GradeLevelService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GradeLevelService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على الصفوف ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع الصفوف الدراسية
        /// </summary>
        public async Task<ResponseDto<IEnumerable<GradeLevelDto>>> GetAllAsync()
        {
            try
            {
                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<GradeLevelDto>>(gradeLevels);

                foreach (var dto in dtos)
                {
                    // جلب اسم المدرسة
                    var school = await _unitOfWork.SchoolRepository.GetByIdAsync(dto.SchoolId);
                    dto.SchoolName = school?.SchoolName;

                    // جلب اسم المرحلة
                    dto.GradeStageName = GetStageName(dto.GradeStage);

                    // جلب عدد الفصول
                    var classRooms = await _unitOfWork.ClassRooms
                        .FindAsync(c => c.GradeLevelId == dto.Id);
                    dto.ClassRoomsCount = classRooms.Count();

                    // جلب عدد المواد
                    var subjects = await _unitOfWork.Subjects
                        .FindAsync(s => s.GradeLevelId == dto.Id);
                    dto.SubjectsCount = subjects.Count();

                    // جلب عدد الطلاب
                    var students = await _unitOfWork.Students
                        .FindAsync(s => s.ClassRoom != null && s.ClassRoom.GradeLevelId == dto.Id);
                    dto.StudentsCount = students.Count();
                }

                _logger.LogInformation("تم جلب {Count} صف دراسي", dtos.Count());
                return ResponseDto<IEnumerable<GradeLevelDto>>.Ok(dtos, "تم جلب الصفوف الدراسية بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع الصفوف الدراسية");
                return ResponseDto<IEnumerable<GradeLevelDto>>.Fail("حدث خطأ أثناء جلب الصفوف الدراسية", statusCode: 500);
            }
        }


        /// <summary>
        /// 🔍 الحصول على صف بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<GradeLevelDto>> GetByIdAsync(int id)
        {
            try
            {
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(id);
                if (gradeLevel == null)
                {
                    return ResponseDto<GradeLevelDto>.NotFound("الصف الدراسي غير موجود");
                }

                var dto = _mapper.Map<GradeLevelDto>(gradeLevel);

                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(gradeLevel.SchoolId);
                dto.SchoolName = school?.SchoolName;
                dto.GradeStageName = GetStageName(gradeLevel.GradeStage);

                // جلب الإحصائيات
                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.GradeLevelId == id);
                dto.ClassRoomsCount = classRooms.Count();

                var subjects = await _unitOfWork.Subjects
                    .FindAsync(s => s.GradeLevelId == id);
                dto.SubjectsCount = subjects.Count();

                // حساب عدد الطلاب
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.ClassRoom != null && s.ClassRoom.GradeLevelId == id);
                dto.StudentsCount = students.Count();

                return ResponseDto<GradeLevelDto>.Ok(dto, "تم جلب الصف الدراسي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الصف {Id}", id);
                return ResponseDto<GradeLevelDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }


        /// <summary>
        /// 📋 الحصول على الصفوف التابعة لمدرسة معينة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<GradeLevelDto>>> GetBySchoolIdAsync(int schoolId)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<IEnumerable<GradeLevelDto>>.NotFound("المدرسة غير موجودة");
                }

                var gradeLevels = await _unitOfWork.GradeLevels
                    .FindAsync(g => g.SchoolId == schoolId);
                var dtos = _mapper.Map<IEnumerable<GradeLevelDto>>(gradeLevels);

                foreach (var dto in dtos)
                {
                    dto.SchoolName = school.SchoolName;
                    dto.GradeStageName = GetStageName(dto.GradeStage);
                    // جلب عدد الفصول
                    var classRooms = await _unitOfWork.ClassRooms
                        .FindAsync(c => c.GradeLevelId == dto.Id);
                    dto.ClassRoomsCount = classRooms.Count();
                    // جلب عدد المواد
                    var subjects = await _unitOfWork.Subjects
                        .FindAsync(s => s.GradeLevelId == dto.Id);
                    dto.SubjectsCount = subjects.Count();
                    // جلب عدد الطلاب
                    var students = await _unitOfWork.Students
                        .FindAsync(s => s.ClassRoom != null && s.ClassRoom.GradeLevelId == dto.Id);
                    dto.StudentsCount = students.Count();
                }

                return ResponseDto<IEnumerable<GradeLevelDto>>.Ok(dtos, "تم جلب الصفوف الدراسية بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الصفوف للمدرسة {SchoolId}", schoolId);
                return ResponseDto<IEnumerable<GradeLevelDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الصفوف حسب المرحلة الدراسية
        /// </summary>
        public async Task<ResponseDto<IEnumerable<GradeLevelDto>>> GetByStageAsync(int stage)
        {
            try
            {
                var gradeLevels = await _unitOfWork.GradeLevels
                    .FindAsync(g => (int)g.GradeStage == stage);
                var dtos = _mapper.Map<IEnumerable<GradeLevelDto>>(gradeLevels);

                foreach (var dto in dtos)
                {
                    var school = await _unitOfWork.SchoolRepository.GetByIdAsync(dto.SchoolId);
                    dto.SchoolName = school?.SchoolName;
                    dto.GradeStageName = GetStageName(dto.GradeStage);
                    // جلب عدد الفصول
                    var classRooms = await _unitOfWork.ClassRooms
                        .FindAsync(c => c.GradeLevelId == dto.Id);
                    dto.ClassRoomsCount = classRooms.Count();
                    // جلب عدد المواد
                    var subjects = await _unitOfWork.Subjects
                        .FindAsync(s => s.GradeLevelId == dto.Id);
                    dto.SubjectsCount = subjects.Count();
                    // جلب عدد الطلاب
                    var students = await _unitOfWork.Students
                        .FindAsync(s => s.ClassRoom != null && s.ClassRoom.GradeLevelId == dto.Id);
                    dto.StudentsCount = students.Count();
                }

                return ResponseDto<IEnumerable<GradeLevelDto>>.Ok(dtos, "تم جلب الصفوف حسب المرحلة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الصفوف حسب المرحلة {Stage}", stage);
                return ResponseDto<IEnumerable<GradeLevelDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الصفوف للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<GradeLevelLookupDto>>> GetLookupAsync(int? schoolId = null)
        {
            try
            {
                IEnumerable<GradeLevel> gradeLevels;

                if (schoolId.HasValue)
                {
                    gradeLevels = await _unitOfWork.GradeLevels
                        .FindAsync(g => g.SchoolId == schoolId.Value);
                }
                else
                {
                    gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<GradeLevelLookupDto>>(gradeLevels);

                foreach (var dto in dtos)
                {
                    dto.GradeStageName = GetStageName(dto.GradeStage);

                    var school = await _unitOfWork.SchoolRepository.GetByIdAsync(dto.SchoolId);
                    dto.SchoolName = school?.SchoolName;

                    //var classRooms = await _unitOfWork.ClassRooms
                    //    .FindAsync(c => c.GradeLevelId == dto.Id);
                    //dto.ClassRoomsCount = classRooms.Count();
                }

                return ResponseDto<IEnumerable<GradeLevelLookupDto>>.Ok(dtos, "تم جلب الصفوف للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الصفوف للقوائم");
                return ResponseDto<IEnumerable<GradeLevelLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن صف ════════════════════════════════════


        /// <summary>
        /// 📊 الحصول على إحصائيات الصف
        /// </summary>
        public async Task<ResponseDto<GradeLevelStatisticsDto>> GetStatisticsAsync(int gradeLevelId)
        {
            try
            {
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(gradeLevelId);
                if (gradeLevel == null)
                {
                    return ResponseDto<GradeLevelStatisticsDto>.NotFound("الصف الدراسي غير موجود");
                }

                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.GradeLevelId == gradeLevelId);
                var subjects = await _unitOfWork.Subjects
                    .FindAsync(s => s.GradeLevelId == gradeLevelId);
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.ClassRoom != null && s.ClassRoom.GradeLevelId == gradeLevelId);

                // جلب المعلمين من خلال المواد
                var teachers = new List<Teacher>();
                foreach (var subject in subjects)
                {
                    var teacherSubjects = await _unitOfWork.TeacherSubjects
                        .FindAsync(ts => ts.SubjectId == subject.Id);
                    foreach (var ts in teacherSubjects)
                    {
                        var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(ts.TeacherId);
                        if (teacher != null && !teachers.Any(t => t.Id == teacher.Id))
                        {
                            teachers.Add(teacher);
                        }
                    }
                }

                // جلب الامتحانات
                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.Subject.GradeLevelId == gradeLevelId);

                var statistics = new GradeLevelStatisticsDto
                {
                    TotalClassRooms = classRooms.Count(),
                    TotalSubjects = subjects.Count(),
                    TotalStudents = students.Count(),
                    TotalTeachers = teachers.Count,
                    TotalExams = exams.Count(),
                    AverageStudentsPerClass = classRooms.Any() ? students.Count() / classRooms.Count() : 0,
                    SuccessRate = 0, // سيتم حسابه لاحقاً
                    AttendanceRate = 0 // سيتم حسابه لاحقاً
                };

                return ResponseDto<GradeLevelStatisticsDto>.Ok(statistics, "تم جلب إحصائيات الصف");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات الصف {GradeLevelId}", gradeLevelId);
                return ResponseDto<GradeLevelStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء صف جديد
        /// </summary>
        public async Task<ResponseDto<GradeLevelDto>> CreateAsync(CreateGradeLevelDto createDto)
        {
            try
            {
                // التحقق من وجود المدرسة
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(createDto.SchoolId);
                if (school == null)
                {
                    return ResponseDto<GradeLevelDto>.Fail("المدرسة غير موجودة");
                }

                // التحقق من وجود صف بنفس الاسم
                var exists = await _unitOfWork.GradeLevels.IsNameExistsAsync(createDto.GradeName);

                if (exists)
                {
                    return ResponseDto<GradeLevelDto>.Fail($"الصف {createDto.GradeName} موجود بالفعل");
                }

                var gradeLevel = _mapper.Map<GradeLevel>(createDto);
                gradeLevel.CreatedAt = DateTime.Now;
                gradeLevel.IsActive = true;

                var created = await _unitOfWork.GradeLevels.AddAsync(gradeLevel);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<GradeLevelDto>(created);
                dto.SchoolName = school.SchoolName;
                dto.GradeStageName = GetStageName(created.GradeStage);

                _logger.LogInformation("تم إنشاء صف جديد: {Name}", created.GradeName);

                return ResponseDto<GradeLevelDto>.Ok(dto, "تم إنشاء الصف الدراسي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء صف جديد");
                return ResponseDto<GradeLevelDto>.Fail("حدث خطأ أثناء إنشاء الصف", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات صف
        /// </summary>
        public async Task<ResponseDto<GradeLevelDto>> UpdateAsync(int id, UpdateGradeLevelDto updateDto)
        {
            try
            {
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(id);
                if (gradeLevel == null)
                {
                    return ResponseDto<GradeLevelDto>.NotFound("الصف الدراسي غير موجود");
                }

                // التحقق من وجود اسم مكرر
                if (!string.IsNullOrEmpty(updateDto.GradeName) &&
                    await _unitOfWork.GradeLevels.IsNameExistsAsync( updateDto.GradeName, id))
                {
                    return ResponseDto<GradeLevelDto>.Fail($"الاسم {updateDto.GradeName} موجود بالفعل");
                }

                _mapper.Map(updateDto, gradeLevel);
                gradeLevel.UpdatedAt = DateTime.Now;

                await _unitOfWork.GradeLevels.UpdateAsync(gradeLevel);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<GradeLevelDto>(gradeLevel);

                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(gradeLevel.SchoolId);
                dto.SchoolName = school?.SchoolName;

                _logger.LogInformation("تم تحديث الصف: {Name}", gradeLevel.GradeName);



                return ResponseDto<GradeLevelDto>.Ok(dto, "تم تحديث الصف الدراسي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث الصف {Id}", id);
                return ResponseDto<GradeLevelDto>.Fail("حدث خطأ أثناء تحديث الصف", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف صف (Soft Delete)
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(id);
                if (gradeLevel == null)
                {
                    return ResponseDto.NotFound("الصف الدراسي غير موجود");
                }

                // التحقق من وجود فصول تابعة
                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.GradeLevelId == id);
                if (classRooms.Any())
                {
                    return ResponseDto.Fail("لا يمكن حذف الصف لأنه يحتوي على فصول تابعة");
                }

                gradeLevel.IsDeleted = true;
                gradeLevel.IsActive = false;
                gradeLevel.DeletedAt = DateTime.Now;
                gradeLevel.UpdatedAt = DateTime.Now;

                await _unitOfWork.GradeLevels.UpdateAsync(gradeLevel);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف الصف: {Name}", gradeLevel.GradeName);
                return ResponseDto.Ok("تم حذف الصف الدراسي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف الصف {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف الصف", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════


        /// <summary>
        /// ✅ التحقق من وجود صف بنفس الاسم
        /// </summary>
        public async Task<ResponseDto<bool>> IsNameExistsAsync(string name)
        {
            try
            {
                var exists = await _unitOfWork.GradeLevels.IsNameExistsAsync(name);
                return ResponseDto<bool>.Ok(exists, exists ? "الاسم موجود" : "الاسم غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في IsNameExistsAsync للصف {Name}", name);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        /// <summary>
        /// ✅ التحقق من وجود صف بنفس الاسم في المدرسة
        /// </summary>
        public async Task<ResponseDto<bool>> IsNameExistsAsync(int schoolId, string name, int? excludeId = null)
        {
            try
            {
                // ✅ استخدام FindAsync بدلاً من IsNameExistsAsync (لأنها لا تدعم excludeId)
                var existing = await _unitOfWork.GradeLevels
                    .FindAsync(g => g.SchoolId == schoolId && g.GradeName == name);

                var exists = existing.Any() && (excludeId == null || existing.All(g => g.Id != excludeId));

                return ResponseDto<bool>.Ok(exists, exists ? "الاسم موجود" : "الاسم غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من الاسم {Name}", name);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion
        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        private string GetStageName(GradeStage stage)
        {
            return stage switch
            {
                GradeStage.Primary => "ابتدائي",
                GradeStage.Preparatory => "إعدادي",
                GradeStage.Secondary => "ثانوي",
                _ => stage.ToString()
            };
        }

        #endregion

    }
}