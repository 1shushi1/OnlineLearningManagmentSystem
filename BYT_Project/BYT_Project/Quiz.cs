using System;
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
        private Dictionary<string, Question> _questions = new Dictionary<string, Question>(); // Qualified association
        private List<Quiz> _relatedQuizzes = new List<Quiz>(); // Reflex association - relates to other quizzes
        private Lesson? _lesson;

        public int QuizID
        {
            get => _quizID;
            set
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

        public Lesson? Lesson => _lesson;

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
                if (value < 0 || value > _totalScore) throw new ArgumentException("Pass mark must be within the range of the total score.");
                _passMark = value;
            }
        }

        public IReadOnlyDictionary<string, Question> Questions => _questions;
        public IReadOnlyList<Quiz> RelatedQuizzes => _relatedQuizzes.AsReadOnly();
        public Quiz() { }
        public Quiz(int quizID, string title, int totalScore, int passMark)
        {
            QuizID = quizID;
            Title = title;
            TotalScore = totalScore;
            PassMark = passMark;
            quizzesList.Add(this);
        }

        public void AddQuestion(string qualifier, Question question)
        {
            if (string.IsNullOrWhiteSpace(qualifier)) throw new ArgumentException("Qualifier cannot be empty.");
            if (question == null) throw new ArgumentException("Question cannot be null.");
            if (_questions.ContainsKey(qualifier)) throw new ArgumentException("A question with this qualifier already exists.");
            if (question.Quiz != null && question.Quiz != this)
                throw new ArgumentException("Question is already associated with another quiz.");

            _questions[qualifier] = question;
            question.AssignToQuiz(this, qualifier);
        }

        public void RemoveQuestion(string qualifier)
        {
            if (!_questions.ContainsKey(qualifier)) throw new ArgumentException("No question exists for the given qualifier.");

            var question = _questions[qualifier];
            _questions.Remove(qualifier);
            question.RemoveQuiz(); 
        }

        public void AddRelatedQuiz(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentException("Quiz cannot be null.");
            if (_relatedQuizzes.Contains(quiz)) throw new ArgumentException("This quiz is already related.");
            if (quiz == this) throw new ArgumentException("A quiz cannot be related to itself."); // Prevent self-reference

            _relatedQuizzes.Add(quiz);

            if (!quiz.RelatedQuizzes.Contains(this))
            {
                quiz.AddRelatedQuiz(this); 
            }
        }



        public void RemoveRelatedQuiz(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentException("Quiz cannot be null.");
            if (!_relatedQuizzes.Contains(quiz)) throw new ArgumentException("This quiz is not related.");

            _relatedQuizzes.Remove(quiz);

            if (quiz.RelatedQuizzes.Contains(this))
            {
                quiz.RemoveRelatedQuiz(this); 
            }
        }



        public static void SaveQuizzes(string path = "quiz.xml")
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

        public static bool LoadQuizzes(string path = "quiz.xml")
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
                quizzesList = new List<Quiz>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading quizzes: {ex.Message}");
                quizzesList = new List<Quiz>();
                return false;
            }
        }

        public void AssignToLesson(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentNullException(nameof(lesson));

            if (_lesson == lesson) return; 

            _lesson?.RemoveQuiz(this);

            _lesson = lesson;

            if (!lesson.Quizzes.Contains(this))
            {
                lesson.AddQuiz(this);
            }
        }

        public void RemoveLesson()
        {
            if (_lesson == null) return;

            var tempLesson = _lesson;
            _lesson = null;

            if (tempLesson.Quizzes.Contains(this))
            {
                tempLesson.RemoveQuiz(this);
            }
        }


        public static List<Quiz> QuizzesList => new List<Quiz>(quizzesList);
    }
}
