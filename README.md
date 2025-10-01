# Task Board App

A simple **Task Board Web App** built with *C#/.NET + Angular*, featuring CRUD operations, task status toggling, and toast notifications for user feedback.  
This project was created as part of my internship application to demonstrate full-stack development skills.

---

##  Features

- Create, edit, and delete tasks
- Toggle task status (Todo → In Progress → Done)
- Toast notifications for success/error feedback
- Styled with Bootstrap + SCSS
- Responsive layout

---

## Tech Stack

**Frontend**
- Angular (Zoneless + Standalone components + Signals)
- Bootstrap 5
- SCSS
- ngx-toastr

**Backend**
- REST API (.NET Core)
- SQLite DB

---

## Visuals
<img width="1912" height="850" alt="taskboard_screenshot" src="https://github.com/user-attachments/assets/5220d86b-3148-4619-9103-e10ae5ea7131" />

---

## Getting Started

### Prerequisites
- Node.js & npm
- Angular CLI
- .NET 8 SDK

### Installation and running
```bash
# Clone repo
git clone https://github.com/Aydin324/TaskBoard-Project.git

# Run backend (runs at port 5109)
cd TaskBoard.Api
dotnet run

# Install frontend dependencies
cd ../TaskBoard.Client
npm install

# Serve frontend (runs at port 4200, script uses proxy configuration)
npm start
```
Open *http://localhost:4200/* in browser


### Running unit tests
```bash
# Don't run in concurrency with backend server
cd ../TaskBoard.Tests
dotnet test
