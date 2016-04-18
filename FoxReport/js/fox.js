/// <reference path="jquery1.7.2.js" />

var keditor;
var keHeight = 400;
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
    var width = $(obj).parent().width() + 18;//KindEditor的p标签padding=5,两边就是10；editLayer的padding：3 5 3 3就是8
    $("#editLayer").width(width).height(keHeight + 30);//加上“保存”按钮的高度
    $("#editLayer").show();
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

function TabPanelClicked(panelId, tabId) {
    var id = "#" + panelId + "_" + tabId;
    if($(id).html().trim() != ""){
        return;//如果有内容，不从后台获取数据。
    }
    $.ajax({
        url: "Home/" + panelId + "/" + tabId,
        type: "get",
        success: function (data) {
            
            $(id).html(data);
        },
        error: function (data) {
            alert("responseText=" + data.responseText + ", data=" + data);
        }
    });
}

$(document).ready(function () {
    var Summary = new Spry.Widget.TabbedPanels("Summary", { defaultTab: 0 });
    var Detail = new Spry.Widget.TabbedPanels("Detail", { defaultTab: 0 });
    var Problem = new Spry.Widget.TabbedPanels("Problem", { defaultTab: 0 });    
});

