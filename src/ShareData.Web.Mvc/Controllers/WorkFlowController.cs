using Castle.MicroKernel.Registration;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using ShareData.Controllers;
using ShareData.Roles;
using ShareData.Roles.Dto;
using ShareData.WorkFlowManagemet;
using ShareData.WorkFlowManagemet.Dto;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.Web.Controllers
{
    public class WorkFlowController : ShareDataControllerBase
    {
        private readonly IWorkFlowAppService _workFlowAppService;
        private readonly IRoleAppService _roleAppService;
        public WorkFlowController(IWorkFlowAppService workFlowAppService, IRoleAppService roleAppService)
        {
            _workFlowAppService = workFlowAppService;
            _roleAppService = roleAppService;
        }
        public async Task<IActionResult> Index()
        {
            var workFlow = _workFlowAppService.Get();
            GetRolesInput input = new();
            var userRoles = await _roleAppService.GetRolesAsync(input);
            ViewData["UserRoles"] = new SelectList(userRoles.Items, "Id", "Name");
            //return View();
            if (workFlow == null)
            {
                // Display under construction message
                return View("_UnderConstruction");
            }
            var root = workFlow.WorkFlowStages.Select(st => new WorkFlowStageShortInfoDto
            {
                WorkFlowId = st.WorkFlowId,
                WorkFlowName = st.WorkFlow.Name,
                WorkFlowDescription = st.WorkFlow.Description,
                WorkFlowStageId = st.Id,
                WorkFlowStageName = st.Name,
                HasAcceptNextStep = st.HasAcceptNextStep,
                HasRejectNextStep = st.HasRejectNextStep
            }).FirstOrDefault();

            string stages = DrawStages(root);
            return View("_DrawStages", stages);
        }
        [HttpPost]
        public async Task<JsonResult> Create([FromBody] CreateWorkFlowInput input)
        {
            return Json(await _workFlowAppService.Create(input));
        }
        [HttpPost]
        public async Task<JsonResult> Update([FromBody] UpdateWorkFlowInput input)
        {
            return Json(await _workFlowAppService.Update(input));
        }
        [HttpPost]
        public async Task<JsonResult> CreateStage([FromBody] CreateWorkFlowStageInput input)
        {
            return Json(await _workFlowAppService.CreateStage(input));
        }
        [HttpPost]
        public async Task<JsonResult> UpdateStage([FromBody] UpdateWorkFlowStageInput input)
        {
            return Json(await _workFlowAppService.UpdateStage(input));
        }
        [HttpGet]
        public async Task<ActionResult> EditWorkFlowModal()
        {
            var workflow = _workFlowAppService.Get();
            return PartialView("_EditWorkFlowModal", workflow);
        }
        [HttpGet]
        public async Task<ActionResult> EditWorkFlowStageModal(int id)
        {
            GetRolesInput input = new();
            var userRoles = await _roleAppService.GetRolesAsync(input);
            var stage = await _workFlowAppService.GetStage(id);
            ViewData["UserRoles"] = new SelectList(userRoles.Items, "Id", "Name", stage.RoleId);
            return PartialView("_EditStageModal", stage);
        }

        private string DrawStages(WorkFlowStageShortInfoDto root)
        {

            StringBuilder htmlBuilder = new();

            // Draw the HTML for the current workflow stage
            if (root is null)
            {
                htmlBuilder.AppendLine($"<a onclick='createWorkFlowStageModal({root.WorkFlowId},null,null)' class='btn btn-success bg-light-success mb-1'><i class='fa fa-plus-square'></i> {@L("Create")} </a>");
                htmlBuilder.AppendLine($"<header><h2 class='text-center'> {root.WorkFlowName} <a onclick='editWorkFlow()' class='btn btn-warning bg-light-warning btn-icon round mr-1 mb-1'><i class='fa fa-edit'></i></a></h2></header>");
                htmlBuilder.AppendLine("<div class='d-flex justify-content-between pb-2'>");
                htmlBuilder.AppendLine($"<h6 class='page-header'>{root.WorkFlowDescription}</h6>");
                htmlBuilder.AppendLine("</div>");
            }
            else
            {
                var children1 = _workFlowAppService.GetChildren(root.WorkFlowStageId);

                htmlBuilder.AppendLine("<div class='row'>");
                htmlBuilder.AppendLine("<div class='col-xs-12'>");
                htmlBuilder.AppendLine($"<a href='#' class='btn lead text-center bg-light-info text-info center-block w-100' onclick='editStage({root.WorkFlowStageId})'>{root.WorkFlowStageName}</a>");
                htmlBuilder.AppendLine("<div class='row'>");
                if (root.HasAcceptNextStep)
                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                if (root.HasRejectNextStep)
                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                htmlBuilder.AppendLine("</div>");
                htmlBuilder.AppendLine("</div>");
                htmlBuilder.AppendLine("</div>");

                htmlBuilder.AppendLine("<div class='row'>");
                if (root.HasAcceptNextStep)
                {
                    var child1 = children1.FirstOrDefault(x => x.LevelStatus == Common.LevelStatus.Accept);
                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'>");
                    if (child1 != null)
                    {
                        htmlBuilder.AppendLine($"<p class='center-block'><span class='btn btn-success btn-sm'>{@L("WorkFlow.Accept")}</span></p>");
                        htmlBuilder.AppendLine("<p class='arrow center-block'><span class='fa fa-arrow-down'></span></p>");
                        htmlBuilder.AppendLine($"<a href='#' class='btn bg-light-success text-success step' onclick='editStage({child1.WorkFlowStageId})'>{child1.WorkFlowStageName}</a>");
                        htmlBuilder.AppendLine("<div class='row'>");
                        if (child1.HasAcceptNextStep)
                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                        if (child1.HasRejectNextStep)
                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                        htmlBuilder.AppendLine("</div>");
                        htmlBuilder.AppendLine("<div class='row'>");
                        var children2 = _workFlowAppService.GetChildren(child1.WorkFlowStageId);
                        if (child1.HasAcceptNextStep)
                        {
                            var child2 = children2.FirstOrDefault(x => x.LevelStatus == Common.LevelStatus.Accept);
                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'>");
                            if (child2 != null)
                            {
                                htmlBuilder.AppendLine($"<p class='center-block'><span class='btn btn-success btn-sm'>{@L("WorkFlow.Accept")}</span></p>");
                                htmlBuilder.AppendLine("<p class='arrow center-block'><span class='fa fa-arrow-down'></span></p>");
                                htmlBuilder.AppendLine($"<a href='#' class='btn bg-light-success text-success step' onclick='editStage({child2.WorkFlowStageId})'>{child2.WorkFlowStageName}</a>");
                                htmlBuilder.AppendLine("<div class='row'>");
                                if (child2.HasAcceptNextStep)
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                if (child2.HasRejectNextStep)
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                htmlBuilder.AppendLine("</div>");
                                htmlBuilder.AppendLine("<div class='row'>");
                                var children3 = _workFlowAppService.GetChildren(child2.WorkFlowStageId);
                                if (child2.HasAcceptNextStep)
                                {
                                    var child3 = children3.FirstOrDefault(x => x.LevelStatus == Common.LevelStatus.Accept);
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'>");
                                    if (child3 != null)
                                    {
                                        htmlBuilder.AppendLine($"<p class='center-block'><span class='btn btn-success btn-sm'>{@L("WorkFlow.Accept")}</span></p>");
                                        htmlBuilder.AppendLine("<p class='arrow center-block'><span class='fa fa-arrow-down'></span></p>");
                                        htmlBuilder.AppendLine($"<a href='#' class='btn bg-light-success text-success step' onclick='editStage({child3.WorkFlowStageId})'>{child3.WorkFlowStageName}</a>");
                                        htmlBuilder.AppendLine("<div class='row'>");
                                        if (child3.HasAcceptNextStep)
                                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                        if (child3.HasRejectNextStep)
                                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                        htmlBuilder.AppendLine("</div>");
                                        htmlBuilder.AppendLine("<div class='row'>");

                                        htmlBuilder.AppendLine("</div>");
                                    }
                                    else
                                        htmlBuilder.AppendLine($"<p class='center-block'><a href='#' onclick='createWorkFlowStageModal({root.WorkFlowId},{child2.WorkFlowStageId},1)' class='btn btn-success btn-sm'>{@L("WorkFlow.Accept")}</a></p>");
                                    htmlBuilder.AppendLine("</div>");
                                }
                                if (child2.HasRejectNextStep)
                                {
                                    var child3 = children3.FirstOrDefault(x => x.LevelStatus == Common.LevelStatus.Reject);
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'>");
                                    if (child3 != null)
                                    {
                                        htmlBuilder.AppendLine($"<p class='center-block'><span class='btn btn-danger btn-sm'>{@L("WorkFlow.Reject")}</span></p>");
                                        htmlBuilder.AppendLine("<p class='arrow center-block'><span class='fa fa-arrow-down'></span></p>");
                                        htmlBuilder.AppendLine($"<a href='#' class='btn bg-light-danger text-danger step' onclick='editStage({child3.WorkFlowStageId})'>{child3.WorkFlowStageName}</a>");
                                        htmlBuilder.AppendLine("<div class='row'>");
                                        if (child3.HasAcceptNextStep)
                                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                        if (child3.HasRejectNextStep)
                                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                        htmlBuilder.AppendLine("</div>");
                                    }
                                    else
                                        htmlBuilder.AppendLine($"<p class='center-block'><a href='#' onclick='createWorkFlowStageModal({root.WorkFlowId},{child2.WorkFlowStageId},0)' class='btn btn-danger btn-sm'>{@L("WorkFlow.Reject")}</a></p>");
                                    htmlBuilder.AppendLine("</div>");
                                }
                                htmlBuilder.AppendLine("</div>");
                            }
                            else
                                htmlBuilder.AppendLine($"<p class='center-block'><a href='#' onclick='createWorkFlowStageModal({root.WorkFlowId},{child1.WorkFlowStageId},1)' class='btn btn-success btn-sm'>{@L("WorkFlow.Accept")}</a></p>");
                            htmlBuilder.AppendLine("</div>");
                        }
                        if (child1.HasRejectNextStep)
                        {
                            var child2 = children2.FirstOrDefault(x => x.LevelStatus == Common.LevelStatus.Reject);
                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'>");
                            if (child2 != null)
                            {
                                htmlBuilder.AppendLine($"<p class='center-block'><span class='btn btn-danger btn-sm'>{@L("WorkFlow.Reject")}</span></p>");
                                htmlBuilder.AppendLine("<p class='arrow center-block'><span class='fa fa-arrow-down'></span></p>");
                                htmlBuilder.AppendLine($"<a href='#' class='btn bg-light-danger text-danger step' onclick='editStage({child2.WorkFlowStageId})'>{child2.WorkFlowStageName}</a>");
                                htmlBuilder.AppendLine("<div class='row'>");
                                if (child2.HasAcceptNextStep)
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                if (child2.HasRejectNextStep)
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                htmlBuilder.AppendLine("</div>");
                                htmlBuilder.AppendLine("<div class='row'>");
                                htmlBuilder.AppendLine("<div class='row'>");
                                var children3 = _workFlowAppService.GetChildren(child2.WorkFlowStageId);
                                if (child2.HasAcceptNextStep)
                                {
                                    var child3 = children3.FirstOrDefault(x => x.LevelStatus == Common.LevelStatus.Accept);
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'>");
                                    if (child3 != null)
                                    {
                                        htmlBuilder.AppendLine($"<p class='center-block'><span class='btn btn-success btn-sm'>{@L("WorkFlow.Accept")}</span></p>");
                                        htmlBuilder.AppendLine("<p class='arrow center-block'><span class='fa fa-arrow-down'></span></p>");
                                        htmlBuilder.AppendLine($"<a href='#' class='btn bg-light-success text-success step' onclick='editStage({child3.WorkFlowStageId})'>{child3.WorkFlowStageName}</a>");
                                        htmlBuilder.AppendLine("<div class='row'>");
                                        if (child3.HasAcceptNextStep)
                                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                        if (child3.HasRejectNextStep)
                                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                        htmlBuilder.AppendLine("</div>");
                                        htmlBuilder.AppendLine("<div class='row'>");

                                        htmlBuilder.AppendLine("</div>");
                                    }
                                    else
                                        htmlBuilder.AppendLine($"<p class='center-block'><a href='#' onclick='createWorkFlowStageModal({root.WorkFlowId},{child2.WorkFlowStageId},1)' class='btn btn-success btn-sm'>{@L("WorkFlow.Accept")}</a></p>");
                                    htmlBuilder.AppendLine("</div>");
                                }
                                if (child2.HasRejectNextStep)
                                {
                                    var child3 = children3.FirstOrDefault(x => x.LevelStatus == Common.LevelStatus.Reject);
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'>");
                                    if (child3 != null)
                                    {
                                        htmlBuilder.AppendLine($"<p class='center-block'><span class='btn btn-danger btn-sm'>{@L("WorkFlow.Reject")}</span></p>");
                                        htmlBuilder.AppendLine("<p class='arrow center-block'><span class='fa fa-arrow-down'></span></p>");
                                        htmlBuilder.AppendLine($"<a href='#' class='btn bg-light-danger text-danger step' onclick='editStage({child3.WorkFlowStageId})'>{child3.WorkFlowStageName}</a>");
                                        htmlBuilder.AppendLine("<div class='row'>");
                                        if (child3.HasAcceptNextStep)
                                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                        if (child3.HasRejectNextStep)
                                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                        htmlBuilder.AppendLine("</div>");
                                    }
                                    else
                                        htmlBuilder.AppendLine($"<p class='center-block'><a href='#' onclick='createWorkFlowStageModal({root.WorkFlowId},{child2.WorkFlowStageId},0)' class='btn btn-danger btn-sm'>{@L("WorkFlow.Reject")}</a></p>");
                                    htmlBuilder.AppendLine("</div>");
                                }
                                htmlBuilder.AppendLine("</div>");
                                htmlBuilder.AppendLine("</div>");
                            }
                            else
                                htmlBuilder.AppendLine($"<p class='center-block'><a href='#' onclick='createWorkFlowStageModal({root.WorkFlowId},{child1.WorkFlowStageId},0)' class='btn btn-danger btn-sm'>{@L("WorkFlow.Reject")}</a></p>");
                            htmlBuilder.AppendLine("</div>");
                        }
                        htmlBuilder.AppendLine("</div>");
                    }
                    else
                        htmlBuilder.AppendLine($"<p class='center-block'><a href='#' onclick='createWorkFlowStageModal({root.WorkFlowId},{child1.WorkFlowStageId},1)' class='btn btn-success btn-sm'>{@L("WorkFlow.Accept")}</a></p>");
                    htmlBuilder.AppendLine("</div>");
                }
                if (root.HasRejectNextStep)
                {
                    var child1 = children1.FirstOrDefault(x => x.LevelStatus == Common.LevelStatus.Reject);
                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'>");
                    if (child1 != null)
                    {
                        htmlBuilder.AppendLine($"<p class='center-block'><span class='btn btn-danger btn-sm'>{@L("WorkFlow.Reject")}</span></p>");
                        htmlBuilder.AppendLine("<p class='arrow center-block'><span class='fa fa-arrow-down'></span></p>");
                        htmlBuilder.AppendLine($"<a href='#' class='btn bg-light-danger text-danger step' onclick='editStage({child1.WorkFlowStageId})'>{child1.WorkFlowStageName}</a>");
                        htmlBuilder.AppendLine("<div class='row'>");
                        if (child1.HasAcceptNextStep)
                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                        if (child1.HasRejectNextStep)
                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                        htmlBuilder.AppendLine("</div>");
                        htmlBuilder.AppendLine("<div class='row'>");
                        var children2 = _workFlowAppService.GetChildren(child1.WorkFlowStageId);
                        if (child1.HasAcceptNextStep)
                        {
                            var child2 = children2.FirstOrDefault(x => x.LevelStatus == Common.LevelStatus.Accept);
                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'>");
                            if (child2 != null)
                            {
                                htmlBuilder.AppendLine($"<p class='center-block'><span class='btn btn-success btn-sm'>{@L("WorkFlow.Accept")}</span></p>");
                                htmlBuilder.AppendLine("<p class='arrow center-block'><span class='fa fa-arrow-down'></span></p>");
                                htmlBuilder.AppendLine($"<a href='#' class='btn bg-light-success text-success step' onclick='editStage({child2.WorkFlowStageId})'>{child2.WorkFlowStageName}</a>");
                                htmlBuilder.AppendLine("<div class='row'>");
                                if (child2.HasAcceptNextStep)
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                if (child2.HasRejectNextStep)
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                htmlBuilder.AppendLine("</div>");
                                htmlBuilder.AppendLine("<div class='row'>");

                                htmlBuilder.AppendLine("</div>");
                            }
                            else
                                htmlBuilder.AppendLine($"<p class='center-block'><a href='#' onclick='createWorkFlowStageModal({root.WorkFlowId},{child1.WorkFlowStageId},1)' class='btn btn-success btn-sm'>{@L("WorkFlow.Accept")}</a></p>");
                            htmlBuilder.AppendLine("</div>");
                        }
                        if (child1.HasRejectNextStep)
                        {
                            var child2 = children2.FirstOrDefault(x => x.LevelStatus == Common.LevelStatus.Reject);
                            htmlBuilder.AppendLine("<div class='col-xs-6 text-center'>");
                            if (child2 != null)
                            {
                                htmlBuilder.AppendLine($"<p class='center-block'><span class='btn btn-danger btn-sm'>{@L("WorkFlow.Reject")}</span></p>");
                                htmlBuilder.AppendLine("<p class='arrow center-block'><span class='fa fa-arrow-down'></span></p>");
                                htmlBuilder.AppendLine($"<a href='#' class='btn bg-light-danger text-danger step' onclick='editStage({child2.WorkFlowStageId})'>{child2.WorkFlowStageName}</a>");
                                htmlBuilder.AppendLine("<div class='row'>");
                                if (child2.HasAcceptNextStep)
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                if (child2.HasRejectNextStep)
                                    htmlBuilder.AppendLine("<div class='col-xs-6 text-center'><p class='arrow'><span class='fa fa-arrow-down'></span></p></div>");
                                htmlBuilder.AppendLine("</div>");
                            }
                            else
                                htmlBuilder.AppendLine($"<p class='center-block'><a href='#' onclick='createWorkFlowStageModal({root.WorkFlowId},{child1.WorkFlowStageId},0)' class='btn btn-danger btn-sm'>{@L("WorkFlow.Reject")}</a></p>");
                            htmlBuilder.AppendLine("</div>");
                        }
                        htmlBuilder.AppendLine("</div>");
                    }
                    else
                        htmlBuilder.AppendLine($"<p class='center-block'><a href='#' onclick='createWorkFlowStageModal({root.WorkFlowId},{child1.WorkFlowStageId},0)' class='btn btn-danger btn-sm'>{@L("WorkFlow.Reject")}</a></p>");
                    htmlBuilder.AppendLine("</div>");
                }

                htmlBuilder.AppendLine("</div>");
                htmlBuilder.AppendLine("</div></div>");
            }

            return htmlBuilder.ToString();
        }

    }
}
