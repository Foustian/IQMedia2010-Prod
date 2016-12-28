﻿var CONST_ZERO = "0";
var CONST_DeleteNoteIQAgent = "Note:  By deleting this agent, you are also deleting all content that has been populated into your Feeds tab and resulting metrics displayed in your Dashboard tab.  Depending on the amount of content that you are deleting, the timing on content being removed from feeds and updated in your dashboard will vary.  You will receive an email notification once the content has been removed from Feeds and updated in your Dashboard.";
var CONST_SuspendNoteIQAgent = "Note:  By suspending this agent, new content will no longer populate for the agent. Existing content will be kept and displayed.";
var TotalTabs = 0;
var _PreviousZipCodes = [];
$(function () {

    //    $("#divIQAgentSetupTabsHeader").delegate("div", "click", function () {
    //        ShowIQAgentSetupTabs($(this).index());
    //    });

    $(".show-hide input").click(function (event) {
        event.stopPropagation();
    });

    $(".chosen-select").chosen({
        display_disabled_options: true,
        default_item: CONST_ZERO,
        width: "93%"
    });

    $("#ddlIQAgentSetupDMA_TV").chosen().change(function (event) {
        SetZipCodeEnabled();
    });

    $("#txtIQAgentSetupZipCodes").blur(function () {
        LookupDMAs(true);
    });

    $("#txtIQAgentSetupFBPageID").blur(function () {
        LookupFBPageUrls(false);
    });

    $("#txtIQAgentSetupFBPage").blur(function () {
        LookupFBPageIDs(false);
    });

    $("#txtIQAgentSetupExcludeFBPageID").blur(function () {
        LookupFBPageUrls(true);
    });

    $("#txtIQAgentSetupExcludeFBPage").blur(function () {
        LookupFBPageIDs(true);
    });

    $("#txtIQAgentSetupBrandId_LR").blur(function () {
        var txtBrandId = $('#txtIQAgentSetupBrandId_LR').val();
        if (txtBrandId != null && txtBrandId.length > 0) {
            AddSearchImageIds(txtBrandId);
        }
        else {
            $("#spantxtIQAgentSetupBrandId_LR").html("").show();
        }
    });
    $("#txtIQAgentSetupBrandId_LR").keyup(function () {
        var txtBrandId = $('#txtIQAgentSetupBrandId_LR').val();
        if (txtBrandId != null && txtBrandId.length > 0 && txtBrandId.slice(-1) == ';') {
            txtBrandId = txtBrandId.substring(0, txtBrandId.length - 1);
            AddSearchImageIds(txtBrandId);
        }
    });
    $("#txtIQAgentSetupSearchImageId_LR").blur(function () {
        var txtSearchId = $('#txtIQAgentSetupSearchImageId_LR').val();
        if (txtSearchId != null && txtSearchId.length > 0) {
            UpdateDisplayedLogos();
        }
        else {
            $("#spantxtIQAgentSetupSearchImageId_LR").html("").show();
            $("#divLRBrandLogos").html("");
        }
    });

    $("#imgTagUserHelp").popover();
});

function AddSearchImageIds(brandId) {
    //get the search image ids
    $("#btnSubmitIQAgentSetupAddEditForm").attr("disabled", "disabled");
    $("#spantxtIQAgentSetupBrandId_LR").html("").show();

    if (brandId != null) {
        var jsonPostData = { BrandID: brandId };

        $.ajax({
            url: "/Setup/GetSearchImageIDs/",
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result != null && result.isSuccess) {
                    if (result.lstSearchImageIds.length > 0) {
                        $.each(result.lstSearchImageIds, function (index, entry) {
                            if ($("#txtIQAgentSetupSearchImageId_LR").val() == null || $("#txtIQAgentSetupSearchImageId_LR").val().indexOf(entry) == -1) {
                                if ($("#txtIQAgentSetupSearchImageId_LR").val() != null && $("#txtIQAgentSetupSearchImageId_LR").val().trim() != '') {
                                    $("#txtIQAgentSetupSearchImageId_LR").val($("#txtIQAgentSetupSearchImageId_LR").val() + ";");
                                }

                                $("#txtIQAgentSetupSearchImageId_LR").val($("#txtIQAgentSetupSearchImageId_LR").val() + entry);
                            }
                            $("#txtIQAgentSetupBrandId_LR").val('');
                        });

                        UpdateDisplayedLogos();
                    }
                    else {
                        $("#spantxtIQAgentSetupBrandId_LR").html(_msgInvalidBrandID).show();
                    }
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured);
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            }
        });
    }
    $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
}

function UpdateDisplayedLogos() {
    var invalidIDs = '';
    $("#spantxtIQAgentSetupSearchImageId_LR").html("").show();

    var txtSearchIds = $("#txtIQAgentSetupSearchImageId_LR").val();
    if (txtSearchIds != null) txtSearchIds = txtSearchIds.trim();
    if (txtSearchIds != null && txtSearchIds.length > 0 && txtSearchIds.slice(-1) == ';') $("#txtIQAgentSetupSearchImageId_LR").val(txtSearchIds.substring(0, txtSearchIds.length - 1));

    var LRSearchIDs = $("#txtIQAgentSetupSearchImageId_LR").val();
    if (LRSearchIDs != null) {
        $("#btnSubmitIQAgentSetupAddEditForm").attr("disabled", "disabled");

        LRSearchIDs = LRSearchIDs.split(";");
        var jsonPostData = { LRSearchIDs: LRSearchIDs };

        $.ajax({
            url: "/Setup/GetSearchImagesById/",
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result != null && result.isSuccess) {
                    if (result.lstInvalidIDs != '') {
                        $("#spantxtIQAgentSetupSearchImageId_LR").html(_msgInvalidSearchImageID + result.lstInvalidIDs).show();
                        $("#divLRBrandLogos").html("");
                    }
                    else {
                        //foreach brand display all logos
                        $("#divLRBrandLogos").html("");
                        var divContent = '';
                        $.each(result.lstSearchImages, function (index, entry) {
                            divContent += '<div style="width:50px; height:50px; padding-left:10px;" class="float-left"><img src="' + entry + '" title="' + result.lstSearchImageIDs[index] + '" style="display: block; margin: auto; vertical-align: middle; height: auto; width:auto;"/></div>';
                        });
                        $("#divLRBrandLogos").html(divContent);
                    }
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured);
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            }
        });
    }
}

