using System;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Question
    {
        private static List<Question> questionList = new List<Question>();
        private int _questionID;
        private string _text;
        private List<string> _options = new List<string>();
        private string _correctAnswer;
        private string _questionType;
        private Quiz _quiz;
        private string _qualifier; // qualified zero-to-many relation with Quiz, Question is a qualifier for Quiz 

        public int QuestionID
        {
            get => _questionID;
            set
            {
                if (value <= 0) throw new ArgumentException("Question ID must be positive.");
                _questionID = value;
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Question text cannot be empty.");
                _text = value;
            }
        }

        public List<string> Options
        {
            get => _options;
            set
            {
                if (value == null || value.Count != 4) throw new ArgumentException("There must be exactly 4 options.");
                _options = value;
            }
        }

        public string CorrectAnswer
        {
            get => _correctAnswer;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Correct answer cannot be empty.");
                if (!_options.Contains(value)) throw new ArgumentException("Correct answer must be one of the options.");
                _correctAnswer = value;
            }
        }

        public string QuestionType
        {
            get => _questionType;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Question type cannot be empty.");
                _questionType = value;
            }
        }

        public Quiz Quiz => _quiz; // Reverse reference to quiz
        public string Qualifier => _qualifier;

        public Question() { }

        public Question(int questionID, string text, List<string> options, string correctAnswer, string questionType)
        {
            QuestionID = questionID;
            Text = text;
            Options = options;
            CorrectAnswer = correctAnswer;
            QuestionType = questionType;
            questionList.Add(this);
        }

        public void AssignToQuiz(Quiz quiz, string qualifier)
        {
            if (quiz == null) throw new ArgumentException("Quiz cannot be null.");

            // Ensures the qualifier string is not null or empty
            if (string.IsNullOrWhiteSpace(qualifier)) throw new ArgumentException("Qualifier cannot be empty.");

            // Ensures the question is not already linked to a different quiz
            if (_quiz != null && _quiz != quiz)
                throw new ArgumentException("Question is already associated with another quiz.");

            _quiz = quiz;
            _qualifier = qualifier;
            
            if (!quiz.Questions.ContainsKey(qualifier))
            {
                quiz.AddQuestion(qualifier, this);
                // Ensures the reverse connection by adding this question to the quiz's qualified list.
            }
        }

        public void RemoveQuiz()
        {
            // Ensures the question is currently linked to a quiz
            if (_quiz == null) throw new ArgumentException("Question is not associated with any quiz.");

            var tempQuiz = _quiz;
            var tempQualifier = _qualifier;
            _quiz = null;
            _qualifier = null;
            // Clears the question's reference to the quiz and its qualifier.

            if (tempQuiz.Questions.ContainsKey(tempQualifier))
            {
                tempQuiz.RemoveQuestion(tempQualifier);
                // Ensures the reverse disconnection by removing the question from the quiz's list.
            }
        }

        public static void SaveQuestions(string path = "question.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Question>));
                    serializer.Serialize(writer, questionList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving questions: {ex.Message}");
            }
        }

        public static bool LoadQuestions(string path = "question.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Question>));
                    questionList = (List<Question>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                questionList = new List<Question>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading questions: {ex.Message}");
                questionList = new List<Question>();
                return false;
            }
        }

        public static List<Question> QuestionList => new List<Question>(questionList);
    }
}
