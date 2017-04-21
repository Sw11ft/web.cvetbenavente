using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace web.cvetbenavente.Controllers
{
    public class ErrorsController : Controller
    {
        [Authorize]
        public IActionResult Index(string StatusCode)
        {
            string message;
            string title;

            switch (StatusCode)
            {
                case "400":
                    message = "O pedido efetuado n�o foi entendido.";
                    title = "Bad Request";
                    break;
                case "401":
                    message = "N�o est� autorizado para ver esta p�gina.";
                    title = "N�o autorizado";
                    break;
                case "403":
                    message = "N�o tem permiss�o para ver esta p�gina.";
                    title = "Proibido";
                    break;
                case "404":
                    message = "Esta p�gina n�o foi encontrada.";
                    title = "P�gina n�o encontrada";
                    break;
                case "500":
                    message = "Ocorreu um erro interno. Contacte a administra��o se isto persistir.";
                    title = "Erro Interno";
                    break;
                default:
                    StatusCode = "404";
                    message = "Esta p�gina n�o foi encontrada.";
                    title = "P�gina n�o encontrada";
                    break;
            }

            ViewBag.StatusCode = StatusCode;
            ViewBag.Message = message;
            ViewBag.Title = title;
            ViewBag.IsAuthenticated = User.Identity.IsAuthenticated;

            return View();
        }
    }
}