function ShowIQAgentSetupAddEditPopup(id) {

    ClearIQAgentSearchRequestInfo();

    if (id <= 0) {

        $("#divTMSetup").hide();
        $("#divTMTabContent").hide();
        $("#divPMSetup").hide();
        $("#divPMTabContent").hide();
        $("#divTWSetup").hide();
        $("#divTWTabContent").hide();
        $("#divIGSetup").hide();
        $("#divIGTabContent").hide();
        
        $("#divIQAgentSetupPopupTitle").html("Create IQAgent");
        $("#divIQAgentSetupAddEditPopup").modal({
            backdrop: "static",
            keyboard: true,
            dynamic: true
        });
    }
    else {

        var jsonPostData = { p_ID: id }
        $("#divIQAgentSetupPopupTitle").html("Update IQAgent");
        $.ajax({
            url: "/Setup/GetIQAgentSetupAddEditForm/",
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                if (result != null && result.isSuccess) {

                    $("#hdnIQAgentSetupAddEditKey").val(result.iqAgentKey);

                    FillIQAgentMediaInformation(result.queryName, result.searchRequestObject);

                    $("#divIQAgentSetupAddEditPopup").modal({
                        backdrop: "static",
                        keyboard: true,
                        dynamic: true
                    });
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
}
function CancelIQAgentPopup(divModalPopupID) {
    $("#" + divModalPopupID).css({ "display": "none" });
    $("#" + divModalPopupID).modal("hide");
}

function ClearIQAgentSearchRequestInfo() {


    
    $("#divTVSetup").show();
    $("#divNMSetup").show();
    $("#divSMSetup").show();
    $("#divFBSetup").show();
    $("#divIGSetup").show();
    $("#divTWSetup").show();
    $("#divTMSetup").show();
    $("#divPMSetup").show();
    $("#divPQSetup").show();
    

    $("#btnSubmitIQAgentSetupAddEditForm").show();
    $("#frmIQAgentSetupAddEdit span").html("").hide();

    $("#hdnIQAgentSetupAddEditKey").val(CONST_ZERO);
    ShowHideTabdiv(0,true);

    



    $("#chkIQAgentSetup_TV").prop("checked", true);
    $("#chkIQAgentSetup_NM").prop("checked", true);
    $("#chkIQAgentSetup_SM").prop("checked", true);
    $("#chkIQAgentSetup_FB").prop("checked", false);
    $("#chkIQAgentSetup_IG").prop("checked", false);
    $("#chkIQAgentSetup_TW").prop("checked", false);
    $("#chkIQAgentSetup_TM").prop("checked", false);
    $("#chkIQAgentSetup_PM").prop("checked", false);
    $("#chkIQAgentSetup_PQ").prop("checked", true);

    $("#txtIQAgentSetupSearchTerm").val("");
    $("#txtIQAgentSetupTitle").val("");

    // TV fields
    $("#txtIQAgentSetupProgramTitle").val("");
    $("#txtIQAgentSetupAppearing").val("");
    $("#txtIQAgentSetupSearchTerm_TV").val("");
    $("#txtIQAgentSetupZipCodes").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_TV").prop("checked", true);

    $("#ddlIQAgentSetupCategory_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupDMA_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupStation_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupAffiliate_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupRegion_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupCountry_TV").val(CONST_ZERO).trigger("chosen:updated");

    SetZipCodeEnabled();

    // NM fields

    $("#txtIQAgentSetupPublication_NM").val("");
    $("#txtIQAgentSetupSearchTerm_NM").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_NM").prop("checked", true);

    $("#ddlIQAgentSetupCategory_NM").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupPublicationCategory_NM").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupGenere_NM").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupRegion_NM").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupLanguage_NM").val("English").trigger("chosen:updated");
    $("#ddlIQAgentSetupCountry_NM").val(CONST_ZERO).trigger("chosen:updated");

    // SM fields

    $("#txtIQAgentSetupSource_SM").val("");
    $("#txtIQAgentSetupAuthor_SM").val("");
    $("#txtIQAgentSetupTitle_SM").val("");
    $("#txtIQAgentSetupSearchTerm_SM").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_SM").prop("checked", true);

    //$("#ddlIQAgentSetupSourceCategory_SM").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupSourceType_SM").val(CONST_ZERO).trigger("chosen:updated");
    //$("#ddlIQAgentSetupSourceRank_SM").val(CONST_ZERO).trigger("chosen:updated");

    // FB fields
    $("#txtIQAgentSetupSearchTerm_FB").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_FB").prop("checked", true);

    $("#chkIQAgentSetupIncludeDefault").prop("checked", true);
    $("#txtIQAgentSetupFBPageID").val("");
    $("#txtIQAgentSetupFBPage").val("");
    $("#txtIQAgentSetupExcludeFBPageID").val("");
    $("#txtIQAgentSetupExcludeFBPage").val("");

    // IG fields
    $("#txtIQAgentSetupSearchTerm_IG").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_IG").prop("checked", false);
    $("#txtIQAgentSetupIGTag").val("");

    // TW fields

    $("#txtIQAgentSetupActor_TW").val("");
    $("#txtIQAgentSetupGnipTag_TW").val("");
    $("#txtIQAgentSetupFollowersCount_From_TW").val("");
    $("#txtIQAgentSetupFollowersCount_To_TW").val("");
    $("#txtIQAgentSetupFriendsCount_From_TW").val("");
    $("#txtIQAgentSetupFriendsCount_To_TW").val("");
    $("#txtIQAgentSetupKloutScore_From_TW").val("");
    $("#txtIQAgentSetupKloutScore_To_TW").val("");
    $("#txtIQAgentSetupSearchTerm_TW").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_TW").prop("checked", true);

    // TM fields
    $("#txtIQAgentSetupTVEyesSearchGUID_TM").val("");

    $("#txtIQAgentSetupExcludeDomains_NM").val("");
    $("#txtIQAgentSetupExcludeDomains_SM").val("");
    $("#txtIQAgentSetupExcludeHandles_TW").val("");

    // PM fields
    $("#txtIQAgentSetupBLPMIXXml_PM").val("");

    // PQ fields
    $("#txtIQAgentSetupPublication_PQ").val("");
    $("#txtIQAgentSetupAuthor_PQ").val("");
    $("#ddlIQAgentSetupLanguage_PQ").val("English").trigger("chosen:updated");
    $("#txtIQAgentSetupSearchTerm_PQ").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_PQ").prop("checked", true);
}

function FillIQAgentMediaInformation(queryName, searchRequestObject) {

    if (searchRequestObject != null) {

        $("#chkIQAgentSetup_TV").prop("checked", searchRequestObject.TVSpecified);
        $("#chkIQAgentSetup_NM").prop("checked", searchRequestObject.NewsSpecified);
        $("#chkIQAgentSetup_SM").prop("checked", searchRequestObject.SocialMediaSpecified);
        $("#chkIQAgentSetup_FB").prop("checked", searchRequestObject.FacebookSpecified);
        $("#chkIQAgentSetup_IG").prop("checked", searchRequestObject.InstagramSpecified);
        $("#chkIQAgentSetup_TW").prop("checked", searchRequestObject.TwitterSpecified);
        $("#chkIQAgentSetup_TM").prop("checked", searchRequestObject.TMSpecified);
        $("#chkIQAgentSetup_PM").prop("checked", searchRequestObject.PMSpecified);
        $("#chkIQAgentSetup_PQ").prop("checked", searchRequestObject.PQSpecified);
        $("#chkIQAgentSetup_LR").prop("checked", searchRequestObject.LRSpecified);

        // Fill "Title(Query Name)"

        if (queryName != null) {
            $("#txtIQAgentSetupTitle").val(queryName);
        }

        // Fill "Search Term"
        if (searchRequestObject.SearchTerm != null) {
            $("#txtIQAgentSetupSearchTerm").val(searchRequestObject.SearchTerm);
        }


        if (searchRequestObject.TMSpecified) {
            $("#divTMSetup").show();
            $("#divTMTabContent").show();
            ShowHideTabdiv(4,true);

            // radio
            if (searchRequestObject.TM != null && searchRequestObject.TMSpecified == true) {

                if (searchRequestObject.TM.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_TM").val(searchRequestObject.TM.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_TM").prop("checked", searchRequestObject.TM.SearchTerm.IsUserMaster);
                }
                
                $("#txtIQAgentSetupTVEyesSearchGUID_TM").val(searchRequestObject.TM.TVEyesSearchGUID);

            }
        }
        if (searchRequestObject.PMSpecified) {
            $("#divPMSetup").show();
            $("#divPMTabContent").show();
            ShowHideTabdiv(5,true);

            // pm
            if (searchRequestObject.PM != null) {

                if (searchRequestObject.PM.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_PM").val(searchRequestObject.PM.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_PM").prop("checked", searchRequestObject.PM.SearchTerm.IsUserMaster);
                }

                $("#txtIQAgentSetupBLPMIXXml_PM").val(searchRequestObject.PM.BLPMXml);
            }
        }
        if (searchRequestObject.PQSpecified) {
            $("#divPQSetup").show();
            $("#divPQTabContent").show();
            ShowHideTabdiv(6, true);

            if (searchRequestObject.PQ != null) {
                if (searchRequestObject.PQ.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_PQ").val(searchRequestObject.PQ.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_PQ").prop("checked", searchRequestObject.PQ.SearchTerm.IsUserMaster);
                }

                if (searchRequestObject.PQ.Publications != null) {
                    var pubOutput = $.map(searchRequestObject.PQ.Publications, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupPublication_PQ").val(pubOutput);
                }
                if (searchRequestObject.PQ.Authors != null) {
                    var authorOutput = $.map(searchRequestObject.PQ.Authors, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupAuthor_PQ").val(authorOutput);
                }
                if (searchRequestObject.PQ.Language_Set != null) {
                    if (searchRequestObject.PQ.Language_Set.IsAllowAll == false) {
                        var arr_PQ_Language = [];
                        $.each(searchRequestObject.PQ.Language_Set.Language, function (index, obj) {
                            arr_PQ_Language.push(obj);
                        });
                        $("#ddlIQAgentSetupLanguage_PQ").val(arr_PQ_Language).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupLanguage_PQ").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }
            }
        }

        $("#txtIQAgentSetupBrandId_LR").val("");
        $("#txtIQAgentSetupSearchImageId_LR").val("");
        $("#divLRBrandLogos").html("");
        if (searchRequestObject.LRSpecified) {
            if (searchRequestObject.LR.SearchIDs != null) {
                var lrOutput = $.map(searchRequestObject.LR.SearchIDs, function (obj, index) { return obj; }).join(';');
                $("#txtIQAgentSetupSearchImageId_LR").val(lrOutput);
                UpdateDisplayedLogos();
            }
        }
        
            if (searchRequestObject.TV != null && searchRequestObject.TVSpecified == true) {
                $("#txtIQAgentSetupProgramTitle").val(searchRequestObject.TV.ProgramTitle);
                $("#txtIQAgentSetupAppearing").val(searchRequestObject.TV.Appearing);

                if (searchRequestObject.TV.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_TV").val(searchRequestObject.TV.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_TV").prop("checked", searchRequestObject.TV.SearchTerm.IsUserMaster);
                }

                // IQ_Dma_Set
                if (searchRequestObject.TV.IQ_Dma_Set != null) {

                    if (searchRequestObject.TV.IQ_Dma_Set.IsAllowAll == false) {
                        var arr_TV_DMA = [];
                        $.each(searchRequestObject.TV.IQ_Dma_Set.IQ_Dma, function (index, obj) {
                            arr_TV_DMA.push(obj.name);
                        });
                        $("#ddlIQAgentSetupDMA_TV").val(arr_TV_DMA).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupDMA_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }
                SetZipCodeEnabled();    

                // Station_Affiliate_Set
                if (searchRequestObject.TV.Station_Affiliate_Set != null) {

                    if (searchRequestObject.TV.Station_Affiliate_Set.IsAllowAll == false) {
                        var arr_TV_Station_Affil = [];
                        $.each(searchRequestObject.TV.Station_Affiliate_Set.Station_Affil, function (index, obj) {
                            arr_TV_Station_Affil.push(obj.name);
                        });
                        $("#ddlIQAgentSetupAffiliate_TV").val(arr_TV_Station_Affil).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupAffiliate_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // IQ_Station_ID
                if (searchRequestObject.TV.IQ_Station_Set != null) {

                    if (searchRequestObject.TV.IQ_Station_Set.IsAllowAll == false) {
                        var arr_TV_Station_ID = [];
                        $.each(searchRequestObject.TV.IQ_Station_Set.IQ_Station_ID, function (index, obj) {
                            arr_TV_Station_ID.push(obj);
                        });
                        $("#ddlIQAgentSetupStation_TV").val(arr_TV_Station_ID).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupStation_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // IQ_Class_Set
                if (searchRequestObject.TV.IQ_Class_Set != null) {

                    if (searchRequestObject.TV.IQ_Class_Set.IsAllowAll == false) {
                        var arr_TV_IQ_Class = [];
                        $.each(searchRequestObject.TV.IQ_Class_Set.IQ_Class, function (index, obj) {
                            arr_TV_IQ_Class.push(obj.num);
                        });
                        $("#ddlIQAgentSetupCategory_TV").val(arr_TV_IQ_Class).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCategory_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // IQ_Region_Set
                if (searchRequestObject.TV.IQ_Region_Set != null) {

                    if (searchRequestObject.TV.IQ_Region_Set.IsAllowAll == false) {
                        var arr_TV_IQ_Region = [];
                        $.each(searchRequestObject.TV.IQ_Region_Set.IQ_Region, function (index, obj) {
                            arr_TV_IQ_Region.push(obj.num);
                        });
                        $("#ddlIQAgentSetupRegion_TV").val(arr_TV_IQ_Region).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupRegion_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // IQ_Country_Set
                if (searchRequestObject.TV.IQ_Country_Set != null) {

                    if (searchRequestObject.TV.IQ_Country_Set.IsAllowAll == false) {
                        var arr_TV_IQ_Country = [];
                        $.each(searchRequestObject.TV.IQ_Country_Set.IQ_Country, function (index, obj) {
                            arr_TV_IQ_Country.push(obj.num);
                        });
                        $("#ddlIQAgentSetupCountry_TV").val(arr_TV_IQ_Country).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCountry_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Zip Codes
                if (searchRequestObject.TV.ZipCodes != null) {
                    var output = $.map(searchRequestObject.TV.ZipCodes, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupZipCodes").val(output);

                    // Populate _PreviousZipCodes so that removing zip codes will correctly remove the corresponding DMA
                    LookupDMAs(false);
                }
                else {
                    _PreviousZipCodes = [];
                }
            }

            // Online News

            if (searchRequestObject.News != null && searchRequestObject.NewsSpecified == true) {

                if (searchRequestObject.News.Publications != null) {
                    var output = $.map(searchRequestObject.News.Publications, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupPublication_NM").val(output);
                }

                if (searchRequestObject.News.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_NM").val(searchRequestObject.News.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_NM").prop("checked", searchRequestObject.News.SearchTerm.IsUserMaster);
                }

                // NewsCategory_Set
                if (searchRequestObject.News.NewsCategory_Set != null) {

                    if (searchRequestObject.News.NewsCategory_Set.IsAllowAll == false) {
                        var arr_NM_Category = [];
                        $.each(searchRequestObject.News.NewsCategory_Set.NewsCategory, function (index, obj) {
                            arr_NM_Category.push(obj);
                        });
                        $("#ddlIQAgentSetupCategory_NM").val(arr_NM_Category).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCategory_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // PublicationCategory_Set
                if (searchRequestObject.News.PublicationCategory_Set != null) {

                    if (searchRequestObject.News.PublicationCategory_Set.IsAllowAll == false) {
                        var arr_NM_PubCategory = [];
                        $.each(searchRequestObject.News.PublicationCategory_Set.PublicationCategory, function (index, obj) {
                            arr_NM_PubCategory.push(obj);
                        });
                        $("#ddlIQAgentSetupPublicationCategory_NM").val(arr_NM_PubCategory).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupPublicationCategory_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Genre_Set
                if (searchRequestObject.News.Genre_Set != null) {

                    if (searchRequestObject.News.Genre_Set.IsAllowAll == false) {
                        var arr_NM_Genere = [];
                        $.each(searchRequestObject.News.Genre_Set.Genre, function (index, obj) {
                            arr_NM_Genere.push(obj);
                        });
                        $("#ddlIQAgentSetupGenere_NM").val(arr_NM_Genere).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupGenere_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Region_Set
                if (searchRequestObject.News.Region_Set != null) {

                    if (searchRequestObject.News.Region_Set.IsAllowAll == false) {
                        var arr_NM_Region = [];
                        $.each(searchRequestObject.News.Region_Set.Region, function (index, obj) {
                            arr_NM_Region.push(obj);
                        });
                        $("#ddlIQAgentSetupRegion_NM").val(arr_NM_Region).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupRegion_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Language_Set
                if (searchRequestObject.News.Language_Set != null) {

                    if (searchRequestObject.News.Language_Set.IsAllowAll == false) {
                        var arr_NM_Language = [];
                        $.each(searchRequestObject.News.Language_Set.Language, function (index, obj) {
                            arr_NM_Language.push(obj);
                        });
                        $("#ddlIQAgentSetupLanguage_NM").val(arr_NM_Language).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupLanguage_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Country_Set
                if (searchRequestObject.News.Country_Set != null) {

                    if (searchRequestObject.News.Country_Set.IsAllowAll == false) {
                        var arr_NM_Country = [];
                        $.each(searchRequestObject.News.Country_Set.Country, function (index, obj) {
                            arr_NM_Country.push(obj);
                        });
                        $("#ddlIQAgentSetupCountry_NM").val(arr_NM_Country).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCountry_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                if (searchRequestObject.News.ExlcudeDomains != null) {
                    var output = $.map(searchRequestObject.News.ExlcudeDomains, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupExcludeDomains_NM").val(output);
                }
            }

            // Social Media

            if (searchRequestObject.SocialMedia != null && searchRequestObject.SocialMediaSpecified == true) {
                $("#txtIQAgentSetupAuthor_SM").val(searchRequestObject.SocialMedia.Author);
                $("#txtIQAgentSetupTitle_SM").val(searchRequestObject.SocialMedia.Title);

                if (searchRequestObject.SocialMedia.Sources != null) {
                    var output = $.map(searchRequestObject.SocialMedia.Sources, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupSource_SM").val(output);
                }

                if (searchRequestObject.SocialMedia.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_SM").val(searchRequestObject.SocialMedia.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_SM").prop("checked", searchRequestObject.SocialMedia.SearchTerm.IsUserMaster);
                }

                // SourceType_Set
                if (searchRequestObject.SocialMedia.SourceType_Set != null) {

                    if (searchRequestObject.SocialMedia.SourceType_Set.IsAllowAll == false) {
                        var arr_SM_SourceType = [];
                        $.each(searchRequestObject.SocialMedia.SourceType_Set.SourceType, function (index, obj) {
                            arr_SM_SourceType.push(obj);
                        });
                        $("#ddlIQAgentSetupSourceType_SM").val(arr_SM_SourceType).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupSourceType_SM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                if (searchRequestObject.SocialMedia.ExlcudeDomains != null) {
                    var output = $.map(searchRequestObject.SocialMedia.ExlcudeDomains, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupExcludeDomains_SM").val(output);
                }
            }

            // Facebook

            if (searchRequestObject.Facebook != null && searchRequestObject.FacebookSpecified == true) {

                if (searchRequestObject.Facebook.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_FB").val(searchRequestObject.Facebook.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_FB").prop("checked", searchRequestObject.Facebook.SearchTerm.IsUserMaster);
                }

                $("#chkIQAgentSetupIncludeDefault").prop("checked", searchRequestObject.Facebook.IncludeDefaultPages);

                if (searchRequestObject.Facebook.FBPages != null) {
                    var FBPageIDs = $.map(searchRequestObject.Facebook.FBPages, function (obj, index) { return obj.ID; }).join('; ');
                    var FBPages = $.map(searchRequestObject.Facebook.FBPages, function (obj, index) { return obj.Page; }).join('; ');
                    $("#txtIQAgentSetupFBPageID").val(FBPageIDs);
                    $("#txtIQAgentSetupFBPage").val(FBPages);
                }

                if (searchRequestObject.Facebook.ExcludeFBPages != null) {
                    var ExcludeFBPageIDs = $.map(searchRequestObject.Facebook.ExcludeFBPages, function (obj, index) { return obj.ID; }).join('; ');
                    var ExcludeFBPages = $.map(searchRequestObject.Facebook.ExcludeFBPages, function (obj, index) { return obj.Page; }).join('; ');
                    $("#txtIQAgentSetupExcludeFBPageID").val(ExcludeFBPageIDs);
                    $("#txtIQAgentSetupExcludeFBPage").val(ExcludeFBPages);
                }
            }

            // Instagram

            if (searchRequestObject.Instagram != null && searchRequestObject.InstagramSpecified == true) {
                if (searchRequestObject.Instagram.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_IG").val(searchRequestObject.Instagram.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_IG").prop("checked", searchRequestObject.Instagram.SearchTerm.IsUserMaster);
                }

                if (searchRequestObject.Instagram.UserTagString != null) {
                    $("#txtIQAgentSetupIGTag").val(searchRequestObject.Instagram.UserTagString);
                }
            }

            // Twitter

            if (searchRequestObject.Twitter != null && searchRequestObject.TwitterSpecified == true) {

                $("#txtIQAgentSetupActor_TW").val(searchRequestObject.Twitter.Actor);

                if (searchRequestObject.Twitter.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_TW").val(searchRequestObject.Twitter.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_TW").prop("checked", searchRequestObject.Twitter.SearchTerm.IsUserMaster);
                }

                var output = $.map(searchRequestObject.Twitter.GnipTagList, function (obj, index) { return obj; }).join('; ');
                $("#txtIQAgentSetupGnipTag_TW").val(output);

                if (searchRequestObject.Twitter.ActorFriendsRange != null) {
                    $("#txtIQAgentSetupFriendsCount_From_TW").val(searchRequestObject.Twitter.ActorFriendsRange.From);
                    $("#txtIQAgentSetupFriendsCount_To_TW").val(searchRequestObject.Twitter.ActorFriendsRange.To);
                }
                if (searchRequestObject.Twitter.ActorFollowersRange != null) {
                    $("#txtIQAgentSetupFollowersCount_From_TW").val(searchRequestObject.Twitter.ActorFollowersRange.From);
                    $("#txtIQAgentSetupFollowersCount_To_TW").val(searchRequestObject.Twitter.ActorFollowersRange.To);
                }
                if (searchRequestObject.Twitter.KloutScoreRange != null) {
                    $("#txtIQAgentSetupKloutScore_From_TW").val(searchRequestObject.Twitter.KloutScoreRange.From);
                    $("#txtIQAgentSetupKloutScore_To_TW").val(searchRequestObject.Twitter.KloutScoreRange.To);
                }

                if (searchRequestObject.Twitter.ExclusionHandles != null) {
                    var output = $.map(searchRequestObject.Twitter.ExclusionHandles, function (obj, index) { return "@" + obj; }).join('; ');
                    $("#txtIQAgentSetupExcludeHandles_TW").val(output);
                }
            }    
    }
}

function SubmitIQAgentSetup() {
    //need to know if valid before message gets erased
    var SearchImageErrorMsg = "";
    if ($("#spantxtIQAgentSetupSearchImageId_LR").text().trim() != '') {
        SearchImageErrorMsg = $("#spantxtIQAgentSetupSearchImageId_LR").text();
    }

    $("#frmIQAgentSetupAddEdit span").html("").hide();

    var flag = true;
    var stringToTest;

    if ($.trim($("#txtIQAgentSetupTitle").val()) == "") {
        $("#spanIQAgentSetupTitle").html(_msgIQAgentSetupTitleRequiredField).show();
        flag = false;
    }

    flag = flag && ValidateSearchTerm('', '', 'txtIQAgentSetupSearchTerm', 'spanIQAgentSetupSearchTerm');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_TV', 'chkIQAgentSetupUserMasterSearchTerm_TV', 'txtIQAgentSetupSearchTerm_TV', 'spantxtIQAgentSetupSearchTerm_TV');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_NM', 'chkIQAgentSetupUserMasterSearchTerm_NM', 'txtIQAgentSetupSearchTerm_NM', 'spantxtIQAgentSetupSearchTerm_NM');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_SM', 'chkIQAgentSetupUserMasterSearchTerm_SM', 'txtIQAgentSetupSearchTerm_SM', 'spantxtIQAgentSetupSearchTerm_SM');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_TW', 'chkIQAgentSetupUserMasterSearchTerm_TW', 'txtIQAgentSetupSearchTerm_TW', 'spantxtIQAgentSetupSearchTerm_TW');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_PQ', 'chkIQAgentSetupUserMasterSearchTerm_PQ', 'txtIQAgentSetupSearchTerm_PQ', 'spantxtIQAgentSetupSearchTerm_PQ');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_FB', 'chkIQAgentSetupUserMasterSearchTerm_FB', 'txtIQAgentSetupSearchTerm_FB', 'spantxtIQAgentSetupSearchTerm_FB');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_IG', '', 'txtIQAgentSetupSearchTerm_IG', ''); 

    if (SearchImageErrorMsg != null && SearchImageErrorMsg.length > 0) {
        $("#spantxtIQAgentSetupSearchImageId_LR").html(SearchImageErrorMsg).show();
        flag = false;
    }
    
    if ($('#chkIQAgentSetup_TV').length > 0 && $('#chkIQAgentSetup_TV').is(":checked") && $.trim($("#txtIQAgentSetupZipCodes").val()) != "") {
        var zipCodes = $.trim($("#txtIQAgentSetupZipCodes").val()).split(';');
        $.each(zipCodes, function (index, obj) {
            if (!ValidateZipCode($.trim(obj))) {
                $("#spantxtIQAgentSetupZipCodes").html(_msgInvalidZipCode + obj).show();
                flag = false;
            }
        });
    }

    if ($('#chkIQAgentSetup_NM').length > 0 && $('#chkIQAgentSetup_NM').is(":checked") && $.trim($("#txtIQAgentSetupExcludeDomains_NM").val()) != "") {
        var domains = $.trim($("#txtIQAgentSetupExcludeDomains_NM").val()).split(';');
        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);

            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }

            if (!TestWildInput(stringToTest)) {
                $("#spanIQAgentSetupExcludeDomains_NM").html(_msgInvalidDomain + obj).show();
                flag = false;
            }
        });
        
    }

    if ($('#chkIQAgentSetup_SM').length > 0 && $('#chkIQAgentSetup_SM').is(":checked") && $.trim($("#txtIQAgentSetupExcludeDomains_SM").val()) != "") {
        var domains = $.trim($("#txtIQAgentSetupExcludeDomains_SM").val()).split(';');
        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);
            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }

            if (!TestWildInput(stringToTest)) {
                $("#spanIQAgentSetupExcludeDomains_SM").html(_msgInvalidDomain+ obj).show();
                flag = false;
            }    
        });
    }

    if ($("#chkIQAgentSetup_FB").length > 0 && $("#chkIQAgentSetup_FB").is(":checked")) {
        if ($.trim($("#txtIQAgentSetupFBPageID").val()) == "") {
            // Only allow no pages if the Include Default checkbox is checked
            if (!$("#chkIQAgentSetupIncludeDefault").is(":checked")) {
                $("#spanIQAgentSetupFBPageID").html(_msgNoFBPages).show();
                flag = false;
            }
        }
        else {
            var FBPageIDs = $.trim($("#txtIQAgentSetupFBPageID").val()).split(';');
            $.each(FBPageIDs, function (index, obj) {
                if (obj != parseInt(obj)) {
                    $("#spanIQAgentSetupFBPageID").html(_msgInvalidFBPageID + obj).show();
                    flag = false;
                }
            });
        }

        if ($.trim($("#txtIQAgentSetupExcludeFBPageID").val()) != "") {
            var ExcludeFBPageIDs = $.trim($("#txtIQAgentSetupExcludeFBPageID").val()).split(';');
            $.each(ExcludeFBPageIDs, function (index, obj) {
                if (obj != parseInt(obj)) {
                    $("#spanIQAgentSetupExcludeFBPageID").html(_msgInvalidFBPageID + obj).show();
                    flag = false;
                }
            });
        }
    }

    if (CheckForDuplicateFBPages()) {
        $("#spanIQAgentSetupExcludeFBPageID").html("1 or more pages have been specified in both the include and exclude lists").show();
        flag = false;
    }

    if ($("#chkIQAgentSetup_IG").length > 0 && $("#chkIQAgentSetup_IG").is(":checked") && !ValidateIGTagsUsers()) {
        flag = false;
    }

    $('.chosen-select').trigger("chosen:updated");

    if (flag == true) {
        $("#frmIQAgentSetupAddEdit").ajaxSubmit({
            target: "",
            success: function (res) {
                if (res.isSuccess == true) {
                    if (res.iqAgentKey <= 0) {
                        ShowPopUpNotification(res.msg);
                        $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
                    }
                    else {
                        CancelIQAgentPopup('divIQAgentSetupAddEditPopup');
                        GetIQAgentSetupContent(res.iqAgentKey);
                        ShowNotification(res.msg);
                    }
                }
                else {
                    ShowNotification(res.msg);
                    $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
                }
            },
            error: function () {
                CancelIQAgentPopup('divIQAgentSetupAddEditPopup');
                ShowNotification(_msgIQAgentSetupSaveError);
            }
        });
    }


    
}

function ShowHideTabdiv(elementIndex,isClearOther) {

    var EleID = '';
    var HeaderID = '';
    switch (elementIndex) {
        case 0:
            EleID = 'divTVTabContent'
            HeaderID = 'divTVSetup';
            break;
        case 1:
            EleID = 'divOnlineNewsTabContent';
            HeaderID = 'divNMSetup';
            break;
        case 2:
            EleID = 'divSocialMediaTabContent';
            HeaderID = 'divSMSetup';
            break;
        case 3:
            EleID = 'divTwitterTabContent';
            HeaderID = 'divTWSetup';
            break;
        case 4:
            EleID = 'divTMTabContent';
            HeaderID = 'divTMSetup';
            break;
        case 5:
            EleID = 'divPMTabContent';
            HeaderID = 'divPMSetup';
            break;
        case 6:
            EleID = 'divPQTabContent';
            HeaderID = 'divPQSetup';
            break;
        case 7:
            EleID = 'divFBTabContent';
            HeaderID = 'divFBSetup';
            break;
        case 8:
            EleID = 'divIGTabContent';
            HeaderID = 'divIGSetup';
            break;
        case 9:
            EleID = 'divLRTabContent';
            HeaderID = 'divLRSetup';
            break;
    }

    if ($("#" + EleID).is(':visible')) {
        $("#" + EleID).hide('slow');
        $("#" + HeaderID).find('img').attr('src', '../images/show.png')
    }
    else {
        $("#" + EleID).show('slow');
        $("#" + HeaderID).find('img').attr('src', '../images/hiden.png')
    }

    if (isClearOther == true) {
        for (idx = 0; idx <= 6; idx++) {
            if (idx != elementIndex) {
                $("#divIQAgentSetupTabs").children().eq(idx * 2 + 1).hide();
                $("#divIQAgentSetupTabs").children().eq(idx * 2).find('img').attr('src', '../images/show.png')
            }
        }
    }
}


function GetIQAgentSetupContent(iqagentkey) {

    $("#divSetupContent").html("");

    $.ajax({
        url: "/Setup/DisplayIQAgentSetupContent/",
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: {},
        success: function (result) {
            if (result != null && result.isSuccess) {
                $("#divSetupContent").html(result.HTML);

                // Display some animation for record that is added/updated
                if (iqagentkey > 0) {
                    //alert(iqagentkey);
                    setTimeout(function () { $("#divSetupIQAgentSearchRequestList_ScrollContent").mCustomScrollbar("scrollTo", "#divIQAgentSearchRequest_" + iqagentkey); }, 500);

                    $("#divIQAgentSearchRequest_" + iqagentkey).animate({ backgroundColor: "#EDB5CC" }, 1000, function () {
                        $("#divIQAgentSearchRequest_" + iqagentkey).animate
                                        ({
                                            backgroundColor: '#fff'
                                        }, 1500);

                    });
                }
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

function DisplayIQAGentSearchXML(itemid) {
    var jsonPostData = { p_ID: itemid }
    $("#textareaIQAgentSetupDisplayXML").val("");
    $.ajax({
        url: _urlSetupIQAgentSearchDisplayXML,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            if (result != null && result.isSuccess) {

                $("#textareaIQAgentSetupDisplayXML").val(result.HTML);

                $("#divIQAgentSetupdisplayXMLPopup").modal({
                    backdrop: "static",
                    keyboard: true,
                    dynamic: true
                });
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


function DeleteIQAgent(itemid, queryname) {
    var jsonPostData = { p_ID: itemid };

    getConfirm("Delete IQAgent - " + queryname + "", CONST_DeleteNoteIQAgent, "Confirm Delete", "Cancel", function (res) {
        if (res) {
            $.ajax({
                url: _urlSetupIQAgentDeleteIQAgent,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    if (result != null && result.isSuccess) {
                        ShowNotification(result.msg);
                        GetIQAgentSetupContent(0);
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
    });
}

var SuspendAgent = function (itemid, queryname, obj) {
    var jsonPostData = { p_ID: itemid };

    getConfirm("Suspend IQAgent - " + queryname + "", CONST_SuspendNoteIQAgent, "Confirm Suspend", "Cancel", function (res) {
        if (res) {
            $.ajax({
                url: _urlSetupAgentSuspendAgent,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    if (result != null && result.isSuccess) {
                        ShowNotification(result.msg);

                        $("#divName_" + itemid).toggleClass("suspended");
                        $("#divInfo_" + itemid).toggleClass("suspended");
                        $("#aNotif_" + itemid).toggleClass("displayNone");
                        $("#aEdit_" + itemid).toggleClass("displayNone");

                        var parent = $(obj).parent();
                        $(obj).remove();
                        $(parent).append('<a href="#" onclick="ResumeSuspendedAgent(' + itemid + ',\'' + queryname.replace(/'/g, '\\\'') + '\',this)" title="Resume">'
                                    + '<img src="/Images/unsuspend.png" /></a>');
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
    });
}

var ResumeSuspendedAgent = function (itemid, queryname, obj) {
    var jsonPostData = { p_ID: itemid };

    getConfirm("Suspend IQAgent - " + queryname + "", "Are you sure to resume the agent?", "Confirm", "Cancel", function (res) {
        if (res) {
            $.ajax({
                url: _urlSetupAgentResumeSuspendedAgent,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    if (result != null && result.isSuccess) {
                        ShowNotification("Agent resumed successfully.");

                        $("#divName_" + itemid).toggleClass("suspended");
                        $("#divInfo_" + itemid).toggleClass("suspended");
                        $("#aNotif_" + itemid).toggleClass("displayNone");
                        $("#aEdit_" + itemid).toggleClass("displayNone");

                        var parent = $(obj).parent();
                        $(obj).remove();
                        $(parent).append('<a href="#" onclick="SuspendAgent(' + itemid + ',\'' + queryname.replace(/'/g, '\\\'') + '\',this)" title="Suspend">'
                                    + '<img src="/Images/suspend.png" /></a>');
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
    });
}

function DisplayIQNotificationSettings(searchrequestid) {

    $("#hdnIQAgentSearchRequestID").val("");
    $("#divIQNotificationMessage").html("")

    var jsonPostData = { p_SearchRequestID: searchrequestid }

    $.ajax({
        url: _urlSetupIQAgentNoficationSettingsDisplay,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            if (result != null && result.isSuccess == true) {

                $("#divIQAgentSetupIQNotificationHTML").html(result.HTML);
                $("#hdnIQAgentSearchRequestID").val(searchrequestid);

                $("#divIQAgentSetupIQNotificationPopup").modal({
                    backdrop: "static",
                    keyboard: true,
                    dynamic: true
                });


                $("#divIQAgentSetupIQNotificationHTML .chosen-select").chosen({
                    display_disabled_options: true,
                    default_item: CONST_ZERO,
                    width: null,
                    inherit_select_classes : true
                });

                //$("#divIQAgentSetupIQNotificationHTML .chosen-container .search-field input").css("width", "100%")

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

function SetZipCodeEnabled() {
    var selectedDMAs = $("#ddlIQAgentSetupDMA_TV").chosen().val();
    if (selectedDMAs != null && selectedDMAs.length == 1 && selectedDMAs[0] == CONST_ZERO) {
        $("#txtIQAgentSetupZipCodes").attr("disabled", "disabled");
    }
    else {
        $("#txtIQAgentSetupZipCodes").removeAttr("disabled");
    }
}

function LookupDMAs(setSelectedDMAs) {
    $("#spantxtIQAgentSetupZipCodes").hide();
    var currentZipCodes = [];
    var dmaPair;
    var selectedDMAs = $("#ddlIQAgentSetupDMA_TV").chosen().val();
    if (selectedDMAs == null) {
        selectedDMAs = [];
    }
    
    if ($.trim($("#txtIQAgentSetupZipCodes").val()) != "") {
        var flag = true;
        var zipCodes = $.trim($("#txtIQAgentSetupZipCodes").val()).split(';');
        $.each(zipCodes, function (index, obj) {
            var zipCode = $.trim(obj);
            if (!ValidateZipCode(zipCode)) {
                $("#spantxtIQAgentSetupZipCodes").html(_msgInvalidZipCode + zipCode).show();
                flag = false;
            }
            else if ($.inArray(zipCode, currentZipCodes) == -1) {
                currentZipCodes.push(zipCode);
            }
        });

        if (flag == true) {
            var jsonPostData = { zipCodes: currentZipCodes };

            $.ajax({
                url: _urlCommonGetDMAsByZipCode,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    if (result != null && result.isSuccess == true) {
                        currentZipCodes = result.dmas;

                        if (setSelectedDMAs) {
                            // For any zip codes that were deleted, unselect the corresponding DMAs
                            $.each(_PreviousZipCodes, function (index, obj) {
                                if ($.inArray(obj, currentZipCodes) == -1) {
                                    dmaPair = obj.split(':');
                                    selectedDMAs.splice($.inArray(dmaPair[1], selectedDMAs), 1);
                                }
                            });

                            // For any zip codes that were added, select the corresponding DMAs
                            if (currentZipCodes.length > 0) {

                                $.each(result.dmas, function (index, obj) {
                                    dmaPair = obj.split(':'); // ZipCode:DMA
                                    if ($.inArray(dmaPair[1], selectedDMAs) == -1) {
                                        selectedDMAs.push(dmaPair[1]);
                                    }
                                });

                                $("#ddlIQAgentSetupDMA_TV").val(selectedDMAs).trigger("chosen:updated");
                            }

                            if (result.invalidZipCodeMsg != "") {
                                $("#spantxtIQAgentSetupZipCodes").html(_msgInvalidZipCode + result.invalidZipCodeMsg).show();
                            }
                        }

                        _PreviousZipCodes = currentZipCodes;
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
    }
    else if (_PreviousZipCodes.length > 0) {
        $.each(_PreviousZipCodes, function (index, obj) {
            if ($.inArray(obj, currentZipCodes) == -1) {
                dmaPair = obj.split(':');
                selectedDMAs.splice($.inArray(dmaPair[1], selectedDMAs), 1);
            }
        });

        $("#ddlIQAgentSetupDMA_TV").val(selectedDMAs).trigger("chosen:updated");
        _PreviousZipCodes = [];
    }
}

function LookupFBPageUrls(isExclude) {
    var txtFBPageIDs;
    var txtFBPages;
    var spanFBPageIDs;
    var spanFBPages;

    if (isExclude) {
        txtFBPageIDs = $("#txtIQAgentSetupExcludeFBPageID");
        txtFBPages = $("#txtIQAgentSetupExcludeFBPage");
        spanFBPageIDs = $("#spanIQAgentSetupExcludeFBPageID");
        spanFBPages = $("#spanIQAgentSetupExcludeFBPage");
    }
    else {
        txtFBPageIDs = $("#txtIQAgentSetupFBPageID");
        txtFBPages = $("#txtIQAgentSetupFBPage");
        spanFBPageIDs = $("#spanIQAgentSetupFBPageID");
        spanFBPages = $("#spanIQAgentSetupFBPage");
    }

    spanFBPageIDs.hide();
    spanFBPages.hide();

    if ($.trim(txtFBPageIDs.val()) != "") {
        var isValid = true;
        var FBPageIDs = $.map($.trim(txtFBPageIDs.val()).split(';'), function (obj, index) {
            return $.trim(obj);
        });
        $.each(FBPageIDs, function (index, obj) {
            if (obj != parseInt(obj)) {
                spanFBPageIDs.html("Invalid Facebook Page ID " + obj).show();
                isValid = false;
            }
        });

        if (isValid) {
            $("#btnSubmitIQAgentSetupAddEditForm").attr("disabled", "disabled");

            var jsonPostData = { FBPageIDs: FBPageIDs };

            $.ajax({
                url: _urlSetupGetFBPageUrlsByID,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result != null && result.isSuccess) {
                        txtFBPages.val(result.FBPages);

                        if (result.invalidIDs != "") {
                            spanFBPageIDs.html(_msgInvalidFBPageID + result.invalidIDs).show();
                        }
                    }
                    else {
                        ShowNotification(_msgErrorOccured);
                    }
                    $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
                },
                error: function (a, b, c) {
                    ShowNotification(_msgErrorOccured);
                    $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
                }
            });     
        }
    }
    else {
        txtFBPages.val("");
    }
}

function LookupFBPageIDs(isExclude) {
    var txtFBPageIDs;
    var txtFBPages;
    var spanFBPageIDs;
    var spanFBPages;

    if (isExclude) {
        txtFBPageIDs = $("#txtIQAgentSetupExcludeFBPageID");
        txtFBPages = $("#txtIQAgentSetupExcludeFBPage");
        spanFBPageIDs = $("#spanIQAgentSetupExcludeFBPageID");
        spanFBPages = $("#spanIQAgentSetupExcludeFBPage");
    }
    else {
        txtFBPageIDs = $("#txtIQAgentSetupFBPageID");
        txtFBPages = $("#txtIQAgentSetupFBPage");
        spanFBPageIDs = $("#spanIQAgentSetupFBPageID");
        spanFBPages = $("#spanIQAgentSetupFBPage");
    }

    spanFBPageIDs.hide();
    spanFBPages.hide();

    if ($.trim(txtFBPages.val()) != "") {
        $("#btnSubmitIQAgentSetupAddEditForm").attr("disabled", "disabled");

        var FBPages = $.map($.trim(txtFBPages.val()).split(';'), function (obj, index) {
            return $.trim(obj);
        });
        var jsonPostData = { FBPages: FBPages };

        $.ajax({
            url: _urlSetupGetFBPageIDsByUrl,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result != null && result.isSuccess) {
                    txtFBPageIDs.val(result.FBPageIDs);

                    if (result.invalidPages != "") {
                        spanFBPages.html(_msgInvalidFBPage + result.invalidPages).show();
                    }
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured);
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            }
        });
    }
    else {
        txtFBPageIDs.val("");
    }
}

// Don't allow a page to be both included and excluded
function CheckForDuplicateFBPages() {
    var txtFBPageIDs = $("#txtIQAgentSetupFBPageID");
    var txtExcludeFBPageIDs = $("#txtIQAgentSetupExcludeFBPageID");
    var isDuplicate = false;

    if ($.trim(txtFBPageIDs.val()) != "" && $.trim(txtExcludeFBPageIDs.val()) != "") {
        var FBPageIDs = $.map($.trim(txtFBPageIDs.val()).split(';'), function (obj, index) {
            return $.trim(obj);
        });
        var excludeFBPageIDs = $.map($.trim(txtExcludeFBPageIDs.val()).split(';'), function (obj, index) {
            return $.trim(obj);
        });

        $.each(excludeFBPageIDs, function (index, obj) {
            if ($.inArray(obj, FBPageIDs) > -1) {
                isDuplicate = true;
            }
        });
    }

    return isDuplicate;
}

function ValidateIGTagsUsers() {
    var text = $("#txtIQAgentSetupIGTag").val();

    // textbox cannot be empty and must contain a user or a tag
    if (text.length == 0 || (text.indexOf("@") < 0 && text.indexOf("#") < 0)) {
        $("#spanIQAgentSetupIGTag").html("Must have a valid user or tag").show();
        return false;
    }

    //user cannot be @ or @) and tag cannot be # or #)
    var indicesUser = [];
    var indicesTag = [];
    var valid = true;
    for(var i=0; i<text.length;i++) {
        if (text[i] === "@") indicesUser.push(i);
        if (text[i] === "#") indicesTag.push(i);
    }
    $.each(indicesUser, function (index, value) {
        if ((text.length - 1) == value || (text[value + 1] == " ") || (text[value + 1] == ")")) {
            $("#spanIQAgentSetupIGTag").html("Invalid user").show();
            valid = false;
            return false;
        }
    });
    $.each(indicesTag, function (index, value) {
        if ((text.length - 1) == value || (text[value + 1] == " ") || (text[value + 1] == ")")) {
            $("#spanIQAgentSetupIGTag").html("Invalid tag").show();
            valid = false;
            return false;
        }
    });
    if (valid == false) {
        return false;
    }

    // Validate matching parentheses
    var openParens = 0;

    for (var i = 0; i < text.length; i++) {
        if (text[i] == "(") {
            openParens++;
        }
        else if (text[i] == ")") {
            openParens--;
        }
    }

    if (openParens != 0) {
        $("#spanIQAgentSetupIGTag").html("Unequal number of parentheses.").show();
        return false;
    }

    // Perform regex validation
    var nestingResult = /[@#]\S*[@#]/.exec(text); // Any instance of @@, @#, #@, or ##, with 0 or more non-whitespace characters between them
    if (nestingResult != null) {
        $("#spanIQAgentSetupIGTag").html("Invalid value: " + nestingResult).show();
        return false;
    }

    var invalidUser = /@\.\w+/.exec(text); // Users starting with .
    if (invalidUser != null) {
        $("#spanIQAgentSetupIGTag").html("Invalid user: " + invalidUser).show();
        return false;
    }

    invalidUser = /@\w+\.(?:\s|$|\))/.exec(text);  // Users ending with .
    if (invalidUser != null) {
        $("#spanIQAgentSetupIGTag").html("Invalid user: " + invalidUser).show();
        return false;
    }

    var invalidCharacters = /[^\w.#@\s\(\)]+/.exec(text);  // Any character that is not alphanumeric, not whitespace, and not in [()._]
    if (invalidCharacters != null) {
        $("#spanIQAgentSetupIGTag").html("Invalid value: " + invalidCharacters).show();
        return false;
    }

    var missingQualifier = /(?:^|\s|\()(?!OR\s|NOT\s|AND\s)[\w._]+/.exec(text); // Text that is not preceded by @ or #, and is not a solr keyword (OR, AND, NOT)
    if (missingQualifier != null) {
        $("#spanIQAgentSetupIGTag").html("Must specify either tag (#) or user (@): " + missingQualifier).show();
        return false;
    }

    return true;
}

function ValidateSearchTerm(chkMediumID, chkSearchTermID, txtSearchTermID, spanErrorMsgID) {
    if ((chkMediumID == '' && chkSearchTermID == '') || ($('#' + chkMediumID).is(":checked") && (chkSearchTermID == '' || !$('#' + chkSearchTermID).is(":checked")))) {
        //update the smart quotes to regular quotes
        var searchTerm = $("#" + txtSearchTermID).val().trim().replace(/[\u201C\u201D]/g, '"')/*.replace(/[\u2018\u2019]/g, "'")*/;
        $("#" + txtSearchTermID).val(searchTerm);

        if (chkSearchTermID != '' && $.trim($("#" + txtSearchTermID).val()) == "") {
            $("#" + spanErrorMsgID).html(_msgIQAgentSetupSearchTermRequiredField).show();
            return false;
        }
        else {
            // Basic verification for matching numbers of " and (), to help prevent errors in solr
            var openParens = 0;
            var doubleQuotes = 0;

            for (var i = 0; i < searchTerm.length; i++) {
                if (searchTerm[i] == "(") {
                    openParens++;
                }
                else if (searchTerm[i] == ")") {
                    openParens--;
                }
                else if (searchTerm[i] == "\"") {
                    doubleQuotes++;
                }
            }

            if (doubleQuotes % 2 != 0) {
                $("#" + spanErrorMsgID).html("Unequal number of double quotes.").show();
                return false;
            }
            if (openParens != 0) {
                $("#" + spanErrorMsgID).html("Unequal number of parentheses.").show();
                return false;
            }
        }
    }

    return true;
}
