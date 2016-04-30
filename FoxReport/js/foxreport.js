﻿/// <reference path="jquery1.7.2.js" />

var foxParam = {
    activePage: "Report",
    pageIndexSummaryTargetStrategy: 1,
    pageIndexSummarySuggest: 1,
    pageIndexSummaryVersion: 1,
    pageIndexSummaryFeedback: 1
};

var keditor;
var keHeight = 350;
KindEditor.ready(function (K) {
    keditor = K.create("#keText", {
        width: "100%", height: keHeight, resizeType: 0,
        items: ['source', 'preview', 'code', 'undo', 'redo', '|', 'cut', 'copy', 'paste',
		'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
		'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
		'superscript', 'clearhtml', 'quickformat', '|',
		'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
		'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|',
        'table', 'link', 'unlink']
    });
});
//处理编辑按钮事件<button onclick="editText(this);" class="btnEdit"></button>
function editText(obj) {
    var id = $(obj).siblings("div").attr("id");
    $("#editId").val(id);
    var content = $("#" + id).html();
    keditor.html(content);    
    var width = $(obj).parent().width() + 12;//KindEditor的p标签padding=5,两边就是10；editLayer的padding：0 2 0 0就是2   
    if (width < 170) { width = 170;}
    $("#editLayer").width(width).height(keHeight + 21);//加上“保存”按钮的高度
    $("#editLayer").show();
    $("#editBack").show();
    $("#btnBox").width(width + 2);
    if (id.indexOf("Project_Info") != -1
        || id.indexOf("Teamwork_Info") != -1
        || id.indexOf("Assist_Info") != -1) {
        $("#editLayer").removeClass("centerBox");
    } else {
        $("#editLayer").addClass("centerBox");
    }
}
//function changeIsForeign(n) {
//    if (n) {
//        $("#reportIsForeign").text("国外");
//    } else {
//        $("#reportIsForeign").text("国内");
//    }
//}

