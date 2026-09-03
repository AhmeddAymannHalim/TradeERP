function t(key) {
    return (window.L && window.L[key]) || key;
}

document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('sidebarToggle')?.addEventListener('click', function () {
        document.body.classList.toggle('sidebar-enable');
    });

    setActiveNavLink();
    setDefaultDates();
    initPhoneInputs();
    initLocationCascade();
    initAjaxForms();
    initThemeToggle();
});

function initThemeToggle() {
    var themeToggleBtn = document.getElementById('themeToggleBtn');
    if (!themeToggleBtn) return;

    updateThemeToggleIcon();

    themeToggleBtn.addEventListener('click', function () {
        var current = document.documentElement.getAttribute('data-bs-theme') === 'dark' ? 'dark' : 'light';
        var next = current === 'dark' ? 'light' : 'dark';
        document.documentElement.setAttribute('data-bs-theme', next);
        localStorage.setItem('theme', next);
        updateThemeToggleIcon();
    });
}

function updateThemeToggleIcon() {
    var icon = document.getElementById('themeToggleIcon');
    if (!icon) return;
    var isDark = document.documentElement.getAttribute('data-bs-theme') === 'dark';
    icon.className = isDark ? 'ti ti-sun' : 'ti ti-moon';
}

function initAjaxForms() {
    document.querySelectorAll('form').forEach(form => {
        if (form.method.toLowerCase() === 'get' || form.dataset.ajaxBound) return;
        form.dataset.ajaxBound = 'true';

        form.addEventListener('submit', function (e) {
            if (e.defaultPrevented) return;

            if (window.jQuery && $(form).data('validator') && !$(form).valid()) {
                return;
            }

            e.preventDefault();

            const submitBtn = form.querySelector('button[type="submit"]');
            submitBtn?.setAttribute('disabled', 'disabled');

            fetch(form.action, {
                method: 'POST',
                body: new FormData(form),
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            })
                .then(r => r.json())
                .then(result => {
                    clearValidationErrors(form);

                    if (result.success) {
                        showToast(result.message || t('SavedSuccessfully'), 'success');

                        if (result.devResetLink && window.Swal) {
                            Swal.fire({
                                title: 'Dev mode: no SMTP configured',
                                html: 'Email:SmtpHost is empty, so nothing was actually emailed. Use this link to continue testing:<br><br>' +
                                    '<a href="' + result.devResetLink + '" style="word-break:break-all;">' + result.devResetLink + '</a>',
                                icon: 'info',
                                confirmButtonText: 'OK'
                            });
                            submitBtn?.removeAttribute('disabled');
                            return;
                        }

                        setTimeout(() => {
                            if (result.redirectUrl) location.href = result.redirectUrl;
                        }, 1800);
                    } else {
                        if (result.errors) applyValidationErrors(form, result.errors);
                        showToast(result.message || t('SomethingWentError'), 'error');
                        submitBtn?.removeAttribute('disabled');
                    }
                })
                .catch(() => {
                    showToast(t('SomethingWentError'), 'error');
                    submitBtn?.removeAttribute('disabled');
                });
        });
    });
}

function clearValidationErrors(form) {
    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.textContent = '';
        span.classList.remove('field-validation-error');
        span.classList.add('field-validation-valid');
    });
}

function applyValidationErrors(form, errors) {
    Object.keys(errors).forEach(field => {
        const span = form.querySelector(`[data-valmsg-for="${field}"]`);
        if (span && errors[field] && errors[field].length) {
            span.textContent = errors[field][0];
            span.classList.remove('field-validation-valid');
            span.classList.add('field-validation-error');
        }
    });
}

function initLocationCascade() {
    const isArabicPage = document.documentElement.lang === 'ar';
    const country = document.getElementById('countrySelect');
    const gov = document.getElementById('govSelect');
    const town = document.getElementById('townSelect');
    const village = document.getElementById('villageSelect');

    if (!country || !gov) return;

    function resetSelect(select, placeholder) {
        if (!select) return;
        select.innerHTML = `<option value="">${placeholder}</option>`;
    }

    function fillSelect(select, items) {
        items.forEach(item => {
            const opt = document.createElement('option');
            opt.value = item.id ?? item.Id;
            opt.textContent = isArabicPage ? (item.arName ?? item.ArName) : (item.enName ?? item.EnName);
            select.appendChild(opt);
        });
    }

    country.addEventListener('change', () => {
        resetSelect(gov, gov.dataset.placeholder || '');
        resetSelect(town, town?.dataset.placeholder || '');
        resetSelect(village, village?.dataset.placeholder || '');

        if (!country.value) return;

        getData(`/Lookup/GetGovsByCountryId?countryId=${country.value}`).then(data => {
            if (data) fillSelect(gov, data);
        });
    });

    gov.addEventListener('change', () => {
        resetSelect(town, town?.dataset.placeholder || '');
        resetSelect(village, village?.dataset.placeholder || '');

        if (!gov.value || !town) return;

        getData(`/Lookup/GetTownsByGovId?govId=${gov.value}`).then(data => {
            if (data) fillSelect(town, data);
        });
    });

    town?.addEventListener('change', () => {
        resetSelect(village, village?.dataset.placeholder || '');

        if (!town.value || !village) return;

        getData(`/Lookup/GetVillagesByTownId?townId=${town.value}`).then(data => {
            if (data) fillSelect(village, data);
        });
    });
}

