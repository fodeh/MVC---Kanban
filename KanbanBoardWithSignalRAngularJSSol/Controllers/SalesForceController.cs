using System;
using System.Net;
using KanbanBoardWithSignalRAngularJSSol.WebReference;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;

namespace KanbanBoardWithSignalRAngularJSSol.Controllers
{
    public class SalesForce : Controller //Class controller for managing salesforce connection and data extraction
    {
        private SforceService binding; //Creates binding opject
        [HttpPost, ValidateInput(false)] //Removes uneccessary validation check

        //InitializeAsync handles the connection, login, and query extraction. Required fields pass all neccessary data to be used anywhere outside this function
        public void InitializeAsync(int numRecord, sObject[] records, string[] CaseNumArray, string[] SubjectArray, string[] DescArray, string[] OwnerArray, string[] CreatorArray, DateTime?[] CreatedDateArray, double[] TotalHoursArray)
        {
            ServicePointManager.Expect100Continue = true; //Enables prolonged use
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //Sets needed security level

            var list = new List<string>(); //List to hold case numbers that have been used
            string user = ""; //Test salesforce user
            string pass = ""; //Test salesforce pass
            int Id = 0; //Variable used to determine employee id *other controllers ask for this even though it's useless, it's more annoying to remove 
            int i = 0; //variable for loops

            binding = new SforceService(); //Initializs binding object
            binding.Timeout = 60000; //Sets maximum time per binding
            WebReference.LoginResult lr; //lr is my login result
            lr = binding.login(user, pass); //Used lr to create credentials
            string authEndPoint = binding.Url; //Not sure what this does honestly

            binding.Url = lr.serverUrl; //Sets binding to correct server
            binding.SessionHeaderValue = new SessionHeader(); //Header needed for API format
            binding.SessionHeaderValue.sessionId = lr.sessionId; //Gives binder the session id

            //---------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Query pulls all requested fields with cases that fit the correct case status and only pulls cases for the dev team.
            string soqlQuery = "SELECT Subject, CaseNumber, Owner.Name, CreatedDate, Total_Time_Logged__c, CreatedBy.Name, Description FROM Case where (Status = 'Case Opened' or Status = 'Case Assigned' or Status = 'Case Re-Opened' or Status = 'Case Being Researched' or Status = 'Case Being Researched - Client' or Status = 'Case Being Tested - Client')" +
            "and (Owner.Name = '')";
            QueryResult qr = binding.query(soqlQuery); //variable holding data pulled from the query
            //Conection string to our db - modeloffice. Uses IP instead of name and connects to AttendanceDB
            string connect = "Data Source=;Initial Catalog=AttendanceDB;User ID=;Password=;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //---------------------------------------------------------------------------------------------------------------------------------------------------------------------  

            if (qr.size > 0) //Only runs if there's data
            {
                Console.WriteLine(""); //Probably dont need
                sObject[] records1 = qr.records; //object to hold data records
                numRecord = records1.Length; //numRecord keeps track of the amount of cases pulled. This is used in other classes if needed to loop through cases
                records = records1; //Transfers data to different container. records can be called externally for access

                for (i = 0; i < records.Length; i++) //loop to pass through each data record
                {
                    Case c = (Case)records[i]; //A case object to pull case specific information

                    using (var connection = new SqlConnection(connect))
                    {
                        string query1 = "select CaseNumber from Tasks";
                        SqlCommand command1 = new SqlCommand(query1, connection); //assigns query to command varaible
                        connection.Open(); //opens db
                        SqlDataReader qr1 = command1.ExecuteReader(); //executes query
                        while (qr1.Read())
                        {
                            list.Add(qr1.GetString(0));
                        }
                        connection.Close(); //closes db
                    }

                    if (list.Contains(c.CaseNumber))
                    {
                        //Do nothing
                    }
                    else
                    {
                        SubjectArray[i] = c.Subject; //subject array
                        CaseNumArray[i] = c.CaseNumber; //case number array
                        OwnerArray[i] = c.Owner.Name1; //case owner array
                        CreatorArray[i] = c.CreatedBy.Name; //case creator array
                        CreatedDateArray[i] = c.CreatedDate; //date creation
                        TotalHoursArray[i] = c.Total_Time_Logged__c.Value; //hours logged
                        list.Add(c.CaseNumber); //Adds unused cases to the used list

                        if (c.Description == null) { DescArray[i] = " "; } //check to avoid null description. *Form explodes if null
                        else { DescArray[i] = c.Description; } //Description array

                        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

                        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

                        using (var connection = new SqlConnection(connect)) //DB connection
                        {
                            //Parameterized strings for queries to avoid SQL errors
                            string q2 = "Values (" + "'" + CaseNumArray[i].ToString() + "'" + ", " + "'" + DescArray[i].ToString().Replace("'", "''") + "'" + ", " + 1 + ", " + "'" + OwnerArray[i].ToString() + "'" + ", " + "'" + CreatorArray[i].ToString() + "'" + ", " + TotalHoursArray[i] + ", " + "'" + CreatedDateArray[i] + "'" + ", " + Id + ", " + "'" + SubjectArray[i].ToString().Replace("'", "''") + "'" + ")";
                            string q1 = "Insert into Tasks (CaseNumber, Description, ColumnId, EmployeeName, PmAssignedBy, selectedHours, StartDate, EmployeeId, Name) ";
                            string query = q1 + q2;

                            SqlCommand command = new SqlCommand(query, connection); //assigns query to command varaible
                            connection.Open(); //opens db
                            command.ExecuteNonQuery(); //executes query
                            connection.Close(); //closes db
                        }
                    }
                }
            }
        }
        public void logout() //function to logout
        {
            binding.logout(); //releases account from binding
        }

