# Project Canary Emissions - Take-Home Assignment

This project involves building a system to process and visualize methane emissions data. The backend is scaffolded in C# using .NET 8, and you will complete the implementation and create a React frontend.

## Goals

1. Ingest emissions data from two CSV types:
   - **Measured emissions**: Includes site, equipment, date range, and methane mass. Sites may have multiple concurrent equipment leaks and each equipment will belong to a group.
   - **Estimated emissions**: Includes site, equipment group, date range, and methane mass. Estimated data is already aggregated at the group level.
2. Save this data and its relationships in a PostgreSQL database.
3. Provide an API to retrieve aggregated emissions data grouped by site, month, or equipment group.
4. Build a React frontend with a chart comparing measured vs. estimated emissions, allowing toggling between grouping modes.

## Setup

To set up and run the project, follow these steps:

1. Start the PostgreSQL database using Docker Compose. From the root directory, run:

```docker-compose up -d```

2. Apply the database migrations to set up the schema. Run the following command:

```ef migrations add InitialCreate -- "Host=localhost;Port=5433;Database=project_canary_takehome;Username=project_canary_takehome;Password=giveemissionsthebird"```

```ef database update -- "Host=localhost;Port=5433;Database=project_canary_takehome;Username=project_canary_takehome;Password=giveemissionsthebird"```

## Deliverables

Submit a zip of your completed backend and frontend solutions. Keep your implementation clean and simple and contact us if you have questions. Good luck!

## Rob's Notes
The entity framework InitialCreate call above threw an error. I updated the call so that it will succeed.

docker-compose.yml was updated to set up pgadmin4 as well. This can be accessed from localhost:5050, but can be removed if not wanted. I needed a simple postgresql management interface.

I manually created seed data for an emission_sites and an equipment_groups table. That seed data is specified in ProjectCanaryDbContext.cs.

The backend is set up to allow cors specifically only in dev mode and only for requests from localhost:3000. So The frontend must run on that port.

The frontend requires a .env.local file to point it to the dotnet backend. The backend is currently configured to port 5134 when using the http profile, so the contents of this file should be
NEXT_PUBLIC_API_URL=http://localhost:5134
This would change if you are using iisExpress or another profile.

The frontend sorts by month by default.

If I had more time or had to make this similar to a production app, I would do the following:
* Add logging everywhere
* Do something more clever than catching all exceptions in controller methods. I am currently swallowing the exception messages because you usually wouldn't want the
  consumer of your api to see exception messages because it is ugly and also gives insight into your code. Having said that, catching all exceptions isn't terribly elegant.
  I would definitely try to return something like a 409 when the user attempts to upload duplicate data.
* Add more tests. 
* GetMeasuredVsEstimatedChartData in the EmissionsController converts month int string to month name when you are ordering by month. This is gross and difficult to read. I would find a cleaner way to do this.
* The month group by parameter is currently called YearAndMonth. In the real world, we would have results from multiple years so you would either have to display results from all months from all years that have data, or you would have to specify the year in your query. Since we only have data from 2023, this is not a problem in the take home.