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

        public int PaymentID
        {
            get => _paymentID;
            private set
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
            private set
            {
                if (value > DateTime.Now) throw new ArgumentException("Payment date cannot be in the future.");
                _paymentDate = value;
            }
        }

        public Payment(int paymentID, double amount, DateTime paymentDate)
        {
            PaymentID = paymentID;
            Amount = amount;
            PaymentDate = paymentDate;
            paymentsList.Add(this);
        }

        public static void SavePayments(string path = "payments.xml")
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

        public static bool LoadPayments(string path = "payments.xml")
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
                paymentsList.Clear();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading payments: {ex.Message}");
                paymentsList.Clear();
                return false;
            }
        }

        // Public static property to expose the paymentsList for testing purposes
        public static List<Payment> PaymentsList => paymentsList;
    }
}
