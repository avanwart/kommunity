//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootBaronLib.AppSpec.DasKlub.BOL;
using System.Web.Security;
using BootBaronLib.Configs;
using LitS3;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace DasKlub.Controllers
{
    public class PhotosController : Controller
    {
        //
        // GET: /Photo/

        PhotoItems pitms = null;
        public static readonly int pageSize = 25;
        PhotoItem pitm = null;
        MembershipUser mu = null;

        public ActionResult Index()
        {
            pitms = new PhotoItems();
            pitms.UseThumb = true;
            pitms.ShowTitle = false;
            ViewBag.TotalPhotos = pitms.GetPhotoItemsPageWise(1, pageSize);

            return View(pitms);
        }

        public ActionResult Detail(int photoItemID)
        {
            pitm = new PhotoItem(photoItemID);

            StatusUpdates sus = new StatusUpdates();

            StatusUpdate su = new StatusUpdate();

            su.GetStatusUpdateByPhotoID(photoItemID);

            su.PhotoDisplay = true;
            sus.Add(su);

            if (string.IsNullOrWhiteSpace(pitm.Title))
            {
                pitm.Title = String.Format("{0:u}", pitm.CreateDate);
            }

            sus.IncludeStartAndEndTags = false;
            ViewBag.StatusUpdateList = @"<ul id=""status_update_list_items"">" + sus.ToUnorderdList + @"</ul>";

            PhotoItem pitm2 = new PhotoItem();

            pitm2.GetPreviousPhoto(pitm.CreateDate);
            if (pitm2.PhotoItemID > 0)
            {
                pitm2.ShowTitle = false;
                pitm2.UseThumb = true;
                ViewBag.PreviousPhoto = pitm2;
            }

            pitm2 = new PhotoItem();
            pitm2.GetNextPhoto(pitm.CreateDate);

            if (pitm2.PhotoItemID > 0)
            {
                pitm2.ShowTitle = false;
                pitm2.UseThumb = true;
                ViewBag.NextPhoto = pitm2;
            }

            return View(pitm);
        }



        public ActionResult Delete(int photoItemID)
        {
            mu = Membership.GetUser();

            pitm = new PhotoItem(photoItemID);


            if (pitm.CreatedByUserID == Convert.ToInt32(mu.ProviderUserKey))
            {
                S3Service s3 = new S3Service();

                s3.AccessKeyID = AmazonCloudConfigs.AmazonAccessKey;
                s3.SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey;


                pitm.Delete();


                if (!string.IsNullOrEmpty(pitm.FilePathStandard))
                {
                    // delete the existing photos
                    try
                    {
                        if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName,  pitm.FilePathStandard))
                        {
                            s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathStandard);
                        }

                        if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName,  pitm.FilePathRaw))
                        {
                            s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathRaw);
                        }

                        if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName,  pitm.FilePathThumb))
                        {
                            s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathThumb);
                        }

                    }
                    catch
                    {
                        // whatever
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public JsonResult PhotoItems(int pageNumber)
        {
            pitms = new PhotoItems();
            pitms.GetPhotoItemsPageWise(pageNumber, pageSize);

            pitms.ShowTitle = false;
            pitms.UseThumb = true;
            pitms.IncludeStartAndEndTags = false;

            return Json(new
            {
                ListItems = pitms.ToUnorderdList
            });
        }
 
 

    }
}


 
    //public const string Host = "pop.gmail.com";
    //public const int Port = 995;
    //public string Email;
    //public string Password;
    //public const int NoOfEmailsPerPage = 5;
    //public const string SelfLink = "<a href=\"Pop3Client.aspx?page={0}\">{1}</a>";
    //public const string DisplayEmailLink = "<a href=\"DisplayPop3Email.aspx?emailId={0}\">{1}</a>";
    //protected void Page_Load(object sender, EventArgs e)
    //{

    //    int page = 1;
    //    if (Request.QueryString["page"] == null)
    //    {
    //        Response.Redirect("Pop3Client.aspx?page=1");
    //        Response.Flush();
    //        Response.End();
    //    }
    //    else
    //        page = Convert.ToInt32(Request.QueryString["page"]);
    //    try
    //    {
    //        Email = Session["email"].ToString();
    //        Password = Session["pwd"].ToString();
    //    }
    //    catch (Exception ex) { Response.Redirect("Home.aspx"); }
    //    int totalEmails;
    //    List<Email> emails;
    //    string emailAddress;
    //    using (Prabhu.Pop3Client client = new Prabhu.Pop3Client(Host, Port, Email, Password, true))
    //    {
    //        emailAddress = client.Email;
    //        client.Connect();
    //        totalEmails = client.GetEmailCount();
    //        emails = client.FetchEmailList(((page - 1) * NoOfEmailsPerPage) + 1, NoOfEmailsPerPage);
    //    }
    //    int totalPages;
    //    int mod = totalEmails % NoOfEmailsPerPage;
    //    if (mod == 0)
    //        totalPages = totalEmails / NoOfEmailsPerPage;
    //    else
    //        totalPages = ((totalEmails - mod) / NoOfEmailsPerPage) + 1;
    //    for (int i = 0; i < emails.Count; i++)
    //    {
    //        Email email = emails[i];
    //        int emailId = ((page - 1) * NoOfEmailsPerPage) + i + 1;
    //        TableCell noCell = new TableCell();
    //        noCell.CssClass = "emails-table-cell";
    //        noCell.Text = Convert.ToString(emailId);
    //        TableCell fromCell = new TableCell();
    //        fromCell.CssClass = "emails-table-cell";
    //        fromCell.Text = email.From;
    //        TableCell subjectCell = new TableCell();
    //        subjectCell.CssClass = "emails-table-cell";
    //        subjectCell.Style["width"] = "300px";
    //        subjectCell.Text = String.Format(DisplayEmailLink, emailId, email.Subject);
    //        TableCell dateCell = new TableCell();
    //        dateCell.CssClass = "emails-table-cell";
    //        if (email.UtcDateTime != DateTime.MinValue)
    //            dateCell.Text = email.UtcDateTime.ToString();
    //        TableRow emailRow = new TableRow();
    //        emailRow.Cells.Add(noCell);
    //        emailRow.Cells.Add(fromCell);
    //        emailRow.Cells.Add(subjectCell);
    //        emailRow.Cells.Add(dateCell);
    //        EmailsTable.Rows.AddAt(2 + i, emailRow);
    //    }
    //    if (totalPages > 1)
    //    {
    //        if (page > 1)
    //            PreviousPageLiteral.Text = String.Format(SelfLink, page - 1, "Previous Page");
    //        if (page > 0 && page < totalPages)
    //            NextPageLiteral.Text = String.Format(SelfLink, page + 1, "Next Page");
    //    }
    //    EmailFromLiteral.Text = Convert.ToString(((page - 1) * NoOfEmailsPerPage) + 1);
    //    EmailToLiteral.Text = Convert.ToString(page * NoOfEmailsPerPage);
    //    EmailTotalLiteral.Text = Convert.ToString(totalEmails);
    //    EmailLiteral.Text = emailAddress;
    //}
 






    //public const string Host = "pop.gmail.com";
    //public const int Port = 995;
    //public string Email ;
    //public string Password;
    //protected static Regex CharsetRegex = new Regex("charset=\"?(?<charset>[^\\s\"]+)\"?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    //protected static Regex QuotedPrintableRegex = new Regex("=(?<hexchars>[0-9a-fA-F]{2,2})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    //protected static Regex UrlRegex = new Regex("(?<url>https?://[^\\s\"]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    //protected static Regex FilenameRegex = new Regex("filename=\"?(?<filename>[^\\s\"]+)\"?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    //protected static Regex NameRegex = new Regex("name=\"?(?<filename>[^\\s\"]+)\"?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    int emailId = -1;
    //    if (Request.QueryString["emailId"] == null)
    //    {
    //        Response.Redirect("Pop3Client.aspx");
    //        Response.Flush();
    //        Response.End();
    //    }
    //    else
    //        Email = Session["email"].ToString();
    //    Password = Session["pwd"].ToString();
    //        emailId = Convert.ToInt32(Request.QueryString["emailId"]);
    //    Email email = null;
    //    List<MessagePart> msgParts = null;

    //    string messageDeleteID = string.Empty;

    //    using (Prabhu.Pop3Client client = new Prabhu.Pop3Client (Host, Port,Email, Password, true))
    //    {
    //        client.Connect();
    //        email = client.FetchEmail(emailId);
    //        msgParts = client.FetchMessageParts(emailId);

    //       client.Delete(emailId.ToString());
    //    }
    //    if (email == null || msgParts == null)
    //    {
    //        Response.Redirect("Pop3Client.aspx");
    //        Response.Flush();
    //        Response.End();
    //    }
    //    MessagePart preferredMsgPart = FindMessagePart(msgParts, "text/html");
    //    if (preferredMsgPart == null)
    //        preferredMsgPart = FindMessagePart(msgParts, "text/plain");
    //    else if (preferredMsgPart == null && msgParts.Count > 0)
    //        preferredMsgPart = msgParts[0];
    //    string contentType, charset, contentTransferEncoding, body = null;
    //    if (preferredMsgPart != null)
    //    {
    //        contentType = preferredMsgPart.Headers["Content-Type"];
    //        charset = "us-ascii";
    //        contentTransferEncoding =preferredMsgPart.Headers["Content-Transfer-Encoding"];
    //        Match m = CharsetRegex.Match(contentType);
    //        if (m.Success)
    //            charset = m.Groups["charset"].Value;
    //        HeadersLiteral.Text = contentType != null ? "Content-Type: " +contentType + "<br />" : string.Empty;
    //        HeadersLiteral.Text += contentTransferEncoding != null ?"Content-Transfer-Encoding: " +contentTransferEncoding : string.Empty;
    //        if (contentTransferEncoding != null)
    //        {
    //            if (contentTransferEncoding.ToLower() == "base64")
    //                body = DecodeBase64String(charset,preferredMsgPart.MessageText);
    //            else if (contentTransferEncoding.ToLower() =="quoted-printable")
    //                body = DecodeQuotedPrintableString(preferredMsgPart.MessageText);
    //            else
    //                body = preferredMsgPart.MessageText;
    //        }
    //        else
    //            body = preferredMsgPart.MessageText;
    //    }
    //    EmailIdLiteral.Text = Convert.ToString(emailId);
    //    DateLiteral.Text = email.UtcDateTime.ToString(); ;
    //    FromLiteral.Text = email.From;
    //    SubjectLiteral.Text = email.Subject;
    //    BodyLiteral.Text = preferredMsgPart != null ? (preferredMsgPart.Headers["Content-Type"].IndexOf("text/plain") != -1 ?"<pre>" + FormatUrls(body) + "</pre>" : body) : null;
    //    ListAttachments(msgParts);

    //    Bitmap map;

    //    //using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(msgParts[1].MessageText)))
    //    //{
    //    //    using (FileStream fs = File.Create(@"C:\tmp\img.jpg"))
    //    //    {
    //    //        map = (Bitmap)System.Drawing.Image.FromStream(stream);

    //    //        map.Save(fs, System.Drawing.Imaging.ImageFormat.Gif);
    //    //    }
    //    //}

    //    //using (Prabhu.Pop3Client client = new Prabhu.Pop3Client(Host, Port, Email, Password, true))
    //    //{
    //    //    client.Connect();

    //    //    client.Delete(messageDeleteID);
    //    //}

        
    //}
    //protected Decoder GetDecoder(string charset)
    //{
    //    Decoder decoder;
    //    switch (charset.ToLower())
    //    {
    //        case "utf-7":
    //            decoder = Encoding.UTF7.GetDecoder();
    //            break;
    //        case "utf-8":
    //            decoder = Encoding.UTF8.GetDecoder();
    //            break;
    //        case "us-ascii":
    //            decoder = Encoding.ASCII.GetDecoder();
    //            break;
    //        case "iso-8859-1":
    //            decoder = Encoding.ASCII.GetDecoder();
    //            break;
    //        default:
    //            decoder = Encoding.ASCII.GetDecoder();
    //            break;
    //    }
    //    return decoder;
    //}
    //protected string DecodeBase64String(string charset, string encodedString)
    //{
    //    Decoder decoder = GetDecoder(charset);
    //    byte[] buffer = Convert.FromBase64String(encodedString);
    //    char[] chararr = new char[decoder.GetCharCount(buffer,0, buffer.Length)];
    //    decoder.GetChars(buffer, 0, buffer.Length, chararr, 0);
    //    return new string(chararr);
    //}
    //protected string DecodeQuotedPrintableString(string encodedString)
    //{
    //    StringBuilder b = new StringBuilder();
    //    int startIndx = 0;
    //    MatchCollection matches = QuotedPrintableRegex.Matches(encodedString);
    //    for (int i = 0; i < matches.Count; i++)
    //    {
    //        Match m = matches[i];
    //        string hexchars = m.Groups["hexchars"].Value;
    //        int charcode = Convert.ToInt32(hexchars, 16);
    //        char c = (char)charcode;
    //        if (m.Index > 0)
    //            b.Append(encodedString.Substring(startIndx, (m.Index - startIndx)));
    //        b.Append(c);
    //        startIndx = m.Index + 3;
    //    }
    //    if (startIndx < encodedString.Length)
    //        b.Append(encodedString.Substring(startIndx));
    //    return Regex.Replace(b.ToString(), "=\r\n", "");
    //}
    //protected void ListAttachments(List<MessagePart> msgParts)
    //{
    //    bool attachmentsFound = false;
    //    StringBuilder b = new StringBuilder();
    //    b.Append("<ol>");
    //    foreach (MessagePart p in msgParts)
    //    {
    //        string contentType = p.Headers["Content-Type"];
    //        string contentDisposition = p.Headers["Content-Disposition"];
    //        Match m;
    //        if (contentDisposition != null)
    //        {
    //            m = FilenameRegex.Match(contentDisposition);
    //            if (m.Success)
    //            {
    //                attachmentsFound = true;
    //                b.Append("<li>").Append(m.Groups["filename"].Value).Append("</li>");
                    
    //            }
    //        }
    //        else if (contentType != null)
    //        {
    //            m = NameRegex.Match(contentType);

    //            if (m.Success)
    //            {
    //                attachmentsFound = true;
    //                b.Append("<li>").Append(m.Groups["filename"].Value).Append("</li>");
    //            }
    //        }
    //    }
    //    b.Append("</ol>");
    //    if (attachmentsFound)
    //        AttachmentsLiteral.Text = b.ToString();
    //    else
    //        AttachementsRow.Visible = false;
    //}
    //protected MessagePart FindMessagePart(List<MessagePart> msgParts,string contentType)
    //{
    //    foreach (MessagePart p in msgParts)
    //        if (p.ContentType != null && p.ContentType.IndexOf(contentType) != -1)
    //            return p;
    //    return null;
    //}
    //protected string FormatUrls(string plainText)
    //{
    //    string replacementLink = "<a href=\"${url}\">${url}</a>";
    //    return UrlRegex.Replace(plainText, replacementLink);
    //}
