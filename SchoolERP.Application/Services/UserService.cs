using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.UserContacts;
using SchoolERP.Application.DTOs.UserRoles;
using SchoolERP.Application.DTOs.Users;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👤  خدمة المستخدمين (UserService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة المستخدمين
    /// 📦  الاستخدام: في UsersController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserService : IUserService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على المستخدمين ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع المستخدمين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserDto>>> GetAllAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<UserDto>>(users);

                //// ✅ جلب اسم المدرسة لكل مستخدم ///  تم عمل دالة اخري بديلة في UserRepository
                //foreach (var dto in dtos)
                //{
                //    var school = await _unitOfWork.Schools.GetByIdAsync(dto.SchoolId);
                //    dto.SchoolName = school?.SchoolName;
                //}

                // ✅ جلب اسم المدرسة لكل مستخدم
                foreach (var dto in dtos)
                {
                    var school = await _unitOfWork.SchoolRepository.GetByIdAsync(dto.SchoolId);
                    dto.SchoolName = school?.SchoolName;
                }

                _logger.LogInformation("تم جلب {Count} مستخدم", dtos.Count());
                return ResponseDto<IEnumerable<UserDto>>.Ok(dtos, "تم جلب المستخدمين بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع المستخدمين");
                return ResponseDto<IEnumerable<UserDto>>.Fail("حدث خطأ أثناء جلب المستخدمين", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين التابعين لمدرسة معينة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserDto>>> GetBySchoolIdAsync(int schoolId)
        {
            try
            {
                //var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                //if (school == null)
                //{
                //    return ResponseDto<IEnumerable<UserDto>>.NotFound("المدرسة غير موجودة");
                //}

                var users = await _unitOfWork.Users.GetUsersBySchoolAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<UserDto>>(users);

                return ResponseDto<IEnumerable<UserDto>>.Ok(dtos, "تم جلب المستخدمين بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدمين للمدرسة {SchoolId}", schoolId);
                return ResponseDto<IEnumerable<UserDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب النوع
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserDto>>> GetByUserTypeAsync(int userType)
        {
            try
            {
                var users = await _unitOfWork.Users.GetUsersByTypeAsync(userType);
                var dtos = _mapper.Map<IEnumerable<UserDto>>(users);

                return ResponseDto<IEnumerable<UserDto>>.Ok(dtos, "تم جلب المستخدمين حسب النوع");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدمين حسب النوع {UserType}", userType);
                return ResponseDto<IEnumerable<UserDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب الحالة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserDto>>> GetByStatusAsync(int status)
        {
            try
            {
                var users = await _unitOfWork.Users.GetUsersByStatusAsync(status);
                var dtos = _mapper.Map<IEnumerable<UserDto>>(users);

                return ResponseDto<IEnumerable<UserDto>>.Ok(dtos, "تم جلب المستخدمين حسب الحالة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدمين حسب الحالة {Status}", status);
                return ResponseDto<IEnumerable<UserDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين المعلقين (في انتظار التفعيل)
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserDto>>> GetPendingUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetPendingUsersAsync();
                var dtos = _mapper.Map<IEnumerable<UserDto>>(users);

                return ResponseDto<IEnumerable<UserDto>>.Ok(dtos, "تم جلب المستخدمين المعلقين");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدمين المعلقين");
                return ResponseDto<IEnumerable<UserDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين النشطين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserDto>>> GetActiveUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetActiveUsersAsync();
                var dtos = _mapper.Map<IEnumerable<UserDto>>(users);

                return ResponseDto<IEnumerable<UserDto>>.Ok(dtos, "تم جلب المستخدمين النشطين");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدمين النشطين");
                return ResponseDto<IEnumerable<UserDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب الدور
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserDto>>> GetByRoleAsync(int roleType)
        {
            try
            {
                var users = await _unitOfWork.Users.GetUsersByRoleAsync(roleType);
                var dtos = _mapper.Map<IEnumerable<UserDto>>(users);

                return ResponseDto<IEnumerable<UserDto>>.Ok(dtos, "تم جلب المستخدمين حسب الدور");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدمين حسب الدور {RoleType}", roleType);
                return ResponseDto<IEnumerable<UserDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserLookupDto>>> GetLookupAsync(int? userType = null)
        {
            try
            {
                IEnumerable<User> users;

                if (userType.HasValue)
                {
                    users = await _unitOfWork.Users.GetUsersByTypeAsync(userType.Value);
                }
                else
                {
                    users = await _unitOfWork.Users.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<UserLookupDto>>(users);
                return ResponseDto<IEnumerable<UserLookupDto>>.Ok(dtos, "تم جلب المستخدمين للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدمين للقوائم");
                return ResponseDto<IEnumerable<UserLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن مستخدم ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على مستخدم بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<UserDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserWithDetailsAsync(id);
                if (user == null)
                {
                    return ResponseDto<UserDetailsDto>.NotFound("المستخدم غير موجود");
                }

                var dto = _mapper.Map<UserDetailsDto>(user);

                // ✅ جلب اسم المدرسة
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(user.SchoolId);
                dto.SchoolName = school?.SchoolName;

                // ✅ جلب رقم الهاتف من جهات الاتصال
                var phoneContact = user.Contacts?.FirstOrDefault(c => c.ContactType == ContactType.Phone || c.ContactType == ContactType.Mobile);
                if (phoneContact != null)
                {
                    dto.PhoneNumber = phoneContact.ContactValue;
                }

                //// جلب الأدوار
                //var roles = await _unitOfWork.UserRoles
                //    .FindAsync(r => r.UserId == id);
                //dto.UserRoles = _mapper.Map<List<UserRoleDto>>(roles);

                // جلب جهات الاتصال
                var contacts = await _unitOfWork.UserContacts
                    .FindAsync(c => c.UserId == id);
                dto.Contacts = _mapper.Map<List<UserContactDto>>(contacts);

                return ResponseDto<UserDetailsDto>.Ok(dto, "تم جلب المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدم {Id}", id);
                return ResponseDto<UserDetailsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على مستخدم بواسطة اسم المستخدم
        /// </summary>
        public async Task<ResponseDto<UserDto>> GetByUsernameAsync(string username)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return ResponseDto<UserDto>.NotFound("المستخدم غير موجود");
                }

                var dto = _mapper.Map<UserDto>(user);
                return ResponseDto<UserDto>.Ok(dto, "تم جلب المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدم بالاسم {Username}", username);
                return ResponseDto<UserDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على مستخدم بواسطة الرقم القومي
        /// </summary>
        public async Task<ResponseDto<UserDto>> GetByNationalIdAsync(string nationalId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserByNationalIdAsync(nationalId);
                if (user == null)
                {
                    return ResponseDto<UserDto>.NotFound("المستخدم غير موجود");
                }

                var dto = _mapper.Map<UserDto>(user);
                return ResponseDto<UserDto>.Ok(dto, "تم جلب المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدم بالرقم القومي {NationalId}", nationalId);
                return ResponseDto<UserDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 الحصول على إحصائيات المستخدم
        /// </summary>
        public async Task<ResponseDto<UserStatisticsDto>> GetStatisticsAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserWithDetailsAsync(userId);
                if (user == null)
                {
                    return ResponseDto<UserStatisticsDto>.NotFound("المستخدم غير موجود");
                }

                //var contacts = await _unitOfWork.UserContacts
                //    .FindAsync(c => c.UserId == userId);
                //var roles = await _unitOfWork.UserRoles
                //    .FindAsync(r => r.UserId == userId);

                // ✅ إحصائيات المستخدم الفردي
                var userStats = new UserStatisticsDto
                {
                    LoginCount = 0, // سيتم حسابه لاحقاً
                    ContactsCount = user.Contacts?.Count ?? 0,
                    RolesCount = user.UserRoles?.Count ?? 0,
                    MembershipDays = (int)(DateTime.Now - user.CreatedAt).TotalDays,
                    LastActivityDate = user.LastLogin
                };
                // جلب جميع المستخدمين للإحصائيات العامة
                var allUsers = await _unitOfWork.Users.GetAllAsync();
                var allUsersList = allUsers.ToList();

                var statistics = new UserStatisticsDto
                {
                    TotalUsers = allUsersList.Count,
                    ActiveUsers = allUsersList.Count(u => u.Status == UserStatus.Active),
                    PendingUsers = allUsersList.Count(u => u.Status == UserStatus.Pending),
                    SuspendedUsers = allUsersList.Count(u => u.Status == UserStatus.Suspended),
                    InactiveUsers = allUsersList.Count(u => u.Status == UserStatus.Inactive),
                    StudentsCount = allUsersList.Count(u => u.UserType == UserType.Student),
                    TeachersCount = allUsersList.Count(u => u.UserType == UserType.Teacher),
                    EmployeesCount = allUsersList.Count(u => u.UserType == UserType.Employee),
                    AdminsCount = allUsersList.Count(u => u.UserType == UserType.Admin),
                    UsersByType = allUsersList.GroupBy(u => u.UserType).ToDictionary(g => g.Key, g => g.Count()),
                    UsersByStatus = allUsersList.GroupBy(u => u.Status).ToDictionary(g => g.Key, g => g.Count())
                };

                return ResponseDto<UserStatisticsDto>.Ok(statistics, "تم جلب إحصائيات المستخدم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات المستخدم {UserId}", userId);
                return ResponseDto<UserStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء مستخدم جديد
        /// </summary>
        public async Task<ResponseDto<UserDto>> CreateAsync(CreateUserDto createDto)
        {
            try
            {
                // التحقق من وجود اسم مستخدم مكرر
                if (await _unitOfWork.Users.UsernameExistsAsync(createDto.Username))
                {
                    return ResponseDto<UserDto>.Fail($"اسم المستخدم {createDto.Username} موجود بالفعل");
                }

                // التحقق من وجود رقم قومي مكرر
                if (await _unitOfWork.Users.NationalIdExistsAsync(createDto.NationalId))
                {
                    return ResponseDto<UserDto>.Fail($"الرقم القومي {createDto.NationalId} موجود بالفعل");
                }

                // التحقق من وجود المدرسة
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(createDto.SchoolId);
                if (school == null)
                {
                    return ResponseDto<UserDto>.Fail("المدرسة غير موجودة");
                }

                var user = _mapper.Map<User>(createDto);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createDto.Password);
                user.Status = UserStatus.Pending;
                user.CreatedAt = DateTime.Now;
                user.IsActive = true;

                var created = await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                // إضافة الدور الأساسي
                var userRole = new UserRole
                {
                    UserId = created.Id,
                    RoleType = createDto.UserType,
                    IsPrimary = true,
                    StartDate = DateTime.Now
                };
                await _unitOfWork.UserRoles.AddAsync(userRole);
                await _unitOfWork.CompleteAsync();

                // إضافة جهات الاتصال
                foreach (var contactDto in createDto.Contacts)
                {
                    var contact = new UserContact
                    {
                        UserId = created.Id,
                        ContactType = contactDto.ContactType,
                        ContactValue = contactDto.ContactValue,
                        IsPrimary = contactDto.IsPrimary,
                        Notes = contactDto.Notes,
                        CreatedAt = DateTime.Now
                    };
                    await _unitOfWork.UserContacts.AddAsync(contact);
                }
                await _unitOfWork.CompleteAsync();

                // إنشاء الكيان المناسب حسب نوع المستخدم
                await CreateUserTypeEntityAsync(created.Id, createDto);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<UserDto>(created);
                _logger.LogInformation("تم إنشاء مستخدم جديد: {Username}", created.Username);

                return ResponseDto<UserDto>.Ok(dto, "تم إنشاء المستخدم بنجاح، في انتظار التفعيل");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء مستخدم جديد");
                return ResponseDto<UserDto>.Fail("حدث خطأ أثناء إنشاء المستخدم", statusCode: 500);
            }
        }

        /// <summary>
        /// ➕ إنشاء الكيان المناسب حسب نوع المستخدم
        /// </summary>
        private async Task CreateUserTypeEntityAsync(int userId, CreateUserDto createDto)
        {
            switch (createDto.UserType)
            {
                case UserType.Student:
                    var student = new Student
                    {
                        UserId = userId,
                        StudentCode = GenerateCode("STU"),
                        AcademicYearId = 1, // سيتم تحديدها لاحقاً
                        ClassRoomId = createDto.ClassRoomId,
                        ParentName = createDto.ParentName,
                        ParentPhone = createDto.ParentPhone,
                        EnrollmentDate = DateTime.Now,
                        IsGraduated = false,
                        CreatedAt = DateTime.Now
                    };
                    await _unitOfWork.Students.AddAsync(student);
                    break;

                case UserType.Teacher:
                    var teacher = new Teacher
                    {
                        UserId = userId,
                        TeacherCode = GenerateCode("TCH"),
                        Qualification = createDto.Qualification,
                        Specialization = createDto.Specialization,
                        HireDate = DateTime.Now,
                        IsHomeroomTeacher = false,
                        CreatedAt = DateTime.Now
                    };
                    await _unitOfWork.TeacherRepository.AddAsync(teacher);
                    break;

                case UserType.Employee:
                    var employee = new Employee
                    {
                        UserId = userId,
                        EmployeeCode = GenerateCode("EMP"),
                        JobTitle = createDto.JobTitle ?? "موظف",
                        HireDate = DateTime.Now,
                        CreatedAt = DateTime.Now
                    };
                    await _unitOfWork.EmployeeRepository.AddAsync(employee);
                    break;
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات مستخدم
        /// </summary>
        public async Task<ResponseDto<UserDto>> UpdateAsync(int id, UpdateUserDto updateDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserWithDetailsAsync(id);
                if (user == null)
                {
                    return ResponseDto<UserDto>.NotFound("المستخدم غير موجود");
                }

                _mapper.Map(updateDto, user);
                user.UpdatedAt = DateTime.Now;

                // تحديث جهات الاتصال
                if (updateDto.Contacts != null)
                {
                    // حذف جهات الاتصال المحددة للحذف
                    var toDelete = updateDto.Contacts.Where(c => c.IsDeleted && c.Id.HasValue).ToList();
                    foreach (var contactDto in toDelete)
                    {
                        var contact = await _unitOfWork.UserContacts.GetByIdAsync(contactDto.Id.GetValueOrDefault());
                        if (contact != null)
                        {
                            await _unitOfWork.UserContacts.DeleteAsync(contact);
                        }
                    }

                    // تحديث أو إضافة جهات الاتصال
                    foreach (var contactDto in updateDto.Contacts.Where(c => !c.IsDeleted))
                    {
                        if (contactDto.Id.HasValue)
                        {
                            // تحديث موجود
                            var contact = await _unitOfWork.UserContacts.GetByIdAsync(contactDto.Id.Value);
                            if (contact != null)
                            {
                                contact.ContactType = contactDto.ContactType ?? contact.ContactType;
                                contact.ContactValue = contactDto.ContactValue ?? contact.ContactValue;
                                contact.IsPrimary = contactDto.IsPrimary ;
                                contact.IsVerified = contactDto.IsVerified ;
                                contact.Notes = contactDto.Notes ?? contact.Notes;
                                contact.UpdatedAt = DateTime.Now;
                                await _unitOfWork.UserContacts.UpdateAsync(contact);
                            }
                        }
                        else
                        {
                            // إضافة جديدة
                            var contact = new UserContact
                            {
                                UserId = id,
                                ContactType = contactDto.ContactType ?? ContactType.Phone,
                                ContactValue = contactDto.ContactValue ?? string.Empty,
                                IsPrimary = contactDto.IsPrimary ,
                                IsVerified = contactDto.IsVerified ,
                                Notes = contactDto.Notes,
                                CreatedAt = DateTime.Now
                            };
                            await _unitOfWork.UserContacts.AddAsync(contact);
                        }
                    }
                }

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<UserDto>(user);
                _logger.LogInformation("تم تحديث المستخدم: {Username}", user.Username);

                return ResponseDto<UserDto>.Ok(dto, "تم تحديث المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث المستخدم {Id}", id);
                return ResponseDto<UserDto>.Fail("حدث خطأ أثناء تحديث المستخدم", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف مستخدم (Soft Delete)
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                {
                    return ResponseDto.NotFound("المستخدم غير موجود");
                }

                user.IsDeleted = true;
                user.IsActive = false;
                user.Status = UserStatus.Inactive;
                user.DeletedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف المستخدم: {Username}", user.Username);
                return ResponseDto.Ok("تم حذف المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف المستخدم {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف المستخدم", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تغيير حالة المستخدم ════════════════════════════════════

        /// <summary>
        /// 🔄 تفعيل المستخدم
        /// </summary>
        public async Task<ResponseDto> ActivateAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                {
                    return ResponseDto.NotFound("المستخدم غير موجود");
                }

                user.Status = UserStatus.Active;
                user.IsActive = true;
                user.UpdatedAt = DateTime.Now;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم تفعيل المستخدم: {Username}", user.Username);
                return ResponseDto.Ok("تم تفعيل المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تفعيل المستخدم {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء تفعيل المستخدم", statusCode: 500);
            }
        }

        /// <summary>
        /// ⏸️ تعليق المستخدم
        /// </summary>
        public async Task<ResponseDto> SuspendAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                {
                    return ResponseDto.NotFound("المستخدم غير موجود");
                }

                user.Status = UserStatus.Suspended;
                user.IsActive = false;
                user.UpdatedAt = DateTime.Now;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم تعليق المستخدم: {Username}", user.Username);
                return ResponseDto.Ok("تم تعليق المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تعليق المستخدم {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء تعليق المستخدم", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔄 استعادة مستخدم محذوف (Soft Delete Restore)
        /// </summary>
        public async Task<ResponseDto> RestoreAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                {
                    return ResponseDto.NotFound("المستخدم غير موجود");
                }

                if (!user.IsDeleted)
                {
                    return ResponseDto.Fail("المستخدم غير محذوف");
                }

                user.IsDeleted = false;
                user.IsActive = true;
                user.Status = UserStatus.Pending; // يحتاج تفعيل مرة أخرى
                user.DeletedAt = null;
                user.UpdatedAt = DateTime.Now;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم استعادة المستخدم: {Username}", user.Username);
                return ResponseDto.Ok("تم استعادة المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء استعادة المستخدم {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء استعادة المستخدم", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف نهائي للمستخدم (Hard Delete)
        /// </summary>
        public async Task<ResponseDto> HardDeleteAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                {
                    return ResponseDto.NotFound("المستخدم غير موجود");
                }

                // حذف البيانات المرتبطة
                var contacts = await _unitOfWork.UserContacts
                    .FindAsync(c => c.UserId == id);
                foreach (var contact in contacts)
                {
                    await _unitOfWork.UserContacts.DeleteAsync(contact);
                }

                var roles = await _unitOfWork.UserRoles
                    .FindAsync(r => r.UserId == id);
                foreach (var role in roles)
                {
                    await _unitOfWork.UserRoles.DeleteAsync(role);
                }

                // حذف الكيان المرتبط حسب النوع
                await DeleteUserTypeEntityAsync(user);

                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف المستخدم نهائياً: {Username}", user.Username);
                return ResponseDto.Ok("تم حذف المستخدم نهائياً");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء الحذف النهائي للمستخدم {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء الحذف النهائي", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف الكيان المرتبط حسب نوع المستخدم
        /// </summary>
        private async Task DeleteUserTypeEntityAsync(User user)
        {
            switch (user.UserType)
            {
                case UserType.Student:
                    var students = await _unitOfWork.Students
                        .FindAsync(s => s.UserId == user.Id);
                    foreach (var student in students)
                    {
                        await _unitOfWork.Students.DeleteAsync(student);
                    }
                    break;

                case UserType.Teacher:
                    var teachers = await _unitOfWork.TeacherRepository
                        .FindAsync(t => t.UserId == user.Id);
                    foreach (var teacher in teachers)
                    {
                        // حذف الروابط مع المواد
                        var teacherSubjects = await _unitOfWork.TeacherSubjects
                            .FindAsync(ts => ts.TeacherId == teacher.Id);
                        foreach (var ts in teacherSubjects)
                        {
                            await _unitOfWork.TeacherSubjects.DeleteAsync(ts);
                        }
                        await _unitOfWork.TeacherRepository.DeleteAsync(teacher);
                    }
                    break;

                case UserType.Employee:
                    var employees = await _unitOfWork.EmployeeRepository
                        .FindAsync(e => e.UserId == user.Id);
                    foreach (var employee in employees)
                    {
                        await _unitOfWork.EmployeeRepository.DeleteAsync(employee);
                    }
                    break;
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود اسم مستخدم
        /// </summary>
        public async Task<ResponseDto<bool>> IsUsernameExistsAsync(string username)
        {
            try
            {
                var exists = await _unitOfWork.Users.UsernameExistsAsync(username);
                return ResponseDto<bool>.Ok(exists, exists ? "اسم المستخدم موجود" : "اسم المستخدم غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من اسم المستخدم {Username}", username);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        /// <summary>
        /// ✅ التحقق من وجود رقم قومي
        /// </summary>
        public async Task<ResponseDto<bool>> IsNationalIdExistsAsync(string nationalId)
        {
            try
            {
                var exists = await _unitOfWork.Users.NationalIdExistsAsync(nationalId);
                return ResponseDto<bool>.Ok(exists, exists ? "الرقم القومي موجود" : "الرقم القومي غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من الرقم القومي {NationalId}", nationalId);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion

        // ============================================================
        // دوال رفع الصورة
        // ============================================================

        /// <summary>
        /// رفع صورة الملف الشخصي
        /// </summary>
        public string UploadProfileImage(IFormFile file, string? oldImagePath = null, string? webRootPath = null)
        {
            if (file == null || file.Length == 0)
                return oldImagePath ?? string.Empty;

            // حذف الصورة القديمة
            if (!string.IsNullOrEmpty(oldImagePath) && webRootPath != null)
            {
                var oldFullPath = Path.Combine(webRootPath, oldImagePath.TrimStart('/'));
                if (File.Exists(oldFullPath))
                    File.Delete(oldFullPath);
            }

            // إنشاء اسم فريد للصورة
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadsFolder = Path.Combine(webRootPath ?? "wwwroot", "uploads", "profiles");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"/uploads/profiles/{fileName}";
        }

        /// <summary>
        /// حذف الصورة
        /// </summary>
        public void DeleteProfileImage(string imagePath, string? webRootPath = null)
        {
            if (string.IsNullOrEmpty(imagePath) || webRootPath == null)
                return;

            var fullPath = Path.Combine(webRootPath, imagePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🏷️ إنشاء كود فريد
        /// </summary>
        private string GenerateCode(string prefix)
        {
            var random = new Random();
            var number = random.Next(1000, 9999);
            return $"{prefix}-{DateTime.Now:yyyyMMdd}-{number}";
        }

        #endregion
    }
}