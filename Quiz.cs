using System;
using System.IO;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Quiz
    {
        private static List<Quiz> quizzesList = new List<Quiz>();
        private int _quizID;
        private string _title;
        private int _totalScore;
        private int _passMark;

        public int QuizID
        {
            get => _quizID;
            private set
            {
                if (value <= 0) throw new ArgumentException("Quiz ID must be positive.");
                _quizID = value;
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Title cannot be empty.");
                _title = value;
            }
        }

        public int TotalScore
        {
            get => _totalScore;
            set
            {
                if (value <= 0) throw new ArgumentException("Total score must be positive.");
                _totalScore = value;
            }
        }

        public int PassMark
        {
            get => _passMark;
            set
            {
                if (value < 0 || value > _totalScore) throw new ArgumentException("Pass mark must be within the range of total score.");
                _passMark = value;
            }
        }

        public Quiz(int quizID, string title, int totalScore, int passMark)
        {
            QuizID = quizID;
            Title = title;
            TotalScore = totalScore;
            PassMark = passMark;
            quizzesList.Add(this);
        }

        public static void SaveQuizzes(string path = "quizzes.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Quiz>));
                    serializer.Serialize(writer, quizzesList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving quizzes: {ex.Message}");
            }
        }

        public static bool LoadQuizzes(string path = "quizzes.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Quiz>));
                    quizzesList = (List<Quiz>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                quizzesList.Clear();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading quizzes: {ex.Message}");
                quizzesList.Clear();
                return false;
            }
        }

        // Public static property to expose the quizzesList for testing purposes
        public static List<Quiz> QuizzesList => quizzesList;
    }
}
