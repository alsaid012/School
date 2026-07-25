// ============================================================
// 🏠  سكريبتات صفحة الرئيسية - Home Page
// ============================================================

$(function () {

    // ---------- تأثير تحريك البطاقات ----------
    $('.stat-card').on('mouseenter', function () {
        $(this).css('transform', 'scale(1.03)');
    }).on('mouseleave', function () {
        $(this).css('transform', 'scale(1)');
    });

    // ---------- تحديث الوقت بشكل دوري ----------
    updateClock();
    setInterval(updateClock, 1000);

    // ---------- تفعيل الرسوم البيانية (Chart.js) إن وجدت ----------
    if (typeof Chart !== 'undefined') {
        // يمكن إضافة رسوم بيانية هنا
        // مثال: رسم بياني لتوزيع المستخدمين
        initCharts();
    }

    // ---------- تفعيل التمرير السلس للروابط ----------
    $('a[href^="#"]').on('click', function (e) {
        e.preventDefault();
        var target = $(this.getAttribute('href'));
        if (target.length) {
            $('html, body').animate({
                scrollTop: target.offset().top - 100
            }, 500);
        }
    });

});

// ============================================================
// 📊  دوال الرسوم البيانية
// ============================================================

function initCharts() {
    // مثال: رسم بياني لتوزيع المستخدمين
    var ctx = document.getElementById('userDistributionChart');
    if (ctx) {
        // يمكن إضافة رسم بياني هنا
    }

    // مثال: رسم بياني للحضور
    var attendanceCtx = document.getElementById('attendanceChart');
    if (attendanceCtx) {
        // يمكن إضافة رسم بياني هنا
    }
}

// ============================================================
// 📋  دوال إضافية للصفحة الرئيسية
// ============================================================

// ---------- تحديث إحصائيات البطاقات ----------
function refreshStats() {
    // يمكن إضافة تحديث للإحصائيات عبر AJAX
    $.ajax({
        url: '/Home/GetStats',
        type: 'GET',
        success: function (data) {
            if (data) {
                // تحديث الأرقام
                $('.stat-number').each(function () {
                    var key = $(this).data('stat');
                    if (key && data[key] !== undefined) {
                        $(this).text(data[key]);
                    }
                });
            }
        },
        error: function () {
            console.log('فشل تحديث الإحصائيات');
        }
    });
}

// ---------- تصدير التقرير ----------
function exportReport(type) {
    var url = '/Home/ExportReport?type=' + type;
    window.open(url, '_blank');
}