function initPhoneInputs() {
    if (typeof window.intlTelInput !== 'function') return;

    const arabCountries = [
        'eg', 'sa', 'ae', 'kw', 'qa', 'bh', 'om', 'jo', 'lb', 'sy',
        'iq', 'ye', 'ps', 'sd', 'ly', 'tn', 'dz', 'ma', 'mr', 'so',
        'dj', 'km'
    ];

    const arabicCountryNames = {
        eg: 'مصر', sa: 'السعودية', ae: 'الإمارات', kw: 'الكويت', qa: 'قطر',
        bh: 'البحرين', om: 'عمان', jo: 'الأردن', lb: 'لبنان', sy: 'سوريا',
        iq: 'العراق', ye: 'اليمن', ps: 'فلسطين', sd: 'السودان', ly: 'ليبيا',
        tn: 'تونس', dz: 'الجزائر', ma: 'المغرب', mr: 'موريتانيا', so: 'الصومال',
        dj: 'جيبوتي', km: 'جزر القمر'
    };

    const isArabicPage = document.documentElement.lang === 'ar';

    document.querySelectorAll('[data-phone-input]').forEach(input => {
        const iti = window.intlTelInput(input, {
            initialCountry: 'eg',
            onlyCountries: arabCountries,
            preferredCountries: ['eg', 'sa', 'ae'],
            separateDialCode: true,
            utilsScript: 'https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js'
        });

        if (isArabicPage) {
            const wrapper = input.closest('.iti');
            wrapper?.querySelectorAll('.iti__country').forEach(li => {
                const code = li.getAttribute('data-country-code');
                const nameSpan = li.querySelector('.iti__country-name');
                if (code && nameSpan && arabicCountryNames[code]) {
                    nameSpan.textContent = arabicCountryNames[code];
                }
            });
        }

        let ready = false;
        input.addEventListener('countrychange', () => {
            if (ready) {
                input.value = '';
            }
        });
        setTimeout(() => { ready = true; }, 0);

        input.addEventListener('input', () => {
            input.value = input.value.replace(/[^\d\s()-]/g, '');
        });

        const form = input.closest('form');
        form?.addEventListener('submit', e => {
            if (input.value.trim() === '') return;

            if (!iti.isValidNumber()) {
                e.preventDefault();
                showToast(t('InvalidPhoneNumber'), 'error');
                return;
            }

            input.value = iti.getNumber();
        });
    });
}

function setDefaultDates() {
    const today = new Date().toISOString().split('T')[0];

    document.querySelectorAll('input[type="date"]').forEach(input => {
        if (!input.value || input.value === '0001-01-01') {
            input.value = today;
        }
    });
}

function setActiveNavLink() {
    const currentLocation = location.pathname;
    const navLinks = document.querySelectorAll('#sidebar-menu a[href]');

    navLinks.forEach(link => {
        if (link.getAttribute('href') === currentLocation) {
            link.classList.add('active');
            const parentCollapse = link.closest('.collapse');
            if (parentCollapse) {
                parentCollapse.classList.add('show');
                const toggler = document.querySelector(`[data-bs-target="#${parentCollapse.id}"], [href="#${parentCollapse.id}"]`);
                toggler?.setAttribute('aria-expanded', 'true');
            }
        }
    });
}

function showToast(message, icon = 'info') {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: icon,
        title: message,
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true
    });
}

function confirmAction(message, title) {
    return Swal.fire({
        title: title || t('AreYouSure'),
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: t('Yes'),
        cancelButtonText: t('Cancel')
    }).then(result => result.isConfirmed);
}

function postData(url, data) {
    return fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .catch(error => {
            console.error('Error:', error);
        });
}

function getData(url) {
    return fetch(url)
        .then(response => response.json())
        .catch(error => {
            console.error('Error:', error);
        });
}

function formatDate(date, locale) {
    return new Date(date).toLocaleDateString(locale || (window.CurrentCulture === 'ar' ? 'ar-EG' : 'en-US'));
}

function formatCurrency(amount, currency = 'USD') {
    return new Intl.NumberFormat(window.CurrentCulture === 'ar' ? 'ar-EG' : 'en-US', {
        style: 'currency',
        currency: currency
    }).format(amount);
}

function Delete(url) {
    confirmAction(t('ActionCannotBeUndone'), t('AreYouSure')).then(confirmed => {
        if (!confirmed) return;

        fetch(url)
            .then(response => response.json())
            .then(result => {
                if (result.success) {
                    showToast(result.message || t('DeletedSuccessfully'), 'success');
                    setTimeout(() => location.reload(), 800);
                } else {
                    showToast(result.message || result.errorMessage || t('DeleteFailed'), 'error');
                }
            })
            .catch(() => showToast(t('DeleteFailed'), 'error'));
    });
}

function PostBill(url) {
    confirmAction(t('ConfirmPostBill'), t('AreYouSure')).then(confirmed => {
        if (!confirmed) return;

        fetch(url)
            .then(response => response.json())
            .then(result => {
                if (result.success) {
                    showToast(result.message || t('PostedSuccessfully'), 'success');
                    setTimeout(() => location.reload(), 800);
                } else {
                    showToast(result.message || t('ErrorWhileSaving'), 'error');
                }
            })
            .catch(() => showToast(t('ErrorWhileSaving'), 'error'));
    });
}

window.TradeERP = {
    showToast: showToast,
    confirmAction: confirmAction,
    postData: postData,
    getData: getData,
    formatDate: formatDate,
    formatCurrency: formatCurrency,
    setDefaultDates: setDefaultDates,
    initPhoneInputs: initPhoneInputs,
    initLocationCascade: initLocationCascade,
    initAjaxForms: initAjaxForms
};
