
        // ============================================================
    // 🚀  سكريبتات الصفحة
    // ============================================================

    $(document).ready(function () {

        // ==========================================================
        // 🔘  إظهار/إخفاء كلمة المرور
        // ==========================================================
        $('#togglePassword').on('click', function () {
            var passwordInput = $('#passwordInput');
            var icon = $(this).find('i');

            if (passwordInput.attr('type') === 'password') {
                passwordInput.attr('type', 'text');
                icon.removeClass('fa-eye').addClass('fa-eye-slash');
            } else {
                passwordInput.attr('type', 'password');
                icon.removeClass('fa-eye-slash').addClass('fa-eye');
            }
        });

    // ==========================================================
    // 🔄  تأثير تحميل الزر عند الضغط
    // ==========================================================
    $('#loginForm').on('submit', function () {
                var btn = $('#loginBtn');
    btn.addClass('loading');
    btn.prop('disabled', true);
            });

    // ==========================================================
    // 🌙  تبديل الوضع الليلي
    // ==========================================================
    var isDarkMode = localStorage.getItem('darkMode') === 'true';

    if (isDarkMode) {
        $('body').addClass('dark-mode');
    $('#themeToggle i').removeClass('fa-moon').addClass('fa-sun');
    $('#themeText').text('نهاري');
            }

    $('#themeToggle').on('click', function () {
        $('body').toggleClass('dark-mode');
    var isDark = $('body').hasClass('dark-mode');

    localStorage.setItem('darkMode', isDark);

    if (isDark) {
        $(this).find('i').removeClass('fa-moon').addClass('fa-sun');
    $('#themeText').text('نهاري');
                } else {
        $(this).find('i').removeClass('fa-sun').addClass('fa-moon');
    $('#themeText').text('ليلي');
                }
            });

    // ==========================================================
    // 🎯  إخفاء رسائل الخطأ عند الكتابة
    // ==========================================================
    $('.form-control').on('input', function () {
        $(this).removeClass('is-invalid');
    $(this).siblings('.field-validation-error').text('');
            });

    // ==========================================================
    // 🎨  تأثيرات عند التركيز على الحقول
    // ==========================================================
    $('.form-control').on('focus', function () {
        $(this).closest('.mb-3').find('.form-label').addClass('text-primary');
            }).on('blur', function () {
        $(this).closest('.mb-3').find('.form-label').removeClass('text-primary');
            });

        });
