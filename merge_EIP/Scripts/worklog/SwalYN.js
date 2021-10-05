function delbtn(e) {
    Swal.fire({
        title: '是否確定刪除?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonText: '取消',
        confirmButtonText: '刪除'
    }).then((result) => {
        if (result.isConfirmed) {
            document.location.href = e.target.getAttribute("href");
        }
    })
    return false;
}

function clossbtn(e) {
    Swal.fire({
        title: '是否確定關閉?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonText: '取消',
        confirmButtonText: '確定'
    }).then((result) => {
        if (result.isConfirmed) {
            document.location.href = e.target.getAttribute("href");
        }
    })
    return false;
}

function YNbtn(e, YN, Ename) {
    Swal.fire({
        title: `確定要${YN} ${Ename} 的申請嗎?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3b5998',
        cancelButtonText: '取消',
        confirmButtonText: '確定'
    }).then((result) => {
        if (result.isConfirmed) {
            document.location.href = e.target.getAttribute("href");
        }
    })
    return false;
}

function showIMG(e, Ename) {
    const xhttp = new XMLHttpRequest();
    xhttp.open("POST", "/api/FormImgAPI" + encodeURI(`?FID=${e.target.getAttribute("data-fid")}`), true);
    xhttp.send();
    // 要取得JSON要等他load完才拿得到
    xhttp.onload = () => {
        var data = JSON.parse(xhttp.responseText);

        Swal.fire({
            title: `查看${Ename}的附件`,
            imageUrl: `${data}`,
            //imageWidth: 400,
            //imageHeight: 200,
            imageAlt: 'Custom image',
        })
    }
    return false;
}


