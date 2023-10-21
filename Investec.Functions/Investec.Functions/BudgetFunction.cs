using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Communication.Email;
using Azure;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Investec.Functions;

public static class BudgetFunction
{
    [FunctionName("BudgetFunction")]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        Transaction data = JsonConvert.DeserializeObject<Transaction>(requestBody);
        await SendNotification();
        return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(data));
    }

    private static async Task SendNotification()
    {
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from = new SendGrid.Helpers.Mail.EmailAddress("donotreply55664@gmail.com", "Investec");
        var subject = "Sending with SendGrid is Fun";
        var to = new SendGrid.Helpers.Mail.EmailAddress("daniel@agilebridge.co.za", "Danie");
        var plainTextContent = "and easy to do anywhere with C#.";
        var htmlContent = "<strong>and easy to do anywhere with C#.</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);
    }
}