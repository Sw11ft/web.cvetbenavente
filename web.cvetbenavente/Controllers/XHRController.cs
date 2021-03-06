﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.cvetbenavente.Data;
using Microsoft.AspNetCore.Authorization;
using static web.cvetbenavente.Models.Enums;
using Microsoft.EntityFrameworkCore;
using static web.cvetbenavente.Services.Helpers;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web.cvetbenavente.Controllers
{
    [Authorize]
    public class XHRController : Controller
    {
        private readonly ApplicationDbContext db;

        public XHRController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: /<controller>/
        public IActionResult Registos(bool clientes = true, bool animais = true, bool especies = true)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") {
                int? nrClientes = (clientes) ? db.Clientes.Where(x => x.Active).Count() : (int?)null;
                int? nrAnimais = (animais) ? db.Animais.Where(x => x.Cliente.Active && !x.Removido).Count() : (int?)null;
                int? nrEspecies = (especies) ? db.Especies.Where(x => x.Active).Count() : (int?)null;

                return Json(new { clientes = nrClientes, animais = nrAnimais, especies = nrEspecies });
            }

            return Json(new { error = true, message = "O pedido não foi efetuado através de XHR." });
        }

        public IActionResult Generos()
        {
            var generos = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(typeof(Genero)))
            {
                generos.Add((int)item, item.ToString());
            }

            return Json(generos);
        }

        public IActionResult Search(string q)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (!string.IsNullOrWhiteSpace(q))
                {
                    q = (q.Trim());

                    var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
                    var options = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace;

                    var cl = db.Clientes
                            .Where(x => compareInfo.IndexOf(x.Nome ?? "", q, options) > -1 ||
                                        compareInfo.IndexOf(x.Morada ?? "", q, options) > -1 ||
                                        compareInfo.IndexOf(x.Localidade ?? "", q, options) > -1 ||
                                        compareInfo.IndexOf(x.CodPostal ?? "", q, options) > -1 ||
                                        compareInfo.IndexOf(x.Contacto ?? "", q, options) > -1 ||
                                        compareInfo.IndexOf(x.Observacoes ?? "", q, options) > -1)
                            .Where(x => x.Active)
                            .Select(x => new
                            {
                                id = x.Id,
                                nome = x.Nome,
                                contacto = x.Contacto,
                                morada = x.Morada,
                                codpostal = x.CodPostal,
                                localidade = x.Localidade
                            })
                            .OrderBy(x => x.nome)
                            .ToList();

                    var an = db.Animais
                                .Include(x => x.Cliente)
                                .Include(x => x.Especie)
                                .Where(x => compareInfo.IndexOf(x.Nome ?? "", q, options) > -1 ||
                                            compareInfo.IndexOf(x.Especie.Nome ?? "", q, options) > -1 ||
                                            compareInfo.IndexOf(x.Especie.NomeF ?? "", q, options) > -1 ||
                                            compareInfo.IndexOf(x.Observacoes ?? "", q, options) > -1 ||
                                            compareInfo.IndexOf(x.Cliente.Nome ?? "", q, options) > -1 ||
                                            compareInfo.IndexOf(x.Cliente.Observacoes ?? "", q, options) > -1)
                                .Where(x => !x.Removido && x.Cliente.Active)
                                .Select(x => new
                                {
                                    id = x.Id,
                                    nome = x.Nome,
                                    especie = x.Genero == Genero.M ? x.Especie.Nome : x.Especie.NomeF,
                                    genero = x.Genero,
                                    imagem = x.Especie.Imagem,
                                    cliente = x.Cliente.Nome,
                                    contacto = x.Cliente.Contacto,
                                    idCliente = x.Cliente.Id
                                })
                                .OrderBy(x => x.cliente).ThenBy(x => x.nome)
                                .ToList();

                    var esp = db.Especies
                                    .Where(x => (x.Nome).Contains(q) ||
                                                (x.NomeF).Contains(q))
                                    .Where(x => x.Active)
                                    .Select(x => new
                                    {
                                        id = x.Id,
                                        nome = x.Nome,
                                        nomeF = x.NomeF,
                                        imagem = x.Imagem
                                    }).ToList();

                    return Json(new { clientes = cl, animais = an, especies = esp });
                }

                return null;
            }
            else
            {
                return Json(new { error = true, message = "O pedido não foi efetuado através de XHR." });
            }
        }
    }
}
