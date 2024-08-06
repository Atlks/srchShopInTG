global using static mdsj.lib.EmlGgl;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class EmlGgl
    {
        public static async Task gglML(string credentialsPath,string subjectQuery,string saveDir)
        {
            // ----------------------OAuth 2.0 Scopes for Gmail API
            string[] Scopes = { GmailService.Scope.GmailReadonly };
            string ApplicationName = "Gmail API .NET Quickstart";
            //   string credentialsPath = "path/to/your/credentials.json"; // Update the path to your credentials file

            UserCredential credential;

            using (var stream =
                new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                // credPath is a dir not file
                string credPath = "token.json"; //here just   bin/xxxDir/xxtok.json
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }

            // Create Gmail API service
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            //-------------------------------- Define request parameters
            var request = service.Users.Messages.List("me");
            request.Q = $"subject:{subjectQuery}"; // 搜索标题包含指定内容的邮件
            request.LabelIds = "INBOX";
            request.IncludeSpamTrash = false;
            request.MaxResults = 500;   // Messages.Count def is 100

            string pageToken = null;
            do
            {
                request.PageToken = pageToken;
                try
                {
                    ListMessagesResponse response = await processCurPage(saveDir, service, request);
                    pageToken = response.NextPageToken;
                }catch(Exception e)
                {
                    Print(e);
                }             

            } while (pageToken != null); // 继续循环直到没有更多的页码           
            
        }

        private static async Task<ListMessagesResponse> processCurPage(string saveDir, GmailService service, UsersResource.MessagesResource.ListRequest request)
        {
            // Fetch emails
            var response = await request.ExecuteAsync();

            Console.WriteLine("Messages:");
            if (response.Messages != null && response.Messages.Count > 0)
            {
                foreach (var message in response.Messages)
                {
                    var emailInfoRequest = service.Users.Messages.Get("me", message.Id);
                    var emailInfoResponse = await emailInfoRequest.ExecuteAsync();
                    try
                    {
                        foreachMessage(message, emailInfoResponse, saveDir);
                    }
                    catch (Exception e)
                    {
                        Print(e);
                    }

                }
            }

            return response;
        }

        public static void foreachMessage(Google.Apis.Gmail.v1.Data.Message? message, Google.Apis.Gmail.v1.Data.Message emailInfoResponse, string saveDir)
        {
            Print(EncodeJson(message));
            Console.WriteLine($"Message ID: {message.Id}");
            Console.WriteLine($"Snippet: {emailInfoResponse.Snippet}");

            // 获取邮件标题
            var headers = emailInfoResponse.Payload.Headers;
            var subjectHeader = headers.FirstOrDefault(header => header.Name == "Subject");
            var subject = subjectHeader?.Value;

            // 获取邮件正文
            string body = null;
            if (emailInfoResponse.Payload.Body != null)
            {
                //just here is ok
                body = emailInfoResponse.Payload.Body.Data;
                if (emailInfoResponse.Payload.MimeType == "text/plain" || emailInfoResponse.Payload.MimeType == "text/html")
                {
                    // 可能需要解码Base64编码的内容
                    body = DecodeBase64(body);
                    //   hstb.Add("body", body);
                }
            }
            else if (emailInfoResponse.Payload.Parts != null)
            {//her not go,here should mlt file ,hav file att file ,then hava here 
                //if no file ,just body mode
                foreach (var part in emailInfoResponse.Payload.Parts)
                {
                    if (part.MimeType == "text/plain" || part.MimeType == "text/html")
                    {
                        body = part.Body.Data;
                        if (part.MimeType == "text/plain")
                        {
                            // 可能需要解码Base64编码的内容
                            body = DecodeBase64(body);

                        }

                        break;
                    }
                }
            }


            // 打印邮件标题和正文
            System.Console.WriteLine($"Subject: {subject}");
            System.Console.WriteLine($"Body: {Left(body, 200)}");
            SortedList hstb = new SortedList();
            hstb.Add("subject", subject);

            hstb.Add("body", body);//   

            String fname = $"{saveDir}/{ConvertToValidFileName2024(subject)}.htm";
            DateTime now = DateTime.Now;
            string customFormat = now.ToString("yyyy-MM-dd.HHmmss.fff");
            if (!IsExistFil(fname))
                WriteAllText(fname, body);
        }

    }
}
