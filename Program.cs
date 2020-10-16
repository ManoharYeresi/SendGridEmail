using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SendGrid
{
    class Program
    {
        static void Main(string[] args)
        {
            //Grid grid = new Grid();
            //grid.Execute();
            SendGridEmail sendGrid = new SendGridEmail();
            sendGrid.SendMailContents();
            sendGrid.SendEmail();

        }
    }
    public class SendGridEmail
    {
        private readonly string SendGridUrl = "https://api.sendgrid.com/v3/mail/send";
        private readonly string AuthHeaderValue = string.Format("Bearer {0}", "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");
        private WebRequest webRequest;
        protected const string HtmlTypeEmail = "text/html";
        protected SendGridEmailRequest emailRequest;
        protected Personalization personalization;

        private void InitWebRequest()
        {
            this.webRequest = WebRequest.Create(this.SendGridUrl);
            this.webRequest.Method = "POST";
            this.webRequest.ContentType = "application/json";
            this.webRequest.Headers.Add("Authorization", this.AuthHeaderValue);
            this.webRequest.Proxy = WebRequest.GetSystemWebProxy();
        }

        public SendGridEmail()
        {
            if (this.emailRequest == null)
            {
                this.emailRequest = new SendGridEmailRequest();
            }

            if (this.emailRequest.personalizations == null)
            {
                this.emailRequest.personalizations = new List<Personalization>();
            }

            if (this.emailRequest.content == null)
            {
                this.emailRequest.content = new List<Content>();
            }

            this.InitWebRequest();
        }

        protected SendGridEmail(From from, Personalization personalization, Content content)
        {
            if (this.emailRequest == null)
            {
                this.emailRequest = new SendGridEmailRequest();
            }

            if (this.emailRequest.personalizations == null)
            {
                this.emailRequest.personalizations = new List<Personalization>();
            }

            if (this.emailRequest.content == null)
            {
                this.emailRequest.content = new List<Content>();
            }

            this.emailRequest.from = from;
            //this.emailRequest.from = "EIM_IPSD@cat.com";
            this.emailRequest.personalizations.Add(personalization);
            this.emailRequest.content.Add(content);
            this.InitWebRequest();
        }

        public void SetEmailRequest(From from, Personalization personalization, Content content)
        {
            this.emailRequest.from = from;
            this.emailRequest.personalizations.Add(personalization);
            this.emailRequest.content.Add(content);
        }

        public string SendEmail()
        {
            using (var streamWriter = new StreamWriter(this.webRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(this.emailRequest);
                streamWriter.Write(json);
            }

            HttpWebResponse response = (HttpWebResponse)this.webRequest.GetResponse();
            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                return "EmailSent";
            }
            else
            {
                return "EmailFailed";
            }

            //           htmlContent = htmlContent.Replace("{​​Message}​​", request.Message).Replace("{​​Notes}​​", request.Notes);

            //           htmlContent = htmlContent.Replace("\n", "");

            //           htmlContent = htmlContent.Replace("\t", "");

            //           htmlContent = htmlContent.Replace("\"", "");

            //           Models.BusinessEntities.V4.Content content = new Models.BusinessEntities.V4.Content

            //           {​​

            //value = htmlContent, // "<strong>" + request.Message + "</strong>" + "<div>" + request.Notes + "</div>",

            //               type = "text/html"

            //           }​​;



            //emailRequest.content = new List<Models.BusinessEntities.V4.Content>();

            //emailRequest.content.Add(content);
        }

        public void SendMailContents()
        {
            var lstOrders = GetSPData();
            string htmlcontent = string.Empty;
            SendGridEmailRequest emailRequest = new SendGridEmailRequest();
            List<Personalization> lstPersonalization = new List<Personalization>();
            List<Content> lstContents = new List<Content>();
            List<To> lstTo = new List<To>();
            List<CC> lstCC = new List<CC>();
            Personalization personalization = new Personalization();
            From from = new From();

            from.email = "EIM_IPSD@cat.com";
            this.emailRequest.from = from;



            To to = new To();
            //to.email = "sashi_bacchu@srivensolutions.com";
            //to.email = "Bacchu_Sashi@perkins.com";
            to.email = "manohar_yeresi@srivensolutions.com";

            lstTo.Add(to);
            personalization.to = lstTo;
            CC cc = new CC();
            //cc.email = "manohar_yeresi@srivensolutions.com";
            cc.email = "manoyeresi@gmail.com";
            //cc.email = "Yeresi_Manohar@perkins.com";

            //CC cc1 = new CC();
            //cc1.email = "Basaramani_Amarnath_X1@perkins.com";

            lstCC.Add(cc);
            //lstCC.Add(cc1);

            personalization.cc = lstCC;
            personalization.subject = "New eCommerce B2B Order O-0000000069";

            lstPersonalization.Add(personalization);
            this.emailRequest.personalizations = lstPersonalization;

            Content content = new Content();
            htmlcontent = htmlcontent + "New eCommerce B2B Order O-0000000069" + "<br><br>";
            htmlcontent = htmlcontent+"<table style='width:80%;border: 1px solid black;border-collapse: collapse;'>";
            htmlcontent = htmlcontent + "<tr>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:1px;'>Customer Account Number</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Order Number</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Order Date</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Grand Total</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Currency</th></tr>";


            htmlcontent = htmlcontent + "<tr>";
            htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + lstOrders[0].AccountNumber + "</td>";
            htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + lstOrders[0].OrderNumber + "</td>";
            htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + ((DateTime)lstOrders[0].OrderDate).ToString("dd/MM/yyyy") + "</td>";
            htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + ((decimal)lstOrders[0].SubTotal).ToString("F") + "</td>";
            htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + lstOrders[0].Currency + "</td>";

            htmlcontent = htmlcontent + "</table>";
            htmlcontent = htmlcontent + "<br><br><br>";

            htmlcontent = htmlcontent+ "<table style='width:100%;border: 1px solid black;border-collapse: collapse;'>";
            htmlcontent = htmlcontent + "<tr>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Item Title</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Sku</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Quantity</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Item Price</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Address Line1</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Address Line2</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Address Line3</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Address City</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>State Code</th>";
            htmlcontent = htmlcontent + "<th style='border: 1px solid black;border-collapse: collapse;padding:5px;'>Address Zip</th></tr>";

            foreach (var order in lstOrders)
            {
                htmlcontent = htmlcontent + "<tr>";
                htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + order.ItemTitle + "</td>";
                htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + order.ItemSku + "</td>";
                htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + order.Quantity + "</td>";
                htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + ((decimal)order.ItemPrice).ToString("F") + "</td>";
                htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + order.AddressLine1 + "</td>";
                htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + order.AddressLine2 + "</td>";
                htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + order.AddressLine3 + "</td>";
                htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + order.AddressCity + "</td>";
                htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + order.StateCode + "</td>";
                htmlcontent = htmlcontent + "<td style='text-align:center;border: 1px solid black;border-collapse: collapse;padding:5px;'>" + order.AddressZip + "</td>";
                htmlcontent = htmlcontent + "</tr>";
            }
            htmlcontent = htmlcontent + "</table>";

            content.value = htmlcontent;
            content.type = "text/html";
            lstContents.Add(content);
            this.emailRequest.content = lstContents;
            this.InitWebRequest();
        }


        public List<Orders> GetSPData()
        {
            List<Orders> lstOrders = new List<Orders>();
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-S431IC5;Initial Catalog=Sriven;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_GetOrder", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Orders orders = new Orders()
                {
                    //AccountNumber = sqlDataReader.IsDBNull(0) ? string.Empty : sqlDataReader.GetString(0),
                    //OrderNumber = sqlDataReader.IsDBNull(1) ? string.Empty : sqlDataReader.GetString(1),
                    //OrderDate = sqlDataReader.IsDBNull(2) ? (DateTime?)null : sqlDataReader.GetDateTime(2),
                    //ItemTitle = (sqlDataReader.IsDBNull(3) || sqlDataReader.GetString(3) == "NULL") ? string.Empty : sqlDataReader.GetString(3),
                    //ItemSku = sqlDataReader.IsDBNull(4) ? string.Empty : sqlDataReader.GetString(4),
                    //Quantity = sqlDataReader.IsDBNull(5) ? (int?)null : sqlDataReader.GetInt32(5),
                    //Currency = sqlDataReader.IsDBNull(6) ? string.Empty : sqlDataReader.GetString(6),
                    //ItemPrice = sqlDataReader.IsDBNull(7) ? (decimal?)null : sqlDataReader.GetDecimal(7),
                    //SubTotal = sqlDataReader.IsDBNull(8) ? (decimal?)null : sqlDataReader.GetDecimal(8),
                    //AddressLine1 = (sqlDataReader.IsDBNull(9) || sqlDataReader.GetString(9) == "NULL") ? string.Empty : sqlDataReader.GetString(9),
                    //AddressLine2 = sqlDataReader.IsDBNull(10) ? string.Empty : sqlDataReader.GetString(10),
                    //AddressLine3 = sqlDataReader.IsDBNull(11) ? string.Empty : sqlDataReader.GetString(11),
                    //AddressCity = sqlDataReader.IsDBNull(12) ? string.Empty : sqlDataReader.GetString(12),
                    //StateCode = sqlDataReader.IsDBNull(13) ? string.Empty : sqlDataReader.GetString(13),
                    //AddressZip = sqlDataReader.IsDBNull(14) ? string.Empty : sqlDataReader.GetString(14),

                    AccountNumber = reader["CUST_ERP_ACCNT_NUMBER"].ToString(),
                    OrderNumber = reader["ORDER_NUMBER"].ToString(),
                    OrderDate = Convert.ToDateTime(reader["ORDER_ORDERDATE"]),//(DateTime?)null : sqlDataReader.GetDateTime(2),
                    ItemTitle = reader["ORDER_SHIP_TITLE"].ToString(),
                    ItemSku = reader["ORDER_SHIP_SKU"].ToString(),
                    Quantity = Convert.ToInt32(reader["ORDER_SHIP_ITEM_QTY"]),
                    Currency = reader["CURRENCY"].ToString(),
                    ItemPrice = Convert.ToDecimal(reader["ORDER_SHIP_ITEMPRICE"]),
                    SubTotal = Convert.ToDecimal(reader["ORDER_GRAND_TOTAL"]),
                    AddressLine1 = reader["ORDER_SPG_ADDRESSLINE1"].ToString(),
                    AddressLine2 = reader["ORDER_SPG_ADDRESSLINE2"].ToString(),
                    AddressLine3 = reader["ORDER_SPG_ADDRESSLINE3"].ToString(),
                    AddressCity = reader["ORDER_SPG_ADDRESS_CITY"].ToString(),
                    StateCode = reader["ORDER_SPG_STATECODE"].ToString(),
                    AddressZip = reader["ORDER_SPG_ADDRESS_ZIP"].ToString(),
                    B2BEECOMMERCEEMAIL=reader["B2B_ECOMMERCE_EMAIL"].ToString(),
                };
                lstOrders.Add(orders);
            }
            return lstOrders;
        }
    }







}
