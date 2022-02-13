using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MVCDTS.Models;
using NLog;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MVCDTS.DAL
{
    public class StudentDAL
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;
        private SqlConnection _connection;
        private bool _isConnected;

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public StudentDAL()
        {
            _connectionString = GetConfiguration().GetSection("Data").GetSection("DataContext").Value;
            _connection = new SqlConnection(_connectionString);
            _isConnected = false;
        }

        private bool Connect()
        {
            try
            {
                if (_isConnected)
                {
                    return _isConnected;
                }
                _connection.Open();
                _isConnected = true;
            }
            catch
            {
                _isConnected = false;
            }
            return _isConnected;
        }

        public DataSet GetStudents()
        {
            DataSet dataSet = new DataSet();
            if (Connect())
            {
                try
                {
                    SqlCommand sql = new SqlCommand("usp_GetDetails", _connection);
                    sql.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sql);
                    dataAdapter.Fill(dataSet);
                    _connection.Close();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
            return dataSet;
        }

        public void InsertStudents(StudentModel studentModel)
        {
            if (Connect())
            {
                try
                {
                    SqlCommand sql = new SqlCommand("usp_InsertRecord", _connection);
                    sql.CommandType = CommandType.StoredProcedure;
                    sql.Parameters.AddWithValue("@Name",studentModel.Name);
                    sql.Parameters.AddWithValue("@Age",studentModel.Age);
                    sql.Parameters.AddWithValue("@Address",studentModel.Address);
                    sql.Parameters.AddWithValue("@Subject_Opted",studentModel.Subject_opted);
                    sql.ExecuteNonQuery();
                    _connection.Close();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
        }

        public void UpdateStudent(StudentModel studentModel)
        {
            if (Connect())
            {
                try
                {
                    SqlCommand sql = new SqlCommand("usp_UpdateRecord", _connection);
                    sql.CommandType = CommandType.StoredProcedure;
                    sql.Parameters.AddWithValue("@Id", studentModel.ID);
                    sql.Parameters.AddWithValue("@Name", studentModel.Name);
                    sql.Parameters.AddWithValue("@Age", studentModel.Age);
                    sql.Parameters.AddWithValue("@Address", studentModel.Address);
                    sql.Parameters.AddWithValue("@Subject_Opted", studentModel.Subject_opted);
                    sql.ExecuteNonQuery();
                    _connection.Close();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
        }

        public DataSet ShowRecordById(int Id)
        {
            DataSet ds = new DataSet();
            if (Connect())
            {
                try
                {
                    SqlCommand sql = new SqlCommand("usp_GetRecordById", _connection);
                    sql.CommandType = CommandType.StoredProcedure;
                    sql.Parameters.AddWithValue("@Id", Id);
                    SqlDataAdapter adapter = new SqlDataAdapter(sql);
                    adapter.Fill(ds);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
            return ds;
        }

        public void DeleteStudent(int Id)
        {
            if (Connect())
            {
                try
                {
                    SqlCommand sql = new SqlCommand("usp_DeleteRecord", _connection);
                    sql.CommandType = CommandType.StoredProcedure;
                    sql.Parameters.AddWithValue("@Id", Id);
                    sql.ExecuteNonQuery();
                    _connection.Close();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
        }
    }
}
