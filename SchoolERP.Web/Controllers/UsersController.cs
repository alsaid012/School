using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Users;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Enums;
using SchoolERP.Web.Models;
using System.IO;

namespace SchoolERP.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsersController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public UsersController(
            IUserService userService,
            IUnitOfWork unitOfWork,
            ILogger<UsersController> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion

        #region ════════════════════════════════════ دوال رفع الصورة ════════════════════════════════════

        /// <summary>
        /// 📤 رفع صورة الملف الشخصي
        /// </summary>
        private string UploadProfileImage(IFormFile file, string? oldImagePath = null)
        {
            if (file == null || file.Length == 0)
                return oldImagePath ?? string.Empty;

            // حذف الصورة القديمة
            if (!string.IsNullOrEmpty(oldImagePath))
            {
                var oldFullPath = Path.Combine(_webHostEnvironment.WebRootPath, oldImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldFullPath))
                    System.IO.File.Delete(oldFullPath);
            }

            // إنشاء اسم فريد للصورة
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");

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
        /// 🗑️ حذف الصورة
        /// </summary>
        private void DeleteProfileImage(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;

            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }

        #endregion

        #region ════════════════════════════════════ Index (عرض المستخدمين + البحث) ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع المستخدمين مع البحث والفلترة
        /// </summary>
        public async Task<IActionResult> Index(
            string? searchTerm = null,
            UserType? userType = null,
            UserStatus? status = null,
            int? schoolId = null)
        {
            try
            {
                var response = await _userService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب المستخدمين: {Message}", response.Message);
                    return View("Error");
                }

                // تطبيق الفلاتر
                var viewModel = UserSearchViewModel.ApplyFilters(
                    response.Data,
                    searchTerm,
                    userType,
                    status,
                    schoolId
                );

                // جلب المدارس للقائمة المنسدلة
                var schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                viewModel.Schools = schools.ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Index");
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Details (تفاصيل المستخدم) ════════════════════════════════════

        /// <summary>
        /// 🔍 عرض تفاصيل مستخدم
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _userService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }
                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للمستخدم {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create (إنشاء مستخدم جديد) ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء مستخدم جديد
        /// </summary>
        [Authorize(Roles = "Admin")]
        // ============================================================
        // Create (GET) - باستخدام ViewModel
        // ============================================================
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Create()
        //{
        //    var viewModel = new CreateUserViewModel
        //    {
        //        Schools = (await _unitOfWork.SchoolRepository.GetAllAsync()).ToList()
        //    };
        //    return View(viewModel);
        //}
        public async Task<IActionResult> Create()
        {
            ViewBag.Schools = await _unitOfWork.SchoolRepository.GetAllAsync();
            return View(new CreateUserDto());
        }

        /// <summary>
        /// ➕ إنشاء مستخدم جديد (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateUserDto createDto)
        {


            if (!ModelState.IsValid)
            {

                // ✅ سجل الأخطاء
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("ModelState Invalid: {Errors}", string.Join(", ", errors));

                var schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                ViewBag.Schools = schools.ToList();
                return View(createDto);
                
            }

            try
            {
                if (createDto.ProfileImage != null)
                {
                    createDto.ProfilePicture = UploadProfileImage(createDto.ProfileImage);
                }

                var response = await _userService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء المستخدم");
                    var schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                    ViewBag.Schools = schools.ToList();
                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء المستخدم بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                var schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                ViewBag.Schools = schools.ToList();
                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit (تعديل مستخدم) ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل مستخدم
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _userService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateUserDto
                {
                    FullName = response.Data.FullName,
                    Email = response.Data.Email,
                    PhoneNumber = response.Data.PhoneNumber,
                    Address = response.Data.Address,
                    Status = response.Data.Status,
                    Gender = response.Data.Gender,
                    ProfilePicture = response.Data.ProfilePicture,
                    CurrentProfilePicture = response.Data.ProfilePicture
                };

                // ✅ جلب جهات الاتصال
                var contacts = await _unitOfWork.UserContacts
                    .FindAsync(c => c.UserId == id);
                ViewBag.Contacts = contacts.ToList();
                ViewBag.UserId = id;
                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للمستخدم {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث مستخدم (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id, UpdateUserDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = id;
                var contacts = await _unitOfWork.UserContacts
                    .FindAsync(c => c.UserId == id);
                ViewBag.Contacts = contacts.ToList();
                return View(updateDto);
            }

            try
            {
                // رفع الصورة الجديدة (إن وجدت)
                if (updateDto.ProfileImage != null)
                {
                    updateDto.ProfilePicture = UploadProfileImage(updateDto.ProfileImage, updateDto.CurrentProfilePicture);
                }
                else if (updateDto.RemoveImage)
                {
                    DeleteProfileImage(updateDto.CurrentProfilePicture);
                    updateDto.ProfilePicture = null;
                }
                else
                {
                    updateDto.ProfilePicture = updateDto.CurrentProfilePicture;
                }

                var response = await _userService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث المستخدم");
                    ViewBag.UserId = id;
                    var contacts = await _unitOfWork.UserContacts
                            .FindAsync(c => c.UserId == id);
                    ViewBag.Contacts = contacts.ToList();
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث المستخدم بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للمستخدم {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                ViewBag.UserId = id;
                var contacts = await _unitOfWork.UserContacts
                    .FindAsync(c => c.UserId == id);
                ViewBag.Contacts = contacts.ToList();
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete (حذف مستخدم) ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف مستخدم (Soft Delete)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _userService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف المستخدم";
                }
                else
                {
                    TempData["Success"] = "تم حذف المستخدم بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للمستخدم {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ════════════════════════════════════ Activate / Suspend (تفعيل / تعليق) ════════════════════════════════════

        /// <summary>
        /// 🔄 تفعيل المستخدم
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                var response = await _userService.ActivateAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء تفعيل المستخدم";
                }
                else
                {
                    TempData["Success"] = "تم تفعيل المستخدم بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Activate للمستخدم {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// ⏸️ تعليق المستخدم
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Suspend(int id)
        {
            try
            {
                var response = await _userService.SuspendAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء تعليق المستخدم";
                }
                else
                {
                    TempData["Success"] = "تم تعليق المستخدم بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Suspend للمستخدم {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}