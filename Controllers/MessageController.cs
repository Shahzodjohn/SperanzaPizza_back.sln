using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Data.DTO.Messages;
using SperanzaPizzaApi.Infrastructure.Filters;
using SperanzaPizzaApi.Models;
using SperanzaPizzaApi.Services.Messages;

namespace SperanzaPizzaApi.Controllers
{
    public class MessageController : ControllerBase
    {
        private readonly dbPizzaContext _context;
        private readonly MessageService _messageService;

        public MessageController(dbPizzaContext context, MessageService service)
        {
            _context = context;
            _messageService = service;
        }

        [HttpPost("send")]
        public async Task<ActionResult> SendClientsMessage([FromBody]CreateNewMessageParams request)
        {
            //validation
            if (request.clientname == null || request.email == null || request.message == null)
                return BadRequest(new {message = "Заполнены не все данные"});
            DmClientMessage entity = new DmClientMessage{
                ClientName = request.clientname,
                Email = request.email,
                Phone = request.phone,
                Message = request.message,
                CreatedDate = DateTime.Now
            };
            await _context.DmClientMessages.AddAsync(entity);
            var success = await _messageService.SendMessage(request);
            if (success) 
                return Ok(new {message = "Сообщение успешно отправлено"});
            else
                return BadRequest(new {message = "Не удалось отправить сообщение"});
        }
    }
}