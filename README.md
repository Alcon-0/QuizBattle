
<h1 align="center" style="font-weight: bold;"> Quiz Battle Application 🎮📚</h1>

<p align="center">
<a href="#tech">Technologies</a>
<a href="#started">Getting Started</a>
<a href="#routes">API Endpoints</a>

 
</p>


<p align="center">Quiz Battle is a full-stack application for creating and playing quizzes</p>



<h2 id="technologies">💻 Technologies</h2>

- React frontend (Vite)
- ASP.NET Core WebAPI backend
- SQL Server for relational data
- MongoDB for image storage

<h2 id="started">🚀 Getting started</h2>

Here described how to run project locally

<h3>Prerequisites</h3>

Here you list all prerequisites necessary for running this project.

- [Docker](https://docs.docker.com/desktop/release-notes/) ≥ 4.0
- [Node.js](https://nodejs.org/en/download) 20.x
- [Git](https://git-scm.com/downloads) >= 2.34.0

<h3>Cloning</h3>

How to clone project

```bash
git clone https://github.com/Alcon-0/QuizBattle.git
```

<h3>Starting</h3>

#### How to start project

Run the docker with web api and databases
```bash 
cd QuizBattle
docker-compose up -d --build
```
Build and run the react application
```bash 
cd QuizBattle.Client
npm install
npm run build
npm run preview
```

<h2 id="routes">📍 API Endpoints</h2>



| route               | description                                          
|----------------------|-----------------------------------------------------
| <kbd>GET api/quizzes</kbd>     | retrieves quizzes info see [response details](#get-auth-detail)
| <kbd>GET api/quizzes/{quizId}/questions</kbd>     | retrieves quizze questions see [response details](#post-auth-detail)

<h3 id="get-auth-detail">GET api/quizzes</h3>

**RESPONSE**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "string",
    "description": "string",
    "createdAt": "2025-05-16T12:58:21.369Z",
    "imageId": "string",
    "imageUrl": "string"
  }
]
```

<h3 id="post-auth-detail">GET api/quizzes/{quizId}/questions</h3>

**RESPONSE**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "text": "string",
    "options": [
      {
        "id": 0,
        "text": "string"
      }
    ],
    "correctAnswerIndex": 0,
    "imageId": "string",
    "imageUrl": "string"
  }
]
```


# License

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Alcon-0/QuizBattle/blob/main/LICENSE)

# Author

Developed by **Viktor Yelenetskyi**  
📧 Email: [yelenetskyi@gmail.com]  
💼 GitHub: [https://github.com/Alcon-0](https://github.com/Alcon-0)

This project was created as part of a personal project.

© 2025 All rights reserved.


