using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.ExamResults;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  خدمة نتائج الامتحانات (ExamResultService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة نتائج الامتحانات
    /// 📦  الاستخدام: في ExamResultsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ExamResultService : IExamResultService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExamResultService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public ExamResultService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ExamResultService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على النتائج ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع نتائج الامتحانات
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamResultDto>>> GetAllAsync()
        {
            try
            {
                var results = await _unitOfWork.ExamResults.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ExamResultDto>>(results);

                foreach (var dto in dtos)
                {
                    await PopulateExamResultDto(dto);
                }

                _logger.LogInformation("تم جلب {Count} نتيجة", dtos.Count());
                return ResponseDto<IEnumerable<ExamResultDto>>.Ok(dtos, "تم جلب النتائج بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع النتائج");
                return ResponseDto<IEnumerable<ExamResultDto>>.Fail("حدث خطأ أثناء جلب النتائج", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على نتائج امتحان معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamResultDto>>> GetByExamIdAsync(int examId)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
                if (exam == null)
                {
                    return ResponseDto<IEnumerable<ExamResultDto>>.NotFound("الامتحان غير موجود");
                }

                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.ExamId == examId);
                var dtos = _mapper.Map<IEnumerable<ExamResultDto>>(results);

                foreach (var dto in dtos)
                {
                    await PopulateExamResultDto(dto);
                }

                return ResponseDto<IEnumerable<ExamResultDto>>.Ok(dtos, "تم جلب النتائج بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب نتائج الامتحان {ExamId}", examId);
                return ResponseDto<IEnumerable<ExamResultDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على نتائج طالب معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamResultDto>>> GetByStudentIdAsync(int studentId)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(studentId);
                if (student == null)
                {
                    return ResponseDto<IEnumerable<ExamResultDto>>.NotFound("الطالب غير موجود");
                }

                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.StudentId == studentId);
                var dtos = _mapper.Map<IEnumerable<ExamResultDto>>(results);

                foreach (var dto in dtos)
                {
                    await PopulateExamResultDto(dto);
                }

                return ResponseDto<IEnumerable<ExamResultDto>>.Ok(dtos, "تم جلب نتائج الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب نتائج الطالب {StudentId}", studentId);
                return ResponseDto<IEnumerable<ExamResultDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على نتائج طالب في عام دراسي معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamResultDto>>> GetByStudentAndAcademicYearAsync(int studentId, int academicYearId)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(studentId);
                if (student == null)
                {
                    return ResponseDto<IEnumerable<ExamResultDto>>.NotFound("الطالب غير موجود");
                }

                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(academicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<IEnumerable<ExamResultDto>>.NotFound("العام الدراسي غير موجود");
                }

                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.StudentId == studentId && er.Exam.AcademicYearId == academicYearId);
                var dtos = _mapper.Map<IEnumerable<ExamResultDto>>(results);

                foreach (var dto in dtos)
                {
                    await PopulateExamResultDto(dto);
                }

                return ResponseDto<IEnumerable<ExamResultDto>>.Ok(dtos, "تم جلب نتائج الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب نتائج الطالب {StudentId} للعام الدراسي {AcademicYearId}", studentId, academicYearId);
                return ResponseDto<IEnumerable<ExamResultDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على نتائج فصل معين في امتحان معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamResultDto>>> GetByClassRoomAndExamAsync(int classRoomId, int examId)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(classRoomId);
                if (classRoom == null)
                {
                    return ResponseDto<IEnumerable<ExamResultDto>>.NotFound("الفصل غير موجود");
                }

                var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
                if (exam == null)
                {
                    return ResponseDto<IEnumerable<ExamResultDto>>.NotFound("الامتحان غير موجود");
                }

                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.ExamId == examId && er.Student.ClassRoomId == classRoomId);
                var dtos = _mapper.Map<IEnumerable<ExamResultDto>>(results);

                foreach (var dto in dtos)
                {
                    await PopulateExamResultDto(dto);
                }

                return ResponseDto<IEnumerable<ExamResultDto>>.Ok(dtos, "تم جلب النتائج بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب نتائج الفصل {ClassRoomId} للامتحان {ExamId}", classRoomId, examId);
                return ResponseDto<IEnumerable<ExamResultDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        ///// <summary>
        ///// 📋 الحصول على ترتيب الطلاب في امتحان معين
        ///// </summary>
        //public async Task<ResponseDto<IEnumerable<StudentRankDto>>> GetRankedResultsAsync(int examId)
        //{
        //    try
        //    {
        //        var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
        //        if (exam == null)
        //        {
        //            return ResponseDto<IEnumerable<StudentRankDto>>.NotFound("الامتحان غير موجود");
        //        }

        //        // ✅ جلب النتائج مع الطلاب والمستخدمين
        //        var results = await _unitOfWork.ExamResults
        //            .FindAsync(er => er.ExamId == examId);

        //        var rankedResults = results
        //            .OrderByDescending(r => r.Score)
        //            .Select((r, index) => new StudentRankDto
        //            {
        //                StudentId = r.StudentId,
        //                StudentName = r.Student?.User?.FullName ?? string.Empty,
        //                Score = r.Score,
        //                Percentage = r.Percentage ?? 0,
        //                Rank = index + 1,
        //                Grade = r.Grade,
        //                IsPassed = r.Score >= 50
        //            })
        //            .ToList();

        //        return ResponseDto<IEnumerable<StudentRankDto>>.Ok(rankedResults, "تم جلب ترتيب الطلاب بنجاح");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ أثناء جلب ترتيب الطلاب للامتحان {ExamId}", examId);
        //        return ResponseDto<IEnumerable<StudentRankDto>>.Fail("حدث خطأ", statusCode: 500);
        //    }
        //}

        /// <summary>
        /// 📋 الحصول على ترتيب الطلاب في امتحان معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentRankDto>>> GetRankedResultsAsync(int examId)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
                if (exam == null)
                {
                    return ResponseDto<IEnumerable<StudentRankDto>>.NotFound("الامتحان غير موجود");
                }

                // ✅ جلب النتائج مع الطلاب والمستخدمين باستخدام Include
                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.ExamId == examId);

                // ✅ تحويل النتائج إلى قائمة مع أسماء الطلاب
                var rankedResults = new List<StudentRankDto>();
                var rank = 1;

                // ✅ ترتيب النتائج تنازلياً حسب الدرجة
                var sortedResults = results.OrderByDescending(r => r.Score).ToList();

                foreach (var result in sortedResults)
                {
                    // ✅ جلب الطالب
                    var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(result.StudentId);

                    // ✅ الحصول على اسم الطالب
                    string studentName = "غير معروف";
                    if (student != null)
                    {
                        studentName = student.User?.FullName ?? student.StudentCode ?? "غير معروف";
                    }

                    rankedResults.Add(new StudentRankDto
                    {
                        StudentId = result.StudentId,
                        StudentName = studentName,
                        Score = result.Score,
                        Percentage = result.Percentage ?? 0,
                        Rank = rank++,
                        Grade = result.Grade ?? GetGrade(result.Percentage ?? 0),
                        IsPassed = result.Score >= 50
                    });
                }

                return ResponseDto<IEnumerable<StudentRankDto>>.Ok(rankedResults, "تم جلب ترتيب الطلاب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب ترتيب الطلاب للامتحان {ExamId}", examId);
                return ResponseDto<IEnumerable<StudentRankDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على النتائج للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamResultLookupDto>>> GetLookupAsync(int? examId = null)
        {
            try
            {
                IEnumerable<ExamResult> results;

                if (examId.HasValue)
                {
                    results = await _unitOfWork.ExamResults
                        .FindAsync(er => er.ExamId == examId.Value);
                }
                else
                {
                    results = await _unitOfWork.ExamResults.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<ExamResultLookupDto>>(results);

                foreach (var dto in dtos)
                {
                    var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(dto.StudentId);
                    var exam = await _unitOfWork.Exams.GetByIdAsync(dto.ExamId);

                    dto.StudentName = student?.User?.FullName;
                    dto.ExamName = exam?.ExamName;
                }

                return ResponseDto<IEnumerable<ExamResultLookupDto>>.Ok(dtos, "تم جلب النتائج للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب النتائج للقوائم");
                return ResponseDto<IEnumerable<ExamResultLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن نتيجة ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على نتيجة بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<ExamResultDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _unitOfWork.ExamResults.GetByIdAsync(id);
                if (result == null)
                {
                    return ResponseDto<ExamResultDto>.NotFound("النتيجة غير موجودة");
                }

                var dto = _mapper.Map<ExamResultDto>(result);
                await PopulateExamResultDto(dto);

                return ResponseDto<ExamResultDto>.Ok(dto, "تم جلب النتيجة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب النتيجة {Id}", id);
                return ResponseDto<ExamResultDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على نتيجة طالب في امتحان معين
        /// </summary>
        public async Task<ResponseDto<ExamResultDto>> GetByExamAndStudentAsync(int examId, int studentId)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
                if (exam == null)
                {
                    return ResponseDto<ExamResultDto>.NotFound("الامتحان غير موجود");
                }

                var student = await _unitOfWork.Students.GetByIdAsync(studentId);
                if (student == null)
                {
                    return ResponseDto<ExamResultDto>.NotFound("الطالب غير موجود");
                }

                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.ExamId == examId && er.StudentId == studentId);
                var result = results.FirstOrDefault();

                if (result == null)
                {
                    return ResponseDto<ExamResultDto>.NotFound("النتيجة غير موجودة");
                }

                var dto = _mapper.Map<ExamResultDto>(result);
                await PopulateExamResultDto(dto);

                return ResponseDto<ExamResultDto>.Ok(dto, "تم جلب النتيجة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب النتيجة للامتحان {ExamId} والطالب {StudentId}", examId, studentId);
                return ResponseDto<ExamResultDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ الإحصائيات ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصائيات نتائج الامتحان
        /// </summary>
        public async Task<ResponseDto<ExamResultStatisticsDto>> GetStatisticsAsync(int examId)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
                if (exam == null)
                {
                    return ResponseDto<ExamResultStatisticsDto>.NotFound("الامتحان غير موجود");
                }

                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.ExamId == examId);

                var statistics = new ExamResultStatisticsDto
                {
                    TotalStudents = results.Count(),
                    PassedStudents = results.Count(r => r.Score >= 50),
                    FailedStudents = results.Count(r => r.Score < 50),
                    ExcellentStudents = results.Count(r => r.Score >= 90),
                    GoodStudents = results.Count(r => r.Score >= 80 && r.Score < 90),
                    PassedOnlyStudents = results.Count(r => r.Score >= 50 && r.Score < 80),
                    FailedOnlyStudents = results.Count(r => r.Score < 50),
                    AverageScore = results.Any() ? (decimal)results.Average(r => r.Score) : 0,
                    MaxScore = results.Any() ? results.Max(r => r.Score) : 0,
                    MinScore = results.Any() ? results.Min(r => r.Score) : 0,
                    SuccessRate = results.Any() ? (decimal)results.Count(r => r.Score >= 50) / results.Count() * 100 : 0,
                    GradeDistribution = new Dictionary<string, int>(),
                    StudentRanks = new List<StudentRankDto>()
                };

                // توزيع الدرجات حسب التقديرات
                var gradeDistribution = new Dictionary<string, int>
                {
                    { "A (90-100)", results.Count(r => r.Score >= 90) },
                    { "B (80-89)", results.Count(r => r.Score >= 80 && r.Score < 90) },
                    { "C (70-79)", results.Count(r => r.Score >= 70 && r.Score < 80) },
                    { "D (60-69)", results.Count(r => r.Score >= 60 && r.Score < 70) },
                    { "F (0-59)", results.Count(r => r.Score < 60) }
                };
                statistics.GradeDistribution = gradeDistribution;

                // ترتيب الطلاب
                var rankedResults = results
                    .OrderByDescending(r => r.Score)
                    .Select((r, index) => new StudentRankDto
                    {
                        StudentId = r.StudentId,
                        StudentName = r.Student?.User?.FullName ?? string.Empty,
                        Score = r.Score,
                        Percentage = r.Percentage ?? 0,
                        Rank = index + 1,
                        Grade = r.Grade,
                        IsPassed = r.Score >= 50
                    })
                    .ToList();
                statistics.StudentRanks = rankedResults;

                return ResponseDto<ExamResultStatisticsDto>.Ok(statistics, "تم جلب إحصائيات النتائج");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات النتائج للامتحان {ExamId}", examId);
                return ResponseDto<ExamResultStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 الحصول على متوسط درجات طالب
        /// </summary>
        public async Task<ResponseDto<object>> GetStudentAverageAsync(int studentId, int academicYearId)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(studentId);
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

                var groupedBySubject = results
                    .GroupBy(r => r.Exam.SubjectId)
                    .Select(g => new
                    {
                        SubjectId = g.Key,
                        SubjectName = g.First().Exam?.Subject?.SubjectName ?? string.Empty,
                        ExamsCount = g.Count(),
                        Average = g.Average(r => r.Score),
                        Max = g.Max(r => r.Score),
                        Min = g.Min(r => r.Score)
                    })
                    .ToList();

                var result = new
                {
                    StudentId = studentId,
                    StudentName = student.User?.FullName,
                    AcademicYearId = academicYearId,
                    AcademicYearName = academicYear.YearName,
                    TotalExams = results.Count(),
                    OverallAverage = results.Any() ? results.Average(r => r.Score) : 0,
                    PassedExams = results.Count(r => r.Score >= 50),
                    FailedExams = results.Count(r => r.Score < 50),
                    SuccessRate = results.Any() ? (double)results.Count(r => r.Score >= 50) / results.Count() * 100 : 0,
                    Subjects = groupedBySubject
                };

                return ResponseDto<object>.Ok(result, "تم جلب متوسط درجات الطالب");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب متوسط درجات الطالب {StudentId} للعام الدراسي {AcademicYearId}", studentId, academicYearId);
                return ResponseDto<object>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إضافة نتيجة جديدة
        /// </summary>
        public async Task<ResponseDto<ExamResultDto>> CreateAsync(CreateExamResultDto createDto)
        {
            try
            {
                // التحقق من وجود الامتحان
                var exam = await _unitOfWork.Exams.GetByIdAsync(createDto.ExamId);
                if (exam == null)
                {
                    return ResponseDto<ExamResultDto>.Fail("الامتحان غير موجود");
                }

                // التحقق من وجود الطالب
                var student = await _unitOfWork.Students.GetByIdAsync(createDto.StudentId);
                if (student == null)
                {
                    return ResponseDto<ExamResultDto>.Fail("الطالب غير موجود");
                }

                // التحقق من عدم وجود نتيجة مكررة
                if (await _unitOfWork.ExamResults
                    .AnyAsync(er => er.ExamId == createDto.ExamId && er.StudentId == createDto.StudentId))
                {
                    return ResponseDto<ExamResultDto>.Fail("هذه النتيجة موجودة بالفعل لهذا الطالب في هذا الامتحان");
                }

                // التحقق من أن الدرجة لا تتجاوز الدرجة النهائية
                if (createDto.Score > exam.MaxScore)
                {
                    return ResponseDto<ExamResultDto>.Fail($"الدرجة {createDto.Score} تتجاوز الدرجة النهائية {exam.MaxScore}");
                }

                var result = _mapper.Map<ExamResult>(createDto);
                result.Percentage = exam.MaxScore > 0 ? (decimal)createDto.Score / exam.MaxScore * 100 : 0;
                result.Grade = GetGrade(result.Percentage ?? 0);
                result.CreatedAt = DateTime.Now;
                result.IsActive = true;

                var created = await _unitOfWork.ExamResults.AddAsync(result);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<ExamResultDto>(created);
                await PopulateExamResultDto(dto);

                _logger.LogInformation("تم إضافة نتيجة جديدة للطالب {StudentId} في الامتحان {ExamId}", createDto.StudentId, createDto.ExamId);

                return ResponseDto<ExamResultDto>.Ok(dto, "تم إضافة النتيجة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إضافة نتيجة جديدة");
                return ResponseDto<ExamResultDto>.Fail("حدث خطأ أثناء إضافة النتيجة", statusCode: 500);
            }
        }


        /// <summary>
        /// ➕➕ إضافة نتائج متعددة (دفعة واحدة)
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamResultDto>>> CreateRangeAsync(IEnumerable<CreateExamResultDto> createDtos)
        {
            try
            {
                var results = new List<ExamResult>();
                var examIds = createDtos.Select(d => d.ExamId).Distinct().ToList();

                // ✅ التحقق من وجود الامتحانات
                foreach (var examId in examIds)
                {
                    var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
                    if (exam == null)
                    {
                        return ResponseDto<IEnumerable<ExamResultDto>>.Fail($"الامتحان {examId} غير موجود");
                    }
                }

                // ✅ التحقق من وجود الطلاب
                var studentIds = createDtos.Select(d => d.StudentId).Distinct().ToList();
                foreach (var studentId in studentIds)
                {
                    var student = await _unitOfWork.Students.GetByIdAsync(studentId);
                    if (student == null)
                    {
                        return ResponseDto<IEnumerable<ExamResultDto>>.Fail($"الطالب {studentId} غير موجود");
                    }
                }

                foreach (var createDto in createDtos)
                {
                    var exam = await _unitOfWork.Exams.GetByIdAsync(createDto.ExamId);
                    var result = _mapper.Map<ExamResult>(createDto);

                    // ✅ حساب النسبة المئوية
                    if (exam != null && exam.MaxScore > 0)
                    {
                        result.Percentage = (decimal)createDto.Score / exam.MaxScore * 100;
                        result.Grade = GetGrade(result.Percentage ?? 0);
                    }
                    else
                    {
                        result.Percentage = 0;
                        result.Grade = "F";
                    }

                    result.CreatedAt = DateTime.Now;
                    result.IsActive = true;
                    results.Add(result);
                }

                // ✅ استخدام AddRangeAsync
                var created = await _unitOfWork.ExamResults.AddRangeAsync(results);
                await _unitOfWork.CompleteAsync();

                var dtos = _mapper.Map<IEnumerable<ExamResultDto>>(created);

                // ✅ تعبئة البيانات الإضافية
                foreach (var dto in dtos)
                {
                    await PopulateExamResultDto(dto);
                }

                _logger.LogInformation("تم إضافة {Count} نتيجة جديدة", dtos.Count());
                return ResponseDto<IEnumerable<ExamResultDto>>.Ok(dtos, "تم إضافة النتائج بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إضافة نتائج متعددة");
                return ResponseDto<IEnumerable<ExamResultDto>>.Fail("حدث خطأ أثناء إضافة النتائج", statusCode: 500);
            }
        }

        ///// <summary>
        ///// ➕➕ إضافة نتائج متعددة (دفعة واحدة)
        ///// </summary>
        //public async Task<ResponseDto<IEnumerable<ExamResultDto>>> CreateRangeAsync(IEnumerable<CreateExamResultDto> createDtos)
        //{
        //    try
        //    {
        //        var results = new List<ExamResult>();
        //        var examIds = createDtos.Select(d => d.ExamId).Distinct().ToList();

        //        // التحقق من وجود الامتحانات
        //        foreach (var examId in examIds)
        //        {
        //            var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
        //            if (exam == null)
        //            {
        //                return ResponseDto<IEnumerable<ExamResultDto>>.Fail($"الامتحان {examId} غير موجود");
        //            }
        //        }

        //        foreach (var createDto in createDtos)
        //        {
        //            var exam = await _unitOfWork.Exams.GetByIdAsync(createDto.ExamId);
        //            var result = _mapper.Map<ExamResult>(createDto);
        //            result.Percentage = exam != null && exam.MaxScore > 0 ? (decimal)createDto.Score / exam.MaxScore * 100 : 0;
        //            result.Grade = GetGrade(result.Percentage ?? 0);
        //            result.CreatedAt = DateTime.Now;
        //            result.IsActive = true;
        //            results.Add(result);
        //        }

        //        var created = await _unitOfWork.ExamResults.AddRangeAsync(results);
        //        await _unitOfWork.CompleteAsync();

        //        var dtos = _mapper.Map<IEnumerable<ExamResultDto>>(created);
        //        foreach (var dto in dtos)
        //        {
        //            await PopulateExamResultDto(dto);
        //        }

        //        _logger.LogInformation("تم إضافة {Count} نتيجة جديدة", dtos.Count());
        //        return ResponseDto<IEnumerable<ExamResultDto>>.Ok(dtos, "تم إضافة النتائج بنجاح");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ أثناء إضافة نتائج متعددة");
        //        return ResponseDto<IEnumerable<ExamResultDto>>.Fail("حدث خطأ أثناء إضافة النتائج", statusCode: 500);
        //    }
        //}

        /// <summary>
        /// ✏️ تحديث بيانات نتيجة
        /// </summary>
        public async Task<ResponseDto<ExamResultDto>> UpdateAsync(int id, UpdateExamResultDto updateDto)
        {
            try
            {
                var result = await _unitOfWork.ExamResults.GetByIdAsync(id);
                if (result == null)
                {
                    return ResponseDto<ExamResultDto>.NotFound("النتيجة غير موجودة");
                }

                // تحديث الدرجة وإعادة حساب النسبة المئوية
                if (updateDto.Score.HasValue)
                {
                    var exam = await _unitOfWork.Exams.GetByIdAsync(result.ExamId);
                    if (exam != null && updateDto.Score.Value > exam.MaxScore)
                    {
                        return ResponseDto<ExamResultDto>.Fail($"الدرجة {updateDto.Score} تتجاوز الدرجة النهائية {exam.MaxScore}");
                    }

                    result.Score = updateDto.Score.Value;
                    result.Percentage = exam != null && exam.MaxScore > 0 ? (decimal)updateDto.Score.Value / exam.MaxScore * 100 : 0;
                    result.Grade = GetGrade(result.Percentage ?? 0);
                }

                if (!string.IsNullOrEmpty(updateDto.Remarks))
                {
                    result.Remarks = updateDto.Remarks;
                }

                if (updateDto.IsActive)
                {
                    result.IsActive = updateDto.IsActive;
                }

                result.UpdatedAt = DateTime.Now;

                await _unitOfWork.ExamResults.UpdateAsync(result);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<ExamResultDto>(result);
                await PopulateExamResultDto(dto);

                _logger.LogInformation("تم تحديث النتيجة {Id}", id);
                return ResponseDto<ExamResultDto>.Ok(dto, "تم تحديث النتيجة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث النتيجة {Id}", id);
                return ResponseDto<ExamResultDto>.Fail("حدث خطأ أثناء تحديث النتيجة", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف نتيجة
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var result = await _unitOfWork.ExamResults.GetByIdAsync(id);
                if (result == null)
                {
                    return ResponseDto.NotFound("النتيجة غير موجودة");
                }

                await _unitOfWork.ExamResults.DeleteAsync(result);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف النتيجة {Id}", id);
                return ResponseDto.Ok("تم حذف النتيجة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف النتيجة {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف النتيجة", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود نتيجة مكررة
        /// </summary>
        public async Task<ResponseDto<bool>> IsExistsAsync(int examId, int studentId, int? excludeId = null)
        {
            try
            {
                var exists = await _unitOfWork.ExamResults
                    .AnyAsync(er => er.ExamId == examId && 
                                    er.StudentId == studentId && 
                                    (excludeId == null || er.Id != excludeId));
                return ResponseDto<bool>.Ok(exists, exists ? "النتيجة موجودة" : "النتيجة غير موجودة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من وجود النتيجة");
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 📝 تعبئة البيانات الإضافية في ExamResultDto
        /// </summary>
        private async Task PopulateExamResultDto(ExamResultDto dto)
        {
            try
            {
                // ✅ جلب الطالب مع المستخدم
                //var student = await _unitOfWork.Students.GetWithDetailsAsync(dto.StudentId);
                var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(dto.StudentId);

                if (student != null)
                {
                    dto.StudentName = student.User?.FullName ?? student.StudentCode ?? "غير معروف";
                    dto.StudentCode = student.StudentCode ?? string.Empty;

                    // ✅ جلب اسم الفصل من الطالب
                    if (student.ClassRoomId.HasValue)
                    {
                        var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(student.ClassRoomId.Value);
                        dto.ClassRoomName = classRoom?.ClassName ?? "غير محدد";
                    }
                }
                else
                {
                    dto.StudentName = "غير معروف";
                    dto.StudentCode = string.Empty;
                }

                // ✅ جلب الامتحان مع المادة
                var exam = await _unitOfWork.Exams
                    .GetWithDetailsAsync(dto.ExamId);

                if (exam != null)
                {
                    dto.ExamName = exam.ExamName ?? string.Empty;
                    dto.MaxScore = exam.MaxScore;
                    dto.ExamDate = exam.ExamDate;
                    dto.SubjectName = exam.Subject?.SubjectName ?? string.Empty;

                    // ✅ جلب اسم الفصل من الامتحان إذا لم يكن من الطالب
                    if (exam.ClassRoomId.HasValue && string.IsNullOrEmpty(dto.ClassRoomName))
                    {
                        var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(exam.ClassRoomId.Value);
                        dto.ClassRoomName = classRoom?.ClassName ?? "غير محدد";
                    }
                }
                else
                {
                    dto.ExamName = "غير معروف";
                    dto.MaxScore = 100;
                    dto.ExamDate = DateTime.Now;
                    dto.SubjectName = string.Empty;
                }

                // ✅ حساب النسبة المئوية
                if (dto.MaxScore > 0)
                {
                    dto.Percentage = (decimal)dto.Score / dto.MaxScore * 100;
                }
                else
                {
                    dto.Percentage = 0;
                }

                // ✅ تحديد التقدير
                dto.Grade = GetGrade(dto.Percentage ?? 0);

                // ✅ تحديد حالة النجاح
                dto.IsPassed = dto.Score >= 50;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في PopulateExamResultDto للنتيجة {Id}", dto.Id);
            }
        }


        ////////      ============  هذه الدالة ناقصة ====================
        ///// <summary>
        ///// 📝 تعبئة البيانات الإضافية في ExamResultDto
        ///// </summary>
        //private async Task PopulateExamResultDto(ExamResultDto dto)
        //{
        //    var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(dto.StudentId);
        //    var exam = await _unitOfWork.Exams.GetByIdAsync(dto.ExamId);

        //    dto.StudentName = student?.User?.FullName;
        //    dto.StudentCode = student?.StudentCode;
        //    dto.ExamName = exam?.ExamName;
        //    dto.MaxScore = exam?.MaxScore ?? 0;
        //    dto.ExamDate = exam?.ExamDate ?? DateTime.Now;
        //    dto.SubjectName = exam?.Subject?.SubjectName;
        //    dto.ClassRoomName = exam?.ClassRoom?.ClassName;
        //    dto.IsPassed = dto.Score >= 50;
        //}

        /// <summary>
        /// 📝 الحصول على التقدير بناءً على النسبة المئوية
        /// </summary>
        /// <summary>
        /// 📝 الحصول على التقدير بناءً على النسبة المئوية
        /// </summary>
        private string GetGrade(decimal percentage)
        {
            return percentage switch
            {
                >= 90 => "A (ممتاز)",
                >= 80 => "B (جيد جداً)",
                >= 70 => "C (جيد)",
                >= 60 => "D (مقبول)",
                >= 50 => "E (ضعيف)",
                _ => "F (راسب)"
            };
        }

        #endregion
    }
}