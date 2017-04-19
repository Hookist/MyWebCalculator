using Calc.DataLayer.DBLayer;
using Calc.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace WebCalc.Controllers
{
    /// <summary>
    ///  Класс контроллер Calculator
    ///  выполняет операции калькулятора
    /// </summary>
    public class CalculatorController : Controller
    {

        IGenericRepository<History> repository;

        /// <summary>
        /// Библиотека Ninject проверяет конструктор CalculatorController и обнаруживает, 
        /// что он объявляет зависимость от интерфейса IGenericRepository<History>, для которой предусмотрена привязка.
        /// </summary>
        public CalculatorController(IGenericRepository<History> repository)
        {
            this.repository = repository; 
        }

        // GET: Calculator
        /// <summary>
        /// Этот метод действия создает экземпляр перечисления Операций
        /// и отправляет View клиенту
        /// </summary>
        /// <returns> Views.Calculator.Index.cshtml </returns>
        public ActionResult Index()
        {
            var o = new Operator();
            return View(o);
        }

        // POST: Calculator
        /// <summary>
        /// Этот метод действия обрабатывает поступившие данные
        /// от клиента возращая результат (без перезагрузки страницы
        /// с помощью частичного представления)
        /// </summary>
        /// <param name="x"> Первое число </param>
        /// <param name="_operator"> экземпляр операции котору нужно сделвть над 2 числами </param>
        /// <param name="y"> Второе число </param>
        /// <returns> Частичное представление </returns>
        [HttpPost]
        async public Task<ActionResult> Index(string x, 
            Operator _operator,
            string y)
        {
            /// записывает поступившые данные в динамический словарь
            /// чтобы можно было использовать их в частичном представлении
            ViewBag.id = (int)_operator;
            ViewBag.x = x;
            ViewBag.y = y;

            // переменные для поступивших 2 чисел
            float valueX, valueY;

            
            /// Попытка разбора строки в число.
            /// Если не удаеться разобрать строку то возвращаеться частичное
            /// представление пользователю с выводом ошибки.
            /// В противном случае записывает числа в переменные
            if(!float.TryParse(x, out valueX) || !float.TryParse(y, out valueY))
            {
                ViewBag.result = $"Parse failed, value {x} or {y} is not a number. ";
                return PartialView();
            }

            /// Проверка поступившей операции с ее выполнением
            switch (_operator)
            {
                case Operator.Addition :
                    ViewBag.result = Addition(valueX, valueY);
                    break;
                case Operator.Subtraction :
                    ViewBag.result = Subtraction(valueX, valueY);
                    break;
                case Operator.Multiplication :
                    ViewBag.result = Multiplication(valueX, valueY);
                    break;
                case Operator.Division :
                    if (IsZero(valueX))
                    {
                        ViewBag.result = "You can't division on zero";
                        return PartialView();
                    }
                    ViewBag.result = Division(valueX, valueY);
                    break;
                default :
                    break;
            }

            // берем имя с атрибута Display у _operator что бы после записать в базу для удобочитаемости
            var type = typeof(Operator);
            var memInfo = type.GetMember(Enum.GetName(typeof(Operator), _operator));
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            var name = ((DisplayAttribute)attributes[0]).GetName();

            // асинхронный вызов функции добовления нового Лога
            await AddLogToDatabase(ViewBag.result, valueX, valueY, name);

            return PartialView();
        }

        /// <summary>
        /// Этот метод передаёт принимает данные для нового Лога и добавляет его 
        /// в базу данных 
        /// </summary>
        /// <param name="result">результат сложения</param>
        /// <param name="_operator"> конкретная Операция</param>
        /// <param name="valueX"> Первое число </param>
        /// <param name="valueY"> Второе число </param>
        /// <returns>Возвращает Task<int> который можно вызвать асинхронно </returns>
        Task<int> AddLogToDatabase(float result, float valueX, float valueY, string _operator) 
        {

            History newRecord = new History() { LogTime = DateTime.Now, Description = $"{valueX} {_operator} {valueY} = {result}"  };
            return repository.AddOrUpdateAsync(newRecord);
        }

        /// <summary>
        /// Этот метод проверяет являеться ли число 0
        /// </summary>
        /// <param name="x">число с плавающей точкой которое нужно проверить</param>
        /// <returns>Результат проверки</returns>
        bool IsZero(float x) => x == 0; // проверка являеться ли число 0
        /// <summary>
        /// Этот метод возвращает результат сложения 2 чисел с плавающей точкой
        /// </summary>
        /// <param name="x">Первое число</param>
        /// <param name="y">Второе число</param>
        /// <returns>Результат сложения</returns>
        float Addition(float x, float y) => x + y;
        /// <summary>
        /// Этот метод возвращает результат вычитания 2 чисел с плавающей точкой
        /// </summary>
        /// <param name="x">Первое число</param>
        /// <param name="y">Второе число</param>
        /// <returns>Результат вычитания</returns>
        float Subtraction(float x, float y) => x - y;
        /// <summary>
        /// Этот метод возвращает результат умножения 2 чисел с плавающей точкой
        /// </summary>
        /// <param name="x">Первое число</param>
        /// <param name="y">Второе число</param>
        /// <returns>Результат умножения</returns>
        float Multiplication(float x, float y) => x * y;
        /// <summary>
        /// Этот метод возвращает результат деления 2 чисел с плавающей точкой
        /// </summary>
        /// <param name="x">Первое число</param>
        /// <param name="y">Второе число</param>
        /// <returns>Результат деления</returns>
        float Division(float x, float y) => x / y;
        
      
    }

    /// <summary>
    /// Перечисление операций которые можно производить над числами
    /// </summary>
    public enum Operator
    {
        [Display(Name = "+")]
        Addition = 1,
        [Display(Name = "-")]
        Subtraction = 2,
        [Display(Name = "*")]
        Multiplication = 3,
        [Display(Name = "/")]
        Division = 4
    }

}