//blur事件，保存项目名称按钮
function saveProjectName(obj) {
    var old = $(obj).attr("old");
    var projectName = $(obj).val().trim();

    var name = "Project_Info_ProjectName_";
    var id = $(obj).attr("id").replace(name, "");

    if (old && old == projectName) { //已经有项目名称 且 名称没有改变        
        return;
    }
    if (projectName == "") {
        if (id == "0") {
            return;
        } 
    }
    $(obj).val(projectName).attr("old", projectName);   //保存项目名称

    var userId = $("#ddlTracker").val();
    var week = $("#ddlWeekSearch").val();
    var isForeign = $("input[name=radioCountrySearch]:checked").val();
    
   
    $.ajax({
        url: "Report/SaveContent/" + name + id,
        data: "userId=" + userId + "&week=" + week + "&isForeign=" + isForeign + "&content=" + encodeURIComponent(projectName),
        type: "POST",
        success: function (data) {
            if (id == "0") {//新增项目名称            
                var prjHtml = "<div class='projectBox'>" + $("#newProjectInfo").html().replace(/_0"/g, "_" + data.NewId + "\"") + "</div>";
                $(prjHtml).insertBefore("#newProjectInfo");
                $("#Project_Info_ProjectName_" + data.NewId).val($("#Project_Info_ProjectName_0").val());//设置插入行的项目名称
                $("#Project_Info_ProjectName_0").val(""); //清空新增行的项目名称 
                $("#Project_" + data.NewId).click(function(){
                    projectUpDown(this);//展开/折叠脚本
                });
            } else {//id不是0为编辑已有项目名称
                $("#" + name + id).val(projectName);
            }
        },
        error: function (data) {
            alert("responseText=" + data.responseText + ", data=" + data);
        }
    });
}
//点击富文本框保存按钮
function saveText() {
    loadingShow();
    var id = $("#editId").val();
    var content = keditor.html();
    var userId = $("#ddlTracker").val();
    var week = $("#ddlWeekSearch").val();
    var isForeign = $("input[name=radioCountrySearch]:checked").val();
    $.ajax({
        url: "Report/SaveContent/" + id,
        data: "userId=" + userId + "&week=" + week + "&isForeign=" + isForeign + "&content=" + encodeURIComponent(content),
        type: "POST",
        success: function (data) {
            loadingHide();
            $("#resultMsg").html("<h1>数据保存成功！</h1>").show(200).fadeOut(3000);
            if (id.substr(id.length - 2) == "_0") {//以_0结尾为新增
                if (id.indexOf("Project_Info") != -1) {//新增项目概述
                    var prjHtml = "<div class='projectBox'>" + $("#newProjectInfo").html().replace(/_0"/g, "_" + data.NewId + "\"") + "</div>";
                    $(prjHtml).insertBefore("#newProjectInfo");                    
                    $("#" + id.replace("_0", "_" + data.NewId)).html(content);//显示内容到正确的列
                } else if (id.indexOf("Teamwork_Info") != -1) {
                    $("#Teamwork_Info_Content_0").html(content);
                    $("#Teamwork_Info_Content_0").attr("id", "Teamwork_Info_Content_" + data.NewId);
                } else if (id.indexOf("Assist_Info") != -1) {
                    $("#Assist_Info_Content_0").html(content);
                    $("#Assist_Info_Content_0").attr("id", "Assist_Info_Content_" + data.NewId);
                } else {
                    ids = id.split('_');
                    var trNewId = "#trNew" + ids[1];//获取新增行的id
                    //添加一行数据（复制新增行的html代码，插入新增行之前。修改id为新数据的id)
                    $("<tr class=\"trContent\">" + $(trNewId).html().replace(/_0">/g, "_" + data.NewId + "\">") + "</tr>").insertBefore(trNewId);
                    $("#" + id.replace("_0", "_" + data.NewId)).html(content);//显示内容到正确的列
                }
            } else {//不_0结尾为编辑已有
                $("#" + id).html(content);
            }
            
            cancelText();
        },
        error: function (data) {
            loadingHide();
            alert("responseText=" + data.responseText + ", data=" + data);
        }
    });    
}
//点击富文本框取消按钮
function cancelText() {
    $("#editId").val("");
    keditor.html("");
    $("#editLayer").hide();
    $("#editBack").hide();
}
//删除指定表名、指定id的数据
function deleteData(tableName, id){    
    $.ajax({
        url: "Report/DeleteData/" + id,
        data: "tableName=" + tableName,
        type: "POST",
        success: function (data) {
            if (data.DeleteCount > 0) {
                $("#resultMsg").html("<h1>数据删除成功！</h1>").show(500).fadeOut(5000);
                if (tableName.indexOf("Summary_") != -1) {
                    $("#tr" + tableName.replace("Summary_", "") + id).remove();//删除：一、整体概况的行
                } else if (tableName == "Affair_Product") {
                    $("#trProduct" + id).remove();//删除：三、重点事务 产品事务
                } else if (tableName == "Project_Info") {
                    $("#Project_Info_Box" + id).remove();//删除：项目概况的一条记录
                }                    
            }
        },
        error: function (data) {
            alert("responseText=" + data.responseText + ", data=" + data);
        }
    });    
}

function confirmDelete(tableName, dataId) {
    $("#delTableName").val(tableName);
    $("#delDataId").val(dataId);
    $("#delCover").show();
}
function delOK() {
    deleteData($("#delTableName").val(), $("#delDataId").val());
    clearDeleteData();
}
function clearDeleteData() {
    $("#delTableName").val("");
    $("#delDataId").val("");
    $("#delCover").hide();
}
function delCancel() {    
    clearDeleteData();
}
function getSummaryTab(tab) {
    loadingShow();
    var userId = $("#ddlTracker").val();
    var week = $("#ddlWeekSearch").val();
    var isForeign = $("input[name=radioCountrySearch]:checked").val();
   
    $("#SummaryTabs").find(".activeTab").removeClass("activeTab");
    $("#" + tab).addClass("activeTab");
    $.ajax({
        url: "Report/Summary/" + tab + "?pageIndex=" + foxParam["pageIndexSummary" + tab] +"&userId=" + userId + "&week=" + week + "&isForeign=" + isForeign,
        type: "GET",
        cache:false,
        success: function (data) {
            loadingHide();
            $("#tabSummaryContent").html(data);
        },
        error: function (data) {
            loadingHide();
            alert("responseText=" + data.responseText + ", data=" + data);
        }
    });
}

