/// <reference path="jquery-1.10.2.intellisense.js" />
var WebApp = {
    Init: function () {
        WebApp.Plugins.Init();
        WebApp.Events.Init();
    },
    Data: {
        DataTable: null,
    },
    Plugins: {
        Init: function () {
            WebApp.Plugins.PrepareDataTables();
            WebApp.Plugins.ActivateMiscOnDemandPlugins();
            WebApp.Plugins.PrepareDateRange();
        },
        PrepareDataTables: function () {
            $('.data-table').each(function (index, table) {
                WebApp.Data.DataTable = $(table).DataTable({
                    dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
                    "oLanguage": {
                        "sLengthMenu": '<span class="hide">Show:</span> _MENU_',
                        "sSearch": '<span class="hide">Search:</span> _INPUT_',
                        "oPaginate": {
                            "sNext": '&rarr;',
                            "sLast": 'Last',
                            "sFirst": 'First',
                            "sPrevious": '&larr;'
                        }
                    },
                    responsive: true,
                    "bProcessing": true,
                    "bServerSide": true,
                    "bFilter": true,
                    "scrollX": true,
                    "bPaginate": true,
                    "aaSorting": [
                        [$(table).attr('data-key'), 'asc']
                    ],
                    "aoColumnDefs": [{
                        "bSortable": false,
                        "aTargets": ["no-sort"]
                    }],
                    "aLengthMenu": [
                        [10, 25, 50, 100],
                        [10, 25, 50, 100]
                    ],
                    "sAjaxSource": function () {
                        return $(table).attr('data-source')
                    }(),
                    "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                        switch ($(table).attr('data-type')) {
                            case 'statusPopover':
                                break;
                            default:
                                break;
                        }
                    },
                    fnDrawCallback: function (oSettings) {

                    }
                });
            })
        },
        PrepareDateRange: function () {
            $('.date-range').each(function (index, input) {
                var _start = $("#daterange_start").val();
                var _end = $("#daterange_end").val();

                $(input).daterangepicker({
                    locale: {
                        format: 'DD/MM/YYYY'
                    },
                    ranges: {
                        'Today': [moment(), moment()],
                        'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
                        'Last 7 Days': [moment().subtract('days', 6), moment()],
                        'Last 30 Days': [moment().subtract('days', 29), moment()],
                        'This Month': [moment().startOf('month'), moment().endOf('month')],
                        'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
                    },
                    autoUpdateInput: false,
                    function(_start, _end) {
                        if (_start != null && _end != null) {
                            startDate: _start;
                            endDate: _end
                        }
                    }

                }, function (start, end) {

                    switch ($(input).attr('data-type')) {
                        case 'all-list':
                            var _allStatus = $('._status').val();
                            window.location.href = '/backend/default/dashboard?daterange=' + start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY') + '&status=' + _allStatus + '';
                            break;

                        default:
                            break;
                    }
                    $(input).val(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY'));
                });

                $(input).parent().find('.input-group-btn').click(function () {
                    $(input).trigger('click')
                })
            });
        },
        ActivateMiscOnDemandPlugins: function () {
            $('.timepicker').each(function () {
                $(this).timepicker({
                    timeFormat: 'h:mm p',
                    interval: 10,
                    defaultTime: $(this).attr('data-final'),
                    startTime: '09:00',
                    dynamic: false,
                    dropdown: true,
                    scrollbar: true
                });
            });                      
        }
    },
    Events: {
        Init: function () {
            WebApp.Events.BindStaticEvents();
            WebApp.Events.BindPluginInitializations();
        },
        BindStaticEvents: function () {
            $(document)

                .on('click', '.date-range', function () {
                    var _allStatus = $('._status').val();
                    var _date = $(".date-range").val();
                    var _today = new Date();

                    if (_date == null || _date == '') {
                        var _d = _today.getDate();
                        var _m = _today.getMonth() + 1;
                        var _y = _today.getFullYear();

                        if (_d < 10) {
                            _d = '0' + _d;
                        }
                        if (_m < 10) {
                            _m = '0' + _m;
                        }

                        var _newDate = _d + "/" + _m + "/" + _y;
                        $('.date-range').val(_newDate + " - " + _newDate);

                        var _datataype = $(this).attr('data-type');
                        if (_datataype == "all-list") {
                            window.location.href = '/backend/default/dashboard?daterange=' + _newDate + ' - ' + _newDate + '&status=' + _allStatus + '';
                        }
                    }
                })

                .on('click', '.confirm', function () {
                    return confirm($(this).attr('data-confirm'));
                })
                .on("change", "#chk_all_emails", function () {
                    if ($(this).is(':checked')) {
                        $.get('/backend/registrations/GetEmails?i=1&q=&all=true',
                            function (response) {
                                $('#EmailID').select2('data', response.Results);
                            });
                    } else {
                        $('#EmailID').select2('data', []);
                    }
                })
                .on('change', 'input,select,textarea', function () {
                    if($(this).valid) $(this).valid();
                })
                .on("change", '.document-uploader', function (e) {
                    var _this = $(this);

                    $("#" + _this.attr("file-list-area")).empty();

                    if (_this.valid()) {
                        selDiv = $("#" + _this.attr("file-list-area"));

                        if (_this.attr("multiple") != "multiple") {
                            selDiv.html('');
                            storedFiles = [];
                        }

                        WebApp.Core.handleDocumentSelect(e);
                    }
                });
            $(document).on('click', '.close_imgip', function () {

                var _confirm = confirm("Are you sure want to delete?");

                if (_confirm != false) {
                    var currentItem = $(this).closest('div');
                    var _currentPage = currentItem.find('input.current_Pageip').val();

                    var _index = currentItem.find('input.photo_IDip').val();
                    var modelID = $('.model_idip').val();
                    var modelfile = $('.model_fileip_' + _index).val();

                    if (_currentPage == "ProjectDocument") {
                        $.get('/backend/Gallery/DeleteDocumentFile', { id: modelID, File: modelfile }, function (data) {
                            if (data.status == true) {
                                currentItem.find('input[type="hidden"]').remove();
                                $('._uploaded_photoip').val(data.file);
                                $('._uploaded_photoip').trigger('change');
                                $('.model_fileip_' + _index).remove();
                                $('.closeip_' + _index).remove();
                                $('.urlip_' + _index).remove();
                                $("a").closest('.urlip_' + _index).removeAttr("href");
                            }
                            else {
                                return false;
                            }
                        })
                    }

                    else {
                        alert("Requested Image is not Removed, try Later!");
                        return false;
                    }
                }
                else {
                    return false;
                }
            });

             
        },
        BindPluginInitializations: function () {
            // Standard Plugin Initialization (Replaces livequery)
            $('select').not('.dataTables_length select').each(function () {
                $(this).select2();
            });
            $('select.auto-select').each(function () {
                $(this).val($(this).attr('data-selected')).trigger('change');
            });
            $('.data-table').each(function () {
                $('.dataTables_filter input[type=search]').attr('placeholder', 'Search');
                $('.dataTables_length select').addClass('form-control');
            });
            $('select.form-select').each(function () {
                $(this).select2();
            });
            $('.styled').each(function () {
                if($(this).uniform) $(this).uniform({ radioClass: 'choice' });
            });
            $('.file-styled').each(function () {
                if($(this).uniform) $(this).uniform({
                    fileButtonHtml: '<i class="icon-upload"></i>',
                    wrapperClass: 'bg-primary'
                });
            });
            $('.html-editor').each(function () {
                if($(this).summernote) $(this).summernote({ height: 150 });
            });
            $('.toggle-switch').each(function () {
                if($(this).bootstrapToggle) $(this).bootstrapToggle({ on: 'Yes', off: 'No' });
            });
            $('[data-toggle="popover"]').popover();
            
            $('input.datepicker,input.pickadate').each(function () {
                var _options = { todayHighlight: true, autoclose: true, format: "mm/dd/yyyy" };
                if ($(this).attr('data-min') != null) _options.startDate = $(this).attr('data-min');
                if ($(this).attr('data-max') != null) _options.endDate = $(this).attr('data-max');
                if ($(this).is('.hidden-previous')) _options.startDate = new Date();
                if($(this).datepicker) $(this).datepicker(_options);
            });

            $('input.autocomplete').each(function () {
                $(this).removeClass('autocomplete');
                var source = $(this).attr('data-source');
                var multiple = $(this).attr('data-multiple') == 'true' ? true : false;
                $(this).select2({
                    multiple: multiple,
                    minimumInputLength: 0,
                    allowClear: true,
                    ajax: {
                        quietMillis: 150,
                        url: function () { return source; },
                        dataType: 'json',
                        data: function (term, page) { return { i: page, q: term }; },
                        results: function (data, page) {
                            var _hasMoreResults = (page * 20) < data.Total;
                            return { results: data.Results, more: _hasMoreResults };
                        }
                    }
                });

                var deafult = $(this).attr('data-select2-default');
                if (deafult) {
                    var _item = $.parseJSON(deafult);
                    $(this).select2('data', _item);
                }
            });
            
            $('.form-group').each(function () {
                if ($(this).find('input[data-val-required],textarea[data-val-required],select[data-val-required]').not('[type="checkbox"]').length > 0) {
                    if($(this).find('label span.text-danger').length === 0) {
                        $(this).find('label').first().append('<span class="text-danger">&nbsp;*</span>');
                    }
                }
            });
        }
    },
    Core: {
        RebindFormValidation: function () {
            $("form")
                .removeData("validator")
                .removeData("unobtrusiveValidation");

            $.validator
                .unobtrusive
                .parse("form");
        },
        GetNumber: function (val) {
            val = parseFloat(val);

            if (isNaN(val)) {
                return 0;
            }
            else {
                return val;
            }
        },
        ProcessNames: function () {
            function SetAttributes(index, item) {

                $(item).find('input, textarea, select').attr('id', function (i, attr) {
                    if (attr != undefined) {
                        var name = attr.split('.')[1];
                        var prefix = attr.split('[')[0];

                        return prefix + '[' + index + '].' + attr.split('.')[1]
                    }
                });

                $(item).find('select').attr('name', function (i, attr) {
                    if (attr != undefined) {
                        var name = attr.split('.')[1];
                        var prefix = attr.split('[')[0];

                        return prefix + '[' + index + '].' + attr.split('.')[1]
                    }
                });

                $(item).find('input, textarea').attr('name', function (i, attr) {
                    if (attr != undefined) {
                        var name = attr.split('.')[1];
                        var prefix = attr.split('[')[0];

                        return prefix + '[' + index + '].' + attr.split('.')[1]
                    }
                });

                $(item).find('input, text').attr('name', function (i, attr) {
                    if (attr != undefined) {
                        var name = attr.split('.')[1];
                        var prefix = attr.split('[')[0];

                        return prefix + '[' + index + '].' + attr.split('.')[1]
                    }
                });

                $(item).find('span.field-validation-valid').attr('data-valmsg-for', function (i, attr) {
                    if (attr != undefined) {
                        var name = attr.split('.')[1];
                        var prefix = attr.split('[')[0];

                        return prefix + '[' + index + '].' + attr.split('.')[1]
                    }
                });

                $(item).find('span.text-danger').attr('data-valmsg-for', function (i, attr) {
                    if (attr != undefined) {
                        var name = attr.split('.')[1];
                        var prefix = attr.split('[')[0];

                        return prefix + '[' + index + '].' + attr.split('.')[1]
                    }
                });

                $(item).find('select2-offscreen').attr('name', function (i, attr) {
                    if (attr != undefined) {
                        var name = attr.split('.')[1];
                        var prefix = attr.split('[')[0];

                        return prefix + '[' + index + '].' + attr.split('.')[1]
                    }
                });
            }

            $('.duplicatable-form-fields').each(function (index, item) {
                SetAttributes(index, item);
            });
            $('.add-more-item').each(function (index, item) {
                SetAttributes(index, item);
            });
        },
        HighlightCurrentMenuItem: function (item, index) {
            var _count = 0;
            var _testCount = 0;

            if (index != "" || index != "undefined") {
                if (index == 1) {
                    _count = 1;
                }
                if (index == 2) {
                    _count = 2;
                }
            }

            var _match = $('ul.navigation.navigation-main li a span:contains(' + item + ')');
            var _link = null;

            if (_match.length > 0) {
                $('ul.navigation.navigation-main li a span').each(function () {
                    if (_count >= _testCount) {
                        if ($(this).text().toLowerCase() == item.toLowerCase()) {
                            _testCount++;
                            _link = $(this).parent().parent()
                        }
                    }
                });
            } else {
                _link = $('ul.navigation.navigation-main li a:contains(' + item + ')').parent()
            }

            _link.addClass('active');
            _link.parentsUntil('ul.navigation').attr('style', 'display: block;');
            _link.parentsUntil('ul.navigation').addClass('active');
        },
        ProcessTableValidation: function (item) {
            item.find("input[type='text']").each(function () {
                $(this).rules("add", {
                    required: true
                });
            });
        },
        handleDocumentSelect: function (e) {
            var storedFiles = [];
            var files = e.target.files;

            var filesArr = Array.prototype.slice.call(files);
            var i = 0;
            var filename = "";
            var _fileName;
            filesArr.forEach(function (f) {
                var ext = f.name.split('.').pop();
                // filename = f.name.split('.')[0].length > 15 ? f.name.substring(0, 15).replace(/ /g, "_") + "...." + ext : f.name;
                filename = f.name;

                var icon = "";
                switch (ext) {
                    case 'pdf':
                        icon = "icon-print";
                        break;
                    default:
                        icon = "icon-print";
                }

                storedFiles.push(f);
                var html = "<div class='upload_main7 upload_div7_" + i + "'><i class='" + icon + "'></i><label id='delete-IPbill'>" + filename + "</label> <i class='icon-cancel-square IPbill-delete delete2_" + i + "' data-attribute=" + i + "></i> <br clear=\"left\"/></div>";

                selDiv.append(html);

                console.log(storedFiles);

                i = i + 1;
                var _uploadedPaymentFiles = $('#IPbill_upload').val();

                _fileName = _uploadedPaymentFiles + "|" + filename

                $('#IPbill_upload').val(_fileName);

            });
        },
    },
    Notifications: {
        Show: function (_type, _msg) {
            switch ((_type)) {
                case 'Error':
                    toastr.error(_msg, 'Error')
                    break;
                case 'Success':
                    toastr.success(_msg, 'Success')
                    break;
                case 'Info':
                    toastr.info(_msg, 'Info')
                    break;
            }
        }
    }
}

$(document).ready(function () {
    WebApp.Init();
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-center",
        "onclick": null,
        "showDuration": "6000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "10000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
});