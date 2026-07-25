using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.ClassRooms;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Teachers;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👨‍🏫  خدمة المعلمين (TeacherService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة المعلمين
    /// 📦  الاستخدام: في TeachersController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class TeacherService : ITeacherService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TeacherService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public TeacherService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TeacherService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ جلب البيانات ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع المعلمين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<TeacherDto>>> GetAllAsync()
        {
            try
            {
                var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<TeacherDto>>(teachers);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    if (user != null)
                    {
                        dto.FullName = user.FullName;
                        dto.Email = user.Email;

                        // ✅ جلب رقم الهاتف من جهات الاتصال

                        var phoneContact = user.Contacts?.FirstOrDefault(c => c.ContactType == ContactType.Phone || c.ContactType == ContactType.Mobile);
                        dto.PhoneNumber = phoneContact?.ContactValue;
                    }

                    var school = await _unitOfWork.SchoolRepository.GetByIdAsync(user?.SchoolId ?? 0);
                    dto.SchoolName = school?.SchoolName;
                }

                _logger.LogInformation("تم جلب {Count} معلم", dtos.Count());
                return ResponseDto<IEnumerable<TeacherDto>>.Ok(dtos, "تم جلب المعلمين بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetAllAsync");
                return ResponseDto<IEnumerable<TeacherDto>>.Fail("حدث خطأ أثناء جلب المعلمين", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على معلم بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<TeacherDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetWithDetailsAsync(id);
                if (teacher == null)
                    return ResponseDto<TeacherDetailsDto>.NotFound("المعلم غير موجود");

                var dto = _mapper.Map<TeacherDetailsDto>(teacher);

                var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                dto.FullName = user?.FullName ?? string.Empty;
                dto.Email = user?.Email;
                var phoneContact = user?.Contacts?.FirstOrDefault(c => c.ContactType == ContactType.Phone || c.ContactType == ContactType.Mobile);
                dto.PhoneNumber = phoneContact?.ContactValue;

                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(user?.SchoolId ?? 0);
                dto.SchoolName = school?.SchoolName;

                // جلب المواد التي يدرسها
                var teacherSubjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.TeacherId == id);
                foreach (var ts in teacherSubjects)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(ts.SubjectId);
                    if (subject != null)
                    {
                        var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
                        dto.Subjects.Add(new SubjectTeacherDto
                        {
                            SubjectId = subject.Id,
                            SubjectName = subject.SubjectName,
                            GradeLevelName = gradeLevel?.GradeName,
                            IsPrimary = ts.IsPrimary
                        });
                    }
                }

                // جلب الفصول التي يشرف عليها
                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.TeacherId == id);
                dto.ClassRooms = _mapper.Map<List<ClassRoomDto>>(classRooms);

                // جلب الإحصائيات
                dto.Statistics = await GetTeacherStatisticsAsync(id);

                return ResponseDto<TeacherDetailsDto>.Ok(dto, "تم جلب المعلم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByIdAsync للمعلم {Id}", id);
                return ResponseDto<TeacherDetailsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على معلم بواسطة الكود
        /// </summary>
        public async Task<ResponseDto<TeacherDto>> GetByCodeAsync(string teacherCode)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetByCodeAsync(teacherCode);
                if (teacher == null)
                    return ResponseDto<TeacherDto>.NotFound("المعلم غير موجود");

                var dto = _mapper.Map<TeacherDto>(teacher);

                var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                dto.FullName = user?.FullName ?? string.Empty;

                return ResponseDto<TeacherDto>.Ok(dto, "تم جلب المعلم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByCodeAsync للمعلم {TeacherCode}", teacherCode);
                return ResponseDto<TeacherDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث والفلترة ════════════════════════════════════

        public async Task<ResponseDto<IEnumerable<TeacherDto>>> GetBySchoolIdAsync(int schoolId)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                    return ResponseDto<IEnumerable<TeacherDto>>.NotFound("المدرسة غير موجودة");

                var teachers = await _unitOfWork.TeacherRepository.GetBySchoolIdAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<TeacherDto>>(teachers);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<TeacherDto>>.Ok(dtos, "تم جلب المعلمين بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetBySchoolIdAsync للمدرسة {SchoolId}", schoolId);
                return ResponseDto<IEnumerable<TeacherDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        public async Task<ResponseDto<IEnumerable<TeacherDto>>> GetBySpecializationAsync(string specialization)
        {
            try
            {
                var teachers = await _unitOfWork.TeacherRepository.GetBySpecializationAsync(specialization);
                var dtos = _mapper.Map<IEnumerable<TeacherDto>>(teachers);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<TeacherDto>>.Ok(dtos, "تم جلب المعلمين حسب التخصص");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetBySpecializationAsync للتخصص {Specialization}", specialization);
                return ResponseDto<IEnumerable<TeacherDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        public async Task<ResponseDto<IEnumerable<TeacherDto>>> GetHomeroomTeachersAsync()
        {
            try
            {
                var teachers = await _unitOfWork.TeacherRepository.GetHomeroomTeachersAsync();
                var dtos = _mapper.Map<IEnumerable<TeacherDto>>(teachers);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<TeacherDto>>.Ok(dtos, "تم جلب معلمي الفصل");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetHomeroomTeachersAsync");
                return ResponseDto<IEnumerable<TeacherDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        public async Task<ResponseDto<IEnumerable<TeacherDto>>> GetBySubjectIdAsync(int subjectId)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);
                if (subject == null)
                    return ResponseDto<IEnumerable<TeacherDto>>.NotFound("المادة غير موجودة");

                var teachers = await _unitOfWork.TeacherRepository.GetBySubjectIdAsync(subjectId);
                var dtos = _mapper.Map<IEnumerable<TeacherDto>>(teachers);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<TeacherDto>>.Ok(dtos, "تم جلب المعلمين حسب المادة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetBySubjectIdAsync للمادة {SubjectId}", subjectId);
                return ResponseDto<IEnumerable<TeacherDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ القوائم المنسدلة ════════════════════════════════════

        public async Task<ResponseDto<IEnumerable<TeacherLookupDto>>> GetLookupAsync(int? schoolId = null)
        {
            try
            {
                IEnumerable<Teacher> teachers;

                if (schoolId.HasValue)
                {
                    teachers = await _unitOfWork.TeacherRepository.GetBySchoolIdAsync(schoolId.Value);
                }
                else
                {
                    teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<TeacherLookupDto>>(teachers);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.Id);
                    dto.FullName = user?.FullName ?? string.Empty;

                    var subjects = await _unitOfWork.TeacherSubjects
                        .FindAsync(ts => ts.TeacherId == dto.Id);
                    dto.SubjectsCount = subjects.Count();
                }

                return ResponseDto<IEnumerable<TeacherLookupDto>>.Ok(dtos, "تم جلب المعلمين للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetLookupAsync");
                return ResponseDto<IEnumerable<TeacherLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ الإحصائيات ════════════════════════════════════

        public async Task<ResponseDto<TeacherStatisticsDto>> GetStatisticsAsync(int teacherId)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
                if (teacher == null)
                    return ResponseDto<TeacherStatisticsDto>.NotFound("المعلم غير موجود");

                var statistics = await GetTeacherStatisticsAsync(teacherId);
                return ResponseDto<TeacherStatisticsDto>.Ok(statistics, "تم جلب إحصائيات المعلم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetStatisticsAsync للمعلم {TeacherId}", teacherId);
                return ResponseDto<TeacherStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        private async Task<TeacherStatisticsDto> GetTeacherStatisticsAsync(int teacherId)
        {
            try
            {
                var teacherSubjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.TeacherId == teacherId);

                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.TeacherId == teacherId);

                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.TeacherId == teacherId);

                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.TeacherId == teacherId);

                var attendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.EmployeeId == teacherId);

                var students = new List<Student>();
                foreach (var classRoom in classRooms)
                {
                    var classStudents = await _unitOfWork.Students
                        .FindAsync(s => s.ClassRoomId == classRoom.Id);
                    students.AddRange(classStudents);
                }

                var totalDays = attendances.Count();
                var presentDays = attendances.Count(a => a.Status == AttendanceStatus.Present);

                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);

                var statistics = new TeacherStatisticsDto
                {
                    TotalSubjects = teacherSubjects.Count(),
                    TotalClassRooms = classRooms.Count(),
                    TotalStudents = students.DistinctBy(s => s.Id).Count(),
                    WeeklyHours = schedules.Count(),
                    TotalExams = exams.Count(),
                    AverageStudentScore = 0,
                    StudentSuccessRate = 0,
                    YearsOfExperience = teacher != null ? DateTime.Now.Year - teacher.HireDate.Year : 0,
                    HomeroomClassRoomsCount = classRooms.Count(c => c.TeacherId == teacherId),
                    HomeroomStudentsCount = students.DistinctBy(s => s.Id).Count(),
                    AbsentDays = attendances.Count(a => a.Status == AttendanceStatus.Absent),
                    LateDays = attendances.Count(a => a.Status == AttendanceStatus.Late),
                    AttendancePercentage = totalDays > 0 ? (decimal)presentDays / totalDays * 100 : 0
                };

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في حساب إحصائيات المعلم {TeacherId}", teacherId);
                return new TeacherStatisticsDto();
            }
        }

        #endregion

        #region ════════════════════════════════════ العمليات الأساسية ════════════════════════════════════

        public async Task<ResponseDto<TeacherDto>> CreateAsync(CreateTeacherDto createDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(createDto.UserId);
                if (user == null)
                    return ResponseDto<TeacherDto>.Fail("المستخدم غير موجود");

                if (await _unitOfWork.TeacherRepository.IsTeacherCodeExistsAsync(createDto.TeacherCode))
                    return ResponseDto<TeacherDto>.Fail($"كود المعلم {createDto.TeacherCode} موجود بالفعل");

                var teacher = _mapper.Map<Teacher>(createDto);
                teacher.CreatedAt = DateTime.Now;
                teacher.IsActive = true;

                var created = await _unitOfWork.TeacherRepository.AddAsync(teacher);
                await _unitOfWork.CompleteAsync();

                // إضافة المواد التي يدرسها
                foreach (var subjectId in createDto.SubjectIds)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);
                    if (subject != null)
                    {
                        var teacherSubject = new TeacherSubject
                        {
                            TeacherId = created.Id,
                            SubjectId = subjectId,
                            IsPrimary = false,
                            CreatedAt = DateTime.Now
                        };
                        await _unitOfWork.TeacherSubjects.AddAsync(teacherSubject);
                    }
                }
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<TeacherDto>(created);
                dto.FullName = user.FullName;

                _logger.LogInformation("تم إنشاء معلم جديد: {TeacherCode}", created.TeacherCode);
                return ResponseDto<TeacherDto>.Ok(dto, "تم إنشاء المعلم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في CreateAsync للمعلم {TeacherCode}", createDto.TeacherCode);
                return ResponseDto<TeacherDto>.Fail("حدث خطأ أثناء إنشاء المعلم", statusCode: 500);
            }
        }

        public async Task<ResponseDto<TeacherDto>> UpdateAsync(int id, UpdateTeacherDto updateDto)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetWithDetailsAsync(id);
                if (teacher == null)
                    return ResponseDto<TeacherDto>.NotFound("المعلم غير موجود");

                _mapper.Map(updateDto, teacher);
                teacher.UpdatedAt = DateTime.Now;

                // تحديث المواد التي يدرسها
                if (updateDto.SubjectIds != null)
                {
                    // حذف المواد القديمة
                    var existingSubjects = await _unitOfWork.TeacherSubjects
                        .FindAsync(ts => ts.TeacherId == id);
                    foreach (var ts in existingSubjects)
                    {
                        await _unitOfWork.TeacherSubjects.DeleteAsync(ts);
                    }

                    // إضافة المواد الجديدة
                    foreach (var subjectId in updateDto.SubjectIds)
                    {
                        var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);
                        if (subject != null)
                        {
                            var teacherSubject = new TeacherSubject
                            {
                                TeacherId = id,
                                SubjectId = subjectId,
                                IsPrimary = false,
                                CreatedAt = DateTime.Now
                            };
                            await _unitOfWork.TeacherSubjects.AddAsync(teacherSubject);
                        }
                    }
                }

                await _unitOfWork.TeacherRepository.UpdateAsync(teacher);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<TeacherDto>(teacher);

                var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                dto.FullName = user?.FullName ?? string.Empty;

                _logger.LogInformation("تم تحديث المعلم: {TeacherCode}", teacher.TeacherCode);
                return ResponseDto<TeacherDto>.Ok(dto, "تم تحديث المعلم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في UpdateAsync للمعلم {Id}", id);
                return ResponseDto<TeacherDto>.Fail("حدث خطأ أثناء تحديث المعلم", statusCode: 500);
            }
        }

        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(id);
                if (teacher == null)
                    return ResponseDto.NotFound("المعلم غير موجود");

                // التحقق من وجود فصول يشرف عليها
                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.TeacherId == id);
                if (classRooms.Any())
                    return ResponseDto.Fail("لا يمكن حذف المعلم لأنه يشرف على فصول");

                // حذف المواد المرتبطة
                var teacherSubjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.TeacherId == id);
                foreach (var ts in teacherSubjects)
                {
                    await _unitOfWork.TeacherSubjects.DeleteAsync(ts);
                }

                teacher.IsDeleted = true;
                teacher.IsActive = false;
                teacher.DeletedAt = DateTime.Now;
                teacher.UpdatedAt = DateTime.Now;

                await _unitOfWork.TeacherRepository.UpdateAsync(teacher);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف المعلم: {TeacherCode}", teacher.TeacherCode);
                return ResponseDto.Ok("تم حذف المعلم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في DeleteAsync للمعلم {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف المعلم", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق ════════════════════════════════════

        public async Task<ResponseDto<bool>> IsTeacherCodeExistsAsync(string teacherCode)
        {
            try
            {
                var exists = await _unitOfWork.TeacherRepository.IsTeacherCodeExistsAsync(teacherCode);
                return ResponseDto<bool>.Ok(exists, exists ? "كود المعلم موجود" : "كود المعلم غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في IsTeacherCodeExistsAsync للمعلم {TeacherCode}", teacherCode);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion
    }
}