//動態時間顯示
//得到時間並寫入div
function getDate() {
    //得到當下時間
    var date = new Date();
    //格式化為本地時間
    var date1 = date.toLocaleString();
    //得到div
    var div1 = document.getElementById("times");
    //將時間寫入div
    div1.innerHTML = date1;
}
//使用定時器每秒向div寫入當前時間
setInterval("getDate()", 1000);
