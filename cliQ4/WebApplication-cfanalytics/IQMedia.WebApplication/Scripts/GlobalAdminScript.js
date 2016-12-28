var _ClientList;
var _FliqClientList;
var selects;
var _ActiveUserSortColumn = '';
var _ActiveUserSortDirectionIsAsc = false;
var _ActiveUserSearchTerm = '';

$(function () {

    $("#ulSideMenu li").removeAttr("class");
    $("#liMenuGlobalAdmin").attr("class", "active");
    if (_ClientList == undefined) {
        FillClientList();
    }
    if (_FliqClientList == undefined) {
        FillFliqClientList();
    }

    $("#txtActiveUserSearchTerm").keypress(function (e) {
        if (e.keyCode == 13) {
            SearchActiveUsers();
        }
    });
    $("#txtActiveUserSearchTerm").blur(function () {
        SearchActiveUsers();
    });

    $("#divResult_ScrollContent").css("height", documentHeight - 260);

    $("#divResult_ScrollContent").enscroll({
        verticalTrackClass: 'track4',
        verticalHandleClass: 'handle4',
        margin: '0 0 0 10px'
    });
});


function RemoveUser(EmailAddress,SessionID) {

    var jsonPostData = {
        p_EmailAddress: EmailAddress,
        p_SessionID : SessionID

    }
    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlGlobalAdminRemoveUserFromCache,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnRemoveUserComplete,
        error: OnRemoveUserFail
    });
}

function OnRemoveUserComplete(result) {
    if (result.isSuccess) {
        $('#divResult').html(result.globalAdminHTML);
        ShowNotification('User reset successfully');
    }
    else {
        ShowNotification('Some error occured, try again later');
    }
}

function OnRemoveUserFail(result) {
    ShowNotification('Some error occured, try again later');
}

function RemoveAllUsers() {
    getConfirm("Reset All Users", _msgConfirmRemoveAllUsers, "Confirm Reset", "Cancel", function (res) {
        if (res) {
            $.ajax({

                type: 'POST',
                dataType: 'json',
                url: _urlGlobalAdminRemoveAllUsers,
                contentType: 'application/json; charset=utf-8',
                success: OnRemoveAllUsersComplete,
                error: OnRemoveAllUsersFail
            });
        }
    });
}

function OnRemoveAllUsersComplete(result) {
    if (result.isSuccess) {        

        setTimeout(function () { window.location = "/sign-in" }, 3000);

        ShowNotification('All Users have been reset successfully');
    }
    else {
        ShowNotification('Some error occured, try again later');
    }
}

function OnRemoveAllUsersFail(result) {
    ShowNotification('Some error occured, try again later');
}

function GetActiveUsers() {
    var jsonPostData = {
        p_SearchTerm: _ActiveUserSearchTerm,
        p_SortColumn: _ActiveUserSortColumn,
        p_IsAsc : _ActiveUserSortDirectionIsAsc
    }
    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlGlobalAdminGetActiveUsersFromCache,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#liActiveUsers").ActiveNav();
                $(".span9-custom > div").hide();
                $("#divResult").show();

                $("#divResult_Content").html(result.HTML);

                $("#divResult_ScrollContent").css("height", documentHeight - 260);


                $("#divResult_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin: '0 0 0 10px'
                });

                $("#txtActiveUserSearchTerm").val(_ActiveUserSearchTerm);

            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divResult_Content', null, '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetActiveUsers()"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divResult_Content', null, '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetActiveUsers()"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}


function SearchActiveUsers() {
    if (_ActiveUserSearchTerm != $("#txtActiveUserSearchTerm").val().trim()) {
        _ActiveUserSearchTerm = $("#txtActiveUserSearchTerm").val().trim();
        GetActiveUsers();
    }
}

function ClearActiveUser() {
    if (_ActiveUserSearchTerm != '') {
        _ActiveUserSearchTerm = ''
        GetActiveUsers();
    }
}

function SortActiveUser(sortColumn, sortDirection) {
    if (sortColumn != _ActiveUserSortColumn || sortDirection != _ActiveUserSortDirectionIsAsc) {
        _ActiveUserSortColumn = sortColumn;
        _ActiveUserSortDirectionIsAsc = sortDirection;
        SetDirectionHTMLActiveUser();
        GetActiveUsers();
    }
}

