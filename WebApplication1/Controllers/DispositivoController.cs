using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Models;
using WebApplication1.Hubs;
namespace WebApplication1.Controllers
{
    public class DispositivoController : Controller

    {
        private readonly DispositivoDBContext _dbContex;
        private readonly IHubContext<MyHub> _hubContext;
        public DispositivoController(DispositivoDBContext dbContext, IHubContext<MyHub> hubContext)
        {
            _dbContex = dbContext;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            // Recuperar todos los dispositivos
            var dispositivos = _dbContex.Dispositivos.ToList();

            // Datos para la gráfica de litros
            ViewBag.Nombres = dispositivos.Select(d => d.Nombre).ToList();
            ViewBag.Litros = dispositivos.Select(d => d.LitrosRegistrados).ToList();

            // Datos para la gráfica de estado (Apagado/Encendido)
            var dispositivosApagados = _dbContex.Dispositivos.Count(d => d.Estatus == EstatusDispositivos.Apagado);
            var dispositivosEncendidos = _dbContex.Dispositivos.Count(d => d.Estatus == EstatusDispositivos.Encendido);

            ViewBag.DispositivosApagados = dispositivosApagados;
            ViewBag.DispositivosEncendidos = dispositivosEncendidos;

            return View(dispositivos);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Dispositivo dispositivo) //Crear Dispositivo (inserccion en la bd)
        {
            if (ModelState.IsValid)
            {

                _dbContex.Dispositivos.Add(dispositivo);
                _dbContex.SaveChanges();

                _hubContext.Clients.All.SendAsync("DispositivoAgregado", "Nuevo dispositivo agregado!");
               /*  _hubContext.Clients.All.SendAsync("DispositivoAgregado", dispositivo.Id, dispositivo.Latitud, dispositivo.Longitud); */

                return RedirectToAction("Index");
            }

            return View(dispositivo);
        }
        [HttpPost]
        public IActionResult Delete(int id)    //Elimina filas mediante boton
        {
            var dispositivo = _dbContex.Dispositivos.Find(id);
            if (dispositivo != null)
            {
                string nombreDelDispositivoEliminado = dispositivo.Nombre;
                DateTime fechaActual = DateTime.Now;
                _dbContex.Dispositivos.Remove(dispositivo);
                _dbContex.SaveChanges();

                _hubContext.Clients.All.SendAsync("DispositivoEliminado", nombreDelDispositivoEliminado, fechaActual.ToString("dd/MM/yyyy HH:mm:ss"));
                // Asumiendo que tienes una instancia de IHubContext


            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CambiarEstado(int id)
        {
            var dispositivo = _dbContex.Dispositivos.FirstOrDefault(d => d.Id == id);
            if (dispositivo == null)
            {
                return NotFound();
            }

            dispositivo.Estatus = dispositivo.Estatus == EstatusDispositivos.Apagado ? EstatusDispositivos.Encendido : EstatusDispositivos.Apagado;
            _dbContex.SaveChanges();

            _hubContext.Clients.All.SendAsync("EstatusCambiado", dispositivo.Nombre, dispositivo.Estatus.ToString());
            _hubContext.Clients.All.SendAsync("ActualizarTablaDispositivos");

            var todosApagados = !_dbContex.Dispositivos.Any(d => d.Estatus == EstatusDispositivos.Encendido);
            if (todosApagados)
            {
                Console.WriteLine("Todos los dispositivos están apagados.");
                _hubContext.Clients.All.SendAsync("TodosApagados", "¡Atención! Todos los dispositivos están apagados. Por favor encienda al menos uno.");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetAllDispositivos()
        {
            var dispositivos = _dbContex.Dispositivos.ToList();
            return Json(dispositivos);
        }

        [HttpGet]
        public JsonResult ObtenerDatosGraficaEstatus()
        {
            var dispositivosApagados = _dbContex.Dispositivos.Count(d => d.Estatus == EstatusDispositivos.Apagado);
            var dispositivosEncendidos = _dbContex.Dispositivos.Count(d => d.Estatus == EstatusDispositivos.Encendido);

            return Json(new { dispositivosApagados, dispositivosEncendidos });
        }
        [HttpGet]
        public JsonResult ObtenerDatosGraficaLitros()
        {
            var dispositivos = _dbContex.Dispositivos.ToList();
            var nombres = dispositivos.Select(d => d.Nombre).ToList();
            var litrosPorDispositivo = dispositivos.Select(d => d.LitrosRegistrados).ToList();

            return Json(new { nombres, litrosPorDispositivo });
        }

        [HttpGet]
        public JsonResult GetTotalLitros()
        {
            var totalLitros = _dbContex.Dispositivos.Sum(d => d.LitrosRegistrados);
            return Json(new { totalLitros });
        }

     

    }
}