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