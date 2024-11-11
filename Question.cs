using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Question
    {
        private static List<Question> questionsList = new List<Question>();
        private int _questionID;
        private string _text;
        private List<string> _options;
        private string _correctAnswer;
        private string _questionType;

        public int QuestionID
        {
            get => _questionID;
            private set
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
                if (value == null || value.Count < 1 || value.Count > 4) throw new ArgumentException("There should be 1 to 4 options.");
                _options = value;
            }
        }

        public string CorrectAnswer
        {
            get => _correctAnswer;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Correct answer cannot be empty.");
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

        public Question(int questionID, string text, List<string> options, string correctAnswer, string questionType)
        {
            QuestionID = questionID;
            Text = text;
            Options = options;
            CorrectAnswer = correctAnswer;
            QuestionType = questionType;
            questionsList.Add(this);
        }

        public static void SaveQuestions(string path = "questions.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Question>));
                    serializer.Serialize(writer, questionsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving questions: {ex.Message}");
            }
        }

        public static bool LoadQuestions(string path = "questions.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Question>));
                    questionsList = (List<Question>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                questionsList.Clear();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading questions: {ex.Message}");
                questionsList.Clear();
                return false;
            }
        }

        // Public static property to expose the questionsList for testing purposes
        public static List<Question> QuestionsList => questionsList;
    }
}