function getProjectInfoTab(obj) {
    loadingShow();
    var userId = $("#ddlTracker").val();
    var week = $("#ddlWeekSearch").val();
    var isForeign = $("input[name=radioCountrySearch]:checked").val();
    var v = $(obj).attr("id").split("_");
    var column = v[0], recordId = v[1];
    //var showContentId = "#ProjectInfo_" + column + "_" + recordId;//新增内容的div的id
    var showContentId = "#ProjectInfo_Content_" + recordId;
    $("#ProjectInfo_" + recordId).find(".activeTab").removeClass("activeTab");
    $("#" + column + "_" + recordId).addClass("activeTab");
    $.ajax({
        url: "Report/ProjectInfo/" + column + "?recordId=" + recordId + "&userId=" + userId + "&week=" + week + "&isForeign=" + isForeign,
        type: "GET",
        cache: false,
        success: function (data) {
            loadingHide();
            $(showContentId).html(data);
        },
        error: function (data) {
            loadingHide();
            alert("responseText=" + data.responseText + ", data=" + data);
        }
    });
}

//查询
function Search() {
    var isForeign = $("input[name=radioCountrySearch]:checked").val();
    var week = $("#ddlWeekSearch").val();
    var userId = $("#ddlTracker").val();
    //var project = encodeURIComponent( $("#searchProjectName").val() );
    //var start = encodeURIComponent( $("#startDate").val() );
    //var end = encodeURIComponent($("#endDate").val());
    loadingShow();
    $.ajax({
        url: "Report/Search/" + isForeign + "?week=" + week + "&userId=" + userId + "&isForeign=" + isForeign,
        type: "GET",
        success: function (data) {
            $("#pageReportContent").html(data);
            loadingHide();
        },
        error: function (data) {
            loadingHide();
            alert("responseText=" + data.responseText + ", data=" + data);
        }
    });
}
$(document).ready(function () {
    $("#page" + foxParam.activePage).show();    
});
//处理展开/折叠事件
function projectUpDown(obj) {
    var id = "#" + $(obj).attr("id").replace("_", "Info_");
    if ($(obj).attr("arrow") != "up") {
        $(obj).attr("arrow", "up");
        $(obj).children(".projectArrow").removeClass("arrowDown").addClass("arrowUp");
        $(id).slideDown();
    } else {
        $(obj).attr("arrow", "down");
        $(obj).children(".projectArrow").removeClass("arrowUp").addClass("arrowDown");
        $(id).slideUp();
    }
}
//点击菜单，加载页面
function showPage(name) {
    loadingShow();
    
    $.ajax({
        url: name + "/Index",
        type: "GET",
        success: function (data) {
            $("#page" + foxParam.activePage).hide();
            loadingHide();
            foxParam.activePage = name;
            $("#page" + foxParam.activePage + "Content").html(data);//设置页的html
            $("#page" + foxParam.activePage).show();
           
        },
        error: function (data) {
            loadingHide();
            alert("responseText=" + data.responseText + ", data=" + data);
        }
    });
}
function loadingShow(n) {
    if (n) {
        $("#loadingImg").attr("src", "/img/loading" + n + ".gif");
    } else {
        $("#loadingImg").attr("src", "/img/loadingDefault.gif");
    }
    $("#loadingDiv").show();
}
function loadingHide() {
    $("#loadingDiv").hide();
}