
var actionConfirm = $(document).find('#hdn-action-confirm').val();
var actionDeleteMessage = $(document).find('#hdn-action-delete-message').val();
var actionPublishMessage = $(document).find('#hdn-action-publish-message').val();
var actionCancelChangesMessage = $(document).find('#hdn-action-cancel-changes-message').val();
var actionConfirmDeleteMessage = $(document).find('#hdn-action-confirm-delete-message').val();
var actionPublishAllMessage = $(document).find('#hdn-action-publish-all-message').val();
var actionCreateCopyMessage = $(document).find('#hdn-action-create-copy-message').val();
var actionYes = $(document).find('#hdn-action-yes').val();
var actionNo = $(document).find('#hdn-action-no').val();
var actionOk = $(document).find('#hdn-action-ok').val();
var actionCancel = $(document).find('#hdn-action-cancel').val();
var actionConfirm = $(document).find('#hdn-action-confirm').val();

import { LoadMultiLanguageFields } from './multilingual-field.js';
LoadMultiLanguageFields();


export function DoAjaxPostActionWithConfirmMessage(url, message) {
    ShowConfirmMessage(message, function (confirmed) {
        if (confirmed) {

            $.post(url, function (data) {
                GetDataListAjax('.items-list', true);
                ShowMessage(data.success, data.status, data.message, true);
            });
        }
    });
}
export function ClearFormErrors(form) {
    $(form).find('[data-valmsg-for]').each(function () {
        $(this).html('').removeClass('field-validation-error').addClass('field-validation-valid');
    });
}
export function SetFormErrors(form, errors) {
    $.each(errors, function (fieldName, errors) {
        var validationMessageElement = $(form).find('[data-valmsg-for="' + fieldName + '"]');
        validationMessageElement.removeClass('field-validation-valid').addClass('field-validation-error');
        validationMessageElement.text(errors.join(' '));
    });
}

//modal start
$(document).on('click', '.modal-close', function (e) {
    e.preventDefault();
    var element = $(this);
    var mainModal = element.closest('#main-modal');
    mainModal.find('#item-editor').html('');
    mainModal.modal('hide');
});
function UpdateApprovalsLinks(element) {
    var controller = $(element).closest('table').data('controller');
    var link = $(element).attr('href').split('/');
    return !IsNullOrEmpty(controller) ? `/${controller}/${link[2]}/${link[3]}` : $(element).attr('href');
}
function ModalAfterShowOperations() {
    SetSelectDefaultOption();
    LoadMultiLanguageFields();
/*    LoadUploadAndChooseFile();*/
    //FillAjaxSelectItems();
    //GetImagePathById();
    //GetFilePathById();
    //JavaScriptLibrariesInitializer();
    //TagifyInitializer();

}
function GetPartialViewToMainModal(element) {
    $('#item-editor').empty();

    var mainModal = $('#main-modal');

    $.ajax({
        type: "GET",
        url: UpdateApprovalsLinks(element),// $(element).attr('href'),
        dataType: "html",
        success: function (data) {
            $('#item-editor').html(data);
            ModalAfterShowOperations();
            mainModal.modal('show');
        }
    });
    return false;
}
// Modal End

function SetSelectDefaultOption() {
    $(document).find('.select-ajax:not(.no-default-option)').each(function () {
        $(this).append($('<option value="">' + $('#hdn-please-select').val() + '</option>'));
    });
}



// Ajax Start
$(document).on('click', '.history-ajax-link', function (e) {
    e.preventDefault();
    GetPartialViewToMainModal(this);
    $(document).find('#main-modal .modal-title').html($('#hdn-changes-logs').val());
    $(document).find('#main-modal .modal-submit').hide();
});
$(document).on('click', '.view-history-ajax-link', function (e) {
    e.preventDefault();
    GetPartialViewToMainModal(this);
    $(document).find('#main-modal .modal-title').html($('#hdn-changes-view').val());
    $(document).find('#main-modal .modal-submit').hide();
});
// Ajax End
function ShowMessage(success, status, message, reload) {

    let isSuccess = String(success).toLowerCase() === "true";
    Swal.fire({
        title: status,
        text: message,
        icon: isSuccess ? 'success' : 'error',
        confirmButtonText: '<i class="fa fa fa-check"></i> ' + actionOk,
        buttonsStyling: false,
        customClass: {
            confirmButton: "btn btn-dark mx-1",
        },
        showClass: {
            popup: 'animate__animated animate__fadeIn'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOut'
        }
    }).then(function () {
        if (String(reload).toLowerCase() === "true")
            location.reload();
        else
            return;
    });

}

function ShowConfirmMessage(message, callback) {

    Swal.fire({
        title: actionConfirm,
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: '<i class="fa fa fa-check"></i> ' + actionConfirm,
        cancelButtonText: '<i class="fa fa fa-close"></i> ' + actionCancel,
        buttonsStyling: false,
        customClass: {
            confirmButton: "btn btn-dark mx-1",
            cancelButton: "btn btn-light mx-1"
        },
        showClass: {
            popup: 'animate__animated animate__fadeIn'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOut'
        }
    }).then((confirmed) => {
        callback(confirmed && confirmed.value == true);
    });
}

function GetDataListAjax(selector, reload) {

    let container = $(document).find('.items-list');
    if (container != undefined && container != null && reload == false) {

        $(selector).each(function () {
            let element = $(this);
            $.post($(element).data('url'), function (data) {
                $(element).html(data);
            });
        });
    }
    else {
        if (reload) {
            location.reload();
        }
    }
}

function IsNullOrEmpty(value) {
    return value === null || value === undefined || value === '';
}


$(document).on('click', '.delete-ajax-link', function (e) {
    e.preventDefault();
    DoAjaxPostActionWithConfirmMessage(UpdateApprovalsLinks(this), actionDeleteMessage);
});

$(document).on('click', '.confirm-publish-ajax-link', function (e) {
    e.preventDefault();
    DoAjaxPostActionWithConfirmMessage(UpdateApprovalsLinks(this), actionPublishMessage);
});
$(document).on('click', '.confirm-delete-ajax-link', function (e) {
    e.preventDefault();
    DoAjaxPostActionWithConfirmMessage(UpdateApprovalsLinks(this), actionConfirmDeleteMessage);
});
$(document).on('click', '.cancel-ajax-link', function (e) {
    e.preventDefault();
    DoAjaxPostActionWithConfirmMessage(UpdateApprovalsLinks(this), actionCancelChangesMessage);
});

$(document).on('click', '.publish-all-ajax-link', function (e) {
    e.preventDefault();
    DoAjaxPostActionWithConfirmMessage($(this).attr('href'), actionPublishAllMessage);
});


$(document).on('click', '.copy-ajax-link', function (e) {
    e.preventDefault();
    DoAjaxPostActionWithConfirmMessage(UpdateApprovalsLinks(this), actionCreateCopyMessage);
});
$(document).on('click', '.transfer-ajax-link', function (e) {
    e.preventDefault();
    DoAjaxPostActionWithConfirmMessage(UpdateApprovalsLinks(this), actionTransferMessage);
});

$(document).on('click', '.ajax-link', function (e) {
    e.preventDefault();
    let elementHref = $(this).attr('href');
    ShowConfirmMessage($('#hdn-change-item-message').val(), function (confirmed) {
        if (confirmed) {

            $.post(elementHref, function (data) {
                GetDataListAjax('.items-list', false);
                ShowMessage(data.success, data.status, data.message, false);
            });
        }
    });
});