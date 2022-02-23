using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SperanzaPizzaApi.Data.DTO.Messages;
using SperanzaPizzaApi.Infrastructure.Helpers;

namespace SperanzaPizzaApi.Services.Messages
{
     public class MessageService
     { 
         private MessageSecretsHelperModel _config;

         public MessageService(MessageSecretsHelperModel config)
         {
             _config = config;
         }
        public async Task<bool> SendMessage(CreateNewMessageParams model)
        {
            try{
                 // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.SenderEmail));
                email.To.Add(MailboxAddress.Parse(_config.ReceiverEmail)); //bibi.34@o2.pl
                email.Subject = "Pizzeria Speranza: Wiadomości od klientów";
                
                
                string bodyContent = @"<!DOCTYPE html>
                                        <html>
                                        <head>
                                        <style>
                                            p {
                                                font-size: 20px;
                                                margin-left : 30px;
                                            }
                                            a {
                                                font-weight : bold;
                                            }

                                        </style>
                                        </head>
                                        <body>
                                        <p> Dzień dobry, otrzymałeś nowy list od klientów Pizzeria Speranza</p>";
                bodyContent += $"<p> <a> Imię: </a>  {model.clientname} </p> ";
                bodyContent += $" <p> <a>Telefon:  </a> {model.phone}  </p> ";
                bodyContent += $" <p> <a> Email:  </a> {model.email}</p> ";
                bodyContent += $" <div> <p> <a> Wiadomość: </a> {model.message} </p> </div> </body> </html>";
                email.Body = new TextPart(TextFormat.Html) { 
                    Text = bodyContent };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.SenderEmail, _config.SenderPassword);
                smtp.Send(email);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }
    }
}