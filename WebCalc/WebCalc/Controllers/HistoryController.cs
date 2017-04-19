using Calc.DataLayer.DBLayer;
using Calc.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebCalc.Controllers
{
    /// <summary>
    ///  Класс контроллер History
    ///  производит операции над History
    /// </summary>
    public class HistoryController : Controller
    {

        IGenericRepository<History> repository;

        /// <summary>
        /// Библиотека Ninject проверяет конструктор CalculatorController и обнаруживает, 
        /// что он объявляет зависимость от интерфейса IGenericRepository<History>, для которой предусмотрена привязка.
        /// </summary>
        public HistoryController(IGenericRepository<History> repository)
        {
            this.repository = repository;
        }

        // GET: Logs
        /// <summary>
        /// Этот метод возвращает History с базы данных
        /// </summary>
        /// <returns>Возвращает страницу Views.Histories.Index.cshtml с History </returns>
        public ActionResult Index()
        {
            var logs = repository.GetAll().Reverse(); 
            return View(logs.ToList());
        }

    }
}
