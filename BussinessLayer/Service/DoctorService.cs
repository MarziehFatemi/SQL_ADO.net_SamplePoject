
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLayer.ViewModels;
using System.Data.SqlClient;
using System.Data;

namespace BussinessLayer.Service
{

    public class DoctorService
    {

        public bool GetAllDrs(out DataTable dataTable, out string Error)
        {

            SqlCommand cmd = new SqlCommand(@"Select * From Physician");
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
                            return true;


                        }
                        catch (Exception ex)
                        {
                            Error = ex.ToString();
                            return false;
                        }


                    }
                }

            }


        }

        public bool GetAllDrUserNames(out string[] UserNames, out string Error)
        {
            DataTable AllDrs = new DataTable();
            UserNames = new string[1];

            if (GetAllDrs(out AllDrs, out Error))
            {
                Array.Resize(ref UserNames, AllDrs.Rows.Count);
                for (int k = 0; k < AllDrs.Rows.Count; k++)
                {
                    UserNames[k] = AllDrs.Rows[k]["UserName"].ToString();


                }

                return true;
            }
            else
            {
                return false;
            }
        }


        ////public DoctorViewModel GetADr(string UserName)
        ////{
        ////    var Dr = new DoctorViewModel(); 

        ////    using (var db = new HiExpertDataBaseContext())
        ////    {

        ////        Dr = db.Doctors.Where(v => v.UserName == UserName).Select(v => new DoctorViewModel
        ////        {
        ////            Name = v.Name,
        ////            UserName = v.UserName,
        ////            Phone = v.Phone,
        ////            CardNumber = v.CardNumber,
        ////            AccountNumber = v.AccountNumber,
        ////            TotalCheckedOut = v.TotalCheckedOut,
        ////            TotalSale = v.TotalSale,
        ////            TotalIncome = v.TotalIncome,
        ////            CommissionPercent = v.CommissionPercent,
        ////            Credit = v.Credit
        ////        }).FirstOrDefault();
        ////    }

        ////    return Dr;

        ////}

        public int AddDr(string UserName, string Name, string Phone, string CardNumber, string AccountNumber, int CommissionPercent, out string Error)
        {
            Error = string.Empty;



            if (UserName == "" || Name == "" || Phone == "")
            {

                Error = "Please fill all the texts";
                return -1;
            }
            else
            {
                try
                {
                    bool IsNew = true;
                    if (GetAllDrs(out DataTable AllDrs, out Error))
                    {
                        for (int k = 0; k < AllDrs.Rows.Count; k++)
                        {
                            if (AllDrs.Rows[k]["UserName"].ToString() == UserName)
                            {
                                Error = "The UserName is aleardy exist";
                                IsNew = false;

                                break;
                            }
                        }

                        if (IsNew)
                        {
                            using (SqlConnection con = new SqlConnection(BasicParam.ConStr))
                            {

                                con.Open();
                                SqlCommand cmd2 = new SqlCommand(@"
                        insert into Physician(UserName,Name,Phone,CardNumber,AccountNumber)values(
                        @UserName,@Name,@Phone, @CardNumber, @AccountNumber,@CommisionPercent");

                                cmd2.Parameters.AddWithValue("@UserName", UserName);
                                cmd2.Parameters.AddWithValue("@Name", Name);
                                cmd2.Parameters.AddWithValue("@CommisionPercent", CommissionPercent);
                                cmd2.Parameters.AddWithValue("@Phone", Phone);
                                cmd2.Parameters.AddWithValue("@CardNumber", CardNumber);
                                cmd2.Parameters.AddWithValue("@AccountNumber", AccountNumber);

                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd2.Connection = con;
                                    sda.SelectCommand = cmd2;

                                    sda.SelectCommand.ExecuteNonQuery();
                                    con.Close();

                                    Error = "The User is Added Succussfully";
                                    return 1;
                                }

                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {

                        return -3;

                    }

                }
                catch (Exception e)
                {
                    Error = e.ToString();
                    return -2;
                }

            }

        }


        public bool RemoveDr(string UserName, string Error)
        {
            Error = string.Empty;

            if (UserName == "")
            {
                Error = "Please fill The UserName";
                return false;

            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(BasicParam.ConStr))
                    {


                        SqlCommand cmd2 = new SqlCommand(@"Delete from Physician WHERE USERNAME = @UserName");

                        cmd2.Parameters.AddWithValue("@UserName", UserName);


                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd2.Connection = con;
                            sda.SelectCommand = cmd2;
                            con.Open();
                            sda.SelectCommand.ExecuteNonQuery();

                            con.Close();

                            Error = "The User is removed Succussfully";
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

        public bool UpdateDr(string UserName, string Name, string Phone, string CardNumber, string AccountNumber,
        int CommissionPercent, out string Error)
        {
            Error = string.Empty;

            if (UserName == "" || Name == "" || Phone == "")
            {

                Error = "Please fill all the texts";
                return false;
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(BasicParam.ConStr))
                    {
                        SqlCommand cmd2 = new SqlCommand(@"
                        UPDATE Physician SET Name = @Name WHERE USERNAME = @UserName;
                        UPDATE Physician SET Phone = @Phone WHERE USERNAME = @UserName;
                        UPDATE Physician SET CardNumber = @CardNumber WHERE USERNAME = @UserName; 
                        UPDATE Physician SET AccountNumber = @AccountNumber WHERE USERNAME = @UserName;
                        UPDATE Physician SET CommisionPercent = @CommisionPercent WHERE USERNAME = @UserName; ");

                        cmd2.Parameters.AddWithValue("@UserName", UserName);
                        cmd2.Parameters.AddWithValue("@Name", Name);

                        cmd2.Parameters.AddWithValue("@CommisionPercent", CommissionPercent);
                        cmd2.Parameters.AddWithValue("@Phone", Phone);
                        cmd2.Parameters.AddWithValue("@CardNumber", CardNumber);
                        cmd2.Parameters.AddWithValue("@AccountNumber", AccountNumber);


                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd2.Connection = con;
                            sda.SelectCommand = cmd2;
                            con.Open();
                            sda.SelectCommand.ExecuteNonQuery();

                            con.Close();

                            Error = "The User is Edited Succussfully";
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

        public bool SearchDr(string Ch, out DataTable dataTable, out string Error)
        {
            Error = "";
            dataTable = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(BasicParam.ConStr))
                {
                    SqlCommand cmd2 = new SqlCommand(@"SELECT * FROM Physician where (Name like '%'+@ch+'%' or UserName like '%'+@ch+'%'
or phone like '%'+@ch+'%' or AccountNumber like '%'+@ch+'%' or CardNumber like '%'+@ch+'%'
or TotalSale like '%'+@ch+'%' or TotalIncome like '%'+@ch+'%' or CommisionPercent like '%'+@ch+'%'
);");
                    cmd2.Parameters.AddWithValue("@ch", Ch);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd2.Connection = con;
                        sda.SelectCommand = cmd2;
                        using (dataTable = new DataTable())
                        {
                            try
                            {
                                sda.Fill(dataTable);
                                Error = "It is done";
                                return true;


                            }
                            catch (Exception ex)
                            {
                                Error = ex.ToString();
                                return false;
                            }


                        }
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
}
