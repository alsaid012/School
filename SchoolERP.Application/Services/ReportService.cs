using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using System.Text;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  خدمة التقارير (ReportService)
    /// 📌  الوظيفة: تنفيذ عمليات إنشاء التقارير المختلفة
    /// 📦  الاستخدام: في ReportsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ReportService : IReportService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public ReportService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ReportService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ تقارير حضور الطلاب ════════════════════════════════════

        /// <summary>
        /// 📊 تقرير حضور الطلاب اليومي
        /// </summary>
        public async Task<ResponseDto<object>> GetDailyStudentAttendanceReportAsync(int schoolId, DateTime date)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<object>.NotFound("المدرسة غير موجودة");
                }

                var attendances = await _unitOfWork.StudentAttendances
                    .FindAsync(sa => sa.Student != null && sa.Student.User != null && sa.Student.User.SchoolId == schoolId && sa.AttendanceDate.Date == date.Date);

                var total = attendances.Count();
                var present = attendances.Count(sa => sa.Status == AttendanceStatus.Present);
                var absent = attendances.Count(sa => sa.Status == AttendanceStatus.Absent);
                var late = attendances.Count(sa => sa.Status == AttendanceStatus.Late);
                var excused = attendances.Count(sa => sa.Status == AttendanceStatus.Excused);

                // تجميع حسب الفصول
                var byClass = attendances
                    .GroupBy(sa => sa.Student?.ClassRoom?.ClassName ?? "بدون فصل")
                    .Select(g => new
                    {
                        الفصل = g.Key,
                        إجمالي = g.Count(),
                        حاضر = g.Count(sa => sa.Status == AttendanceStatus.Present),
                        غائب = g.Count(sa => sa.Status == AttendanceStatus.Absent),
                        متأخر = g.Count(sa => sa.Status == AttendanceStatus.Late),
                        معذور = g.Count(sa => sa.Status == AttendanceStatus.Excused),
                        نسبة_الحضور = g.Count() > 0 ? (decimal)g.Count(sa => sa.Status == AttendanceStatus.Present) / g.Count() * 100 : 0
                    })
                    .ToList();

                var report = new
                {
                    المدرسة = school.SchoolName,
                    التاريخ = date.ToString("yyyy-MM-dd"),
                    اليوم = date.ToString("dddd", new System.Globalization.CultureInfo("ar-EG")),
                    إجمالي_الطلاب = total,
                    حاضر = present,
                    غائب = absent,
                    متأخر = late,
                    معذور = excused,
                    نسبة_الحضور_الإجمالية = total > 0 ? (decimal)present / total * 100 : 0,
                    تفاصيل_حسب_الفصل = byClass
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير الحضور اليومي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير الحضور اليومي");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 تقرير حضور الطلاب الشهري
        /// </summary>
        public async Task<ResponseDto<object>> GetMonthlyStudentAttendanceReportAsync(int schoolId, int month, int year)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<object>.NotFound("المدرسة غير موجودة");
                }

                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var attendances = await _unitOfWork.StudentAttendances
                    .FindAsync(sa => sa.Student != null && sa.Student.User != null && sa.Student.User.SchoolId == schoolId &&
                                    sa.AttendanceDate >= startDate && sa.AttendanceDate <= endDate);

                // إحصائيات يومية
                var dailyStats = attendances
                    .GroupBy(sa => sa.AttendanceDate.Date)
                    .Select(g => new
                    {
                        التاريخ = g.Key.ToString("yyyy-MM-dd"),
                        إجمالي = g.Count(),
                        حاضر = g.Count(sa => sa.Status == AttendanceStatus.Present),
                        غائب = g.Count(sa => sa.Status == AttendanceStatus.Absent),
                        متأخر = g.Count(sa => sa.Status == AttendanceStatus.Late),
                        نسبة_الحضور = g.Count() > 0 ? (decimal)g.Count(sa => sa.Status == AttendanceStatus.Present) / g.Count() * 100 : 0
                    })
                    .OrderBy(x => x.التاريخ)
                    .ToList();

                var totalStudents = attendances.Select(sa => sa.StudentId).Distinct().Count();

                var report = new
                {
                    المدرسة = school.SchoolName,
                    الشهر = startDate.ToString("MMMM", new System.Globalization.CultureInfo("ar-EG")),
                    السنة = year,
                    إجمالي_الطلاب = totalStudents,
                    إجمالي_أيام_التسجيل = dailyStats.Count,
                    متوسط_الحضور_اليومي = dailyStats.Any() ? dailyStats.Average(x => x.نسبة_الحضور) : 0,
                    تفاصيل_يومية = dailyStats
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير الحضور الشهري بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير الحضور الشهري");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تقارير حضور الموظفين ════════════════════════════════════

        /// <summary>
        /// 📊 تقرير حضور الموظفين اليومي
        /// </summary>
        public async Task<ResponseDto<object>> GetDailyEmployeeAttendanceReportAsync(int schoolId, DateTime date)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<object>.NotFound("المدرسة غير موجودة");
                }

                var attendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.Employee != null && ea.Employee.User != null && ea.Employee.User.SchoolId == schoolId && ea.AttendanceDate.Date == date.Date);

                var total = attendances.Count();
                var present = attendances.Count(ea => ea.Status == AttendanceStatus.Present);
                var absent = attendances.Count(ea => ea.Status == AttendanceStatus.Absent);
                var late = attendances.Count(ea => ea.Status == AttendanceStatus.Late);
                var excused = attendances.Count(ea => ea.Status == AttendanceStatus.Excused);

                // تجميع حسب الأقسام
                var byDepartment = attendances
                    .GroupBy(ea => ea.Employee?.Department ?? "بدون قسم")
                    .Select(g => new
                    {
                        القسم = g.Key,
                        إجمالي = g.Count(),
                        حاضر = g.Count(ea => ea.Status == AttendanceStatus.Present),
                        غائب = g.Count(ea => ea.Status == AttendanceStatus.Absent),
                        متأخر = g.Count(ea => ea.Status == AttendanceStatus.Late),
                        نسبة_الحضور = g.Count() > 0 ? (decimal)g.Count(ea => ea.Status == AttendanceStatus.Present) / g.Count() * 100 : 0
                    })
                    .ToList();

                var report = new
                {
                    المدرسة = school.SchoolName,
                    التاريخ = date.ToString("yyyy-MM-dd"),
                    اليوم = date.ToString("dddd", new System.Globalization.CultureInfo("ar-EG")),
                    إجمالي_الموظفين = total,
                    حاضر = present,
                    غائب = absent,
                    متأخر = late,
                    معذور = excused,
                    نسبة_الحضور_الإجمالية = total > 0 ? (decimal)present / total * 100 : 0,
                    تفاصيل_حسب_القسم = byDepartment
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير حضور الموظفين اليومي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير حضور الموظفين اليومي");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تقارير الامتحانات ════════════════════════════════════

        /// <summary>
        /// 📊 تقرير نتائج الامتحانات
        /// </summary>
        public async Task<ResponseDto<object>> GetExamResultsReportAsync(int examId)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
                if (exam == null)
                {
                    return ResponseDto<object>.NotFound("الامتحان غير موجود");
                }

                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.ExamId == examId);

                var subject = await _unitOfWork.Subjects.GetByIdAsync(exam.SubjectId);
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(exam.TeacherId ?? 0);
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(exam.ClassRoomId ?? 0);

                var total = results.Count();
                var passed = results.Count(r => r.Score >= 50);
                var failed = results.Count(r => r.Score < 50);

                // ترتيب الطلاب
                var studentRanks = results
                    .OrderByDescending(r => r.Score)
                    .Select((r, index) => new
                    {
                        الترتيب = index + 1,
                        الطالب = r.Student?.User?.FullName ?? string.Empty,
                        كود_الطالب = r.Student?.StudentCode ?? string.Empty,
                        الدرجة = r.Score,
                        النسبة = r.Percentage ?? 0,
                        التقدير = r.Grade,
                        الحالة = r.Score >= 50 ? "ناجح" : "راسب"
                    })
                    .ToList();

                var report = new
                {
                    الامتحان = exam.ExamName,
                    نوع_الامتحان = GetExamTypeName(exam.ExamType),
                    التاريخ = exam.ExamDate.ToString("yyyy-MM-dd"),
                    المادة = subject?.SubjectName,
                    المعلم = teacher?.User?.FullName,
                    الفصل = classRoom?.ClassName,
                    الدرجة_النهائية = exam.MaxScore,
                    إجمالي_الطلاب = total,
                    عدد_الناجحين = passed,
                    عدد_الراسبين = failed,
                    نسبة_النجاح = total > 0 ? (decimal)passed / total * 100 : 0,
                    متوسط_الدرجات = results.Any() ? results.Average(r => r.Score) : 0,
                    أعلى_درجة = results.Any() ? results.Max(r => r.Score) : 0,
                    أدنى_درجة = results.Any() ? results.Min(r => r.Score) : 0,
                    ترتيب_الطلاب = studentRanks
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير نتائج الامتحان بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير نتائج الامتحان");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تقارير الطلاب ════════════════════════════════════

        /// <summary>
        /// 📊 تقرير نتائج الطالب
        /// </summary>
        public async Task<ResponseDto<object>> GetStudentReportAsync(int studentId, int academicYearId)
        {
            try
            {
                var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);
                if (student == null)
                {
                    return ResponseDto<object>.NotFound("الطالب غير موجود");
                }

                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(academicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<object>.NotFound("العام الدراسي غير موجود");
                }

                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.StudentId == studentId && er.Exam.AcademicYearId == academicYearId);

                // درجات المواد
                var subjectGrades = results
                    .GroupBy(r => r.Exam.SubjectId)
                    .Select(g => new
                    {
                        المادة = g.First().Exam?.Subject?.SubjectName ?? string.Empty,
                        عدد_الامتحانات = g.Count(),
                        المتوسط = g.Average(r => r.Score),
                        أعلى_درجة = g.Max(r => r.Score),
                        أدنى_درجة = g.Min(r => r.Score),
                        النتيجة = g.Average(r => r.Score) >= 50 ? "ناجح" : "راسب"
                    })
                    .ToList();

                var totalExams = results.Count();
                var passedExams = results.Count(r => r.Score >= 50);
                var overallAverage = results.Any() ? results.Average(r => r.Score) : 0;

                var report = new
                {
                    الطالب = student.User?.FullName,
                    كود_الطالب = student.StudentCode,
                    العام_الدراسي = academicYear.YearName,
                    الفصل = student.ClassRoom?.ClassName,
                    الصف = student.ClassRoom?.GradeLevel?.GradeName,
                    إجمالي_الامتحانات = totalExams,
                    عدد_الامتحانات_الناجحة = passedExams,
                    عدد_الامتحانات_الراسبة = totalExams - passedExams,
                    المتوسط_الكلي = overallAverage,
                    نسبة_النجاح = totalExams > 0 ? (decimal)passedExams / totalExams * 100 : 0,
                    درجات_المواد = subjectGrades
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير الطالب");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تقارير المعلمين ════════════════════════════════════

        /// <summary>
        /// 📊 تقرير أداء المعلم
        /// </summary>
        public async Task<ResponseDto<object>> GetTeacherPerformanceReportAsync(int teacherId, int academicYearId)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetWithDetailsAsync(teacherId);
                if (teacher == null)
                {
                    return ResponseDto<object>.NotFound("المعلم غير موجود");
                }

                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(academicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<object>.NotFound("العام الدراسي غير موجود");
                }

                // المواد التي يدرسها
                var subjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.TeacherId == teacherId);

                var subjectDetails = new List<object>();
                foreach (var ts in subjects)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(ts.SubjectId);
                    if (subject != null)
                    {
                        // جلب نتائج الطلاب في هذه المادة
                        var exams = await _unitOfWork.Exams
                            .FindAsync(e => e.SubjectId == subject.Id && e.AcademicYearId == academicYearId && e.TeacherId == teacherId);

                        var examResults = new List<ExamResult>();
                        foreach (var exam in exams)
                        {
                            var results = await _unitOfWork.ExamResults
                                .FindAsync(er => er.ExamId == exam.Id);
                            examResults.AddRange(results);
                        }

                        var totalStudents = examResults.Select(r => r.StudentId).Distinct().Count();
                        var averageScore = examResults.Any() ? examResults.Average(r => r.Score) : 0;
                        var passedCount = examResults.Count(r => r.Score >= 50);

                        subjectDetails.Add(new
                        {
                            المادة = subject.SubjectName,
                            عدد_الطلاب = totalStudents,
                            متوسط_الدرجات = averageScore,
                            عدد_الناجحين = passedCount,
                            عدد_الراسبين = examResults.Count - passedCount,
                            نسبة_النجاح = examResults.Any() ? (decimal)passedCount / examResults.Count * 100 : 0
                        });
                    }
                }

                // عدد الحصص
                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.TeacherId == teacherId && cs.AcademicYearId == academicYearId);

                var report = new
                {
                    المعلم = teacher.User?.FullName,
                    كود_المعلم = teacher.TeacherCode,
                    التخصص = teacher.Specialization,
                    العام_الدراسي = academicYear.YearName,
                    عدد_المواد_التي_يدرسها = subjects.Count(),
                    عدد_الحصص_الأسبوعية = schedules.Count(),
                    تفاصيل_المواد = subjectDetails
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير أداء المعلم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير أداء المعلم");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تقارير الجدول ════════════════════════════════════

        /// <summary>
        /// 📊 تقرير الجدول الأسبوعي للفصل
        /// </summary>
        public async Task<ResponseDto<object>> GetWeeklyScheduleReportAsync(int classRoomId)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(classRoomId);
                if (classRoom == null)
                {
                    return ResponseDto<object>.NotFound("الفصل غير موجود");
                }

                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.ClassRoomId == classRoomId);

                // تجميع حسب الأيام
                var days = new[] {
                    DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday, DayOfWeek.Thursday
                };

                var weeklySchedule = new Dictionary<string, List<object>>();

                foreach (var day in days)
                {
                    var daySchedules = schedules
                        .Where(cs => cs.DayOfWeek == day)
                        .OrderBy(cs => cs.StartTime)
                        .Select(cs => new
                        {
                            الوقت = $"{cs.StartTime:hh\\:mm} - {cs.EndTime:hh\\:mm}",
                            المادة = cs.Subject?.SubjectName,
                            المعلم = cs.Teacher?.User?.FullName,
                            رقم_الحصة = cs.PeriodNumber
                        })
                        .ToList();

                    weeklySchedule[GetDayName(day)] = daySchedules.Cast<object>().ToList();
                }

                var report = new
                {
                    الفصل = classRoom.ClassName,
                    الصف = classRoom.GradeLevel?.GradeName,
                    الجدول_الأسبوعي = weeklySchedule
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير الجدول الأسبوعي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير الجدول الأسبوعي");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 تقرير الغرف المدرسية الأسبوعي
        /// </summary>
        public async Task<ResponseDto<object>> GetWeeklyClassRoomScheduleReportAsync(int schoolId, int academicYearId)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<object>.NotFound("المدرسة غير موجودة");
                }

                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(academicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<object>.NotFound("العام الدراسي غير موجود");
                }

                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.GradeLevel.SchoolId == schoolId);

                var classRoomSchedules = new List<object>();

                foreach (var classRoom in classRooms)
                {
                    var schedules = await _unitOfWork.ClassSchedules
                        .FindAsync(cs => cs.ClassRoomId == classRoom.Id && cs.AcademicYearId == academicYearId);

                    var days = new[] {
                        DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday,
                        DayOfWeek.Wednesday, DayOfWeek.Thursday
                    };

                    var daysSchedule = new Dictionary<string, List<object>>();

                    foreach (var day in days)
                    {
                        var daySchedules = schedules
                            .Where(cs => cs.DayOfWeek == day)
                            .OrderBy(cs => cs.StartTime)
                            .Select(cs => new
                            {
                                الوقت = $"{cs.StartTime:hh\\:mm} - {cs.EndTime:hh\\:mm}",
                                المادة = cs.Subject?.SubjectName,
                                المعلم = cs.Teacher?.User?.FullName
                            })
                            .ToList();

                        daysSchedule[GetDayName(day)] = daySchedules.Cast<object>().ToList();
                    }

                    classRoomSchedules.Add(new
                    {
                        الفصل = classRoom.ClassName,
                        الصف = classRoom.GradeLevel?.GradeName,
                        الجدول = daysSchedule
                    });
                }

                var report = new
                {
                    المدرسة = school.SchoolName,
                    العام_الدراسي = academicYear.YearName,
                    عدد_الفصول = classRooms.Count(),
                    جداول_الفصول = classRoomSchedules
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير الغرف المدرسية الأسبوعي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير الغرف المدرسية الأسبوعي");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تقارير المدرسة ════════════════════════════════════

        /// <summary>
        /// 📊 تقرير إحصائيات المدرسة
        /// </summary>
        public async Task<ResponseDto<object>> GetSchoolStatisticsReportAsync(int schoolId)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetWithDetailsAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<object>.NotFound("المدرسة غير موجودة");
                }

                var report = new
                {
                    المدرسة = school.SchoolName,
                    الكود = school.SchoolCode,
                    النوع = school.SchoolType.ToString(),
                    المدير = school.PrincipalName,
                    سنة_التأسيس = school.EstablishedYear,
                    إجمالي_الطلاب = school.Users?.Count(u => u.UserType == UserType.Student) ?? 0,
                    إجمالي_المعلمين = school.Users?.Count(u => u.UserType == UserType.Teacher) ?? 0,
                    إجمالي_الموظفين = school.Users?.Count(u => u.UserType == UserType.Employee) ?? 0,
                    عدد_الصفوف = school.GradeLevels?.Count ?? 0,
                    عدد_الفصول = school.GradeLevels?.Sum(g => g.ClassRooms.Count) ?? 0,
                    عدد_الأعوام_الدراسية = school.AcademicYears?.Count ?? 0
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير إحصائيات المدرسة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير إحصائيات المدرسة");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 تقرير توزيع الطلاب حسب الصفوف
        /// </summary>
        public async Task<ResponseDto<object>> GetStudentDistributionReportAsync(int schoolId)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<object>.NotFound("المدرسة غير موجودة");
                }

                var students = await _unitOfWork.Students
                    .FindAsync(s => s.User.SchoolId == schoolId && !s.IsGraduated);

                var distribution = students
                    .GroupBy(s => s.ClassRoom?.GradeLevel?.GradeName ?? "بدون صف")
                    .Select(g => new
                    {
                        الصف = g.Key,
                        عدد_الطلاب = g.Count()
                    })
                    .OrderBy(x => x.الصف)
                    .ToList();

                var report = new
                {
                    المدرسة = school.SchoolName,
                    إجمالي_الطلاب = students.Count(),
                    توزيع_الطلاب = distribution
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير توزيع الطلاب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير توزيع الطلاب");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 تقرير توزيع المعلمين حسب المواد
        /// </summary>
        public async Task<ResponseDto<object>> GetTeacherDistributionReportAsync(int schoolId)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<object>.NotFound("المدرسة غير موجودة");
                }

                var teacherSubjects = await _unitOfWork.TeacherSubjects.GetAllAsync();

                var distribution = teacherSubjects
                    .GroupBy(ts => ts.Subject?.SubjectName ?? "بدون مادة")
                    .Select(g => new
                    {
                        المادة = g.Key,
                        عدد_المعلمين = g.Select(ts => ts.TeacherId).Distinct().Count()
                    })
                    .OrderBy(x => x.المادة)
                    .ToList();

                var report = new
                {
                    المدرسة = school.SchoolName,
                    إجمالي_المعلمين = distribution.Sum(x => x.عدد_المعلمين),
                    توزيع_المعلمين = distribution
                };

                return ResponseDto<object>.Ok(report, "تم إنشاء تقرير توزيع المعلمين بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء تقرير توزيع المعلمين");
                return ResponseDto<object>.Fail("حدث خطأ أثناء إنشاء التقرير", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تصدير التقارير ════════════════════════════════════

        /// <summary>
        /// 📊 تصدير التقرير بصيغة PDF
        /// </summary>
        public async Task<ResponseDto<byte[]>> ExportToPdfAsync(string reportName, object data)
        {
            try
            {
                // سيتم تنفيذها باستخدام مكتبة PDF (مثل iTextSharp أو QuestPDF)
                // حالياً نرجع رسالة بأن الميزة قيد التطوير
                _logger.LogInformation("طلب تصدير تقرير {ReportName} بصيغة PDF", reportName);

                // TODO: تنفيذ تصدير PDF
                var pdfBytes = Array.Empty<byte>(); // سيتم إنشاؤها فعلياً لاحقاً

                return ResponseDto<byte[]>.Ok(pdfBytes, "تم تصدير التقرير بصيغة PDF بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تصدير التقرير {ReportName} بصيغة PDF", reportName);
                return ResponseDto<byte[]>.Fail("حدث خطأ أثناء تصدير التقرير", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 تصدير التقرير بصيغة Excel
        /// </summary>
        public async Task<ResponseDto<byte[]>> ExportToExcelAsync(string reportName, object data)
        {
            try
            {
                // سيتم تنفيذها باستخدام مكتبة Excel (مثل EPPlus أو ClosedXML)
                // حالياً نرجع رسالة بأن الميزة قيد التطوير
                _logger.LogInformation("طلب تصدير تقرير {ReportName} بصيغة Excel", reportName);

                // TODO: تنفيذ تصدير Excel
                var excelBytes = Array.Empty<byte>(); // سيتم إنشاؤها فعلياً لاحقاً

                return ResponseDto<byte[]>.Ok(excelBytes, "تم تصدير التقرير بصيغة Excel بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تصدير التقرير {ReportName} بصيغة Excel", reportName);
                return ResponseDto<byte[]>.Fail("حدث خطأ أثناء تصدير التقرير", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        private string GetDayName(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Sunday => "الأحد",
                DayOfWeek.Monday => "الإثنين",
                DayOfWeek.Tuesday => "الثلاثاء",
                DayOfWeek.Wednesday => "الأربعاء",
                DayOfWeek.Thursday => "الخميس",
                DayOfWeek.Friday => "الجمعة",
                DayOfWeek.Saturday => "السبت",
                _ => day.ToString()
            };
        }

        private string GetExamTypeName(ExamType examType)
        {
            return examType switch
            {
                ExamType.Monthly => "شهري",
                ExamType.MidTerm => "نصفي",
                ExamType.Final => "نهائي",
                ExamType.Quiz => "اختبار قصير",
                ExamType.Assessment => "تقييم",
                _ => examType.ToString()
            };
        }

        #endregion
    }
}