# Testing the application
## Installation

### React applications

Install the Apps inside the folder ``App/personalinfo_sso_chalenge`` and ``App/login_sso_chalenge/.env``
Run the aplications using the ``"npm start"`` command

Run the App/personalinfo_sso_chalenge React App on port
```bash
http://localhost:7771
```
or change the port at file: ``App/personalinfo_sso_chalenge/.env``

Run the App/login_sso_chalenge React App any port you want, example
```bash
http://localhost:3000
```
or change the por at file: ``App/login_sso_chalenge/.env``

### Backend

Run the MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login/App .csproj on port
```bash
https://localhost:7234 
```
or change it at: ``launchSettings.json applicationUrl``

## Usage
On the login page:
Try to use an unauthorized user, it will show an alert with an error
```
ERROR:401
```
On the login page: Use one of the allowed users bellow to log in
```
 Username = "admin", Password = "teste" 
 Username = "admin1", Password = "teste" 
 Username = "ciro", Password = "challenge
 Username = "author", Password = "myPass"
```
Testing: 
It will redirect to the personal info page
The personal info will show the login users details and the token at the URL
Example Personal Info:
```
Personal Info
Username | Password | Login Id
admin | teste | 28fb02e0-b305-4898-b30a-a24632e57538
admin1 | teste | af379aee-8bcd-48d5-8274-148be88b17bb
ciro | challenge | bc3276f4-84cc-4667-8542-4a75bcbadc20
author | myPass | 6bed0927-cace-4a8b-871a-49f90966f3ab
```
Example URL with the token:
```
http://localhost:7771/?token=eyJhbGciOiJIUzI1NiIs...
```
# What I like to have time to do

I got this challenge in the Tuesday afternoon and I am delivering it Thiuursday Morning, but if I had more time I'd like to:

## Tests
 - Include unit tests for the minimal API, Service, and Repository using xUnit
 - Make unit tests for the Apps 
 - Make an integration tests with the entire flow, using Selenium
 - Integrate with SonarCube or similar to bring metrics
## Documentation
 - Make the swagger documentation properly in C#
 - Add logs management
 - Improve the read-me documentation add more steps to facilitate the deploy
## ERROR TREATMENT
 - Add better error treatment maybem include custom exceptions and better error messages
 - Replace the messages for each Status Code in Swagger and the Apps
## UX and UI
- Improve the style of the React Apps using Bootstrap
- Improve UX and UI of the Apps

 