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
                    message = "O pedido efetuado não foi entendido.";
                    title = "Bad Request";
                    break;
                case "401":
                    message = "Não está autorizado para ver esta página.";
                    title = "Não autorizado";
                    break;
                case "403":
                    message = "Não tem permissão para ver esta página.";
                    title = "Proibido";
                    break;
                case "404":
                    message = "Esta página não foi encontrada.";
                    title = "Página não encontrada";
                    break;
                case "500":
                    message = "Ocorreu um erro interno. Contacte a administração se isto persistir.";
                    title = "Erro Interno";
                    break;
                default:
                    StatusCode = "404";
                    message = "Esta página não foi encontrada.";
                    title = "Página não encontrada";
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