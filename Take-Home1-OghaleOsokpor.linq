<Query Kind="Statements">
  <Connection>
    <ID>6479f7e6-f818-4797-bfa5-0a28994457c6</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>DESKTOP-E3GBQLM\SQLEXPRESS</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>StartTed-2025-Sept</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

//================ Question 1 ====================
// Show upcoming club activities on/after Jan 1, 2025
// Exclude: "BTech Club Meeting" and "Scheduled Room"  
// Output: StartDate, VenueName, ClubName, ActivityTitle
// Order: ascending by StartDate

ClubActivities
  .Where(a =>
	  a.StartDate >= new DateTime(2025, 1, 1) &&
	  a.Name != "BTech Club Meeting" &&
	  a.CampusVenue.Location != "Scheduled Room"
  )
  .Select(a => new
  {
	  StartDate = a.StartDate,
	  Venue = a.CampusVenue.Location,
	  Club = a.Club.ClubName,
	  Activity = a.Name
  })
  .OrderBy(x => x.StartDate)
  .Dump();





//================ Question 2 ====================
// Map SchoolCode to its full school name:
//   "SAMIT" → "School of Advance Media and IT"
//   "SEET"  → "School of Electrical Engineering Technology"
//   All other codes → "Unknown"
// For each program, include the program name, count the number of required courses and optional courses,
// keep only programs with RequiredCourseCount ≥ 22,
// order by Program (ascending).
// Output: School, Program, RequiredCourseCount, OptionalCourseCount

Programs
  .Select(p => new
  {
	  School =
		  p.SchoolCode == "SAMIT" ? "School of Advance Media and IT" :
		  p.SchoolCode == "SEET" ? "School of Electrical Engineering Technology" :
									"Unknown",
	  Program = p.ProgramName,
	  RequiredCourseCount = p.ProgramCourses.Count(course => course.Required),
	  OptionalCourseCount = p.ProgramCourses.Count(course => !course.Required)
  })
  .Where(x => x.RequiredCourseCount >= 22)
  .OrderBy(x => x.Program)
  .Dump();


//================ Question 3 ====================
// Show all non-Canadian students who have not made any tuition payments.
// Output: StudentNumber, CountryName, FullName, ClubMembershipCount ("None" if zero).
// Order: LastName ascending.

Students
  .Where(student =>
	  student.Countries.CountryName != "Canada" &&
	  student.StudentPayments.Count() == 0
  )
  .OrderBy(student => student.LastName)
  .Select(student => new
  {
	  StudentNumber = student.StudentNumber,
	  CountryName = student.Countries.CountryName,
	  FullName = student.FirstName + " " + student.LastName,
	  ClubMembershipCount = student.ClubMembers.Count() == 0
							  ? "None"
							  : student.ClubMembers.Count().ToString()
  })
  .Dump();


//================ Question 4 ====================
// Show all active instructors (Position = "Instructor", ReleaseDate = null)
// who have taught at least one class in ClassOfferings.
// Output: ProgramName, FullName, WorkLoad ("High" >24, "Med" >8, else "Low").
// Order: by number of ClassOfferings (descending), then by LastName (ascending).

Employees
  .Where(instructor =>
	  instructor.Position.Description == "Instructor" &&
	  instructor.ReleaseDate == null &&
	  instructor.ClassOfferings.Count() > 0
  )
  .Select(instructor => new
  {
	  ProgramName = instructor.Program.ProgramName,
	  FullName = instructor.FirstName + " " + instructor.LastName,
	  LastName = instructor.LastName,               
	  OfferingCount = instructor.ClassOfferings.Count(),
	  WorkLoad = instructor.ClassOfferings.Count() > 24 ? "High"
					: instructor.ClassOfferings.Count() > 8 ? "Med"
					: "Low"
  })
  .OrderByDescending(x => x.OfferingCount)
  .ThenBy(x => x.LastName)
  .Select(x => new
  {
	  ProgramName = x.ProgramName,
	  FullName = x.FullName,
	  WorkLoad = x.WorkLoad
  })
  .Dump();

//================ Question 5 ====================
// Snapshot of all clubs.
// Output: Supervisor ("Unknown" if Employee is null), ClubName, MemberCount,
// Activities ("None Schedule" if no ClubActivities, else the count).
// Order: MemberCount descending.

Clubs
  .Select(club => new
  {
	  Supervisor = club.Employee == null
					  ? "Unknown"
					  : (club.Employee.FirstName + " " + club.Employee.LastName),
	  Club = club.ClubName,
	  MemberCount = club.ClubMembers.Count(),
	  Activities = club.ClubActivities.Count() == 0
					  ? "None Schedule"
					  : club.ClubActivities.Count().ToString()
  })
  .OrderByDescending(x => x.MemberCount)
  .Dump();
