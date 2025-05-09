# Freelancing Platform (Angular & ASP.NET Core)

A full-stack freelancing platform connecting clients with freelancers, supporting bidding/fixed-price projects,With real time bidding, real-time chat, payments, and advanced role management.

## Key Features

### User Roles
- **Freelancers**: Create profiles, bid on projects, withdraw earnings, Apply for proposals, manage profile
- **Clients**: Post projects (fixed-price/bidding), Accept proposals, Check freelancers profile and contact them
- **Admins**: Moderate content, manage categories/skills, handle disputes  

### Core Functionalities
- **Project Marketplace**  
  - Bidding system with proposals  
  - Fixed-price projects with milestones  
  - Category/subcategory organization  

- **Payment System**  
  - Stripe integration for deposits/withdrawals  
  - Escrow payments for milestones  
  - Subscription plans for freelancers  

- **Real-Time Features**  
  - SignalR chat & notifications  
  - Live project updates for biddings 
  - Sentiment analysis on reviews using AI  

- **Profile Management**  
  - Portfolio projects with images  
  - Skill endorsements  
  - Education/certification tracking  

## Tech Stack

### Frontend
- **Angular 19** with TypeScript  
- **PrimeNG** Component Library  
- **RxJS** for state management  
- **SignalR Client** for real-time features  

### Backend
- **ASP.NET Core 8** WebAPI  
- **Entity Framework Core** with SQL Server  
- **SignalR** for real-time communication  
- **Stripe.NET** for payment processing  
- **JWT Authentication** with Role-based authorization  

### Infrastructure
- **Middleware**: Request logging, exception handling,Custom middleware for banning users 
- **Filters**: Action/Result filters for validation, Custom filters for allowing Authors only
- **Background Services**: Payment reconciliation  

## Project Structure

### API Controllers
| Category          | Key Controllers                          |
|-------------------|-----------------------------------------|
| Projects          | `BiddingProjectController`, `FixedPriceProjectController`, `ProposalController` |
| Payments          | `StripeController`, `DepositPaymentsController`, `FreelancerWithdrawalController` |
| User Management   | `CertificatesController`, `EducationsController`, `UserSkillsController` |
| Real-Time         | `ChatController`, `NotificationsController` |
| Admin             | `BansController`, `CategoryController`, `SubcategoryController` |

### Key Models
```mermaid
classDiagram
    class User {
        +Id
        +UserType (Admin/Client/Freelancer)
        +Skills[]
        +PortfolioProjects[]
    }
    class Project {
        +Type (Bidding/FixedPrice)
        +Milestones[]
        +Proposals[]
    }
    class Payment {
        +StripeTransactionId
        +EscrowReleaseDate
    }
    User "1" --> "*" Project
    Project "1" --> "*" Payment
