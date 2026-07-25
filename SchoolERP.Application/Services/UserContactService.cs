using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.UserContacts;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📞  خدمة جهات الاتصال (UserContactService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة جهات الاتصال
    /// 📦  الاستخدام: في UserContactsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserContactService : IUserContactService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserContactService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public UserContactService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UserContactService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على جهات الاتصال ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع جهات الاتصال
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserContactDto>>> GetAllAsync()
        {
            try
            {
                var contacts = await _unitOfWork.UserContacts.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<UserContactDto>>(contacts);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.UserName = user?.FullName;
                    dto.ContactTypeName = GetContactTypeName(dto.ContactType);
                }

                _logger.LogInformation("تم جلب {Count} جهة اتصال", dtos.Count());
                return ResponseDto<IEnumerable<UserContactDto>>.Ok(dtos, "تم جلب جهات الاتصال بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع جهات الاتصال");
                return ResponseDto<IEnumerable<UserContactDto>>.Fail("حدث خطأ أثناء جلب جهات الاتصال", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جهات اتصال مستخدم معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserContactDto>>> GetByUserIdAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ResponseDto<IEnumerable<UserContactDto>>.NotFound("المستخدم غير موجود");
                }

                var contacts = await _unitOfWork.UserContacts
                    .FindAsync(uc => uc.UserId == userId);
                var dtos = _mapper.Map<IEnumerable<UserContactDto>>(contacts);

                foreach (var dto in dtos)
                {
                    dto.UserName = user.FullName;
                    dto.ContactTypeName = GetContactTypeName(dto.ContactType);
                }

                return ResponseDto<IEnumerable<UserContactDto>>.Ok(dtos, "تم جلب جهات الاتصال بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جهات اتصال المستخدم {UserId}", userId);
                return ResponseDto<IEnumerable<UserContactDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جهات الاتصال حسب النوع
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserContactDto>>> GetByTypeAsync(int contactType)
        {
            try
            {
                var type = (ContactType)contactType;
                var contacts = await _unitOfWork.UserContacts
                    .FindAsync(uc => uc.ContactType == type);
                var dtos = _mapper.Map<IEnumerable<UserContactDto>>(contacts);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.UserName = user?.FullName;
                    dto.ContactTypeName = GetContactTypeName(dto.ContactType);
                }

                return ResponseDto<IEnumerable<UserContactDto>>.Ok(dtos, "تم جلب جهات الاتصال حسب النوع");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جهات الاتصال حسب النوع {ContactType}", contactType);
                return ResponseDto<IEnumerable<UserContactDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جهة الاتصال الأساسية لمستخدم
        /// </summary>
        public async Task<ResponseDto<UserContactDto>> GetPrimaryContactAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ResponseDto<UserContactDto>.NotFound("المستخدم غير موجود");
                }

                var contacts = await _unitOfWork.UserContacts
                    .FindAsync(uc => uc.UserId == userId && uc.IsPrimary);
                var contact = contacts.FirstOrDefault();

                if (contact == null)
                {
                    return ResponseDto<UserContactDto>.NotFound("لا توجد جهة اتصال أساسية للمستخدم");
                }

                var dto = _mapper.Map<UserContactDto>(contact);
                dto.UserName = user.FullName;
                dto.ContactTypeName = GetContactTypeName(contact.ContactType);

                return ResponseDto<UserContactDto>.Ok(dto, "تم جلب جهة الاتصال الأساسية");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جهة الاتصال الأساسية للمستخدم {UserId}", userId);
                return ResponseDto<UserContactDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جهات الاتصال للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserContactLookupDto>>> GetLookupAsync(int? userId = null)
        {
            try
            {
                IEnumerable<UserContact> contacts;

                if (userId.HasValue)
                {
                    contacts = await _unitOfWork.UserContacts
                        .FindAsync(uc => uc.UserId == userId.Value);
                }
                else
                {
                    contacts = await _unitOfWork.UserContacts.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<UserContactLookupDto>>(contacts);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.UserName = user?.FullName;
                    dto.ContactTypeName = GetContactTypeName(dto.ContactType);
                }

                return ResponseDto<IEnumerable<UserContactLookupDto>>.Ok(dtos, "تم جلب جهات الاتصال للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جهات الاتصال للقوائم");
                return ResponseDto<IEnumerable<UserContactLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن جهة اتصال ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على جهة اتصال بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<UserContactDto>> GetByIdAsync(int id)
        {
            try
            {
                var contact = await _unitOfWork.UserContacts.GetByIdAsync(id);
                if (contact == null)
                {
                    return ResponseDto<UserContactDto>.NotFound("جهة الاتصال غير موجودة");
                }

                var dto = _mapper.Map<UserContactDto>(contact);

                var user = await _unitOfWork.Users.GetByIdAsync(contact.UserId);
                dto.UserName = user?.FullName;
                dto.ContactTypeName = GetContactTypeName(contact.ContactType);

                return ResponseDto<UserContactDto>.Ok(dto, "تم جلب جهة الاتصال بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جهة الاتصال {Id}", id);
                return ResponseDto<UserContactDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ الإحصائيات ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصائيات جهات الاتصال
        /// </summary>
        public async Task<ResponseDto<UserContactStatisticsDto>> GetStatisticsAsync()
        {
            try
            {
                var contacts = await _unitOfWork.UserContacts.GetAllAsync();

                var statistics = new UserContactStatisticsDto
                {
                    TotalContacts = contacts.Count(),
                    PhoneContacts = contacts.Count(c => c.ContactType == ContactType.Phone || c.ContactType == ContactType.Mobile),
                    EmailContacts = contacts.Count(c => c.ContactType == ContactType.Email),
                    WhatsAppContacts = contacts.Count(c => c.ContactType == ContactType.WhatsApp),
                    FacebookContacts = contacts.Count(c => c.ContactType == ContactType.Facebook),
                    PrimaryContacts = contacts.Count(c => c.IsPrimary),
                    VerifiedContacts = contacts.Count(c => c.IsVerified),
                    ActiveUsersWithContacts = contacts.Select(c => c.UserId).Distinct().Count(),
                    AverageContactsPerUser = contacts.Select(c => c.UserId).Distinct().Any()
                        ? (decimal)contacts.Count() / contacts.Select(c => c.UserId).Distinct().Count()
                        : 0,
                    ContactsByType = new Dictionary<string, int>(),
                    TopUsersWithContacts = new List<TopUserContactsDto>()
                };

                // توزيع جهات الاتصال حسب النوع
                var typeDistribution = new Dictionary<string, int>
                {
                    { "هاتف", contacts.Count(c => c.ContactType == ContactType.Phone || c.ContactType == ContactType.Mobile) },
                    { "بريد إلكتروني", contacts.Count(c => c.ContactType == ContactType.Email) },
                    { "واتساب", contacts.Count(c => c.ContactType == ContactType.WhatsApp) },
                    { "فيسبوك", contacts.Count(c => c.ContactType == ContactType.Facebook) }
                };
                statistics.ContactsByType = typeDistribution;

                // أكثر المستخدمين جهات اتصال
                var topUsers = contacts
                    .GroupBy(c => c.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        Count = g.Count(),
                        PrimaryCount = g.Count(c => c.IsPrimary),
                        VerifiedCount = g.Count(c => c.IsVerified)
                    })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToList();

                foreach (var user in topUsers)
                {
                    var userEntity = await _unitOfWork.Users.GetByIdAsync(user.UserId);
                    statistics.TopUsersWithContacts.Add(new TopUserContactsDto
                    {
                        UserId = user.UserId,
                        UserName = userEntity?.FullName ?? string.Empty,
                        ContactsCount = user.Count,
                        PrimaryContactsCount = user.PrimaryCount,
                        VerifiedContactsCount = user.VerifiedCount
                    });
                }

                return ResponseDto<UserContactStatisticsDto>.Ok(statistics, "تم جلب إحصائيات جهات الاتصال");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات جهات الاتصال");
                return ResponseDto<UserContactStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء جهة اتصال جديدة
        /// </summary>
        public async Task<ResponseDto<UserContactDto>> CreateAsync(CreateUserContactDto createDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(createDto.UserId);
                if (user == null)
                {
                    return ResponseDto<UserContactDto>.Fail("المستخدم غير موجود");
                }

                // التحقق من وجود قيمة مكررة
                if (await _unitOfWork.UserContacts
                    .AnyAsync(uc => uc.UserId == createDto.UserId && uc.ContactValue == createDto.ContactValue))
                {
                    return ResponseDto<UserContactDto>.Fail("هذه القيمة موجودة بالفعل للمستخدم");
                }

                // إذا كانت جهة اتصال أساسية، إلغاء التحديد من البقية
                if (createDto.IsPrimary)
                {
                    await UnsetPrimaryContactAsync(createDto.UserId);
                }

                var contact = _mapper.Map<UserContact>(createDto);
                contact.CreatedAt = DateTime.Now;
                contact.IsActive = true;

                var created = await _unitOfWork.UserContacts.AddAsync(contact);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<UserContactDto>(created);
                dto.UserName = user.FullName;
                dto.ContactTypeName = GetContactTypeName(createDto.ContactType);

                _logger.LogInformation("تم إنشاء جهة اتصال جديدة للمستخدم {UserId}", createDto.UserId);

                return ResponseDto<UserContactDto>.Ok(dto, "تم إنشاء جهة الاتصال بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء جهة اتصال جديدة");
                return ResponseDto<UserContactDto>.Fail("حدث خطأ أثناء إنشاء جهة الاتصال", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات جهة اتصال
        /// </summary>
        public async Task<ResponseDto<UserContactDto>> UpdateAsync(int id, UpdateUserContactDto updateDto)
        {
            try
            {
                var contact = await _unitOfWork.UserContacts.GetByIdAsync(id);
                if (contact == null)
                {
                    return ResponseDto<UserContactDto>.NotFound("جهة الاتصال غير موجودة");
                }

                // التحقق من وجود قيمة مكررة
                if (!string.IsNullOrEmpty(updateDto.ContactValue) &&
                    await _unitOfWork.UserContacts
                        .AnyAsync(uc => uc.UserId == contact.UserId && uc.ContactValue == updateDto.ContactValue && uc.Id != id))
                {
                    return ResponseDto<UserContactDto>.Fail("هذه القيمة موجودة بالفعل للمستخدم");
                }

                // إذا كانت جهة اتصال أساسية، إلغاء التحديد من البقية
                if (updateDto.IsPrimary && updateDto.IsPrimary)
                {
                    await UnsetPrimaryContactAsync(contact.UserId, id);
                }

                _mapper.Map(updateDto, contact);
                contact.UpdatedAt = DateTime.Now;

                await _unitOfWork.UserContacts.UpdateAsync(contact);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<UserContactDto>(contact);

                var user = await _unitOfWork.Users.GetByIdAsync(contact.UserId);
                dto.UserName = user?.FullName;
                dto.ContactTypeName = GetContactTypeName(contact.ContactType);

                _logger.LogInformation("تم تحديث جهة الاتصال {Id}", id);
                return ResponseDto<UserContactDto>.Ok(dto, "تم تحديث جهة الاتصال بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث جهة الاتصال {Id}", id);
                return ResponseDto<UserContactDto>.Fail("حدث خطأ أثناء تحديث جهة الاتصال", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف جهة اتصال
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var contact = await _unitOfWork.UserContacts.GetByIdAsync(id);
                if (contact == null)
                {
                    return ResponseDto.NotFound("جهة الاتصال غير موجودة");
                }

                await _unitOfWork.UserContacts.DeleteAsync(contact);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف جهة الاتصال {Id}", id);
                return ResponseDto.Ok("تم حذف جهة الاتصال بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف جهة الاتصال {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف جهة الاتصال", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تعيين جهة اتصال أساسية ════════════════════════════════════

        /// <summary>
        /// 🔄 تعيين جهة اتصال كأساسية
        /// </summary>
        public async Task<ResponseDto> SetPrimaryAsync(int id, int userId)
        {
            try
            {
                var contact = await _unitOfWork.UserContacts.GetByIdAsync(id);
                if (contact == null || contact.UserId != userId)
                {
                    return ResponseDto.NotFound("جهة الاتصال غير موجودة");
                }

                // إلغاء التحديد من البقية
                await UnsetPrimaryContactAsync(userId, id);

                // تعيين الحالية كأساسية
                contact.IsPrimary = true;
                contact.UpdatedAt = DateTime.Now;

                await _unitOfWork.UserContacts.UpdateAsync(contact);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم تعيين جهة الاتصال {Id} كأساسية للمستخدم {UserId}", id, userId);
                return ResponseDto.Ok("تم تعيين جهة الاتصال كأساسية بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تعيين جهة الاتصال {Id} كأساسية", id);
                return ResponseDto.Fail("حدث خطأ أثناء تعيين جهة الاتصال", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود جهة اتصال بنفس القيمة
        /// </summary>
        public async Task<ResponseDto<bool>> IsValueExistsAsync(string value, int? excludeId = null)
        {
            try
            {
                var exists = await _unitOfWork.UserContacts
                    .AnyAsync(uc => uc.ContactValue == value && (excludeId == null || uc.Id != excludeId));
                return ResponseDto<bool>.Ok(exists, exists ? "القيمة موجودة" : "القيمة غير موجودة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من القيمة {Value}", value);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔄 إلغاء تحديد جهة الاتصال الأساسية
        /// </summary>
        private async Task UnsetPrimaryContactAsync(int userId, int? excludeId = null)
        {
            var contacts = await _unitOfWork.UserContacts
                .FindAsync(uc => uc.UserId == userId && uc.IsPrimary && (excludeId == null || uc.Id != excludeId));

            foreach (var contact in contacts)
            {
                contact.IsPrimary = false;
                contact.UpdatedAt = DateTime.Now;
                await _unitOfWork.UserContacts.UpdateAsync(contact);
            }
            await _unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// 📝 الحصول على اسم نوع جهة الاتصال بالعربية
        /// </summary>
        private string GetContactTypeName(ContactType type)
        {
            return type switch
            {
                ContactType.Phone => "هاتف",
                ContactType.Mobile => "موبايل",
                ContactType.Email => "بريد إلكتروني",
                ContactType.WhatsApp => "واتساب",
                ContactType.Facebook => "فيسبوك",
                _ => type.ToString()
            };
        }

        #endregion
    }
}