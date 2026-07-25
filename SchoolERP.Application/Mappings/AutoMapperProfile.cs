using AutoMapper;
using SchoolERP.Application.DTOs.AcademicYears;
using SchoolERP.Application.DTOs.ClassRooms;
using SchoolERP.Application.DTOs.ClassSchedules;
using SchoolERP.Application.DTOs.Departments;
using SchoolERP.Application.DTOs.EmployeeAttendances;
using SchoolERP.Application.DTOs.Employees;
using SchoolERP.Application.DTOs.ExamResults;
using SchoolERP.Application.DTOs.Exams;
using SchoolERP.Application.DTOs.Governorates;
using SchoolERP.Application.DTOs.GradeLevels;
using SchoolERP.Application.DTOs.Schools;
using SchoolERP.Application.DTOs.StudentAttendances;
using SchoolERP.Application.DTOs.Students;
using SchoolERP.Application.DTOs.Subjects;
using SchoolERP.Application.DTOs.Teachers;
using SchoolERP.Application.DTOs.TeacherSubjects;
using SchoolERP.Application.DTOs.UserContacts;
using SchoolERP.Application.DTOs.UserRoles;
using SchoolERP.Application.DTOs.Users;
using SchoolERP.Application.DTOs.Users.Contacts;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Mappings
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔄  ملف تعيين AutoMapper (AutoMapper Profile)
    /// 📌  الوظيفة: تحويل الكيانات (Entities) إلى DTOs والعكس
    /// 📦  الاستخدام: في جميع الـ Services
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region ════════════════════════════════════ Governorate ════════════════════════════════════

            CreateMap<Governorate, GovernorateDto>();
            CreateMap<Governorate, GovernorateDetailsDto>();
            CreateMap<Governorate, GovernorateLookupDto>();
            CreateMap<CreateGovernorateDto, Governorate>();
            CreateMap<UpdateGovernorateDto, Governorate>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ Department ════════════════════════════════════

            CreateMap<Department, DepartmentDto>();
            CreateMap<Department, DepartmentDetailsDto>()
                .ForMember(dest => dest.GovernorateName, opt => opt.MapFrom(src => src.Governorate.Name))
                .ForMember(dest => dest.SchoolsCount, opt => opt.MapFrom(src => src.Schools.Count))
                .ForMember(dest => dest.Schools, opt => opt.MapFrom(src => src.Schools));

            CreateMap<Department, DepartmentLookupDto>();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ School ════════════════════════════════════

            CreateMap<School, SchoolDto>();
            CreateMap<School, SchoolDetailsDto>();
            CreateMap<School, SchoolLookupDto>();
            CreateMap<School, SchoolStatisticsDto>();
            CreateMap<CreateSchoolDto, School>();
            CreateMap<UpdateSchoolDto, School>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ User ════════════════════════════════════

            // ✅ Entity → DTO
            CreateMap<User, UserDto>()
                            .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School.SchoolName));

            CreateMap<User, UserDetailsDto>()
                            .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School.SchoolName));
                          
               
            CreateMap<User, UserLookupDto>();
            CreateMap<User, UserStatisticsDto>();

            // ✅ Create DTO → Entity
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.Contacts, opt => opt.Ignore())
                .ForMember(dest => dest.Students, opt => opt.Ignore())
                .ForMember(dest => dest.Teachers, opt => opt.Ignore())
                .ForMember(dest => dest.Employees, opt => opt.Ignore());
          
            // ✅ Update DTO → Entity (تحديث جزئي)
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Contacts, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.Students, opt => opt.Ignore())
                .ForMember(dest => dest.Teachers, opt => opt.Ignore())
                .ForMember(dest => dest.Employees, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            #endregion

            #region ════════════════════════════════════ Student ════════════════════════════════════

            CreateMap<Student, StudentDto>();
            CreateMap<Student, StudentDetailsDto>();
            CreateMap<Student, StudentLookupDto>();
            CreateMap<Student, StudentStatisticsDto>();
            CreateMap<CreateStudentDto, Student>();
            CreateMap<UpdateStudentDto, Student>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ Teacher ════════════════════════════════════

            CreateMap<Teacher, TeacherDto>();
            CreateMap<Teacher, TeacherDetailsDto>();
            CreateMap<Teacher, TeacherLookupDto>();
            CreateMap<Teacher, TeacherStatisticsDto>();
            CreateMap<CreateTeacherDto, Teacher>()
                .ForMember(dest => dest.TeacherSubjects, opt => opt.Ignore());
            CreateMap<UpdateTeacherDto, Teacher>()
                .ForMember(dest => dest.TeacherSubjects, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ Employee ════════════════════════════════════

            CreateMap<Employee, EmployeeDto>();
            CreateMap<Employee, EmployeeDetailsDto>();
            CreateMap<Employee, EmployeeLookupDto>();
            CreateMap<Employee, EmployeeStatisticsDto>();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ GradeLevel ════════════════════════════════════

            CreateMap<GradeLevel, GradeLevelDto>()
                .ForMember(dest => dest.GradeStageName, opt => opt.MapFrom(src => src.GradeStage.ToString()));
           
            CreateMap<GradeLevel, GradeLevelLookupDto>()
           .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.SchoolId))
           .ForMember(dest => dest.GradeStage, opt => opt.MapFrom(src => src.GradeStage))
           .ForMember(dest => dest.GradeStageName, opt => opt.MapFrom(src => src.GradeStage.ToString()))
           .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School.SchoolName));


            CreateMap<GradeLevel, GradeLevelStatisticsDto>();
            CreateMap<CreateGradeLevelDto, GradeLevel>();
            CreateMap<UpdateGradeLevelDto, GradeLevel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ ClassRoom ════════════════════════════════════

            CreateMap<ClassRoom, ClassRoomDto>();
            CreateMap<ClassRoom, ClassRoomDetailsDto>();
            CreateMap<ClassRoom, ClassRoomLookupDto>();
            CreateMap<ClassRoom, ClassRoomStatisticsDto>();
            CreateMap<CreateClassRoomDto, ClassRoom>();
            CreateMap<UpdateClassRoomDto, ClassRoom>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ Subject ════════════════════════════════════

            CreateMap<Subject, SubjectDto>();
            CreateMap<Subject, SubjectDetailsDto>();
            CreateMap<Subject, SubjectLookupDto>();
            CreateMap<Subject, SubjectStatisticsDto>();
            CreateMap<CreateSubjectDto, Subject>();
            CreateMap<UpdateSubjectDto, Subject>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ TeacherSubject ════════════════════════════════════

            CreateMap<TeacherSubject, TeacherSubjectDto>();
            CreateMap<TeacherSubject, TeacherSubjectLookupDto>();
            CreateMap<CreateTeacherSubjectDto, TeacherSubject>();
            CreateMap<UpdateTeacherSubjectDto, TeacherSubject>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ ClassSchedule ════════════════════════════════════

            CreateMap<ClassSchedule, ClassScheduleDto>()
                .ForMember(dest => dest.DayName, opt => opt.MapFrom(src => src.DayOfWeek.ToString()));
            CreateMap<ClassSchedule, ClassScheduleLookupDto>()
                .ForMember(dest => dest.DayName, opt => opt.MapFrom(src => src.DayOfWeek.ToString()));
            CreateMap<ClassSchedule, ClassScheduleStatisticsDto>();
            CreateMap<CreateClassScheduleDto, ClassSchedule>();
            CreateMap<UpdateClassScheduleDto, ClassSchedule>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ Exam ════════════════════════════════════

            CreateMap<Exam, ExamDto>()
                .ForMember(dest => dest.ExamTypeName, opt => opt.MapFrom(src => src.ExamType.ToString()));
            CreateMap<Exam, ExamDetailsDto>()
                .ForMember(dest => dest.ExamTypeName, opt => opt.MapFrom(src => src.ExamType.ToString()));
            CreateMap<Exam, ExamLookupDto>()
                .ForMember(dest => dest.ExamTypeName, opt => opt.MapFrom(src => src.ExamType.ToString()));
            CreateMap<Exam, ExamStatisticsDto>();
            CreateMap<CreateExamDto, Exam>();
            CreateMap<UpdateExamDto, Exam>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ ExamResult ════════════════════════════════════

            CreateMap<ExamResult, ExamResultDto>()
                    .ForMember(dest => dest.StudentName, opt => opt.Ignore())
                    .ForMember(dest => dest.StudentCode, opt => opt.Ignore())
                    .ForMember(dest => dest.ExamName, opt => opt.Ignore())
                    .ForMember(dest => dest.SubjectName, opt => opt.Ignore())
                    .ForMember(dest => dest.ClassRoomName, opt => opt.Ignore())
                    .ForMember(dest => dest.MaxScore, opt => opt.Ignore())
                    .ForMember(dest => dest.Percentage, opt => opt.Ignore())
                    .ForMember(dest => dest.Grade, opt => opt.Ignore())
                    .ForMember(dest => dest.IsPassed, opt => opt.Ignore())
                    .ForMember(dest => dest.IsPassed, opt => opt.MapFrom(src => src.Score >= 50)); // افتراض 50% نجاح
            CreateMap<ExamResult, ExamResultLookupDto>()
                .ForMember(dest => dest.IsPassed, opt => opt.MapFrom(src => src.Score >= 50));
            CreateMap<ExamResult, ExamResultStatisticsDto>();
            CreateMap<CreateExamResultDto, ExamResult>();
            CreateMap<UpdateExamResultDto, ExamResult>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ AcademicYear ════════════════════════════════════

            CreateMap<AcademicYear, AcademicYearDto>();
            CreateMap<AcademicYear, AcademicYearDetailsDto>();
            CreateMap<AcademicYear, AcademicYearLookupDto>();
            CreateMap<AcademicYear, AcademicYearStatisticsDto>();
            CreateMap<CreateAcademicYearDto, AcademicYear>();
            CreateMap<UpdateAcademicYearDto, AcademicYear>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ StudentAttendance ════════════════════════════════════

            CreateMap<StudentAttendance, StudentAttendanceDto>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<StudentAttendance, StudentAttendanceLookupDto>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<StudentAttendance, StudentAttendanceStatisticsDto>();
            CreateMap<CreateStudentAttendanceDto, StudentAttendance>();
            CreateMap<UpdateStudentAttendanceDto, StudentAttendance>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ EmployeeAttendance ════════════════════════════════════

            CreateMap<EmployeeAttendance, EmployeeAttendanceDto>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<EmployeeAttendance, EmployeeAttendanceLookupDto>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<EmployeeAttendance, EmployeeAttendanceStatisticsDto>();
            CreateMap<CreateEmployeeAttendanceDto, EmployeeAttendance>();
            CreateMap<UpdateEmployeeAttendanceDto, EmployeeAttendance>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ UserContact ════════════════════════════════════

            CreateMap<UserContact, UserContactDto>()
                .ForMember(dest => dest.ContactTypeName, opt => opt.MapFrom(src => src.ContactType.ToString()));
            CreateMap<UserContact, UserContactLookupDto>()
                .ForMember(dest => dest.ContactTypeName, opt => opt.MapFrom(src => src.ContactType.ToString()));
            CreateMap<UserContact, UserContactStatisticsDto>();


            CreateMap<CreateUserContactDto, UserContact>();
            CreateMap<UpdateUserContactDto, UserContact>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion

            #region ════════════════════════════════════ UserRole ════════════════════════════════════

            CreateMap<UserRole, UserRoleDto>()
                .ForMember(dest => dest.RoleTypeName, opt => opt.MapFrom(src => src.RoleType.ToString()));

            CreateMap<UserRole, UserRoleLookupDto>()
                .ForMember(dest => dest.RoleTypeName, opt => opt.MapFrom(src => src.RoleType.ToString()));
            CreateMap<UserRole, UserRoleStatisticsDto>();
            CreateMap<CreateUserRoleDto, UserRole>();
            CreateMap<UpdateUserRoleDto, UserRole>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            #endregion
        }
    }
}