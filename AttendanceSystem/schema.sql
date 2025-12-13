PRAGMA foreign_keys = ON;

-- =====================
-- Departments
-- =====================
CREATE TABLE Departments (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT
);

-- =====================
-- Instructors
-- =====================
CREATE TABLE Instructors (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT,
    LastName TEXT,
    Gmail TEXT
);

-- =====================
-- Students
-- =====================
CREATE TABLE Students (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT,
    LastName TEXT,
    Gmail TEXT,
    DepartmentID INTEGER,
    Level INTEGER,
    NFC_Tag TEXT,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(ID)
);

-- =====================
-- Courses
-- =====================
CREATE TABLE Courses (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT,
    Code TEXT,
    InstructorID INTEGER,
    Level INTEGER,
    FOREIGN KEY (InstructorID) REFERENCES Instructors(ID)
);

-- =====================
-- Lectures
-- =====================
CREATE TABLE Lectures (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    CourseID INTEGER,
    StartTime TEXT,
    EndTime TEXT,
    Room TEXT,
    FOREIGN KEY (CourseID) REFERENCES Courses(ID)
);

-- =====================
-- Lecture Sessions
-- =====================
CREATE TABLE LectureSessions (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    LectureID INTEGER NOT NULL,
    StartedAt TEXT DEFAULT CURRENT_TIMESTAMP,
    ClosedAt TEXT,
    IsOpen INTEGER NOT NULL DEFAULT 1,
    FOREIGN KEY (LectureID) REFERENCES Lectures(ID)
);

-- =====================
-- Enrolments
-- =====================
CREATE TABLE Enrolments (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentID INTEGER NOT NULL,
    CourseID INTEGER NOT NULL,
    FOREIGN KEY (StudentID) REFERENCES Students(ID),
    FOREIGN KEY (CourseID) REFERENCES Courses(ID)
);

CREATE INDEX IX_Enrolments_StudentID ON Enrolments(StudentID);
CREATE INDEX IX_Enrolments_CourseID ON Enrolments(CourseID);

-- =====================
-- Attendance
-- =====================
CREATE TABLE Attendance (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentID INTEGER,
    LectureID INTEGER,
    ScanTime TEXT,
    Status TEXT DEFAULT 'Present',
    FOREIGN KEY (StudentID) REFERENCES Students(ID),
    FOREIGN KEY (LectureID) REFERENCES Lectures(ID),
    UNIQUE (StudentID, LectureID)
);

CREATE INDEX IX_Attendance_StudentID ON Attendance(StudentID);

-- =====================
-- Blocked Students
-- =====================
CREATE TABLE BlockedStudents (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentID INTEGER NOT NULL,
    LectureID INTEGER,
    BlockedAt TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (StudentID) REFERENCES Students(ID),
    FOREIGN KEY (LectureID) REFERENCES Lectures(ID)
);

-- =====================
-- Indexes
-- =====================
CREATE INDEX IX_LectureSessions_LectureID_IsOpen
ON LectureSessions(LectureID, IsOpen);
