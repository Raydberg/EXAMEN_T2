using EXAMEN_T2.Datos;
using EXAMEN_T2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EXAMEN_T2.Controllers
{
    public class ProductoController : Controller
    {
        private readonly AppDbContext _contexto;
        private readonly IWebHostEnvironment entorno;

        public ProductoController(AppDbContext context,IWebHostEnvironment entorno)
        {
            _contexto = context;
            this.entorno = entorno;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _contexto.Productos.OrderByDescending(p=>p.IdProducto).ToListAsync());
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear (ProductoDTO productoDTO)
        {
            if (productoDTO.ArchivoImagen == null)
            {
                ModelState.AddModelError("Archivo de Imagen", "El archivo de imagen es obligatorio");
            }
            if (!ModelState.IsValid)
            {
                return View(productoDTO);
            }
            string nuevoNombreArchivo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            nuevoNombreArchivo += Path.GetExtension(productoDTO.ArchivoImagen.FileName);

            string rutaCompletaImagen = entorno.WebRootPath + "/imagenes/" + nuevoNombreArchivo;
            using (var stream = System.IO.File.Create(rutaCompletaImagen))
            {
                productoDTO.ArchivoImagen.CopyTo(stream);
            }
            Producto p = new Producto()
            {
                Nombre = productoDTO.Nombre,
                Marca = productoDTO.Marca,
                Categoria = productoDTO.Categoria,
                Precio = productoDTO.Precio,
                Descripcion = productoDTO.Descripcion,
                Imagen = nuevoNombreArchivo,
                FechaCreacion = DateTime.Now,
            };
            _contexto.Productos.Add(p);
            _contexto.SaveChanges();
                return RedirectToAction("Index", "Producto");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var p = _contexto.Productos.Find(id);
            if(p == null) {

                return RedirectToAction("Index", "Producto");
            }
            var productoDTO = new Producto() {
                Nombre = p.Nombre,
                Marca = p.Marca,
                Categoria = p.Categoria,
                Precio = p.Precio,
                Descripcion = p.Descripcion,
            };
            ViewData["IdProducto"] = p.IdProducto;
            ViewData["Imagen"] = p.Imagen;
            ViewData["FechaCreacion"] = p.FechaCreacion.ToString("MM/dd/yyyy");
            return View(productoDTO);

        }
        [HttpPost]
        public  IActionResult Editar(int id,ProductoDTO productoDTO)
        {
            var p = _contexto.Productos.Find(id);
            if (p==null)
            {
                return RedirectToAction("Index","Producto");
            }
            if (!ModelState.IsValid)
            {
                ViewData["IdProducto"]=p.IdProducto;
                ViewData["Imagen"]=p.Imagen;
                ViewData["FechaCreacion"]=p.FechaCreacion.ToString("MM/dd/yyyy");
                return View(productoDTO);
            }
            string nuevoNombreArchivo = p.Imagen;
            if (productoDTO.ArchivoImagen != null)
            {
                nuevoNombreArchivo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                nuevoNombreArchivo += Path.GetExtension(productoDTO.ArchivoImagen.FileName);
            }
            string rutaCompletaImagenAntigua = entorno.WebRootPath + "/imagenes/" + p.Imagen;
            System.IO.File.Delete(rutaCompletaImagenAntigua);
            p.Nombre=productoDTO.Nombre;
            p.Marca=productoDTO.Marca;
            p.Categoria=productoDTO.Categoria;
            p.Precio=productoDTO.Precio;
            p.Descripcion=productoDTO.Descripcion;
            p.Imagen = nuevoNombreArchivo;
            _contexto.SaveChanges();
            return RedirectToAction("Index","Producto");
        }

        public IActionResult Eliminar(int id)
        {
            var p = _contexto.Productos.Find(id);
            if (p==null)
            {
                return RedirectToAction("index", "Producto");
            }
            string rutaCompletaImagen = entorno.WebRootPath + "/imagenes/" + p.Imagen;
            System.IO.File.Delete(rutaCompletaImagen);
            _contexto.Productos.Remove(p);
            _contexto.SaveChanges(true);
            return RedirectToAction("Index", "Producto");
        }






    }
}
