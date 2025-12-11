using AttendanceSystem.Data;
using AttendanceSystem.Entites;

namespace AttendanceSystem.Services
{
    public class LectureService
    {
        // ------------------------- Helpers -------------------------

        private static lecture? GetLecture(int lectureId, AppDbContext context)
        {
            return context.Lectures.FirstOrDefault(l => l.ID == lectureId);
        }

        private static bool CourseExists(int courseId, AppDbContext context)
        {
            return context.Courses.Any(c => c.ID == courseId);
        }

        private static bool IsTimeValid(DateTime start, DateTime end)
        {
            return end > start;
        }

        private static bool IsRoomAvailable(string room, DateTime start, DateTime end, AppDbContext context)
        {
            return !context.Lectures.Any(l =>
                l.Room == room &&
                (
                    (start >= l.StartTime && start < l.EndTime) ||
                    (end > l.StartTime && end <= l.EndTime) ||
                    (start <= l.StartTime && end >= l.EndTime)
                ));
        }

        private static bool HasAttendance(int lectureId, AppDbContext context)
        {
            return context.Attendance.Any(a => a.LectureID == lectureId);
        }

        // ------------------------- Add Lecture -------------------------

        public static string AddLecture(int courseId, DateTime startTime, DateTime endTime, string room)
        {
            using var context = new AppDbContext();

            if (!CourseExists(courseId, context))
                return "Course does not exist.";

            if (!IsTimeValid(startTime, endTime))
                return "End time must be greater than start time.";

            if (!IsRoomAvailable(room, startTime, endTime, context))
                return "Room already booked in this time range.";

            var lecture = new lecture
            {
                CourseID = courseId,
                StartTime = startTime,
                EndTime = endTime,
                Room = room
            };

            context.Lectures.Add(lecture);
            context.SaveChanges();

            return "Lecture added successfully.";
        }

        // ------------------------- Delete Lecture -------------------------

        public static string DeleteLecture(int lectureId)
        {
            using var context = new AppDbContext();

            var lecture = GetLecture(lectureId, context);

            if (lecture == null)
                return "Lecture not found.";

            if (HasAttendance(lectureId, context))
                return "Cannot delete lecture because it has attendance records.";

            context.Lectures.Remove(lecture);
            context.SaveChanges();

            return "Lecture deleted successfully.";
        }

        // ------------------------- Update Lecture -------------------------

        public static string UpdateLecture(int lectureId, int courseId, DateTime startTime, DateTime endTime, string room)
        {
            using var context = new AppDbContext();

            var lecture = GetLecture(lectureId, context);

            if (lecture == null)
                return "Lecture not found.";

            if (!CourseExists(courseId, context))
                return "Course does not exist.";

            if (!IsTimeValid(startTime, endTime))
                return "End time must be greater than start time.";

            // لو هنغير الوقت أو القاعة → نعمل تشيك
            if ((lecture.StartTime != startTime || lecture.EndTime != endTime || lecture.Room != room) &&
                !IsRoomAvailable(room, startTime, endTime, context))
            {
                return "Room already booked in this time range.";
            }

            lecture.CourseID = courseId;
            lecture.StartTime = startTime;
            lecture.EndTime = endTime;
            lecture.Room = room;

            context.SaveChanges();

            return "Lecture updated successfully.";
        }

        // ------------------------- Getters -------------------------

        public static List<lecture> GetAllLectures()
        {
            using var context = new AppDbContext();
            return context.Lectures.ToList();
        }

        public static lecture? GetLectureById(int lectureId)
        {
            using var context = new AppDbContext();
            return context.Lectures.FirstOrDefault(l => l.ID == lectureId);
        }
    }
}
