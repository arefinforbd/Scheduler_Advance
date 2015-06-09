using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ServicePROWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicePROWeb.Controllers
{
    public class SchedulerController : Controller
    {
        public ActionResult Job_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<TaskViewModel> tasks = new List<TaskViewModel>
            {
                new TaskViewModel {
                    TaskID = 101,
                    Title = "Title 101",
                    Start = new DateTime(2015, 5, 28, 2, 00, 00),
                    End = new DateTime(2015, 5, 28, 2, 30, 00),
                    Description = "Description 101",
                    IsAllDay = false,
                    //RecurrenceRule = "Rule 101",
                    //RecurrenceException = "Exception 101",
                    //RecurrenceID = 1001,
                    OwnerID = 10001
                },
                new TaskViewModel {
                    TaskID = 102,
                    Title = "Title 102",
                    Start = new DateTime(2015, 5, 28, 3, 00, 00),
                    End = new DateTime(2015, 5, 28, 3, 30, 00),
                    Description = "Description 102",
                    IsAllDay = false,
                    //RecurrenceRule = "Rule 102",
                    //RecurrenceException = "Exception 102",
                    //RecurrenceID = 1002,
                    OwnerID = 10002
                },
                new TaskViewModel {
                    TaskID = 103,
                    Title = "Title 103",
                    Start = new DateTime(2015, 5, 28, 4, 00, 00),
                    End = new DateTime(2015, 5, 28, 4, 30, 00),
                    Description = "Description 103",
                    IsAllDay = false,
                    //RecurrenceRule = "Rule 103",
                    //RecurrenceException = "Exception 103",
                    //RecurrenceID = 1003,
                    OwnerID = 10003
                }
            };

            //var taskList = tasks.AsQueryable();

            return Json(tasks.ToDataSourceResult(request));
        }

        public ActionResult Job_Create([DataSourceRequest] DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                //taskService.Insert(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Job_Update([DataSourceRequest]DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                //taskService.Update(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Job_Delete([DataSourceRequest]DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                //taskService.Delete(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Resource_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<TaskViewModel> meetings = new List<TaskViewModel>
            {
                new TaskViewModel {
                    TaskID = 101,
                    TechID = 1,
                    MeetingID = 1001,
                    Title = "Title 101",
                    Start = new DateTime(2015, 5, 28, 2, 00, 00),
                    End = new DateTime(2015, 5, 28, 2, 30, 00),
                    Description = "Description 101",
                    IsAllDay = false,
                    //RecurrenceRule = "Rule 101",
                    //RecurrenceException = "Exception 101",
                    //RecurrenceID = 1001,
                    OwnerID = 10001
                },
                new TaskViewModel {
                    TaskID = 102,
                    TechID = 1,
                    MeetingID = 1002,
                    Title = "Title 102",
                    Start = new DateTime(2015, 5, 28, 3, 00, 00),
                    End = new DateTime(2015, 5, 28, 3, 30, 00),
                    Description = "Description 102",
                    IsAllDay = false,
                    //RecurrenceRule = "Rule 102",
                    //RecurrenceException = "Exception 102",
                    //RecurrenceID = 1002,
                    OwnerID = 10002
                },
                new TaskViewModel {
                    TaskID = 103,
                    TechID = 2,
                    MeetingID = 1003,
                    Title = "Title 103",
                    Start = new DateTime(2015, 5, 28, 4, 00, 00),
                    End = new DateTime(2015, 5, 28, 4, 30, 00),
                    Description = "Description 103",
                    IsAllDay = false,
                    //RecurrenceRule = "Rule 103",
                    //RecurrenceException = "Exception 103",
                    //RecurrenceID = 1003,
                    OwnerID = 10003
                }
            };

            return Json(meetings.ToDataSourceResult(request));
        }

        public ActionResult Resource_Create([DataSourceRequest] DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                //taskService.Insert(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Resource_Update([DataSourceRequest]DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                //taskService.Update(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Resource_Delete([DataSourceRequest]DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                //taskService.Delete(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Techs()
        {
            List<Tech> techs = new List<Tech>
            {
                new Tech
                {
                    TechName = "Jack",
                    TechID = 1,
                    Color = "#6eb3fa"
                },
                new Tech
                {
                    TechName = "Lochlan",
                    TechID = 2,
                    Color = "#f58a8a"
                },
                new Tech
                {
                    TechName = "Arefin",
                    TechID = 3,
                    Color = "#0583fa"
                },
                new Tech
                {
                    TechName = "Faisal",
                    TechID = 4,
                    Color = "#9635a0"
                }
            };

            return Json(techs, JsonRequestBehavior.AllowGet);
        }
    
        public ActionResult Skills()
        {
            List<Skill> skills = new List<Skill>
            {
                new Skill
                {
                    SkillID = 1,
                    SkillName = "ERP"
                },
                new Skill
                {
                    SkillID = 2,
                    SkillName = "CPR"
                }
            };

            return Json(skills, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Vertical_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<Tech> techs1 = new List<Tech>
            {
                new Tech { TechID = 1 },
                new Tech { TechID = 4 }
            };

            List<Tech> techs2 = new List<Tech>
            {
                new Tech { TechID = 2 },
                new Tech { TechID = 3 }
            };

            List<TaskViewModel> tasks = new List<TaskViewModel>
            {
                new TaskViewModel {
                    TaskID = 1,
                    SkillID = 1,
                    TechID = 2,
                    Title = "Job 101",
                    Start = new DateTime(2015, 5, 28, 00, 00, 00),
                    End = new DateTime(2015, 5, 28, 00, 30, 00),
                    Description = "Description 101",
                    IsAllDay = false
                },
                new TaskViewModel {
                    TaskID = 2,
                    SkillID = 2,
                    TechID = 1,
                    Title = "Job 102",
                    Start = new DateTime(2015, 5, 28, 1, 00, 00),
                    End = new DateTime(2015, 5, 28, 1, 30, 00),
                    Description = "Description 102",
                    IsAllDay = false
                },
                new TaskViewModel {
                    TaskID = 1,
                    SkillID = 1,
                    TechID = 3,
                    Title = "Job 103",
                    Start = new DateTime(2015, 5, 28, 00, 00, 00),
                    End = new DateTime(2015, 5, 28, 00, 30, 00),
                    Description = "Description 103",
                    IsAllDay = false
                },
                new TaskViewModel {
                    TaskID = 2,
                    SkillID = 2,
                    TechID = 4,
                    Title = "Job 104",
                    Start = new DateTime(2015, 5, 25, 1, 00, 00),
                    End = new DateTime(2015, 5, 25, 1, 30, 00),
                    Description = "Description 104",
                    IsAllDay = true
                }
            };

            return Json(tasks.ToDataSourceResult(request));
        }

        public ActionResult Vertical_Create([DataSourceRequest] DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                //taskService.Insert(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Vertical_Update([DataSourceRequest]DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                //taskService.Update(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Vertical_Delete([DataSourceRequest]DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                //taskService.Delete(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }
    }
}