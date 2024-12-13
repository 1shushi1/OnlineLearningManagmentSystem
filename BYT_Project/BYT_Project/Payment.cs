using System;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Payment
    {
        private static List<Payment> paymentsList = new List<Payment>();
        private int _paymentID;
        private double _amount;
        private DateTime _paymentDate;
        private Student _student;

        public int PaymentID
        {
            get => _paymentID;
            set
            {
                if (value <= 0) throw new ArgumentException("Payment ID must be positive.");
                _paymentID = value;
            }
        }

        public double Amount
        {
            get => _amount;
            set
            {
                if (value <= 0) throw new ArgumentException("Amount must be positive.");
                _amount = value;
            }
        }

        public DateTime PaymentDate
        {
            get => _paymentDate;
            set
            {
                if (value > DateTime.Now) throw new ArgumentException("Payment date cannot be in the future.");
                _paymentDate = value;
            }
        }

        public Student Student => _student;

        public Payment() { }
        public Payment(int paymentID, double amount, DateTime paymentDate)
        {
            PaymentID = paymentID;
            Amount = amount;
            PaymentDate = paymentDate;
            paymentsList.Add(this);
        }

        public void AssignToStudent(Student student)
        {
            if (student == null) throw new ArgumentException("Student cannot be null.");
            if (_student == student) throw new ArgumentException("Payment is already assigned to this student.");

            if (_student != null && _student != student)
                throw new ArgumentException("Payment is already assigned to a student.");

            _student = student;

            if (!student.Payments.Contains(this))
            {
                student.AddPayment(this);
            }
        }


        public void RemoveStudent()
        {
            if (_student == null) throw new ArgumentException("Payment is not assigned to any student.");

            var tempStudent = _student;
            _student = null;

            if (tempStudent.Payments.Contains(this))
            {
                tempStudent.RemovePayment(this); 
            }
        }

        public static void SavePayments(string path = "payment.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Payment>));
                    serializer.Serialize(writer, paymentsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving payments: {ex.Message}");
            }
        }

        public static bool LoadPayments(string path = "payment.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Payment>));
                    paymentsList = (List<Payment>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                paymentsList = new List<Payment>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading payments: {ex.Message}");
                paymentsList = new List<Payment>();
                return false;
            }
        }
        public static List<Payment> PaymentsList => new List<Payment>(paymentsList);
    }
}
