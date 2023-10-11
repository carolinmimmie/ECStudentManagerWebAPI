using System.ComponentModel.DataAnnotations;
using ECStudentManagerWebAPI.Data;
using ECStudentManagerWebAPI.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECStudentManagerWebAPI.Controllers;

// GET | POST | PUT | DELETE ... /students -> StudentsController

//Detta är en attribut som märker klassen StudentsController som en Web API-kontroller. 
[ApiController]
// Detta är ett attribut som används för att definiera ruttens basväg för denna kontroller.
[Route("[controller]")]

//Detta deklarerar själva kontrollerklassen och ärver från ControllerBase. 
//ControllerBase är en grundläggande kontrollerklass som används för API:er.
public class StudentsController : ControllerBase
{

    // Detta skapar ett privat fält (context) som håller en instans av ApplicationDbContext.
    // ApplicationDbContext används för att interagera med databasen. readonly betyder att 
    // värdet av context inte kan ändras efter att det har tilldelats i konstruktorn.
    private readonly ApplicationDbContext context;


    // Detta är konstruktorn för StudentsController. 
    // Den tar in en parameter av typen ApplicationDbContext
    // och använder den för att sätta värdet på det privata fältet context. 
    // Konstruktorn spelar en viktig roll när en ny instans av StudentsController skapas,
    // eftersom den används för att ställa in objektet med den nödvändiga databaskontexten.
    public StudentsController(ApplicationDbContext context)
    {
        this.context = context;
    }

    // POST /students
    // {
    //   "firstName": "Jane",
    //   "lastName": "Doe",
    //   "socialSecurityNumber": "19900101-2020",
    //   "email": "jane@doe.com"
    // }
    [HttpPost]
    public ActionResult<StudentDto> CreateStudent(CreateStudentRequest createStudentRequest)
    {
        // 1 - Skapa ett objekt av typ Student och kopiera över värden från createStudentRequest
        var student = new Student
        {
            FirstName = createStudentRequest.FirstName,
            LastName = createStudentRequest.LastName,
            SocialSecurityNumber = createStudentRequest.SocialSecurityNumber,
            Email = createStudentRequest.Email
        };

        // 2 - Spara studerande till databasen

        context.Students.Add(student);

        context.SaveChanges();

        // 3 - För över information från entitet till DTO och returnera till klienten
        var studentDto = new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            SocialSecurityNumber = student.SocialSecurityNumber,
            Email = student.Email
        };

        return Created("", studentDto); // 201 Created
    }

    // GET /students
    [HttpGet]
    public IEnumerable<StudentDto> GetStudents()
    {
        var students = context.Students.ToList();

        var studentsDto = students.Select(x => new StudentDto//skapar en ny samling av StudentDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            SocialSecurityNumber = x.SocialSecurityNumber,
            Email = x.Email
        });

        return studentsDto; // 200 OK
    }

    // GET /students/{id}
    [HttpGet("{id}")]
    public ActionResult<StudentDto> GetStudent(int id)
    {
        var student = context.Students.FirstOrDefault(x => x.Id == id);//letar efter den studerande 

        if (student is null) // om studerander inte finns
            return NotFound(); // returnera 404 Not Found

        var studentDto = new StudentDto // om den studerande fanns vill vi mappa detaljerna från den studerande till en Dto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            SocialSecurityNumber = student.SocialSecurityNumber,
            Email = student.Email
        };

        return studentDto; //retuerna 200 OK plis informationen ovan om studerande
    }

    // DELETE /students/{id}
    [HttpDelete("{id}")]
    public ActionResult DeleteStudent(int id)
    {
        var student = context.Students.FirstOrDefault(x => x.Id == id);

        if (student is null)
            return NotFound(); // 404 Not Found        

        context.Students.Remove(student);

        // SQL DELETE skickas till databasen för att radera den studerande
        context.SaveChanges();

        return NoContent(); // 204 No Content
    }

    // DET SOM SKICKAS IN I JSON-DATAN SOM SKICKAS IN I BODY I THUNDER CLIENT
    // PUT /students/1
    // 
    // {
    //   "id": 1,
    //   "firstName": "Jane",
    //   "lastName": "Doe",
    //   "socialSecurityNumber": "19900101-2010",
    //   "email": "jane@outlook.com"
    // }
    [HttpPut("{id}")]
    public ActionResult UpdateStudent(int id, UpdateStudentRequest updateStudentRequest) // id och inkommande data
    {
        if (id != updateStudentRequest.Id) // om id från body inte stämmer överrens med updatedstudendRequest Id
            return BadRequest(); // returnera 400 Bad Request

        var student = context.Students.FirstOrDefault(x => x.Id == id); // letar efter id i databasen

        if (student is null) // om det inte hittas 
            return NotFound(); //  returnera 404 Not Found        

        // om den studerande fanns då vill vi uppdatera egenskaperna så de matchar datat vi fick in.
        student.FirstName = updateStudentRequest.FirstName;
        student.LastName = updateStudentRequest.LastName;
        student.SocialSecurityNumber = updateStudentRequest.SocialSecurityNumber;
        student.Email = updateStudentRequest.Email;

        // Skickar SQL UPDATE till databasen
        context.SaveChanges();

        return NoContent(); // 204 No Content
    }
}

public class UpdateStudentRequest
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SocialSecurityNumber { get; set; }
    public string Email { get; set; }
}

public class StudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SocialSecurityNumber { get; set; }
    public string Email { get; set; }
}


//Dto-klass
public class CreateStudentRequest
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [MaxLength(13)]
    [Required]
    public string SocialSecurityNumber { get; set; }

    [Required]
    public string Email { get; set; }
}