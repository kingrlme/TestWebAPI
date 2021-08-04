using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Model.DataAccessLayer;
using Dapper;
using System.Text;

namespace dotNetCore5WebAPI_0323.Configuration
{
    public class DataAccessService
    {
        private static readonly string MSSQL_STR = Utils.Configuration.Databases.MSSQL_STR;

        /// <summary>
        /// 連線字串(已改從appsettings.json 讀取資料庫字串)、此行可刪
        /// </summary>
        private static readonly string _connectionStr = "Data Source=localhost;" +
                                                        "Database=EmmaDB;" +
                                                        "user id=emma;" +
                                                        "password=emma";

        /// <summary>
        /// 全域的資料庫連線字串
        /// </summary>
        public static string ConnectionStr { get { return MSSQL_STR; } }

        /// <summary>
        /// 查詢所有 Organization
        /// </summary>
        /// <returns></returns>
        public List<Organization> GetAllOrganization()
        {
            List<Organization> Org = null;

            string sqlCommand = @"
                    SELECT  [SID]
                            ,[OrgID]
                            ,[OrgName]
                            ,[OrgStatus]
                            ,[OrgCreateYear]
                            ,[OrgCreateMonth]
                            ,[CreateTime]
                            ,[WGS84X]
                            ,[WGS84Y]
                        FROM [Organization]
                        ORDER BY CreateTime
                    ";
            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
                {
                    Org = conn.Query<Organization>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            return Org;
        }

        /// <summary>
        /// 查詢 Organization
        /// </summary>
        /// <param name="SID">SID</param>
        /// <returns></returns>
        public List<Organization> GetOneOrgByOrgID(string SID)
        {
            List<Organization> OneOrg = null;

            string sqlCommand = @"
                    SELECT  [SID]
                            ,[OrgID]
                            ,[OrgName]
                            ,[OrgStatus]
                            ,[OrgCreateYear]
                            ,[OrgCreateMonth]
                            ,[CreateTime]
                            ,[WGS84X]
                            ,[WGS84Y]
                        FROM [Organization]
                    WHERE 1=1 
                    ";

            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
                {
                    if (!SID.Equals(""))
                    {
                        sqlCommand += " AND [Organization].SID=@SID";
                        DynamicParameters parameters = new();
                        parameters.Add("SID", SID, DbType.String, ParameterDirection.Input, 50);
                        OneOrg = conn.Query<Organization>(sqlCommand, parameters, commandType: CommandType.Text).ToList();
                    }
                    else
                    {
                        OneOrg = conn.Query<Organization>(sqlCommand).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            return OneOrg;
        }

        /// <summary>
        /// 新增組織
        /// </summary>
        /// <param name="NewOrg"></param>
        /// <returns>0:新增失敗;1:新增成功;2.已存在相同UID之使用者</returns>
        public int InsertOrgData(List<Organization> NewOrg)
        {
            int count = 0;
            string sqlCommand;
            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
                {
                    // Check User is exist or not
                    sqlCommand = @"SELECT * FROM [Organization] WHERE [Organization].SID=@SID";
                    List<Organization> OneOrg = null;
                    OneOrg = conn.Query<Organization>(sqlCommand, new { NewOrg[0].SID }).ToList();
                    if (OneOrg == null || OneOrg.Count == 0)
                    {
                        sqlCommand = @"
                                    DECLARE @NEXT INT SELECT @NEXT = MAX([SID]) FROM [Organization]
                                    SET @NEXT = CASE WHEN @NEXT IS NULL THEN 1 ELSE @NEXT + 1 END 

                                   INSERT INTO [Organization]
                                               ([SID]
                                               ,[OrgID]
                                               ,[OrgName]
                                               ,[OrgStatus]
                                               ,[OrgCreateYear]
                                               ,[OrgCreateMonth]
                                               ,[CreateTime]
                                               ,[WGS84X]
                                               ,[WGS84Y]
                                                ,[Geom])
                                        VALUES
                                           (@NEXT
                                           ,@OrgID
                                           ,@OrgName
                                           ,@OrgStatus
                                           ,@OrgCreateYear
                                           ,@OrgCreateMonth
                                           ,convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),108),':','')
                                           ,@WGS84X
                                           ,@WGS84Y
                                           ,geometry::STGeomFromText('POINT(' + CAST(@WGS84X AS NVARCHAR(20)) + ' ' + CAST(CAST(@WGS84Y AS decimal(20, 8)) AS nvarchar) + ')', 0)
                                            )";
                        //新增多筆參數
                        count = conn.Execute(sqlCommand, new
                        {
                            NewOrg[0].SID,
                            NewOrg[0].OrgID,
                            NewOrg[0].OrgName,
                            NewOrg[0].OrgStatus,
                            NewOrg[0].OrgCreateYear,
                            NewOrg[0].OrgCreateMonth,
                            NewOrg[0].CreateTime,
                            NewOrg[0].WGS84X,
                            NewOrg[0].WGS84Y
                        });
                    }
                    else
                    {
                        count = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            return count;
        }


        /// <summary>
        /// Delete Organization By SID
        /// </summary>
        /// <param name="SID">Organization SID</param>
        /// <returns>0:刪除失敗;1:刪除成功;2.找不到該 SID 組織資料</returns>
        public int DeleteOrganizationBySID(string SID)
        {
            int count = 0;
            string sqlCommand;
            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
                {
                    // Check User is exist or not
                    sqlCommand = @"SELECT SID FROM [Organization] WHERE [Organization].SID=@SID";
                    List<Organization> OneOrg = null;
                    OneOrg = conn.Query<Organization>(sqlCommand, new { SID }).ToList();
                    if (OneOrg == null || OneOrg.Count == 0)
                    {
                        count = 2;
                    }
                    else
                    {
                        sqlCommand = @"DELETE FROM [Organization] WHERE [SID] = @SID";
                        count = conn.Execute(sqlCommand, new { SID });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            return count;
        }

        /// <summary>
        /// 更新 Organization
        /// </summary>
        /// <param name="SID">Organization SID</param>
        /// <param name="NewOrg">Update Date</param>
        /// <returns>0:更新失敗;1:已更新;2.找不到該SID組織資料</returns>
        public int UpdateOrganization(string SID, List<Organization> NewOrg)
        {
            int count = 0;
            string sqlCommand;
            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
                {
                    // Check is exist or not
                    sqlCommand = @"
                    SELECT  [SID]
                            ,[OrgID]
                            ,[OrgName]
                            ,[OrgStatus]
                            ,[OrgCreateYear]
                            ,[OrgCreateMonth]
                            ,[CreateTime]
                            ,[WGS84X]
                            ,[WGS84Y]
                        FROM [Organization]
                    WHERE 1=1 
                        AND [Organization].SID=@SID
                    ";

                    List<Organization> OneOrg = null;

                    DynamicParameters parameters = new();
                    parameters.Add("SID", SID, DbType.String, ParameterDirection.Input, 50);
                    OneOrg = conn.Query<Organization>(sqlCommand, parameters, commandType: CommandType.Text).ToList();

                    if (OneOrg == null || OneOrg.Count == 0)
                    {
                        count = 2;
                    }
                    else
                    {
                        sqlCommand = @"
                                UPDATE [Organization]
                                   SET [OrgID] = @OrgID
                                      ,[OrgName] = @OrgName
                                      ,[OrgStatus] = @OrgStatus
                                      ,[OrgCreateYear] = @OrgCreateYear
                                      ,[OrgCreateMonth] = @OrgCreateMonth
                                      ,[WGS84X] = @WGS84X
                                      ,[WGS84Y] = @WGS84Y
                                      ,[Geom] = geometry::STGeomFromText('POINT(' + CAST(@WGS84X AS NVARCHAR(20)) + ' ' + CAST(CAST(@WGS84Y AS decimal(20, 8)) AS nvarchar) + ')', 0)
                                 WHERE 1=1
                                ";

                        if (!SID.Equals("") && NewOrg != null)
                        {
                            sqlCommand += " AND [SID]=@SID";
                            count = conn.Execute(sqlCommand, NewOrg);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            return count;

        }





        /// <summary>
        /// 查詢所有 UserData
        /// </summary>
        /// <returns></returns>
        public List<UserData> GetAllUsers()
        {
            List<UserData> users = null;

            string sqlCommand = @"
                    SELECT [UID]
                          ,[UserID]
                          ,[UserName]
                          ,[UserPhone]
                          ,[UserOrgID]
                          ,[UserAddrZipCode]
                          ,[UserAddr]
                          ,[CityID]
                          ,[TownID]
                          ,[User_status]
                    FROM [UserData]
                    WHERE 1=1 
                    ORDER BY [UID] 
                    ";
            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
                {
                    users = conn.Query<UserData>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            return users;
        }

        /// <summary>
        /// 查詢 UserData
        /// </summary>
        /// <param name="UID">UserID</param>
        /// <returns></returns>
        public List<UserData> GetUserData(string UID)
        {
            List<UserData> users = null;

            string sqlCommand = @"
                    SELECT [UID]
                          ,[UserID]
                          ,[UserName]
                          ,[UserPhone]
                          ,[UserOrgID]
                          ,[UserAddrZipCode]
                          ,[UserAddr]
                          ,[CityID]
                          ,[TownID]
                          ,[User_status]
                    FROM [UserData]
                    WHERE 1=1 
                    ";

            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
                {
                    if (!UID.Equals(""))
                    {
                        sqlCommand += " AND [UserData].UID=@UID";
                        DynamicParameters parameters = new();
                        parameters.Add("UID", UID, DbType.String, ParameterDirection.Input, 50);
                        users = conn.Query<UserData>(sqlCommand, parameters, commandType: CommandType.Text).ToList();
                        //接回Output值
                        //int outputResult = parameters.Get<int> ("@OutPut1");
                    }
                    else
                    {
                        users = conn.Query<UserData>(sqlCommand).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            return users;
        }

        /// <summary>
        /// 更新 UserData
        /// </summary>
        /// <param name="UID">User UID</param>
        /// <param name="OneUser">Update Date</param>
        /// <returns>0:更新失敗;1:已更新;2.找不到該UID使用者資料</returns>
        public int UpdateUserData(string UID, List<UserData> NewUser)
        {
            int count = 0;
            string sqlCommand;
            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
                {
                    // Check User is exist or not
                    sqlCommand = @"SELECT * FROM [UserData] WHERE [UserData].UID=@UID";
                    List<UserData> OneUser = null;
                    OneUser = conn.Query<UserData>(sqlCommand, new { UID }).ToList();
                    if (OneUser == null || OneUser.Count == 0)
                    {
                        count = 2;
                    }
                    else
                    {
                        sqlCommand = @"
                                UPDATE [UserData]
                                   SET [UserID] = @UserID
                                      ,[UserName] = @UserName
                                      ,[UserPhone] = @UserPhone
                                      ,[UserOrgID] = @UserOrgID
                                      ,[UserAddrZipCode] = @UserAddrZipCode
                                      ,[UserAddr] = @UserAddr
                                      ,[CityID] = @CityID
                                      ,[TownID] = @TownID
                                      ,[User_status] = @User_status
                                 WHERE 1=1
                                ";

                        if (!UID.Equals("") && NewUser != null)
                        {
                            sqlCommand += " AND [UserData].UID=@UID";
                            count = conn.Execute(sqlCommand, NewUser);
                        }
                    }
                        
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            return count;

        }

        /// <summary>
        /// Delete UserData By UID
        /// </summary>
        /// <param name="UID">User UID</param>
        /// <returns>0:刪除失敗;1:刪除成功;2.找不到該UID使用者資料</returns>
        public int DeleteUserData(string UID)
        {
            int count = 0;
            string sqlCommand;
            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
                {
                    // Check User is exist or not
                    sqlCommand = @"SELECT * FROM [UserData] WHERE [UserData].UID=@UID";
                    List<UserData> OneUser = null;
                    OneUser = conn.Query<UserData>(sqlCommand, new { UID }).ToList();
                    if (OneUser == null || OneUser.Count == 0)
                    {
                        count = 2;
                    }
                    else
                    {
                        sqlCommand = @"DELETE FROM [UserData] WHERE [UID] = @UID";
                        count = conn.Execute(sqlCommand, new { UID });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            return count;
        }

        /// <summary>
        /// 新增使用者資料
        /// </summary>
        /// <param name="NewUser"></param>
        /// <returns>0:新增失敗;1:新增成功;2.已存在相同UID之使用者</returns>
        public int InsertUserData(List<UserData> NewUser)
        {
            int count = 0;
            string sqlCommand;
            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
                {
                    // Check User is exist or not
                    sqlCommand = @"SELECT * FROM [UserData] WHERE [UserData].UID=@UID";
                    List<UserData> OneUser = null;
                    OneUser = conn.Query<UserData>(sqlCommand, new { NewUser[0].UID }).ToList();
                    if (OneUser == null || OneUser.Count == 0)
                    {
                        sqlCommand = @"INSERT INTO [UserData]([UID],[UserID],
                                                   [UserName],[UserPhone],[UserOrgID],
                                                   [UserAddrZipCode],[UserAddr],[CityID],
                                                   [TownID],[User_status])
                                        VALUES
                                                   (@UID,@UserID
                                                   ,@UserName,@UserPhone,@UserOrgID
                                                   ,@UserAddrZipCode,@UserAddr,@CityID
                                                   ,@TownID,@User_status)";
                        //新增多筆參數
                        count = conn.Execute(sqlCommand, new
                        {
                            NewUser[0].UID,
                            NewUser[0].UserID,
                            NewUser[0].UserName,
                            NewUser[0].UserPhone,
                            NewUser[0].UserOrgID,
                            NewUser[0].UserAddrZipCode,
                            NewUser[0].UserAddr,
                            NewUser[0].CityID,
                            NewUser[0].TownID,
                            NewUser[0].User_status
                        });
                    }
                    else
                    {
                        count = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            return count;
        }


    }
}
