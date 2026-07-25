// ============================================================
// 🛠️  السكريبتات العامة - SchoolERP
// ============================================================

$(function () {

    // ---------- تفعيل التلميحات (Tooltips) ----------
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // ---------- تفعيل التنبيهات المنبثقة (Popovers) ----------
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // ---------- إخفاء رسائل النجاح بعد 5 ثواني ----------
    setTimeout(function () {
        $('.alert-success').fadeOut('slow', function () {
            $(this).remove();
        });
    }, 5000);

    // ---------- إخفاء رسائل الخطأ بعد 7 ثواني ----------
    setTimeout(function () {
        $('.alert-danger').fadeOut('slow', function () {
            $(this).remove();
        });
    }, 7000);

    // ---------- تفعيل تحديث الوقت (سيتم استخدامه في الصفحات) ----------
    window.updateClock = function () {
        var now = new Date();
        var time = now.toLocaleTimeString('ar-EG', {
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
        });
        var date = now.toLocaleDateString('ar-EG', {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
        $('.current-time').text(time);
        $('.current-date').text(date);
    };

    // ---------- تفعيل الـ DataTables العامة ----------
    $('.data-table').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.4/i18n/ar.json'
        },
        pageLength: 25,
        responsive: true
    });

    // ---------- تفعيل الـ Select2 إن وجد ----------
    if ($.fn.select2) {
        $('.select2').select2({
            theme: 'bootstrap-5',
            dir: 'rtl'
        });
    }

    // ---------- التحقق من صحة النماذج بشكل عام ----------
    $('form[data-validate="true"]').on('submit', function (e) {
        var isValid = true;
        $(this).find('[required]').each(function () {
            if (!$(this).val()) {
                $(this).addClass('is-invalid');
                isValid = false;
            } else {
                $(this).removeClass('is-invalid');
            }
        });
        if (!isValid) {
            e.preventDefault();
            alert('يرجى ملء جميع الحقول المطلوبة');
        }
    });

    // ---------- تفعيل الـ Switch لتغيير النص ----------
    $('.form-switch input[type="checkbox"]').on('change', function () {
        var label = $(this).closest('.form-check').find('.form-check-label');
        var trueText = $(this).data('true-text') || 'مفعل';
        var falseText = $(this).data('false-text') || 'غير مفعل';
        if ($(this).is(':checked')) {
            label.html('<span class="text-success"><i class="fas fa-check-circle"></i> ' + trueText + '</span>');
        } else {
            label.html('<span class="text-secondary"><i class="fas fa-circle"></i> ' + falseText + '</span>');
        }
    });

});

// ============================================================
// 🔧  دوال عامة يمكن استخدامها في أي صفحة
// ============================================================

// ---------- دالة تأكيد الحذف ----------
function confirmDelete(message) {
    return confirm(message || 'هل أنت متأكد من حذف هذا العنصر؟');
}

// ---------- دالة تأكيد التفعيل/التعطيل ----------
function confirmToggle(message) {
    return confirm(message || 'هل أنت متأكد من تغيير حالة هذا العنصر؟');
}

// ---------- دالة عرض رسالة نجاح ----------
function showSuccess(message) {
    if (message) {
        alert('✅ ' + message);
    }
}

// ---------- دالة عرض رسالة خطأ ----------
function showError(message) {
    if (message) {
        alert('❌ ' + message);
    }
}

// ---------- دالة تحويل التاريخ إلى صيغة عربية ----------
function formatDateArabic(date) {
    if (!date) return '';
    var d = new Date(date);
    return d.toLocaleDateString('ar-EG', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}

// ---------- دالة تحويل الوقت إلى صيغة 12 ساعة ----------
function formatTime12h(time) {
    if (!time) return '';
    var parts = time.split(':');
    var hours = parseInt(parts[0]);
    var minutes = parts[1];
    var ampm = hours >= 12 ? 'م' : 'ص';
    hours = hours % 12;
    hours = hours ? hours : 12;
    return hours + ':' + minutes + ' ' + ampm;
}