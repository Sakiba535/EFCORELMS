using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1LMS.Models;

namespace WebApplication1LMS.Controllers
{
    public class StudentsController : Controller
    {
        private readonly LibraryContext _context;

        public StudentsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var data =await _context.students.Include(i=>i.Books).ToListAsync();

            ViewBag.Count = data.Count;

            ViewBag.Total = data.Sum(i => i.Books.Sum(j => j.RentPrice));

            ViewBag.Average = data.Count > 0 ? data.Average(i => i.Books.Sum(j => j.RentPrice)) : 0;



            return View(data);
            
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.students.Include(i=>i.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View(new Student());
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentName,Address,ContactNo,Books")] Student student, string command="")
        {
            if (command == "Add")
            {
                student.Books.Add(new());
                return View (student);

            }
            else if (command.Contains("delete"))
            {
                int idx = int.Parse(command.Split('-')[1]);

                student.Books.RemoveAt(idx);
                ModelState.Clear();
                return View(student);
            }
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.students.Include(i=>i.Books).FirstOrDefaultAsync(i=>i.Id==id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentName,Address,ContactNo,Books")] Student student, string command = "")
        {
            if (command == "Add")
            {
                student.Books.Add(new());
                return View(student);
            }
            else if (command.Contains("delete"))
            {
                int idx = int.Parse(command.Split('-')[1]);
               

                student.Books.RemoveAt(idx);
                ModelState.Clear();
                return View(student);
            }


            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);

                   var bookIdList=student.Books.Select(i=>i.Id).ToList();

                    var delItems=await _context.books.Where(i=>i.StudentId==id).Where(i=>!bookIdList.Contains(i.Id)).ToListAsync();


                    _context.books.RemoveRange(delItems);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.students.Include(i=>i.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var student = await _context.students.FindAsync(id);
            //if (student != null)
            //{
            //    _context.students.Remove(student);
            //}

            await _context.Database.ExecuteSqlAsync($"exec spDeleteStudent {id}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.students.Any(e => e.Id == id);
        }
    }
}