        public void upLoad()
        {
            ServicePointManager.Expect100Continue = true; //Enables prolonged use
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //Sets needed security level

            var list = new List<string>(); //List to hold case numbers that have been used
            string user = ""; //Test salesforce user
            string pass = ""; //Test salesforce pass

            binding = new SforceService(); //Initializs binding object
            binding.Timeout = 60000; //Sets maximum time per binding
            WebReference.LoginResult lr; //lr is my login result
            lr = binding.login(user, pass); //Used lr to create credentials
            string authEndPoint = binding.Url; //Not sure what this does honestly

            binding.Url = lr.serverUrl; //Sets binding to correct server
            binding.SessionHeaderValue = new SessionHeader(); //Header needed for API format
            binding.SessionHeaderValue.sessionId = lr.sessionId; //Gives binder the session id

            try
            {
                Case c = new Case();
                Case[] cases = new Case[1];
                CaseComment com = new CaseComment();
                CaseComment[] coms = new CaseComment[1];
    
                //c.AccountId = "GBS Non Case Events";
                //c.Subject = "API Test";
                //c.Type = "MAC - Non-Billable";
                //c.Case_Product_Category__c = "Project";
                //c.Product_Subcategory__c = "Training";
                //c.Priority = "SLA - 5 - Client Request/Client Created Issue";
                //c.Origin = "Phone";
                //c.Reason = "Test Description";
                //c.Status = "Case Opened";
                //c.OwnerId = "Firaus Odeh";
                //c.ContactId = "";
                //c.Description = "Test";
                //c.IsEscalated = false;
                //c.SuppliedName = "Test Case";
                //c.SuppliedEmail = "";
                //c.SuppliedPhone = "";
                //c.SuppliedCompany = "";
                //SaveResult[] results = binding.create(cases);
                //Debug.WriteLine("valid Data");
                c.Id = "5000g000026SPQ8AAO";
                string query1 = "Insert into Case (Comments) Value ('Test') Where CaseNumber = '00311360'";
                QueryResult result = binding.query(query1);
                com.Id = result.ToString();
                c.Subject = "Test";
                com.CommentBody = "Test comment";
                c.Comments = "test";
                coms[0] = com;
                SaveResult[] check = binding.create(coms);
            }
            catch
            {
                Debug.WriteLine("Invalid data");
            }
        }
    }
}







