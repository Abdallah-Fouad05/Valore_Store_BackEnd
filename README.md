# 🛒 Valore Store - Back-End API

The **Valore Store Back-End API** is a robust and scalable RESTful service built with **.NET 9**.
It powers an E-Commerce platform by handling business logic, data management, and client-server communication.

---

## 🚀 Features

* 🔐 Authentication & Authorization (JWT Ready)
* 👤 User Management
* 🛍️ Product Management
* 📦 Order Processing System
* 📊 Order & Payment Status Tracking
* 🧩 Modular & Scalable Architecture

---

## 🏗️ Tech Stack

* **.NET 8**
* **C#**
* **ASP.NET Core Web API**
* **SQL Server**
* **ADO.net**
* **RESTful Architecture**

---

## 📂 Project Structure

```bash
Back-End/
│── Controllers/       # API Endpoints
│── Business/          # Business Logic
│── Data/              # Data Access Layer
│── DTOs/              # Entities & DTOs
│── Program.cs         # Entry Point
```

---

## 🗄️ Database Design

The system is built using a well-structured relational database.

### 📊 ERD Diagram

### ERD
![ERD](https://github.com/user-attachments/assets/a9288e6c-127a-4684-9443-3d667e384673)

### 🔑 Core Entities:

* Users
* Products and Categories
* Orders
* OrderItems
* Carts
* CartItems
* Shipping
* Status
* Review

---

## ⚙️ Getting Started

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/your-username/valore-store.git
cd valore-store/Back-End
```

---

### 2️⃣ Configure Database

Update your **connection string** inside:

```json
appsettings.json
```

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ValoreStore;Trusted_Connection=True;"
}
```

---

### 3️⃣ Apply Migrations

```bash
dotnet ef database update
```

---

### 4️⃣ Run the API

```bash
dotnet run
```

API will run on:

```
https://localhost:xxxx
```

---

## 📬 API Endpoints

| Method | Endpoint           | Description        |
| ------ | ------------------ | ------------------ |
| GET    | /api/products      | Get all products   |
| GET    | /api/products/{id} | Get product by ID  |
| POST   | /api/products      | Create new product |
| PUT    | /api/products/{id} | Update product     |
| DELETE | /api/products/{id} | Delete product     |

*(More endpoints available for Users, Orders, Shipping, etc.)*

---

## 🔐 Authentication

* JWT-based authentication (if implemented)
* Secure endpoints with `[Authorize]`

---

## 🧠 Best Practices Applied

* Clean Architecture Principles
* Separation of Concerns
* Repository Pattern
* DTO Usage
* Validation & Error Handling

---

## 📌 Future Improvements

* 🛒 Shopping Cart System
* ❤️ Wishlist Feature
* ⭐ Product Reviews & Ratings
* 🎟️ Coupons & Discounts
* 📊 Admin Dashboard

---

## 👨‍💻 Author

**Abdallah Fouad**
Back-End Developer (.NET)

---

## ⭐ Support

If you like this project, consider giving it a ⭐ on GitHub!
