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

function YNbtn(e,YN,Ename) {
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