using System;
using System.Net.Mail;
using System.Reflection;
using System.Web.Mvc;
using LesserToUmbraco.Models;
using log4net;
using Umbraco.Web.Mvc;

namespace LesserToUmbraco.Controllers
{
    public class ContactController : SurfaceController
    {
        private string GetViewPath(string name)
        {
            return $"/Views/Partials/Contact/{name}.cshtml";
        }

        // GET: Contact
        [HttpGet]
        public ActionResult RenderForm()
        {
            return PartialView(GetViewPath("_ContactForm"), new ContactViewModel());
        }

        [HttpPost]
        public ActionResult RenderForm(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return RenderForm();

            return PartialView(GetViewPath("_ContactForm"), model);
        }

        [HttpPost]
        public ActionResult SubmitForm(ContactViewModel model)
        {
            bool success = false;
            if (ModelState.IsValid)
                success = SendEmail(model);

            return PartialView(GetViewPath(success ? "_Success" : "_Error"));
        }

        private bool SendEmail(ContactViewModel model)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                var email = new MailMessage();
                email.Subject = $"Enquiry from {model.Name} - {model.Email}";
                email.Body = model.Message;

                var toAddress = System.Web.Configuration.WebConfigurationManager.AppSettings["ContactFormEmailTo"];
                email.To.Add(new MailAddress(toAddress));

                var fromAddress = System.Web.Configuration.WebConfigurationManager.AppSettings["ContactFormEmailFrom"];
                email.From = new MailAddress(fromAddress);

                var smtpClient = new SmtpClient();
                // set it up in IIS  --> website--> SMTP E-mail settings
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtpClient.Send(email);

                return true;
            }
            catch (Exception e)
            {
                log.Error("Contact Form Error", e);
                return false;
            }
        }
    }
}