using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FincaMVC.Controllers
{
    public class CreateEmp : Controller
    {
        // GET: CreateEmp
        public ActionResult Index()
        {
            return View();
        }

        // GET: CreateEmp/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreateEmp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreateEmp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateEmp/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CreateEmp/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateEmp/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreateEmp/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