function SetDirectionHTMLActiveUser() {
    if (_ActiveUserSortColumn == 'LastAccessTime' && _ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgLastAccessTimeAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'LastAccessTime' && !_ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgLastAccessTimeDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'LoginID' && _ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgEmailAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'LoginID' && !_ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgEmailDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'FirstName' && _ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgFirstNameAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'FirstName' && !_ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgFirstNameDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'LastName' && _ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgLastNameAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'LastName' && !_ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgLastNameDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'Server' && !_ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgServerDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'Server' && _ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgServerAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else {
        $('#aSortDirectionActiveUser').html(_msgLastAccessTimeDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
}

function FillClientList() {
    $.ajax({
        url: _urlGlobalAdminGetClientsList,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: {},
        success: function (result) {
            if (result != null && result.isSuccess) {
                _ClientList = result.clientList;

                $("#txtClients").autocomplete({
                    source: _ClientList
                });

                $(".ui-autocomplete").css("max-height", documentHeight - 300);
                $(".ui-autocomplete").css("overflow", "auto");

                var optClients = '<option value="">All</option>';
                $.each(_ClientList, function (index, item) {
                    optClients = optClients + '<option value="' + item + '">' + item + '</option>';
                });

                $("#ddlClients").html(optClients);
                $("#ddlUGCMap_Client").html(optClients); 

                selects = $(".chosen-select").chosen({
                    width: "93%"
                });

                $('#ddlClients').trigger("chosen:updated");
                $('#ddlUGCMap_Client').trigger("chosen:updated");
            }
            else {
                ShowNotification(_msgErrorOccured);
                FillClientList();
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured, a);
            FillClientList();
        }
    });
}

function FillFliqClientList() {
    $.ajax({
        url: _urlFliqCustomerGetFliqClientsList,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: {},
        success: function (result) {
            if (result != null && result.isSuccess) {
                _FliqClientList = result.clientList;
                var optClients = '<option value="">All</option>';
                $.each(_FliqClientList, function (index, item) {
                    optClients = optClients + '<option value="' + item + '">' + item + '</option>';
                });

                $("#ddlFliq_Clients").html(optClients);
                $("#ddlFliq_AppClients").html(optClients);
                $("#ddlFliq_AppClients2").html(optClients);
                

                selects = $(".chosen-select").chosen({
                    width: "93%"
                });

                $('#ddlFliq_Clients').trigger("chosen:updated");
                $('#ddlFliq_AppClients').trigger("chosen:updated");
                $('#ddlFliq_AppClients2').trigger("chosen:updated");
            }
            else {
                ShowNotification(_msgErrorOccured);
                var optClients = '<option value="">All</option>';
                $("#ddlFliq_Clients").html(optClients);
                $("#ddlFliq_AppClients").html(optClients);
                $("#ddlFliq_AppClients2").html(optClients);
                

                selects = $(".chosen-select").chosen({
                    width: "93%"
                });

                $('#ddlFliq_Clients').trigger("chosen:updated");
                $('#ddlFliq_AppClients').trigger("chosen:updated");
                $('#ddlFliq_AppClients2').trigger("chosen:updated");
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured, a);
            var optClients = '<option value="">All</option>';
            $("#ddlFliq_Clients").html(optClients);
            $("#ddlFliq_AppClients").html(optClients);
            $("#ddlFliq_AppClients2").html(optClients);

            selects = $(".chosen-select").chosen({
                width: "93%"
            });

            $('#ddlFliq_Clients').trigger("chosen:updated");
            $('#ddlFliq_AppClients').trigger("chosen:updated");
            $('#ddlFliq_AppClients2').trigger("chosen:updated");
        }
    });
}

function ShowElevatedSupportMessage(chkRole, roleName) {
    if ($(chkRole).is(":checked") && (roleName == "v4UGC" || roleName == "v4API")) {
        ShowNotification("Please contact elevated support to fully enable this role");
    }
}

function ResetPasswordAttempts(customerKey) {
    var jsonPostData = {
        p_CustomerKey: customerKey
    }

    $.ajax({
        url: _urlGlobalAdminResetPasswordAttempts,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                ShowNotification("Login successfully reset");
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}
