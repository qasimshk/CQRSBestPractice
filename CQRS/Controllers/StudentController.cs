﻿using AutoMapper;
using CQRS.Business.Utils;
using CQRS.Commands;
using CQRS.Helper;
using CQRS.Models;
using CQRS.Queries;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CQRS.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly Messages _messages;

        public StudentController(IMapper mapper, Messages messages)
        {
            _mapper = mapper;
            _messages = messages;
        }

        [HttpGet]
        public IActionResult Ping()
        {
            return Ok(DateTime.Now.ToShortTimeString());
        }

        [HttpGet("{Id}/courses")] // Query
        public async Task<IActionResult> GetStudentCourse(int Id)
        {
            return FromResult((Results)await _messages
                .DispatchAsync(new StudentCoursesInfoQuery(Id)));
        }

        [HttpGet("All")] // Query
        public async Task<IActionResult> GetStudent()
        {
            return FromResult((Results)await _messages
                .DispatchAsync(new AllStudentsInfoQuery()));
        }

        [HttpGet("{Id}", Name = "GetStudentById")] // Query
        public async Task<IActionResult> GetStudentById(int Id)
        {
            return FromResult((Results)await _messages
                .DispatchAsync(new AllStudentsInfoQuery(Id)));
        }

        [HttpPost("Register")] // Command
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStudent(
            [FromBody] RegisterStudentDto Student)
        {
            var resp = await _messages.DispatchAsync(
                _mapper.Map<RegisterStudentInfoCommand>(Student));

            if (resp.IsSuccessful)
            {
                var studentToReturn = _mapper.Map<StudentsViewDto>(resp.ResponseMessage);
                return CreatedAtRoute("GetStudentById",
                    new { studentToReturn.Id },
                    studentToReturn);
            }
            return FromResult(resp);
        }

        [HttpPut("Update/{Id}")] // Command
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStudent(
            int id, [FromBody]UpdateStudentDto student)
        {
            var command = _mapper.Map<EditStudentInfoCommand>(student);
            command.StudentId = id;
            return FromResult(await _messages.DispatchAsync(command));
        }

        [HttpDelete("UnRegister/{Id}")] // Command
        public async Task<IActionResult> UnRegisterStudent(int Id)
        {
            return FromResult(await _messages.DispatchAsync(
                new UnRegisteredStudentCommand
                {
                    StudentId = Id
                }));
        }

        [HttpPatch("{Id}")] // Command
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartialUpdateStudent(int Id,
            [FromBody] JsonPatchDocument<PatchStudentDto> patchStudentUpdate)
        {
            return FromResult(await _messages.DispatchAsync(
                _mapper.Map<JsonPatchDocument<PartialEditStudentInfoCommand>>(
                    patchStudentUpdate), Id));

            #region obsolete

            /*
            var dbstudent = await _studentRepository.GetStudentByIdAsync(Id);
            if (dbstudent == null)
                return NotFound();

            var studentToPatch = _mapper.Map<PatchStudentDto>(dbstudent);

            patchStudentUpdate.ApplyTo(studentToPatch, ModelState);

            if (studentToPatch.FirstName == studentToPatch.LastName)
                ModelState.AddModelError(nameof(PatchStudentDto),
                    "First name and last name cannot be same!");

            TryValidateModel(studentToPatch);
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            await _studentRepository.UpdateStudentAsync(
                _mapper.Map(studentToPatch, dbstudent));
            return NoContent();
            */

            #endregion obsolete
        }
    }
}