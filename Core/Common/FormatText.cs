using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Core.Common
{
    public sealed class FormatText
    {
        private static readonly FormatText instance = new FormatText();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static FormatText()
        {
        }

        private FormatText()
        {
        }

        public static FormatText Instance
        {
            get
            {
                return instance;
            }
        }

        public string GetDateTimeConversion(string dateTime)
        {

            var result = System.DateTime.ParseExact(dateTime, "MM/dd/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToString();

            result.ToString();
            return result;
        }

        public DateTime ChangeTimeZone(DateTime utcTime, string timeZone = "AUS Eastern Standard Time")
        {
            try
            {
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cstZone);
                return cstTime;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Email Template
        /// </summary>
        /// <param name="header">National Parks</param>
        /// <param name="title">Password Recovery</param>
        /// <param name="content">Please click the link below to change your password.
        /// </param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public string EmailTemplate(string header, string title, string Code, string footer)
        {

            string template = @"
<table width='100%' height='100%' border='0' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF' style='margin: 0px; background-color: #FFFFFF; font-size: 12px; text-decoration: none;'> <tr> <td valign='top'> <table width='100%' border='0' cellspacing='0' cellpadding='0'>
<tr> 
<td>
<table width='600px' border='0' align='left' cellpadding='0' cellspacing='0'>
<tr> 
<td>
<table width='100%' border='0' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF'> 
<tr><td><hr size='1' noshade style='border-top:1px dotted; color:#EAEAEA'></td> </tr>
<tr> <td style='font-size: 34px; padding-top: 10px; padding-right: 30px; padding-bottom: 0px;font-family: Century Gothic, sans-serif; padding-left: 20px; font-weight: normal; color: #4BA046;'><p><strong> Password Reset Request</strong></p></td> </tr>
<tr> <td>
<p style='font-size:14px;margin-left: 20px;margin-right:20px;margin-top:20px; color:#5A5757;font-family: Century Gothic, sans-serif;'>
Dear User,</p> 
<p style='font-size: 14px; margin-left: 20px;margin-right:20px;margin-top:10px;font-family: Century Gothic, sans-serif;color:#5A5757'>
 We received a request to reset your password. If you made this request, you can confirm it by entering the verification Code .Your UpSurge Verification Code is :" + Code + @"
 </p> <p style='font-size: 14px; margin-left: 20px;margin-right:20px;margin-top:10px;font-family: Century Gothic, sans-serif;color:#5A5757'>If you didn't request this password reset, it's ok to ignore this mail.</p> 
 <p style='font-size: 14px;margin-left: 20px;margin-right:20px;font-family: Century Gothic, sans-serif;margin-bottom:20px;color:#5A5757'> Warm Regards,<br> UpSurge Team</p></td> </tr> </table></td> </tr> <tr> <td><hr size='1' noshade style='border-top:1px dotted; color:#EAEAEA'></td> </tr>
<tr> <td style='font-size: 12px; color: #999999; padding-top: 0px;font-family: Century Gothic, sans-serif;'> <p align='center'> This is an auto-generated email. Please do not reply to this email. <br> <p align='center'>Copyright © 2021 UpSurge App.. All rights reserved. </p></td> 
</tr>
<tr><td> </td></tr> 
</table>
</td> 
</tr>
</table>
</td>
</tr> 
</table>";
            return template;
        }

        public static string ParagraphHtml(string Paragraph)
        {
            return @"<p style=""margin-bottom:15px;"">
            	" + Paragraph + @"</p>
            ";
        }
        public static string ParagraphHtmlMar10(string Paragraph)
        {
            return @"<p style=""margin-bottom:10px;"">
				" + Paragraph + @"</p>
			";
        }
        public static string ParagraphHtmlMar5(string Paragraph)
        {
            return @"<p style=""margin-bottom:5px;"">
				" + Paragraph + @"</p>
			";
        }

        public static string ParagraphHtmlMar2(string Paragraph)
        {
            return @"<p style=""margin-bottom:2px;"">
				" + Paragraph + @"</p>
			";
        }

        public string LinkHtml(string Link)
        {
            return @"<p style=""margin-bottom:15px;"">
            	<a href=""" + Link + @""" style=""color:#2f82de;font-weight:bold;text-decoration:none;"">" + Link.Substring(0, Link.Length > 50 ? 50 : Link.Length) + @"</a></p>
            ";
        }

     
        /// <summary>
        /// get current userid
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// get the current username
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// get the current user role name
        /// </summary>
        /// <returns></returns>


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        /// <summary>
        /// Export to CSV
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>

        /// Export To Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>


        public static string EmailTemplate(string header, string content, string footer)
        {
            string template = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html><head><title></title><meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""><style type=""text/css"">
@media only screen and (max-device-width: 480px) { 
table[class=w0], td[class=w0] { width: 0 !important; }
table[class=w10], td[class=w10], img[class=w10] { width:10px !important; }
table[class=w15], td[class=w15], img[class=w15] { width:5px !important; }
table[class=w30], td[class=w30], img[class=w30] { width:10px !important; }
table[class=w60], td[class=w60], img[class=w60] { width:10px !important; }
table[class=w125], td[class=w125], img[class=w125] { width:80px !important; }
table[class=w130], td[class=w130], img[class=w130] { width:55px !important; }
table[class=w140], td[class=w140], img[class=w140] { width:90px !important; }
table[class=w160], td[class=w160], img[class=w160] { width:180px !important; }
table[class=w170], td[class=w170], img[class=w170] { width:100px !important; }
table[class=w180], td[class=w180], img[class=w180] { width:80px !important; }
table[class=w195], td[class=w195], img[class=w195] { width:80px !important; }
table[class=w220], td[class=w220], img[class=w220] { width:80px !important; }
table[class=w240], td[class=w240], img[class=w240] { width:180px !important; }
table[class=w255], td[class=w255], img[class=w255] { width:185px !important; }
table[class=w275], td[class=w275], img[class=w275] { width:135px !important; }
table[class=w280], td[class=w280], img[class=w280] { width:135px !important; }
table[class=w300], td[class=w300], img[class=w300] { width:140px !important; }
table[class=w325], td[class=w325], img[class=w325] { width:95px !important; }
table[class=w360], td[class=w360], img[class=w360] { width:140px !important; }
table[class=w410], td[class=w410], img[class=w410] { width:180px !important; }
table[class=w470], td[class=w470], img[class=w470] { width:200px !important; }
table[class=w580], td[class=w580], img[class=w580] { width:280px !important; }
table[class=w640], td[class=w640], img[class=w640] { width:300px !important; }
table[class*=hide], td[class*=hide], img[class*=hide], p[class*=hide], span[class*=hide] { display:none !important; }
table[class=h0], td[class=h0] { height: 0 !important; }
p[class=footer-content-left] { text-align: center !important; }
#headline p { font-size: 30px !important; }
.article-content, #left-sidebar{ -webkit-text-size-adjust: 90% !important; -ms-text-size-adjust: 90% !important; }
.header-content, .footer-content-left {-webkit-text-size-adjust: 80% !important; -ms-text-size-adjust: 80% !important;}
 } 
#outlook a { padding: 0; }	
body { width: 100% !important; }
.ReadMsgBody { width: 100%; }
.ExternalClass { width: 100%; display:block !important; } 
body { background-color: #ececec; margin: 0; padding: 0; }
img { height: auto; line-height: 100%; outline: none; text-decoration: none; display: block;}
br, strong br, b br, em br, i br { line-height:100%; }
h1, h2, h3, h4, h5, h6 { line-height: 100% !important; -webkit-font-smoothing: antialiased; }
h1 a, h2 a, h3 a, h4 a, h5 a, h6 a { color: blue !important; }
h1 a:active, h2 a:active,  h3 a:active, h4 a:active, h5 a:active, h6 a:active {	color: red !important; }
h1 a:visited, h2 a:visited,  h3 a:visited, h4 a:visited, h5 a:visited, h6 a:visited { color: purple !important; }
  
table td, table tr { border-collapse: collapse; }
.yshortcuts, .yshortcuts a, .yshortcuts a:link,.yshortcuts a:visited, .yshortcuts a:hover, .yshortcuts a span {
color: black; text-decoration: none !important; border-bottom: none !important; background: none !important;
}	
code {
  white-space: normal;
  word-break: break-all;
}
#background-table { background-color: #ececec; }
#top-bar { border-radius:6px 6px 0px 0px; -moz-border-radius: 6px 6px 0px 0px; -webkit-border-radius:6px 6px 0px 0px; -webkit-font-smoothing: antialiased; background-color: #D3A600; color: #d9fffd; }
#top-bar a { font-weight: bold; color: #d9fffd; text-decoration: none;}
#footer { border-radius:0px 0px 6px 6px; -moz-border-radius: 0px 0px 6px 6px; -webkit-border-radius:0px 0px 6px 6px; -webkit-font-smoothing: antialiased; }
body, td { font-family: 'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif; }
.header-content, .footer-content-left, .footer-content-right { -webkit-text-size-adjust: none; -ms-text-size-adjust: none; }
.header-content { font-size: 12px; color: #d9fffd; }
.header-content a { font-weight: bold; color: #d9fffd; text-decoration: none; }
#headline p { color: #d9fffd; font-family: 'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif; font-size: 36px; text-align: center; margin-top:0px; margin-bottom:30px; }
#headline p a { color: #d9fffd; text-decoration: none; }
.article-title { font-size: 18px; line-height:24px; color: #c25130; font-weight:bold; margin-top:0px; margin-bottom:18px; font-family: 'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif; }
.article-title a { color: #c25130; text-decoration: none; }
.article-title.with-meta {margin-bottom: 0;}
.article-meta { font-size: 13px; line-height: 20px; color: #ccc; font-weight: bold; margin-top: 0;}
.article-content { font-size: 13px; line-height: 18px; color: #444444; margin-top: 0px; margin-bottom: 18px; font-family: 'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif; }
.article-content a { color: #3f6569; font-weight:bold; text-decoration:none; }
.article-content img { max-width: 100% }
.article-content ol, .article-content ul { margin-top:0px; margin-bottom:18px; margin-left:19px; padding:0; }
.article-content li { font-size: 13px; line-height: 18px; color: #444444; }
.article-content li a { color: #3f6569; text-decoration:underline; }
.article-content p {margin-bottom: 15px;}
.footer-content-left { font-size: 12px; line-height: 15px; color: #d9fffd; margin-top: 0px; margin-bottom: 15px; }
.footer-content-left a { color: #d9fffd; font-weight: bold; text-decoration: none; }
.footer-content-right { font-size: 11px; line-height: 16px; color: #d9fffd; margin-top: 0px; margin-bottom: 15px; }
.footer-content-right a { color: #d9fffd; font-weight: bold; text-decoration: none; }
#footer { background-color: #D3A600; color: #d9fffd; }
#footer a { color: #d9fffd; text-decoration: none; font-weight: bold; }
#permission-reminder { white-space: normal; }
#street-address { color: #d9fffd; white-space: normal; }
.header-bg{background-color:#D3A600  !important}
.footer-bg{background-color:#D3A600  !important}
.topbar-bg{background-color:#9B7A00 !important}
</style>
<!--[if gte mso 9]>
<style _tmplitem=""134"" >
.article-content ol, .article-content ul {
   margin: 0 0 0 24px;
   padding: 0;
   list-style-position: inside;
}
</style>
<![endif]--><meta name=""robots"" content=""noindex,nofollow"">
<meta property=""og:title"" content=""NewCapmaign"">
</head><body style=""width:100% !important;background-color:#ececec;margin-top:0;margin-bottom:0;margin-right:0;margin-left:0;padding-top:0;padding-bottom:0;padding-right:0;padding-left:0;font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;""><table id=""background-table"" style=""background-color:#ececec;"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
	<tbody><tr style=""border-collapse:collapse;"">
		<td style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" align=""center"" bgcolor=""#ececec"">
			<table class=""w640"" style=""margin-top:0;margin-bottom:0;margin-right:10px;margin-left:10px;"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""640"">
				<tbody><tr style=""border-collapse:collapse;""><td class=""w640"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" height=""20"" width=""640""></td></tr>
				
				<tr style=""border-collapse:collapse;"">
					<td class=""w640"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""640"">
						<table id=""top-bar"" class=""w640 topbar-bg"" style=""border-radius:6px 6px 0px 0px;-moz-border-radius:6px 6px 0px 0px;-webkit-border-radius:6px 6px 0px 0px;-webkit-font-smoothing:antialiased;background-color:#D3A600;color:#d9fffd;"" border=""0"" cellpadding=""0"" cellspacing=""0"" bgcolor=""#D3A600"" width=""640"">
	<tbody><tr style=""border-collapse:collapse;"">
		<td class=""w15"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""15""></td>
		<td class=""w325"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" align=""left"" valign=""middle"" width=""350"">
			<table class=""w325"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""350"">
				<tbody><tr style=""border-collapse:collapse;""><td class=""w325"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" height=""8"" width=""350""></td></tr>
			</tbody></table>
			<div class=""header-content"" style=""-webkit-text-size-adjust:none;-ms-text-size-adjust:none;font-size:12px;color:#D3A600;""></div>
			<table class=""w325"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""350"">
				<tbody><tr style=""border-collapse:collapse;""><td class=""w325"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" height=""8"" width=""350""></td></tr>
			</tbody></table>
		</td>
		<td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td>
		<td class=""w255"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" align=""right"" valign=""middle"" width=""255"">
			<table class=""w255"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""255"">
				<tbody><tr style=""border-collapse:collapse;""><td class=""w255"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" height=""8"" width=""255""></td></tr>
			</tbody></table>
			<table border=""0"" cellpadding=""0"" cellspacing=""0"">
	<tbody><tr style=""border-collapse:collapse;"">
		
		
		
	</tr>
</tbody></table>
			<table class=""w255"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""255"">
				<tbody><tr style=""border-collapse:collapse;""><td class=""w255"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" height=""8"" width=""255""></td></tr>
			</tbody></table>
		</td>
		<td class=""w15"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""15""></td>
	</tr>
</tbody></table>
						
					</td>
				</tr>
				<tr style=""border-collapse:collapse;"">
				<td id=""header"" class=""w640 header-bg"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" align=""center"" bgcolor=""#D3A600"" width=""640"">
	
	<table class=""w640"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""640"">
		<tbody><tr style=""border-collapse:collapse;""><td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td><td class=""w580"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" height=""30"" width=""580""></td><td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td></tr>
		<tr style=""border-collapse:collapse;"">
			<td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td>
			<td class=""w580"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""580"">
				<div id=""headline"" align=""center"">
					<p style=""color:#d9fffd;font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;font-size:36px;text-align:center;margin-top:0px;margin-bottom:30px;"">
						<strong><a href=""#"" style=""color:#d9fffd;text-decoration:none;"">"

    + header + @"</a></strong>
					</p>
				</div>
			</td>
			<td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td>
		</tr>
	</tbody></table>
	
	
</td>
				</tr>
				
				<tr style=""border-collapse:collapse;""><td class=""w640"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" bgcolor=""#ffffff"" height=""30"" width=""640""></td></tr>
				<tr id=""simple-content-row"" style=""border-collapse:collapse;""><td class=""w640"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" bgcolor=""#ffffff"" width=""640"">
	<table class=""w640"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""640"">
		<tbody><tr style=""border-collapse:collapse;"">
			<td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td>
			<td class=""w580"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""580"">
				
						<table class=""w580"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""580"">
							<tbody><tr style=""border-collapse:collapse;"">
								<td class=""w580"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""580"">
									<p class=""article-title"" style=""font-size:18px;line-height:24px;color:#c25130;font-weight:bold;margin-top:0px;margin-bottom:18px;font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;"" align=""left"">" +
    content
    + @"

	<p style=""margin-bottom:15px; font-color:#aaaaaa; font-size:10px;"">
This E-Mail may contain confidential and/or privileged information. It is only intended for the use of the addressee indicated in this message.
</p>


								</td>
							</tr>
							<tr style=""border-collapse:collapse;""><td class=""w580"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" height=""10"" width=""580""></td></tr>
						</tbody></table>
					
			</td>
			<td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td>
		</tr>
	</tbody></table>
</td></tr>
				<tr style=""border-collapse:collapse;""><td class=""w640"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" bgcolor=""#ffffff"" height=""15"" width=""640""></td></tr>
				
				<tr style=""border-collapse:collapse;"">
				<td class=""w640"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""640"">
	<table id=""footer"" class=""w640 footer-bg"" style=""border-radius:0px 0px 6px 6px;-moz-border-radius:0px 0px 6px 6px;-webkit-border-radius:0px 0px 6px 6px;-webkit-font-smoothing:antialiased;background-color:#D3A600;color:#d9fffd;"" border=""0"" cellpadding=""0"" cellspacing=""0"" bgcolor=""#D3A600"" width=""640"">
		<tbody><tr style=""border-collapse:collapse;""><td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td><td class=""w580 h0"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" height=""30"" width=""360""></td><td class=""w0"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""60""></td><td class=""w0"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""160""></td><td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td></tr>
		<tr style=""border-collapse:collapse;"">
			<td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td>
			<td class=""w580"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" valign=""top"" width=""360"">
			<span class=""hide""><p id=""permission-reminder"" class=""footer-content-left"" style=""-webkit-text-size-adjust:none;-ms-text-size-adjust:none;font-size:12px;line-height:15px;color:#d9fffd;margin-top:0px;margin-bottom:15px;white-space:normal;"" align=""left""><span>" + "" /*You're receiving this because you opt for the same*/ + @"</span></p></span>
			<p class=""footer-content-left"" style=""-webkit-text-size-adjust:none;-ms-text-size-adjust:none;font-size:12px;line-height:15px;color:#d9fffd;margin-top:0px;margin-bottom:15px;"" align=""left""></p>
			</td>
			<td class=""hide w0"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""60""></td>
			<td class=""hide w0"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" valign=""top"" width=""160"">
			<p id=""street-address"" class=""footer-content-right"" style=""-webkit-text-size-adjust:none;-ms-text-size-adjust:none;font-size:11px;line-height:16px;margin-top:0px;margin-bottom:15px;color:#d9fffd;white-space:normal;"" align=""right""><span>" + footer + @"</span></p>
			</td>
			<td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td>
		</tr>
		<tr style=""border-collapse:collapse;""><td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td><td class=""w580 h0"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" height=""15"" width=""360""></td><td class=""w0"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""60""></td><td class=""w0"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""160""></td><td class=""w30"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" width=""30""></td></tr>
	</tbody></table>
</td>
				</tr>
				<tr style=""border-collapse:collapse;""><td class=""w640"" style=""font-family:'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;border-collapse:collapse;"" height=""60"" width=""640""></td></tr>
			</tbody></table>
		</td>
	</tr>
</tbody></table>
</body></html>

";

            return template;

        }

    }

}
