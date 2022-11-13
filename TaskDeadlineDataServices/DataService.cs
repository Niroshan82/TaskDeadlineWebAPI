using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using TaskDeadlineDataServices.Models;

namespace TaskDeadlineDataServices
{
    public class DataService : IDataService
    {
        private IConfiguration _config;
        private readonly ILogger<DataService> _logger;
        public DataService(IConfiguration config, ILogger<DataService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public ResponseDetails AddHolidayDetails(HolidayDetails holidayDetails)
        {
            ResponseDetails responseDetails = new ResponseDetails();
            try
            {
                _logger.LogInformation(" DataService.AddHolidayDetails | AddHolidayDetails process start in the data service layer at {Time}", DateTime.UtcNow);

                using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("DefaultConnectionString")))
                    if (holidayDetails != null)
                    {
                        using (SqlCommand cmd = new SqlCommand("AddHolidayDetails", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HolidayName", holidayDetails.holidayName);
                            cmd.Parameters.AddWithValue("@HolidayDate", holidayDetails.holidayDate);
                            cmd.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                            cn.Open();
                            int i = cmd.ExecuteNonQuery();
                            cn.Close();
                            if (i > 0)
                            {

                                responseDetails.statusCode = 200;
                                responseDetails.statusMessage = "Holiday details successfully inserted.";
                                _logger.LogInformation(" DataService.AddHolidayDetails | AddHolidayDetails process completed in the data service layer at {Time}", DateTime.UtcNow);
                                return responseDetails;
                            }
                            else
                            {
                                responseDetails.statusCode = 400;
                                responseDetails.statusMessage = "Cannot save, Please check with administrator.";
                                _logger.LogInformation(" DataService.AddHolidayDetails | AddHolidayDetails insert part does not complete in the data service layer at {Time}", DateTime.UtcNow);
                                return responseDetails;
                            }

                        }

                    }
                    else
                    {
                        _logger.LogInformation(" DataService.AddHolidayDetails | AddHolidayDetails process completed in the data service layer at {Time}", DateTime.UtcNow);
                        return responseDetails;
                    }
            }
            catch (Exception ex)
            {
                _logger.LogError(" DataService.AddHolidayDetails | Error while inserting the holiday details in the data service layer at {Time}", DateTime.UtcNow);
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public List<HolidayDetails> GetAllHolidayDetails()
        {
            _logger.LogInformation("DataService.GetAllHolidayDetails | GetAllHolidayDetails process start in data service layer at {Time}", DateTime.UtcNow);
            List<HolidayDetails> holidayDetailsList = new List<HolidayDetails>();
            try
            {
                using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("DefaultConnectionString")))
                {
                    SqlDataAdapter da = new SqlDataAdapter("GetAllHolidayDetails", cn);
                    cn.Open();
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            HolidayDetails holidayDetails = new HolidayDetails();
                            holidayDetails.holidayID = Convert.ToInt32(dt.Rows[i]["HolidayID"]);
                            holidayDetails.holidayName = dt.Rows[i]["HolidayName"].ToString()!;
                            holidayDetails.holidayDate = (DateTime)dt.Rows[i]["HolidayDate"];
                            holidayDetailsList.Add(holidayDetails);
                        }
                    }
                    if (holidayDetailsList.Count > 0)
                    {
                        _logger.LogInformation(" DataService.GetAllHolidayDetails | GetAllHolidayDetails have been listed in the service layer at {Time}", DateTime.UtcNow);
                        cn.Close();
                        return holidayDetailsList;
                    }
                    else
                    {
                        _logger.LogInformation(" DataService.GetAllHolidayDetails | GetAllHolidayDetails is getting issue while that have been listed in the service layer at {Time}", DateTime.UtcNow);
                        cn.Close();
                        return holidayDetailsList;
                    }

                }

            }
            catch (Exception ex)
            {

                _logger.LogError(" DataService.GetAllHolidayDetails | Error while getting the KPI details in service layer at {Time}", DateTime.UtcNow);
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

    }
}
