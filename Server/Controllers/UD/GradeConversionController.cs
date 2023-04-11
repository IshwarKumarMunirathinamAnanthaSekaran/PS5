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
namespace DOOR.Server.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradeConversionController :BaseController 
	{
        public GradeConversionController(DOOROracleContext _DBcontext,
    OraTransMsgs _OraTransMsgs)
    : base(_DBcontext, _OraTransMsgs)

        {
        }

        [HttpGet]
        [Route("GetGradeConversion")]
        public async Task<IActionResult> GetGradeConversion()
        {
            List<GradeConversionDTO> lst = await _context.GradeConversions
                .Select(sp => new GradeConversionDTO
                {
                    SchoolId = sp.SchoolId,
                    LetterGrade = sp.LetterGrade,
                    GradePoint = sp.GradePoint,
                    MaxGrade = sp.MaxGrade,
                    MinGrade = sp.MinGrade,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetGradeConversion/{_SchoolID}/{_LetterGrade}")]
        public async Task<IActionResult> GetGradeConversion(int _SchoolID, String _LetterGrade)
        {
            GradeConversionDTO? lst = await _context.GradeConversions
                .Where(x => x.SchoolId == _SchoolID && x.LetterGrade == _LetterGrade)
                .Select(sp => new GradeConversionDTO
                {
                    SchoolId = sp.SchoolId,
                    LetterGrade = sp.LetterGrade,
                    GradePoint = sp.GradePoint,
                    MaxGrade = sp.MaxGrade,
                    MinGrade = sp.MinGrade,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }).FirstOrDefaultAsync();
            return Ok(lst);
        }

        [HttpPost]
        [Route("PostGradeConversion")]
        public async Task<IActionResult> PostGradeConversion([FromBody] GradeConversionDTO _GradeConversionDTO)
        {
            try
            {
                GradeConversion c = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversionDTO.SchoolId && x.LetterGrade == _GradeConversionDTO.LetterGrade).FirstOrDefaultAsync();

                if (c == null)
                {
                    c = new GradeConversion
                    {
                        SchoolId = _GradeConversionDTO.SchoolId,
                        LetterGrade = _GradeConversionDTO.LetterGrade,
                        GradePoint = _GradeConversionDTO.GradePoint,
                        MaxGrade = _GradeConversionDTO.MaxGrade,
                        MinGrade = _GradeConversionDTO.MinGrade,
                        CreatedBy = _GradeConversionDTO.CreatedBy,
                        CreatedDate = _GradeConversionDTO.CreatedDate,
                        ModifiedBy = _GradeConversionDTO.ModifiedBy,
                        ModifiedDate = _GradeConversionDTO.ModifiedDate

                    };
                    _context.GradeConversions.Add(c);
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
        [Route("PutGradeConversion")]
        public async Task<IActionResult> PutGradeConversion([FromBody] GradeConversionDTO _GradeConversionDTO)
        {
            try
            {
                GradeConversion c = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversionDTO.SchoolId && x.LetterGrade == _GradeConversionDTO.LetterGrade).FirstOrDefaultAsync();

                if (c != null)
                {
                    
                    c.SchoolId = _GradeConversionDTO.SchoolId;
                    c.LetterGrade = _GradeConversionDTO.LetterGrade;
                    c.GradePoint = _GradeConversionDTO.GradePoint;
                    c.MaxGrade = _GradeConversionDTO.MaxGrade;
                    c.MinGrade = _GradeConversionDTO.MinGrade;
                    c.CreatedBy = _GradeConversionDTO.CreatedBy;
                    c.CreatedDate = _GradeConversionDTO.CreatedDate;
                    c.ModifiedBy = _GradeConversionDTO.ModifiedBy;
                    c.ModifiedDate = _GradeConversionDTO.ModifiedDate;

                    _context.GradeConversions.Update(c);
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
        [Route("DeleteGradeConversion/{_SchoolID}/{_LetterGrade}")]
        public async Task<IActionResult> DeleteGradeConversion(int _SchoolID, String _LetterGrade)
        {
            try
            {
                GradeConversion c = await _context.GradeConversions.Where(x => x.SchoolId == _SchoolID && x.LetterGrade == _LetterGrade).FirstOrDefaultAsync();

                if (c != null)
                {
                    _context.GradeConversions.Remove(c);
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

