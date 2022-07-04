﻿using Microsoft.AspNetCore.Mvc;
using ProjetoAspNetMVC02.Entities;
using ProjetoAspNetMVC02.Interfaces;
using ProjetoAspNetMVC02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC02.Controllers
{
    public class ClientController : Controller
    {
        private IClientRepository _clientRepository;
        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Register(ClientRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new Client();

                    client.Name = model.Name;
                    client.Email = model.Email;
                    _clientRepository.Insert(client);

                    TempData["Messege"] = $"Client {client.Name}, was registred successfully.";
                }
                catch (Exception e)
                {

                    TempData["Messege"] = e.Message;
                }

            }
            return View();

        }

        public IActionResult Consult()
        {
            try
            {
                TempData["Consult"] = _clientRepository.Consult();
            }
            catch (Exception e)
            {

                TempData["Messege"] = e.Message;
            }
            return View();
        }
        [HttpPost]
        public IActionResult Consult(ClientConsultModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    TempData["Consult"] = _clientRepository.ConsultByName(model.Name);
                }
                else
                {
                    TempData["Consult"] = _clientRepository.Consult();
                }

            }
            catch (Exception e)
            {

                TempData["Messege"] = e.Message;
            }
            return View();
        }

        public IActionResult Delete(Guid id)
        {
            try
            {
                //Search client in data base using ID for it.
                var client = _clientRepository.GetByID(id);
                //Deleting client
                _clientRepository.Delete(client);
                TempData["Messege"] = $"Client {client.Name} was deleted successfully";

            }
            catch (Exception e)
            {

                TempData["Messege"] = e.Message;
            }
            return RedirectToAction("Consult");
        }
        public IActionResult Edit(Guid id)
        {
            var model = new ClientEditModel();
            try
            {
                //Search client in data base using ID for it.
                var client = _clientRepository.GetByID(id);

                // Client data to model
                model.ClientID = client.ClientID;
                model.Name = client.Name;
                model.Email = client.Email;

            }
            catch (Exception e)
            {

                TempData["Messege"] = e.Message;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(ClientEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new Client();
                    client.ClientID = model.ClientID;
                    client.Name = model.Name;
                    client.Email = model.Email;
                    _clientRepository.Alter(client);

                    TempData["Messege"] = $"Client {client.Name}, was edited successfully.";
                }
                catch (Exception e)
                {

                    TempData["Messege"] = e.Message;
                }

            }
            return View();
        }
    }
}
