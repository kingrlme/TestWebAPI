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
    public class UserDataController : ControllerBase
    {
        private readonly ILogger<UserDataController> _logger;
        public UserDataController(ILogger<UserDataController> logger)
        {
            _logger = logger;
        }

        // Emma Connect to SQL SERVER and get DataTables
        readonly DataAccessService _DataAccess = new();

        /// <summary>
        /// 取得所有User資料
        /// </summary>
        /// <remarks>
        /// 備註
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<UserData> GetAllUsers()
        {
            List<UserData> AllUsers = _DataAccess.GetAllUsers();
            if (AllUsers != null) return AllUsers.ToArray();
            return new List<UserData> { };
        }

        /// <summary>
        /// 取得特定User資料
        /// </summary>
        /// <param name="UID">欲查詢之使用者ID</param>
        /// <returns></returns>
        [HttpGet("{UID}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserData> GetOneUserByUID(string UID)
        {
            if (UID == null) return BadRequest();
            List<UserData> OneUser = _DataAccess.GetUserData(UID);
            if (OneUser == null || OneUser.Count == 0)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetOneUserByUID), new { id = OneUser[0].UID }, OneUser);
        }

        /// <summary>
        /// 新增一個使用者
        /// </summary>
        /// <param name="NewUser"></param>
        /// <returns>新增的使用者</returns>
        /// <response code="204">新增成功</response>
        /// <response code="400">已存在相同 UID 之資料</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateNewUser(List<UserData> NewUser)
        { 
            if (NewUser == null || NewUser.Count == 0)
            {
                return NotFound();
            }
            int InsertStatus = _DataAccess.InsertUserData(NewUser);
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
        /// 更新使用者
        /// </summary>
        /// <param name="UID">欲變更之使用者ID</param>
        /// <param name="OneUser">新的使用者資料</param>
        /// <returns></returns>
        [HttpPut("{UID}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateOneUser(string UID, List<UserData> OneUser)
        {
            if (UID == null || !UID.Equals(OneUser[0].UID) || OneUser == null)
            {
                return BadRequest();
            }
            int UpdateStatus = _DataAccess.UpdateUserData(UID, OneUser);
            switch (UpdateStatus)
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
                        return NotFound();  //已存在相同 UID 之資料
                    }
                default:
                    {
                        return NoContent();
                    }
            }
        }


        /// <summary>
        /// 刪除一筆使用者
        /// </summary>
        /// <param name="UID">欲刪除之使用者ID</param>
        /// <returns></returns>
        [HttpDelete("{UID}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteOneUserByUID(string UID)
        {
            if (UID == null) return BadRequest();
            int DelteStatus = _DataAccess.DeleteUserData(UID);
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
                        return NotFound();  //找不到符合的使用者資料
                    }
                default:
                    {
                        return NotFound();
                    }
            }
        }
        




    }
}
