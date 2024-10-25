using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssetSentry.Models;

namespace AssetSentry.Controllers
{
    public class DevicesController : Controller
    {
        private AssetSentryContext _context;

        public DevicesController(AssetSentryContext context) => _context = context; 

        public IActionResult DeviceList(string searchString)
        {
            //List<Device> devices = _context.Devices.OrderBy(x => x.Name).ToList();
            //var model = new DeviceViewModel { Devices = devices };
            //return View(model);
            
            DeviceViewModel deviceViewModel = new DeviceViewModel();

            deviceViewModel.Statuses = _context.Statuses.ToList();

            IQueryable<Device> query = _context.Devices.Include(x => x.Status);

            deviceViewModel.Devices = query.OrderBy(x => x.Id).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                var foundDevices = query.Where(s => s.Name!.ToUpper().Contains(searchString.ToUpper())
                || s.Description!.ToUpper().Contains(searchString.ToUpper())
                || s.Status.Name!.ToUpper().Contains(searchString.ToUpper())).ToList();

                deviceViewModel.Devices = foundDevices;
            }

            return View(deviceViewModel);

        }

        public IActionResult AddDevice()
        {
            DeviceViewModel deviceViewModel = new DeviceViewModel();
            deviceViewModel.Statuses = _context.Statuses.ToList();
            return View(deviceViewModel);
        }

        [HttpPost]
        public IActionResult AddDevice(DeviceViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Devices.Add(model.NewDevice);
                _context.SaveChanges();
                return RedirectToAction("DeviceList");
            }
            else
            {
                model.Statuses = _context.Statuses.ToList();
                return View(model);
            }
        }

        //This was part of the template... leaving now for ref if needed

        //// GET: Devices/Create
        //public IActionResult Add()
        //{
        //    return View();
        //}

        //// POST: Devices/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Description")] Device device)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(device);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(device);
        //}

        // GET: Devices/Edit/5
        public async Task<IActionResult> EditDevice(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDevice(int id, [Bind("Id,Name,Description,StatusId")] Device device)
        {
            if (id != device.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DeviceList));
            }
            return View(device);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("DeviceList");
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}
