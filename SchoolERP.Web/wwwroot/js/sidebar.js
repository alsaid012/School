// ============================================================
// 📱  سكريبتات الشريط الجانبي (Sidebar) - SchoolERP
// ============================================================

$(function () {

    // =========================================================
    // 1️⃣  فتح المجموعة التي تحتوي على الرابط النشط
    // =========================================================
    function openActiveGroup() {
        $('.sidebar-section .collapse').each(function () {
            var collapse = $(this);
            var hasActive = collapse.find('.nav-link.active').length > 0;
            if (hasActive) {
                collapse.addClass('show');
                var toggle = $('[data-bs-target="#' + collapse.attr('id') + '"]');
                toggle.attr('aria-expanded', 'true');
            }
        });
    }
    openActiveGroup();

    // =========================================================
    // 2️⃣  حفظ حالة الفتح/الغلق في LocalStorage
    // =========================================================
    $('.sidebar-section .section-toggle').on('click', function () {
        var target = $(this).data('bs-target');
        var isExpanded = $(this).attr('aria-expanded') === 'true';
        // حفظ الحالة في LocalStorage
        localStorage.setItem('sidebar_' + target, isExpanded ? 'collapsed' : 'expanded');
    });

    // =========================================================
    // 3️⃣  استعادة الحالة من LocalStorage
    // =========================================================
    function restoreSidebarState() {
        $('.sidebar-section .section-toggle').each(function () {
            var target = $(this).data('bs-target');
            var state = localStorage.getItem('sidebar_' + target);
            if (state === 'collapsed') {
                $(this).attr('aria-expanded', 'false');
                $(target).removeClass('show');
            } else if (state === 'expanded') {
                $(this).attr('aria-expanded', 'true');
                $(target).addClass('show');
            }
        });
    }
    restoreSidebarState();

    // =========================================================
    // 4️⃣  تصغير/توسيع الشريط الجانبي (اختياري)
    // =========================================================
    $('#sidebarToggle').on('click', function () {
        $('.sidebar-wrapper').toggleClass('sidebar-collapsed');
        var isCollapsed = $('.sidebar-wrapper').hasClass('sidebar-collapsed');
        localStorage.setItem('sidebar_collapsed', isCollapsed ? 'true' : 'false');
        // تغيير نص الزر
        $(this).find('i').toggleClass('fa-chevron-right fa-chevron-left');
    });

    // استعادة حالة التصغير
    var isSidebarCollapsed = localStorage.getItem('sidebar_collapsed') === 'true';
    if (isSidebarCollapsed) {
        $('.sidebar-wrapper').addClass('sidebar-collapsed');
        $('#sidebarToggle').find('i').removeClass('fa-chevron-left').addClass('fa-chevron-right');
    }

    // =========================================================
    // 5️⃣  إغلاق جميع المجموعات (اختياري)
    // =========================================================
    $('#sidebarCollapseAll').on('click', function () {
        $('.sidebar-section .collapse').removeClass('show');
        $('.sidebar-section .section-toggle').attr('aria-expanded', 'false');
        // تحديث LocalStorage
        $('.sidebar-section .section-toggle').each(function () {
            var target = $(this).data('bs-target');
            localStorage.setItem('sidebar_' + target, 'collapsed');
        });
    });

    // =========================================================
    // 6️⃣  فتح جميع المجموعات (اختياري)
    // =========================================================
    $('#sidebarExpandAll').on('click', function () {
        $('.sidebar-section .collapse').addClass('show');
        $('.sidebar-section .section-toggle').attr('aria-expanded', 'true');
        // تحديث LocalStorage
        $('.sidebar-section .section-toggle').each(function () {
            var target = $(this).data('bs-target');
            localStorage.setItem('sidebar_' + target, 'expanded');
        });
    });

    // =========================================================
    // 7️⃣  تحديث حالة الـ Sidebar عند تغيير الصفحة (للـ SPA)
    // =========================================================
    $(document).on('click', '.sidebar-section .nav-link', function () {
        // يمكن إضافة تأثيرات أو تحليلات هنا
        console.log('تم النقر على: ' + $(this).text().trim());
    });

    // =========================================================
    // 8️⃣  عرض/إخفاء الـ Sidebar في الجوال
    // =========================================================
    $('#sidebarMobileToggle').on('click', function () {
        $('.sidebar-wrapper').toggleClass('d-none d-md-block');
    });

    // =========================================================
    // 9️⃣  إضافة تأثيرات حركية للـ Sidebar
    // =========================================================
    $('.sidebar-section .collapse').on('show.bs.collapse', function () {
        // عند فتح المجموعة
        var toggle = $('[data-bs-target="#' + $(this).attr('id') + '"]');
        toggle.addClass('open');
    });

    $('.sidebar-section .collapse').on('hide.bs.collapse', function () {
        // عند غلق المجموعة
        var toggle = $('[data-bs-target="#' + $(this).attr('id') + '"]');
        toggle.removeClass('open');
    });

    // =========================================================
    // 🔟  إضافة أداة بحث داخل الـ Sidebar (اختياري)
    // =========================================================
    $('#sidebarSearch').on('keyup', function () {
        var searchTerm = $(this).val().toLowerCase();
        $('.sidebar-section .nav-link').each(function () {
            var text = $(this).text().toLowerCase();
            if (text.includes(searchTerm)) {
                $(this).show();
                // إظهار المجموعة الأم إذا كان هناك نتيجة
                $(this).closest('.collapse').addClass('show');
                $(this).closest('.collapse').siblings('.section-toggle').attr('aria-expanded', 'true');
            } else {
                $(this).hide();
            }
        });
        // إذا كان البحث فارغاً، إظهار الكل واستعادة الحالة
        if (searchTerm === '') {
            $('.sidebar-section .nav-link').show();
            restoreSidebarState();
        }
    });

});

// ============================================================
// 🔧  دوال إضافية للـ Sidebar
// ============================================================

// ---------- تصغير الشريط الجانبي ----------
function toggleSidebar() {
    $('.sidebar-wrapper').toggleClass('sidebar-collapsed');
    var isCollapsed = $('.sidebar-wrapper').hasClass('sidebar-collapsed');
    localStorage.setItem('sidebar_collapsed', isCollapsed ? 'true' : 'false');
}

// ---------- فتح مجموعة محددة ----------
function openSidebarGroup(groupId) {
    var target = '#collapse' + groupId;
    $(target).addClass('show');
    $('[data-bs-target="' + target + '"]').attr('aria-expanded', 'true');
    localStorage.setItem('sidebar_' + target, 'expanded');
}

// ---------- غلق مجموعة محددة ----------
function closeSidebarGroup(groupId) {
    var target = '#collapse' + groupId;
    $(target).removeClass('show');
    $('[data-bs-target="' + target + '"]').attr('aria-expanded', 'false');
    localStorage.setItem('sidebar_' + target, 'collapsed');
}

// ---------- الحصول على حالة المجموعة ----------
function getSidebarGroupState(groupId) {
    var target = '#collapse' + groupId;
    return localStorage.getItem('sidebar_' + target) || 'expanded';
}

// ---------- إعادة تعيين جميع الإعدادات ----------
function resetSidebarSettings() {
    localStorage.clear();
    location.reload();
}