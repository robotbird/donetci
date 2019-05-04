using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utry.Core.Domain;
using Utry.CI.Models;
using Utry.Framework.Web;
using Utry.Framework.Utils;
using Utry.Core.Services;

namespace Utry.CI.Controllers
{
    public class ReportController : BaseController
    {
        ReportService _reportService = new ReportService();

        public ActionResult ReportList(ReportModel model)
        {
            const int pageSize = 15;
            int count = 0;
            int pageIndex = PressRequest.GetInt("page", 1);
            var where = "";
            if (!string.IsNullOrEmpty(model.ReportName) && Utils.IsSafeSqlString(model.ReportName))
            {
                where = " and r.ReportName like '%" + model.ReportName + "%'";
            }
            var list = _reportService.GetReportList(pageSize, pageIndex, out count, where);
            model.Pagelist.LoadPagedList(list);
            model.ReportList = (List<CIReport>)list;
            return View(model);
        }

        [HttpGet]
        public ActionResult ReportAdd(string id)
        {
            ReportModel model = new ReportModel();
            if (id != null)
            {
                model.Report = _reportService.GetById(id);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReportAdd(CIReport report)
        {
            ReportModel model = new ReportModel();
            model.Report = report;
            if (string.IsNullOrEmpty(report.ID))
            {
                report.ID = Guid.NewGuid().ToString();
                report.AddTime = DateTime.Now;
                int num = _reportService.InsertReport(report);
                if (num > 0)
                {
                    return Redirect(Url.Action("ReportList", "Report"));
                }
                else
                {
                    ErrorNotification("添加失败");
                    return View(model);
                }
            }
            else
            {
                int num = _reportService.UpdateReport(report);
                if (num > 0)
                {
                    return Redirect(Url.Action("ReportList", "Report"));
                }
                else
                {
                    ErrorNotification("修改失败");
                    return View(model);
                }
            }
        }


        public ActionResult DeleteReport(string id)
        {
            int num = _reportService.DeleteReport(id);
            if (num > 0)
            {
                SuccessNotification("删除成功");
                return Redirect(Url.Action("ReportList", "Report"));
            }
            else
            {
                ErrorNotification("删除失败");
                return Redirect(Url.Action("ReportList", "Report"));
            }
        }


        public ActionResult ReportInfo(string id)
        {
            ReportModel model = new ReportModel();
            model.Report = _reportService.GetById(id);
            var SQL = model.Report.ReportSQL;
            if (!string.IsNullOrEmpty(PressRequest.GetFormString("BeginDate")))
            {
                SQL = SQL.Replace("@BeginDate", "'" + PressRequest.GetFormString("BeginDate") + "'");
                model.BeginDate = PressRequest.GetFormString("BeginDate");
            }
            else
            {
                SQL = SQL.Replace("@BeginDate", "''");
            }

            if (!string.IsNullOrEmpty(PressRequest.GetFormString("EndDate")))
            {
                SQL = SQL.Replace("@EndDate", "'" + PressRequest.GetFormString("EndDate") + "'");
                model.EndDate = PressRequest.GetFormString("EndDate");
            }
            else
            {
                SQL = SQL.Replace("@EndDate", "'"+ DateTime.Now +"'");
            }
            model.dtReport = _reportService.GetReportResultBySql(SQL);
            return View(model);
        }

        [HttpPost]
        public ActionResult ExportToExcel(string id)
        {
            ReportModel model = new ReportModel();
            model.Report = _reportService.GetById(id);
            var SQL = model.Report.ReportSQL;
            if (!string.IsNullOrEmpty(PressRequest.GetFormString("BeginDate")))
            {
                SQL = SQL.Replace("@BeginDate", "'" + PressRequest.GetFormString("BeginDate") + "'");
                model.BeginDate = PressRequest.GetFormString("BeginDate");
            }
            else
            {
                SQL = SQL.Replace("@BeginDate", "''");
            }

            if (!string.IsNullOrEmpty(PressRequest.GetFormString("EndDate")))
            {
                SQL = SQL.Replace("@EndDate", "'" + PressRequest.GetFormString("EndDate") + "'");
                model.EndDate = PressRequest.GetFormString("EndDate");
            }
            else
            {
                SQL = SQL.Replace("@EndDate", "'" + DateTime.Now + "'");
            }
            model.dtReport = _reportService.GetReportResultBySql(SQL);
            ExcelHelper.ExportByWeb(model.dtReport, model.Report.ReportName, model.Report.ReportName + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".xls");
            SuccessNotification("导出excel成功");
            return Redirect(Url.Action("reportInfo", "report", new { id= model.Report.ID}));
        }


    }
}
