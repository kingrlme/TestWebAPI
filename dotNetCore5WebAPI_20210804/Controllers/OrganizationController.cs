using dotNetCore5WebAPI_0323.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCore5WebAPI_0323.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;
        public OrganizationController(ILogger<OrganizationController> logger)
        {
            _logger = logger;
        }

        // Emma Connect to SQL SERVER and get DataTables
        readonly DataAccessService _DataAccess = new();

        /// <summary>
        /// 取得所有組織資料
        /// </summary>
        /// <remarks>
        /// 備註
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<Organization> GetAllOrganization()
        {
            List<Organization> AllOrg = _DataAccess.GetAllOrganization();
            if (AllOrg != null) return AllOrg.ToArray();
            return new List<Organization> { };
        }

        /// <summary>
        /// 取得特定組織資料
        /// </summary>
        /// <param name="SID">欲查詢之組織 SID</param>
        /// <returns></returns>
        [HttpGet("{SID}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Organization> GetOneOrgByOrgID(string SID)
        {
            if (SID == null) return BadRequest();
            List<Organization> OneOrg = _DataAccess.GetOneOrgByOrgID(SID);
            if (OneOrg == null || OneOrg.Count == 0)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetOneOrgByOrgID), new { id = OneOrg[0].SID }, OneOrg);
        }

        /// <summary>
        /// 新增一個組織
        /// </summary>
        /// <param name="NewOrg"></param>
        /// <returns>新增的組織</returns>
        /// <response code="204">新增成功</response>
        /// <response code="400">已存在相同 SID 之資料</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateNewUser(List<Organization> NewOrg)
        {
            if (NewOrg == null || NewOrg.Count == 0)
            {
                return NotFound();
            }
            int InsertStatus = _DataAccess.InsertOrgData(NewOrg);
            switch (InsertStatus)
            {
                case 0:
                    {
                        return NotFound();  // 沒有新增資料
                    }
                case 1:
                    {
                        return NoContent(); // 已新增一筆進資料庫。
                    }
                case 2:
                    {
                        return BadRequest();  //已存在相同 UID 之資料
                    }
                default:
                    {
                        return NotFound();
                    }
            }
        }

        /// <summary>
        /// 更新組織
        /// </summary>
        /// <param name="SID">欲變更之組織SID</param>
        /// <param name="OneOrg">新的組織資料</param>
        /// <returns></returns>
        [HttpPut("{SID}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateOneOrg(string SID, List<Organization> OneOrg)
        {
            if (SID == null || !SID.Equals(OneOrg[0].SID.ToString()) || OneOrg == null)
            {
                return BadRequest();
            }
            int UpdateStatus = _DataAccess.UpdateOrganization(SID, OneOrg);
            switch (UpdateStatus)
            {
                case 0:
                    {
                        return NotFound();  // 沒有更新資料
                    }
                case 1:
                    {
                        return NoContent(); // 已更新資料
                    }
                case 2:
                    {
                        return NotFound();  // 找不到欲更新之組織資料
                    }
                default:
                    {
                        return NoContent();
                    }
            }
        }


        /// <summary>
        /// 刪除一筆組織資料
        /// </summary>
        /// <param name="SID">欲刪除之組織 SID</param>
        /// <returns></returns>
        [HttpDelete("{SID}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteOneUserByUID(string SID)
        {
            if (SID == null) return BadRequest();
            int DelteStatus = _DataAccess.DeleteOrganizationBySID(SID);
            switch (DelteStatus)
            {
                case 0:
                    {
                        return NotFound();  //刪除失敗
                    }
                case 1:
                    {
                        return NoContent(); //刪除成功
                    }
                case 2:
                    {
                        return NotFound();  //找不到符合的資料
                    }
                default:
                    {
                        return NotFound();
                    }
            }
        }





    }
}
