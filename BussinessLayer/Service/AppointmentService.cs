using BussinessLayer.ViewModels;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public  class AppointmentService
    {

        public bool  GetAllAppointments( out DataTable dataTable, out string Error)
        {
            
            SqlCommand cmd = new SqlCommand(@"Select * From Appointment"); 
            using (SqlConnection con = new SqlConnection(BasicParam.ConStr))
            {

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (dataTable = new DataTable())
                    {
                        try
                        {
                            sda.Fill(dataTable);
                            Error = "It is done";
                            return  true;
                            
                            
                        }
                        catch (Exception ex)
                        {
                            Error =ex.ToString();
                            return false; 
                        }


                    }
                }

            }
           
       
        }

       
        public bool AddAppointment(string Dr, string Patient, DateTime Date, int Payment, out string Error)
        {
            Error = string.Empty;

                
                                    
            if (Patient == "" || Dr == "" || Date.ToString() == ""  ||Payment <=0)
            {         
                
                Error = "Please fill all the texts with valid values";
                return false;
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(BasicParam.ConStr))
                    {
                        SqlCommand cmd = new SqlCommand(@"EXEC AddAppointment @Price0, @Cust0, @Dr0, @DrShare0, @Date0");
                        cmd.Parameters.AddWithValue("@Dr0", Dr);
                        cmd.Parameters.AddWithValue("@Cust0", Patient);
                        cmd.Parameters.AddWithValue("@Date0", Date);
                        cmd.Parameters.AddWithValue("@Price0", Payment);
                        cmd.Parameters.AddWithValue("@DrShare0", 50);


                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            con.Open();
                            sda.SelectCommand.ExecuteNonQuery();

                            con.Close();
                            Error ="data entered succesfully. . . .";
                            return true; 


                        }
                    }
                }
                catch (Exception e)
                {
                    Error = e.ToString();
                    return false;
                }

            }

        }

        public bool FilterAppointment(string Dr, string Patient, DateTime DateFrom, DateTime DateTo, int PriceFrom, int PriceTo, out string Error )
        {
            Error = string.Empty;

                using (SqlConnection con = new SqlConnection(BasicParam.ConStr))
                {
                    SqlCommand cmd2 = new SqlCommand(@"Exec FilterAppointment  @PriceFrom0, @Priceto0,@Cust0,@Dr0,  @StartDate0, @EndDate0");
                    cmd2.Connection = con;
                    cmd2.Parameters.AddWithValue("@Dr0", Dr);
                    cmd2.Parameters.AddWithValue("@Cust0", Patient);
                    cmd2.Parameters.AddWithValue("@PriceFrom0", PriceFrom);
                    cmd2.Parameters.AddWithValue("@PriceTo0", PriceTo);

                    cmd2.Parameters.AddWithValue("@StartDate0", DateFrom);
                    cmd2.Parameters.AddWithValue("@EndDate0", DateTo);


                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd2.Connection = con;
                        sda.SelectCommand = cmd2;
                        con.Open();
                        sda.SelectCommand.ExecuteNonQuery();

                        con.Close();
                        Error = "data entered succesfully. . . .";
                        return true;


                    }

                }

           
        }


      

    }
}
