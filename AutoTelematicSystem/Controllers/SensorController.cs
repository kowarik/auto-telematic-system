using AutoTelematicSystem.Data;
using AutoTelematicSystem.Data.Entities;
using AutoTelematicSystem.Dtos.Error;
using AutoTelematicSystem.Dtos.SensorType;
using AutoTelematicSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoTelematicSystem.Controllers
{
    [Authorize]
    public class SensorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISensorService _sensorService;

        public SensorController(AppDbContext context, ISensorService carService)
        {
            _context = context;
            _sensorService = carService;
        }

        #region SensorType
        [HttpGet]
        public async Task<IActionResult> SensorTypeList()
        {
            var sensorTypes = await _context.SensorTypes
                .Select(st => new GetSensorTypeDto
                {
                    Id = st.Id,
                    Name = st.Name,
                    Description = st.Description
                })
                .ToListAsync();
            return View(sensorTypes);
        }

        [HttpGet]
        public async Task<IActionResult> SensorTypeDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensorType = await _context.SensorTypes
                .Include(st => st.Errors)
                .Select(st => new GetSensorTypeDto
                {
                    Id = st.Id,
                    Name = st.Name,
                    Description = st.Description,
                    Errors = st.Errors
                        .Select(e => new GetErrorDto
                        {
                            Id = e.Id,
                            Code = e.Code,
                            Description = e.Description
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(st => st.Id == id);

            if (sensorType == null)
            {
                return NotFound();
            }

            return View(sensorType);
        }

        [HttpGet]
        public IActionResult SensorTypeAdd()
        {
            return View(new AddSensorTypeDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SensorTypeAdd(AddSensorTypeDto sensorTypeDto)
        {
            if (ModelState.IsValid)
            {
                var sensorType = new SensorType
                {
                    Name = sensorTypeDto.Name,
                    Description = sensorTypeDto.Description
                };
                await _context.AddAsync(sensorType);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(SensorTypeList));
            }
            return View(sensorTypeDto);
        }

        [HttpGet]
        public async Task<IActionResult> SensorTypeEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensorType = await _context.SensorTypes
                .Select(st => new GetSensorTypeDto
                {
                    Id = st.Id,
                    Name = st.Name,
                    Description = st.Description
                })
                .FirstOrDefaultAsync(st => st.Id == id);
            if (sensorType == null)
            {
                return NotFound();
            }

            return View(sensorType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SensorTypeEdit(int id, GetSensorTypeDto sensorTypeDto)
        {
            if (id != sensorTypeDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var sensorType = await _context.SensorTypes.FindAsync(sensorTypeDto.Id);

                if(sensorType != null)
                {
                    sensorType.Name = sensorTypeDto.Name;
                    sensorType.Description = sensorTypeDto.Description;
                    _context.Entry(sensorType).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(SensorTypeList));
                }
            }
            return View(sensorTypeDto);
        }

        public async Task<IActionResult> SensorTypeDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensorType = await _context.SensorTypes
                .Select(st => new GetSensorTypeDto
                {
                    Id = st.Id,
                    Name = st.Name,
                    Description = st.Description
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sensorType == null)
            {
                return NotFound();
            }

            return View(sensorType);
        }

        [HttpPost, ActionName("SensorTypeDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SensorTypeDeleteConfirmed(int id)
        {
            var sensorType = await _context.SensorTypes.FindAsync(id);
            if (sensorType != null)
            {
                _context.SensorTypes.Remove(sensorType);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(SensorTypeList));
        }
        #endregion

        #region Error
        [HttpGet]
        public async Task<IActionResult> ErrorList(int sensorTypeId)
        {
            var errors = await _context.Errors
                .Where(e => e.SensorTypeId == sensorTypeId)
                .Select(e => new GetErrorDto
                {
                    Id = e.Id,
                    Code = e.Code,
                    Description = e.Description
                })
                .ToListAsync();

            ViewBag.SensorTypeId = sensorTypeId;
            return View(errors);
        }

        [HttpGet]
        public async Task<IActionResult> ErrorDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var error = await _context.Errors
                .Select(e => new GetErrorDto
                {
                    Id = e.Id,
                    Code = e.Code,
                    Description = e.Description,
                    SensorTypeId = e.SensorTypeId
                })
                .FirstOrDefaultAsync(e => e.Id == id);

            if (error == null)
            {
                return NotFound();
            }

            return View(error);
        }

        [HttpGet]
        public IActionResult ErrorAdd(int sensorTypeId)
        {
            var errorDto = new AddErrorDto { SensorTypeId = sensorTypeId };
            return View(errorDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ErrorAdd(AddErrorDto errorDto)
        {
            if (ModelState.IsValid)
            {
                var error = new Error
                {
                    SensorTypeId = errorDto.SensorTypeId,
                    Code = errorDto.Code,
                    Description = errorDto.Description
                };
                await _context.Errors.AddAsync(error);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ErrorList), new { sensorTypeId = errorDto.SensorTypeId });
            }
            return View(errorDto);
        }

        [HttpGet]
        public async Task<IActionResult> ErrorEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var error = await _context.Errors
                .Select(e => new GetErrorDto
                {
                    Id = e.Id,
                    Code = e.Code,
                    Description = e.Description,
                    SensorTypeId = e.SensorTypeId
                })
                .FirstOrDefaultAsync(e => e.Id == id);

            if (error == null)
            {
                return NotFound();
            }

            return View(error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ErrorEdit(int id, GetErrorDto errorDto)
        {
            if (id != errorDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var error = await _context.Errors.FindAsync(id);
                if (error != null)
                {
                    error.Code = errorDto.Code;
                    error.Description = errorDto.Description;
                    _context.Entry(error).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ErrorList), new { sensorTypeId = error.SensorTypeId });
                }
            }
            return View(errorDto);
        }

        [HttpGet]
        public async Task<IActionResult> ErrorDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var error = await _context.Errors
                .Select(e => new GetErrorDto
                {
                    Id = e.Id,
                    Code = e.Code,
                    Description = e.Description
                })
                .FirstOrDefaultAsync(e => e.Id == id);

            if (error == null)
            {
                return NotFound();
            }

            return View(error);
        }

        [HttpPost, ActionName("ErrorDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ErrorDeleteConfirmed(int id)
        {
            var error = await _context.Errors.FindAsync(id);
            if (error != null)
            {
                _context.Errors.Remove(error);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ErrorList), new { sensorTypeId = error.SensorTypeId });
        }
        #endregion
    }
}
