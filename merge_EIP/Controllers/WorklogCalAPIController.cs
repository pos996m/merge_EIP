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
    public class WorklogCalAPIController : ApiController
    {
        FormModelEntities db = new FormModelEntities();

        // GET: api/WorklogCalAPI
        public JsonResult<List<CalData>> Get(string EID, string DepID)
        {
            List<CalData> calDatas = new List<CalData>();

            // 先撈留言板
            var MsgAll = db.messageBoard.Where(x => x.State == "所有人" || x.assignDepartment == DepID || x.assignPerson == EID || x.employeeID == EID)
                .OrderByDescending(x => x.goTop).ThenByDescending(x => x.messageDate).ToList();
            // 限制只能有打勾的觀看
            MsgAll = MsgAll.Where(x => x.toCalendar).ToList();
            foreach (messageBoard item in MsgAll)
            {
                calDatas.Add(new CalData
                {
                    num = item.messageboardNumber,
                    title = item.messageTitle + " (留言板)",
                    start = Convert.ToDateTime(item.toMsgDate).ToString("yyyy-MM-ddThh:mm"),
                    color = "rgb(65 139 202/1)",
                    state = "留言板"
                });
            }

            // 再撈待辦事項
            // 如果狀態是打勾就不顯示到行事曆
            var toDoListAll = db.Backlog.Where(x => x.employeeID == EID && x.checkState == false).ToList();
            foreach (Backlog item in toDoListAll)
            {
                string timeSpan = "";
                if (item.backlogTime != null)
                {
                    string x = item.backlogTime.ToString();
                    timeSpan = "T" + TimeSpan.Parse(x).ToString(@"hh\:mm");
                }

                calDatas.Add(new CalData
                {
                    num = item.backlogNumber,
                    title = item.backlogTxet +" (待辦事項)",
                    start = Convert.ToDateTime(item.backlogDate).ToString("yyyy-MM-dd") + timeSpan,
                    color = "rgb(217 84 79/1)",
                    state = "待辦事項"
                });
            }

            // 取得請假
            var dayOffAll = db.punchIn.Where(x => x.State == "請假" && x.Employee.departmentID == DepID).ToList();
            foreach (punchIn item in dayOffAll)
            {
                calDatas.Add(new CalData
                {
                    title = $"{item.Employee.Name} 請假",
                    start = Convert.ToDateTime(item.punchinDate).ToString("yyyy-MM-dd"),
                    color = "rgb(239 172 79/1)",
                    state = "請假"
                });
            }


            return Json(calDatas);
        }
    }
}
