using CalendarAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        public static List<Person> Persons = new List<Person>()
        {
            new Person()
            {
                Name = "Ze",
                Role = "Interview",
                Slots = new List<Slot>()
                {
                    new Slot()
                    {
                        DateStart = DateTime.Now,
                        DateEnd = DateTime.Now.AddDays(5)
                    }
                }
            }, new Person()
            {
                Name = "Pedro",
                Role = "Candidate",
                Slots = new List<Slot>()
                {
                    new Slot()
                    {
                        DateStart = DateTime.Now,
                        DateEnd = DateTime.Now.AddDays(5)
                    }
                }
            }
        };
        public CalendarController()
        {
        }
        [HttpGet]
        public List<Person> Get()
        {
            return Persons.ToList();
        }
        [HttpPost]
        public Person Post(Person person)
        {
            if (person != null)
            {
                Persons.Add(person);
            }
            return person;
        }

        [HttpGet]

        public List<Slot> Available([FromQuery] string candidate, [FromQuery] List<string> interviewers)
        {
            List<Slot> slots = new List<Slot>();
            if (string.IsNullOrEmpty(candidate) || interviewers.Count == 0) return new List<Slot>();
            Person personCandidate = Persons.Where(x => x.Name == candidate && x.Role.ToLowerInvariant() == "candidate").FirstOrDefault();
            if (candidate == null) return new List<Slot>();

            List<Person> personInterviewers = (from persons in Persons
                                               join inter in interviewers on persons.Name equals inter
                                               where persons.Role.ToLowerInvariant() == "interview"
                                               select persons).ToList();
            if (personInterviewers.Count == 0) return new List<Slot>();
            personInterviewers.ForEach(x => slots.AddRange(x.Slots));
            return slots;
        }
    }
}