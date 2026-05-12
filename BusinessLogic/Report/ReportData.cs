using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Report
{
    public class ReportData
    {
        CData data;
        private List<SqlParameter> sp;
        private List<SqlParameter> optSp;

        public List<SqlParameter> Sp
        {
            get
            {
                return sp;
            }

            set
            {
                sp = value;
            }
        }

        public List<SqlParameter> OptSp
        {
            get
            {
                return optSp;
            }
            set
            {
                optSp = value;
            }
        }

        public ReportData()
        {
            data = new CData();
            Sp = new List<SqlParameter>();
            OptSp = new List<SqlParameter>();
        }
        #region Public method
        public void AddSqlParameter(string key, object value)
        {
            Sp.Add(new SqlParameter(key, value));
        }

        public void AddListSqlParameter(Dictionary<string, object> objParms)
        {
            foreach (string item in objParms.Keys)
            {
                object obj2 = objParms[item];
                sp.Add(new SqlParameter("@" + item, obj2));
            }
        }
        /// <summary>
        /// Add tham số điều kiện vào store procedure
        /// </summary>
        /// <param name="lstKey"></param>
        /// <param name="objParms"></param>
        public void AddListSqlParameter(string[] lstKey, Dictionary<string, object> objParms)
        {
            foreach (string item in lstKey)
            {
                if (item != null && item != "")
                {
                    object obj2 = objParms[item];
                    sp.Add(new SqlParameter("@" + item, obj2));
                }
            }
        }
        /// <summary>
        /// Add tham số để store procedure truyền trả dữ liệu về
        /// </summary>
        /// <param name="lstKey"></param>
        public void AddListOptSqlParameter(string[] lstKey)
        {
            foreach (var key in lstKey)
            {
                var pr = new SqlParameter(key, SqlDbType.VarChar, 15);
                pr.Direction = ParameterDirection.Output;
                optSp.Add(pr);
            }
        }
        public void AddOutSqlParameter(string key)
        {
            var pr = new SqlParameter(key, SqlDbType.VarChar, 15);
            pr.Direction = ParameterDirection.Output;
            optSp.Add(pr);
        }
        public void changSqlParameter(string key, object value)
        {
            Sp.Where(w => w.ParameterName == key).ToList().ForEach(s => s.Value = value);
        }
        public void setSqlParameter(List<SqlParameter> ipSP)
        {
            foreach (var item in ipSP)
            {
                Sp.Add(new SqlParameter(item.ParameterName, item.Value));
            };
        }
        public DataSet GetDataSet(string strProc)
        {
            return data.ExecuteDSQuery(strProc, Sp);
        }

        public DataSet GetDataSet(string strProc, out Dictionary<string, object> optParams)
        {
            return data.ExecuteDSQuery(strProc, Sp, OptSp, out optParams);
        }

        public DataTable GetDataTable(string strProc)
        {
            var dataset = data.ExecuteDSQuery(strProc, Sp);
            if (dataset != null && dataset.Tables.Count > 0)
            {
                return dataset.Tables[0];
            }
            return null;
        }
        #endregion
    }
}
