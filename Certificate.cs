using System;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Certificate
    {
        private static List<Certificate> certificatesList = new List<Certificate>();
        private int _certificateID;
        private DateTime _completionDate;
        private string _certificateDescription;

        public int CertificateID
        {
            get => _certificateID;
            private set
            {
                if (value <= 0) throw new ArgumentException("Certificate ID must be positive.");
                _certificateID = value;
            }
        }

        public DateTime CompletionDate
        {
            get => _completionDate;
            private set
            {
                if (value > DateTime.Now) throw new ArgumentException("Completion date cannot be in the future.");
                _completionDate = value;
            }
        }

        public string CertificateDescription
        {
            get => _certificateDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Certificate description cannot be empty.");
                _certificateDescription = value;
            }
        }

        public Certificate(int certificateID, DateTime completionDate, string certificateDescription)
        {
            CertificateID = certificateID;
            CompletionDate = completionDate;
            CertificateDescription = certificateDescription;
            certificatesList.Add(this);
        }

        public static void SaveCertificates(string path = "certificates.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Certificate>));
                    serializer.Serialize(writer, certificatesList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving certificates: {ex.Message}");
            }
        }

        public static bool LoadCertificates(string path = "certificates.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Certificate>));
                    certificatesList = (List<Certificate>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                certificatesList.Clear();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading certificates: {ex.Message}");
                certificatesList.Clear();
                return false;
            }
        }

        // Public static property to expose the certificatesList for testing purposes
        public static List<Certificate> CertificatesList => certificatesList;
    }
}
