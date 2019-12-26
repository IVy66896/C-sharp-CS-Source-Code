using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public class MSDB
{
    public MSDB()
    {
    }

    public static DataTable GetDataTable(string Conn, string ProcName, Dictionary<string, object> dict = null)
    {
        DataTable dt = new DataTable();

        using (SqlConnection sc = new SqlConnection(WebConfigurationManager.ConnectionStrings[Conn].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand(ProcName, sc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (dict != null)
                {
                    foreach (var item in dict)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    sc.Open();
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }


    public static DataSet GetDataSet(string Conn, string ProcName, Dictionary<string, object> dict = null)
    {
        DataSet ds = new DataSet();

        using (SqlConnection sc = new SqlConnection(WebConfigurationManager.ConnectionStrings[Conn].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand(ProcName, sc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (dict != null)
                {
                    foreach (var item in dict)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    sc.Open();
                    da.Fill(ds);
                }
            }
        }

        return ds;
    }

    public static int ExecuteNonQuery(string Conn, string ProcName, Dictionary<string, object> dict = null)
    {
        int rtn = 0;
        using (SqlConnection sc = new SqlConnection(WebConfigurationManager.ConnectionStrings[Conn].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand(ProcName, sc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (dict != null)
                {
                    foreach (var item in dict)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                sc.Open();
                rtn = cmd.ExecuteNonQuery();

            }
        }

        return rtn;
    }

    public static DataTable GetDataTable(string Conn, string ProcName, ref Dictionary<string, object> OutDict, Dictionary<string, object> dict = null)
    {
        DataTable dt = new DataTable();

        Dictionary<string, object> tmpOutDict = new Dictionary<string, object>();
        Dictionary<string, SqlParameter> paramDict = new Dictionary<string, SqlParameter>();

        using (SqlConnection sc = new SqlConnection(WebConfigurationManager.ConnectionStrings[Conn].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand(ProcName, sc))
            {
                cmd.CommandType = CommandType.StoredProcedure;


                if (dict != null)
                {

                    foreach (var item in dict)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                if (OutDict != null)
                {

                    foreach (var item in OutDict)
                    {
                        SqlParameter sp = cmd.Parameters.AddWithValue(item.Key, OutDict[item.Key]);
                        sp.Direction = ParameterDirection.Output;
                        paramDict[item.Key] = sp;
                    }
                }

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    sc.Open();
                    da.Fill(dt);
                }

                foreach (var item in OutDict)
                {
                    tmpOutDict[item.Key] = paramDict[item.Key].Value;
                }

                OutDict = tmpOutDict;

            }
        }

        return dt;
    }

    public static DataSet GetDataSet(string Conn, string ProcName, ref Dictionary<string, object> OutDict, Dictionary<string, object> dict = null)
    {
        DataSet ds = new DataSet();

        Dictionary<string, object> tmpOutDict = new Dictionary<string, object>();
        Dictionary<string, SqlParameter> paramDict = new Dictionary<string, SqlParameter>();

        using (SqlConnection sc = new SqlConnection(WebConfigurationManager.ConnectionStrings[Conn].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand(ProcName, sc))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (dict != null)
                {

                    foreach (var item in dict)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                if (OutDict != null)
                {

                    foreach (var item in OutDict)
                    {
                        SqlParameter sp = cmd.Parameters.AddWithValue(item.Key, OutDict[item.Key]);
                        sp.Direction = ParameterDirection.Output;
                        paramDict[item.Key] = sp;
                    }
                }

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    sc.Open();
                    da.Fill(ds);
                }

                foreach (var item in OutDict)
                {
                    tmpOutDict[item.Key] = paramDict[item.Key].Value;
                }

                OutDict = tmpOutDict;

            }
        }

        return ds;
    }

    public static int ExecuteNonQuery(string Conn, string ProcName, ref Dictionary<string, object> OutDict, Dictionary<string, object> dict = null)
    {
        int rtn = 0;

        Dictionary<string, object> tmpOutDict = new Dictionary<string, object>();
        Dictionary<string, SqlParameter> paramDict = new Dictionary<string, SqlParameter>();

        using (SqlConnection sc = new SqlConnection(WebConfigurationManager.ConnectionStrings[Conn].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand(ProcName, sc))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (dict != null)
                {
                    foreach (var item in dict)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                if (OutDict != null)
                {

                    foreach (var item in OutDict)
                    {
                        SqlParameter sp = cmd.Parameters.AddWithValue(item.Key, OutDict[item.Key]);
                        sp.Direction = ParameterDirection.Output;
                        paramDict[item.Key] = sp;
                    }
                }

                sc.Open();
                rtn = cmd.ExecuteNonQuery();

                foreach (var item in OutDict)
                {
                    tmpOutDict[item.Key] = paramDict[item.Key].Value;
                }

                OutDict = tmpOutDict;
            }
        }

        return rtn;
    }
}
