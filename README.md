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
