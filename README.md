# 📝 Todo List API

This is a **Todo List API** built with **.NET Core**, following **Clean Architecture** principles. It provides endpoints to manage Todo items, such as creating,
retrieving, marking as completed, and fetching pending tasks. The API is built with extensibility and simplicity in mind,
using **Entity Framework Core** with an in-memory database for data management.

---

## 🚀 Features

- ✅ Create a new Todo item.
- 📋 Retrieve all Todo items.
- 🔍 Retrieve only pending (not completed) Todo items.
- ✔️ Mark a Todo item as completed.
- 🌐 Fully RESTful API design.
- 🛠️ Integrated Swagger UI for easy API exploration.

---

## 🛠️ Technologies Used

- **.NET Core 8.0**
- **Entity Framework Core** (In-Memory Database)
- **Swagger/OpenAPI** for API documentation.
- **Clean Architecture** principles.
- **Dependency Injection** for service and repository layers.

---

## ⚙️ Getting Started
### 🏃‍♂️ Setup and Run

Follow these steps to clone and run the project:
**Clone the Repository**
   ```bash
   git clone https://github.com/Shazllyy/Nitaj_Task.git
   cd todo-list-api
   run the project 
##############################################################################################
## 🧪 Testing

This project includes unit tests to ensure the reliability and correctness of its features.
The tests cover key functionalities of the application, including services and repositories.
Follow the steps below to run the tests:

### 🛠️ Running the Tests

1. **Navigate to the Root of the Project**  
   Open a terminal in the project's root directory.

2. **Run the Tests**  
   Use the following command to execute all unit tests:
   ```bash
   dotnet test
