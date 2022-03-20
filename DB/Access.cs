using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace RCPA.DB
{
  /// <summary>
  /// Description of Access database utility.
  /// </summary>
  public class Access
  {
    public int PageSize { get; set; }

    private string connectionString;

    private ObjectPool<OleDbConnection> pool;

    public Access(string accessFileName)
    {
      var acFile = accessFileName.Replace('\\', '/').Replace("//", "/");
      this.connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", acFile);
      this.pool = new ObjectPool<OleDbConnection>(() => new OleDbConnection(connectionString));
      this.PageSize = 14;
    }

    //执行单条插入语句，并返回id，不需要返回id的用ExceuteNonQuery执行。
    public int ExecuteInsert(string sql, OleDbParameter[] parameters)
    {
      //Debug.WriteLine(sql);
      var connection = pool.New();
      try
      {
        OleDbCommand cmd = new OleDbCommand(sql, connection);
        try
        {
          connection.Open();
          if (parameters != null)
            cmd.Parameters.AddRange(parameters);
          cmd.ExecuteNonQuery();
          cmd.CommandText = @"select @@identity";
          int value = Int32.Parse(cmd.ExecuteScalar().ToString());
          return value;
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      finally
      {
        pool.Delete(connection);
      }
    }

    public int ExecuteInsert(string sql)
    {
      return ExecuteInsert(sql, null);
    }

    //执行带参数的sql语句,返回影响的记录数（insert,update,delete)
    public int ExecuteNonQuery(string sql, OleDbParameter[] parameters)
    {
      //Debug.WriteLine(sql);
      //Debug.WriteLine(sql);
      var connection = pool.New();
      try
      {
        OleDbCommand cmd = new OleDbCommand(sql, connection);
        try
        {
          connection.Open();
          if (parameters != null)
            cmd.Parameters.AddRange(parameters);
          int rows = cmd.ExecuteNonQuery();
          return rows;
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      finally
      {
        pool.Delete(connection);
      }
    }

    //执行不带参数的sql语句，返回影响的记录数
    //不建议使用拼出来SQL
    public int ExecuteNonQuery(string sql)
    {
      return ExecuteNonQuery(sql, null);
    }

    //执行单条语句返回第一行第一列,可以用来返回count(*)
    public int ExecuteScalar(string sql, OleDbParameter[] parameters)
    {
      //Debug.WriteLine(sql);
      //Debug.WriteLine(sql);
      var connection = pool.New();
      try
      {
        OleDbCommand cmd = new OleDbCommand(sql, connection);
        try
        {
          connection.Open();
          if (parameters != null) cmd.Parameters.AddRange(parameters);
          int value = Int32.Parse(cmd.ExecuteScalar().ToString());
          return value;
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      finally
      {
        pool.Delete(connection);
      }
    }

    public int ExecuteScalar(string sql)
    {
      return ExecuteScalar(sql, null);
    }

    //执行事务
    public void ExecuteTrans(List<string> sqlList, List<OleDbParameter[]> paraList)
    {
      //Debug.WriteLine(sql);
      //Debug.WriteLine(sql);
      var connection = pool.New();
      try
      {
        OleDbCommand cmd = new OleDbCommand();
        OleDbTransaction transaction = null;
        cmd.Connection = connection;
        try
        {
          connection.Open();
          transaction = connection.BeginTransaction();
          cmd.Transaction = transaction;

          for (int i = 0; i < sqlList.Count; i++)
          {
            cmd.CommandText = sqlList[i];
            if (paraList != null && paraList[i] != null)
            {
              cmd.Parameters.Clear();
              cmd.Parameters.AddRange(paraList[i]);
            }
            cmd.ExecuteNonQuery();
          }
          transaction.Commit();

        }
        catch (Exception e)
        {
          try
          {
            transaction.Rollback();
          }
          catch
          {

          }
          throw e;
        }
      }
      finally
      {
        pool.Delete(connection);
      }
    }

    public void ExecuteTrans(List<string> sqlList)
    {
      ExecuteTrans(sqlList, null);
    }

    //执行查询语句，返回dataset
    public DataSet ExecuteQuery(string sql, OleDbParameter[] parameters)
    {
      //Debug.WriteLine(sql);
      var connection = pool.New();
      try
      {
        DataSet ds = new DataSet();
        try
        {
          connection.Open();

          OleDbDataAdapter da = new OleDbDataAdapter(sql, connection);
          if (parameters != null) da.SelectCommand.Parameters.AddRange(parameters);
          da.Fill(ds, "ds");
        }
        catch (Exception ex)
        {
          throw ex;
        }
        return ds;
      }
      finally
      {
        pool.Delete(connection);
      }
    }

    public DataSet ExecuteQuery(string sql)
    {
      return ExecuteQuery(sql, null);
    }

    //执行查询语句返回datareader，使用后要注意close
    //这个函数在AccessPageUtils中使用，执行其它查询时最好不要用
    public OleDbDataReader ExecuteReader(string sql)
    {
      //Debug.WriteLine(sql);
      //Debug.WriteLine(sql);
      var connection = pool.New();
      try
      {
        OleDbCommand cmd = new OleDbCommand(sql, connection);
        try
        {
          connection.Open();
          return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception e)
        {
          connection.Close();
          throw e;
        }
      }
      finally
      {
        pool.Delete(connection);
      }
    }

    //每页默认记录条数PAGE_SIZE
    public DataSet query(string sql, int curPageIndex, int totalRecord)
    {
      return query(sql, curPageIndex, PageSize, totalRecord);
    }

    //通过datareader读取当前页数据，并且封装为dataset返回
    public DataSet query(string sql, int curPageIndex, int pageSize, int totalRecord)
    {
      DataSet ds = new DataSet();
      OleDbDataReader dr = ExecuteReader(sql);
      //
      DataTable st = dr.GetSchemaTable();
      DataTable dt = ds.Tables.Add("ds");
      foreach (DataRow row in st.Rows)
      {
        DataColumn c = new DataColumn();
        c.ColumnName = row["ColumnName"].ToString();
        c.DataType = System.Type.GetType(row["DataType"].ToString());
        dt.Columns.Add(c);
        //Debug.WriteLine(c.ColumnName+"--"+c.DataType);
      }
      //
      int colCount = st.Rows.Count;
      int count = 0;
      int start = curPageIndex * pageSize;
      int end = start + pageSize;
      while (dr.Read())
      {
        if (count < start)
        {
          count++;
          continue;
        }

        DataRow r = dt.NewRow();
        for (int i = 0; i < colCount; i++)
        {
          r[i] = dr[i];
        }

        dt.Rows.Add(r);
        count++;

        if (count >= end)
          break;
      }
      dr.Close();

      return ds;
    }
  }
}
