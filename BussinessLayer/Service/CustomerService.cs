
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BussinessLayer.ViewModels;

namespace BussinessLayer.Service
{
    public class CustomerService
    {
        public bool GetAllCustomers(out DataTable dataTable, out string Error)
        {

            SqlCommand cmd = new SqlCommand(@"Select * From Patient");
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

        public bool GetAllCustomerUserNames(out string[] UserNames, out string Error)
        {
            DataTable AllCs = new DataTable();
            UserNames = new string[1];

            if (GetAllCustomers(out AllCs, out Error))
            {
                Array.Resize(ref UserNames, AllCs.Rows.Count);
                for (int k = 0; k < AllCs.Rows.Count; k++)
                {
                    UserNames[k] = AllCs.Rows[k]["UserName"].ToString();


                }

                return true;
            }
            else
            {
                return false;
            }
        }


        ////public CustomerViewModel GetACustomer(string UserName)
        ////{
        ////    var User = new CustomerViewModel();

        ////    using (var db = new HiExpertDataBaseContext())
        ////    {


        ////        User = db.Customers.Where(v => v.UserName == UserName).Select(v => new CustomerViewModel
        ////        {
        ////            Name = v.Name,
        ////            UserName = v.UserName,
        ////            Phone = v.Phone,
        ////            Email = v.Email,
        ////            TotalPayment = v.TotalPayment
        ////        }).FirstOrDefault(); 
        ////    }
        ////    return User; 

        ////}


        public int AddCustomer(string UserName, string Name, string Phone, string Email, out string Error)
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
                    if (GetAllCustomers(out DataTable AllCs, out Error))
                    {
                        for (int k = 0; k < AllCs.Rows.Count; k++)
                        {
                            if (AllCs.Rows[k]["UserName"].ToString() == UserName)
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
                            insert into Patient(UserName,Name,Phone,Email)values( @UserName,@Name,@Email,@Phone)");
                                cmd2.Parameters.AddWithValue("@UserName", UserName);
                                cmd2.Parameters.AddWithValue("@Name", Name);
                                cmd2.Parameters.AddWithValue("@Email", Email);
                                cmd2.Parameters.AddWithValue("@Phone", Phone);

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


        public bool RemoveCustomer(string UserName, string Error)
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


                        SqlCommand cmd2 = new SqlCommand(@"Delete from Patient WHERE USERNAME = @UserName");

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

        public bool UpdateCustomer(string UserName, string Name, string Phone, string Email, out string Error, params object[] List)
        {
            Error = string.Empty;

            if (UserName == "" || Name == "")
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
                        UPDATE Patient SET Name = @Name WHERE USERNAME = @UserName;
                        UPDATE Patient SET Email = @Email WHERE USERNAME = @UserName;
                        UPDATE Patient SET Phone = @Phone WHERE USERNAME = @UserName;");
                        cmd2.Parameters.AddWithValue("@UserName", UserName);
                        cmd2.Parameters.AddWithValue("@Name", Name);
                        cmd2.Parameters.AddWithValue("@Email", Email);
                        cmd2.Parameters.AddWithValue("@Phone", Phone);

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


        public bool SearchUser(string Ch, out DataTable dataTable, out string Error)
        {
            Error = "";
            dataTable = new DataTable(); 
            try
            {
                using (SqlConnection con = new SqlConnection(BasicParam.ConStr))
                {
                    SqlCommand cmd2 = new SqlCommand(@"SELECT * FROM Patient where (Name like '%'+@ch+'%' or UserName like '%'+@ch+'%'
or phone like '%'+@ch+'%' or Email like '%'+@ch+'%');");

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

