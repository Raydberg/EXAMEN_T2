using EXAMEN_T2.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EXAMEN_T2.Controllers
{
    public class ProductoController : Controller
    {
        private readonly AppDbContext _contexto;
        public ProductoController(AppDbContext contexto)
        {
            _contexto = contexto;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _contexto.Productos.OrderByDescending(p=>p.IdProducto).ToListAsync());
        }




    }
}
