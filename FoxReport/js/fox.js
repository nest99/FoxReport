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

function editText(obj) {
    var id = $(obj).siblings("div").attr("id");
    $("#editId").val(id);
    var content = $("#" + id).html();
    keditor.html(content);    
    var width = $(obj).parent().width() + 12;//KindEditor的p标签padding=5,两边就是10；editLayer的padding：0 2 0 0就是2   
    $("#editLayer").width(width).height(keHeight + 21);//加上“保存”按钮的高度
    $("#editLayer").show();
    $("#btnBox").width(width + 2);
    if (id.indexOf("Project_Info") != -1) {
        $("#editLayer").removeClass("centerBox");
    } else {
        $("#editLayer").addClass("centerBox");
    }
}

function saveText() {
    var id = $("#editId").val();
    var content = keditor.html();
    
    $.ajax({
        url: "Home/SaveContent/" + id,
        data: "content=" + encodeURIComponent(content),
        type: "POST",
        success: function (data) {
            if (id.substr(id.length - 2) == "_0") {//以_0结尾为新增
                ids = id.split('_');
                var trNewId = "#trNew" + ids[1];//获取新增行的id
                //添加一行数据（复制新增行的html代码，插入新增行之前。修改id为新数据的id)
                $("<tr>" + $(trNewId).html().replace(/_0">/g, "_" + data.NewId + "\">") + "</tr>").insertBefore(trNewId);
                $("#" + id.replace("_0", "_" + data.NewId)).html(content);//显示内容到正确的列
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

function cancelText() {
    $("#editId").val("");
    keditor.html("");
    $("#editLayer").hide();
}

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
    
    $(".projectName").each(function(){
        $(this).click(function () {
            var id = "#" + $(this).attr("id").replace("_", "Info_");
            if ($(this).attr("arrow") != "up") {
                $(this).attr("arrow", "up");
                $(this).children(".projectArrow").removeClass("arrowDown").addClass("arrowUp");
                //$(id).show();
                $(id).slideDown();
            } else {
                $(this).attr("arrow", "down");
                $(this).children(".projectArrow").removeClass("arrowUp").addClass("arrowDown");
                //$(id).hide();
                $(id).slideUp();
            }
        });
    });
});

