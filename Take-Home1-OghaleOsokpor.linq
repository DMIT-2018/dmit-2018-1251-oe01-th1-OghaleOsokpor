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