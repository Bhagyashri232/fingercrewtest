using System.Data.SqlClient;

namespace FingerCrew.Model
{
    public class StoreErrorLog
    {
        DBHelper dbHelper = new DBHelper();
        public void StoreError(ErroLogBO param)
        {
            List<SqlParameter> lstSqlParameter = new List<SqlParameter>();
            lstSqlParameter.Add(new SqlParameter("@mode", param.mode));
            lstSqlParameter.Add(new SqlParameter("@fnname", param.fnName));
            lstSqlParameter.Add(new SqlParameter("@error_message", param.error_message));
            lstSqlParameter.Add(new SqlParameter("@error_description", param.error_description));
            dbHelper.ExecuteNonQuery("SP_StoreErrorLog", lstSqlParameter.ToArray());
        }
    }

    public class ErroLogBO
    {
        public string mode { get; set; }
        public string fnName { get; set; }
        public string error_message { get; set; }
        public string error_description { get; set; }
    }
}
