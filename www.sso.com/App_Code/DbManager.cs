using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DbManager
/// </summary>
public class DbManager
{
    private string ConnectionString = "";
    public DbManager()
    {
        ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
    }

    public List<WebUser> AuthenticateUser(string UserName, string Password)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        try
        {

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendLine(@"select 
SSO_USERS.su_GUID, (select SSO_TOKENS.st_GUID from SSO_TOKENS where SSO_TOKENS.st_UserID = SSO_USERS.su_GUID
and st_ExpireDate > getdate()) as st_GUID,SSO_USERS.su_name as UserName,SSO_USERS.su_name as FirstName,SSO_USERS.su_Surname as LastName
from SSO_USERS inner join SSO_PASSWORD_LIST on SSO_USERS.su_GUID = SSO_PASSWORD_LIST.spi_UserGUID 
inner join SSO_CUSTOMER_USERS on SSO_USERS.su_GUID = SSO_CUSTOMER_USERS.scu_UserGUID
where
 su_IsActive = 1 and spi_ExpireDate > getdate() and ");
            sql.AppendLine(" su_name =  " + "'" + UserName + "'");
            sql.AppendLine(" and spi_Password =  " + "'" + Password + "'");
            SqlCommand comm = conn.CreateCommand();
            comm.CommandText = sql.ToString();

            comm.CommandType = CommandType.Text;
            conn.Open();

            SqlDataReader reader = comm.ExecuteReader();

            List<WebUser> objectList = new List<WebUser>();
            while (reader.Read())
            {
                WebUser customObject = new WebUser()
                {
                    // srt_id = Convert.ToInt32(reader["srt_id"]),
                    UniqueId = reader["su_GUID"].ToString(),
                    Token = reader["st_GUID"].ToString(),
                    UserName = reader["UserName"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),

                };
                objectList.Add(customObject);
            }
            return objectList;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

   
    public List<WebUser> GetWebUserByUniqueId(string UniqueId)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        try
        {

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendLine(@"select 
SSO_USERS.su_GUID, (select SSO_TOKENS.st_GUID from SSO_TOKENS where SSO_TOKENS.st_UserID = SSO_USERS.su_GUID
and st_ExpireDate > getdate()) as st_GUID,SSO_USERS.su_name as UserName,SSO_USERS.su_name as FirstName,SSO_USERS.su_Surname as LastName
from SSO_USERS inner join SSO_PASSWORD_LIST on SSO_USERS.su_GUID = SSO_PASSWORD_LIST.spi_UserGUID 
inner join SSO_CUSTOMER_USERS on SSO_USERS.su_GUID = SSO_CUSTOMER_USERS.scu_UserGUID
where
 su_IsActive = 1 and spi_ExpireDate > getdate() and ");
            sql.AppendLine("  SSO_USERS.su_GUID =  " + "'" + UniqueId + "'");

            SqlCommand comm = conn.CreateCommand();
            comm.CommandText = sql.ToString();

            comm.CommandType = CommandType.Text;
            conn.Open();

            SqlDataReader reader = comm.ExecuteReader();

            List<WebUser> objectList = new List<WebUser>();
            while (reader.Read())
            {
                WebUser customObject = new WebUser()
                {
                    // srt_id = Convert.ToInt32(reader["srt_id"]),
                    UniqueId = reader["su_GUID"].ToString(),
                    Token = reader["st_GUID"].ToString(),
                    UserName = reader["UserName"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),

                };
                objectList.Add(customObject);
            }
            return objectList;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public string GenerateTokenForUser(string UserUniqueID, double Time)
    {

        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {

            string sql =
               "INSERT INTO SSO_TOKENS (st_GUID,st_UserID,st_ExpireDate) VALUES(newid(),'"+ UserUniqueID + @"', '"+ DateTime.Now.AddMinutes(Time).ToString("yyyy-MM-dd HH:mm:ss") + "'); select SSO_TOKENS.st_GUID from SSO_TOKENS where SSO_TOKENS.st_UserID = '" + UserUniqueID + "' and st_ExpireDate > getdate();";
            
            SqlCommand cmd = new SqlCommand(sql, conn);
           
          
            try
            {
                conn.Open();

                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = sql.ToString();
                comm.CommandType = CommandType.Text;
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    return reader["st_GUID"].ToString();
                }
                return "";   
            }
            catch 
            {
                return "";
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    
    }

    public List<WebUserRights> GetUserRightsOnPlatform(string token,string platformId)
    {
        List < WebUserRights > ReturnRightsForUser= new List<WebUserRights>();
        List<CustomerID> objectList = new List<CustomerID>();
        SqlConnection conn = new SqlConnection(ConnectionString);

        #region GetAllCustID
        try
        {

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendLine(@"select DISTINCT scu_CustomerID from 
SSO_TOKENS inner join SSO_CUSTOMER_USERS on SSO_CUSTOMER_USERS.scu_UserGUID = SSO_TOKENS.st_UserID
INNER JOIN SSO_CUSTOMER_APPLICATIONS ON scu_CustomerID = sca_CustomerId
WHERE SSO_TOKENS.st_GUID = '"+ token + @"'
AND SSO_CUSTOMER_APPLICATIONS.sca_AplicationID = '"+ platformId + "'");
        

            SqlCommand comm = conn.CreateCommand();
            comm.CommandText = sql.ToString();

            comm.CommandType = CommandType.Text;
            conn.Open();

            SqlDataReader reader = comm.ExecuteReader();

          
            while (reader.Read())
            {
                CustomerID customObject = new CustomerID()
                {
                    // srt_id = Convert.ToInt32(reader["srt_id"]),
                    ID = reader["scu_CustomerID"].ToString(),
                };
                objectList.Add(customObject);
            }
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
        #endregion

        #region CreateListOfRights

        foreach(CustomerID Customer in objectList)
        {
            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.AppendLine(@"select 
SSO_APPLICATION_RIGHTS.sar_Nomination, (select sart_name from SSO_APPLICATION_RIGHT_TYPE where sart_ID = SSO_APPLICATION_RIGHTS.sar_SART_ID) as [role]
 from 
SSO_TOKENS inner join SSO_CUSTOMER_USERS on SSO_CUSTOMER_USERS.scu_UserGUID = SSO_TOKENS.st_UserID
INNER JOIN SSO_CUSTOMER_APPLICATIONS ON scu_CustomerID = sca_CustomerId
inner join SSO_APPLICATION_RIGHTS on SSO_CUSTOMER_APPLICATIONS.sca_AplicationID = SSO_APPLICATION_RIGHTS.sar_SA_ID
inner join SSO_USER_RIGHTS on SSO_USER_RIGHTS.sur_SCU_ID = SSO_CUSTOMER_USERS.scu_ID and SSO_USER_RIGHTS.sur_SCA_ID = SSO_CUSTOMER_APPLICATIONS.sca_Id
and SSO_USER_RIGHTS.sur_SAR_ID = SSO_APPLICATION_RIGHTS.sar_ID
WHERE SSO_TOKENS.st_GUID = '"+ token + "' AND SSO_CUSTOMER_APPLICATIONS.sca_AplicationID = '"+ platformId + @"'
and  SSO_USER_RIGHTS.sur_Active = 1 and SSO_CUSTOMER_APPLICATIONS.sca_IsEnabled= 1
");


                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = sql.ToString();

                comm.CommandType = CommandType.Text;
                conn.Open();

                SqlDataReader reader = comm.ExecuteReader();

                List<UserRightsObject> UserRightsVector = new List<UserRightsObject>();
                while (reader.Read())
                {
                    UserRightsObject customObjectUserRights = new UserRightsObject()
                    {
                        // srt_id = Convert.ToInt32(reader["srt_id"]),
                        RightType = reader["sar_Nomination"].ToString(),
                        Right = reader["role"].ToString(),
                    };
                    UserRightsVector.Add(customObjectUserRights);
                }
                WebUserRights z = new WebUserRights();
                z.CustomerId = Customer.ID;
                z.UserRightsVector = UserRightsVector;
                ReturnRightsForUser.Add(z);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        #endregion
        return ReturnRightsForUser;
    }

}