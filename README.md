# Pico y Placa Predictor 
### Made by: Daniel Jaramillo

A C# (.NET 8.0) console application that predicts whether a vehicle can circulate in Quito, Ecuador based on the previous "Pico y Placa" traffic restriction system.

## Overview

The Pico y Placa system is a traffic management measure implemented in Quito to reduce congestion during peak hours. Vehicles are restricted from circulating based on the last digit of their license plate, the day of the week, and the time of day.

This application takes three inputs:
- **License plate** (e.g., `PBX-1234` for cars, `ABC-123A` or `AB-123A` for motorcycles)
- **Date** (format: `yyyy-MM-dd`)
- **Time** (format: `HH:mm`)

And returns whether the vehicle **CAN** or **CANNOT** circulate, along with an explanation.

---

## [How Pico y Placa Works, 2024](https://antsimulador.com/pico-y-placa-quito/)

### Restriction Schedule

**Days**: Monday through Friday (weekends are restriction-free)

**Hours**:
- **Morning**: 07:00 - 09:30
- **Evening**: 16:00 - 19:30

### License Plate Restrictions by Day

| Day       | Restricted Last Digits |
|-----------|------------------------|
| Monday    | 1, 2                   |
| Tuesday   | 3, 4                   |
| Wednesday | 5, 6                   |
| Thursday  | 7, 8                   |
| Friday    | 9, 0                   |

### Vehicle Types Supported

**Cars**: `ABC-1234` (3 letters and 3 or 4 digits)
- The last **digit** determines the restriction

**Motorcycles**: `ABC-123A` (2 or 3 letters - 3 digits - 1 letter)
- The last **digit before the letter** determines the restriction
- Example: `ABC-123A` → uses digit `3` (not the letter `A`)

---

## Features

 **Vehicle Type Detection**: Automatically detects cars vs motorcycles  
 **Input Validation**: Comprehensive validation for license plates, dates, and times  
 **Weekend Detection**: No restrictions on Saturdays and Sundays  
 **Time Boundary Accuracy**: Precise handling of restriction start/end times  
 **Clean Architecture**: Separation of concerns 
 **Well-Tested**: Comprehensive unit tests with edge case coverage  

---

## Project Structure
```
PicoYPlaca/
├── Models/
│   └── LicensePlate.cs              # License plate validation & parsing
├── Services/
│   ├── RestrictionsConfig.cs        # Configuration (data: hours, digits)
│   └── Restrictions.cs              # Business logic (checker)
├── Program.cs                        # Console application entry point
└── PicoPlaca.csproj                 # Project file

PicoPlaca.Tests/
├── LicensePlateTests.cs             # Tests for plate validation
└── RestrictionsTests.cs             # Tests for restriction logic
```

### Key Components

#### 1. **LicensePlate** (Model)
- Validates license plate format
- Extracts the last digit
- Detects vehicle type (car vs motorcycle)

#### 2. **RestrictionsConfig** (Configuration)
- Stores restriction rules (what days, what digits, what hours)

#### 3. **Restrictions** (Business Logic)
- Uses configuration to check if vehicle can circulate
- Implements decision logic 
- Returns explanations

#### 4. **Program** (Console Interface)
- Handles user input
- Displays results
- Manages the application flow

---

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher

**Check if you have .NET installed:**
```bash
dotnet --version
```

### Installation

1. **Clone or download the project**

2. **Run the application**
```bash
   dotnet run
```
---

## Architecture

### Design Principles

This solution follows **SOLID principles** and **Clean Architecture**:

#### 1. **Separation of Concerns**
```
┌─────────────────────────────────────┐
│  Program.cs (Presentation)          │
│  - User input/output                │
│  - Display logic                    │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│  Restrictions (Business Logic)      │
│  - Decision making                  │
│  - Applies rules                    │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│  RestrictionsConfig (Data)          │
│  - Rules storage                    │
│  - Hours, digits, days              │
└─────────────────────────────────────┘
```

#### 2. **Single Responsibility Principle**
- **LicensePlate**: Only handles plate validation and parsing
- **RestrictionsConfig**: Only stores configuration data
- **Restrictions**: Only applies business logic
- **Program**: Only handles user interaction

#### 3. **Dependency Injection**
```csharp
// Restrictions depends on RestrictionsConfig
public Restrictions(RestrictionsConfig config)
{
    _config = config ?? throw new ArgumentNullException(nameof(config));
}
```

This allows:
- Easy testing with custom configurations
- Flexibility to change rules without changing logic
- Support for multiple cities with different rules

### Data Flow
```
User Input (Plate, Date, Time)
        ↓
LicensePlate validates and extracts digit
        ↓
Restrictions.CheckRestriction()
        ↓
    Is weekend? → YES → Return "CAN circulate"
        ↓ NO
    Is restricted time? → NO → Return "CAN circulate"
        ↓ YES
    Is restricted digit? → NO → Return "CAN circulate"
        ↓ YES
Return "CANNOT circulate"
```

---

## Testing

### Running Tests
```bash
# Run all tests
dotnet test

```

### Test Coverage

The project includes comprehensive tests made in xUnit for:

#### LicensePlate Tests
-  Valid car plate formats (`ABC-1234`, `ABC-123`)
-  Valid motorcycle plate formats (`ABC-123A`)
-  Invalid formats (wrong length, missing dash, etc.)
-  Last digit extraction for all digits (0-9)
-  Case insensitivity and whitespace handling
-  Vehicle type detection

#### Restrictions Tests
-  Weekend scenarios (should always allow)
-  Restricted hours + restricted plate (should block)
-  Restricted hours + non-restricted plate (should allow)
-  Non-restricted hours (should always allow)
-  Edge times (7:00, 9:30, 16:00, 19:30)
-  Just outside edge times (6:59, 9:31, 15:59, 19:31)
-  All digits (0-9) on their respective restricted days
-  Morning vs evening restrictions
-  Custom configuration testing

## Acknowledgment
Rules and restrictions are based on [Pico y Placa from 2024](https://antsimulador.com/pico-y-placa-quito/) 