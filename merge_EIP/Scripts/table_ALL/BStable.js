// 判斷是否為電腦登入
function IsPC() {
    var userAgentInfo = navigator.userAgent;
    var Agents = ["Android", "iPhone",
        "SymbianOS", "Windows Phone",
        "iPad", "iPod"];
    var flag = true;
    for (var v = 0; v < Agents.length; v++) {
        if (userAgentInfo.indexOf(Agents[v]) > 0) {
            flag = false;
            break;
        }
    }
    return flag;
}
$('#sort-table').bootstrapTable();
$('#sort-table2').bootstrapTable();
$('#sort-table3').bootstrapTable();
$('#sort-table4').bootstrapTable();

$(function () {
    if (IsPC()) {
        //$("table").colResizable();
    }
});