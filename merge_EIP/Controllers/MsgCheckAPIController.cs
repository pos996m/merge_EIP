using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace merge_EIP.Controllers
{
    public class MsgCheckAPIController : ApiController
    {
        FormModelEntities db = new FormModelEntities();

        // GET: api/MsgCheckAPI
        public JsonResult<List<watchCount>> Get(int num, string state)
        {
            var checkList = db.watchCount.Where(x => x.messageboardNumber == num).ToList();
            if (state == "部門")
            {
                checkList = checkList.Where(x => x.Employee.departmentID == x.messageBoard.assignDepartment).ToList();
            }
            else if (state == "私人")
            {
                checkList = checkList.Where(x => x.employeeID == x.messageBoard.assignPerson || x.employeeID == x.messageBoard.employeeID).ToList();
            }
            int cnt = 0;

            // 將名字匯入自訂屬性
            foreach (var item in checkList)
            {
                checkList[cnt].employeeName = $"{item.Employee.Department.departmentName}{item.Employee.Position.positionName} {item.Employee.Name}";
                cnt++;
            }

            return Json(checkList);
        }
    }
}
