using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.ClassRooms;
using SchoolERP.Application.DTOs.ClassSchedules;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Exams;
using SchoolERP.Application.DTOs.Students;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏫  خدمة الفصول الدراسية (ClassRoomService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة الفصول الدراسية
    /// 📦  الاستخدام: في ClassRoomsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassRoomService : IClassRoomService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ClassRoomService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public ClassRoomService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ClassRoomService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ جلب البيانات ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع الفصول الدراسية
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassRoomDto>>> GetAllAsync()
        {
            try
            {
                var classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ClassRoomDto>>(classRooms);

                foreach (var dto in dtos)
                {
                    // جلب اسم الصف
                    var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(dto.GradeLevelId);
                    dto.GradeLevelName = gradeLevel?.GradeName;

                    // جلب اسم المعلم
                    if (dto.TeacherId.HasValue)
                    {
                        var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId.Value);
                        if (teacher != null)
                        {
                            var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                            dto.TeacherName = user?.FullName;
                        }
                    }

                    // جلب عدد الطلاب
                    var students = await _unitOfWork.Students
                        .FindAsync(s => s.ClassRoomId == dto.Id);
                    dto.StudentsCount = students.Count();
                }

                _logger.LogInformation("تم جلب {Count} فصل دراسي", dtos.Count());
                return ResponseDto<IEnumerable<ClassRoomDto>>.Ok(dtos, "تم جلب الفصول الدراسية بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetAllAsync");
                return ResponseDto<IEnumerable<ClassRoomDto>>.Fail("حدث خطأ أثناء جلب الفصول", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على فصل بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<ClassRoomDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(id);
                if (classRoom == null)
                    return ResponseDto<ClassRoomDetailsDto>.NotFound("الفصل غير موجود");

                var dto = _mapper.Map<ClassRoomDetailsDto>(classRoom);

                // جلب اسم الصف
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(classRoom.GradeLevelId);
                dto.GradeLevelName = gradeLevel?.GradeName;

                // جلب اسم المعلم
                if (classRoom.TeacherId.HasValue)
                {
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(classRoom.TeacherId.Value);
                    if (teacher != null)
                    {
                        var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                        dto.TeacherName = user?.FullName;
                    }
                }

                // جلب الطلاب
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.ClassRoomId == id);
                dto.Students = _mapper.Map<List<StudentDto>>(students);
                dto.StudentsCount = students.Count();

                // جلب جدول الحصص
                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.ClassRoomId == id);
                dto.Schedules = _mapper.Map<List<ClassScheduleDto>>(schedules);

                // جلب الامتحانات
                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.ClassRoomId == id);
                dto.Exams = _mapper.Map<List<ExamDto>>(exams);

                // جلب الإحصائيات
                dto.Statistics = await GetClassRoomStatisticsAsync(id);

                return ResponseDto<ClassRoomDetailsDto>.Ok(dto, "تم جلب الفصل بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByIdAsync للفصل {Id}", id);
                return ResponseDto<ClassRoomDetailsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث والفلترة ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على الفصول التابعة لصف معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassRoomDto>>> GetByGradeLevelIdAsync(int gradeLevelId)
        {
            try
            {
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(gradeLevelId);
                if (gradeLevel == null)
                    return ResponseDto<IEnumerable<ClassRoomDto>>.NotFound("الصف غير موجود");

                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.GradeLevelId == gradeLevelId);
                var dtos = _mapper.Map<IEnumerable<ClassRoomDto>>(classRooms);

                foreach (var dto in dtos)
                {
                    dto.GradeLevelName = gradeLevel.GradeName;
                }

                return ResponseDto<IEnumerable<ClassRoomDto>>.Ok(dtos, "تم جلب الفصول بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByGradeLevelIdAsync للصف {GradeLevelId}", gradeLevelId);
                return ResponseDto<IEnumerable<ClassRoomDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الفصول التابعة لمدرسة معينة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassRoomDto>>> GetBySchoolIdAsync(int schoolId)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                    return ResponseDto<IEnumerable<ClassRoomDto>>.NotFound("المدرسة غير موجودة");

                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.GradeLevel.SchoolId == schoolId);
                var dtos = _mapper.Map<IEnumerable<ClassRoomDto>>(classRooms);

                return ResponseDto<IEnumerable<ClassRoomDto>>.Ok(dtos, "تم جلب الفصول بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetBySchoolIdAsync للمدرسة {SchoolId}", schoolId);
                return ResponseDto<IEnumerable<ClassRoomDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الفصول التي يشرف عليها معلم معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassRoomDto>>> GetByTeacherIdAsync(int teacherId)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
                if (teacher == null)
                    return ResponseDto<IEnumerable<ClassRoomDto>>.NotFound("المعلم غير موجود");

                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.TeacherId == teacherId);
                var dtos = _mapper.Map<IEnumerable<ClassRoomDto>>(classRooms);

                return ResponseDto<IEnumerable<ClassRoomDto>>.Ok(dtos, "تم جلب الفصول بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByTeacherIdAsync للمعلم {TeacherId}", teacherId);
                return ResponseDto<IEnumerable<ClassRoomDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ القوائم المنسدلة ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على الفصول للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassRoomLookupDto>>> GetLookupAsync(int? gradeLevelId = null)
        {
            try
            {
                IEnumerable<ClassRoom> classRooms;

                if (gradeLevelId.HasValue)
                {
                    classRooms = await _unitOfWork.ClassRooms
                        .FindAsync(c => c.GradeLevelId == gradeLevelId.Value);
                }
                else
                {
                    classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<ClassRoomLookupDto>>(classRooms);

                foreach (var dto in dtos)
                {
                    // جلب اسم الصف
                    var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(dto.Id);
                    dto.GradeLevelName = gradeLevel?.GradeName;
                }

                return ResponseDto<IEnumerable<ClassRoomLookupDto>>.Ok(dtos, "تم جلب الفصول للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetLookupAsync");
                return ResponseDto<IEnumerable<ClassRoomLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ الإحصائيات ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصائيات الفصل
        /// </summary>
        public async Task<ResponseDto<ClassRoomStatisticsDto>> GetStatisticsAsync(int classRoomId)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(classRoomId);
                if (classRoom == null)
                    return ResponseDto<ClassRoomStatisticsDto>.NotFound("الفصل غير موجود");

                var statistics = await GetClassRoomStatisticsAsync(classRoomId);
                return ResponseDto<ClassRoomStatisticsDto>.Ok(statistics, "تم جلب إحصائيات الفصل");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetStatisticsAsync للفصل {ClassRoomId}", classRoomId);
                return ResponseDto<ClassRoomStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        private async Task<ClassRoomStatisticsDto> GetClassRoomStatisticsAsync(int classRoomId)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(classRoomId);
                if (classRoom == null)
                    return new ClassRoomStatisticsDto();

                var students = await _unitOfWork.Students
                    .FindAsync(s => s.ClassRoomId == classRoomId);
                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.ClassRoomId == classRoomId);
                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.ClassRoomId == classRoomId);

                var teachers = new List<Teacher>();
                foreach (var schedule in schedules)
                {
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(schedule.TeacherId);
                    if (teacher != null && !teachers.Any(t => t.Id == teacher.Id))
                    {
                        teachers.Add(teacher);
                    }
                }

                var subjects = schedules.Select(s => s.SubjectId).Distinct().Count();

                var statistics = new ClassRoomStatisticsDto
                {
                    TotalStudents = students.Count(),
                    TotalSubjects = subjects,
                    TotalTeachers = teachers.Count,
                    TotalExams = exams.Count(),
                    WeeklyHours = schedules.Count(),
                    OccupancyRate = classRoom.Capacity > 0 ? (decimal)students.Count() / classRoom.Capacity * 100 : 0,
                    AttendanceRate = 0,
                    SuccessRate = 0,
                    MaleStudents = 0,
                    FemaleStudents = 0
                };

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في حساب إحصائيات الفصل {ClassRoomId}", classRoomId);
                return new ClassRoomStatisticsDto();
            }
        }

        #endregion

        #region ════════════════════════════════════ العمليات الأساسية ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء فصل جديد
        /// </summary>
        public async Task<ResponseDto<ClassRoomDto>> CreateAsync(CreateClassRoomDto createDto)
        {
            try
            {
                // التحقق من وجود الصف
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(createDto.GradeLevelId);
                if (gradeLevel == null)
                    return ResponseDto<ClassRoomDto>.Fail("الصف غير موجود");

                // التحقق من وجود المعلم
                if (createDto.TeacherId.HasValue)
                {
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(createDto.TeacherId.Value);
                    if (teacher == null)
                        return ResponseDto<ClassRoomDto>.Fail("المعلم غير موجود");
                }

                // التحقق من وجود اسم مكرر
                if (await _unitOfWork.ClassRooms.IsNameExistsAsync(createDto.ClassName,createDto.GradeLevelId ))
                    return ResponseDto<ClassRoomDto>.Fail($"الفصل {createDto.ClassName} موجود بالفعل");

                var classRoom = _mapper.Map<ClassRoom>(createDto);
                classRoom.CreatedAt = DateTime.Now;
                classRoom.IsActive = true;

                var created = await _unitOfWork.ClassRooms.AddAsync(classRoom);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<ClassRoomDto>(created);
                dto.GradeLevelName = gradeLevel.GradeName;

                _logger.LogInformation("تم إنشاء فصل جديد: {ClassName}", created.ClassName);
                return ResponseDto<ClassRoomDto>.Ok(dto, "تم إنشاء الفصل بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في CreateAsync للفصل {ClassName}", createDto.ClassName);
                return ResponseDto<ClassRoomDto>.Fail("حدث خطأ أثناء إنشاء الفصل", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات فصل
        /// </summary>
        public async Task<ResponseDto<ClassRoomDto>> UpdateAsync(int id, UpdateClassRoomDto updateDto)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(id);
                if (classRoom == null)
                    return ResponseDto<ClassRoomDto>.NotFound("الفصل غير موجود");

                // التحقق من وجود الصف
                if (updateDto.GradeLevelId.HasValue)
                {
                    // ✅ تغيير الاسم من gradeLevel إلى gradeLevelCheck
                    var gradeLevelCheck = await _unitOfWork.GradeLevels.GetByIdAsync(updateDto.GradeLevelId.Value);
                    if (gradeLevelCheck == null)
                        return ResponseDto<ClassRoomDto>.Fail("الصف غير موجود");
                }

                // التحقق من وجود المعلم
                if (updateDto.TeacherId.HasValue)
                {
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(updateDto.TeacherId.Value);
                    if (teacher == null)
                        return ResponseDto<ClassRoomDto>.Fail("المعلم غير موجود");
                }

                // ✅ التحقق من وجود اسم مكرر (استخدام FindAsync بدلاً من IsNameExistsAsync)
                if (!string.IsNullOrEmpty(updateDto.ClassName))
                {
                    var gradeLevelId = updateDto.GradeLevelId ?? classRoom.GradeLevelId;
                    var existing = await _unitOfWork.ClassRooms
                        .FindAsync(c => c.GradeLevelId == gradeLevelId && c.ClassName == updateDto.ClassName && c.Id != id);
                    if (existing.Any())
                        return ResponseDto<ClassRoomDto>.Fail($"الاسم {updateDto.ClassName} موجود بالفعل");
                }

                _mapper.Map(updateDto, classRoom);
                classRoom.UpdatedAt = DateTime.Now;

                await _unitOfWork.ClassRooms.UpdateAsync(classRoom);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<ClassRoomDto>(classRoom);

                // ✅ تغيير الاسم من gradeLevel إلى gradeLevelInfo
                var gradeLevelInfo = await _unitOfWork.GradeLevels.GetByIdAsync(classRoom.GradeLevelId);
                dto.GradeLevelName = gradeLevelInfo?.GradeName;

                _logger.LogInformation("تم تحديث الفصل: {ClassName}", classRoom.ClassName);
                return ResponseDto<ClassRoomDto>.Ok(dto, "تم تحديث الفصل بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في UpdateAsync للفصل {Id}", id);
                return ResponseDto<ClassRoomDto>.Fail("حدث خطأ أثناء تحديث الفصل", statusCode: 500);
            }
        }
        /// <summary>
        /// 🗑️ حذف فصل (Soft Delete)
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(id);
                if (classRoom == null)
                    return ResponseDto.NotFound("الفصل غير موجود");

                // التحقق من وجود طلاب
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.ClassRoomId == id);
                if (students.Any())
                    return ResponseDto.Fail("لا يمكن حذف الفصل لأنه يحتوي على طلاب");

                classRoom.IsDeleted = true;
                classRoom.IsActive = false;
                classRoom.DeletedAt = DateTime.Now;
                classRoom.UpdatedAt = DateTime.Now;

                await _unitOfWork.ClassRooms.UpdateAsync(classRoom);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف الفصل: {ClassName}", classRoom.ClassName);
                return ResponseDto.Ok("تم حذف الفصل بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في DeleteAsync للفصل {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف الفصل", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود فصل بنفس الاسم في الصف
        /// </summary>
        public async Task<ResponseDto<bool>> IsNameExistsAsync(int gradeLevelId, string name, int? excludeId = null)
        {
            try
            {
                // ✅ استخدام FindAsync بدلاً من IsNameExistsAsync
                var existing = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.GradeLevelId == gradeLevelId && c.ClassName == name);

                // ✅ التحقق من excludeId
                var exists = existing.Any() && (excludeId == null || existing.All(c => c.Id != excludeId));

                return ResponseDto<bool>.Ok(exists, exists ? "الاسم موجود" : "الاسم غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في IsNameExistsAsync للفصل {Name}", name);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion
    }
}