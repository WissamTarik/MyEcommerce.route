# 🛒 MyEcommerce API

A full-featured **E-Commerce Backend API** built with **ASP.NET Core** using **Onion Architecture**, designed to handle real-world e-commerce workflows with scalability and clean code principles.

---

## 🚀 Features

-  Authentication & Authorization (JWT)
- 👤 User & Role Management (Admin / User)
-  Products, Brands & Categories
-  Wishlist & 🛒 Shopping Cart
- Orders & Checkout Flow
-  Stripe Payment Integration
- Real-time Notifications using SignalR
- Redis Caching
- Email Service (SMTP)
- Swagger API Documentation
-  Onion Architecture (Clean Architecture)

---
##  Architecture:
The project follows **Onion Architecture** principles:

- 📄 Swagger API Documentation
- 🧱 Onion Architecture (Clean Architecture)

---

## 🏗️ Architecture

The project follows **Onion Architecture** principles:

Domain
Application
Persistence
Infrastructure
Presentation (API)

## 🛠️ Tech Stack

- **ASP.NET Core 8**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **SignalR**
- **Redis**
- **Stripe API**
- **Swagger**
- **AutoMapper**

---

## 🔔 Real-time Notifications

- Notifications are sent when:
  - An order is successfully created
- Implemented using:
  - SignalR Hub
  - Database persistence
  - Secure JWT-based connections

## ⚙️ Configuration

Sensitive data is stored in:

appsettings.Development.json

> ❗ This file is ignored for security reasons.

Example:
```json
{
  "JwtOptions": {
    "SecretKey": "DEV_SECRET_KEY_ONLY"
  }
}



## 🏗️ Architecture

The project follows **Onion Architecture** principles:
