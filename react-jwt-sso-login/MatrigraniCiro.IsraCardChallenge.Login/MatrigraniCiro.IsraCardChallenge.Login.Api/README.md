# Testing the application
## Installation

Install the App 

Run the MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login/App .csproj on port
```bash
https://localhost:7234 
```
or change it at: ``launchSettings.json applicationUrl``

Run the App/personalinfo_sso_chalenge React App on port
```bash
http://localhost:7771
```
or change the por at file: ``App/personalinfo_sso_chalenge/.env``

Run the App/login_sso_chalenge React App any port you want, example
```bash
http://localhost:3000
```
or change the por at file: ``App/login_sso_chalenge/.env``

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
 - Make unit tests for the minimal API, Service, and Repository using xUnit
 - Make unit tests for the Apps 
 - Make the swagger documentation
 - Improve the style of the React Apps using Bootstrap for example
 - Add logs
- Add better error threatment