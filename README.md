RaceDay API - Final POE

Student Information
- Name:Lesego
- Student Number:ST10485315
- **Module:** PROG6212

## Setup Instructions
1. Clone this repository.
2. Open the solution in Visual Studio.
3. Open the Package Manager Console and run: `Update-Database`
4. Press F5 to run the API. Swagger will open automatically.

## Roles
- **Organiser:** Can create, update, and delete events, categories, and capture results.
- **Participant:** Can view events, enroll in events, and view their own results.

## CI/CD
![Green Build](YOUR_SCREENSHOT_LINK_HERE) 
*(Take a screenshot of your green GitHub Actions checkmark and upload it to the repo, then link it here).*

## Docker Setup (MVC Application)
To run the MVC application inside a Docker container, use the following commands from the root directory:

1. Build the Docker image:
   `docker build -t raceday-mvc -f RaceDayMVC/Dockerfile .`
2. Run the container:
   `docker run -p 8080:80 raceday-mvc`
3. Open your browser and navigate to `http://localhost:8080`

## Video Demonstration
https://www.youtube.com/@Sal-y4h4b 
