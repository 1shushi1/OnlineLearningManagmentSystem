using System;
using System.Collections.Generic;
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
        private List<Enrollment> _enrollments = new List<Enrollment>(); // Association with Enrollment

        public int CertificateID
        {
            get => _certificateID;
            set
            {
                if (value <= 0) throw new ArgumentException("Certificate ID must be positive.");
                _certificateID = value;
            }
        }

        public DateTime CompletionDate
        {
            get => _completionDate;
            set
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

        public IReadOnlyList<Enrollment> Enrollments => _enrollments.AsReadOnly(); // Encapsulated list

        public Certificate() { }

        public Certificate(int certificateID, DateTime completionDate, string certificateDescription)
        {
            CertificateID = certificateID;
            CompletionDate = completionDate;
            CertificateDescription = certificateDescription;
            certificatesList.Add(this);
        }

        public void AddEnrollment(Enrollment enrollment)
        {
            if (enrollment == null) throw new ArgumentNullException(nameof(enrollment));
            if (_enrollments.Contains(enrollment))
                throw new ArgumentException("Enrollment is already associated with this certificate.");
            if (enrollment.Certificate != null && enrollment.Certificate != this)
                throw new ArgumentException("Enrollment is already associated with another certificate.");

            _enrollments.Add(enrollment);
            enrollment.SetCertificate(this);
        }




        public void RemoveEnrollment(Enrollment enrollment)
        {
            if (enrollment == null || !_enrollments.Contains(enrollment))
                throw new ArgumentException("Enrollment is not associated with this certificate.");

            _enrollments.Remove(enrollment);

            // Reverse connection check
            if (enrollment.Certificate == this)
            {
                enrollment.RemoveCertificate();
            }
        }









        public static void SaveCertificates(string path = "certificate.xml")
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

        public static bool LoadCertificates(string path = "certificate.xml")
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
                certificatesList = new List<Certificate>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading certificates: {ex.Message}");
                certificatesList = new List<Certificate>();
                return false;
            }
        }

        public static List<Certificate> CertificatesList => new List<Certificate>(certificatesList);
    }
}