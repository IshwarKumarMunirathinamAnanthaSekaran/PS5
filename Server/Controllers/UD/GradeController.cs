using DOOR.EF.Data;
using DOOR.EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text.Json;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Http.Headers;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using DOOR.Server.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Numerics;
using DOOR.Shared.DTO;
using DOOR.Shared.Utils;
using DOOR.Server.Controllers.Common;
using Microsoft.Extensions.Hosting;
using static System.Collections.Specialized.BitVector32;

namespace DOOR.Server.Controllers.UD
{

    [ApiController]
    [Route("api/[controller]")]
    public class GradeController: BaseController
	{
        public GradeController(DOOROracleContext _DBcontext,
    OraTransMsgs _OraTransMsgs)
    : base(_DBcontext, _OraTransMsgs)

        {
        }

        [HttpGet]
        [Route("GetGrade")]
        public async Task<IActionResult> GetGrade()
        {
            List<GradeDTO> lst = await _context.Grades
                .Select(sp => new GradeDTO
                {
                    SchoolId = sp.SchoolId,
                    StudentId = sp.StudentId,
                    SectionId = sp.SectionId,
                    GradeTypeCode = sp.GradeTypeCode,
                    GradeCodeOccurrence = sp.GradeCodeOccurrence,
                    NumericGrade = sp.NumericGrade,
                    Comments = sp.Comments,
                     CreatedBy = sp.CreatedBy,
                       CreatedDate =sp.CreatedDate,
                       ModifiedBy = sp.ModifiedBy,
                       ModifiedDate = sp.ModifiedDate
                     
                }).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetGrade/{_SchoolID}/{_StudentID}/{_SectionID}/{_GradeTypeCode}/{_GradeCodeOccurrence}")]
        public async Task<IActionResult> GetGrade(int _SchoolID, int _StudentID,int _SectionID, String _GradeTypeCode, int _GradeCodeOccurrence)
        { 
            GradeDTO? lst = await _context.Grades
                .Where(x => x.SchoolId == _SchoolID && x.StudentId == _StudentID && x.SectionId == _SectionID && x.GradeTypeCode == _GradeTypeCode && x.GradeCodeOccurrence == _GradeCodeOccurrence)
                .Select(sp => new GradeDTO
                {
                    SchoolId = sp.SchoolId,
                    StudentId = sp.StudentId,
                    SectionId = sp.SectionId,
                    GradeTypeCode = sp.GradeTypeCode,
                    GradeCodeOccurrence = sp.GradeCodeOccurrence,
                    NumericGrade = sp.NumericGrade,
                    Comments = sp.Comments,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate

                }).FirstOrDefaultAsync();
            return Ok(lst);
        }

        [HttpPost]
        [Route("PostGrade")]
        public async Task<IActionResult> PostGrade([FromBody] GradeDTO _GradeDTO)
        {
            try
            {
                Grade c = await _context.Grades.Where(x => x.SchoolId == _GradeDTO.SchoolId && x.StudentId == _GradeDTO.StudentId && x.SectionId == _GradeDTO.SectionId && x.GradeTypeCode == _GradeDTO.GradeTypeCode && x.GradeCodeOccurrence == _GradeDTO.GradeCodeOccurrence).FirstOrDefaultAsync();

                if (c == null)
                {
                    c = new Grade
                    {

                        
                        SchoolId = _GradeDTO.SchoolId,
                        StudentId = _GradeDTO.StudentId,
                        SectionId = _GradeDTO.SectionId,
                        GradeTypeCode = _GradeDTO.GradeTypeCode,
                        GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence,
                        NumericGrade = _GradeDTO.NumericGrade,
                        Comments = _GradeDTO.Comments,
                        CreatedBy = _GradeDTO.CreatedBy,
                        CreatedDate = _GradeDTO.CreatedDate,
                        ModifiedBy = _GradeDTO.ModifiedBy,
                        ModifiedDate = _GradeDTO.ModifiedDate

                    };
                    _context.Grades.Add(c);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }

        [HttpPut]
        [Route("PutGrade")]
        public async Task<IActionResult> PutGrade([FromBody] GradeDTO _GradeDTO)
        {
            try
            {
                Grade c = await _context.Grades.Where(x => x.SchoolId == _GradeDTO.SchoolId && x.StudentId == _GradeDTO.StudentId && x.SectionId == _GradeDTO.SectionId && x.GradeTypeCode == _GradeDTO.GradeTypeCode && x.GradeCodeOccurrence == _GradeDTO.GradeCodeOccurrence).FirstOrDefaultAsync();

                if (c != null)
                {

                    c.SchoolId = _GradeDTO.SchoolId;
                    c.StudentId = _GradeDTO.StudentId;
                    c.SectionId = _GradeDTO.SectionId;
                    c.GradeTypeCode = _GradeDTO.GradeTypeCode;
                    c.GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence;
                    c.NumericGrade = _GradeDTO.NumericGrade;
                    c.Comments = _GradeDTO.Comments;
                    c.CreatedBy = _GradeDTO.CreatedBy;
                    c.CreatedDate = _GradeDTO.CreatedDate;
                    c.ModifiedBy = _GradeDTO.ModifiedBy;
                    c.ModifiedDate = _GradeDTO.ModifiedDate;

                    _context.Grades.Update(c);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("DeleteGrade/{_SchoolID}/{_StudentID}/{_SectionID}/{_GradeTypeCode}/{_GradeCodeOccurrence}")]
        public async Task<IActionResult> DeleteGrade(int _SchoolID, int _StudentID, int _SectionID, String _GradeTypeCode, int _GradeCodeOccurrence)
        {
            try
            {
                Grade c = await _context.Grades.Where(x => x.SchoolId == _SchoolID && x.StudentId == _StudentID && x.SectionId == _SectionID && x.GradeTypeCode == _GradeTypeCode && x.GradeCodeOccurrence == _GradeCodeOccurrence).FirstOrDefaultAsync();

                if (c != null)
                {
                    _context.Grades.Remove(c);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }



    }
}    

