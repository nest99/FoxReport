/// <reference path="jquery1.7.2.js" />
var arrowUp = "▲", arrowDown = "▼";
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
    $("#editLayer").width(width).height(keHeight + 21);//加上“保存”按钮的高度
    $("#editLayer").show();
    $("#btnBox").width(width + 2);
    if (id.indexOf("Project_Info") != -1
        || id.indexOf("Teamwork_Info") != -1
        || id.indexOf("Assist_Info") != -1) {
        $("#editLayer").removeClass("centerBox");
    } else {
        $("#editLayer").addClass("centerBox");
    }
}

//点击保存项目名称按钮
function saveProjectName(obj) {
    var id = $(obj).attr("id").substr($(obj).attr("id").indexOf("_") + 1);
    var name = "Project_Info_ProjectName_";
    var projectName = $("#" + name + id).val();
    if ($.trim(projectName) == "") {
        alert("请输入项目名称");
        return;
    }
    $.ajax({
        url: "Home/SaveContent/" + name + id,
        data: "content=" + encodeURIComponent(projectName),
        type: "POST",
        success: function (data) {
            if (id == "0") {//0为新增                
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
    var id = $("#editId").val();
    var content = keditor.html();
    
    $.ajax({
        url: "Home/SaveContent/" + id,
        data: "content=" + encodeURIComponent(content),
        type: "POST",
        success: function (data) {
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
            alert("responseText=" + data.responseText + ", data=" + data);
        }
    });    
}
//点击富文本框取消按钮
function cancelText() {
    $("#editId").val("");
    keditor.html("");
    $("#editLayer").hide();
}
function deleteData(tableName, id){
    var cfm = confirm("你确定要删除此数据吗？");
    if (cfm) {
        $.ajax({
            url: "Home/DeleteData/" + id,
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
}
//处理Tab切换事件
function TabPanelClicked(panelId, tabId, spry, tab) {
    var id;
    var recordId = 0;
    if (panelId.indexOf("ProjectInfo") != -1) {
        id = "#ProjectInfo_" + tabId;
        recordId = tabId.substr(1 + tabId.indexOf("_"));
        panelId = "ProjectInfo";
        tabId = tabId.substr(0, tabId.indexOf("_"));        
    } else {
        id = "#" + panelId + "_" + tabId;
    }
    if ($(id).html().trim() != "") {
        spry.showPanel(tab);
        return;//如果有内容，不从后台获取数据。
    }
    $.ajax({
        url: "Home/" + panelId + "/" + tabId + "?recordId=" + recordId,
        type: "get",
        success: function (data) {            
            $(id).html(data);
            spry.showPanel(tab);
        },
        error: function (data) {
            alert("responseText=" + data.responseText + ", data=" + data);
        }
    });
}

$(document).ready(function () {
    var Summary = new Spry.Widget.TabbedPanels("Summary", { defaultTab: 0 });
    //var Detail = new Spry.Widget.TabbedPanels("Detail", { defaultTab: 0 });
    //var Problem = new Spry.Widget.TabbedPanels("Problem", { defaultTab: 0 });
    //var ProjectDetail = new Spry.Widget.TabbedPanels("ProjectDetail", { defaultTab: 0 });
    //var Project2Detail = new Spry.Widget.TabbedPanels("Project2Detail", { defaultTab: 0 });
    
    //二、项目概述中点击每个项目时展开/折叠详细信息
    $(".projectName").each(function(){
        $(this).click(function () {
            //var id = "#" + $(this).attr("id").replace("_", "Info_");
            //if ($(this).attr("arrow") != "up") {
            //    $(this).attr("arrow", "up");
            //    $(this).children(".projectArrow").removeClass("arrowDown").addClass("arrowUp");
            //    //$(id).show();
            //    $(id).slideDown();
            //} else {
            //    $(this).attr("arrow", "down");
            //    $(this).children(".projectArrow").removeClass("arrowUp").addClass("arrowDown");
            //    //$(id).hide();
            //    $(id).slideUp();
            //}
            projectUpDown(this);
        });
    });
});

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